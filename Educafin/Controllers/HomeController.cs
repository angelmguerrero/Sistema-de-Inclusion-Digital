using Educafin.Clases;
using Educafin.Clases.Transacciones;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Educafin.Controllers
{
    public class HomeController : Controller
    {

        HomeTransacciones objHomeTrans = new HomeTransacciones();

        public string tipus;
        public string CURP;



        public ActionResult Index()
        {
            Session["CURP"] = null;
            Session["nombre"] = null;
            Session["tipo"] = null;
            Session["pertenece"] = null;
            Session["grado"] = null;
            Session["cctTemp"] = null;
            Session["estatus"] = null;

            ViewBag.error = 10;
            return View();
        }


        [HttpPost]
        public ActionResult Index(string txtNombre, string txtpass)
        {
            int error = 10;
            //-1 = Nombre de Usuario incorrecto
            //0 = ERROR Contraseña incorrecta
            //2 = ERROR El usuario esta inactivo
            //3 = ERROR Su vigencia como Usuario expiro

            try
            {
                objHomeTrans.consultaDatosUsuario(txtNombre);

                if (txtpass.Equals(objHomeTrans.getPassword()))
                {
                    Session["CURP"] = objHomeTrans.getCURP();
                    Session["nombre"] = objHomeTrans.getNombre();
                    Session["tipo"] = objHomeTrans.getTipo();
                    Session["pertenece"] = objHomeTrans.getPertenece();
                    Session["grado"] = objHomeTrans.getGrado();
                    Session["estatus"] = objHomeTrans.getEstatus();


                    if (objHomeTrans.getEstatus() == "A")
                    {
                        objHomeTrans.consultaInstitucion(Session["pertenece"].ToString()); //get grado, mp, y dependencia

                        switch (objHomeTrans.getTipo())
                        {
                            case "A":
                                Session["cctTemp"] = "";
                                return RedirectToAction("inicio");

                            case "P":
                                return RedirectToAction("inicio");

                            case "I":
                                if (objHomeTrans.getMedioProveedor() == true && objHomeTrans.getDependencia() == null ||
                                    objHomeTrans.getMedioProveedor() == true && objHomeTrans.getDependencia() == "")
                                {
                                    Session.Remove("tipo");
                                    Session["tipo"] = "MP";
                                    System.Diagnostics.Debug.WriteLine(Session["tipo"].ToString());
                                    return RedirectToAction("inicio");

                                }
                                else if (objHomeTrans.getMedioProveedor() == true &&
                                   objHomeTrans.getDependencia() == Session["pertenece"].ToString())
                                {
                                    Session.Remove("tipo");
                                    Session["tipo"] = "MI";
                                    System.Diagnostics.Debug.WriteLine(Session["tipo"].ToString());
                                    return RedirectToAction("inicio");

                                }
                                else if (objHomeTrans.getMedioProveedor() == false &&
                                   objHomeTrans.getDependencia() != Session["pertenece"].ToString())
                                {
                                    Session["tipo"] = "I";
                                    System.Diagnostics.Debug.WriteLine(Session["tipo"].ToString());
                                    return RedirectToAction("inicio");
                                }
                                break;

                            case "B":
                                return RedirectToAction("inicio");

                            case "T":
                                Session.Remove("tipo");
                                Session["tipo"] = "T";
                                DateTime date_Time = DateTime.Now;

                                objHomeTrans.consultaFechaLimite(txtNombre); //RETORNA objHomeTrans.getDateLimit();

                                if (objHomeTrans.getDateLimit() >= date_Time)
                                {
                                    ViewBag.error = error;
                                    return RedirectToAction("inicio");
                                    
                                }
                                else
                                {
                                   // error = 5; // "Su vigencia como Usuario Temporal expiro";
                                    objHomeTrans.bloqueoUsuarioTemp(txtNombre);
                                    return RedirectToAction("index");
                                }

                        } //Switch

                    } //Estatus A
                    else if (objHomeTrans.getEstatus() == "E") //Estatus Espera
                    {
                        return RedirectToAction("AvisoPrivacidad");
                    }
                    else if (objHomeTrans.getEstatus() == "I") //Estatus I
                    {
                        error = 2; // "El usuario esta inactivo"
                        ViewBag.error = error;
                        return RedirectToAction("index");
                    }
                }
                else
                {
                    error = 0; // "Password Incorrecta" 
                    ViewBag.error = error;
                }
        }
            catch (Exception ex) { Console.WriteLine(ex.Message); error = -1; /*Nombre de Usuario incorrecto*/ }
    ViewBag.error = error;

            return View();
        }


        public ActionResult inicio()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            ViewBag.nombre1 = (String)Session["nombre"];
            Session["estatus"] = "A";

            return View();
        }

        public ActionResult CambiaPass()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            ViewBag.nombre1 = (String)Session["nombre"];
            return View();
        }

        [HttpPost]
        public ActionResult CambiaPass(string pass)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            objHomeTrans.cambiaContraseña(pass, Session["CURP"].ToString());

            return RedirectToAction("inicio");
        }


        public ActionResult AvisoPrivacidad()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            ViewBag.nombre1 = (String)Session["nombre"];
            return View();
        }

        [HttpPost]
        public ActionResult AvisoPrivacidad(Object avisoPriv)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            string CURP = Session["CURP"].ToString();

            if (avisoPriv.ToString().Equals("System.String[]")) //En caso de ser seleccionado el check
            {
                objHomeTrans.consultaAvisoPrivacidad(CURP);
            }
            else if (avisoPriv.ToString().Equals("System.Object")) //En caso de no ser seleccionado el check
            {
                TempData["Message"] = "Debe leer y aceptar el aviso de privacidad para continuar";
                TempData["error"] = "alert alert-danger";
                return RedirectToAction("AvisoPrivacidad");
            }
            return RedirectToAction("CambiaPass");
        }

        public ActionResult ForgotPass()
        {
            return View();
        }

        public ActionResult RecoverPass(FormCollection form)
        {
            var correo1 = form["email"];
            var curp = form["curp"];

            string result = "";

            Hashtable consulta = buscaCurp(curp);

            if (consulta["found"].ToString().Equals("true"))
            {
                if (consulta["correo"].ToString().Equals(correo1))
                {
                    enviarCorreo(correo1, curp);
                }
                result += "Si el correo coincide con el correo registrado, se le enviará una contraseña nueva";
            }
            else
            {
                result += "CURP no existente en el sistema";
            }
            ViewData["resultado"] = result;
            return View();
        }



        private string enviarCorreo(string corre, string curp)
        {
            string password = Metodos.generatePassword(6);
            string subject = "Recuperación de contraseña";
            string contenido = "";
            contenido += "<center>";
            contenido += "<h4>";
            contenido += "Recuperación de contraseña, su curp:<br>";
            contenido += "<b>" + curp + "</b><br>";
            contenido += "Ha solicitado una nueva contraseña<br>";
            contenido += "Para countinuar, acceda en la siguiente página:<br><br>";
            contenido += "<a href='http://tabletasube.itleon.edu.mx/'  target='_blank'>http://tabletasube.itleon.edu.mx/</a><br>";
            contenido += "<br>con la siguiente contraseña: ";
            contenido += "<strong>";
            contenido += password;
            contenido += "</strong></h4>";
            contenido += "</center>";
            Conexion con = new Conexion();
            string campos = "password='" + password + "', estatus ='E'";
            string condicion = "curp = '" + curp + "'";

            password = password.Replace("'", "");
            curp = curp.Replace("'", "");

            objHomeTrans.cambiaUsuarioEspera(curp, password);

            if (true)//con.actualizarcorreo(password,curp)
            {
                // //("Se generó la contraseña correctamente");
            }
            else
            {
                return "Algo extraño sucedió y no pudimos generar tu contraseña, vuelva a intentarlo mas tarde";
            }
            if (Metodos.sendMail(contenido, corre, subject))
            {
                return "Se ha enviado un correo con su contraseña";
            }
            else
            {
                return "Algo extraño sucedió, vuelva a intentarlo mas tarde";
            }

        }

        protected Hashtable buscaCurp(String curp)
        {
            Hashtable consulta = new Hashtable();

            objHomeTrans.consultaCorreo(curp);
            if (objHomeTrans.getCorreo() != null || objHomeTrans.getCorreo() != "")
            {

                consulta["correo"] = objHomeTrans.getCorreo();
                consulta["found"] = "true";
            }
            else
            {
                consulta["found"] = "false";
            }
            return consulta;
        }

    }
}