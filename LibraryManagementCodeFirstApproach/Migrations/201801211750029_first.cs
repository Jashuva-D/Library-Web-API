namespace LibraryManagementCodeFirstApproach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Author",
                c => new
                    {
                        AuthorID = c.String(nullable: false, maxLength: 128),
                        firstName = c.String(),
                        lastName = c.String(),
                    })
                .PrimaryKey(t => t.AuthorID);
            
            CreateTable(
                "dbo.Book",
                c => new
                    {
                        BookID = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        GenreType = c.String(),
                    })
                .PrimaryKey(t => t.BookID);
            
            CreateTable(
                "dbo.BookRepository",
                c => new
                    {
                        BookID = c.String(nullable: false, maxLength: 128),
                        PublisherID = c.String(nullable: false, maxLength: 128),
                        Edition = c.String(nullable: false, maxLength: 128),
                        NumberOfCopies = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BookID, t.PublisherID, t.Edition })
                .ForeignKey("dbo.Book", t => t.BookID, cascadeDelete: true)
                .ForeignKey("dbo.Publisher", t => t.PublisherID, cascadeDelete: true)
                .Index(t => t.BookID)
                .Index(t => t.PublisherID);
            
            CreateTable(
                "dbo.Publisher",
                c => new
                    {
                        PublisherID = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        ContactNumber = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.PublisherID);
            
            CreateTable(
                "dbo.CustomerRepository",
                c => new
                    {
                        CustomerID = c.String(nullable: false, maxLength: 128),
                        BookID = c.String(nullable: false, maxLength: 128),
                        IssuedDate = c.DateTime(nullable: false),
                        ReceivedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.CustomerID, t.BookID })
                .ForeignKey("dbo.Book", t => t.BookID, cascadeDelete: true)
                .ForeignKey("dbo.Customer", t => t.CustomerID, cascadeDelete: true)
                .Index(t => t.CustomerID)
                .Index(t => t.BookID);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        CustomerID = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Email = c.String(),
                        MobileNumber = c.String(),
                    })
                .PrimaryKey(t => t.CustomerID);
            
            CreateTable(
                "dbo.BookAuthors",
                c => new
                    {
                        Book_BookID = c.String(nullable: false, maxLength: 128),
                        Author_AuthorID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Book_BookID, t.Author_AuthorID })
                .ForeignKey("dbo.Book", t => t.Book_BookID, cascadeDelete: true)
                .ForeignKey("dbo.Author", t => t.Author_AuthorID, cascadeDelete: true)
                .Index(t => t.Book_BookID)
                .Index(t => t.Author_AuthorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerRepository", "CustomerID", "dbo.Customer");
            DropForeignKey("dbo.CustomerRepository", "BookID", "dbo.Book");
            DropForeignKey("dbo.BookRepository", "PublisherID", "dbo.Publisher");
            DropForeignKey("dbo.BookRepository", "BookID", "dbo.Book");
            DropForeignKey("dbo.BookAuthors", "Author_AuthorID", "dbo.Author");
            DropForeignKey("dbo.BookAuthors", "Book_BookID", "dbo.Book");
            DropIndex("dbo.BookAuthors", new[] { "Author_AuthorID" });
            DropIndex("dbo.BookAuthors", new[] { "Book_BookID" });
            DropIndex("dbo.CustomerRepository", new[] { "BookID" });
            DropIndex("dbo.CustomerRepository", new[] { "CustomerID" });
            DropIndex("dbo.BookRepository", new[] { "PublisherID" });
            DropIndex("dbo.BookRepository", new[] { "BookID" });
            DropTable("dbo.BookAuthors");
            DropTable("dbo.Customer");
            DropTable("dbo.CustomerRepository");
            DropTable("dbo.Publisher");
            DropTable("dbo.BookRepository");
            DropTable("dbo.Book");
            DropTable("dbo.Author");
        }
    }
}
