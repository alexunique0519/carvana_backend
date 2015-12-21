namespace carpool_web_backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.RideRequest", newName: "RideRequestModels");
            RenameTable(name: "dbo.RideShare", newName: "RideShareModels");
            DropForeignKey("dbo.RideRequest", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.RideRequestModels", new[] { "UserId" });
            AddColumn("dbo.RideShareModels", "SeatsAvailable", c => c.Int(nullable: false));
            AlterColumn("dbo.RideRequestModels", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.RideRequestModels", "UserId");
            AddForeignKey("dbo.RideRequestModels", "UserId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.RideShareModels", "SeatsAvalible");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RideShareModels", "SeatsAvalible", c => c.Int(nullable: false));
            DropForeignKey("dbo.RideRequestModels", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.RideRequestModels", new[] { "UserId" });
            AlterColumn("dbo.RideRequestModels", "UserId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.RideShareModels", "SeatsAvailable");
            CreateIndex("dbo.RideRequestModels", "UserId");
            AddForeignKey("dbo.RideRequest", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.RideShareModels", newName: "RideShare");
            RenameTable(name: "dbo.RideRequestModels", newName: "RideRequest");
        }
    }
}
