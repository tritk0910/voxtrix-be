using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Channel> Channels { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Reaction> Reactions { get; set; }
    public DbSet<Server> Servers { get; set; }
    public DbSet<ServerBan> ServerBans { get; set; }
    public DbSet<ServerMember> ServerMembers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the one-to-one relationship
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ServerMember>(e =>
            {
                e.HasKey(sm => sm.ServerMemberId);
                // Foreign key relationships
                e.HasOne(sm => sm.Member)
                    .WithMany(u => u.ServerMembers)
                    .HasForeignKey(sm => sm.MemberId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(sm => sm.Server)
                    .WithMany(s => s.ServerMembers)
                    .HasForeignKey(sm => sm.ServerId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(sm => new { sm.ServerId, sm.MemberId })
                    .IsUnique();
            }
        );

        modelBuilder.Entity<Invite>(e =>
            {
                e.HasKey(i => i.InviteId);
                e.HasOne(i => i.Server)
                    .WithMany(s => s.Invites)
                    .HasForeignKey(i => i.ServerId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(i => i.InviteCode)
                    .IsUnique();
            }
        );

        modelBuilder.Entity<Server>(e =>
            {
                e.HasKey(s => s.ServerId);
                e.HasOne(s => s.Owner)
                    .WithMany(u => u.OwnedServers)
                    .HasForeignKey(s => s.OwnerId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(s => s.OwnerId)
                    .IsUnique();
            }
        );

        modelBuilder.Entity<Message>(e =>
            {
                e.HasKey(m => m.MessageId);
                e.HasOne(m => m.User)
                    .WithMany(u => u.Messages)
                    .HasForeignKey(m => m.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(m => m.Channel)
                    .WithMany(c => c.Messages)
                    .HasForeignKey(m => m.ChannelId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(m => new { m.ChannelId, m.AuthorId })
                .IsUnique();
            }
        );

        modelBuilder.Entity<Reaction>(e =>
            {
                e.HasKey(r => r.ReactionId);
                e.HasOne(r => r.User)
                    .WithMany(u => u.Reactions)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(r => r.Message)
                    .WithMany(m => m.Reactions)
                    .HasForeignKey(r => r.MessageId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(r => new { r.MessageId, r.UserId })
                    .IsUnique();
            }
        );

        modelBuilder.Entity<ServerBan>(e =>
            {
                e.HasKey(sb => sb.BanId);
                e.HasOne(sb => sb.User)
                    .WithMany(u => u.ServerBans)
                    .HasForeignKey(sb => sb.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(sb => sb.Server)
                    .WithMany(s => s.Bans)
                    .HasForeignKey(sb => sb.ServerId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(sb => new { sb.ServerId, sb.UserId })
                    .IsUnique();
            }
        );

        modelBuilder.Entity<Channel>(e =>
            {
                e.HasKey(c => c.ChannelId);
                e.HasOne(c => c.Server)
                    .WithMany(s => s.Channels)
                    .HasForeignKey(c => c.ServerId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(c => c.ServerId).IsUnique();
            }
        );

        modelBuilder.Entity<AppUser>(e =>
        {
            e.HasIndex(u => u.UserName).IsUnique();
            e.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<Role>(e =>
        {
            e.HasIndex(s => s.ServerId).IsUnique();
        });

        modelBuilder.Entity<UserRole>(e =>
        {
            e.HasIndex(sm => new { sm.UserId, sm.RoleId }).IsUnique();
        });

        modelBuilder.Entity<VoiceState>(e =>
        {
            e.HasIndex(sm => new { sm.UserId, sm.ChannelId }).IsUnique();
        });
    }
}