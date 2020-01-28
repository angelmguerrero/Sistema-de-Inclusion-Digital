using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Conexion
/// </summary>
public class Conexion
{
    public static SqlConnection cone()
    {

        SqlConnection conn = new SqlConnection("Data Source = WIN-N3F8Q1B09K2\\SQLEXPRESS; Initial Catalog = educafin;User ID=sa;Password=SIDITL2017*;Application Name=EntityFramework");
        conn.Open();
        return conn;
    }

    public Conexion()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    SqlConnection cn = new SqlConnection("Data Source = WIN-N3F8Q1B09K2\\SQLEXPRESS; Initial Catalog = educafin;User ID=sa;Password=SIDITL2017*;Application Name=EntityFramework");
    private SqlCommandBuilder cmb;
    DataSet ds = new DataSet();
    SqlDataAdapter da;
    SqlCommand comando;

    public void conectar()
    {
        try
        {
            cn.Open();          
        }
        catch
        {
            ////("se ha producido una excepción: " + e);
        }
        finally
        {
            cn.Close();
        }
    }

    public DataTable consulta(String sql)
    {
        DataTable dt = new DataTable();
        comando = new SqlCommand(sql, cn);
        da = new SqlDataAdapter(comando);
        da.Fill(dt);
        return dt;
    }

    public Boolean insertar(String sql)
    {
        cn.Open();
        comando = new SqlCommand(sql, cn);
        int contador = comando.ExecuteNonQuery();
        if (contador >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Boolean actualizar(String tabla, String campos, String condicion)
    {
        cn.Open();
        String update = "UPDATE " + tabla + " SET " + campos + " WHERE " + condicion + ";";
        //MessageBox.Show(update);
        SqlDataAdapter SDA = new SqlDataAdapter(update, cn);
        SDA.SelectCommand.ExecuteNonQuery();
        cn.Close();
        return true;
    }

    public static SqlConnection conexion()
    {
        SqlConnection conn = new SqlConnection("Data Source = WIN-N3F8Q1B09K2\\SQLEXPRESS; Initial Catalog = educafin;User ID=sa;Password=SIDITL2017*;Application Name=EntityFramework");
        conn.Open();
        return conn;
    }


}