﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Persistence;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250124143752_UpdateEntities")]
    partial class UpdateEntities
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("Avatar")
                        .HasColumnType("text");

                    b.Property<string>("Bio")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DisplayName")
                        .HasColumnType("text");

                    b.Property<DateOnly>("DoB")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Channel", b =>
                {
                    b.Property<string>("ChannelId")
                        .HasColumnType("text");

                    b.Property<string>("ChannelName")
                        .HasColumnType("text");

                    b.Property<int>("ChannelType")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("ParentChannelId")
                        .HasColumnType("text");

                    b.Property<string>("ServerId")
                        .HasColumnType("text");

                    b.HasKey("ChannelId");

                    b.HasIndex("ParentChannelId");

                    b.HasIndex("ServerId")
                        .IsUnique();

                    b.ToTable("Channels");
                });

            modelBuilder.Entity("Domain.Entities.Invite", b =>
                {
                    b.Property<string>("InviteId")
                        .HasColumnType("text");

                    b.Property<string>("ChannelId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("InviteCode")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<int>("MaxUses")
                        .HasColumnType("integer");

                    b.Property<string>("ServerId")
                        .HasColumnType("text");

                    b.Property<int>("Uses")
                        .HasColumnType("integer");

                    b.HasKey("InviteId");

                    b.HasIndex("ChannelId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("InviteCode")
                        .IsUnique();

                    b.HasIndex("ServerId");

                    b.ToTable("Invite");
                });

            modelBuilder.Entity("Domain.Entities.Message", b =>
                {
                    b.Property<string>("MessageId")
                        .HasColumnType("text");

                    b.Property<string>("AttachmentURL")
                        .HasColumnType("text");

                    b.Property<string>("AuthorId")
                        .HasColumnType("text");

                    b.Property<string>("ChannelId")
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("EditedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("MessageId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ChannelId", "AuthorId")
                        .IsUnique();

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Domain.Entities.Reaction", b =>
                {
                    b.Property<string>("ReactionId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Emoji")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("MessageId")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("ReactionId");

                    b.HasIndex("UserId");

                    b.HasIndex("MessageId", "UserId")
                        .IsUnique();

                    b.ToTable("Reactions");
                });

            modelBuilder.Entity("Domain.Entities.Role", b =>
                {
                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.Property<string>("Color")
                        .HasMaxLength(7)
                        .HasColumnType("character varying(7)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Permissions")
                        .HasColumnType("text");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.Property<string>("RoleName")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("ServerId")
                        .HasColumnType("text");

                    b.HasKey("RoleId");

                    b.HasIndex("ServerId")
                        .IsUnique();

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Domain.Entities.Server", b =>
                {
                    b.Property<string>("ServerId")
                        .HasColumnType("text");

                    b.Property<string>("BannerImage")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("OwnerId")
                        .HasColumnType("text");

                    b.Property<string>("ServerName")
                        .HasColumnType("text");

                    b.HasKey("ServerId");

                    b.HasIndex("OwnerId")
                        .IsUnique();

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("Domain.Entities.ServerBan", b =>
                {
                    b.Property<string>("BanId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Reason")
                        .HasColumnType("text");

                    b.Property<string>("ServerId")
                        .HasColumnType("text");

                    b.Property<DateTime>("UnbannedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("BanId");

                    b.HasIndex("UserId");

                    b.HasIndex("ServerId", "UserId")
                        .IsUnique();

                    b.ToTable("ServerBans");
                });

            modelBuilder.Entity("Domain.Entities.ServerMember", b =>
                {
                    b.Property<string>("ServerMemberId")
                        .HasColumnType("text");

                    b.Property<bool>("IsBanned")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("JoinedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("MemberId")
                        .HasColumnType("text");

                    b.Property<string>("ServerId")
                        .HasColumnType("text");

                    b.HasKey("ServerMemberId");

                    b.HasIndex("MemberId");

                    b.HasIndex("ServerId", "MemberId")
                        .IsUnique();

                    b.ToTable("ServerMembers");
                });

            modelBuilder.Entity("Domain.Entities.UserRole", b =>
                {
                    b.Property<string>("UserRoleId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("UserRoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId", "RoleId")
                        .IsUnique();

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("Domain.Entities.VoiceState", b =>
                {
                    b.Property<string>("VoiceStateId")
                        .HasColumnType("text");

                    b.Property<string>("ChannelId")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeafened")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsMuted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSelfDeafened")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSelfMuted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsStreaming")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSuppressed")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsVideoEnabled")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsVoiceActivityDetected")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsVoiceActivityEnabled")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsVoiceConnected")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsVoiceDisconnected")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("JoinedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("LeftAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("VoiceStateId");

                    b.HasIndex("ChannelId");

                    b.HasIndex("UserId", "ChannelId")
                        .IsUnique();

                    b.ToTable("VoiceState");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Channel", b =>
                {
                    b.HasOne("Domain.Entities.Channel", "ParentChannel")
                        .WithMany()
                        .HasForeignKey("ParentChannelId");

                    b.HasOne("Domain.Entities.Server", "Server")
                        .WithMany("Channels")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("ParentChannel");

                    b.Navigation("Server");
                });

            modelBuilder.Entity("Domain.Entities.Invite", b =>
                {
                    b.HasOne("Domain.Entities.Channel", "Channel")
                        .WithMany()
                        .HasForeignKey("ChannelId");

                    b.HasOne("Domain.Entities.AppUser", "User")
                        .WithMany("Invites")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("Domain.Entities.Server", "Server")
                        .WithMany("Invites")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Channel");

                    b.Navigation("Server");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.Message", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", "User")
                        .WithMany("Messages")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.Entities.Channel", "Channel")
                        .WithMany("Messages")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Channel");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.Reaction", b =>
                {
                    b.HasOne("Domain.Entities.Message", "Message")
                        .WithMany("Reactions")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.Entities.AppUser", "User")
                        .WithMany("Reactions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Message");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.Role", b =>
                {
                    b.HasOne("Domain.Entities.Server", "Server")
                        .WithMany()
                        .HasForeignKey("ServerId");

                    b.Navigation("Server");
                });

            modelBuilder.Entity("Domain.Entities.Server", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", "Owner")
                        .WithMany("OwnedServers")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Domain.Entities.ServerBan", b =>
                {
                    b.HasOne("Domain.Entities.Server", "Server")
                        .WithMany("Bans")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.Entities.AppUser", "User")
                        .WithMany("ServerBans")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Server");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.ServerMember", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", "Member")
                        .WithMany("ServerMembers")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.Entities.Server", "Server")
                        .WithMany("ServerMembers")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Member");

                    b.Navigation("Server");
                });

            modelBuilder.Entity("Domain.Entities.UserRole", b =>
                {
                    b.HasOne("Domain.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.HasOne("Domain.Entities.AppUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.VoiceState", b =>
                {
                    b.HasOne("Domain.Entities.Channel", "Channel")
                        .WithMany()
                        .HasForeignKey("ChannelId");

                    b.HasOne("Domain.Entities.AppUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Channel");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Domain.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.AppUser", b =>
                {
                    b.Navigation("Invites");

                    b.Navigation("Messages");

                    b.Navigation("OwnedServers");

                    b.Navigation("Reactions");

                    b.Navigation("ServerBans");

                    b.Navigation("ServerMembers");
                });

            modelBuilder.Entity("Domain.Entities.Channel", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("Domain.Entities.Message", b =>
                {
                    b.Navigation("Reactions");
                });

            modelBuilder.Entity("Domain.Entities.Server", b =>
                {
                    b.Navigation("Bans");

                    b.Navigation("Channels");

                    b.Navigation("Invites");

                    b.Navigation("ServerMembers");
                });
#pragma warning restore 612, 618
        }
    }
}
