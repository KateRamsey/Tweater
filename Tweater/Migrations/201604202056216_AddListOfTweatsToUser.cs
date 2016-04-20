namespace Tweater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddListOfTweatsToUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tweats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Body = c.String(maxLength: 140),
                        CreateDate = c.DateTime(nullable: false),
                        Author_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .Index(t => t.Author_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tweats", "Author_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Tweats", new[] { "Author_Id" });
            DropTable("dbo.Tweats");
        }
    }
}
