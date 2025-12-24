using Data.InMemory;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Data.SqlServer
{
    public class MediaDbContext : DbContext
    {
        public MediaDbContext(DbContextOptions<MediaDbContext> options) : base(options) { }

        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<AudioTrack> Tracks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Credentials> Credentials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Playlist>().HasKey(p => p.Id);
            modelBuilder.Entity<AudioTrack>().HasKey(p => p.Id);
            modelBuilder.Entity<User>().HasKey(p => p.Id);
            modelBuilder.Entity<Credentials>().HasKey(p => p.Id);

            modelBuilder.Entity<Playlist>().HasMany(p => p.Tracks)
                .WithMany();

            modelBuilder.Entity<AudioTrack>().HasOne(t => t.User)
                .WithMany(p => p.Tracks)
                .HasForeignKey(u => u.UserId)
                .IsRequired(false);

            modelBuilder.Entity<User>().HasOne(c => c.Credentials);
        }
    }
}
