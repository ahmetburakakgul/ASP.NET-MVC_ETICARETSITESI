namespace ETicaretSitesi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataBase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Adres",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Baslik = c.String(nullable: false, maxLength: 50),
                        Tanım = c.String(nullable: false, maxLength: 250),
                        OlusturmaTarihi = c.DateTime(nullable: false),
                        GuncellemeTarihi = c.DateTime(),
                        Member_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Members", t => t.Member_Id, cascadeDelete: true)
                .Index(t => t.Member_Id);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Adı = c.String(maxLength: 25),
                        Soyadı = c.String(maxLength: 25),
                        Email = c.String(nullable: false, maxLength: 60),
                        Biografi = c.String(maxLength: 300),
                        Password = c.String(nullable: false, maxLength: 60),
                        ProfilResmi = c.String(maxLength: 150),
                        MemberType = c.Int(nullable: false),
                        OlusturmaTarihi = c.DateTime(nullable: false),
                        GuncellemeTarihi = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 300),
                        Member_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                        OlusturmaTarihi = c.DateTime(nullable: false),
                        GuncellemeTarihi = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Members", t => t.Member_Id, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.Member_Id)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UrunAdi = c.String(nullable: false, maxLength: 30),
                        UrunAcıklama = c.String(maxLength: 300),
                        Fiyat = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UrunResmi = c.String(),
                        SatıştaMı = c.Boolean(nullable: false),
                        Stok = c.Int(nullable: false),
                        YıldızVerenKullanıcı = c.Int(nullable: false),
                        YıldızSayısı = c.Int(nullable: false),
                        Category_Id = c.Int(nullable: false),
                        İsDeleted = c.Boolean(),
                        OlusturmaTarihi = c.DateTime(nullable: false),
                        GuncellemeTarihi = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.Category_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Adi = c.String(nullable: false, maxLength: 50),
                        Aciklama = c.String(maxLength: 250),
                        ParentId = c.Int(),
                        İsDeleted = c.Boolean(),
                        OlusturmaTarihi = c.DateTime(nullable: false),
                        GuncellemeTarihi = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Fiyat = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AlımAdet = c.Int(nullable: false),
                        İndirim = c.Int(),
                        OlusturmaTarihi = c.DateTime(nullable: false),
                        GuncellemeTarihi = c.DateTime(),
                        Product_Id = c.Int(nullable: false),
                        Order_Id = c.Guid(nullable: false),
                        Orders_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Orders_Id)
                .ForeignKey("dbo.Product", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.Product_Id)
                .Index(t => t.Orders_Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Numara = c.String(maxLength: 12),
                        Durum = c.String(nullable: false, maxLength: 50),
                        Aciklama = c.String(maxLength: 250),
                        Member_Id = c.Int(nullable: false),
                        Adres = c.String(nullable: false),
                        OlusturmaTarihi = c.DateTime(nullable: false),
                        GuncellemeTarihi = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Members", t => t.Member_Id, cascadeDelete: true)
                .Index(t => t.Member_Id);
            
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OlusturmaTarihi = c.DateTime(nullable: false),
                        GuncellemeTarihi = c.DateTime(),
                        Konu = c.String(nullable: false, maxLength: 300),
                        OkunduMu = c.Boolean(nullable: false),
                        Member_To_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Members", t => t.Member_To_Id)
                .Index(t => t.Member_To_Id);
            
            CreateTable(
                "dbo.MessageReplies",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OlusturmaTarihi = c.DateTime(nullable: false),
                        GuncellemeTarihi = c.DateTime(),
                        Text = c.String(nullable: false, maxLength: 300),
                        OkunduMu = c.Boolean(nullable: false),
                        Message_Id = c.Guid(nullable: false),
                        Member_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Members", t => t.Member_Id, cascadeDelete: true)
                .ForeignKey("dbo.Message", t => t.Message_Id, cascadeDelete: true)
                .Index(t => t.Message_Id)
                .Index(t => t.Member_Id);
            
            CreateTable(
                "dbo.Galeri",
                c => new
                    {
                        ResimId = c.Int(nullable: false, identity: true),
                        ResimUrl = c.String(),
                    })
                .PrimaryKey(t => t.ResimId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Adres", "Member_Id", "dbo.Members");
            DropForeignKey("dbo.MessageReplies", "Message_Id", "dbo.Message");
            DropForeignKey("dbo.MessageReplies", "Member_Id", "dbo.Members");
            DropForeignKey("dbo.Message", "Member_To_Id", "dbo.Members");
            DropForeignKey("dbo.Comments", "Product_Id", "dbo.Product");
            DropForeignKey("dbo.OrderDetails", "Product_Id", "dbo.Product");
            DropForeignKey("dbo.OrderDetails", "Orders_Id", "dbo.Orders");
            DropForeignKey("dbo.Orders", "Member_Id", "dbo.Members");
            DropForeignKey("dbo.Product", "Category_Id", "dbo.Category");
            DropForeignKey("dbo.Category", "ParentId", "dbo.Category");
            DropForeignKey("dbo.Comments", "Member_Id", "dbo.Members");
            DropIndex("dbo.MessageReplies", new[] { "Member_Id" });
            DropIndex("dbo.MessageReplies", new[] { "Message_Id" });
            DropIndex("dbo.Message", new[] { "Member_To_Id" });
            DropIndex("dbo.Orders", new[] { "Member_Id" });
            DropIndex("dbo.OrderDetails", new[] { "Orders_Id" });
            DropIndex("dbo.OrderDetails", new[] { "Product_Id" });
            DropIndex("dbo.Category", new[] { "ParentId" });
            DropIndex("dbo.Product", new[] { "Category_Id" });
            DropIndex("dbo.Comments", new[] { "Product_Id" });
            DropIndex("dbo.Comments", new[] { "Member_Id" });
            DropIndex("dbo.Adres", new[] { "Member_Id" });
            DropTable("dbo.Galeri");
            DropTable("dbo.MessageReplies");
            DropTable("dbo.Message");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Category");
            DropTable("dbo.Product");
            DropTable("dbo.Comments");
            DropTable("dbo.Members");
            DropTable("dbo.Adres");
        }
    }
}
