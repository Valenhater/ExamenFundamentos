using ExamenFundamentos.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ExamenFundamentos.Repositories
{
    #region PROCEDURES
    /*
        create or replace procedure sp_delete_comic
        (p_idcomic COMICS.IDCOMIC%TYPE)
        as
        begin
          delete from COMICS where IDCOMIC = p_idcomic;
          commit;
         end;

            CREATE OR REPLACE PROCEDURE SP_INSERT_COMIC(
            nombre IN VARCHAR2,
            imagen IN VARCHAR2,
            descripcion IN VARCHAR2
        )
        IS
            maxID NUMBER;
        BEGIN
            SELECT NVL(MAX(IDCOMIC), 0) INTO maxID FROM COMICS;

            maxID := maxID + 1;

            INSERT INTO COMICS (IDCOMIC, NOMBRE, IMAGEN, DESCRIPCION)
            VALUES (maxID, nombre, imagen, descripcion);
    
            COMMIT;
        END SP_INSERT_COMIC;
     */
    #endregion
    public class RepositoryComicsOracle : IRepositoryComics
    {
        private DataTable tablaComics;
        private OracleConnection cn;
        private OracleCommand com;

        public RepositoryComicsOracle()
        {
            string connectionString = "Data Source=LOCALHOST:1521/XE; Persist Security Info=True; User Id=SYSTEM; Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand(); 
            this.com.Connection = this.cn;
            string sql = "select * from COMICS";
            OracleDataAdapter ad = new OracleDataAdapter(sql,this.cn);
            this.tablaComics = new DataTable();
            ad.Fill(this.tablaComics);
        }

        public void DeleteComic(int id)
        {
            OracleParameter pamId = new OracleParameter(":p_idcomic", id);
            this.com.Parameters.Add(pamId);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "sp_delete_comic";
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable() select datos;
            List<Comic> comics = new List<Comic>();
            foreach (var row in consulta)
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
            foreach (string nom in consulta)
            {
                nombres.Add(nom);
            }
            return nombres;
        }

        public void InsertComic(int id, string nombre, string imagen, string descripcion)
        {
            id = tablaComics.AsEnumerable().Max(datos => datos.Field<int>("IDCOMIC"));
            id += id + 1;
            string sql = "insert into COMICS values(:idcomic, :nombre, :imagen, :descripcion)";
            OracleParameter pamId = new OracleParameter(":idcomic",id);
            this.com.Parameters.Add(pamId);
            OracleParameter pamNom = new OracleParameter(":nombre", nombre);
            this.com.Parameters.Add(pamNom);
            OracleParameter pamImg = new OracleParameter(":imagen", imagen);
            this.com.Parameters.Add(pamImg);
            OracleParameter pamDesc = new OracleParameter(":descripcion", descripcion);
            this.com.Parameters.Add(pamDesc);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void InsertComicProcedure( string nombre, string imagen, string descripcion)
        {
            OracleParameter pamNom = new OracleParameter(":nombre", nombre);
            this.com.Parameters.Add(pamNom);
            OracleParameter pamImg = new OracleParameter(":imagen", imagen);
            this.com.Parameters.Add(pamImg);
            OracleParameter pamDesc = new OracleParameter(":descripcion", descripcion);
            this.com.Parameters.Add(pamDesc);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERT_COMIC";
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}
