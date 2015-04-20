namespace App.SAC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RaceDateNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Races", "RaceDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Races", "RaceDate", c => c.DateTime(nullable: false));
        }
    }
}
