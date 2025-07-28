using Microsoft.EntityFrameworkCore;

namespace WebFM_Style.Models;

public partial class FmStyleDbContext : DbContext
{
    public FmStyleDbContext()
    {
    }

    public FmStyleDbContext(DbContextOptions<FmStyleDbContext> options): base(options)
    {

    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountType> AccountTypes { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Collection> Collections { get; set; }

    public virtual DbSet<CollectionProduct> CollectionProducts { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductSizeColor> ProductSizeColors { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<ProductsInventory> ProductsInventorys { get; set; }

    public virtual DbSet<ReceiptProduct> ReceiptProducts { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(
            "Server=HT\\SQLEXPRESS;Database=FM_Style_DB;User Id=huy;Password=123456;MultipleActiveResultSets=true;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.Property(e => e.Birthday).HasColumnType("datetime");

            entity.HasOne(d => d.AccountType).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.AccountTypeId)
                .HasConstraintName("FK_Account_AccountType");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Account_Role");
        });

        modelBuilder.Entity<AccountType>(entity =>
        {
            entity.ToTable("AccountType");
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("Address");

            entity.HasOne(d => d.Account).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Address_Account");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

          
            entity.Property(e => e.Cdt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("CDT");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Categories)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK_Category_Suppliers");
        });

        modelBuilder.Entity<Collection>(entity =>
        {
            entity.ToTable("Collection");
        });

        modelBuilder.Entity<CollectionProduct>(entity =>
        {
            entity.ToTable("CollectionProduct");

            entity.Property(e => e.Cdt)
                .HasMaxLength(10)
                .HasDefaultValueSql("(getdate())")
                .IsFixedLength()
                .HasColumnName("CDT");

            entity.HasOne(d => d.Collection).WithMany(p => p.CollectionProducts)
                .HasForeignKey(d => d.CollectionId)
                .HasConstraintName("FK_CollectionProduct_Collection");

            entity.HasOne(d => d.Product).WithMany(p => p.CollectionProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_CollectionProduct_Products");
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.Property(e => e.Color1)
                .HasMaxLength(50)
                .HasColumnName("Color");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.Property(e => e.DiscountPercent)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Discount_percent");
        });

        modelBuilder.Entity<Image>(entity =>
        {

            entity.HasOne(d => d.Product).WithMany(p => p.Images)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Images_Products");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.CreateDay).HasColumnType("datetime");
            entity.Property(e => e.Total).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Account).WithMany(p => p.Oders)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Oders_Account");

            entity.HasOne(d => d.Discount).WithMany(p => p.Oders)
                .HasForeignKey(d => d.DiscountId)
                .HasConstraintName("FK_Oders_Discount");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductSizeColorId });

            entity.ToTable("Order_Items");

            entity.Property(e => e.ProductSizeColorId).HasColumnName("ProductSize_ColorId");

            entity.HasOne(d => d.Oder).WithMany(p => p.OderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Oder_Items_Oders");

            entity.HasOne(d => d.ProductSizeColor).WithMany(p => p.OderItems)
                .HasForeignKey(d => d.ProductSizeColorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Oder_Items_ProductSize_Colors");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");

            entity.HasOne(d => d.Oders).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrdersId)
                .HasConstraintName("FK_Payments_Oders");

            entity.HasOne(d => d.PaymentMethods).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PaymentMethodsId)
                .HasConstraintName("FK_Payments_PaymentMethods");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductTypeId)
                .HasConstraintName("FK_Products_ProductTypes");
        });

        modelBuilder.Entity<ProductSizeColor>(entity =>
        {
            entity.ToTable("ProductSize_Colors");

            entity.Property(e => e.ProductInventoryId).HasColumnName("Product_InventoryId");

            entity.HasOne(d => d.Color).WithMany(p => p.ProductSizeColors)
                .HasForeignKey(d => d.ColorId)
                .HasConstraintName("FK_ProductSize_Colors_Colors");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductSizeColors)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductSize_Colors_Products");

            entity.HasOne(d => d.ProductInventory).WithMany(p => p.ProductSizeColors)
                .HasForeignKey(d => d.ProductInventoryId)
                .HasConstraintName("FK_ProductSize_Colors_Products_Inventorys");

            entity.HasOne(d => d.Size).WithMany(p => p.ProductSizeColors)
                .HasForeignKey(d => d.SizeId)
                .HasConstraintName("FK_ProductSize_Colors_Size");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.Property(e => e.Cdt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("CDT");

            entity.HasOne(d => d.Category).WithMany(p => p.ProductTypes)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_ProductTypes_Category");
        });

        modelBuilder.Entity<ProductsInventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Product_Inventory");

            entity.ToTable("Products_Inventorys");
        });

        modelBuilder.Entity<ReceiptProduct>(entity =>
        {
            entity.ToTable("Receipt_Products");

            entity.Property(e => e.CreateDay).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ProductSizeColorId).HasColumnName("ProductSize_ColorId");

            entity.HasOne(d => d.ProductSizeColor).WithMany(p => p.ReceiptProducts)
                .HasForeignKey(d => d.ProductSizeColorId)
                .HasConstraintName("FK_Receipt_Products_ProductSize_Colors");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.ToTable("Size");

            entity.Property(e => e.Size1)
                .HasMaxLength(50)
                .HasColumnName("Size");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Cdt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("CDT");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
