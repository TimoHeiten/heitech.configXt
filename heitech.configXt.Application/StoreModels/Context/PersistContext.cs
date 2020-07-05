using System;
using heitech.configXt.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace heitech.configXt.Application
{
    public class PersistContext : DbContext
    {
        public string Connection {get;set;}
        public PersistContext()
        {
            
        }

        public PersistContext(DbContextOptions options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder oB)
        {
            if (!oB.IsConfigured)
            {
                string dbConnect = Connection ?? "Data Source=" + System.IO.Path.Combine(Environment.CurrentDirectory, "config.db") + ";";
                oB.UseSqlite(dbConnect);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var configEntity = builder.Entity<ConfigEntity>();
            configEntity.HasKey(x => x.Id);
            configEntity.Property(x => x.Name)
                        .HasMaxLength(255)
                        .IsRequired();
            configEntity.Property(x => x.Value)
                        .HasMaxLength(255)
                        .IsRequired();
            configEntity.Ignore(x => x.CrudOperationName);
            configEntity.HasOne(x => x.AppClaim)
                        .WithOne(a => a.ConfigEntity)
                        .HasForeignKey(nameof(ApplicationClaim));

            var user = builder.Entity<UserEntity>();
            user.HasKey(x => x.Id);
            user.Property(u => u.Name)
                .HasMaxLength(150)
                .IsRequired();
            user.Property(u => u.PasswordHash)
                .HasMaxLength(255)
                .IsRequired();
            
            var claim = builder.Entity<ApplicationClaim>();
            claim.HasKey(x => x.Id);
            claim.Property(x => x.Name)
                 .HasMaxLength(150)
                 .IsRequired();
            claim.Property(x => x.CanRead)
                 .IsRequired();
            claim.Property(x => x.CanWrite)
                 .IsRequired();
            claim.HasOne(c => c.User).WithMany(u => u.Claims);
            claim.Ignore(c => c.ConfigEntityName);
        }
    }
}