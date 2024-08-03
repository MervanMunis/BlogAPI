using System;
using BlogAPI.Entities;
using BlogAPI.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Author>? Authors { get; set; }
        public DbSet<Post>? Posts { get; set; }
        public DbSet<Comment>? Comments { get; set; }
        public DbSet<Like>? Likes { get; set; }
        public DbSet<CommentLike>? CommentLikes { get; set; }
        public DbSet<Bookmark>? Bookmarks { get; set; }
        public DbSet<ReadingList>? ReadingLists { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<SubCategory>? SubCategories { get; set; }
        public DbSet<PostSubCategory>? PostSubCategory { get; set; }
        public DbSet<Tag>? Tags { get; set; }
        public DbSet<PostTag>? PostTag { get; set; }
        public DbSet<Follower>? Followers { get; set; }
        public DbSet<Following>? Followings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many relationships
            modelBuilder.Entity<PostTag>()
                .HasKey(pt => new { pt.PostId, pt.TagId });

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.PostId);

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.TagId);

            modelBuilder.Entity<PostSubCategory>()
                .HasKey(ps => new { ps.PostId, ps.SubCategoriesId });

            modelBuilder.Entity<PostSubCategory>()
                .HasOne(ps => ps.Post)
                .WithMany(p => p.PostSubCategories)
                .HasForeignKey(ps => ps.PostId);

            modelBuilder.Entity<PostSubCategory>()
                .HasOne(ps => ps.SubCategory)
                .WithMany(sc => sc.PostSubCategories)
                .HasForeignKey(ps => ps.SubCategoriesId);

            modelBuilder.Entity<Follower>()
                .HasKey(f => new { f.AuthorId, f.FollowedAuthorId });

            modelBuilder.Entity<Follower>()
                .HasOne(f => f.Author)
                .WithMany(a => a.Following)
                .HasForeignKey(f => f.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Follower>()
                .HasOne(f => f.FollowedAuthor)
                .WithMany(a => a.Followers)
                .HasForeignKey(f => f.FollowedAuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure composite key for Bookmark entity
            modelBuilder.Entity<Bookmark>()
                .HasKey(b => new { b.PostId, b.AuthorId });

            // Configure BookMark relationship to avoid cascade delete cycles
            modelBuilder.Entity<Bookmark>()
                .HasOne(b => b.Post)
                .WithMany(p => p.Bookmarks)
                .HasForeignKey(b => b.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bookmark>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Bookmarks)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Comment relationship to avoid cascade delete cycles
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure composite key for Like entity
            modelBuilder.Entity<Like>()
                .HasKey(l => new { l.PostId, l.AuthorId });

            // Configure Like relationship to avoid cascade delete cycles
            modelBuilder.Entity<Like>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Author)
                .WithMany(a => a.Likes)
                .HasForeignKey(l => l.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure composite key for Bookmark entity
            modelBuilder.Entity<CommentLike>()
                .HasKey(l => new { l.CommentId, l.AuthorId });

            // Configure ReadingList relationship to avoid cascade delete cycles
            modelBuilder.Entity<ReadingList>()
                .HasOne(rl => rl.Post)
                .WithMany(p => p.ReadingLists)
                .HasForeignKey(rl => rl.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReadingList>()
                .HasOne(rl => rl.Author)
                .WithMany(a => a.ReadingLists)
                .HasForeignKey(rl => rl.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
