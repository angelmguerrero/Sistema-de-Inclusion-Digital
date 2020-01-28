//using Educafin.Models;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Mvc;
//using System.Net.Mail;
//using System.Collections;
//using Microsoft.Reporting.WebForms;
//using Educafin.Clases;
//using System.Data.SqlClient;
//using Educafin.Clases.Transacciones;

//namespace Educafin.Controllers
//{
//    public class BeneficiarioController : Controller
//    {


//        // GET: Beneficiario
//        public ActionResult Index()
//        {
//            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
//            return View();
//        }

//        public ActionResult Insertar()
//        {
//            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
//            return View();
//        }


//        public ActionResult Registrado(FormCollection form)
//        {
//            var correo1 = form["Email"];
//            var correo2 = form["EmailV"];
//            var curp = form["curp"];

//            string result = "";
//            string error = "";

//            if (correo1 == correo2)
//            {
//                Hashtable consulta = buscaCurp(curp);
//                if (consulta["found"].ToString().Equals("true"))
//                {
//                    if (consulta["estatus"].ToString().Equals("E"))
//                    {
//                        result += enviarCorreo(correo1, curp);
//                        error = "alert-success";
//                    }
//                    else
//                    {
//                        error = "alert-danger";
//                        result += "La curp ya tiene un correo";
//                    }
//                }
//                else
//                {
//                    error = "alert-danger";
//                    result += "CURP no existente en el sistema";
//                }
//            }
//            else
//            {
//                error = "alert-danger";
//                result += "Los correos no coinciden";
//            }
//            TempData["error"] = error;
//            TempData["message"] = result;

//            return View();
//        }



//        private string enviarCorreo(string corre, string curp)
//        {

//            string password = Metodos.generatePassword(6);
//            string contenido = "";
//            contenido += "<center>";

//            contenido += "<h4>";
//            contenido += "Bienvenido al programa de inclusión digital, su curp:<br>";
//            contenido += "<b>" + curp + "</b><br>";
//            contenido += "Ha sido registrada con este correo.< br>";
//            contenido += "Para countinuar, acceda en la siguiente página:<br><br>";
//            contenido += "<a href='http://tabletasube.itleon.edu.mx/'  target='_blank'>http://tabletasube.itleon.edu.mx/</a><br>";
//            contenido += "<br>con la siguiente contraseña: ";
//            contenido += "<strong>";
//            contenido += password;
//            contenido += "</strong></h4>";
//            contenido += "</center>";

//            Conexion con = new Conexion();
//            string campos = "password='" + password + "', estatus ='E', correo='" + corre + "'";
//            string condicion = "curp = '" + curp + "'";


//            password = password.Replace("'", "");
//            curp = curp.Replace("'", "");
//            corre = corre.Replace("'", "");

//            objBenefTrans.beneficiarioContraseña(curp, corre, password);

//            if (true)//con.actualizar2("Usuario", campos, condicion)
//            {
//                ////("Se generó la contraseña correctamente");
//                TempData["error"] = "alert-success";
//            }
//            else
//            {
//                return "Algo extraño sucedió y no pudimos generar tu contraseña, vuelva a intentarlo mas tarde";
//                TempData["error"] = "alert-danger";
//            }

//            //Metodos.sendMail(contenido, corre, subject)
//            if (true)
//            {
//                return "Se ha enviado un correo con su contraseña, el correo puede tardar en llegar hasta 12 horas despues del registro";
//                TempData["error"] = "alert-success";
//            }
//            else
//            {
//                return "Se ha enviado un correo con su contraseña, el correo puede tardar en llegar hasta 12 horas despues del registro";
//                TempData["error"] = "alert-danger";
//            }
//        }

//        [AllowAnonymous]
//        public ActionResult Registro(string returnUrl)
//        {
//            ViewBag.ReturnUrl = returnUrl;
//            return View();
//        }




//        //public ActionResult MisDatos(FormCollection form = null)
//        //{
//        //    if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }        
//        //    string curp = Session["CURP"].ToString();

//        //    if (form.HasKeys())
//        //    {
//        //        //se va a guardar
//        //        var telCas = form["telefonoc"];
//        //        var telCel = form["telefonocl"];
//        //        var cp = form["codigopostal"];
//        //        var edo = form["estado"];
//        //        var muni = form["municipio"];
//        //        var col = form["colonia"];
//        //        var calle = form["calle"];
//        //        var numI = form["numExt"];
//        //        var numE = form["numInt"];
//        //        var tutor = form["tutor"];
//        //        var credencial = form["16240840"];
//        //        System.Diagnostics.Debug.WriteLine("---> Credencial " + credencial);
//        //        if (String.IsNullOrEmpty(credencial))
//        //        {
//        //            credencial = "0";
//        //        }
//        //        else
//        //        {
//        //            credencial = "1";
//        //        }

//        //        if (updDatos(curp, edo, muni, col, calle, numI, numE, telCas, telCel, cp, tutor, credencial))
//        //        {
//        //            //bien
//        //            TempData["error"] = "alert-success";
//        //            TempData["message"] = "Datos actualizados correctamente";
//        //        }
//        //        else
//        //        {
//        //            //mal
//        //            TempData["error"] = "alert-danger";
//        //            TempData["message"] = "Error al actualizar los datos";
//        //        }

//        //    }
//        //    else
//        //    {
//        //        //primera vez
//        //    }

//        //    Hashtable datos = buscaCurp(curp);
//        //    if (datos["found"].ToString() == "true")
//        //    {
//        //        ViewData["nombre"] = datos["nombre"].ToString();
//        //        ViewData["apellido_pat"] = datos["apellido_pat"].ToString();
//        //        ViewData["apellido_mat"] = datos["apellido_mat"].ToString();
//        //        ViewData["estado_r"] = datos["estado_r"].ToString();
//        //        ViewData["municipio"] = datos["municipio"].ToString();
//        //        ViewData["colonia"] = datos["colonia"].ToString();
//        //        ViewData["calle"] = datos["calle"].ToString();
//        //        ViewData["num_int"] = datos["num_int"].ToString();
//        //        ViewData["num_ext"] = datos["num_ext"].ToString();
//        //        ViewData["telefono"] = datos["telefono"].ToString();
//        //        ViewData["celular"] = datos["celular"].ToString();
//        //        ViewData["correo"] = datos["correo"].ToString();
//        //        ViewData["codigopostal"] = datos["codigopostal"].ToString();
//        //        ViewData["tutor"] = datos["tutor"].ToString();

//        //        if (String.IsNullOrEmpty(datos["credencial"].ToString()))
//        //        {
//        //            ViewData["credencial"] = "0";
//        //        }
//        //        else
//        //        {
//        //            if (datos["credencial"].Equals("True"))
//        //            {
//        //                ViewData["credencial"] = "1";
//        //            }
//        //            else
//        //            {
//        //                ViewData["credencial"] = "0";
//        //            }
//        //        }

//        //        string date = datos["fecha_nac"].ToString();

//        //        int dia = Convert.ToInt32(date.Substring(0, 2));
//        //        int mes = Convert.ToInt32(date.Substring(3, 2));
//        //        int ano = Convert.ToInt32(date.Substring(6, 4));

//        //        DateTime oldDate = new DateTime(ano, mes, dia);
//        //        DateTime newDate = DateTime.Now;
//        //        // Difference in days, hours, and minutes.
//        //        TimeSpan ts = newDate - oldDate;
//        //        // Difference in days.
//        //        int differenceInDays = ts.Days;
//        //        int anos = Convert.ToInt32(Math.Floor(differenceInDays / 365.25));
//        //        ViewData["edad"] = anos.ToString();
//        //    }

//        //    return View();

//        //}



//        public ActionResult Acuses()
//        {
//            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
//            return View();
//        }

//        protected Hashtable buscaCurp(String curp)
//        {
//            Conexion con = new Conexion();
//            Hashtable consulta = new Hashtable();

//            if (con.consulta(objBenefTrans.consultaBeneficiarioEInstitucion(curp)).Rows.Count > 0)
//            {
//                foreach (DataRow dbRow in con.consulta(objBenefTrans.consultaBeneficiarioEInstitucion(curp)).Rows)
//                {
//                    consulta["nombre"] = objBenefTrans.getNombre();
//                    consulta["apellido_pat"] = objBenefTrans.getApePat();
//                    consulta["apellido_mat"] = objBenefTrans.getApeMat();
//                    consulta["fecha_nac"] = objBenefTrans.getFechaNac();
//                    consulta["estado_r"] = objBenefTrans.getEdoRec();
//                    consulta["municipio"] = objBenefTrans.getMunicipio();
//                    consulta["colonia"] = objBenefTrans.getColonia();
//                    consulta["calle"] = objBenefTrans.getCalle();
//                    consulta["num_int"] = objBenefTrans.getNumInt();
//                    consulta["num_ext"] = objBenefTrans.getNumExt();
//                    consulta["telefono"] = objBenefTrans.getTelefono();
//                    consulta["correo"] = objBenefTrans.getCorreo();
//                    consulta["codigopostal"] = objBenefTrans.getCodigoPostal();
//                    consulta["celular"] = objBenefTrans.getCelular();
//                    consulta["tutor"] = objBenefTrans.getTutor();
//                    consulta["credencial"] = objBenefTrans.getCredencial();
//                    consulta["estatus"] = objBenefTrans.getEstatus();

//                    consulta["found"] = "true";
//                }

//                System.Diagnostics.Debug.WriteLine(consulta["nombre"].ToString());
//                System.Diagnostics.Debug.WriteLine(consulta["apellido_pat"].ToString());
//                System.Diagnostics.Debug.WriteLine(consulta["apellido_mat"].ToString());
//                System.Diagnostics.Debug.WriteLine(consulta["fecha_nac"].ToString());
//                System.Diagnostics.Debug.WriteLine(consulta["estado_r"].ToString());
//                System.Diagnostics.Debug.WriteLine(consulta["municipio"].ToString());
//                System.Diagnostics.Debug.WriteLine(consulta["colonia"].ToString());
//                System.Diagnostics.Debug.WriteLine(consulta["calle"].ToString());
//                System.Diagnostics.Debug.WriteLine(consulta["num_int"].ToString());
//                System.Diagnostics.Debug.WriteLine(consulta["num_ext"].ToString());
//                System.Diagnostics.Debug.WriteLine(consulta["telefono"].ToString());
//                System.Diagnostics.Debug.WriteLine(consulta["correo"].ToString());
//                System.Diagnostics.Debug.WriteLine(consulta["codigopostal"].ToString());
//                System.Diagnostics.Debug.WriteLine(consulta["celular"].ToString());
//                System.Diagnostics.Debug.WriteLine(consulta["tutor"].ToString());
//                System.Diagnostics.Debug.WriteLine(consulta["credencial"].ToString());
//                System.Diagnostics.Debug.WriteLine(consulta["estatus"].ToString());
//            }
//            else
//            {
//                consulta["found"] = "false";
//            }
//            return consulta;
//        }

//        protected Boolean updDatos(String curp, String Edo, String Muni,
//            String col, String calle, String numint, String numext,
//            String tel, String telce, String cp, String tut, String credencial)
//        {
//            Conexion con = new Conexion();

//            string ad =
//            "estado_r='" + Edo +
//            "',municipio='" + Muni +
//            "',colonia='" + col +
//            "',calle='" + calle +
//            "',num_int='" + numint +
//            "',num_ext='" + numext +
//            "',codigopostal='" + cp +
//            "',telefono='" + tel +
//            "',celular='" + telce + "'";


//            if (con.actualizar("Usuario", ad, "curp = '" + curp + "'"))
//            {
//                ad =
//                    "tutor='" + tut +
//                    "',credencial='" + credencial + "'";
//                if (con.actualizar("Institucion_beneficiario", ad, "curp = '" + curp + "'"))
//                {
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public ActionResult GeneraReporte()
//        {
//            ReportViewer reportViewer = new ReportViewer();
//            Conexion con = new Conexion();
//            Warning[] warnings;
//            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
//            if (con.consulta(objBenefTrans.consultaDomicilio(Session["CURP"].ToString())).Rows.Count > 0)
//            {
//                foreach (DataRow dbRow in con.consulta(objBenefTrans.consultaDomicilio(Session["CURP"].ToString())).Rows)
//                {
//                    if (string.IsNullOrEmpty(dbRow["codigopostal"].ToString()) && string.IsNullOrEmpty(dbRow["calle"].ToString()) &&
//                        string.IsNullOrEmpty(dbRow["Credencial"].ToString()))
//                    {
//                        TempData["error"] = "alert alert-danger";
//                        TempData["message"] = "Por Favor llena tus datos antes de descargar acuse";
//                        return RedirectToAction("MisDatos", "Beneficiario");
//                    }
//                }
//            }
//            else
//            {
//                TempData["error"] = "alert alert-danger";
//                TempData["message"] = "Error al consultar sus Datos";
//                return RedirectToAction("Index", "Home");
//            }

//            string[] streamIds;
//            string fecha_rec = DateTime.Now.ToString("MM-dd-yy");
//            string dependencia = "";
//            string iden = "";
//            string mimeType = string.Empty;
//            string encoding = string.Empty;
//            string extension = string.Empty;
//            Hashtable datos = buscaCurp(Session["CURP"].ToString());
//            if (con.consulta(objBenefTrans.consultaCredencial(Session["CURP"].ToString())).Rows.Count > 0)
//            {
//                foreach (DataRow dbRow in con.consulta(objBenefTrans.consultaCredencial(Session["CURP"].ToString())).Rows)
//                {
//                    iden = dbRow["Credencial"].ToString();
//                }
//            }
//            else
//            {
//                TempData["error"] = "alert alert-danger";
//                TempData["message"] = "Error al consultar sus Datos";
//                return RedirectToAction("MisDatos", "Beneficiario");
//            }
//            reportViewer.ProcessingMode = ProcessingMode.Local;
//            if (iden == "False")
//            {
//                if (con.consulta(objBenefTrans.consultaDatosInstitucion(Session["CURP"].ToString())).Rows.Count > 0)
//                {
//                    foreach (DataRow dbRow in con.consulta(objBenefTrans.consultaDatosInstitucion(Session["CURP"].ToString())).Rows)
//                    {
//                        dependencia = dbRow["nombre"].ToString();
//                        reportViewer.LocalReport.ReportPath = Server.MapPath(@"~\Reportes\ReporteAMNMD.rdlc");
//                    }
//                }
//                else
//                {
//                    reportViewer.LocalReport.ReportPath = Server.MapPath(@"~\Reportes\ReporteAMN.rdlc");
//                }

//            }
//            else if (iden == "True")
//            {
//                if (con.consulta(objBenefTrans.consultaDatosInstitucion(Session["CURP"].ToString())).Rows.Count > 0)
//                {
//                    foreach (DataRow dbRow in con.consulta(objBenefTrans.consultaDatosInstitucion(Session["CURP"].ToString())).Rows)
//                    {
//                        dependencia = dbRow["nombre"].ToString();
//                        //(iden);
//                        reportViewer.LocalReport.ReportPath = Server.MapPath(@"~\Reportes\ReporteAMYMD.rdlc");
//                    }
//                }
//                else
//                {
//                    //(iden);
//                    reportViewer.LocalReport.ReportPath = Server.MapPath(@"~\Reportes\ReporteAMY.rdlc");
//                }
//            }
//            else
//            {
//                TempData["error"] = "alert alert-danger";
//                TempData["message"] = "Error al consultar sus Identificacion " + iden;
//                return RedirectToAction("MisDatos", "Beneficiario");
//            }

//            try
//            {
//                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsUsuario",
//                con.consulta(objBenefTrans.consultaDatosBeneficiario(Session["CURP"].ToString()))));
//            }
//            catch (EntitySqlException e)
//            {

//            }
//            try
//            {
//                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsInstBen",
//                    con.consulta(objBenefTrans.consultaBeneficiarioActivo(Session["CURP"].ToString()))));
//            }
//            catch (EntitySqlException e)
//            {

//            }
//            try
//            {
//                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsTablet",
//                    con.consulta(Session["CURP"].ToString())));
//            }
//            catch (EntitySqlException e)
//            {

//            }
//            try
//            {
//                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsInstitucion",
//                    con.consulta(objBenefTrans.consultaInstitucionDelBeneficiario(Session["CURP"].ToString()))));
//            }
//            catch (EntitySqlException e)
//            {

//            }

//            ReportParameter[] parameters = new ReportParameter[3];
//            parameters[0] = new ReportParameter("curp", Session["CURP"].ToString());
//            parameters[1] = new ReportParameter("fecha", fecha_rec);
//            parameters[2] = new ReportParameter("dependencia", dependencia);
//            reportViewer.LocalReport.SetParameters(parameters);
//            reportViewer.LocalReport.Refresh();
//            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
//            Response.Buffer = true;
//            Response.Clear();
//            Response.ContentType = mimeType;
//            Response.AddHeader("content-disposition", "attachment; filename=" + Session["CURP"].ToString() + "." + extension);
//            Response.BinaryWrite(bytes);
//            Response.Flush();

//            ViewBag.ReportViewer = reportViewer;
//            return View();
//        }

//        protected void pdf(String plantilla, String nombre_al, String correo, String domicilio,
//                                    String colonia, String municipio)
//        {
//            Response.Clear();
//            Response.ContentType = "application/pdf";
//            Response.AddHeader("content-disposition", "attachment;filename=Formulario.pdf");
//            DatosAlumnoAcuse pr = new DatosAlumnoAcuse(nombre_al, correo, domicilio, colonia, municipio);
//            pr.FillPDFBenMay(Server.MapPath(plantilla), Response.OutputStream);
//        }

//        protected int calcEdad(String fecnac)
//        {
//            int edad = 18;
//            string date = fecnac;

//            int dia = Convert.ToInt32(date.Substring(0, 2));
//            int mes = Convert.ToInt32(date.Substring(3, 2));
//            int ano = Convert.ToInt32(date.Substring(6, 4));

//            DateTime oldDate = new DateTime(ano, mes, dia);
//            DateTime newDate = DateTime.Now;
//            // Difference in days, hours, and minutes.
//            TimeSpan ts = newDate - oldDate;
//            // Difference in days.
//            int differenceInDays = ts.Days;
//            edad = Convert.ToInt32(Math.Floor(differenceInDays / 365.25));

//            return edad;
//        }
//    }
//}








using Educafin.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Collections;
using Microsoft.Reporting.WebForms;
using Educafin.Clases;
using System.Data.SqlClient;
using Educafin.Clases.Transacciones;

namespace Educafin.Controllers
{
    public class BeneficiarioController : Controller
    {

        // GET: Beneficiario
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            return View();
        }

        public ActionResult Insertar()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            return View();
        }


        public ActionResult Registrado(FormCollection form)
        {
            var correo1 = form["Email"];
            var correo2 = form["EmailV"];
            var curp = form["curp"];

            string result = "";
            string error = "";

            if (correo1 == correo2)
            {
                Hashtable consulta = buscaCurp(curp);

      
                System.Diagnostics.Debug.WriteLine(consulta["found"].ToString());
                System.Diagnostics.Debug.WriteLine(consulta["found"].ToString());
                if (consulta["found"].ToString().Equals("true"))
                {

                    //if (consulta["correo"].ToString().Equals("true"))
                    if (consulta["estatus"].ToString().Equals("E"))
                    {
                        result += enviarCorreo(correo1, curp);
                        error = "alert-success";

                    }
                    else
                    {
                        error = "alert-danger";
                        result += "La curp ya tiene un correo";
                    }

                }
                else
                {
                    error = "alert-danger";
                    result += "CURP no existente en el sistema";
                }

            }
            else
            {
                error = "alert-danger";
                result += "Los correos no coinciden";
            }


            TempData["error"] = error;
            TempData["message"] = result;

            return View();
        }



        private string enviarCorreo(string corre, string curp)
        {

            string password = Metodos.generatePassword(6);
            string contenido = "";
            contenido += "<center>";

            contenido += "<h4>";
            contenido += "Bienvenido al programa de inclusión digital, su curp:<br>";
            contenido += "<b>" + curp + "</b><br>";
            contenido += "Ha sido registrada con este correo.< br>";
            contenido += "Para countinuar, acceda en la siguiente página:<br><br>";
            contenido += "<a href='http://tabletasube.itleon.edu.mx/'  target='_blank'>http://tabletasube.itleon.edu.mx/</a><br>";
            contenido += "<br>con la siguiente contraseña: ";
            contenido += "<strong>";
            contenido += password;
            contenido += "</strong></h4>";
            contenido += "</center>";

            Conexion con = new Conexion();
            string campos = "password='" + password + "', estatus ='E', correo='" + corre + "'";
            string condicion = "curp = '" + curp + "'";


            password = password.Replace("'", "");
            curp = curp.Replace("'", "");
            corre = corre.Replace("'", "");

            BeneficiarioTransacciones objBenefTrans = new BeneficiarioTransacciones();
            objBenefTrans.beneficiarioContraseña(curp, corre, password);

            if (true)//con.actualizar2("Usuario", campos, condicion)
            {
                ////("Se generó la contraseña correctamente");
                TempData["error"] = "alert-success";
            }
            else
            {
                return "Algo extraño sucedió y no pudimos generar tu contraseña, vuelva a intentarlo mas tarde";
                TempData["error"] = "alert-danger";
            }

            //Metodos.sendMail(contenido, corre, subject)
            if (true)
            {
                return "Se ha enviado un correo con su contraseña, el correo puede tardar en llegar hasta 12 horas despues del registro";
                TempData["error"] = "alert-success";
            }
            else
            {
                return "Se ha enviado un correo con su contraseña, el correo puede tardar en llegar hasta 12 horas despues del registro";
                TempData["error"] = "alert-danger";
            }
        }








        //private string enviarCorreo(string corre, string curp)
        //{


        //    string password = Metodos.generatePassword(6);
        //    string subject = "Envio contraseña";
        //    string contenido = "";
        //    contenido += "<center>";

        //    contenido += "<h4>";
        //    contenido += "Bienvenido al programa de inclusión digital, su curp:<br>";
        //    contenido += "<b>" + curp + "</b><br>";
        //    contenido += "Ha sido registrada con este correo.< br>";
        //    contenido += "Para countinuar, acceda en la siguiente página:<br><br>";
        //    contenido += "<a href='http://tabletasube.itleon.edu.mx/'  target='_blank'>http://tabletasube.itleon.edu.mx/</a><br>";
        //    contenido += "<br>con la siguiente contraseña: ";
        //    contenido += "<strong>";
        //    contenido += password;
        //    contenido += "</strong></h4>";
        //    contenido += "</center>";

        //    Conexion con = new Conexion();
        //    string campos = "password='" + password + "', estatus ='E', correo='" + corre + "'";
        //    string condicion = "curp = '" + curp + "'";


        //    password = password.Replace("'", "");
        //    curp = curp.Replace("'", "");
        //    corre = corre.Replace("'", "");


        //    using (SqlConnection conex = Conexion.conexion())
        //    {
        //        SqlCommand comando = new SqlCommand("UPDATE usuario SET password=@pass, correo=@correo2, estatus='E' where CURP=@curp2;", conex);
        //        comando.Parameters.AddWithValue("@pass", password.ToString());
        //        comando.Parameters.AddWithValue("@curp2", curp.ToString());
        //        comando.Parameters.AddWithValue("@correo2", corre.ToString());

        //        SqlDataReader read = comando.ExecuteReader();

        //        read.Close();
        //        conex.Close();
        //    }

        //    if (true)//con.actualizar2("Usuario", campos, condicion)
        //    {
        //        ////("Se generó la contraseña correctamente");
        //        TempData["error"] = "alert-success";
        //    }
        //    else
        //    {
        //        return "Algo extraño sucedió y no pudimos generar tu contraseña, vuelva a intentarlo mas tarde";
        //        TempData["error"] = "alert-danger";
        //    }

        //    //Metodos.sendMail(contenido, corre, subject)
        //    if (true)
        //    {
        //        return "Se ha enviado un correo con su contraseña, el correo puede tardar en llegar hasta 12 horas despues del registro";
        //        TempData["error"] = "alert-success";
        //    }
        //    else
        //    {
        //        return "Se ha enviado un correo con su contraseña, el correo puede tardar en llegar hasta 12 horas despues del registro";
        //        TempData["error"] = "alert-danger";
        //    }

   // }

    [AllowAnonymous]
        public ActionResult Registro(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult MisDatos(FormCollection form = null)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            string curp = Session["CURP"].ToString();
            if (form.HasKeys())
            {
                //se va a guardar
                var telCas = form["telefonoc"];
                var telCel = form["telefonocl"];
                var cp = form["codigopostal"];
                var edo = form["estado"];
                var muni = form["municipio"];
                var col = form["colonia"];
                var calle = form["calle"];
                var numI = form["numExt"];
                var numE = form["numInt"];
                var tutor = form["tutor"];
                var credencial = form["16240840"];
                System.Diagnostics.Debug.WriteLine("---> Credencial " + credencial);
                if (String.IsNullOrEmpty(credencial))
                {
                    credencial = "0";
                }
                else
                {
                    credencial = "1";
                }

                if (updDatos(curp, edo, muni, col, calle, numI, numE, telCas, telCel, cp, tutor, credencial))
                {
                    //bien
                    TempData["error"] = "alert-success";
                    TempData["message"] = "Datos actualizados correctamente";
                }
                else
                {
                    //mal
                    TempData["error"] = "alert-danger";
                    TempData["message"] = "Error al actualizar los datos";
                }

            }
            else
            {
                //primera vez
            }

            Hashtable datos = buscaCurp(curp);
            if (datos["found"].ToString() == "true")
            {
                ViewData["nombre"] = datos["nombre"].ToString();
                ViewData["apellido_pat"] = datos["apellido_pat"].ToString();
                ViewData["apellido_mat"] = datos["apellido_mat"].ToString();
                ViewData["estado_r"] = datos["estado_r"].ToString();
                ViewData["municipio"] = datos["municipio"].ToString();
                ViewData["colonia"] = datos["colonia"].ToString();
                ViewData["calle"] = datos["calle"].ToString();
                ViewData["num_int"] = datos["num_int"].ToString();
                ViewData["num_ext"] = datos["num_ext"].ToString();
                ViewData["telefono"] = datos["telefono"].ToString();
                ViewData["celular"] = datos["celular"].ToString();
                ViewData["correo"] = datos["correo"].ToString();
                ViewData["codigopostal"] = datos["codigopostal"].ToString();
                ViewData["tutor"] = datos["tutor"].ToString();

                if (String.IsNullOrEmpty(datos["credencial"].ToString()))
                {
                    ViewData["credencial"] = "0";
                }
                else
                {
                    if (datos["credencial"].Equals("True"))
                    {
                        ViewData["credencial"] = "1";
                    }
                    else
                    {
                        ViewData["credencial"] = "0";
                    }
                }
                string date = datos["fecha_nac"].ToString();

                int dia = Convert.ToInt32(date.Substring(0, 2));
                int mes = Convert.ToInt32(date.Substring(3, 2));
                int ano = Convert.ToInt32(date.Substring(6, 4));

                DateTime oldDate = new DateTime(ano, mes, dia);
                DateTime newDate = DateTime.Now;
                // Difference in days, hours, and minutes.
                TimeSpan ts = newDate - oldDate;
                // Difference in days.
                int differenceInDays = ts.Days;
                int anos = Convert.ToInt32(Math.Floor(differenceInDays / 365.25));
                ViewData["edad"] = anos.ToString();
            }

            return View();

        }



        public ActionResult Acuses()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            return View();
        }

        protected Hashtable buscaCurp(String curp)
        {
            Conexion con = new Conexion();
            Hashtable consulta = new Hashtable();

            if (con.consulta("SELECT DISTINCT A.nombre, A.apellido_pat, A.apellido_mat, A.fecha_nac, A.estado_r, A.municipio, " +
                    "A.colonia, A.calle, A.num_int, A.num_ext, A.telefono, A.correo, A.codigopostal, " +
                    "A.celular, C.tutor, C.Credencial, B.estatus FROM persona A " +
                    "LEFT JOIN folio C ON C.CURP = A.CURP " +
                    "LEFT JOIN usuario B ON C.CURP = B.CURP " + //Se cambio institucion_beneficiario por usuario
                    "WHERE A.CURP = '" + curp + "' ").Rows.Count > 0)
            {
                foreach (DataRow dbRow in con.consulta( //"SELECT * from usuario u inner join institucion_beneficiario i on u.curp = i.curp WHERE u.curp = '" + curp + "'"
                    "SELECT DISTINCT A.nombre, A.apellido_pat, A.apellido_mat, A.fecha_nac, A.estado_r, A.municipio, " +
                    "A.colonia, A.calle, A.num_int, A.num_ext, A.telefono, A.correo, A.codigopostal, " +
                    "A.celular, C.tutor, C.Credencial, B.estatus FROM persona A " +
                    "LEFT JOIN folio C ON C.CURP = A.CURP " +
                    "LEFT JOIN usuario B ON C.CURP = B.CURP " + //Se cambio institucion_beneficiario por usuario
                    "WHERE A.CURP = '" + curp +"' " ).Rows)
                {

                    consulta["nombre"] = dbRow["nombre"].ToString();
                    consulta["apellido_pat"] = dbRow["apellido_pat"].ToString();
                    consulta["apellido_mat"] = dbRow["apellido_mat"].ToString();

                    consulta["fecha_nac"] = dbRow["fecha_nac"].ToString();
                    consulta["estado_r"] = dbRow["estado_r"].ToString();
                    consulta["municipio"] = dbRow["municipio"].ToString();
                    consulta["colonia"] = dbRow["colonia"].ToString();
                    consulta["calle"] = dbRow["calle"].ToString();
                    consulta["num_int"] = dbRow["num_int"].ToString();
                    consulta["num_ext"] = dbRow["num_ext"].ToString();
                    consulta["telefono"] = dbRow["telefono"].ToString();
                    consulta["correo"] = dbRow["correo"].ToString();
                    consulta["codigopostal"] = dbRow["codigopostal"].ToString();
                    consulta["celular"] = dbRow["celular"].ToString();
                    consulta["tutor"] = dbRow["tutor"].ToString();
                    consulta["credencial"] = dbRow["credencial"].ToString();
                    //consulta["tipo"] = dbRow["tipo"].ToString();
                    consulta["estatus"] = dbRow["estatus"].ToString();

                    consulta["found"] = "true";
                }
            }
            else
            {
                consulta["found"] = "false";
            }

            return consulta;
        }


        protected Boolean updDatos(String curp, String Edo, String Muni,
            String col, String calle, String numint, String numext,
            String tel, String telce, String cp, String tut, String credencial)
        {
            Conexion con = new Conexion();

            string ad =
            "estado_r='" + Edo +
            "',municipio='" + Muni +
            "',colonia='" + col +
            "',calle='" + calle +
            "',num_int='" + numint +
            "',num_ext='" + numext +
            "',codigopostal='" + cp +
            "',telefono='" + tel +
            "',celular='" + telce + "'";


            if (con.actualizar("Persona", ad, "curp = '" + curp + "'"))
            {
                DateTime localDate = DateTime.Now;
                ad =
                    "tutor='" + tut +
                    "',credencial='" + credencial + 
                       "',fecha_folio='" + localDate + "'";
                     
                if (con.actualizar("folio", ad, "curp = '" + curp + "'"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }



        }

        public ActionResult GeneraReporte()
        {
            ReportViewer reportViewer = new ReportViewer();
            Conexion con = new Conexion();
            Warning[] warnings;
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }


            if (con.consulta(

                            //"SELECT u.codigopostal, u.calle, i.Credencial " +
                            //"from usuario u join institucion_beneficiario i " +
                            //"on u.CURP = i.CURP WHERE u.CURP = '" + Session["CURP"].ToString() + "' AND i.estatus = 'A'"

                            "SELECT u.codigopostal, u.calle, i.Credencial from " +
                            "persona u LEFT JOIN folio i on u.CURP = i.CURP " +
                            "LEFT JOIN institucion_beneficiario B ON i.CURP = B.CURP " +
                            "WHERE u.CURP = '" + Session["CURP"].ToString() + "' AND B.estatus = 'A' ").Rows.Count > 0)
            {
                foreach (DataRow dbRow in con.consulta(

                            //"SELECT u.codigopostal, u.calle, i.Credencial " +
                            //"from usuario u join institucion_beneficiario i " +
                            //"on u.CURP = i.CURP WHERE u.CURP = '" + Session["CURP"].ToString() + "' AND i.estatus = 'A'"

                            "SELECT u.codigopostal, u.calle, i.Credencial from " +
                            "persona u LEFT JOIN folio i on u.CURP = i.CURP " +
                            "LEFT JOIN institucion_beneficiario B ON i.CURP = B.CURP " +
                            "WHERE u.CURP = '" + Session["CURP"].ToString() + "' AND B.estatus = 'A' ").Rows)
                {
                    if (string.IsNullOrEmpty(dbRow["codigopostal"].ToString()) && string.IsNullOrEmpty(dbRow["calle"].ToString()) &&
                        string.IsNullOrEmpty(dbRow["Credencial"].ToString()))
                    {
                        TempData["error"] = "alert alert-danger";
                        TempData["message"] = "Por Favor llena tus datos antes de descargar acuse";
                        return RedirectToAction("MisDatos", "Beneficiario");
                    }
                }
            }
            else
            {
                TempData["error"] = "alert alert-danger";
                TempData["message"] = "Error al consultar sus Datos";
                return RedirectToAction("Index", "Home");
            }

            string[] streamIds;
            string fecha_rec = DateTime.Now.ToString("dd-MM-yyyy");
            string dependencia = "";
            string iden = "";
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            Hashtable datos = buscaCurp(Session["CURP"].ToString());
            if (con.consulta(

                             //"SELECT Credencial from institucion_beneficiario " +
                             //"WHERE CURP = '" + Session["CURP"].ToString() + "' AND estatus = 'A'"

                             "SELECT Credencial from folio A " +
                             "LEFT JOIN institucion_beneficiario B ON A.CURP = B.CURP " +
                             "WHERE A.CURP = '" + Session["CURP"].ToString() + "' AND B.estatus = 'A'").Rows.Count > 0)
            {
                foreach (DataRow dbRow in con.consulta(

                             //"SELECT Credencial from institucion_beneficiario " +
                             //"WHERE CURP = '" + Session["CURP"].ToString() + "' AND estatus = 'A'"

                             "SELECT Credencial from folio A " +
                             "LEFT JOIN institucion_beneficiario B ON A.CURP = B.CURP " +
                             "WHERE A.CURP = '" + Session["CURP"].ToString() + "' AND B.estatus = 'A'").Rows)
                {
                    iden = dbRow["Credencial"].ToString();
                }
            }
            else
            {
                TempData["error"] = "alert alert-danger";
                TempData["message"] = "Error al consultar sus Datos";
                return RedirectToAction("MisDatos", "Beneficiario");
            }
            reportViewer.ProcessingMode = ProcessingMode.Local;

            if (iden == "False")
            {

                //bUSCA EL NOMBRE DE LA INSTITUCION
                if (con.consulta("SELECT nombre from institucion " +
                                 "WHERE CCT = (SELECT dependencia from institucion " +
                                 "WHERE CCT = (SELECT CCT from institucion_beneficiario " +
                                 "WHERE CURP = '" + Session["CURP"].ToString() + "' AND estatus = 'A')) ").Rows.Count > 0)
                {
                    foreach (DataRow dbRow in con.consulta("SELECT nombre from institucion " +
                                 "WHERE CCT = (SELECT dependencia from institucion " +
                                 "WHERE CCT = (SELECT CCT from institucion_beneficiario " +
                                 "WHERE CURP = '" + Session["CURP"].ToString() + "' AND estatus = 'A')) ").Rows)
                    {
                        dependencia = dbRow["nombre"].ToString();
                        reportViewer.LocalReport.ReportPath = Server.MapPath(@"~\Reportes\ReporteAMNMD.rdlc");
                    }
                }
                else
                {
                    reportViewer.LocalReport.ReportPath = Server.MapPath(@"~\Reportes\ReporteAMN.rdlc");
                }

            }
            else if (iden == "True")
            {
                if (con.consulta("SELECT nombre from institucion " +
                                 "WHERE CCT = (SELECT dependencia from institucion " +
                                 "WHERE CCT = (SELECT CCT from institucion_beneficiario " +
                                 "WHERE CURP = '" + Session["CURP"].ToString() + "' AND estatus = 'A')) ").Rows.Count > 0)
                {
                    foreach (DataRow dbRow in con.consulta("SELECT nombre from institucion " +
                                 "WHERE CCT = (SELECT dependencia from institucion " +
                                 "WHERE CCT = (SELECT CCT from institucion_beneficiario " +
                                 "WHERE CURP = '" + Session["CURP"].ToString() + "' AND estatus = 'A')) ").Rows)
                    {
                        dependencia = dbRow["nombre"].ToString();
                        //(iden);
                        reportViewer.LocalReport.ReportPath = Server.MapPath(@"~\Reportes\ReporteAMYMD.rdlc");
                    }
                }
                else
                {
                    //(iden);
                    reportViewer.LocalReport.ReportPath = Server.MapPath(@"~\Reportes\ReporteAMY.rdlc");
                }
            }
            else
            {
                TempData["error"] = "alert alert-danger";
                TempData["message"] = "Error al consultar sus Identificacion " + iden;
                return RedirectToAction("MisDatos", "Beneficiario");
            }

            try
            {
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsUsuario",
                //con.consulta("SELECT * from usuario "
                //+ "WHERE curp = '" + Session["CURP"].ToString() + "'")));
                // con.consulta("SELECT * from persona "
                //+ "WHERE curp = '" + Session["CURP"].ToString() + "'")));

                con.consulta("SELECT CURP, nombre, apellido_pat, apellido_mat, estado_nac, sexo, fecha_nac, codigopostal, estado_r, " +
                             "municipio = CASE municipio " +
                             "WHEN 'Dolores Hidalgo Cuna de la Independencia Nacional' THEN 'Dolores Hidalgo' " +
                             "END, " +
                             "municipio, colonia, " +
                             "calle, num_int, num_ext, telefono, celular, correo from persona " +
                             "WHERE curp = '" + Session["CURP"].ToString() + "'")));
            }
            catch (EntitySqlException e)
            {

            }
            try
            {
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsInstBen",
                    con.consulta("SELECT * from institucion_beneficiario "
                    + "WHERE CURP = '" + Session["CURP"].ToString() + "' AND estatus = 'A'")));
            }
            catch (EntitySqlException e)
            {

            }
            try
            {
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsTablet",
                    con.consulta("SELECT * from tablet "
                    + "WHERE beneficiario = '" + Session["CURP"].ToString() + "'")));
            }
            catch (EntitySqlException e)
            {

            }
            try
            {
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsInstitucion",
                    con.consulta("SELECT * from institucion "
                    + "WHERE CCT = (SELECT CCT from institucion_beneficiario "
                    + "WHERE CURP = '" + Session["CURP"].ToString() + "' AND estatus = 'A')")));
            }
            catch (EntitySqlException e)
            {

            }

            ReportParameter[] parameters = new ReportParameter[3];
            parameters[0] = new ReportParameter("curp", Session["CURP"].ToString());
            parameters[1] = new ReportParameter("fecha", fecha_rec);
            parameters[2] = new ReportParameter("dependencia", dependencia);
            reportViewer.LocalReport.SetParameters(parameters);
            reportViewer.LocalReport.Refresh();
            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename=" + Session["CURP"].ToString() + "." + extension);
            Response.BinaryWrite(bytes);
            Response.Flush();

            ViewBag.ReportViewer = reportViewer;
            return View();
        }

        protected void pdf(String plantilla, String nombre_al, String correo, String domicilio,
                                    String colonia, String municipio)
        {
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Formulario.pdf");
            Printer pr = new Printer(nombre_al, correo, domicilio, colonia, municipio);
            pr.FillPDFBenMay(Server.MapPath(plantilla), Response.OutputStream);
        }

        protected int calcEdad(String fecnac)
        {
            int edad = 18;
            string date = fecnac;

            int dia = Convert.ToInt32(date.Substring(0, 2));
            int mes = Convert.ToInt32(date.Substring(3, 2));
            int ano = Convert.ToInt32(date.Substring(6, 4));

            DateTime oldDate = new DateTime(ano, mes, dia);
            DateTime newDate = DateTime.Now;
            // Difference in days, hours, and minutes.
            TimeSpan ts = newDate - oldDate;
            // Difference in days.
            int differenceInDays = ts.Days;
            edad = Convert.ToInt32(Math.Floor(differenceInDays / 365.25));

            return edad;
        }
    }
}