namespace ApiCatchFilms.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesEntitiesDatabase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prices", "adult_price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Prices", "child_price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Prices", "old_man_price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Functions", "description", c => c.String(maxLength: 500));
            DropColumn("dbo.Prices", "type");
            DropColumn("dbo.Prices", "price");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Prices", "price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Prices", "type", c => c.Int(nullable: false));
            AlterColumn("dbo.Functions", "description", c => c.String(nullable: false, maxLength: 500));
            DropColumn("dbo.Prices", "old_man_price");
            DropColumn("dbo.Prices", "child_price");
            DropColumn("dbo.Prices", "adult_price");
        }
    }
}
