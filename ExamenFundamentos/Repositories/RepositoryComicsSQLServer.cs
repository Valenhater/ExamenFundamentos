using ExamenFundamentos.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Data;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExamenFundamentos.Repositories
{

    #region PROCEDURES
    /*
        create procedure SP_DELETE_COMIC
        (@idcomic int)
        as
	        delete from COMICS where IDCOMIC = @idcomic
        go


    
        create procedure SP_INSERT_COMIC
        @nombre NVARCHAR(100),@imagen NVARCHAR(100),@descripcion NVARCHAR(100)
        as
        begin
	        DECLARE @maxId INT;

	        SELECT @maxID = MAX(IDCOMIC) FROM COMICS;

	        SET @maxID = @maxId + 1;

	        INSERT INTO COMICS (IDCOMIC, NOMBRE, IMAGEN, DESCRIPCION)
	        VALUES (@maxId,@nombre,@imagen,@descripcion);
        END
    */
    #endregion
    public class RepositoryComicsSQLServer : IRepositoryComics
    {
        private DataTable tablaComics;
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryComicsSQLServer()
        {
            string connectionString = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            this.tablaComics = new DataTable();
            string sql = "select * from COMICS";
            SqlDataAdapter ad = new SqlDataAdapter(sql,this.cn);
            ad.Fill(this.tablaComics);

        }

        public void DeleteComic(int id)
        {
            this.com.Parameters.AddWithValue("@idcomic",id);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_DELETE_COMIC";
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();

        }

        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable() select datos;
            List<Comic> comics = new List<Comic>();
            foreach(var row in consulta)
            {
                Comic comic = new Comic
                {
                    IdComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION")
                };
                comics.Add(comic);
            }
            return comics;
        }

        public Comic GetComicsID(int id)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable() where datos.Field<int>("IDCOMIC") == id select datos;
            var row = consulta.First();
            Comic comic = new Comic
            {
                IdComic = row.Field<int>("IDCOMIC"),
                Nombre = row.Field<string>("NOMBRE"),
                Imagen = row.Field<string>("IMAGEN"),
                Descripcion = row.Field<string>("DESCRIPCION")
            };
            return comic;
        }

        public Comic GetComicsNombre(string nombre)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable() where datos.Field<string>("NOMBRE") == nombre select datos;
            var row = consulta.First();
            Comic comic = new Comic
            {
                IdComic = row.Field<int>("IDCOMIC"),
                Nombre = row.Field<string>("NOMBRE"),
                Imagen = row.Field<string>("IMAGEN"),
                Descripcion = row.Field<string>("DESCRIPCION")
            };
            return comic; 
        }

        public List<string> GetNombres()
        {
            var consulta = (from datos in this.tablaComics.AsEnumerable() select datos.Field<string>("NOMBRE")).Distinct();
            List<string> nombres = new List<string>();
            foreach(string nom in consulta)
            {
                nombres.Add(nom);
            }
            return nombres;
        }

        public void InsertComic(int id, string nombre, string imagen, string descripcion)
        {
            id = tablaComics.AsEnumerable().Max(datos => datos.Field<int>("IDCOMIC"));
            id += id + 1;
            string sql = "insert into COMICS values(@idcomic, @nombre, @imagen, @descripcion)";
            this.com.Parameters.AddWithValue("@idcomic",id);
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@imagen", imagen);
            this.com.Parameters.AddWithValue("@descripcion", descripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void InsertComicProcedure(string nombre, string imagen, string descripcion)
        {
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@imagen", imagen);
            this.com.Parameters.AddWithValue("@descripcion", descripcion);
            this.com.CommandType= CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERT_COMIC";
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}
