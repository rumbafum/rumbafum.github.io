namespace App.SAC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RaceResultPoints : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RaceResults", "Points", c => c.Int(nullable: false));
            CreateIndex("dbo.Athletes", "TeamId");
            CreateIndex("dbo.Athletes", "AgeRankId");
            CreateIndex("dbo.RaceResults", "RaceId");
            CreateIndex("dbo.RaceResults", "AthleteId");
            AddForeignKey("dbo.Athletes", "AgeRankId", "dbo.AgeRanks", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Athletes", "TeamId", "dbo.Teams", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RaceResults", "AthleteId", "dbo.Athletes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RaceResults", "RaceId", "dbo.Races", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RaceResults", "RaceId", "dbo.Races");
            DropForeignKey("dbo.RaceResults", "AthleteId", "dbo.Athletes");
            DropForeignKey("dbo.Athletes", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Athletes", "AgeRankId", "dbo.AgeRanks");
            DropIndex("dbo.RaceResults", new[] { "AthleteId" });
            DropIndex("dbo.RaceResults", new[] { "RaceId" });
            DropIndex("dbo.Athletes", new[] { "AgeRankId" });
            DropIndex("dbo.Athletes", new[] { "TeamId" });
            DropColumn("dbo.RaceResults", "Points");
        }
    }
}
