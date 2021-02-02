namespace City_Corporate_Book.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommentAgreeDisagreeStatus",
                c => new
                    {
                        No = c.Int(nullable: false, identity: true),
                        PostId = c.Int(nullable: false),
                        Action = c.Int(nullable: false),
                        UserEmail = c.String(),
                    })
                .PrimaryKey(t => t.No);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CommentAgreeDisagreeStatus");
        }
    }
}
