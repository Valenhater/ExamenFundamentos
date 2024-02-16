using ExamenFundamentos.Models;

namespace ExamenFundamentos.Repositories
{
    public interface IRepositoryComics
    {
        List<Comic> GetComics();
        void InsertComic(int id, string nombre, string imagen, string descripcion);
        Comic GetComicsNombre(string nombre);
        List<string> GetNombres();
        void DeleteComic(int id);   

        Comic GetComicsID(int id);

        void InsertComicProcedure( string nombre, string imagen, string descripcion);
    }
}
