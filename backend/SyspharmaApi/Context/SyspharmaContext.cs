using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using SyspharmaApi.Models;

namespace SyspharmaApi.Context;

public partial class SyspharmaContext : DbContext
{
    public SyspharmaContext()
    {
    }

    public SyspharmaContext(DbContextOptions<SyspharmaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Config> Configs { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Error> Errors { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<InventoryMovement> InventoryMovements { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductBatch> ProductBatches { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserToken> UserTokens { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Config>(entity =>
        {
            entity.HasKey(e => e.Idconfig).HasName("PRIMARY");

            entity.ToTable("config");

            entity.Property(e => e.Idconfig).HasColumnName("idconfig");
            entity.Property(e => e.Canshow)
                .HasDefaultValueSql("'1'")
                .HasColumnName("canshow");
            entity.Property(e => e.Description)
                .HasMaxLength(512)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Value).HasColumnName("value");
            entity.Property(e => e.Valueisbool).HasColumnName("valueisbool");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Idcustomer).HasName("PRIMARY");

            entity.ToTable("customer");

            entity.Property(e => e.Idcustomer).HasColumnName("idcustomer");
            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("active");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createdat");
            entity.Property(e => e.Document)
                .HasMaxLength(30)
                .HasColumnName("document");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Othername)
                .HasMaxLength(100)
                .HasColumnName("othername");
            entity.Property(e => e.Phone)
                .HasMaxLength(45)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Error>(entity =>
        {
            entity.HasKey(e => e.Iderror).HasName("PRIMARY");

            entity.ToTable("error");

            entity.Property(e => e.Iderror).HasColumnName("Iderror");
            entity.Property(e => e.Title)
                .HasMaxLength(512)
                .HasColumnName("title");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ErrorAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("errorat");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.Idinventory).HasName("PRIMARY");

            entity.ToTable("inventory");

            entity.HasIndex(e => e.Idproduct, "fk_inventory_product_idx");

            entity.HasIndex(e => e.Idstore, "fk_inventory_store_idx");

            entity.Property(e => e.Idinventory).HasColumnName("idinventory");
            entity.Property(e => e.Idproduct).HasColumnName("idproduct");
            entity.Property(e => e.Idstore).HasColumnName("idstore");
            entity.Property(e => e.Minimum)
                .HasPrecision(12, 3)
                .HasColumnName("minimum");
            entity.Property(e => e.Quantity)
                .HasPrecision(12, 3)
                .HasColumnName("quantity");

            entity.HasOne(d => d.IdproductNavigation).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.Idproduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_inventory_product");

            entity.HasOne(d => d.IdstoreNavigation).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.Idstore)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_inventory_store");
        });

        modelBuilder.Entity<InventoryMovement>(entity =>
        {
            entity.HasKey(e => e.IdinventoryMovement).HasName("PRIMARY");

            entity.ToTable("inventory_movement");

            entity.HasIndex(e => e.Idinventory, "fk_inventory_movement_inventory_idx");

            entity.Property(e => e.IdinventoryMovement).HasColumnName("idinventory_movement");
            entity.Property(e => e.Balance)
                .HasPrecision(12, 3)
                .HasColumnName("balance");
            entity.Property(e => e.Datemov)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("datemov");
            entity.Property(e => e.Idinventory).HasColumnName("idinventory");
            entity.Property(e => e.Isentry).HasColumnName("isentry");
            entity.Property(e => e.Quantity)
                .HasPrecision(12, 3)
                .HasColumnName("quantity");

            entity.HasOne(d => d.IdinventoryNavigation).WithMany(p => p.InventoryMovements)
                .HasForeignKey(d => d.Idinventory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_inventory_movement_inventory");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Idorder).HasName("PRIMARY");

            entity.ToTable("order");

            entity.HasIndex(e => e.Idcustomer, "fk_order_customer");

            entity.HasIndex(e => e.Idstore, "fk_order_store");

            entity.HasIndex(e => e.Iduser, "fk_order_user");

            entity.HasIndex(e => e.Ordernumber, "ordernumber_UNIQUE").IsUnique();

            entity.Property(e => e.Idorder).HasColumnName("idorder");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createdat");
            entity.Property(e => e.Discountvalue)
                .HasPrecision(12, 2)
                .HasColumnName("discountvalue");
            entity.Property(e => e.Idcustomer).HasColumnName("idcustomer");
            entity.Property(e => e.Idstore).HasColumnName("idstore");
            entity.Property(e => e.Iduser).HasColumnName("iduser");
            entity.Property(e => e.Notes)
                .HasColumnType("text")
                .HasColumnName("notes");
            entity.Property(e => e.Orderdate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("orderdate");
            entity.Property(e => e.Ordernumber)
                .HasMaxLength(50)
                .HasColumnName("ordernumber");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'1'")
                .HasColumnName("status");
            entity.Property(e => e.Subtotal)
                .HasPrecision(12, 2)
                .HasColumnName("subtotal");
            entity.Property(e => e.Totalvalue)
                .HasPrecision(12, 2)
                .HasColumnName("totalvalue");

            entity.HasOne(d => d.IdcustomerNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Idcustomer)
                .HasConstraintName("fk_order_customer");

            entity.HasOne(d => d.IdstoreNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Idstore)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_store");

            entity.HasOne(d => d.IduserNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Iduser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_user");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Idorderitem).HasName("PRIMARY");

            entity.ToTable("order_item");

            entity.HasIndex(e => e.Idbatch, "fk_orderitem_batch");

            entity.HasIndex(e => e.Idorder, "fk_orderitem_order");

            entity.HasIndex(e => e.Idproduct, "fk_orderitem_product");

            entity.Property(e => e.Idorderitem).HasColumnName("idorderitem");
            entity.Property(e => e.Discountvalue)
                .HasPrecision(12, 2)
                .HasColumnName("discountvalue");
            entity.Property(e => e.Idbatch).HasColumnName("idbatch");
            entity.Property(e => e.Idorder).HasColumnName("idorder");
            entity.Property(e => e.Idproduct).HasColumnName("idproduct");
            entity.Property(e => e.Notes)
                .HasColumnType("text")
                .HasColumnName("notes");
            entity.Property(e => e.Quantity)
                .HasPrecision(12, 3)
                .HasColumnName("quantity");
            entity.Property(e => e.Totalvalue)
                .HasPrecision(12, 2)
                .HasColumnName("totalvalue");
            entity.Property(e => e.Unitcost)
                .HasPrecision(12, 2)
                .HasColumnName("unitcost");
            entity.Property(e => e.Unitprice)
                .HasPrecision(12, 2)
                .HasColumnName("unitprice");

            entity.HasOne(d => d.IdbatchNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.Idbatch)
                .HasConstraintName("fk_orderitem_batch");

            entity.HasOne(d => d.IdorderNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.Idorder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orderitem_order");

            entity.HasOne(d => d.IdproductNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.Idproduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orderitem_product");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Idpayment).HasName("PRIMARY");

            entity.ToTable("payment");

            entity.HasIndex(e => e.Idorder, "fk_payment_order");

            entity.Property(e => e.Idpayment).HasColumnName("idpayment");
            entity.Property(e => e.Amount)
                .HasPrecision(12, 2)
                .HasColumnName("amount");
            entity.Property(e => e.Authorizationcode)
                .HasMaxLength(100)
                .HasColumnName("authorizationcode");
            entity.Property(e => e.Cardlastdigits)
                .HasMaxLength(4)
                .HasColumnName("cardlastdigits");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createdat");
            entity.Property(e => e.Idorder).HasColumnName("idorder");
            entity.Property(e => e.Installments)
                .HasDefaultValueSql("'1'")
                .HasColumnName("installments");
            entity.Property(e => e.Notes)
                .HasColumnType("text")
                .HasColumnName("notes");
            entity.Property(e => e.Paymentdate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("paymentdate");
            entity.Property(e => e.Paymentmethod).HasColumnName("paymentmethod");

            entity.HasOne(d => d.IdorderNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Idorder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_payment_order");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Idproduct).HasName("PRIMARY");

            entity.ToTable("product");

            entity.HasIndex(e => e.Internalcode, "interalcode_UNIQUE").IsUnique();

            entity.Property(e => e.Idproduct).HasColumnName("idproduct");
            entity.Property(e => e.Cost)
                .HasPrecision(12, 2)
                .HasColumnName("cost");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createdat");
            entity.Property(e => e.Description)
                .HasColumnType("tinytext")
                .HasColumnName("description");
            entity.Property(e => e.Internalcode)
                .HasMaxLength(655)
                .HasColumnName("internalcode");
            entity.Property(e => e.Price)
                .HasPrecision(12, 2)
                .HasColumnName("price");
        });

        modelBuilder.Entity<ProductBatch>(entity =>
        {
            entity.HasKey(e => e.Idbatch).HasName("PRIMARY");

            entity.ToTable("product_batch");

            entity.HasIndex(e => e.Idproduct, "fk_productbatch_product");

            entity.HasIndex(e => e.Idsupplier, "fk_productbatch_supplier");

            entity.Property(e => e.Idbatch).HasColumnName("idbatch");
            entity.Property(e => e.Batchcode)
                .HasMaxLength(100)
                .HasColumnName("batchcode");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createdat");
            entity.Property(e => e.Expirationdate).HasColumnName("expirationdate");
            entity.Property(e => e.Idproduct).HasColumnName("idproduct");
            entity.Property(e => e.Idsupplier).HasColumnName("idsupplier");
            entity.Property(e => e.Manufacturingdate).HasColumnName("manufacturingdate");
            entity.Property(e => e.Quantity)
                .HasPrecision(12, 3)
                .HasColumnName("quantity");
            entity.Property(e => e.Unitcost)
                .HasPrecision(12, 2)
                .HasColumnName("unitcost");

            entity.HasOne(d => d.IdproductNavigation).WithMany(p => p.ProductBatches)
                .HasForeignKey(d => d.Idproduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_productbatch_product");

            entity.HasOne(d => d.IdsupplierNavigation).WithMany(p => p.ProductBatches)
                .HasForeignKey(d => d.Idsupplier)
                .HasConstraintName("fk_productbatch_supplier");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.Idstore).HasName("PRIMARY");

            entity.ToTable("store");

            entity.Property(e => e.Idstore).HasColumnName("idstore");
            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("active");
            entity.Property(e => e.Cnpj)
                .HasMaxLength(30)
                .HasColumnName("cnpj");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.Property(e => e.Fantasyname)
                .HasMaxLength(100)
                .HasColumnName("fantasyname");
            entity.Property(e => e.Phone)
                .HasMaxLength(45)
                .HasColumnName("phone");
            entity.Property(e => e.Socialname)
                .HasMaxLength(100)
                .HasColumnName("socialname");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Idsupplier).HasName("PRIMARY");

            entity.ToTable("supplier");

            entity.Property(e => e.Idsupplier).HasColumnName("idsupplier");
            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("active");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createdat");
            entity.Property(e => e.Document)
                .HasMaxLength(30)
                .HasColumnName("document");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .HasColumnName("phone");
            entity.Property(e => e.Tradename)
                .HasMaxLength(255)
                .HasColumnName("tradename");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iduser).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.Idstore, "fk_user_store_idx");

            entity.Property(e => e.Iduser).HasColumnName("iduser");
            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("active");
            entity.Property(e => e.Cpf)
                .HasMaxLength(30)
                .HasColumnName("cpf");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createdat");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.Property(e => e.Idstore).HasColumnName("idstore");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Pass).HasColumnName("pass");
            entity.Property(e => e.Phone)
                .HasMaxLength(45)
                .HasColumnName("phone");

            entity.HasOne(d => d.IdstoreNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Idstore)
                .HasConstraintName("fk_user_store");
        });

        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.HasKey(e => e.Idusertoken).HasName("PRIMARY");

            entity.ToTable("user_token");

            entity.HasIndex(e => e.Iduser, "fk_usertokens_user_idx");

            entity.Property(e => e.Idusertoken).HasColumnName("idusertoken");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpiresAt)
                .HasColumnType("datetime")
                .HasColumnName("expires_at");
            entity.Property(e => e.Iduser).HasColumnName("iduser");
            entity.Property(e => e.Revoked)                
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("revoked");
            entity.Property(e => e.Token)
                .HasMaxLength(512)
                .HasColumnName("token");

            entity.HasOne(d => d.IduserNavigation).WithMany(p => p.UserTokens)
                .HasForeignKey(d => d.Iduser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usertokens_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
