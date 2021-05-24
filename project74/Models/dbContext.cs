using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace project74.Models
{
    public partial class dbContext : DbContext
    {
        public dbContext()
            : base("name=dbContext")
        {
        }

        public virtual DbSet<block_user> block_user { get; set; }
        public virtual DbSet<category> categories { get; set; }
        public virtual DbSet<image> images { get; set; }
        public virtual DbSet<package> packages { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<user_cat> user_cat { get; set; }
        public virtual DbSet<user> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<image>()
                .Property(e => e.img_url)
                .IsUnicode(false);

            modelBuilder.Entity<package>()
                .HasMany(e => e.images)
                .WithOptional(e => e.package)
                .WillCascadeOnDelete();

            modelBuilder.Entity<user_cat>()
                .HasMany(e => e.packages)
                .WithOptional(e => e.user_cat)
                .HasForeignKey(e => new { e.u_id, e.cat_id })
                .WillCascadeOnDelete();

            modelBuilder.Entity<user>()
                .Property(e => e.image_id)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.user_cat)
                .WithRequired(e => e.user)
                .HasForeignKey(e => e.u_id);
        }
    }
}
