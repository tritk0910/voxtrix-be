using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Persistence;

public class Seed
{
    public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
    {
        if (!userManager.Users.Any())
        {
            var users = new List<AppUser>
            {
                new() {
                    DisplayName = "Tientien",
                    UserName = "tientien",
                    Email = "tiens2taeyeon@gmail.com"
                },
                new ()
                {
                    DisplayName = "khaitri074",
                    UserName = "khaitri074",
                    Email = "khaitri074@gmail.com"
                }
            };

            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }

            await context.SaveChangesAsync();
        }

        if (!context.Servers.Any())
        {
            var servers = new List<Server>
            {
                new()
                {
                    ServerName = "Test Server",
                    Avatar= "https://i.pinimg.com/736x/24/6c/7b/246c7bc8b021aaa07e43e2ab52c158db.jpg",
                    OwnerId = userManager.Users.FirstOrDefault(x => x.UserName == "khaitri074")?.Id,
                    ServerMembers = {
                        new ServerMember
                        {
                            MemberId = userManager.Users.FirstOrDefault(x => x.UserName == "khaitri074")?.Id,
                            IsOwner = true
                        }
                    },
                    ServerRoles = {
                        new ServerRole
                        {
                            ServerId = context.Servers.FirstOrDefault(x => x.ServerName == "Test Server")?.ServerId,
                            UserId = userManager.Users.FirstOrDefault(x => x.UserName == "khaitri074")?.Id,
                            Role = new Role
                            {
                                RoleName = "everyone",
                                Permissions = (long)(RolePermission.ViewChannel | RolePermission.ReadMessageHistory),
                                Color = "#000000",
                                Position = 0,
                                IsDefault = true
                            }
                        }
                    }
                }
            };

            await context.Servers.AddRangeAsync(servers);
            await context.SaveChangesAsync();
        }

        if (!context.Channels.Any())
        {
            var categories = new List<Channel>
            {
                new()
                {
                    ChannelName = "sample category",
                    ServerId = context.Servers.FirstOrDefault(x => x.ServerName == "Test Server")?.ServerId,
                    ChannelType = ChannelType.Category,
                },
                new()
                {
                    ChannelName = "sample category 2",
                    ServerId = context.Servers.FirstOrDefault(x => x.ServerName == "Test Server")?.ServerId,
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
                    ServerId = context.Servers.FirstOrDefault(x => x.ServerName == "Test Server")?.ServerId,
                    ChannelType = ChannelType.Text,
                    ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "sample category" && x.ChannelType == ChannelType.Category)?.ChannelId
                },
                new()
                {
                    ChannelName = "general 2",
                    ServerId = context.Servers.FirstOrDefault(x => x.ServerName == "Test Server")?.ServerId,
                    ChannelType = ChannelType.Text,
                    ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "sample category")?.ChannelId
                },
                new()
                {
                    ChannelName = "voice 1",
                    ServerId = context.Servers.FirstOrDefault(x => x.ServerName == "Test Server")?.ServerId,
                    ChannelType = ChannelType.Voice,
                    ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "sample category 2")?.ChannelId
                },
                new()
                {
                    ChannelName = "voice 2",
                    ServerId = context.Servers.FirstOrDefault(x => x.ServerName == "Test Server")?.ServerId,
                    ChannelType = ChannelType.Voice,
                    ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "sample category 2")?.ChannelId
                },
            };

            await context.Channels.AddRangeAsync(channels);
            await context.SaveChangesAsync();
        }
    }
}