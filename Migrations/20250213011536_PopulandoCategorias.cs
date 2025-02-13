using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeiraAPI.Migrations
{
    /// <inheritdoc />
    public partial class PopulandoCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Categorias(Nome, ImageUrl) Values('Livros','livros.png')");
            mb.Sql("Insert into Categorias(Nome, ImageUrl) Values('Perifericos','periferico.png')");
            mb.Sql("Insert into Categorias(Nome, ImageUrl) Values('Jogos','jogos.png')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Categorias");
        }
    }
}
