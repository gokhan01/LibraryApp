using LibraryApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>().Property(x => x.RegisterDate).HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Book>()
            .HasMany(p => p.Members)
            .WithMany(p => p.Books)
            .UsingEntity<MemberBook>(
                j => j
                    .HasOne(pt => pt.Member)
                    .WithMany(t => t.MemberBooks)
                    .HasForeignKey(pt => pt.MemberId),
                j => j
                    .HasOne(pt => pt.Book)
                    .WithMany(p => p.MemberBooks)
                    .HasForeignKey(pt => pt.BookId),
                j =>
                {
                    j.Property(pt => pt.DeliveryDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
                    j.HasKey(t => t.Id);
                });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberBook> MemberBooks { get; set; }
    }
}
