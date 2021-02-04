using Microsoft.EntityFrameworkCore;
using ModelLayer.Models;

namespace RepositoryLayer
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<FriendList> FriendList { get; set; }
        public DbSet<FavoriteList> FavoriteLists { get; set; }

        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=WhatsThatSong;Trusted_Connection=True;");
        //    }
        //}
    }
}
