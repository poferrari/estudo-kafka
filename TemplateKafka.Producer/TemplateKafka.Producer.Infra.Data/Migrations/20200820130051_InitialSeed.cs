using Microsoft.EntityFrameworkCore.Migrations;
using TemplateKafka.Producer.Infra.Data.Mappings.Entity.Seeds;

namespace TemplateKafka.Producer.Infra.Data.Migrations
{
    public partial class InitialSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ProductStatusSeed.Seed(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
