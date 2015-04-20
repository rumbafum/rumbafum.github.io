namespace App.SAC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAgeRankInRaceResult : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RaceResults", "AgeRankId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RaceResults", "AgeRankId");
        }
    }
}
