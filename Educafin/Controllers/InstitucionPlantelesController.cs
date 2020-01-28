using Educafin.Clases;
using Educafin.Clases.Transacciones;
using Educafin.Models;
using Institucion.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Educafin.Controllers
{
    public class InstitucionPlantelesController : Controller
    {
        InstitucionPlantelesTransacciones objInstTrans = new InstitucionPlantelesTransacciones();
        InstitucionTransacciones objInstitucion = new InstitucionTransacciones();
        Excel objExcel = new Excel();


        public ActionResult RegistroBeneficiario()
        {
            if (Session["CURP"] == null) { return RedirectToAction("Index", "Home"); }
            string dato1 = Server.MapPath(@"~\Content\miarchivo.xlsx");
            string dato2 = Server.MapPath(@"~\Content\miarchivo2.xlsx");
            string CCT = Session["pertenece"].ToString();

            List<Alumno> lista = new List<Alumno>();

            try
            {
                lista = objInstitucion.consultaErroresBenef(CCT);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            InstitucionTransacciones objInstTransac = new InstitucionTransacciones();
            objInstTransac.eliminaErroresBeneficiario(CCT);

            objExcel.cargaarexcel(dato1, lista);
            objExcel.cargaFormatoPlanteles(dato2);

            return View(lista);

        }


        [HttpPost]
        public ActionResult RegistroBeneficiario(HttpPostedFileBase file)
        {
            if (Session["CURP"] == null) { return RedirectToAction("Index", "Home"); }
            string fileLocation;
            DataSet ds = new DataSet();


            try
            {

                if (Request.Files["file"].ContentLength > 0)
                {
                    string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);

                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        fileLocation = Server.MapPath("~/Content/") + Request.Files["file"].FileName;

                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }
                        Request.Files["file"].SaveAs(fileLocation);
                        string excelConnectionString = string.Empty;
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        //connection String for xls file format.
                        if (fileExtension == ".xls")
                        {
                            excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        }
                        //connection String for xlsx file format.
                        else if (fileExtension == ".xlsx")
                        {
                            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        }
                        //Create Connection to Excel work book and add oledb namespace
                        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                        excelConnection.Open();
                        DataTable dt = new DataTable();

                        dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (dt == null)
                        {
                            return null;
                        }
                        String[] excelSheets = new String[dt.Rows.Count];
                        int t = 0;
                        //excel data saves in temp file here.
                        foreach (DataRow row in dt.Rows)
                        {
                            excelSheets[t] = row["TABLE_NAME"].ToString();
                            t++;
                        }
                        OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);
                        string query = string.Format("Select * from [{0}]", excelSheets[0]);
                        using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                        {
                            dataAdapter.Fill(ds);
                        }
                        excelConnection.Close();
                        excelConnection1.Close();
                    }
                    if (fileExtension.ToString().ToLower().Equals(".xml"))
                    {
                        fileLocation = Server.MapPath("~/Content/") + Request.Files["FileUpload"].FileName;
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }
                        Request.Files["FileUpload"].SaveAs(fileLocation);
                        XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                        // DataSet ds = new DataSet();
                        ds.ReadXml(xmlreader);
                        xmlreader.Close();

                    }

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string validaCurp = ds.Tables[0].Rows[i][0].ToString().ToUpper();
                        string matricula = ds.Tables[0].Rows[i][1].ToString();
                        string dependencia = ds.Tables[0].Rows[i][2].ToString();

                        string CCT = Session["pertenece"].ToString();


                        string clave = dependencia + validaCurp;
                        string datetime = DateTime.Now.ToString("yyyy/MM/dd");

                        if (objInstTrans.consultaDependencia(dependencia).Equals(CCT)) //RETORNA select dependencia from institucion
                        {
                            CURP objCurp = new CURP();
                            string[] str = objCurp.get_Curp(validaCurp.ToString().ToUpper()).Split(',');

                            if (str.Length >= 7)
                            {
                                string curp = str[0];
                                string ape_pat = str[1];
                                string ape_mat = str[2];
                                string nombre = str[3];
                                string sexo = str[4];
                                string fecha_nac = str[5];
                                string lugar_nac = str[6];
                                curp.Trim();

                                try
                                {
                                    objInstitucion.consultaEstatusBeneficiario(curp.Trim()); //Usuario
                                }
                                catch (Exception ex) { Console.WriteLine(ex.Message); }

                                string estatus = objInstitucion.getStatus();

                                if (estatus.Equals("I"))
                                {
                                    try
                                    {
                                        objInstitucion.consultaBeneficiariosInactivos(curp.Trim(), dependencia);
                                    }
                                    catch (Exception ex) { Console.WriteLine(ex.Message); }

                                    if (objInstitucion.getCURP() != "")
                                    {
                                        try
                                        {
                                            objInstitucion.trasladoBeneficiario(clave, curp.Trim(), matricula, dependencia);
                                        }
                                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            objInstitucion.consultaBeneficiariosActivos(curp.Trim(), dependencia);
                                        }
                                        catch (Exception ex) { Console.WriteLine(ex.Message); }

                                        if (objInstitucion.getCURP() != "")
                                        {
                                            // MENSAJE DE ERROR EL BENEFICIARIO 
                                            string error = "Se registro en su institución";
                                            try
                                            {
                                                objInstitucion.ingresaErrores(clave, CCT, curp.Trim(), matricula, ape_pat, ape_mat, nombre, datetime, error);
                                            }
                                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                                        }
                                    }
                                }
                                else if (estatus.Equals("A"))
                                {
                                    try
                                    {
                                        objInstitucion.consultaBeneficiariosActivos(curp.Trim(), dependencia);
                                    }
                                    catch (Exception ex) { Console.WriteLine(ex.Message); }

                                    if (objInstitucion.getCURP() != "")
                                    {
                                        string error = "Se registro en su institución anteriormente";
                                        try
                                        {
                                            objInstitucion.ingresaErrores(clave, CCT, curp.Trim(), matricula, ape_pat, ape_mat, nombre, datetime, error);
                                        }
                                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            objInstitucion.consultaBenefEnOtraInst(curp.Trim(), dependencia);
                                        }
                                        catch (Exception ex) { Console.WriteLine(ex.Message); }

                                        if (objInstitucion.getCURP() != "")
                                        {
                                            string mensaje2 = "Registrado en otra institución";
                                            try
                                            {
                                                objInstitucion.ingresaErrores(clave, CCT, validaCurp.Trim(), matricula, ape_pat, ape_mat, nombre, datetime, mensaje2);
                                            }
                                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                                        }
                                    }
                                } //Validación

                                else if (estatus.Equals("E"))
                                {
                                    try
                                    {
                                        objInstitucion.consultaBeneficiarioRegistrado(curp.Trim(), dependencia);
                                    }
                                    catch (Exception ex) { Console.WriteLine(ex.Message); }

                                    if (objInstitucion.getCURP() != "")
                                    {
                                        string error = "Se registro en su institución anteriormente";

                                        try
                                        {
                                            objInstitucion.ingresaErrores(clave, CCT, curp.Trim(), matricula, ape_pat, ape_mat, nombre, datetime, error);
                                        }
                                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            objInstitucion.consultaBenefEnOtraInst(curp.Trim(), dependencia);
                                        }
                                        catch (Exception ex) { Console.WriteLine(ex.Message); }

                                        if (objInstitucion.getCURP() != "")
                                        {
                                            string mensaje3 = "Registrado en otra institución";
                                            try
                                            {
                                                objInstitucion.ingresaErrores(clave, CCT, curp.Trim(), matricula, ape_pat, ape_mat, nombre, datetime, mensaje3);
                                            }
                                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                                        }
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        objInstitucion.registraNuevoBenef(clave, dependencia, curp.Trim(), matricula, datetime, nombre, ape_pat, ape_mat, lugar_nac, sexo, fecha_nac);
                                    }
                                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                                }

                            } // str.lengh = 7
                            else
                            {
                                try
                                {
                                    objInstitucion.registraCURPNoValido(clave, CCT, validaCurp.Trim(), matricula, datetime);
                                }
                                catch (Exception ex) { Console.WriteLine(ex.Message); }
                            }

                        }
                        else

                        {
                            string mensaje = "El CCT no pertenece a su institución";
                            try
                            {
                                objInstitucion.ingresaErrores(clave, CCT, validaCurp.Trim(), matricula, "", "", "", datetime, mensaje);
                            }
                            catch (Exception ex) { Console.WriteLine(ex.Message); }

                        }
                    } // for

                }

            }
            catch
            {
                string CCT = Session["pertenece"].ToString();
                try
                {
                    objInstTrans.registraErrorFormato(CCT);
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }

            return RedirectToAction("RegistroBeneficiario");
        }



        //public PropiedadesExcel DownloadFile()
        //{
        //    return new PropiedadesExcel
        //    {
        //        FileName = "BeneficiariosNoRegistrados.xlsx",
        //        Path = @"~/Content/miarchivo.xlsx"
        //    };
        //}

        //public PropiedadesExcel DownloadTablet()
        //{
        //    return new PropiedadesExcel
        //    {
        //        FileName = "BeneficiariosConTablet.xlsx",
        //        Path = @"~/Content/miarchivo3.xlsx"
        //    };
        //}

        //public PropiedadesExcel DownloadBeneficiario()
        //{
        //    return new PropiedadesExcel
        //    {
        //        FileName = "BeneficiariosRegistrados.xlsx",
        //        Path = @"~/Content/miarchivo.xlsx"
        //    };
        //}

        //public PropiedadesExcel DownloadFormato()
        //{
        //    return new PropiedadesExcel
        //    {
        //        FileName = "FormatoBeneficiario.xlsx",
        //        Path = @"~/Content/miarchivo2.xlsx"
        //    };
        //}


        //public ActionResult ListaBeneficiario()
        //{
        //    if (Session["CURP"] == null) { return RedirectToAction("Index", "Home"); }
        //    string dato1 = Server.MapPath(@"~\Content\miarchivo.xlsx");
        //    string CCT = Session["pertenece"].ToString();//aqui va el CCT que se desea usar

        //    objExcel.cargaBeneficiario(dato1, objInstTrans.consultaBeneficiariosRegistrados(CCT));
        //    return View(objInstTrans.consultaBeneficiariosRegistrados(CCT));
        //}

        //[HttpPost]
        //public ActionResult ListaBeneficiario(FormCollection collection, string update)
        //{
        //    if (Session["CURP"] == null) { return RedirectToAction("Index", "Home"); }
        //    string CCT = Session["pertenece"].ToString();//aqui va el CCT que se desea usar

        //    objInstTrans.bajaBeneficiario(update, CCT);
        //    return View(objInstTrans.consultaBeneficiariosRegistrados(CCT));
        //}


        //public ActionResult ListaBenefTablet()
        //{
        //    if (Session["CURP"] == null) { return RedirectToAction("Index", "Home"); }
        //    string dato1 = Server.MapPath(@"~\Content\miarchivo3.xlsx");
        //    string CCT = Session["pertenece"].ToString();//aqui va el CCT que se desea usar

        //    objExcel.cargaBeneficiarioTablet(dato1, objInstTrans.consultaBenefConTablet(CCT));
        //    return View(objInstTrans.consultaBenefConTablet(CCT));
        //}

        //[HttpPost]
        //public ActionResult ListaBenefTablet(FormCollection collection, string update)
        //{
        //    if (Session["CURP"] == null) { return RedirectToAction("Index", "Home"); }
        //    string CCT = Session["pertenece"].ToString();//aqui va el CCT que se desea usar
        //    string datetime = DateTime.Now.ToString("yyyy/MM/dd");

        //    objInstTrans.bajaBenefConTablet(update, datetime);
        //    return View(objInstTrans.consultaBenefConTablet(CCT));
        //}

        ////Autocompletar con CURP Y MATRICULA

        //public ActionResult RegistroTablet()
        //{
        //    if (Session["CURP"] == null) { return RedirectToAction("Index", "Home"); }
        //    string CCT = Session["pertenece"].ToString();//aqui va el CCT que se desea usar

        //    return View(objInstTrans.consultaErrorBeneficiario(CCT));
        //}


        //[HttpPost]
        //public ActionResult RegistroTablet(FormCollection collection, string curp, string matricula, string parametro, string serie)
        //{
        //    if (Session["CURP"] == null) { return RedirectToAction("Index", "Home"); }
        //    string CCT = Session["pertenece"].ToString();
        //    string grado = Session["grado"].ToString();
        //    List<Models.Alumno> lista = new List<Models.Alumno>();
        //    Models.Alumno dato = new Models.Alumno();

        //    if (parametro == "consulta")
        //    {
        //        if (matricula == "" && curp == "")
        //        {
        //            dato.curp = curp;
        //            dato.matricula = matricula;
        //            dato.nombre = "No Disponible";
        //            lista.Add(dato);
        //        }
        //        else if (matricula != "" && curp == "")
        //        {
        //            objInstTrans.consultaBeneficiarioMatricula(curp);

        //        }

        //        else if (curp != "" && matricula == "")
        //        {
        //            objInstTrans.consultaBenefPorCURP(curp);
        //        } // Fin del IF CURP

        //        else if (curp != "" && matricula != "")
        //        {
        //            objInstTrans.consultaBenefPorCURPYMatricula(curp, matricula);
        //        }
        //        lista.Add(dato);
        //    } //Fin del IF Parametro

        //    else if (serie != "")
        //    {
        //        string cct = Session["pertenece"].ToString();
        //        string datetime = DateTime.Now.ToString("dd/MM/yyyy");
        //        if (curp.ToString() != "" && matricula.ToString() != "")
        //        {
        //            if (objInstTrans.consultaBeneficiarioTablet(curp) != "")
        //            {
        //                dato.status = curp + "ya cuenta con una tablet";
        //            }
        //            else
        //            {

        //                if (objInstTrans.consultaNoSerie(cct, serie) != "")
        //                {
        //                    dato.status = "No de serie ya se asignado";
        //                }
        //                else
        //                {
        //                    if (objInstTrans.consultaSerieNoAsignado(CCT, serie) != "")
        //                    {
        //                        objInstTrans.asignaSerieABeneficiario(curp, datetime, grado, serie);
        //                    }
        //                    else
        //                    {
        //                        dato.status = "No. de serie no disponible";
        //                    }
        //                }
        //            }
        //            lista.Add(dato);
        //        }
        //    }
        //    return View(lista);
        //}


        //public ActionResult RegistroUnico()
        //{
        //    if (Session["CURP"] == null) { return RedirectToAction("Index", "Home"); }
        //    string CCT = Session["pertenece"].ToString();//aqui va el CCT que se desea usar

        //    return View(objInstTrans.consultaErrorTablets(CCT));
        //}


        //[HttpPost]
        //public ActionResult RegistroUnico(FormCollection collection, string curp, string matricula, string parametro)
        //{
        //    if (Session["CURP"] == null) { return RedirectToAction("Index", "Home"); }
        //    string CCT = Session["pertenece"].ToString();//aqui va el CCT que se desea usar
        //    List<Models.Alumno> lista = new List<Models.Alumno>();
        //    Models.Alumno dato = new Models.Alumno();

        //    if (parametro == "consulta")
        //    {
        //        if (matricula == "" && curp == "")
        //        {
        //            dato.no = 1;
        //            dato.curp = curp;
        //            dato.matricula = matricula;
        //            dato.nombre = "";
        //            dato.status = "Campos: CURP y Matricula necesarios";
        //        }

        //        else if (curp != "" && matricula != "")
        //        {
        //            string validaCurp = curp.ToString().ToUpper();
        //            try
        //            {
        //                CURP objCurp = new CURP();
        //                string[] str = objCurp.get_Curp(validaCurp.ToString().ToUpper()).Split(',');

        //                if (str.Length >= 7)
        //                {
        //                    string CURP = str[0];
        //                    string ape_pat = str[1];
        //                    string ape_mat = str[2];
        //                    string nombre = str[3];
        //                    string sexo = str[4];
        //                    string fecha_nac = str[5];
        //                    string lugar_nac = str[6];

        //                    dato.no = 1;
        //                    dato.curp = validaCurp;
        //                    dato.matricula = matricula;
        //                    dato.nombre = ape_pat + " " + ape_mat + " " + nombre;
        //                    dato.status = "No Registrado";

        //                    objInstTrans.consultaEstatusBeneficiario(validaCurp);

        //                    if (objInstTrans.getCURP() != "")
        //                    {
        //                        objInstTrans.consultaBeneficiarioEnInst(validaCurp, CCT);
        //                        if (objInstTrans.getCURP() != "")
        //                        {
        //                            dato.no = 1;
        //                            dato.curp = objInstTrans.getCURP();
        //                            dato.matricula = objInstTrans.getMatricula();
        //                            dato.nombre = ape_pat + " " + ape_mat + " " + nombre;
        //                            dato.status = "Registrado en su Institución";
        //                        }
        //                        else
        //                        {
        //                            objInstTrans.consultaBenefEnOtraInst(validaCurp, CCT);
        //                            if (objInstTrans.getCURP() != "")
        //                            {
        //                                dato.no = 1;
        //                                dato.curp = objInstTrans.getCURP();
        //                                dato.matricula = objInstTrans.getMatricula();
        //                                dato.nombre = ape_pat + " " + ape_mat + " " + nombre;
        //                                dato.status = "Registrado en otra institución";
        //                            }
        //                        }

        //                    } //Validación


        //                    if (objInstTrans.getEstatus() == "I")
        //                    {
        //                        objInstTrans.consultaBeneficiariosInactivos(validaCurp, CCT);

        //                        if (objInstTrans.getCURP() != "")
        //                        {
        //                            dato.no = 1;
        //                            dato.curp = objInstTrans.getCURP();
        //                            dato.matricula = matricula;
        //                            dato.nombre = ape_pat + " " + ape_mat + " " + nombre;
        //                            dato.status = "Dado de baja de su institución";
        //                        }
        //                        else
        //                        {
        //                            dato.no = 1;
        //                            dato.curp = objInstTrans.getCURP();
        //                            dato.matricula = matricula;
        //                            dato.nombre = ape_pat + " " + ape_mat + " " + nombre;
        //                            dato.status = "No Registrado";
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    dato.no = 1;
        //                    dato.curp = "";
        //                    dato.matricula = "";
        //                    dato.nombre = "";
        //                    dato.status = "Verifique el CURP";
        //                }
        //            }
        //            catch (WebException ex)
        //            {
        //                dato.status = "El Servicio no esta disponible actualmente";
        //            }

        //        }//Fin del IF CURP Y MATRICULA
        //        else
        //        {
        //            dato.status = "No se permiten campos vacíos";
        //        }
        //        lista.Add(dato);
        //    } //Fin del IF Parametro

        //    else if (parametro.ToString() == "No Registrado")
        //    {
        //        string cct = Session["pertenece"].ToString();
        //        string clave = cct + curp.ToString();
        //        string datetime = DateTime.Now.ToString("yyyy/MM/dd");
        //        string validaCurp = curp.ToString().ToUpper();
        //        try
        //        {
        //            if (curp.ToString() != "" && matricula.ToString() != "")
        //            {
        //                CURP objCurp = new CURP();
        //                string[] str = objCurp.get_Curp(validaCurp.ToString().ToUpper()).Split(',');

        //                if (str.Length >= 7)
        //                {
        //                    string CURP = str[0];
        //                    string ape_pat = str[1];
        //                    string ape_mat = str[2];
        //                    string nombre = str[3];
        //                    string sexo = str[4];
        //                    string fecha_nac = str[5];
        //                    string lugar_nac = str[6];

        //                    objInstTrans.consultaBenenficiaio(CURP);

        //                    if (objInstTrans.getCURP() != "")
        //                    {

        //                        objInstTrans.ingresoBeneficiario(clave, CCT, CURP, matricula.ToString(), datetime);

        //                        dato.no = 1;
        //                        dato.curp = CURP;
        //                        dato.matricula = matricula;
        //                        dato.nombre = ape_pat + " " + ape_mat + " " + nombre;
        //                        dato.status = "Alumno Registrado";
        //                        lista.Add(dato);

        //                    }
        //                    else
        //                    {
        //                        objInstTrans.registraNuevoBenef(clave, CCT, CURP, matricula.ToString(), datetime, nombre, ape_pat, ape_mat, lugar_nac, sexo, fecha_nac);
        //                        dato.no = 1;
        //                        dato.curp = CURP;
        //                        dato.matricula = matricula;
        //                        dato.nombre = ape_pat + " " + ape_mat + " " + nombre;
        //                        dato.status = "Alumno Registrado";
        //                        lista.Add(dato);
        //                    }
        //                }
        //                else
        //                {
        //                    dato.no = 1;
        //                    dato.curp = "";
        //                    dato.matricula = "";
        //                    dato.nombre = "";
        //                    dato.status = "Error al Realizar el Registro";
        //                    lista.Add(dato);
        //                }
        //            }
        //            else
        //            {
        //                dato.no = 1;
        //                dato.curp = validaCurp;
        //                dato.matricula = matricula;
        //                dato.nombre = "No Disponible";
        //                dato.status = "No se permiten campos vacios";
        //                lista.Add(dato);
        //            }
        //        }
        //        catch (WebException ex)
        //        {
        //            dato.status = "El Servicio no esta disponible actualmente";
        //        }
        //    } // Parametro

        //    else if (parametro.ToString() == "Registrado en su Institución")
        //    {
        //        dato.no = null;
        //        dato.curp = "";
        //        dato.matricula = "";
        //        dato.nombre = "";
        //        dato.status = "Error: Ya se encuentra registrado en su institución";
        //        lista.Add(dato);
        //    }//Parametro
        //    else if (parametro.ToString() == "Registrado en otra institución")
        //    {
        //        dato.no = null;
        //        dato.curp = "";
        //        dato.matricula = "";
        //        dato.nombre = "";
        //        dato.status = "Error: Debera ser dado de baja de la otra institución";
        //        lista.Add(dato);

        //    }//Parametro

        //    else if (parametro.ToString() == "Dado de baja de su institución")
        //    {
        //        string cct = Session["pertenece"].ToString();
        //        string clave = cct + curp.ToString();
        //        string datetime = DateTime.Now.ToString("yyyy/MM/dd");
        //        string validaCurp = curp.ToString().ToUpper();

        //        try
        //        {
        //            if (curp.ToString() != "" && matricula.ToString() != "")
        //            {
        //                CURP objCurp = new CURP();
        //                string[] str = objCurp.get_Curp(validaCurp.ToString().ToUpper()).Split(',');

        //                if (str.Length >= 7)
        //                {
        //                    string CURP = str[0];
        //                    string ape_pat = str[1];
        //                    string ape_mat = str[2];
        //                    string nombre = str[3];
        //                    string sexo = str[4];
        //                    string fecha_nac = str[5];
        //                    string lugar_nac = str[6];

        //                    objInstTrans.consultaBenenficiaio(CURP);

        //                    if (objInstTrans.getCURP() != "")
        //                    {
        //                        objInstTrans.trasladoBeneficiario(clave, CURP, matricula, CCT);

        //                        dato.no = 1;
        //                        dato.curp = CURP;
        //                        dato.matricula = matricula;
        //                        dato.nombre = ape_pat + " " + ape_mat + " " + nombre;
        //                        dato.status = "Registrado Nuevamente";
        //                        lista.Add(dato);
        //                    }
        //                }
        //                else
        //                {
        //                    dato.no = 1;
        //                    dato.curp = "";
        //                    dato.matricula = "";
        //                    dato.nombre = "";
        //                    dato.status = "Error al Realizar el Registro";
        //                    lista.Add(dato);
        //                }
        //            }
        //            else
        //            {
        //                dato.no = 1;
        //                dato.curp = validaCurp;
        //                dato.matricula = matricula;
        //                dato.nombre = "No Disponible";
        //                dato.status = "No se permiten campos vacios";
        //                lista.Add(dato);
        //            }
        //        }
        //        catch (WebException ex)
        //        {
        //            dato.status = "El Servicio no esta disponible actualmente";
        //        }

        //    }//Parametro

        //    else if (parametro.ToString() == "CURP no valida")
        //    {
        //        dato.no = 1;
        //        dato.curp = "";
        //        dato.matricula = "";
        //        dato.nombre = "";
        //        dato.status = "Ingrese un CURP valido";
        //        lista.Add(dato);
        //    } //Parametro

        //    return View(lista);
        //}




    }
}