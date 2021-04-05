using ETicaretSitesi.Models.CodeFirstVT;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ETicaretSitesi.Models.EntityFramework
{
    public class DataBaseContex : DbContext
    {




        static DataBaseContex()
        {
            Database.SetInitializer<DataBaseContex>(null);
        }

        public DataBaseContex() : base("Name=DataBaseContex")
        {

        }

        public DbSet<Adres> Adres { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Galeri> Galeris { get; set; }
        public DbSet<Members> Members { get; set; }
        public DbSet<Message> Mesajs { get; set; }
        public DbSet<MessageReplies> MesajYanıtlarıs { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Adres>()
                .HasRequired(t => t.Member)
                .WithMany(t=>t.Adres)
                .HasForeignKey(t => t.Member_Id);

            modelBuilder.Entity<Comments>()
                .HasRequired(t => t.Members)
                .WithMany(t=>t.Comments)
                .HasForeignKey(t => t.Member_Id);
            modelBuilder.Entity<Comments>()
                .HasRequired(t => t.Product)
                .WithMany(t=>t.Comments)
                .HasForeignKey(t => t.Product_Id);

            modelBuilder.Entity<OrderDetails>()
              .HasRequired(t => t.Product)
              .WithMany(t=>t.OrderDetails)
              .HasForeignKey(t => t.Product_Id);

            modelBuilder.Entity<Orders>()
              .HasRequired(t => t.Members)
              .WithMany(t=>t.Orders)
              .HasForeignKey(t => t.Member_Id);

            modelBuilder.Entity<Product>()
              .HasRequired(t => t.Category)
              .WithMany(t=>t.Products)
              .HasForeignKey(t => t.Category_Id);

            modelBuilder.Entity<MessageReplies>()
              .HasRequired(t => t.Members)
              .WithMany(t=>t.MesajYanıtlarıs)
              .HasForeignKey(t => t.Member_Id)
             .WillCascadeOnDelete(true);

            modelBuilder.Entity<MessageReplies>()
             .HasRequired(t => t.Message)
             .WithMany(t => t.MesajYanıtlarıs)
             .HasForeignKey(t => t.Message_Id)
             .WillCascadeOnDelete(true);

            modelBuilder.Entity<MessageReplies>()
             .HasRequired(t => t.Message)
             .WithMany(t => t.MesajYanıtlarıs)
             .HasForeignKey(t => t.Message_Id)
             .WillCascadeOnDelete(true);

            modelBuilder.Entity<Message>()
            .HasRequired(t => t.Members)
            .WithMany(t => t.Mesajs)
            .HasForeignKey(t => t.Member_To_Id)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
               .HasMany(e => e.Categories1)
               .WithOptional(e => e.Categories2)
               .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Category)
                .HasForeignKey(e => e.Category_Id)
                .WillCascadeOnDelete(false);
        }

    }
}