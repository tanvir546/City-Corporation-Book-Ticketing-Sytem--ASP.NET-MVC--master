namespace City_Corporate_Book.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MemberUserLocationWiseComplains", "Role", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MemberUserLocationWiseComplains", "Role");
        }
    }
}
