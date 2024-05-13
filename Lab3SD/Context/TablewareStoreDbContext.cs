using System;
using System.Collections.Generic;
using Lab3SD.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab3SD.Context;

public partial class TablewareStoreDbContext : DbContext
{
    public TablewareStoreDbContext()
    {
    }

    public TablewareStoreDbContext(DbContextOptions<TablewareStoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DiscountsPromotion> DiscountsPromotions { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ReviewsRating> ReviewsRatings { get; set; }

    public virtual DbSet<ShippingInformation> ShippingInformations { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.CustomerId).ValueGeneratedNever();
            
        });

        modelBuilder.Entity<DiscountsPromotion>(entity =>
        {
            entity.Property(e => e.DiscountId).ValueGeneratedNever();

        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.OrderId).ValueGeneratedNever();

        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.Property(e => e.OrderItemId).ValueGeneratedNever();

        });

        modelBuilder.Entity<PaymentTransaction>(entity =>
        {
            entity.Property(e => e.TransactionId).ValueGeneratedNever();

        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("Products_pk");

            entity.Property(e => e.ProductId).ValueGeneratedNever();
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("Product_Categories_pk");

            entity.Property(e => e.CategoryId).ValueGeneratedNever();
        });

        modelBuilder.Entity<ReviewsRating>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("Reviews_Ratings_pk");

            entity.Property(e => e.ReviewId).ValueGeneratedNever();
        });

        modelBuilder.Entity<ShippingInformation>(entity =>
        {
            entity.HasKey(e => e.ShippingId).HasName("Shipping_Information_pk");

            entity.Property(e => e.ShippingId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("Suppliers_pk");

            entity.Property(e => e.SupplierId).ValueGeneratedNever();
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("User_Accounts_pk");

            entity.Property(e => e.UserId).ValueGeneratedNever();
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("User_Roles_pk");

            entity.Property(e => e.RoleId).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
