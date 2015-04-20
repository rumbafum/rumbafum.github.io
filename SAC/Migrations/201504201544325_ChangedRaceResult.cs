namespace App.SAC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedRaceResult : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RaceResults", "Position", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RaceResults", "Position");
        }
    }
}
