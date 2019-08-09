namespace ApiCatchFilms.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterEntities : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "status", c => c.Int(nullable: false));
            AddColumn("dbo.Movies", "cover_url", c => c.String(maxLength: 800));
            AddColumn("dbo.Movies", "image_url", c => c.String(maxLength: 800));
            AddColumn("dbo.Movies", "raiting", c => c.Single(nullable: false));
            AddColumn("dbo.Users", "image_user_url", c => c.String(maxLength: 800));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "image_user_url");
            DropColumn("dbo.Movies", "raiting");
            DropColumn("dbo.Movies", "image_url");
            DropColumn("dbo.Movies", "cover_url");
            DropColumn("dbo.Movies", "status");
        }
    }
}
