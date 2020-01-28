using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PRUEBA1
{
    public class proveedor
    {
        public static SqlConnection conexion()
        {
            SqlConnection conn = new SqlConnection("Data Source = WIN-N3F8Q1B09K2\\SQLEXPRESS; Initial Catalog = educafin;User ID=sa;Password=SIDITL2017*;Application Name=EntityFramework");
            conn.Open();
            return conn;
        }
    }
}