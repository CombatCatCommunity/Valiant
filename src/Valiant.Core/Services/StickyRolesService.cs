using Discord;
using Discord.WebSocket;
using LiteDB;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;
using Valiant.Models;
using ZLogger;

namespace Valiant.Services;

public class StickyRolesService(DiscordSocketClient discord, ILogger<StickyRolesService> logger) : IHostedService
{
    private readonly DiscordSocketClient _discord = discord;
    private readonly ILogger _logger = logger;

    private readonly LiteDatabase _db = new("Filename=./data/stickyroles.db;Connection=shared");

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _discord.GuildMembersDownloaded += OnGuildMembersDownloadedAsync;
        _discord.GuildAvailable += OnGuildAvailableAsync;
        _discord.GuildMemberUpdated += OnMemberUpdatedAsync;
        _discord.RoleDeleted += OnRoleDeletedAsync;
        _discord.UserJoined += OnUserJoinedAsync;
        _discord.UserBanned += OnUserBannedAsync;

        _logger.ZLogInformation($"Started");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _discord.GuildMembersDownloaded -= OnGuildMembersDownloadedAsync;
        _discord.GuildAvailable -= OnGuildAvailableAsync;
        _discord.GuildMemberUpdated -= OnMemberUpdatedAsync;
        _discord.RoleDeleted -= OnRoleDeletedAsync;
        _discord.UserJoined -= OnUserJoinedAsync;
        _discord.UserBanned -= OnUserBannedAsync;

        _logger.ZLogInformation($"Stopped");
        return Task.CompletedTask;
    }

    private Task OnGuildMembersDownloadedAsync(SocketGuild guild)
        => CheckMissedUpdatesAsync(guild);
    private Task OnGuildAvailableAsync(SocketGuild guild)
        => CheckMissedUpdatesAsync(guild);
    private Task CheckMissedUpdatesAsync(SocketGuild guild)
    {
        if (!guild.HasAllMembers)
        {
            _logger.ZLogDebug($"Members not downloaded, skipping check for {guild} ({guild.Id})");
            return Task.CompletedTask;
        }

        var config = _db.GetCollection<StickyRoleConfig>().FindOne(x => x.GuildId == guild.Id);
        if (config == null || config.RoleIds.Count == 0)
            return Task.CompletedTask;

        var usercol = _db.GetCollection<StickyRoleUser>();
        var updateUsers = new List<StickyRoleUser>();
        var createUsers = new List<StickyRoleUser>();
        foreach (var user in guild.Users)
        {
            var sticky = user.Roles.Select(x => x.Id).Intersect(config.RoleIds);
            if (!sticky.Any())
                continue;

            var stickyuser = usercol.FindOne(x => x.GuildId == user.Guild.Id && x.UserId == user.Id);
            if (stickyuser != null)
            {
                stickyuser.RoleIds = sticky.ToList();
                updateUsers.Add(stickyuser);
            } else
            {
                createUsers.Add(new StickyRoleUser
                {
                    GuildId = user.Guild.Id,
                    UserId = user.Id,
                    RoleIds = sticky.ToList()
                });
            }
        }

        usercol.Update(updateUsers);
        usercol.InsertBulk(createUsers);
        _logger.ZLogDebug($"Updated roles for {updateUsers.Count + createUsers.Count} missed user(s) in {guild} ({guild.Id})");
        return Task.CompletedTask;
    }

    private Task OnMemberUpdatedAsync(Cacheable<SocketGuildUser, ulong> cacheable, SocketGuildUser user)
    {
        var config = _db.GetCollection<StickyRoleConfig>().FindOne(x => x.GuildId == user.Guild.Id);
        if (config == null || config.RoleIds.Count == 0)
            return Task.CompletedTask;

        var sticky = user.Roles.Select(x => x.Id).Intersect(config.RoleIds);
        if (!sticky.Any())
            return Task.CompletedTask;

        var usercol = _db.GetCollection<StickyRoleUser>();
        var stickyuser = usercol.FindOne(x => x.GuildId == user.Guild.Id && x.UserId == user.Id);
        if (stickyuser != null)
        {
            stickyuser.RoleIds = sticky.ToList();
        } 
        else
        {
            usercol.Insert(new StickyRoleUser
            {
                GuildId = user.Guild.Id,
                UserId = user.Id,
                RoleIds = sticky.ToList()
            });
        }

        _logger.ZLogDebug($"Updated roles for {user} ({user.Id}) in {user.Guild} ({user.Guild.Id}): {string.Join(", ", sticky)}");
        return Task.CompletedTask;
    }

    private Task OnRoleDeletedAsync(SocketRole role)
    {
        var configcol = _db.GetCollection<StickyRoleConfig>();
        var config = configcol.FindOne(x => x.GuildId == role.Guild.Id && x.RoleIds.Contains(role.Id));
        if (config == null)
            return Task.CompletedTask;

        var usercol = _db.GetCollection<StickyRoleUser>();
        var users = usercol.Find(x => x.GuildId == role.Guild.Id && x.RoleIds.Contains(role.Id));
        if (users.Any())
        {
            foreach (var user in users)
                user.RoleIds.Remove(role.Id);
            usercol.Update(users);
        }

        config.RoleIds.Remove(role.Id);
        configcol.Update(config);
        _logger.ZLogDebug($"Removed deleted role {role} ({role.Id}) from config for {role.Guild} ({role.Guild.Id})");
        return Task.CompletedTask;
    }

    private async Task OnUserJoinedAsync(SocketGuildUser user)
    {
        var col = _db.GetCollection<StickyRoleUser>();
        var match = col.FindOne(x => x.GuildId == user.Guild.Id && x.UserId == user.Id);
        if (match == null)
            return;

        var options = new RequestOptions()
        {
            AuditLogReason = "Reassigning sticky roles"
        };

        await user.AddRolesAsync(match.RoleIds, options);
        _logger.ZLogInformation($"Reassigned roles for user {user} ({user.Id}) in {user.Guild} ({user.Guild.Id})");
    }

    private Task OnUserBannedAsync(SocketUser user, SocketGuild guild)
    {
        var col = _db.GetCollection<StickyRoleUser>();
        var match = col.FindOne(x => x.GuildId == guild.Id && x.UserId == user.Id);
        if (match == null)
            return Task.CompletedTask;

        col.Delete(match.Id);
        _logger.ZLogDebug($"Removed banned user {user} ({user.Id}) for {guild} ({guild.Id})");
        return Task.CompletedTask;
    }
}
