using Microsoft.EntityFrameworkCore.Migrations;

namespace TemplateKafka.Producer.Infra.Data.Mappings.Entity.Seeds
{
    public static class ProductStatusSeed
    {
        public static void Seed(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT dbo.ProductStatus ON  

                INSERT INTO dbo.ProductStatus (Id, Name) 
                   VALUES 
                    (1,'Created'),
                	(2,'Inactivated'),
                	(3,'Deleted')
                
                SET IDENTITY_INSERT dbo.ProductStatus OFF
            ");
        }
    }
}
