namespace City_Corporate_Book.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aaa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MemberUserLocationWiseComplains", "Agree", c => c.Int(nullable: false));
            AddColumn("dbo.MemberUserLocationWiseComplains", "Disagree", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MemberUserLocationWiseComplains", "Disagree");
            DropColumn("dbo.MemberUserLocationWiseComplains", "Agree");
        }
    }
}
