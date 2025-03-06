using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Persistence;

public class Seed
{
    public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
    {
        if (!userManager.Users.Any())
        {
            var users = new List<(AppUser User, string Password)>
            {
                (new AppUser
                {
                    DisplayName = "Tientien",
                    UserName = "tientien",
                    Email = "tiens2taeyeon@gmail.com"
                }, "Tientien1103"),
                (new AppUser
                {
                    DisplayName = "khaitri074",
                    UserName = "khaitri074",
                    Email = "khaitri074@gmail.com"
                }, "Khaitri074")
            };

            foreach (var (user, password) in users)
            {
                await userManager.CreateAsync(user, password);
            }

            await context.SaveChangesAsync();
        }

        if (!context.Servers.Any())
        {
            var owner = userManager.Users.FirstOrDefault(x => x.UserName == "khaitri074") ?? throw new Exception("Owner user not found");
            var owner2 = userManager.Users.FirstOrDefault(x => x.UserName == "tientien") ?? throw new Exception("Owner user not found");
            var everyoneRole = new Role
            {
                RoleName = "@everyone",
                Permissions = (long)(RolePermission.ViewChannel | RolePermission.ReadMessageHistory),
                Color = "#000000",
                Position = 0,
                IsDefault = true,
            };

            var server = new Server
            {
                ServerName = "Test Server",
                Avatar = "https://i.pinimg.com/736x/24/6c/7b/246c7bc8b021aaa07e43e2ab52c158db.jpg",
                OwnerId = owner.Id,
                Roles = [everyoneRole],
                ServerMembers =
                [
                    new()
                    {
                        MemberId = owner.Id,
                        IsOwner = true,
                        ServerMemberRoles = [new ServerMemberRole { RoleId = everyoneRole.RoleId }]
                    }
                ]
            };

            await context.Servers.AddAsync(server);

            // Add another server
            var anotherServer = new Server
            {
                ServerName = "Another Test Server",
                Avatar = "https://example.com/avatar.jpg",
                OwnerId = owner2.Id,
                Roles = [everyoneRole],
                ServerMembers =
                [
                    new()
                    {
                        MemberId = owner2.Id,
                        IsOwner = true,
                        ServerMemberRoles = [new ServerMemberRole { RoleId = everyoneRole.RoleId }]
                    }
                ]
            };

            await context.Servers.AddAsync(anotherServer);
            await context.SaveChangesAsync();
        }

        if (!context.Channels.Any())
        {
            var server = context.Servers.FirstOrDefault(x => x.ServerName == "Test Server") ?? throw new Exception("Server not found");
            var categories = new List<Channel>
            {
                new()
                {
                    ChannelName = "sample category",
                    ServerId = server.ServerId,
                    ChannelType = ChannelType.Category,
                },
                new()
                {
                    ChannelName = "sample category 2",
                    ServerId = server.ServerId,
                    ChannelType = ChannelType.Category,
                },
            };

            await context.Channels.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            var channels = new List<Channel>
            {
                new()
                {
                    ChannelName = "general",
                    ServerId = server.ServerId,
                    ChannelType = ChannelType.Text,
                    ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "sample category" && x.ChannelType == ChannelType.Category)?.ChannelId
                },
                new()
                {
                    ChannelName = "general 2",
                    ServerId = server.ServerId,
                    ChannelType = ChannelType.Text,
                    ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "sample category")?.ChannelId
                },
                new()
                {
                    ChannelName = "voice 1",
                    ServerId = server.ServerId,
                    ChannelType = ChannelType.Voice,
                    ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "sample category 2")?.ChannelId
                },
                new()
                {
                    ChannelName = "voice 2",
                    ServerId = server.ServerId,
                    ChannelType = ChannelType.Voice,
                    ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "sample category 2")?.ChannelId
                },
            };

            await context.Channels.AddRangeAsync(channels);
            await context.SaveChangesAsync();

            // Add channels for another server
            var anotherServer = context.Servers.FirstOrDefault(x => x.ServerName == "Another Test Server") ?? throw new Exception("Another server not found");
            var anotherCategories = new List<Channel>
            {
                new()
                {
                    ChannelName = "another sample category",
                    ServerId = anotherServer.ServerId,
                    ChannelType = ChannelType.Category,
                },
                new()
                {
                    ChannelName = "another sample category 2",
                    ServerId = anotherServer.ServerId,
                    ChannelType = ChannelType.Category,
                },
            };

            await context.Channels.AddRangeAsync(anotherCategories);
            await context.SaveChangesAsync();

            var anotherChannels = new List<Channel>
            {
                new()
                {
                    ChannelName = "another general",
                    ServerId = anotherServer.ServerId,
                    ChannelType = ChannelType.Text,
                    ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "another sample category" && x.ChannelType == ChannelType.Category)?.ChannelId
                },
                new()
                {
                    ChannelName = "another general 2",
                    ServerId = anotherServer.ServerId,
                    ChannelType = ChannelType.Text,
                    ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "another sample category")?.ChannelId
                },
                new()
                {
                    ChannelName = "another voice 1",
                    ServerId = anotherServer.ServerId,
                    ChannelType = ChannelType.Voice,
                    ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "another sample category 2")?.ChannelId
                },
                new()
                {
                    ChannelName = "another voice 2",
                    ServerId = anotherServer.ServerId,
                    ChannelType = ChannelType.Voice,
                    ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "another sample category 2")?.ChannelId
                },
            };

            await context.Channels.AddRangeAsync(anotherChannels);
            await context.SaveChangesAsync();
        }
    }
}