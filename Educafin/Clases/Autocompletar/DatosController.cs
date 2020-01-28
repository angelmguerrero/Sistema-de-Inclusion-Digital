using Educafin.Clases;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace Educafin.Controllers
{
    public class DatosController : Controller
    {
        //Obtener datos del curp
        [HttpPost]
        public ActionResult DatosUsuario(string curp)
        {
            //string curp = "CXGO920612HGTBDS07";
            CURP ws = new CURP();

            string[] str = ws.get_Curp(curp.ToString().ToUpper()).Split(',');

            var json = (dynamic)null;

            if (str.Length >= 7)
            {

                json = new {
                    curp = str[0],
                    ape_pat = str[1],
                    ape_mat = str[2],
                    nombre = str[3],
                    sexo = str[4],
                    fecha_nac = str[5],
                    lugar_nac = str[6]
                };


            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        //datos de entrega
        public JsonResult autocompletadoent(string cct_int)
        {
            Educafin.Models.AutocomEntreInt dato = new Educafin.Models.AutocomEntreInt();
            return Json(dato.autocomentrega(cct_int), JsonRequestBehavior.AllowGet);
        }
        //datos de entregaMP
        public JsonResult autocompletadoentMP(string cct_int1)
        {
            string pRFC = (string)Session["pertenece"];
            Educafin.Models.AutocomEntreInt dato = new Educafin.Models.AutocomEntreInt();
            return Json(dato.autocomentrmp(cct_int1,pRFC), JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult autocompletadoentInt(string pRFC)
        {
            string pRFC2 = (string)Session["pertenece"];
            Educafin.Models.AutocomEntreInt dato = new Educafin.Models.AutocomEntreInt();
            return Json(dato.lista(pRFC,pRFC2), JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult autocompletadoentELIM(string cct_intELIM)
        {
            string pRFC = (string)Session["pertenece"];
            Educafin.Models.AutocomEntreInt dato = new Educafin.Models.AutocomEntreInt();
            return Json(dato.autocomElm(pRFC,cct_intELIM), JsonRequestBehavior.AllowGet);
        }
        
        //datos del proveedor
        public JsonResult autoProveedor(string rfc)
        {
            List<object> list = new List<object>();

            using (SqlConnection cone = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select RFC, nombre from proveedor where RFC like '{0}%'",rfc), cone);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    var item = new
                    {
                        clave = reader.GetString(0),
                        nombre = reader.GetString(1)
                    };

                    list.Add(item);
                }
                cone.Close();
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        
        //datos de instituciónes
        public JsonResult autoInstitucion(string cct)
        {
            List<object> list = new List<object>();

            using (SqlConnection cone = Conexion.conexion())
            {
                //SqlCommand comando = new SqlCommand(string.Format(
                //"select CCT, nombre from institucion where CCT like '{0}%'",cct), cone);

                SqlCommand comando = new SqlCommand(string.Format(
                "select CCT, nombre from institucion where CCT like '{0}%'", cct), cone);

                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    var item = new
                    {
                        clave = reader.GetString(0),
                        nombre = reader.GetString(1)
                    };

                    list.Add(item);
                }
                cone.Close();
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        //datos de instituciónes para asociar proveedor
        public JsonResult autoInstitucionP(string cct)
        {
            List<object> list = new List<object>();

            using (SqlConnection cone = Conexion.conexion())
            {
                //SqlCommand comando = new SqlCommand(string.Format(
                //"select CCT, nombre from institucion where CCT like '{0}%'",cct), cone);

                SqlCommand comando = 
                    new SqlCommand(string.Format
                    ("select CCT, nombre from institucion where CCT not  in (select cct_int from Proveedor_Institucion )  and(dependencia is null or dependencia = CCT and CCT like '{0}%')", cct), cone);




                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    var item = new
                    {
                        clave = reader.GetString(0),
                        nombre = reader.GetString(1)
                    };

                    list.Add(item);
                }
                cone.Close();
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        //tabletas no asignadas
        public JsonResult autoTabletInstitución(string serie) {
            List<object> list = new List<object>();

            using (SqlConnection cone = Conexion.conexion())
            {
                string cct = "";
                if ((string)Session["tipo"] == "A")
                {
                    cct = (string)Session["cctTemp"];
                }
                else
                {
                    cct = (string)Session["pertenece"];
                }

                SqlCommand comando = new SqlCommand(string.Format(
                "select * from tablet where intitucion ='{1}' and beneficiario is null and serie like '{0}%' and estatus = 'I'", serie, cct), cone);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    var item = new{
                        serie = reader.GetString(0)
                    };

                    list.Add(item);
                }
                cone.Close();
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //datos beneficiario por curp
        public JsonResult autoCurpAlumnos(string curp)
        {
            List<object> list = new List<object>();

            using (SqlConnection cone = Conexion.conexion())
            {
                string cct = "";
                if ((string)Session["tipo"] == "A")
                {
                    cct = (string)Session["cctTemp"];
                }
                else
                {
                    cct = (string)Session["pertenece"];
                }

                SqlCommand comando = new SqlCommand(string.Format(
                //"select ib.CURP, ib.matricula, u.apellido_pat, u.apellido_mat, u.nombre from institucion_beneficiario ib join usuario u on u.CURP = ib.CURP where ib.CCT = '{0}' and ib.estatus = 'A' and ib.CURP like '{1}%' and ib.CURP NOT IN (select t.beneficiario from tablet t where t.intitucion = '{0}' and t.beneficiario is not null) "
                // -- Segundo Cambio
                //"select ib.CURP, ib.matricula, u.apellido_pat, u.apellido_mat, u.nombre " +
                //"from institucion_beneficiario ib join persona u on u.CURP = ib.CURP " +
                //"where ib.CCT = '{0}' and ib.estatus = 'A' and ib.CURP like '{1}%' and ib.CURP NOT IN " +
                //"(select t.beneficiario from tablet t where t.intitucion = '{0}' and t.beneficiario is not null)"
                // -- Tercer cambio si no ha registrado sus datos el beneficiario
                "select ib.CURP, ib.matricula, u.apellido_pat, u.apellido_mat, u.nombre " +
                "from institucion_beneficiario ib "+ 
                "join persona u on u.CURP = ib.CURP " +
                "where ib.CCT = '{0}' and ib.estatus = 'A' and ib.CURP like '{1}%' " +
                "AND codigopostal IS NOT NULL " +
                "AND estado_r IS NOT NULL " +
                "AND municipio IS NOT NULL " +
                "AND colonia IS NOT NULL " +
                "AND calle IS NOT NULL " +
                "AND num_int IS NOT NULL " +
                "AND num_ext IS NOT NULL " +
                "AND telefono IS NOT NULL " +
                "AND celular IS NOT NULL " +
                "AND correo IS NOT NULL " +
                "and ib.CURP NOT IN " +
                "(select t.beneficiario from tablet t where t.intitucion = '{0}' and t.beneficiario is not null)"  , cct, curp), cone);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    var item = new
                    {
                        curp = reader.GetString(0),
                        matricula= reader.GetString(1),
                        pat = reader.GetString(2),
                        mat = reader.GetString(3),
                        nombre = reader.GetString(4)
                    };

                    list.Add(item);
                }
                cone.Close();

            System.Diagnostics.Debug.WriteLine(string.Format(
            //"select ib.CURP, ib.matricula, u.apellido_pat, u.apellido_mat, u.nombre from institucion_beneficiario ib join usuario u on u.CURP = ib.CURP where ib.CCT = '{0}' and ib.estatus = 'A' and ib.CURP like '{1}%' and ib.CURP NOT IN (select t.beneficiario from tablet t where t.intitucion = '{0}' and t.beneficiario is not null) "
            "select ib.CURP, ib.matricula, u.apellido_pat, u.apellido_mat, u.nombre " +
            "from institucion_beneficiario ib join persona u on u.CURP = ib.CURP " +
            "where ib.CCT = '{0}' and ib.estatus = 'A' and ib.CURP like '{1}%' and ib.CURP NOT IN " +
            "(select t.beneficiario from tablet t where t.intitucion = '{0}' and t.beneficiario is not null)"
            , cct, curp));
            }

        
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //datos beneficiario por matricula
        public JsonResult autoMatriculaAlumnos(string matricula)
        {
            List<object> list = new List<object>();

            using (SqlConnection cone = Conexion.conexion())
            {
                string cct = "";
                if ((string)Session["tipo"] == "A")
                {
                    cct = (string)Session["cctTemp"];
                }
                else
                {
                    cct = (string)Session["pertenece"];
                }
                SqlCommand comando = new SqlCommand(string.Format(
                "select ib.CURP, ib.matricula, u.apellido_pat, u.apellido_mat, u.nombre from institucion_beneficiario ib join usuario u on u.CURP = ib.CURP where ib.CCT = '{0}' and ib.estatus = 'A' and ib.matricula like '{1}%' and ib.CURP NOT IN (select t.beneficiario from tablet t where t.intitucion = '{0}' and t.beneficiario is not null) ", cct, matricula), cone);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    var item = new
                    {
                        curp = reader.GetString(0),
                        matricula = reader.GetString(1),
                        pat = reader.GetString(2),
                        mat = reader.GetString(3),
                        nombre = reader.GetString(4)
                    };

                    list.Add(item);
                }
                cone.Close();
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //datos de institucion para medio proveedor
        public JsonResult autoMedioProveedor(string txt, string MP)
        {
            List<object> list = new List<object>();

            using (SqlConnection cone = Conexion.conexion())
            {
                
                SqlCommand comando = new SqlCommand(string.Format(
                "select CCT,nombre from institucion where dependencia is null OR dependencia = '' AND CCT not in (select cct_int from proveedor_institucion ip where ip.cct_int <> '{1}' ) and CCT like '{0}%'", txt, MP), cone);
                System.Diagnostics.Debug.WriteLine(comando);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    var item = new
                    {
                        cct = reader.GetString(0),
                        nombre = reader.GetString(1)
                    };

                    list.Add(item);
                }
                cone.Close();
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }





    }
}