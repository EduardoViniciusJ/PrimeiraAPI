using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeiraAPI.Migrations
{
    /// <inheritdoc />
    public partial class PopulandoProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) " +
                "Values('X2H Mini', 'A série Xlite V3 representa o ápice em termos de performance para jogadores, equipado com o sensor PAW3395, garantindo uma qualidade excepcional', '719.91', 'mouse.png', '30', GETDATE(), 3)");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Produtos");
        }
    }
}
