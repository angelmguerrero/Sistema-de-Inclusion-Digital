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

    public Conexion()
    {
    }

    SqlConnection cn = new SqlConnection("Data Source =Lahera\\Lahera; Initial Catalog = educafinEntregas; Integrated Security = True");
    //SqlConnection cn = new SqlConnection("Data Source = D1GLPXG2\\SQLEDSI; Initial Catalog = educafin;User ID=sa;Password=SIDITL2017*;Application Name=EntityFramework");
    //SqlConnection cn = new SqlConnection("Data Source=WIN-45Q4H6QQS40;Initial Catalog=educafin;Integrated Security=True");
    //SqlConnection cn = new SqlConnection("Data Source =LAHERACORP\\LAHERA; Initial Catalog = educafinEntregas; Integrated Security = True");
    SqlDataAdapter da;
    SqlCommand comando;


    public DataTable consulta(String sql)
    {
        System.Diagnostics.Debug.WriteLine(sql);
        DataTable dt = new DataTable();
        comando = new SqlCommand(sql, cn);
        da = new SqlDataAdapter(comando);
        da.Fill(dt);
        cn.Close();
        return dt;
    }

    public Boolean actualizar(String tabla, String campos, String condicion)
    {
        cn.Open();
        String update = "UPDATE " + tabla + " SET " + campos + " WHERE " + condicion + ";";
        SqlDataAdapter SDA = new SqlDataAdapter(update, cn);
        SDA.SelectCommand.ExecuteNonQuery();
        cn.Close();
        return true;
    }

    public static SqlConnection conexion()
    {
       SqlConnection conn = new SqlConnection("Data Source =Lahera\\Lahera; Initial Catalog = educafinEntregas; Integrated Security = True");
        // SqlConnection conn = new SqlConnection("Data Source = D1GLPXG2\\SQLEDSI; Initial Catalog = educafin;User ID=sa;Password=SIDITL2017*;Application Name=EntityFramework");
        //SqlConnection conn = new SqlConnection("Data Source =WIN-45Q4H6QQS40; Initial Catalog = educafin;User ID=sa;Password=SIDITL2017*; Application Name=EntityFramework");
       //SqlConnection conn = new SqlConnection("Data Source =LAHERACORP\\LAHERA; Initial Catalog = educafinEntregas; Integrated Security = True");
        conn.Open();
        return conn;
    }


}