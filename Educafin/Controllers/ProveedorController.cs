using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Configuration;
using MvcApplication8.Models;
using Institucion.Models;
using Educafin.Clases.Transacciones;

namespace MvcApplication8.Controllers
{
    public class ProveedorController : Controller
    {
        ProveedorTransacciones objProveedor = new ProveedorTransacciones();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult entregainstitucion(string id, string id3)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"]))
            {
                return RedirectToAction("Index", "Home");
            }

            string CCT = (string)Session["pertenece"];//aqui va el institucion
            string consulta = "";
            List<Educafin.Models.EntregaInstitucion> lista = new List<Educafin.Models.EntregaInstitucion>();

            objProveedor.consultaDependencia(CCT); // retorna medioProv y dependencia
            string dependencia = objProveedor.getDependencia();
            if (objProveedor.getMedioProveedor() == false) // SI ES INSTITUCION
            {
                if (objProveedor.getDependencia().Equals(""))
                {
                    consulta = objProveedor.regresaConsulta();
                    lista = objProveedor.consultaEntregaInstitucion(consulta, CCT);
                }
                else
                {
                    consulta = objProveedor.regresaConsulta2();
                    lista = objProveedor.consultaEntregaInstitucion(consulta, dependencia);
                }
            }
            else
            {
                string consulta2 = "";
                consulta = objProveedor.regresaConsulta();
                consulta2 = objProveedor.regresaConsulta2();
                lista = objProveedor.consultaEntregaInstitucion2(consulta, consulta2, CCT, dependencia);
            }
            return View(lista);
        }


        [HttpPost]
        public ActionResult entregainstitucion(string pRFC, DateTime fecha, int Cantidad, string responsable)
        {
            DateTime fechaHoy = DateTime.Now;
            string pinstitucion = (string)Session["pertenece"];//aqui va el instituto
            //string exitinstitucion = "";
            //string estatus = "";
            //string dependecia = "";

            objProveedor.consultaClaveEntregaProveedor(pRFC, pinstitucion); //Retorna claveEntrega

            if (objProveedor.getClaveEntrega() != 0)
            {
                objProveedor.comparaCantidadEntrega(objProveedor.getClaveEntrega()); //retorna CantidadProveedor

                if (Cantidad <= objProveedor.getCantidadProveedor())
                {

                    objProveedor.registraEntregaProveedor(fecha, objProveedor.getClaveEntrega());

                    objProveedor.actualizaFechaEntregaRecibida(Cantidad, objProveedor.getClaveEntrega());


                    TempData["error"] = "alert-success";
                    TempData["message"] = "Entrega Recibida Correctamente: ";
                    return RedirectToAction("entregainstitucion");

                }
                else
                {

                    TempData["error"] = "alert-danger";
                    TempData["message"] = "Error: La cantidad de tabletas supera a las que tiene asignadas";
                    return RedirectToAction("entregainstitucion");
                }

            }
            else
            {

                objProveedor.consultaEntregaRecibidaInst(pRFC, pinstitucion); // RETORNA ClaveEntrega
                int clave = objProveedor.getClaveEntrega();

                if (clave > 0)
                {

                    objProveedor.comparaCantidadEntrega(clave); // retorna cantidadProveedor

                    if (Cantidad <= objProveedor.getCantidadProveedor())
                    {
                        objProveedor.registraEntregaMP(fecha, clave);

                        objProveedor.actualizaFechaEntregaRecibida(Cantidad, clave);

                        TempData["error"] = "alert-success";
                        TempData["message"] = "Entrega Recibida Correctamente: ";
                        return RedirectToAction("entregainstitucion");

                    }
                    else
                    {
                        TempData["error"] = "alert-danger";
                        TempData["message"] = "Error: La cantidad de tabletas supera a las que tiene asignadas";
                        return RedirectToAction("entregainstitucion");
                    }

                }

            }

            return RedirectToAction("entregainstitucion");
        }


        public ActionResult Modificarentregainstitucion(string id, string id2)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"]))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.proveedor = id2;
            ViewBag.clave = id;
            return View();
        }


        [HttpPost]
        public ActionResult Modificarentregainstitucion(string fecha_institucion, int cantidadI, string nombre, string clave)
        {
            string pinstitucion = (string)Session["pertenece"];//aqui va el instituto
            string s = "";
            string a = "";
            string c = "";
            char[] Charcadena = fecha_institucion.ToCharArray();
            for (int i = 0; i < Charcadena.Length; i++)
            {
                int ascii = (int)Charcadena[i];
                if (ascii == 84)
                {
                    s += "-" + c + "-" + a;
                    s += " ";
                }
                else if (ascii == 45)
                {
                    if (a == "")
                    {
                        a = s;
                        s = "";
                    }
                    else if (c == "")
                    {
                        c = s;
                        s = "";
                    }
                    else
                    {
                        s += "-";
                    }

                }
                else
                {
                    s += Charcadena[i];
                }
            }
            try
            {
                objProveedor.consultaProveedor(nombre);
                objProveedor.consultaCatidadEntrega(clave);


                if (objProveedor.getCantidadProveedor() >= cantidadI)
                {
                    objProveedor.registraEntrega(s, cantidadI, clave);
                    return RedirectToAction("entregainstitucion");
                }
                else
                {
                    return RedirectToAction("Errorcantidad", new { id = objProveedor.getCantidadProveedor(), id2 = cantidadI });
                }
            }
            catch
            {
                return View("Error");
            }
        }

        public ActionResult Errorcantidad(string id, string id2)
        {
            ViewBag.Cantidadprovedor = id;
            ViewBag.cantidadinstitucion = id2;
            return View();
        }

        public ActionResult Ecantidad()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"]))
            {
                return RedirectToAction("Index", "Home");
            }
            string dato1 = Server.MapPath(@"~\Content\TabletasExcel.xlsx");
            string dato2 = Server.MapPath(@"~\Content\Formato.xlsx");
            string CCT = (string)Session["pertenece"];

            Excel a = new Excel();
            a.cargaarexcelTableta(dato1, objProveedor.consultaTabletasNombres(CCT));
            return View(objProveedor.consultaTabletasNombres(CCT));
        }

        //Perteneciente al usuario Proveedor

        public ActionResult tabletaeliminar()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"]))
            {
                return RedirectToAction("Index", "Home");
            }
            string CCT = (string)Session["pertenece"];

            return View(objProveedor.consultaTabletaSinBeneficiario(CCT));
        }

        public ActionResult eliminar(string id)
        {
            objProveedor.eliminaTablet(id);
            return RedirectToAction("tabletaEliminar");

        }

        public ActionResult eliminarinstitucion(string cct_intELIM)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"]))
            {
                return RedirectToAction("Index", "Home");
            }
            string proveedor = (string)Session["pertenece"];

            objProveedor.consultaClaveInstConNombre(cct_intELIM); // retorna objProveedor.getInstitucion()

            if (objProveedor.getInstitucion() != null || objProveedor.getInstitucion() != "")
            {
                objProveedor.eliminaTabletEstatusPendiente(objProveedor.getInstitucion(), proveedor);
            }
            return RedirectToAction("tabletaEliminar");
        }

        public ActionResult EntradaModificar(string id, string id4)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"]))
            {
                return RedirectToAction("Index", "Home");
            }
            DateTime fech = new DateTime();

            objProveedor.consultaFechaEntregas(id4);

            ViewBag.institucion = objProveedor.getInstitucion();
            ViewBag.cantidad = objProveedor.getCantidadProveedor();
            fech = objProveedor.getFecha();

            string fechadata = fech.ToString("yyyy-MM-ddTHH:mm");
            ViewBag.fecha = fechadata;
            ViewBag.nombre = id;
            ViewBag.clave = id4;

            return View();
        }

        [HttpPost]
        public ActionResult EntradaModificar(string cct_int, int Cantidad, DateTime fecha, int clave)
        {
            string RFC = (string)Session["pertenece"];
            DateTime fechaHoy = DateTime.Now;
            string fechadata = fecha.ToString("dd/MM/yyyy HH:mm");
            try
            {
                objProveedor.consultarEntregasProvInst(RFC, cct_int);
                objProveedor.consultaCantidadTab(cct_int, RFC, clave);

                int totaltable = objProveedor.getCantidadProveedor() + Cantidad;

                if (objProveedor.getCantidad() >= totaltable)
                {
                    objProveedor.actualizaTablet(fechadata, Cantidad, cct_int, clave);
                    return RedirectToAction("Entrega");
                }
                else
                {
                    TempData["error"] = "alert-danger";
                    TempData["message"] = "Error al Insertar la Cantidad de tabletas";
                    return RedirectToAction("Entrega");
                }
            }
            catch
            {
                TempData["error"] = "alert-danger";
                TempData["message"] = "Error al Insertar la Fecha";
                return View("Entrega");
            }
        }








        //Perteneciente al usuario Proveedor
        //--*LISTO

        public ActionResult Entrega()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"]))
            {
                return RedirectToAction("Index", "Home");
            }

            string RFC = (string)Session["pertenece"];//aqui va el rfc del provedor que desea llamar
            return View(objProveedor.consultaTabletasEntregadas(RFC));
        }

        [HttpPost]
        public ActionResult Entrega(FormCollection collection, string cct_int, DateTime fecha, int Cantidad, string responsable)
        {
            string pRFC = (string)Session["pertenece"];//aqui va el rfc del provedor que desea llamar
            DateTime fechaHoy = DateTime.Now;


            objProveedor.consultaClaveInst(pRFC, cct_int); //Retorna GetInstitucion()

            // if (!objProveedor.getInstitucion().Equals(""))
            if (!objProveedor.getInstitucion().Equals(""))
            {
                //objProveedor.consultaTotalTablets(); // getsetCantidad()

                //objProveedor.consultaCantidadProveedor(pRFC, cct_int); //getCantidadProveedor()

                //objProveedor.consultaCantProvInst(cct_int, pRFC); // getTotal()

                //int totaltable = objProveedor.getTotal() + Cantidad;

                //if (objProveedor.getCantidadProveedor() >= totaltable)
                //{

                objProveedor.consultaTotalTablets(); // getsetCantidad()
                objProveedor.realizaEntrega((objProveedor.getCantidad() + 1), pRFC, cct_int, fecha, Cantidad, responsable);
                System.Diagnostics.Debug.WriteLine("Clave:" + (objProveedor.getCantidad() + 1) + " RFC: " + pRFC + " FECHA: " + fecha + "CCT:" + cct_int + " CANTIDAD: " + fecha + " RESPONSABLE: " + responsable);

                TempData["error"] = "alert-success";
                TempData["message"] = "Dato Insertado Correctamete: ";

                //    }
                //    else
                //    {
                //        TempData["error"] = "alert-danger";
                //        TempData["message"] = "Error al Insertar la Cantidad de tabletas " + objProveedor.getCantidadProveedor() + totaltable;
                //        return RedirectToAction("Entrega", new { id = objProveedor.getCantidadProveedor(), id2 = totaltable, id3 = "cantidad" });
                //    }
                //}
                //else if (!objProveedor.getInstitucion().Equals(""))
                //{
                //    TempData["error"] = "alert-danger";
                //    TempData["message"] = "Error Institucion no asignada al provedor: " + objProveedor.getNombre();
                //    return RedirectToAction("Entrega", new { id = objProveedor.getNombre(), id3 = "cctnoa" });
                //}
                //else
                //{
                //    TempData["error"] = "alert-danger";
                //    TempData["message"] = "Error Institucion no Existe: " + cct_int;
                //    return RedirectToAction("Entrega", new { id = cct_int, id3 = "cctnoa1" });
                //}


            }

            return RedirectToAction("Entrega");
        }










        //Perteneciente al usuario Proveedor
        //--*LISTO
        public ActionResult tableta(string id)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"]))
            {
                return RedirectToAction("Index", "Home");
            }

            string pRFC = (string)Session["pertenece"];//aqui va el rfc del provedor que desea llamar
            string dato1 = Server.MapPath(@"~\Content\Tabletas" + pRFC + "Excel.xlsx");
            string dato2 = Server.MapPath(@"~\Content\Formato.xlsx");
            ViewBag.Operacion = id;

            Excel a = new Excel();
            a.cargaarexcelTableta(dato1, objProveedor.consultaErrores(pRFC));
            a.cargaFormatoTableta(dato2);

            return View(objProveedor.consultaErrores(pRFC));
        }


        [HttpPost]
        public ActionResult tableta(FormCollection collection, HttpPostedFileBase file)
        {
            string pRFC = (string)Session["pertenece"];//aqui va el rfc del provedor que desea llamar

            string fileLocation;
            DataSet ds = new DataSet();
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
                int errores = 0;
                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string CCT = ds.Tables[0].Rows[i][0].ToString();
                        string no_Serie = ds.Tables[0].Rows[i][1].ToString();

                        objProveedor.consultaProvInst(CCT); //RETORNA objProveedor.getInstitucion();

                        if (!objProveedor.getInstitucion().Equals(""))
                        {
                            objProveedor.consultaSerieInstProv(no_Serie); //Retorna  objProveedor.getSerie(); &&  objProveedor.getInstitucion(); & objProveedor.getCCTMedioP();

                            if (!objProveedor.getSerie().Equals(no_Serie))
                            {
                               objProveedor.consultaSerieProv(no_Serie, objProveedor.getCCTMedioP());

                                if (objProveedor.getSerie() == "" || objProveedor.getSerie() == null)
                                {

                                    objProveedor.consultaNombreMP(CCT); //RETORNA objProveedor.getNombre(); && objProveedor.getMedioProveedor();

                                    if (objProveedor.getMedioProveedor() == false)
                                    {
                                        
                                        objProveedor.registraNuevaTablet(no_Serie, pRFC, CCT);
                                    }
                                    else
                                    {
                                        objProveedor.registraTabletDuplicada(no_Serie, pRFC, CCT);
                                    }

                                }
                                else {

                                    objProveedor.registraErrorTablet(no_Serie, pRFC, objProveedor.getNombre(), objProveedor.getMedioProveedor().ToString());
                                    errores++;
                                }
                            }
                            else
                            {
                                objProveedor.registraErrorTablet(no_Serie, pRFC, objProveedor.getNombre(), objProveedor.getMedioProveedor().ToString());
                                errores++;
                            }
                        }
                        else
                        {
                            objProveedor.consultaInstitucion(CCT); //RETORNA objProveedor.getInstitucion(); objProveedor.getNombre();

                            if (!objProveedor.getInstitucion().Equals(""))
                            {
                                objProveedor.registraErrorSinAsignacion(no_Serie, pRFC, objProveedor.getNombre(), CCT);
                            }
                            else
                            {
                                objProveedor.errorCCTNoExiste(no_Serie, pRFC, CCT);
                            }
                            errores++;
                        }
                    }
                    TempData["error"] = "alert-success";
                    TempData["message"] = "Archivo Insertado";
                    return RedirectToAction("tableta", new { id = "Archivo Insertado" });
                }
            }
            TempData["error"] = "alert-danger";
            TempData["messaje"] = "Archivo no Aceptado";
            return RedirectToAction("tableta", new { id = "Error al Insertar archivo" });
        }




        public PropiedadesExcel DownloadFile(string id)
        {
            return new PropiedadesExcel
            {
                FileName = "TabletasError.xlsx",
                Path = @"~/Content/Tabletas" + id + "Excel.xlsx"
            };
        }

        public PropiedadesExcel DownloadListatableta()
        {
            string dato1 = Server.MapPath(@"~\Content\TabletasExcel.xlsx");
            string dato2 = Server.MapPath(@"~\Content\Formato.xlsx");

            string pRFC = (string)Session["pertenece"];//aqui va el rfc del provedor que desea llamar

            Excel a = new Excel();
            a.cargaarexcelTabletaLista(dato1, objProveedor.consultaSeiesConNombre(pRFC));

            return new PropiedadesExcel
            {
                FileName = "ListaTableta.xlsx",
                Path = @"~/Content/TabletasExcel.xlsx"
            };
        }

        public PropiedadesExcel DownloadFormato()
        {
            return new PropiedadesExcel
            {
                FileName = "FormatoTableta.xlsx",
                Path = @"~/Content/Formato.xlsx"
            };
        }

        public ActionResult autocompletadoentrega(string cct_int)
        {
            return Json(objProveedor.autoCompletarEntrega(cct_int), JsonRequestBehavior.AllowGet);
        }


        public ActionResult tabletaeliminarMP()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"]))
            {
                return RedirectToAction("Index", "Home");
            }
            string CCT = (string)Session["pertenece"];//aqui va el rfc del provedor que desea llamar
            string ruta = Server.MapPath(@"~\Content\TabletasEntregadas.xlsx");

            Excel objExcel = new Excel();
            objExcel.cargaarexcelTableta(ruta, objProveedor.tabletasEntregadasAMP(CCT));
            return View(objProveedor.tabletasEntregadasAMP(CCT));
        }

        public PropiedadesExcel TabletasEntregadas(string id)
        {
            return new PropiedadesExcel
            {
                FileName = "TabletasEntregadas.xlsx",
                Path = @"~/Content/TabletasEntregadas.xlsx"
            };
        }

        public ActionResult eliminarMP(string id)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"]))
            {
                return RedirectToAction("Index", "Home");
            }

            objProveedor.bajaTabletasMP(id);
            return RedirectToAction("tabletaeliminarMP");
        }




        public ActionResult eliminarinstitucionMP(string cct_intELIM)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"]))
            {
                return RedirectToAction("Index", "Home");
            }
            string proveedor = (string)Session["pertenece"];

            objProveedor.eliminaInstitucionMP(cct_intELIM, proveedor);
            return RedirectToAction("tabletaeliminarMP");
        }

        public ActionResult tabletaMP(string id)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"]))
            {
                return RedirectToAction("Index", "Home");
            }
            string dato1 = Server.MapPath(@"~\Content\Tabletas_Errores.xlsx");
            string dato2 = Server.MapPath(@"~\Content\Tabletas_Formato.xlsx");
            ViewBag.Operacion = id;
            string pRFC = (string)Session["pertenece"];//aqui va el rfc del provedor que desea llamar

            Excel objExcel = new Excel();
            //a.cargaarexcelTabletaMPError(dato1, objProveedor.consultaErrores(pRFC));
            //a.cargaFormatoTabletaMP(dato2);
            //return View(objProveedor.consultaErrores(pRFC));

            List<Tableta> listatablet = new List<Tableta>();
            listatablet = objProveedor.consultaErrores(pRFC);
            InstitucionTransacciones objInstTransac = new InstitucionTransacciones();
            objProveedor.eliminaErroresTabletas(pRFC);

            objExcel.cargaarexcelTabletaMPError(dato1, listatablet);
            objExcel.cargaFormatoTabletaMP(dato2);

            return View(listatablet);
        }

        public PropiedadesExcel FormatoTabletRecepcion()
        {
            return new PropiedadesExcel
            {
                FileName = "Tabletas_Formato.xlsx",
                Path = @"~/Content/Tabletas_Formato.xlsx"
            };
        }

        public PropiedadesExcel FormatoTabletRecepcionError()
        {
            return new PropiedadesExcel
            {
                FileName = "Tabletas_Errores.xlsx",
                Path = @"~/Content/Tabletas_Errores.xlsx"
            };
        }


        [HttpPost]
        public ActionResult tabletaMP(FormCollection collection, HttpPostedFileBase file)
        {
            string CCT = (string)Session["pertenece"];//aqui va el rfc del provedor que desea llamar

            if (string.IsNullOrEmpty((string)Session["CURP"]))
            {
                return RedirectToAction("Index", "Home");
            }

            string fileLocation;
            DataSet ds = new DataSet();
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

                int errores = 0;
                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string no_Serie = ds.Tables[0].Rows[i][0].ToString();


                        objProveedor.consultaMPdeInstitucion(CCT);

                        if (objProveedor.getMedioProveedor() == false) // antes ! (Es una Institucion)
                        {
                            objProveedor.consultaSerieEInstitucion(no_Serie, CCT); // objProveedor.getSerie(); && objProveedor.getInstitucion(); && objProveedor.getCCTMedioP();

                            if (objProveedor.getSerie().Equals(no_Serie))
                            {
                                objProveedor.consultaGradoDeInstitucion(CCT); // Retorna Grado

                                objProveedor.registraTabletaNoEncontrada(CCT, objProveedor.getCCTMedioP(), no_Serie, objProveedor.getGrado());

                            }
                            else
                            {
                                objProveedor.registraErrorTabAsignada(no_Serie, CCT, objProveedor.getDependencia(), objProveedor.getInstitucion());
                                errores++;
                            }
                        }
                        else //Si es un MP 
                        {
                            objProveedor.consultaSerieMP(no_Serie, CCT); // RETORNA getSerie(reader.GetString(0));    getCCTMedioP(reader.GetString(1));    getRFC(reader.GetString(2));

                            if (objProveedor.getSerie().Equals(no_Serie))
                            {
                                objProveedor.consultaGradoDeInstitucion(CCT); // Retorna Grado

                                objProveedor.registraTabletaNoEncontradaMP(objProveedor.getRFC() , CCT, no_Serie, objProveedor.getGrado());

                            }
                            else
                            {
                                objProveedor.registraErrorTabAsignada(no_Serie, CCT, objProveedor.getDependencia(), objProveedor.getInstitucion());
                                errores++;
                            }

                        }
                    }

                    TempData["error"] = "alert-success";
                    TempData["message"] = "Archivo Insertado Error sucedidos";
                    return RedirectToAction("tableta", new { id = "Archivo Insertado" });
                }

            }
            TempData["error"] = "alert-danger";
            TempData["messaje"] = "Archivo no Aceptado";
            return RedirectToAction("tableta", new { id = "Error al Insertar archivo" });
        }

        public ActionResult EntregaMP()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"]))
            {
                return RedirectToAction("Index", "Home");
            }
            string pRFC = (string)Session["pertenece"];//aqui va el rfc del provedor que desea llamar

            return View(objProveedor.consultaTabletsEntMP(pRFC));
        }


        [HttpPost]
        public ActionResult EntregaMP(FormCollection collection, string cct_int1, DateTime fecha, int Cantidad, string responsable)
        {
            string pRFC = (string)Session["pertenece"];//aqui va el rfc del provedor que desea llamar


            objProveedor.consultaDependenciaInst(pRFC);

            if (!objProveedor.getDependencia().Equals(""))
            {
                //objProveedor.consultaTotalTabletsMP();    //retorna objProveedor.getCantidad();

                //objProveedor.consultaTabletasMPNoEntreg(pRFC);   //retorna objProveedor.getTotal();

                //objProveedor.consultaTabAEntregarMP(pRFC);   //retorna objProveedor.getCantidadProveedor();

                //int totaltable = objProveedor.getCantidadProveedor() + Cantidad;


                //if (objProveedor.getTotal() >= totaltable)
                //{
                objProveedor.registraEntregaMP(pRFC, cct_int1, fecha, Cantidad, responsable);

                TempData["error"] = "alert-success";
                TempData["message"] = "Dato Insertado Correctamete: ";
                return RedirectToAction("EntregaMP");
                //}
                //else
                //{
                //    TempData["error"] = "alert-danger";
                //    TempData["message"] = "Error al Insertar la Cantidad de tabletas" + objProveedor.getTotal() + totaltable;
                //    return RedirectToAction("EntregaMP", new { id = objProveedor.getTotal(), id2 = totaltable, id3 = "cantidad" });
                //}
            }
            else
            {
                objProveedor.consultaInstitucion(cct_int1);

                if (!objProveedor.getInstitucion().Equals(""))
                {
                    TempData["error"] = "alert-danger";
                    TempData["message"] = "Error Institucion no asignada al provedor: " + objProveedor.getNombre();
                    return RedirectToAction("EntregaMP", new { id = objProveedor.getNombre(), id3 = "cctnoa" });
                }
                else
                {
                    TempData["error"] = "alert-danger";
                    TempData["message"] = "Error Institucion no Existe: " + cct_int1;
                    return RedirectToAction("EntregaMP", new { id = cct_int1, id3 = "cctnoa1" });
                }
            }
        }

        public ActionResult EntradaModificarMP(string id, string id4)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"]))
            {
                return RedirectToAction("Index", "Home");
            }

            objProveedor.consultaFechaParaEntrMP(id4); //retorna Inst, Cantidad, Fecha

            string fechadata = objProveedor.getFecha().ToString("yyyy-MM-ddTHH:mm");
            ViewBag.fecha = fechadata;
            ViewBag.cantidad = objProveedor.getCantidad().ToString();
            ViewBag.institucion = objProveedor.getInstitucion();
            ViewBag.clave = id4;
            //ViewBag.nombre = id.ToString();
            //ViewBag.clave = id4.ToString();

            return View();
        }

        [HttpPost]
        public ActionResult EntradaModificarMP(string cct_int1, int Cantidad, DateTime fecha, int clave)
        {
            string pRFC = (string)Session["pertenece"];//aqui va el rfc del provedor que desea llamar
            DateTime fechaHoy = DateTime.Now;
            string fechadata = fecha.ToString("dd/MM/yyyy HH:mm");
            try
            {
                //  objProveedor.consultaTabletasMPNoEntreg(pRFC); // retorna objProveedor.getTotal();

                //  objProveedor.consultaCantProvYEntregaMP(pRFC, clave); //retorna objProveedor.getCantidadProveedor();

                //int totaltable = objProveedor.getCantidadProveedor() + Cantidad;

                //if (objProveedor.getTotal() >= totaltable)
                //{
                objProveedor.actualizaEntregaInstYMP(fechadata, Cantidad, cct_int1, clave);
                return RedirectToAction("Entrega");
                //}
                //else
                //{
                //    TempData["error"] = "alert-danger";
                //    TempData["message"] = "Error al Insertar la Cantidad de tabletas";
                //    return RedirectToAction("EntregaMP");
                //}
            }
            catch
            {
                TempData["error"] = "alert-danger";
                TempData["message"] = "Error al actualizar los datos";
                return View("EntregaMP");
            }
        }




        public ActionResult tabletaMP_Proveedor(string id)
        {
            //Actualiza los numeros de serie para una entrega (Esta entrega es realizada por un MP)

            if (string.IsNullOrEmpty((string)Session["CURP"]))
            {
                return RedirectToAction("Index", "Home");
            }
            string dato1 = Server.MapPath(@"~\Content\Tabletas_Formato_MP.xlsx");
            string dato2 = Server.MapPath(@"~\Content\Tabletas_Errores_MP.xlsx");
            ViewBag.Operacion = id;
            string CCT = (string)Session["pertenece"];//aqui va el rfc del provedor que desea llamar

            Excel objExcel = new Excel();
            //a.cargaarexcelTableta(dato2, objProveedor.consultaErrores(CCT));
            //a.cargaFormatoTableta(dato1);

            //return View(objProveedor.consultaErrores(CCT));



            List<Tableta> listatablet = new List<Tableta>();
            listatablet = objProveedor.consultaErrores(CCT);
            objProveedor.eliminaErroresTabletas(CCT);

            objExcel.cargaarexcelTableta(dato1, listatablet);
            objExcel.cargaFormatoTableta(dato2);

            return View(listatablet);

        }


        public PropiedadesExcel FormatoTabletEntregaMP()
        {
            return new PropiedadesExcel
            {
                FileName = "Tabletas_Formato_MP.xlsx",
                Path = @"~/Content/Tabletas_Formato_MP.xlsx"
            };
        }

        public PropiedadesExcel FormatoTabletMP()
        {
            return new PropiedadesExcel
            {
                FileName = "Tabletas_Errores_MP.xlsx",
                Path = @"~/Content/Tabletas_Errores_MP.xlsx"
            };
        }




        [HttpPost]
        public ActionResult tabletaMP_Proveedor(FormCollection collection, HttpPostedFileBase file)
        {
            //Actualiza los numeros de serie para una entrega (Esta entrega es realizada por un MP)

            string CCT_MP = (string)Session["pertenece"];//aqui va el rfc del provedor que desea llamar

            if (string.IsNullOrEmpty((string)Session["CURP"]))
            {
                return RedirectToAction("Index", "Home");
            }

            string fileLocation;
            DataSet ds = new DataSet();
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

                int errores = 0;
                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string CCT_INST = ds.Tables[0].Rows[i][0].ToString();
                        string no_Serie = ds.Tables[0].Rows[i][1].ToString();

                        objProveedor.consultaMPdeInstitucion(CCT_MP); // retorna  setMedioProveedor

                        if (objProveedor.getMedioProveedor() == true) // antes ! (Es una Institucion)
                        {
                           objProveedor.consultaDependenciaInst(CCT_INST); //RETORNA objProveedor.getDependencia();

                            if (objProveedor.getDependencia().Equals(CCT_MP))
                            {
                                objProveedor.consultaSerieMP(no_Serie, CCT_MP); // objProveedor.getSerie(); && objProveedor.getRFC(); && objProveedor.getCCTMedioP();

                                if (objProveedor.getSerie().Equals(no_Serie))
                                {
                                    objProveedor.registraInstitucionPorMP(objProveedor.getRFC(), CCT_MP, CCT_INST, no_Serie);
                                }
                                else
                                {
                                    objProveedor.registraErrorTabAsignada(no_Serie, CCT_MP, objProveedor.getDependencia(), objProveedor.getInstitucion());
                                    errores++;
                                }
                            }

                        }

                    }

                    TempData["error"] = "alert-success";
                    TempData["message"] = "Archivo Insertado Error sucedidos";
                    return RedirectToAction("tableta", new { id = "Archivo Insertado" });
                }

                TempData["error"] = "alert-danger";
                TempData["messaje"] = "Archivo no Aceptado";
                return RedirectToAction("tableta", new { id = "Error al Insertar archivo" });

            }
            return RedirectToAction("tabletaeliminarMP");
        }





    }// Fin Clase
}