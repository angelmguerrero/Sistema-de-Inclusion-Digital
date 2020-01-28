//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Data.SqlClient;
//using PRUEBA1;

//namespace Educafin.Models
//{
//    public class AutocomEntreInt
//    {

//        public List<MvcApplication8.Models.Institucion> lista(string pRFC, string pRFC2)
//        {
//            List<MvcApplication8.Models.Institucion> lis = new List<MvcApplication8.Models.Institucion>();
//            bool media = false;
//            string dependecia = "";
//            string cct = "";
//            using (SqlConnection cone = PRUEBA1.proveedor.conexion())
//            {
//                SqlCommand comando = new SqlCommand(string.Format(
//               "select cct,[medioprovedor],[dependencia] from institucion where CCT = '{0}'", pRFC2), cone);
//                SqlDataReader reader = comando.ExecuteReader();
//                while (reader.Read())
//                {
//                    cct = reader.GetString(0);
//                    media = reader.GetBoolean(1);
//                    if (reader.GetValue(2) != null)
//                    {
//                        dependecia = reader.GetValue(2).ToString();
//                    }
//                }
//                cone.Close();
//            }
//            if (media == true)
//            {
//                if (!dependecia.Equals(""))
//                {
//                    using (SqlConnection cone = PRUEBA1.proveedor.conexion())
//                    {
//                        SqlCommand comando = new SqlCommand(string.Format(
//                       "select CCT, nombre from institucion where dependencia like '{0}%' and cct='{2}'", pRFC, cct, dependecia), cone);
//                        SqlDataReader reader = comando.ExecuteReader();
//                        while (reader.Read())
//                        {
//                            MvcApplication8.Models.Institucion mde1 = new MvcApplication8.Models.Institucion();
//                            mde1.nombre = reader.GetString(1);
//                            mde1.CCT = reader.GetString(0);
//                            lis.Add(mde1);
//                        }
//                        cone.Close();
//                    }
//                }
//            }
//            else
//            {
//                if (!dependecia.Equals(""))
//                {
//                    using (SqlConnection cone = PRUEBA1.proveedor.conexion())
//                    {
//                        SqlCommand comando = new SqlCommand(string.Format(
//                       "select CCT, nombre from institucion where dependencia like '{0}%' and cct='{2}'", pRFC, cct, dependecia), cone);
//                        SqlDataReader reader = comando.ExecuteReader();
//                        while (reader.Read())
//                        {
//                            MvcApplication8.Models.Institucion mde1 = new MvcApplication8.Models.Institucion();
//                            mde1.nombre = reader.GetString(1);
//                            mde1.CCT = reader.GetString(0);
//                            lis.Add(mde1);
//                        }
//                        cone.Close();
//                    }
//                }

//            }

//            using (SqlConnection cone = PRUEBA1.proveedor.conexion())
//            {
//                SqlCommand comando = new SqlCommand(string.Format(
//               "select distinct i.nombre,p.rfc_prov from Proveedor_Institucion p, proveedor i where rfc_prov like '{0}%' and rfc_prov = RFC ", pRFC), cone);
//                SqlDataReader reader = comando.ExecuteReader();
//                while (reader.Read())
//                {
//                    MvcApplication8.Models.Institucion mde = new MvcApplication8.Models.Institucion();
//                    mde.nombre = reader.GetString(0);
//                    mde.CCT = reader.GetString(1);
//                    lis.Add(mde);
//                }
//                cone.Close();
//            }
//            return lis;
//        }

//        public List<MvcApplication8.Models.Institucion> autocomentrega(string cct_int)
//        {
//            List<MvcApplication8.Models.Institucion> lis = new List<MvcApplication8.Models.Institucion>();
//            using (SqlConnection cone = PRUEBA1.proveedor.conexion())
//            {
//                SqlCommand comando = new SqlCommand(string.Format(
//               "select i.nombre,p.cct_int from Proveedor_Institucion p, institucion i where cct_int like '{0}%' and cct_int = CCT", cct_int), cone);
//                SqlDataReader reader = comando.ExecuteReader();
//                while (reader.Read())
//                {
//                    MvcApplication8.Models.Institucion mde = new MvcApplication8.Models.Institucion();
//                    mde.nombre = reader.GetString(0);
//                    mde.CCT = reader.GetString(1);
//                    mde.direccion = reader.GetString(0);
//                    lis.Add(mde);
//                }
//                cone.Close();
//            }
//            return lis;
//        }

//        public List<MvcApplication8.Models.Institucion> autocomentrmp(string cct_int1,string pRFC)
//        {
//            List<MvcApplication8.Models.Institucion> lis = new List<MvcApplication8.Models.Institucion>();
//            using (SqlConnection cone = PRUEBA1.proveedor.conexion())
//            {
//                SqlCommand comando = new SqlCommand(string.Format(
//               "select CCT,nombre from institucion where dependencia ='{0}' and CCT like '{1}%'",pRFC, cct_int1), cone);
//                SqlDataReader reader = comando.ExecuteReader();
//                while (reader.Read())
//                {
//                    MvcApplication8.Models.Institucion mde = new MvcApplication8.Models.Institucion();
//                    mde.nombre = reader.GetString(1);
//                    mde.CCT = reader.GetString(0);
//                    lis.Add(mde);
//                }
//                cone.Close();
//            }
//            return lis;
//        }

//        public List<MvcApplication8.Models.Institucion> autocomElm(string pRFC, string cct_intELIM)
//        {
//            List<MvcApplication8.Models.Institucion> lis = new List<MvcApplication8.Models.Institucion>();
//            using (SqlConnection cone = PRUEBA1.proveedor.conexion())
//            {
//                SqlCommand comando = new SqlCommand(string.Format(
//                "IF(SELECT distinct ESTATUS FROM [ tablet] WHERE medioprovedor = '" + pRFC + "' AND ESTATUS = 'M') ='M' " +
//                "SELECT distinct cct,nombre FROM [ tablet] t, institucion i WHERE t.ESTATUS ='M' and i.CCT = t.intitucion and cct like '{0}%'" +
//                "ELSE IF (SELECT distinct ESTATUS FROM [ tablet] WHERE proveedor = '" + pRFC + "' AND ESTATUS = 'P')='P' " +
//                "SELECT distinct cct,nombre FROM [ tablet] t, institucion i WHERE t.ESTATUS ='P' and (i.CCT = t.medioprovedor or CCT =t.intitucion) and cct like '{0}%'", cct_intELIM), cone);
//                SqlDataReader reader = comando.ExecuteReader();
//                while (reader.Read())
//                {
//                    MvcApplication8.Models.Institucion mde = new MvcApplication8.Models.Institucion();
//                    mde.nombre = reader.GetString(0);
//                    mde.CCT = reader.GetString(1);
//                    mde.direccion = reader.GetString(0);
//                    lis.Add(mde);
//                }
//                cone.Close();
//            }
//            return lis;
//        }
//    }
//}