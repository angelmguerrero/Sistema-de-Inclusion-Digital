using System.Linq;
using System.Net;
using System.Web.Mvc;
using Educafin.Models;
using Educafin.Clases;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Web;
using System.Collections;
using System.IO;
using System.Data.OleDb;
using System.Xml;
using Institucion.Models;
using Educafin.Clases.Transacciones;

namespace Educafin.Controllers
{

    public class AdministradorController : Controller
    {
        private Model1 db = new Model1();

        private ModeloRegistroUsuario modeluser = new ModeloRegistroUsuario();
        InstitucionTransacciones objInstitucion = new InstitucionTransacciones();
        AdministradorTransacciones objAdmin = new AdministradorTransacciones();
        Excel objExcel = new Excel();

        public ActionResult Institucion()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }

            return View(objAdmin.consultaListaDeInstituciones());
        }

        public ActionResult AsociacionInstitucionConPlanteles(string cct, string consulta, string cctI)//MP, borrar/Insert, Institucion
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (Session["CURP"] == null) { return RedirectToAction("Index", "Home"); }

            if (consulta == "borrar")
            {
                objAdmin.eliminaDependenciaCCT(cctI);
                TempData["error"] = "alert alert-success";
                TempData["message"] = "La institución se borro correctamente";
            }
            if (consulta == "insertar")
            {

                objAdmin.asignaDependenciaCCT(cct, cctI);
                TempData["error"] = "alert alert-success";
                TempData["message"] = "La institución se asocio correctamente";
            }


            ViewBag.nombre = objAdmin.consultaNombreDependencia(cct);
            ViewBag.cct = cct;

            return View(objAdmin.consultaDependencia(cct));
        }


        public ActionResult AsociarProveedor(string rfc, string cct, string cantidad, string consulta)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (consulta == "borrar")
            {
                objAdmin.borraProveedorInst(cct, rfc);
                TempData["error"] = "alert alert-success";
                TempData["message"] = "La institución se borro correctamente";
            }
            if (consulta == "insertar")
            {
                objAdmin.registraProveedorInst(cct, rfc, cantidad);
                TempData["error"] = "alert alert-success";
                TempData["message"] = "La institución se asocio correctamente";
            }

            objAdmin.consultaNombreProveedor(rfc); //RETORNA objAdmin.getNombre();
            ViewBag.nombre = objAdmin.getNombre();
            ViewBag.rfc_prov = rfc;

            return View(objAdmin.consultaEntregaProvInst(rfc));
        }


        [HttpPost]
        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return RedirectToAction("Institucion");
            }

            return View(objAdmin.consultaDatosInstitucion(id));
        }


        public ActionResult Create()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CCT,nombre,grado,estado,municipio,colonia,calle,numero,codigopostal,telefono,coreo,medioprovedor,dependencia")] institucion institucion)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    objAdmin.consultaExistenciaInstitucion(institucion.CCT); //RETORNA objAdmin.getCCTInst();

                    if (objAdmin.getCCTInst().Equals(null) || objAdmin.getCCTInst().Equals(""))
                    {
                        try
                        {
                            objAdmin.registraInstitucion(institucion.CCT, institucion.nombre, institucion.grado, institucion.estado,
                                institucion.municipio, institucion.colonia, institucion.calle,
                                institucion.numero, institucion.codigopostal, institucion.telefono, institucion.coreo,
                                Convert.ToString(institucion.medioprovedor), institucion.dependencia);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }


                        TempData["error"] = "alert alert-succes";
                        TempData["message"] = "La institución se registro correctamente";

                    }
                    else
                    {
                        TempData["error"] = "alert alert-danger";
                        TempData["message"] = "El CCT: " + institucion.CCT + " fue registrado anteriormente";
                    }

                    TempData["error"] = "alert alert-succes";
                    TempData["message"] = "La institución se registro correctamente";
                }
                catch (Exception e)
                {
                    TempData["error"] = "alert alert-danger";
                    TempData["message"] = "Error al registrar la institución";
                }
                return RedirectToAction("Institucion");
            }
            return View(institucion);
        }




        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return RedirectToAction("Institucion");
            }

            Hashtable datos = rellenaCodigoP(id);
            if (datos["found"].ToString() == "true")
            {
                ViewData["colonia"] = datos["colonia"].ToString();
            }

            return View(objAdmin.consultaDatosInstitucion(id));
        }







        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CCT,nombre,grado,estado,municipio,colonia,calle,numero,codigopostal,telefono,coreo,medioprovedor,dependencia")] institucion institucion)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                objAdmin.actualizaDatosInstitucion(institucion);

                TempData["error"] = "alert alert-success";
                TempData["message"] = "La institución se modificó correctamente";

                return RedirectToAction("Institucion");
            }
            return View(institucion);
        }


        public ActionResult Proveedor()
        {
            //DESARROLLADO POR ENTITY FRAMEWORK

            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }


            return View(objAdmin.consultaProveedoresRegistrados());


        }


        public ActionResult ProvDetalles(string id)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null || id == "")
            {
                return RedirectToAction("Proveedor");
            }

            return View(objAdmin.consultaDatosDeProveedor(id));
        }



        public ActionResult ProvNuevo()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //Registro de Proveedor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProvNuevo([Bind(Include = "RFC,nombre,representante,codigopostal,estado,municipio,colonia,calle,num_ext,num_int,telefono,correo")] proveedor proveedor)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (objAdmin.consultaRFCProveedor(proveedor.RFC).Equals(null) || objAdmin.consultaRFCProveedor(proveedor.RFC).Equals(""))
                    {

                        try
                        {
                            objAdmin.registraProveedores(proveedor.RFC, proveedor.nombre, proveedor.representante, proveedor.codigopostal,
                                proveedor.estado, proveedor.municipio, proveedor.colonia, proveedor.calle, proveedor.num_ext, proveedor.num_int,
                                proveedor.telefono, proveedor.correo);
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }

                        TempData["error"] = "alert alert-success";
                        TempData["message"] = "El proveedor se registró correctamente";
                    }
                    else
                    {
                        try
                        {
                            objAdmin.registraErrorProveedor(proveedor.RFC, proveedor.nombre);
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }

                        TempData["error"] = "alert alert-danger";
                        TempData["message"] = "El proveedor ya fue registrado anteriormente";
                    }
                }
                catch (Exception e)
                {
                    TempData["error"] = "alert alert-danger";
                    TempData["message"] = "Error al registrar la proveedor";
                }
                return RedirectToAction("Proveedor");
            }
            return View(proveedor);
        }


        public ActionResult ProvMod(string id)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }


            if (id == null || id == "")
            {
                return RedirectToAction("Proveedor");
            }

            return View(objAdmin.registraProveedor(id));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProvMod([Bind(Include = "RFC,nombre,representante,codigopostal,estado,municipio,colonia,calle,num_ext,num_int,telefono,correo")] proveedor proveedor)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }

            objAdmin.actualizaDatosProveedor(proveedor);

            return RedirectToAction("Proveedor");
        }


        public ActionResult Usuarios()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }

            return View(objAdmin.consultaUsuariosRegistrados());
        }

        public ActionResult CrearUsuario()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        private Boolean validaCurp(String curp)
        {
            return objAdmin.realizaRegistro(curp);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearUsuario([Bind(Include = "CURP,nombre,apellido_pat,apellido_mat,estado_nac,sexo,fecha_nac,codigopostal,estado_r,municipio,colonia,calle,num_int,num_ext,telefono,celular,correo,tipo,pertenece")] usuario usuario)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }

            string CURP = "";
            string ape_pat = "";
            string ape_mat = "";
            string nombre = "";
            string sexo = "";
            string fecha_nac = "";
            string lugar_nac = "";

            if (ModelState.IsValid)
            {
                CURP objCurp = new CURP();
                string[] str = objCurp.get_Curp(usuario.CURP.ToString().ToUpper()).Split(',');


                if (str.Length >= 7)
                {
                    if (validaCurp(usuario.CURP))
                    {
                        CURP = str[0];
                        ape_pat = str[1];
                        ape_mat = str[2];
                        nombre = str[3];
                        sexo = str[4];
                        fecha_nac = str[5];
                        lugar_nac = str[6];
                    }
                    else
                    {
                        TempData["error"] = "alert alert-danger";
                        TempData["message"] = "El CURP indicado ya se encuentra en el sistema";
                        return RedirectToAction("CrearUsuario");
                    }
                    string password = Metodos.generatePassword(6);
                    string subject = "Envio contraseña";
                    string contenido = "";

                    contenido += "<center><img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAisAAAE1CAYAAAAmmqBeAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAiy1JREFUeNrsXQec1NTzn+uNa3Dc0cshvQrSe1eRqoiCgoqoYEXEhqKggv4Q0L8CFlBBQUFFiqj0XgXk6FKO3g644xrX9/5vsskZlmyS3c3uJtn5fj6PPTbZ5GVemW/mzZvxKy4uBgKBQCAQCAS9wp9EQCAQCAQCQc8IJBEYBtGsNGQllJV9rFwjkRAIBAKByArBm4hk5T5W2vGlkcQ5m1nZIioZJDYCgUAgGAH+/uoXd/zIZ0V3uJOV51l53MHfnWblPZ60HCMxEggEAoHICkFrBLAyi5URGlxrDSv/x8omVtJJtAQCgUAgskJwFR14olJP4+ue4UkLWlt2kZgJBAKBQGSF4AxGsjLTA/fZzcr7rGwAsrYQCAQCgcgKQSVGsTLDw/e8wZOjtWC1uORTMxAIBAKByApBCnez8qeX63CUlXE8cSFrC4FAIBCIrBBKgNuS97NSTe0PMm/mwqHT5yGvoBAaJlaG0pERWtdpDitLwLodmogLgUAgEIis+Dh+YGWImhN/37EPvly+Drbs/xcsorby8/OD9554AB7r2R4iw0O1rBs65b4G1iWiC9RUBAKBQCCy4nvASLQ3lE66lHoDRk3/Dtb9c1jxgjUqxMNXY4ZD01rVwJ+RGA2xlJXZQNYWAoFAIBBZ8Sl8x8owuRMOnb4AD7zzKVy8fsPhiz/fvzuMHngPlIkqpWWd0dryP7BaW/ZTExIIBAKByIp5oWhVuZF1E1qNepezrLiCuOhImP3KcGjfqA4EBmiarxIJyxRWNgJZWwgEAoFAZMV0eICVn+VOuPvV/8H2wyc0vemDnVrChMcGQIW4WC0vi/4suAV6E09gCAQCgUAgsmJw4LpMEiuJ9k44fzUV6j/+utsqEBYcBDNHPwa9Wt0JIUGa5rDEpaF3WVkHZG0hEAgEApEVw0IxrkqvN6bClgP/eqQynZrUhWmjhkBi+bLcziKNUAhWaws+J1pbsqjZCQQCgUBkxTj4lJUX7B3MyS+AcgOe9UrFpo4cDIO7tobw0BAtL5sM1i3QGHAujZqfQCAQCERW9A10rL3Git21lx/XbYdnpn3r1UpisLmZLw7jPv203QL9IysLgLZAEwgEAoHIim7Rj5Xf5E4o3ecZKLJYdFPh1x6+D57t1w2iI8K1vCxugZ4I1iWiY9QtCAQCgcgKkRX9AB1rG9k7eC7lOjR44g1dVrxiXGn4+pXh0KpeDQjw13QL9BpW/g+su4nI2kIgEAhEVoiseBHtwLr8YReD358JK3bs0/2DPHZ3Bxj3SB+Ij4nS8rJneNKC1pZd1F0IBAKByAqRFc8D9yJPtnewoLAI4vqNNNQDRUWEwZcvPwHdmtWH4EBNt0DvZuV9VjYAWVsIBAKByAqRFY8AHWtxCaiqvRNW/n0AHpzwmWEfsFerJvDhiEFQJaGMlpfF8L24BRp3EqHFJZ+6EoFAIBBZIbgHfcCaDNAuKg96ETKycwz/oOjP8tkLQ2FAh+Zc8DkNcZSVcTxxIWsLgUAgEFkhaIwNrHS0dzDlRgbUfOQVz3cQPz9oVe8OyLiZAwdPndf8+i3r1oBPn38U6lQur/UW6DmsLAHaAk0gEAhEVgiaoD4rB+VOGDNrAcxescHjFVs4/jm4u4V1c9LkBcvhQ1bchfeeeAAev7sDRIaHanlZdMrFgHO4RHSBuhqBQCAQWSE4h6dZ+cLeQYypgrFVpPDJc49At6b14IfV22Daz39BfmGhZpWqHF8GDn7zn7/vlbQMqPWo+607NSrEw1djhkPTWtU4y46GwGW22UDWFgKBQDAtWfEncbkF6FgrGzhl19Fku8f6tbkTykZHwugHesKZn6bB75NehrYNamlSsdSMLMjNLyj5/8VrnomEf/JiCnQdMxliez8Nb835Ga5naJY2qC8raBpCR+ZRIBPPhkAgEAjGBFlW3IP7eAVqFw2HvwFnr1y/7fverZvA7FeGS/4GfUx+2fg3TFqwHDJv5jpduR53NYTRA++GrJxcRhx+gX/PXfKKkOIYIcNnbd+oDgQGaMqbcXloCisbgawtBAKBoEvQMpD38SsrA+wdxN0/uAtICuunvQH1qlaQvTi22JEzF2Haz3/C8u37TCGwBzu1hAmPDYAKcbFaXhb9WXAL9CaewBAIBAKByAqBoRwrsqaK//20Aj744fYdzeiEevz7KeCIR0dOXj6s2JkEE+cthStpxjci4LbnmaMfg16t7oSQIE0Dzu1n5V1W1gFZWwgEAoHIio9jICuL7B1Eecf0flry2OQnB8IT93Rw+sanLl+DmUvXwLxVW00hyE5N6sK0UUMgsXxZLbdAo7cyWlv+BKu1JYu6LIFAIBBZ8SWUAqujZ6K9Ew6dPg9tnpsoeez4vP9xoexdBYbwX7/vCEyYuwROXLxiCsFOHTkYBndtDeGhIVpeFr2ccQs0BpxLo+5LIBAIRFZ8AYqOtR1f+gD2nThz2/et690BS957UfMKXU5Nhzl/bYKZv62BQovF8AJumFgZZr44jPvUOODcj6wsANoCTSAQCERWTI5ZrDxj7yD6l5S7/znJY7g9uXnt6m6rWJGlGHYcOQHvf78c9h4/ZQphv/bwffBsv24QHRGu5WWRSaLpC5eIjlGXJhAIBCIrZgLGVrkhd8J3f22CFz//4bbvMXMxxlPROFiaXaRlZsOP63dyjr5IoDyBsJBgt92rYlxp+PqV4dCqXg0uR5GGWMPK/4F1NxFZWwgEAoHIiuHRn5XF9g6inMv0HclFrr3NQvBQL3h54N0erzDWaX/yefiIkZa1ew+55R6hwUGwespr0KhGFdhy4F/O+Xfp1j23BKbTEo/d3QHGPdIH4mOitLzsGZ60oLVlF3V1AoFAILJiRASA1bG2vr0TTl++Co2fHCd57MCcSUy5Rnr1AbJy82Dplr3w/vxlXJRbrdCYkZRNn751y3fp2Tdh4fqdjLhsgQPJ59zyPOio/OXLT0C3ZvU5y5WG2M3K+2BNUknWFgKBQCCyYhh05JWXXTzw7v/B6t235zWsX60irJv6uq4e5vj5K/Dp4lXw80bXjQhIFH6Z8AJ0bFxH8jg6G89duYXda6dLUXnl0KtVE/hwxCCoklBGy8vikh9ugcadRGhxyadhQCAQCERW9IzxrEywdxATEZbtN0ry2E9vj4LOTerq8qHyCgph1Z6DMOG7JXDu6nWXrtWsVnUY1rMd3N+hOZQKuz0DM/qz/LZlD3y/agtsO3TcLc+D/iyfvTAUBrA6YPA5DXGUlXE8cSFrC4FAIBBZ0R3QsfZfVhLsnbB82154ZNIXkg117qfpWufFcQvOXU2Fr3/fAF//sREsLmyBxjgp97e/Cx7t0Q5a1q0hec7x85e5JaIf122Hqzcy3fI8eO9Pn38U6lQur/UW6DmsLAHaAk0gEAhEVnSEvrxysosKDzwP2bl5t30/olcneP+J+w31sIVFFth88Bi8N28JHDp9waVr1alSniMtD3dpDWWiSt12HIPb/bUrCeYy4rJ2zyGwuKmvvvfEA/D43R24dAcaAp1yMeAcLhFdoGFCIBAIRFa8iW2stLZ3EIOy1R46VvLY7lkToHJ8acM++NX0TPhh9TaY9vNf3FKXs0C/lntbNYZhPdtzS2JSlo4L11K5e32/eiucS7nuluepUSEevhozHJrWqqb1NnJMBDUbyNpCIBAIRFa8gDtZ2St3wnOfzuUUrC0qlo2FvV9MNIUQ0OKx59hpmLzgd9h60LU4apXjy8Cj3dvCI93bcPFTbIH9FVMJzF25Gf7YkeQSSZLD8/27w+iB90hafFwAWlv+B1Zry34aPgQCgcgKkRVP4Hmwxt+QBC6ZlOkrHdAWg5j1ad3EdALJuJkDv2z8GyYtWO7S7h60bHRtVh+G9WgHd7doDEGBAbedcz0ji/NrQafco2cvueV54qIjYTZrq/aN6mjtW4SEZQorG4GsLQQCgcgKkRU3AR1rMbZKVXsnbN7/L9z35lTpV+yfpkFoUJBphYO96siZizDt5z9h+fZ9Ll2rbEwk59cylBGXmpXKSZ6z88hJ+GzxKnavf9z2TA92agkTHhsAFeJitbws+rPgFuj1rGynYUUgEIisEFnREr1ZWSZ3Qp2hr8Kl1Nsj8A/ocBfMenGYzwgKtyWv2JkEE+cthStprhkR2tSvyTnl9m/XjAvhb4u2z0+Eg6fOu/V5cNvzzNGPQa9Wd0JIkKYB55BpjUTuRcOLQCAQWSGyogX+YOUeewcx/061h0dLHtv0yTioXbmcTwrt1OVrMHPpGi7svivAnTsDO7bkYrc0ueM/41ab5ya4vEvJEXRqUhemjRoCieXLarkF+keetNDyEIFAILJCZMVpYHrkZLkTJs5bAlMX/XHb97GlwuHo3I98XoC4LRkdZSfMXQInLl5x6VoNEytzUWoPJp+D33fs89ozTR05GAZ3bc3FktEAZ3nC8gcNNwKBQGSFyIozeJSVefYO4u6Y2N5P21FoD8Mj3dqQBEXA7d1z/toEM39bA4UuBJzTC5A8zXxxGPfporUFsz12AaszLoFAIBBZIaiGomMt5rvp+NIHksdO/DAFIsNCSYoSKLIUw44jJ+D975fD3uOnTPFMrz18HzzbrxtER4Q7ewl0emrEyjnqIQQCgcgKQS3uY2W53AktR70juZUWk/ktGv8sSVAF0Ofnx/U74X8/reAcdI0OjBmD29Vb1avB5ShyECv4fkcgEAhEVgiqMJeVofYOYlh9DK8vhb8+Ggt33lGFJOgAsH/uTz4PHzHSsnbvIVM802N3d4Bxj/SB+JgoR36Gu89+px5BIBCIrBCUgEtAN+RO+HL5Onj1y59u+z40OBBOL5imddI8n0IWI4JLt+yF9+cvg9SMLMM/T2xkBCx4axS3HVsFNrHSkXoBgUAgskJQAmYd/EXOChBjx7H27Uf7wnP9upEENcLx81fg08Wr4OeNuwz/LJgB+rf3XoII5V1EfUBhCZJAIBDMSFb8SVyqEcbKZLkTTlywvwX34S6tSIIaomalBPj8hUfh7E/TYfbY4VC5bBnDPgtG3x06+QtQ8eLQjlqeQCD4JLEhEagGKgpZe/1Ln/8g+X2jxMpaJ8Qj8MAIsr1bNYHdX7zLygR4+r7ODrF1vWDNnkOSCS+JrBAIBAItAzkCjOT2qr2DufkFkDBAeqfPL+8+D+0b1iIJegiYQHLzwWPw3rwlHo1m6yow4/T+OZO4JI4ywPj+RdTKBALB6KBlIO2BjrWj5E5YYSdyKjZGm/p3kAQ9CMyO3LlxHVg39XU4+M0keP3h+yA4MFD39T6Xcp1bElIARRQkEAi+R2xIBKrQmRXZdZxnpn0r+f2oPl2ciatB0AhloyNh9AM9uSzXv096Gdo20LeFa/uh40qntKdWJRAIPvcSSiJQhfFyBy9cS4X8wkLJY0/cQ7tNdcHK/fygee3qsHjC85BxMwd+2fg3TFqwHDJv5uqqniqWrRpSaxIIBJ+bw0kEimjNyp1yJ7w151fJ76uVKwsV42JIgjpDVHgYI5Ed4Pj3U2D9tDegd+smuqlbJiNSStWnFiQQCL4GsqwoQ3YHBmYPXrz5b8lj7w7rR9LTMdCNtV7VCjD7leFcSP8VO5Ng4rylcCUt3Wt1QudgGrMEAoFAE58jQMda2WQ+mw/8a/dY16b1SIIGQVhIMDzQoTlXTl2+BjOXroF5q7Z6vB53VExQOuUYtRaBQPA10DKQPDqATHZlxBP/+1ry+4e7tDbEDhTC7aheLg6mPP0QnF/4CXz/xtNwR4UEj917cNfWSqdsphYiEAi+BtKm8nhB7uD1jCwuO7DkDwd0J+kZHEGBAdDjrgZcuZyaDnP+2gQzf1sDhRaL2+7ZoHplpVO2UMsQCARfA1lW7AP3uMom85m66A/J7+OiIyGxfFmSoIlQrnQ0jBvcG84u/AQWT3wBmtasrvk9+rZtxsWIkUEyKxepNQgEgq+BLCv2IetYaykuhhlL1kgeG/9oX5KeSRHg7wdt69eEPz98mbOq/bh+J/zvpxWcg66rmPTkQKVTFlMLEAgEXwRZVqSBjrWysVX2/HvK7rH7Wt9JEvQBxEZGcEH/Ts3/GFb971Xo2rS+09dC/6ZKZUsrnUZLQAQCwSdBlhVpYJRQWcfap6d9I/l9j7saQkRoMEnQh+Dn5weNa1SGBeOegazcPFi6ZS+8P38ZpGZkqb7GjJeGKZ2CAVg2kLQJBIIvgiwr0nhU7iBGPT15MUXy2KsP3UvS82GUCg2BId1aw5FvJ8OWT9+CgR1bqPpdnzZNlU6ZyUo6SZhAIBBZISDiWHlQ7oRv/two+X0EU1QNqlUiCRI41KyUAJ+/8Cic/Wk6zHjRvuWkXYNaEBocpHS5lSRRAoFAZIUgoJPcweLiYhj/rXR4/dcevg/8/EiAhFuBRKR6Ofu7w6Y9O0TpEpjdkPxVCASCz4J8Vm4FZlb+SO6Eo+cu2T02qFMLkiDhFmDG7aCgIJixdI3dc2pVKqd0mTfA6rNCIBAIRFYI3HblRLkTnvt0ruT3d9WuDjGlwkmChBIEBQZCQEAAXEnLgJV/75c8Z9KTD3IOugpYQ9IkEAhEVggC7pE7mJNfALvtbFl+m2KrEHgg+UBrij9PQhas2WY3QeGwnu2ULjcPyLGWQCAQWSHwwNgqo+ROWLxJOrtyoL8/tKidSBIklCz73MI2Vkm7m+DyT6mwUKVL/kxSJRAIRFYIArooyeP5/5sn/f2AHuDv75pnrT9Tclxhb+P46S2gA7G4YKReixtz4ZgJwrKPGFsOHoPkS9Lb3L8aM1zpkmeAEhcSCAQCkRUR3pU7ePbKdSiyo7Qfv7u9pgrOm8AlDCkfCiQsWFAGSGIIt8pMvOwjxryV9jfxNL6jitKl3wFaAiIQCAQiKzzQcaCR3Amvf71Q8ns05SfERjl8Q7SeIFHxM8heZ8HyE8gTFyQtRUVFPt9xpJZ9BGRk58DSrXskj730wN2S5MYGtF2ZQCAQiKzcQlbsIr+wEFbs2Cf96ju0n1M3NBJRsUtcAgKgkBEWXyUtSlaxhet3QG5+geSxF+/vqXR5DAJ3koYmgUAgEFlBKDrWrtlzyO6xjo3r+BRREYNb/kCFzYhLASN0vrI8JLfsI8ZcO4618TFRUDoyQuk2M2hoEggEAv+STCKAjqxUljvBXtLCx+/uwJS14/4mevJR0aQTMbISEhxsuueSbDv2rMHsWZWIStLJs3Ag+ZzksTljn1S6DTrWbqKhSSAQCFaQZQVgrNzBlBsZnO+BFJ7p09kpxW5WCBajwsJCcw4W9nyBKgnZ3JX2N/G0aVBL6eefADnWEggEApEVHuhUK+uvMnn+csnvE2KjoVpCnONkxeTJgwRlbibConbZRwAGD/x54y7JYw92aslkpEhYybGWQCAQiKyUQJao4I4XexmW3x3W36kbKllWViQNhgtp3gutERVWBSJDq/KfVaBSbHuIi2zos4QFl30CGVFxhGIu2bLbrjVu4hP3K/1811cbKu+mqck38FSncyQEAoHIiizQsfZVuRN2HrG/GeOelo1MKZSMnLNcuZDGa06YzBGXOuWHQGLZ+7i/1RKWYn6Ls2EHhwPLPmLYi60SFhwE5UvHKP18Ek1LBAKBYPOi78PPjpHcqsqd8OSUOZLf927dhFM8vgIkL7uSJ8Nve+6FfWdnQl6hOneKQIPuesI6oxOtM0TlxIUrsP3wCcljX455QunnN1jZQNMSgUAgEFkp4SJyB9Ozb8KFa6mSx8YMvNcnBYYkRSAt1zIPqFL6SFiMBLW7fewB8wDZ28J9d4vGSj+f+dWGyuRYSyAQCERWOFRkRTZN8qylayW/jwwPhTpVy/t0p0FLy2JGWI5cnK9K+RtlBxQSqyAH/VPEwMzKP67dLnmsU5O6EBKkSNzW0pREIBAIRFYEyDrW4pvx5AXSu4DGDekDftRvOGw+9roqwhKo8/grriz7iPHnriRuq7sUpj87ROnnR4F2AREIBAKRFR7oWPuR3AkHT523e+z+DndRrxEBl4WUloSEbNJ6hKvLPmLIJS2sXq6s0s/HfbWhcj71KAKBQCCyglB0rB35yXeS37dtUAuiwsOo14iAfiybjr2mTAp0aF1xddlHjIvX0mDtXum0DGhVUeFoTEtABAKBQGSlBH3kDt7MzbMbJv3Nwb2px0gALStKy0F68lvRatlHjPlrttndpv1Q51ZKP59DjrUEAoEg83LpY8+LS0Aj5E74af0Oye+D2Vt401pVqcfYQdK5mVC3whBZgoDF28kOnQnypgR8ph8YWRFbT4TnbJhYGcJDQ5QuscQTz/5Up3O3PTYjSabNPmn7vJ54Vl+TsXiI2w4LDa/l6vXc8Xx6qBORFROjq5LSGT1D2kIw5sF7TB8q3xXgDqHkq79zgePsAa0rRUVFrs8arB0EPxj8tF1isbB2xIB0Fiz4N08cnA3ypqY+SbM/uJW8nTwLN7JucmRFAZi0UPOQxUxpIkNqxxdc+oxlpanEeXvZRzIre7AeTLFulVO8zsJWYWt5beH67Jp1bZ65usSzoqzRuWgL+81lDeTclr9XM1bKgYTzPjvnBi/fZOH+7N6nFBShU7xZpZJ19dq2fQuf3TbaofDMe0Qyv2Hn2jGia7UB6Q0QjlxPCyi2q6hOyaI6eapdfQ5+3n7T9eRLNeoQVurbO+HUpavQZMQ4yWMHv5kEZaMjXa5EMOaYkVkS8Xa4fVeAlpX2tT506z2wvzoaaE7o4zoNUPcQU1wLNSIoqKwHstKNn2idnRi38BNtOw2fcwv8t9upncbXRlwAa0gCR3AQeQ4r89QuwzEZl+NlPJB/Bj8X6ruGlRoaymKLqLTTWM7Cddu60LdK5M0KRtUcxcuxgQvXW8Nf84iLz2eUdsVimiVjR9wDfImsdASF6KD93/4E1v1z+Lbv61erCOumvq5JJfDN3tFAaUhekMS4gl6NF0DFWOscg06x1zL3w9XMA3D00nzOKqIFMBT/Qy230iuAY5jKyodMWV5zgaT0ZB/o5dyZxOkU0Lr1OmuDnxRkjMq1D4nLZdxkJVzja65nZSQr/zr4OyO26xz+WQt8iaz4koOt7BJQXkGhJFFBvD20n2aVsOiAHIYERnPEpUmVURy5aJH4hibXFfIKERzCGFauMmU4m5WaDpKU2qysY3/+RUTFJaAz2o9Mlp9JyDielV95GRNR0Qbhbrgm9v/9vBJXg3hWjNquw/ln7eRLncZXyEo0z57t4q9dSXaZX/sGtbQjKzpM7IekRavlm8zcMzQVuzABMcWoarJl5z3BT1hEUrTDc2LCwv7uz8t4AInGEAhmZSYrP7Ait2Zvhnatw8ossC6nEVkxEZCBlpE7YeT07yS/H3FvRwgM0FZMWjiZag30N5FzjlWLqypyBhHsIhQnW6Ykt7DSUIaooEKdw0/OBDcQFlbeZ38vZiWBRGI4DOEVuRTM1K51ZJ6TyIpB8abcwUupNyA7N0+arNzXSfPKFBQWgh59hRLje9E0pw+gE+N+3noiJilVWFmBCpVE5F7Cwso4EoPhCYt4Wa8KKytM2K5okQ3xhQb1BbKC8fFbyJ0w4bvfJL+vXLYMK6XdUik9EhbBAZegG8xh5GSM6P/4FnUviYVAUE06H/OBsdPOFxrTF+KsyDYklyl3nXSm3Hce6+e2SqHvSl5+PgQFBuomFD063hoF6MiLgehwp5StU29cZEOoxIhXnfJDuB1KWgKj9Z5n98wvTOeWvPIKb0jmRsI6hATGcPePDK0CZdn/nSSDHzPCgp/VHJ1shbpeSNsiW8+o0KrcJy4Dai0vs/cLIb7QNa4vpEuGHcBxFRfZiLtnxdh2miy3ehvivoU7C/HZpZ4Z+70aeeP1UI44puRkaB1H7RwZS0hSmjk6dgzWru2+2lDZK+k6+LnJIzD71mXUvug5azf07Kako9B73DTJY2d/mg4hQe7nc+jEi/FX7EHrrctyA/SnnW1dug/uLEKHXQFsEGkqK3wOnNAwgaIz9XFWIeEWb5y8XNnthJNbYvx9UJdN3qg43QVMfYAK25m6ou9S48qjuGdVK2M1wL6HbWeWfoEyPsL6xDUnfLRQuaGMUdZaxlWylfG+szM1a0NBXvi8O9k1Ha0zKvL2tT+87YUIr7Pv7CyHr4cyxE0BWluDjdCuEljLxlM3I5IVR7Yum92y0gEUkhYOnzJb8vuBHVt4hKjoCUYIRrdTRZZnMYTJ2hnFJLylK+U9Ugt8Q+MmQ1asW8dHajrZYvttPva6S4QK65ac8jv3xmgkeKpf4D0wcec1FxzJsX2wnZKvruAsXkaBK+QHyS/2z16Nfywh6ngtvKazMkRCgO2nRegFX25Xo8Ds2lh2G2hqZjak3MiQPPbigB4+1xlQMbuKsm60GAiTiqPASdHRZRgt30rtEQssONk2ZsXVJTgt62vP7K1neKJfIJFDZeRLLwclcwPrX7ZLPc70qxVJD0O3+rMcJpdy/T6YjR1XrWS+2q5GgpkdbDEvyD1yJ3y2eJXk96WjSkHNSr61Y9HVN3IBkaH6TPZ45OICBybUwW4lKraT7W977nVp4sa281R9zQa1/QJlrKVCMxpcJSq24+uahiEOdjmxLEXtSmRFT5B1rMVIstN+/lPy2LghvX2mAyBBWXNopCZLHWgd0KuDpprJzGpaftjjb0bCfZ2ZwJHsaLVM5YtQ09YkY/0Dl15o7BBZMSLQpv6e3AlJJ+xbEfq1bWY6gQhe9lhw/RgH6mL2Ro8Otfh/LYDOo3p+M5QjA3h8zeFnNH3jc/yN0zHCorUTrK9aDORkjuOFZGwAspLi2BxmtnZ9pssFP7O3sVl9VnARWnY94pnp30h+3/nOulAqzHwxdjwxMHH7nr4Vk32nN2ctG+4gLP2b/aFoocJzN/9L5mt39gtOxrREYBjS6ci5ZmrX4uJiJCp+jLDAF+sqmnZ7r1ktKwPlDmK02qNnL0kee/3h+2jkOwFUrkaNH7FLI2c/rSZdNRMpmq+18iMg2JcxJeY0DtSOYbO1q8Vi8eN1uZ+ZLSxmJCu4BDRU7oTv/pJepw4LCYbGiZVp1DsBjC+gd2CQNqkJztntk+6CNfbETFlCk6SzOhsZUv2CZGw8qNkubMZ25S0rAWDyiPRmfLhuCg0Lb85eJHns1UH3gp+fH416B4FxEzAQkhHqaYudOl23ltsqSlYV9/cLkrE5YcZ2LSoqQj2OLh2mVl5mIythrMhqn2PnL9s9NqhzKxrNDr+VRkOHWh/pvp5SS1TX7IT21sdbYrrdnQpaOUQTwO7SJcnYnDBju1osFtTjQbw+p2UggwA9PGvKnfDyDGkF0KRGFSgTFUGj2UFg9Mg4NweC0wJ1Kww23MQlVT9ca9eLf40ZINUvSMZy8hqiqW8aXg/nEE+EPDBru4osKwFmJitm2w0kuwSUm18AWw4ekzw2flg/IDgGzM2htPxjmzvC1UirOKk56hyHk6FUlFItYyxYExZWhczcM5o57+HEikVMBrW0BAm5iuJKNWT1Puty7iMxlHJR6blfaG1twzHiDRljVFct83SJ8ymtSHI9yrH4ekiAMDiiO5dozNKutrhy5cpesxMVs5EVdKyV9fJcvu0faSH4+0OrujWIfTigGLrV+8IrFhUkSBgASi3RsJewTsimqoXCx3uISRtOilqFE8fMtmI548SoBaQSy+FzuDvNgN77hZYyRoWD9RJbDYwsY3EfR4uUK8ofZSKWP7ebkBFndwZpM2u7JiUl7YP/loDIsmIAdGallNwJoz75TvL7Z/p0gQB/UztSa/YmjnlscNJyNZeNq4opsWwvLlQ6Tpi2pAOVeyU2ocilpj+vwVsWykAqJoo1S2ojTWK32P7+qgYECNsPZWjvbRyfy4hxKLToF1rJGMkg5sCxJ2PMSeRqJnVPAvu0eMzHlWrksnxsgVYKd8Ks7frqq68mgcn9VcxGVsbLvqFeTYX8wkLJY0/26khMROEtCCd4HKh6CaePk6fW6eEdBU5a9uSBEztmmP1pR1uXLDi2v8VlJlfbUilLLZIZvK8R3/616BeuZszFtkerlVI9sR2MImPbfu7qPBAs8bLj7rnFjO3677//rkxOTi4wO1EBMI+DbWtW7pQ74e1vfpVmyuXLQvnSMcRIRBMGDjjB8W1Asz/goZZbubcGveb9cQYX0ra4XSkKlijX6nmrBcjVdXEknWqsYkbYiu4uuGoNU5tF29sWSkcQGart2C/rhSVks7Vramrq5TZt2szwlXFpFsuKbJz3gsIiWLz5b8lj7wzt7xMN7ajTFkGJrKhLLYDLDrtAP2/PlVT2AZxssb9Qunv3y5i2SVO7OoqUlJRrU6dO/SEtLS2P/beYL6aGGSwrSGGflTthw74jdo91aVrPq5UPCgyE4KAg03Soq1ev+sTEpfbNUG/buh2pj95zPekVjsjYCNv+Cfpq17Vr1x5KTEz8dMqUKQfZf4v4YjE7YTEDWUGHE9mkhSOmzpH8fki3NhAcGOAdwfv7Q0hwMAQEBJiqQ5UtW9YnJi61yzF6y0HiSH202j3ha3DERymfouRSuzqIrl271j99+vTz77zzTiLeyoasUCJDHeMl2Tf9G5mQlpkteey5ft28Vmm0qJgltP/hw4d9buJS6/OiNxO/I8s6ySm0POFuGZ+nZTZqVycQFxcX//LLLz+RkJBQICIsZFnRMeqCdcuyXXy8aIV0Y0dHcs61RFScx9mzZ2H69OmGtKa4usSBJERp8tIiaZqtn5Gr5mWMRaIGvpwbx1UZ49ZptcqPIuVSuzqLyMjISnv37h3Lk5VCsxMWo5MVWY1TZLHAF8vWSR57d5j3HGuNvvRz6NAhmD17Nu7v5xJD+srSjy0wFom9ZRVU9GsOjXRZ4dvuKpDKEOzo26FSlml8JiNuW1atdI4cuSwvc9dlrGRRw36x08Qy1iPM2K7ly5cf0LNnTyQpaGExtd+KkckKzuLj5E7Yc+y03WO9WjX2jsANGHwOCQku9SxcuBDGjBkDkyZNgq1bt0JUdDQ0btzYkJ2nkgY7o1CpY4hw26ibOKlhQDgtdtLYvg1q4fSKRMQeYcE3wj/2P2xqq0qdOnXKyR3XQsab/33dbvujbPE4WVU8C7O268yZM+/iLSumXgYy8tZl1DbyjrUfSzvW3t2iIYSHBHuHrBho+WfJkiUcSTl+/Dj4BwRwFiEsZcqUgZCQEAiPiIDatWsbsvMgCUCrhatKmZugjr0OSedmap4bSIpUaRXrBgkL+t1gxFcMc44TMP7fneHO9QKlJVgtYoBgv8BIphhzo275IVx/w+/QDwj7it4cr30BZm3X8uXLt2Aff/KWFfOOW3xrNih+ZuUBu2+92TlQedCLksfWfPwaNKxeySuVxm3KctYVHAieim1hDQtvfx0WScpXX33F1RcLEpVAVn/cxRQeHs6VESNGyN7D1eRpSpDL8aL4lsRIhp6VMxITDMjnaZlqAaW4PnruF4i5WxoYzrpkO57dlatGuI+WbYiJLa0WycFukYWZ2tUWTIevY3Nzd/zzi3UVParQbROSOvzy7sBKg1GXgeLliApizh8bJb+PCA2BBl4iKkYDWk8qVKjAlYoVK0KlSpWgSuXKUK1aNajMPvH/Rga+GekZdezUz5ejy2qFrKws2S1saG0imA9mbFc/P79ibxAVT8OoZKWD3EG0Fr07d7HksdcH32f+JAoaITQ0FKpUqVJSqjKSUrVqVY6kIHkpX768oZ8PTbh6jeqLS1T2SAku3RBcQ05OznZZpUYyNidZMWm7eoOotH3tcSIrCsDMyh/JnXDk7EW7xwZ1akkjVq3CDAmB8rxlhSMo7LNcuXKQkJDA7QCKi4vT/TMoLXO2VEjq5y3I5SHRQxJHE/RtWfs1ydicoHY1JlExKllBl+5EuROe/WSu5Pct6tSA6Igw6mkqERQUBPHx8RxBwU8kJ+hcGxsbCzExMRAVFWV4soLWlSYuJht0x4SqVCe9kiyjIDw8HLd6XpA7p0mVkSQoE4La1XhExahkpbfcwZz8Ath7/LTksbce6UM9zUGyEsfICRKT6OhojpxgKVWqFEREREBYmP6Jn5rge+iMqZccLVwa+lofKp6H9VVzHsHOxOfvjyz2Z1dJI8F4MGO7PtPlgke8G7xFVIxIVtAuLtvLft6wU/L7QH9/aF6nOo1UB4C7f5CQCAV9WHBpCEtwcDAEBup/5ztaVtTseOvV+EevExYkKlgPtduTue2T5GzrFCwWi19mZuYXSufhchwlGzQfzNSubH5DouLnbsLiTaJiRLLSRemElz7/Qfr7gT0NFeNET4QFSYn4E7ebocXCCCkDmFKCoqIiVUShW70vvDaBCUTF0fujdYXe/h1HYWGhf3R09En25xw1/cKe/xDBmDBTuyLx5nW52wiLt4mKEcnKu3IHz1y5xoXYl8KwHpTu3hmrBCp6i8Vi5IGs+hnQooGEwdMOeMJ9nSVKuIzVrf4szQLG+QpZQS5+/fr1j9lnrpr2IcJiLpilXXnLSoC79LkeiIrRyAqyjUZyJ4z94kfJ72tXKQ/xMVE0Op1Q9GxSBwMHDuSIiiOEy2rhWMARAE8ArSL9m/3hskUnsex93HWw3q5OvkjWzL68lJ+fj5N7QNmyZU+wvjFG6Xxsn4dabaUlIZPBDO3K5jfU47gmr7lVRS9ExWhkpZPs5MOU6sq/pXMyjH+kL41KJ8lKQUEB92lUwoJkSyArjjwDkgiMHusupS1EpdSCXIiJFtZ7WLuDnKUF6652EhZ2RWGdsESGVnH5+fSMvLy8QOFtNDAwcBb73KpGvgM0IoSCjIj8eB9Gb1c2t6EeD+L1uWl9HYySG0jRsXb17oP2WU6TOjQiXbCsoLI3gjOtPcsKkhR0CHYUaCZGn5DGlUfB0UvzuYyrruT+wIkQI2gKOUXcCbS0YBGA9ca8RbfXKUayLmZPssdIeAA//+XzX+F+1v1qiSwSQUzVkHR2psPh27FfYXRivI5W4eUJrsOo7SqyrGCfLjSjVQVhlNxAuOd4qdwJmAcI8wHZ4ol7OsDkJwfqhx3yjqpGwLVr1zhlj1uVcScQOtcaDRcvXuR2L+FWayQsrma9RiV+Pm0zU/xnOQKQV3hDUrHjxIWJDa2fVbgkalpbG/C+eH97hMNZ4HP9tLOtS6QMrTt6xrJlyx7s16/fOvZnNhKWA83fstTfObE/+3uxo9dCEottcZUVqUSWKI+4yEZcX8DMv2ISSdAvjNKuJ0+e/LxmzZrvsz+zWMn9Yl3FIleu5whJYWPGpbo7Mh8b5XV5rNzBK2kZkkQF8UzvLrp6EIuBllOysrI4Ra92+69enwHJoVZOwkgK9GK638llTt5cYklBM7YWTrauJr/DCVznKB40aNB+RmADGREP6NChg1/9hRP9DrUc/xubfF9hxz925GK2ViyCOWCUdk1KStoH/y0BOb0MpDdLihHJCs58slt5PvhhieT35cvEQNWEMvoiKwbaWZOZmckFhjMqUUlPT7cr75y8fBj64ZfwwfCBUKtSOV3W/59//km5884749W+BWKxOscOdnqSxUzUeB1XYIBdSX45OTlHJd4St7APLA4TFgLBW3j11VeTwAV/Fb2TFAFGcLCVJSq4VXnuyi2Sx94Z2k+XD6Qm7oe3cf78eUM71io9w29b9nB+Tm2emwDzVm3RVb0PHjyY2rFjxzX+/v4OdxS0tKw5NJJbxkELiVrfEzRx4zo7rte7Cledc70817wuIipXSBWaD3l5eTfM8iz//vvvyuTk5AJniAqSFKMQFYTeLSvoWPuq3AnbD5+we+yeFvo0RxcUFpYEVtMjbty4gQrT0FmVjx49yiVfxDQBUhjctTVX9IT8/HzL5MmTD06YMAHf+rMCAwNrO3stJB/7zs7kirC2juvqiLL8MpbV5ya9ZG1eK5hoSSQBuxIr5KFvEqSmpl6+//77X164cOGT8fHxXYz+LG3atJlhVkuK0chKB1aqyp3w1MffSH7ft21TCA0O0u2DIWEJCgzUHWFBp9qdO3dyyz9GiVIrhTp1jKVfli1bdva99947smfPnqvsvzeRSwQHB+dr8ibJCAlaXAT/FncC/XlMFpwOO9LR4uLiPDYWGpO6NzZKly5dbsWKFZ+sXr16QmJi4mI2TzzD5roGRnuOlJSUa1OnTv0hLS0tj/23mC+mJClGISvDZS0AWTfhwrVUyWMvD7xb1w+GyxN5+fkcYdHLLptdu3bBhg0buCzLFStWvCW0PsEtxDB30aJFpz/66KNj586dywRrJFXcnYJe/elakRVPArdlmxB12Hj99ubNm5+GhYWNDgwMpOAoBkZ4eHh83759Z1y5cuX/QkJC2jPF/0BMTMwLRmnXdevWHezTp89PrD+eY/8t4ovFHmExOkkxAlmpiAYSuRNmLl0j+X10RBjUrmyMJQy0sAjLQlxhxMDV7bVqcfz4ca6cOnWK2+KLW3vZmwe3TRn/NkIeIKw/U/TQpYsxLLqsvml79+69+tNPP51dunTpZbDGRcjnicpNnqhkIBc3GllBi4pSEL1t27YtqVevXnWmHAxlpWBj4fElS5Y8P2zYsIX//PNP+2rVqg1n46QVU3aVnbne9evXc9jLQUqVKlUi6tevH6dFHQ8fPpyyb9++y1u3br3QqVOnuGbNmiUkJiZqbubC+7D5oLhu3boJRlZ+CQkJLzCiciQ+Pv5b9t8FWrSru/Drr78eYiQF54xTbK6+ys8RufzcISYrxWYjKQL0HGdlECs/2bVMsHrH9n5a8thHTw2Cx3oaNxcQOuDm5eVBTk4OZGRkQFpaGlxNSYFr16/DDfY3bsfF4xhdVtx+AuFRC/wtkhBc8sEtypGRkRAbGwtxcXHABjBHXKKiovBNhDvurAUIA8thfbHeuEMHl5pS8HnYJ/4fv8dnwfMcRaNGjWDwYO8F1mJ1LgyUCJxz9erVzEuXLmVeuHAh88yZM5lMMV1nE87lzMzMfH5iKbQhKjm8VSWTn4jSmXwmsTZobu/e6BDriaUdtcCouUr+KkyJDt+0adMhphCuMrFlsf5XZLFYgpgYo1m/j2N9Enc/4Sc6G5Vi/eIdvRBlVsfujEBuBGu00FB8SR8xYkSNXr16ta9YsWJjpvzqsPFSjpVbyAcbx3knTpy4dv78+SxG1q78+eefVxhhzRD6ARuz6CBZIOoTwhY2IUGdv+hvu+B3vhXz18I+lcnGLPandCbDTP77ACbbCOSWvIyj+GcJBpUbLoT78PUuFNVd6NtBfAkUfUq+GL/11lt13n333d5ebNZcNk6bsvY74Wi7ehJsDrlcvXr1OUK7spLGyjVWUlhJ5ecMfNnJO9D8LY/t4KA4K1bH2o/kTjiYfM7usQHt7zI2g2STs5DlGC0cYWFhEMlIA04S+B0GOMvPz7eSFWEWc3K5RiAreB8kJUhOBIKC3wmZll1RGH68tQivhaQHg8wheUEIJAn/78wuqY4dO3q1rZCobN68eW3nzp1xv28IP+kHsLYKFCmaknmeL0X8BI8lT2RVEZaAcDLKYvIvMEqfRYuKElE5evToAUZUruPz5+bmCuZrgaEKk3Awr3BzX3jhhbJ6suixPoxvQOvEfP/rr78+zspF9vcfvOKPYuMliicE4aj8kIzBfxFGhf5QLDw/O57PK/p8FWRFSSACWcFr3WRjKovvUzf5a/vb9L1sXkEHgWO7Q4tZvQtEbSiUIhFBCRARFsk3nccff/x+LzdrKCMj+Nb7stp2HTt27J2TJ09+2pOVrFq1ajn24nI1NTU1m2/LDBFByRfJ3rjbNw1KVjDUp6xj7ahP50p+365hLYgKDzU8WRGUOxIV7k2muJgjFRFM0efzVhVU7n54nos+L0hGBOsK3g/JBBb8G7931W9FeB6BeAmxTwSigsQLrSrOxKBB3xpvo379+lVY3c+KyIqUcgIRURGTlVxeaeTwE49QcpjsC43QXzG2C6YlUML8+fM3iiZVi0jRWfjnF2SF8sjs37+/7NretXmb4cqM1Zo9R0SLGlDts2FKZEUgGQIxsIisC5zPERuXGTxRCef7RIhIYfvb9IUCEVER+oSYtTtCVEqMsyIykiPqY0X8dYS+huQwTNRfHTYCw60+E4Ky9OefNUD0zP52lHB1HZDQto60KyPRzyld8/iA6ZB/Ic2hemDfwz5oD+PGjYscM2bMUVG7CkQ0h6+nhciK5yEbIOVmbh4csGNZeXNwb1M0DCp4IfIqWjlQ2QcxZV8qIoJT7IWMqGjh3yL4o+C9xJYcwW8lUIMdS2JLERIUwfyH9xAsREIOH0eAhEoPYG88NatXr34VfX9siEqAjYIRT4K2iipPZGXhvmMy030EQbSoqMlQfezYsdMffPDBfhuyYhH9nSdS4iiDYCbXTL0974HmbxU3/Pt9EClmMekSK5FQ+G95JQikE80ViRSieClIUDp+NkRFzUAU/BbEy4y2FpsgnqyIibWjQcWKRc8vLsUiciJcM0Dq2jNmzGgM+ki8dxebZ0PYnJSn1K6LFi3qXL58edndQ2lLdjtMVMLqVIDQuhVkz1m1ahUm20wVtWuOaL4oNDNR0StZwSUg2V1AC9Zul7bnBQfCnTWrmqJhBBIibCFGZY/kQcggLCz9uOpzJBARvL5Q8J5YhCUgLRx+BUIkPBteF8kKPo8zWZG5jhItnyFVyzdvpbeeL7/8snqPHj122LxBi5VMsaiIl0DEyipf9P8iPZMVjN3SuMooLmmbGrz33nsrRdaDIhvCUmxjbcAJOCA2NjZbj88uQ1jE1gwxSQmUsDBYbEhFkQ2RKxk6NkRFzTKQrdWjSFRXP76OATb1cjRUe7HNc4iL2L/G7rXvueeemnoZvwztWbuuVmrXe++99xGle2G9HUFwxVio+vkwCIgMs3sOe6k7tnLlymSb9hQTXNt+Q2TFA+gqO0KYQhsza4HksdEP3MNZG8xgVeFeR3hfkgBeuasFZ3nhsyXjpxIJEKwrwnKNQFCEosVuIDH5Epa4kKAIJMWZJSAkO3pBpUqV8G06zQ5R8bOxrFhsLAuFcLtJ3cLkspnJ3e5SCC69JJ2bqUnUWUdJClpU8G81+O6777bOnz//JD/hi5c6xGSlQGR14uQXFRWVq9cxioQFP5lyE7dloYh0BthY2AIk+kIx3L6MYlEgK45YPYpF8hWTQvHSkis5ZYpt7nfb1CJ37ZiYmBwjteuZM2c6hIeHt9bSqoIEpdKkQbJEhZ/T0apyw6Zdi2ytWkL9iay4H/g28oHcCcmXrto99ki3NqZolABemTtLEIQlHYHcqVliEd9LTF5sjzlLvvD+AmERLELiOjljIfLUFm81SEhIQMWaBbeb7f0klAjA7aZz8afwfLKvaLhVGAkLLsMkp/wO17IOcLuDbLPDagF0nuUyy8bfp5qkIPbs2XPuiSee2MxbS3JFhEX8NiiWSYnMmGLQvYMxKoetH30Lz3S5YKvc/OH25RApslJspx9IKX5niIQtqQCJfunqG57c4LV7bT23rxRpYS8kY7W2qlSe9CC3BKRirkN/r5sS7Voyp5iZqOiRrKADm2zo0ZdnSr9FNqheEeKiSxm+QbQOEidefvG2tUjqb7MgMjJSWNuWepP0k5jQi23fSAXFJ4KqpEVIHsTxTZCsYEp7TG2fX5jOfSKk0tzbAiPQhgTGcEQIc/xgaH50oHUG58+fzxgxYsR6EDkN84RFbFkRvw0W25Bu3Zm1mfLys1UKGM/iQHMQKzfgn0/t8s1tfcFisahSPIdajrd7zKYvScKbsTiCg4N1nyRNaOs6294ZzOatNlpaVSq80UdpaaqkKRmxm2dbJ1+D3siK7BJQXkEhbNh3RPLY+KH9iagQvDeQAgNR6bqsXAXlgYrmqw2V857qdA5jKwx35BpINLA4SzK0wuuvv74Vg5TBf7sWbvKEzpAOgcXFxRz5QL8GewrD5nvF53M1ToXavqQnkmJEsHn5ZS2tKgnPdofYfupCbJw9e/ZFsC4jcpY3KcLsC9BT1mW0Lct66/25M0n6Ifz9oV2DmsYeDA76pRD0hcLCQlRimpmMRMpkJFiT6RkGmDW6S5cuvy9YsAADbWWKSjbYRN000qRrsVhKlvi0bGt3Q5xd12iZdvUwfhmhRKfaJlpZVZCkxA1V9yJx6tSpodWqVTsG/+0m8/fVNtLTg3diJUbuhGemSSctHNGrE6fsDc7cacYwMIqKiriJTmvCcqjl+AKesBgCixYtOtWoUaOVGzZsOM/+m84XdAzMEJEVQwaw4i0rAUZVGERSnB6/Y2R/m5mj2qqC/im4/KMGSUlJb9SoUQOXUTHegzPby00FPS0DvSV38OL1G5CTL+2P9dR9nYzPGv39acYwMPLy8oS3Hs39LBhh2cDe7jBU7yxW6unx+U+cOJExY8aMfz/77LNjrC+jFQXjQaRZLJZUEVkRloEKwIAxIZhC8+fnTIsv9W3W9/wk+mSxL4xfVVaV39RZVZCo4BZlNdi+ffuMtm3brmJ/RvBfiXf/6KYfeHIc64WstGBFdgHvnW9/lfy+ctkyUCkulogKwavIyckJqF+/fkC9evXgl3oBxe+8845Lg1hCGWAURAwTi/bje8Ga6FMXJOXQoUOpVatWDXvhhRcSp0+ffksuoxs3bpxghCWNyeef/Pz8XcnJyetTU1NT+vfv3xH9fIzUxuw5/KOjo4NjY2OLmjdvbqm/cKJFwzZ26o3Z1ufFVRLBrodv8e34gn0NJ9emEudhBESM+7GHL5vZvS/ZUWiGGL88WSm28Ul6StmqouwHj1uTy7/RR3GLMmLDhg1Lu3Tpgll6Y/h+IQSBC+D/5hz2tZa1bd9h168u6gd1+b9tgQ9/ROgDrBw2O1mRzTpYWGSBRRt2Sh579zHjO9YqkZU5Sw/C9AV7VV/v67e6Q+uG5YHgOSQkJCw/cOCAlm8xW/iJABuym17ICf8WWhQcHByAu7ruuOOOKCz2zo2JibmD/xNJzFPVq1c3bBuHhYW1S0tLS3VDG7dTmgOduCZXmAJKV/EbVEQD+X6m1iu7Ol+6iq6DsXRqOFDdLXpp2/T0dCGYY0nYfZ64tVeyqiBhUQJaVNRsUV63bt32bt26IVHBra1ogbzJ18t/6tSpUU8++eRdERERrfkUAZpm63VyzhH67gj+/6n8NX5mZTlYl4FNQ1bQsfYF2R594F+7x7o3q294ohJAlhWC/UlAFzh+/HhazZo1ORNmSEiILhys0EnR1lFRbiuvj7TxLddkCgh3k43kfZ9slVNP9vEaK501uncNJ+qqC9y8eVPIDF2otn5qrSroo6KGqCQlJZ165plnMDy7kEuqJPL1vn37Ojds2PBD9oJQRef9sTQrffhyBqwJiWdpoit10E86gELSwuFTZkt+P7BjCwgJCgSjAhMQBvPh9AkEPeDUqVPX2aR56Y8//jg2a9asPadPn+YsCQJRIRgOuO19PyMmnUQkJYgVnFT/0pCoGBo5OTm3ZEln8olWUtxqrCqObFFu3Lhx9WPHjr28devWB/v37x/HE6dC3LrcqFGjBW4mKu4A6vWZvKWloRksK7JWlesZWXAtXTqf2Yv39zDkwEBygjFVyFfFvQiIClMbdEmZ1UeFmUo248eP/3HSpEm4JfKW9AC4PTcsLMz/999/7zFy5Mhm1ItMAQy0OYsp4Ebssy3/pluHxu9/uHjxYpYwBgoKCh5in5OVXqKz/k6WvaYjW5TFaN26dYtFixbduWnTps+ys7OPV6pU6VmD9z/sc5jEdAArvxmVrNQC69qYXXzyy1+S35eOKgU1KyYYrtW47MkaZDImKAMnC7VvNb6GiRMnPly1atW5I0aM2C4iLH4vvfRSzXHjxj1YpkyZBJKS+QgLOBhg0BfGLyPoeQMHDjzUvHnz0C1btkwPCAh4TM3vMDmi24haQEBQ586dXy4sLLxgoj64mO9/3xiRrMia2SzFxfB/i1dJHnv7kb6Gaylc9nEm9P3wvg24IsaI91fD9gOXnKqHkIeHCJNvY/jw4cP279+/87PPPsO1Zb+VK1cO6N69OwXjMHGTkwhuR2pq6v4lS5a0uffee8ez+VlX0UVZfSqaTNzoQ/U3Kw7vRvDmOgSuCcp6w+07ccbusT5t7zRM6yApQN8UPeToEZMVZ5IHEsyF9957D7dmXty9e3dXIioEX0RwcHBknz59vtcbUTExnHK49ab2xMU82TXBJ+041nZtWh9KhYY4xsr8/cEfMwmzTyQP/jZWBSELsMVi4Sw6+KkF9Ljsg8/GyYB8ZgyFjLWHIGvXSci/mAbZu07edhzX94MrxEJY3QoQ1bW+qpgOUVFRTc6ePftQpUqVXnK0PjlHL3L1wIBYWCdLRg73nRi4CwL9BfATC9YLIVV/JWjlv0BQ167okyHVppyCrxgLQRVLl7RrRItEVf1Nj2BjoI4rcirKzOX+ticrYQzg2ES5WeVVw2fnHLD6sLzJyiSjkJWH5A5m5eTCyYspksdee7iXqhtgCHvcFqxGKaPytlXgAmFxRKmLyY6/TjIe26KoqIh7JoFAmXE5SOstrLg+LTfBuOt+uNsAdx3gFkmlnQc4mWA8+7QlABcnL+O39rZTnEAYUXlVdd/h66M2F4oweQsTHU7WWC+sn6NwZ9I/b/ap08/PdYq82SN0zvpSYJti2Hg17coR1Au3KjD0L8FrqAXulLHngOrp8asW+Mz4jBlrD6rOBVQyBsS6iY1JVO6x/e9Sta3ZbHMOWPMAzgAH4rB4S5PiEtAQuRO+WymdayEsJBgaVa8kSzrQN0SLXDvOWB44C46VKelWkRcUFHAkCmVFOYn0/VaDE4CaoFNSQMWDk0l5lXEe1FxPzQSmNNk7Q1QI7gPXJh8uc5kwOUJUjCgj7P9aPSNHCJZYST8ShLKPttOFtcWDcw764gxl5TO9kxXZHUC4HDNu9s+Sx159qJddSwAq4EBSvoooLLTGPQoODiYnW53iyozVkiZlR4HXOP/mQkic+4zTZnqcuM69uUizt3+CfoD948xzc10ioGYHKmAcj+4CZ51gxQGrhFnmnG6OkBVvOC1g7SfLnXDs/GW7xx7u3FLSmsI5sBJRUU1WwsLCyGdF50pEy7fCK5+vdroex/t/QkSFiIrPAeWCS3TuJCq2pCh52Beajn2dzzkORcv1hrbCCsp6Xb/42feS3zetWR1iIyNu+Q6XMUKCg0nxOoC4uDha/vExqPUxIWXmO7jkgrnf7MCxgn3f0yRduK+3CIuH55zSFotFdRZ5b2j4e+QO5uYXwPbDJySPvfVo71v+j5aUoMBAGlkEggpk/61+4kUlhqZcUmbmJa9mUIjugND3vSUfvL9ZCIuKOUd1iF9PkxV0rB0ld8KSLXskvw/094dWdf9zQApwMsAageCzk3CGeuKBTnaOWmIIxgEuORCkoQeiIBAWo49BFXOO6pxjntb2mDRLNkDK8/83T/L7Uf27lWQnFnLrOILzVzJh5Y4zcDj5OpxLyeI+xaiUEAmV40tBq4bloTUr9RLL+PSAJXmZD2onPtwRgIVgTqAidlUJ4rZj3L1SwK6TumS3aXyatHIy1Yqw4C4td4b118Gck6hXsiK7Mfz81VTILyyUPPbE3R1K/naEqGBI+m+WHlQMTY/KGQueN51Xxu+MaMUp4gaD5qm+39dvded+Y1SQvExMVi6qU1BXZqwiYZkYrhILcXwUIdAfLisZfUs6khS9WZywrbBOziRENMKcU1xcjMsluCVVMZy6J8lKa1ZkY+S//vUiye/vqJAA5UtHc3/j8o8aZ9qM7HyY8NV2zjrgrGUB8+8M7FbLJyYwkhcB4YwjLsF0b7sOAwPCocnfUztn3GVV0SMwthEGjzNqhGCVUCQsnvRZkd2mVFBYBMu37ZU8Nn5oP+vTqFz+wSWLB99Y4bTiFePnNcdMP3mRvAgCsmiLsvnJykXXyAoqT6llQnz7xwjFRoQQOl+PECLKmhEWi8VPLQ/xFFlBs8izcies33fY7rHOd9blPtVst0XF+yR7w8c3fQKQvAwO9AtAs7sWEWjVTIrkq0JQ00/OvbmQi0GCljjxjjG0sBgReu/3nqyfJ+ec4uJi1WTFU8tAHUEhaeFTU7+R/P7R7m0hONBKUgIUln9wKePlTzZxn3KIigiGnq2rcc6h9auX5s4/xJT2qh1nFH01zAQzy8s2j4y7o1C643448Vd4o4/17/53cQGj3LlEk70rWbNrYd1LsUkvIDKU9wXYYvht0O7qU7YOlFrnfHFff7FGXkVfFfRbwfbGfqqFf4Wnx6+WlgshyWPBhVTNxiuOISxiAmGGOaeoqAiVOip4C1+KvU1WZDO6Xr2RCWmZ2ZLHnu3XrcSqohQafs7Sg4oWAvSpGD24KaeAxcDdLHgMle+Er3eotjQcXDjUUSbJfeohzD3JS7/A9emE57rf8n9nEwA6MiFqUe+qnw+7ZVKN4JWYWWJHGL9vhbrlzV/IKyMQFwey8HoV2Ce1INLCmBVbl5DMabXDCK/lTmuHN+Yc3rKCPKRQ6VxPLAPhGk5nuROmLFwh+X1CbDRULxenyqqCShOVrxxwtwoWW8UrBu5MWTS5l1u24hbz2ZjFSthbMLq87O0aMwukJvpQN5tltZhQK096UHJCFUiMUX0azAR3m/cF0nK022RuuUjvSyxa+Kpg/8ZcOLbLYEjUbcm7N8en3uYcNr/782RFkYt4gqzIOtYWMWX05fJ1ksfefrRvyd9+CmRllYJz6PC+DVTvVEHlPPut7tx2XC1RVFTEFUEBexNGl9fv2/+BcynXTatQpJS6u5UMmq1dnezkMsdyb27P9iC24PW+Vdpj90KigoTl+IDpps7KjCTdHhEXiLqrVqaizFzTzTm8gy0uAymazt29DISOtePkTth11P46ea9Wja1Exc9P9knQh0JuFwq+9eNShkMTL1PAaFXA7bhaWQlu3rzJRd3lnodf1vDG8oYZ5DXnj02wYO12+OXdF+jtVyO4ujYd3bWBKkJju9ZO8CywDVAxeXKLOt4LrS3oV1Fp0iCv9G97yPrbNV8tJOhyJF0gLJhR2RWfEnfvVvJGm/DLQAG84UR2+7K7LSvoaSXrWPvMtG/tEJUmEB5iXX5QiquycvtpRSuBM2jNR2fVAikpKZCdnQ35+flet64YXV7Jl1Jg68FjsHr3Qfh44R+kfXSCiBaJJASDwFu7dpC0oNOmmawspZonqhwfNajjSZMVgajIvrm7m6wMk3/Dz4HTl69KHhsz8O7/KqlgfTh8yr75Gpcmeraq6vQDPKBBkLPTp0/DxYsXIScnBwoKCjjl600YXV7zVm0p8WF57/sl8MC7/2fqJSGjwORBq0wFdJz0pvJEK4tZtsmrtUjoyZqkF4SGhlbZvXt3V/Cyz0o86i65E2b/sUHy+8jwUKhXrWLJ/5WWSuR2orj6pu+K4kasXbsWDhw4ALm5uVBYWFhiJfCmg62R5YU+TgvWbCs5/+1H+3FLQZXjKTeRt0GRb40F9LPwNmExQ1ZvtUH2aHzcjuDg4ApNmzb9ITMzc7ySZcWdPisd5Q6i8pkw9zfJY689fN8ttfZXsRPIrqUgvpTsb/fv3w+NGjWSPQcVuLPxRJKTk6FMmTK6IClmkNdfu/bDlbQM7u/udzWAVwbdSyNeJ8B08MEV5ZcXXI3GadQcKXoEWsIwxou7Y5go9QWjtylGfVazrJax9iB1OjuIiIgYm5WVtalUqVK/e5qsoMb7UO6Ew2fsb8N6sGMLzSqitKV248aNEBISArVr13aLINAqhGRLKHqPF6J3eX2/aiv3GRURBtNHDaFRruWEwd6yXXHiQ8WjNGnjOc4qRiHug6kJnxdCvqNMo7o24EiLp31JcCnI222KPieuyB2fAX8vZ6VCYoaBEV0dn2ZGeHj45+wFdTOb99OljrtrGagTKKR+fu7TuZLft6xXA6IjPLf2jcowLy9P9hy5OCNKQMUuFNzZIiRiNGqQM2/K63JqOqzafcBqval3By396AxKWWuFAFnOIrRuBdPJyxau5u1xFrg7CCOX1lw8miMPnvI/MkuQwIsfLrO7zGNNT7DI5SUvdwTz0xnQh8BuqBN3kZV7ZDtoXj7sPX5a8thbQ/pqy9wVQsnHxcVBVFSUS9eQVdzs2tHR0WjmgrCwMAgKClKV48hb0LO8fli9lfNZsZKVmsQO3PCG6SqQjGCxnZiRxGAuGW/XT0+QUm7pXnY6RdKCeWHqrHkDKk8axFnK3E1cvE1YtLBY2NvlhAQdIzdrYTHzEQddu2TFHctAGFtllNwJizbskh4o7E36rtrVbvse/RbkLBG4g8We06hSGPiHHnpI8YHSXVC+MTExnAIuVaoUp3yDg4NLrAXeglHl9YPIsbZV/TuIXWgMrSZDJCZYMJ6HdcJO1sSR0mxmcGEJRAjGhYpOT5l/sf2woMUFCYU1pP5BzR1FLRk5Xu/3SMhc7aP4eyGWjNa5gczY//VAVrrIHUTiMXrGD5LHXry/p+Q2ZSWyUjm+lF0li1mFXbU0uHKNZs2aQXp6Om7R4hQvLm0IyxreWgoyorw27/+3ZJs7yg2XgeSgdTI4W6uAbWAzT9/PGaAitK2n+LoRXOLBMM12aGi5NRXrZUumtHYMtZUPWhjc6U+Bcsa3cQwWhkpNC38RqTZGJ1pbRedIf0WyghYWlD/KBIkL3gfr664dLp4eT0jKtPLXQZloLRcktLb93wxzjhRZwai2bM6/bSeK1q/3aK9/T+6EM1eulZjybTG0Rxtp5q2wg0bOKXTljjMuLUsoBVBTAloIKlaseMuShjetKkaV1/ertwDBM2/TegQmQjQjkLAg4dJzkLQrn9+aiA+VJpI49G9B8mIG6L1/eSuIn56gtdZEE47sbPfqlz9Jfl+3SgWIj5H2hShWiPaqtINFLrS8EuYs0+btEC0FgpVAsA7okazoUV43sm7Csm3/EJPwAPS624Yma+8SKnsZs7G/mKFtkIDpdZkFrYpmJeveJCuysVUwU+7Kvw9IHhs/tJ/d3ylZVjAQmdwOFMwurOSLoeXvJAUtWvrx9k4go8lr0YadkFdQCAT3A83NerOuoDKkbM36ICxSS3u5LjrI6oUk6NVKhMuEFB1aW7Ki6FiLuVzsoUMj+3E70GdFKZiaXD4bXNZ4+ZNNDi1voN8FKl+zwkjymruSloA8O2nrJzOynmOrBET5lgKxbsFdyPk2CM7A+H9XdvPoaYeLsLylJyCRo0CI2pOVTjjPyZ0w4uM5kt8/eW9HCAyQr4pS4r+B3WrJWgtQmT75/mpVCtiRc40Ko8gLt7gfOn2eRqoHIWxf1ctbpV6tKsEVfNPagyQFd70gaXHViVpvSy/Y7/VCoJCoV3i9j8/1rwkTJkgmNdRyN9BrcgcxoFd2rnQwsad7d1Fm9YysyMUnQcWL1oLpC/bKKtUH31gBw/vU55S1lEUB/TXkrmEL27Dy9RPLuBQUTQq4tHIuJeuW7+S2B0vtxrHN+WMUef28cRexB6+QhPaa7VBxFrj8o+e3StyeSnC9jfWGqp8Ps+uj40migvXwxeXPjRs3ouWiiCcsxVqTlTtRH8qd8P73SyS/L18mBqrEKw96tKyg74pcBmZUvqhgcUeLnOKf8PUOzhEUfTcwFw4q3fOMDOBOFketAyPev3Xr5NdvdXc5GaAt8HkcIQRS5x5cONSQ8jpz+RrN6F4CbllFfwRvTNrcNtnn9L3TRNhOapYorN4gKnpUxkgUKk0aBOddXOJylaj4apbmq1ev+vMkxeIOy0o7uYOFRRb4fvVWyWPvDuuv+ibcriCF6K/vPNWas0IoxfpAJWxmnxS10Lu8bublA8F7SJz7DGfy96SFBZUYEhUjOBWi5Qf9NgiOEz09k1GsHxIGDJPvyUB9eF8kSr5KVBBpaWmBtkQFoYXPCjrWjpE7Ycfh43aP3d2ioeINcEdIsMow9bikMJu9raMVgACmlFdEaAg1nAeBFhZP+bDgffB+Rtn9gDuntFzK8IUopYJC1nsbC1mpPdX3kfjiy4EvExVEbm4ukhVU9n5ak5UOYE1AZBdPTpF2rO3XrhmEMhIiB9zCikTFkUBqqICnju4Iowc3ddl/RG7XjJkIi5Hk9cXLjxOD8IIFAYOAucvHAJW+kETPiGTOVbmgYkSliMrRzEAyZjSF7O6+jzIRSBFtUQYoKCgI5rnJLWRFi2WgkXIH0zKz4VLqDcljox/oKXvhwIAALjCYK4oTHUPRCfTntccdigGCv0PHUsyj40j4+GiNnWsFMqG1H4xR5HUjK/u273o2bwgE77wRo2LGyRtD3eNOEFdC8wvBrswQRwXlUoopnSszVjkcal1wJNarDJBIYltl/33S6TDyQqh+o1qOxH0fl0RdzZGE8uSscv3v8nlLii0sFkugFFnxU4pfooDqrCTLnTBx3hKYuugPCSUVBsfm/U/yN1jDIAetKWqAShR3o0jlr0Eli86j+InEIMoNpAMhyNvbgeGMIK+c/AIoN+DZ24jKoneepxGtE+B6fg7vhFuUmWs3eRtO9rh7Brf74t+otMw6SSOJE2SSe+TibYQOnz+UPTs+f1TXBoYiakJeIKG9pZ4PFXFo3QolbW20Z3RUFlwuoItpXEJGKYdc276v52i53kZWVtbeqKio3uxPfFPGjlWoFVl5lJV5dhkSu3Zs76clj338zMPwaPfbcwEhQQkKDDSEMneCMXLF24kMjYL1/xyGfm9Ns9JXXlZJsz+AauXKknAIBALBZDh+/Pi3tWvXfof9mc7KTTFZcWUZCB1rZZMWHkw+Z/dYv7ZNb/vO1WUfvaOwsLDEsqLGWdjXsf3gMTRF8cZAPwhgJK9qQhwJhkAgEEyIpKSk/WBdArptGciVdRb0hJN1rB0xVdqxtkOjOhAZHlryf6wROtGamagUFRWhiQudhzjripoUAr6My9dvwNfLVlu3q6OYmKymjRpC1igCgeA2ZGdnX9VTfa5cuZLjS7L/9NNPk0Aieq2rZGWA7I1z8+Do2UuSx94cfN9/FcDdPsHBmvun6A0XLlyAnJwcjqwgcVFKH+DreGnaV3A97QYG17EWxlge7NSCBONDwIl69OjRuxcvXnxMT/U6depUErWOOfH1119/kZyc/LO365Gfn2+ZMGHC/jfeeGO3r8h+9uzZs7Zu3Zpl77izDAGXgGT3jy5Ys03y+9DgQGhyRxXub1z2QYuKVm/LerRWIEHZs2cPTryQl5dXQla8jYyMDDhw4ABXPz0h5XoqDBo7EX7fuJ23qlitUHfeUQ3CQoKB4DtISEgImz59+l0dO3Ysv3Llyj16qNO6deu216hR4+Ndu3bNphZyHcuXLz+jl7r8+eefqzp37tw0MTFxoDfrsWzZsrNt27ZdjWTlu+++O7Jo0aIdZu8HKHt8MQHejq4lWemqRBpe+eJHyWNjBt7Lhcx3x7KPQHqQDOiBEOzfvx9+/fVXOHv2LEdS9LIEdOjQIRwQcO7cOTh48CBcvHgRA/F4XV5bduyEZgOGwe/rt/xnUeHl9Onzjyj9vJCmfnOiTJkykT179my2fv36ZTdv3kzxRh2uXbuWxZTHsm7duv3C/lvQqlWrufPnzx/Ixs1WaiHHsXHjxkstWrT4q2/fvptHjhz5R2pqaoa36nL9+vXMV155Zf7x48fPNm7cuJe3ZcP0YgF7wcXsranY9R566KHfJk6c+Knelqi0lH2vXr1+5edwVNwWvtyiJJ3ZDYSvt2gGrWPvhBMXrkCzp9+WPLbj83egTtUKbvM9wOdBQoDOrEgO8P9IisLCHAu2wwYPnDlzhvusVq1aCbmw9ykgOTmZIwEpKSncPaOjoyEuLg7Kli2Lky73/1KlSkFoaChXL0fkIOwmQiKGz5efn19CgvD/wvKSQIjEJO7UqVNw/vx5rl4REREQExPD1Sc+Pp6rU3h4OISEhHAFt417Q15LtifB4fNXwC8oFPxCwthnCCusuwUEwbVlX3L1kpHXYziwwZr6QShuw+HDh1OSkpIuBQQEFDZr1iyevW1Xdvf9/P39LXXq1Cnny0pu7dq16NR/vlGjRg1ZH76L9YlW7pT5vn37Lm/btu38kiVLTjJSf4XvY5f5gsoj/eTJk/UrVao0gvWFtqyNqutRbj/++GMSe45znTt3jmvatGm5ajhIHZADDt26desmODtv47hnL0nXfv/993Nz5sw5zWSGW1Mxsy2adjOqVq2a+9RTT8V17dq1Uq1atWqytm3sifHLZHKWte2xCxcuXGLz44gKFSrU1kN7sTl5SmZmJu6IQRKHgcrSGLkrePvtt2uw+aY+m7uburPvu3POkZI9+xr7GO77vsKTNE22LmOK5LVyJ/QeNw02JR2VPNaIzemPdm8Hgzq3hOiIcLcRFkGh49ILexsD1vDc/1Hh4bHIyMhblDsqWaGg0kVnWGF7sXibsXjbsUCMbGWIJATJCJICvE9sbCxHDEqXLg1RUVEcMXCUrOA9kJzg82DdcBkH68qYKdy4cYN7PnxOPI7PKfxGuD5+CiQEyRLWA+uFRArJCtYVjwv38bS8LH7+8L9Ff0GRX4CVpASHWD8Dg+G1If3g1SF9lOSVcKjl+JK37vo7J/o1b9488OzZsyFMLqXYs0Sz+sexe8ezw/iJS5mlePLtsImP9zkq5CfcLKaocFJJZ/XDiUUwU4Wy+8Tg3KPV/Zg8MVlS7ujRoytPmTJllK+RFSbHvPnz53cZOnToIZT9HXfcUczaN4D1v1DWbyNZG8eyc8ryBTOkRmE78HL3d0Lm+E++oFT5ifQqX9JEkyqeFzhr1qxqrN91ZuOlORtXjdgYq8XGUIS35cb6f2GPHj2+2LBhA27RTGf9NY311VRW0EeggJUAJq8IXmalxf2VD9LFif+RRx6p2KVLl0r169cvx+aRUMZ5yrE55Zb8F6w98k+cOHGNKaFMNjdkrlu37sovv/xyhX9rLuLHTT4/TrJFCjmVl2kau2YmG+95fN1DWLtGsnqUcdf4RZmw774CO86dnsbYsWNnTJ06dQ8vG4G0ZPB1xS29uazvFzHZ+DFdEMzm/XB3zXHCnCMU1mfy+LEYwo+tUNEYC3RG9vxLQAr/mc5/n+cqWZnMyuv2DuYVFEJ8f+U5NDQ4CPq2bQZDe7SFdg1raz2hcZ+oXNHqgMoXFXl2djanhLGgskcFi9/hEoig5MWOr6gYUdmiksSC242D+BxF+L1wH/HSDhbBaVggLEgMkBAIVhUkDHgcr+MIWRGTL6x7eno6V/B58DkEnxjhGWzJCt4P74v3F+qFhAWJCBIoIRAf3sfT8tp08Dj8snkP+AUEcgQFWEGrCv7/+PzpUCY2Wk5evzCicts6c8O/3w/gB1A4P2hx8JbhPyNZQfMRDjhn95EXid4OM/kBlslPxIIVMpK/t5b3y1+9evV97C30fl+0rrC+OZONozH8RFbMT5DBIjnH8gWJSgQ/kQaBc8veFl6ZixVrmoioZPHtXcRf37a/xbB+G3v69OnJFStWrOVNuW3atGldp06d5vHPkf75559X69y5cz1WPwubK/3ZPBnIxncoG98hWNhcEsTGeCAbo378+L5FWTAScpKRs1Ns/AbybcAVNr4D+L/F208F034hL09h3GSLFLGglDPhvxgbfvyYiXDn+H377bcrTpgwYbJe+vjGjRsXsbb5RtTHsnmZ5PD9rcCmz4W5eY7LF5U8/lgIf2+hOHIf27kT2/66iKjcFI0r60uto9YpVmSZyO/b/1F1odz8Ali4fgdXEsvHM9LSDgZ3awMJsVEuN7TwFs8FmOOXDgRljf9HEoHKGRUyFkH52u7UEf8GlS9+CiRDiJMiVrqCEhYUNp6Pyyp4LyQHSBIE5e2MOdWWcAgB5vB5kHCIl4KkINRb+L1QL/w91ksgK0L9PCmvXcs2gX8IG1f+ARxBwaUfP/abKqxvxESVUpLXXHscz+bNWFBWefwACRZNquCkIisUXV8oBfzxIP7YTY3vV9CoUSPZ5Ybcr4ZB0fFtmk2eoU/NhYCabXQxkbO+2xL+W98G0Rt7LvwXo6GIn+DD+HZwxUFOULBC+wrKI1d0b2GNXTivpL3YGMljRKWmt+XWoEGDyozkNm/atGk79pLSyFUrwv333w8zZswovnz58tEdO3bsHDx48Ho2NwSIiIu/qK9bbKwqwpi5ycsyiy85IoVcJCI8/u4cv71795bt3AXrv4L8P6Zo1hY4lnBMybRVIliXGgWCkss/dwFfZ4tofivywBwn3LfQZn4T2jrIwfvYzp1ZIitlPkj4rDg6gDvxbyt28ewn3zksjeRLKfDu3MXw/g9LoGfzRhxx6X5XAy4ImCuERVDQtopUUNaC0sWC1hdbZS/8RrAUiC0GUspXbM0QWxgEXxDBH8TZCLYCkcD6i5d2kEhg3YWgc/asZcLzCNcQ5ICfAsEQ6u5JeR08fR4upOeAXzDTK37+PGGxfn7+0mNK8sLdBJtlyEqRyOQoDJJcfhAHgUR2T0eMePz1C2zePAQlGiB6I9fyfkWsL+WDj4L1g2Znz54NqlKlSpGNXPJ52Rbzf4fYTKJ+Tsrc1iIgmMSFti4WlSKRZY3re7/88ks9PSwvlC5dumbXrl21Jk1+5cqVq9uvX7+6V65c6fzVV199Onbs2MN8PxfLvFhiGUgsSymFLIxXsLHQaD5+Y2JisvXUx/nxfd1mXikUkRM40Pyt4oZ/vw8emuNsizC/2RY/J+dOwcqSJ3oRcYmsvCV38OK1NC6fi9OvL0UWWLFjH1fKl46BId3bwKPd2zodXl2wsAiWAkHZCwQCFbDYQVVY1hCUqe1vhE9Bedou09j+zp7ylvq92ucRSJBAxJBICPVW2mkk/F78TAJJEZZqxMtGnpLXmqXrOR8V6439raH1/ay/aVWvppK8PjjUcny6wqAotHm7yxUNLn8XB7LFZhIuEk2w/vzgC9T4fsWs3XS3+0nLt08lS06lSpXaZY6psTpy6kk/8QRus3STYyNzZ8lKsY1loECkUMXtDTbkhesXbdu2bagXubkTkZGRVceMGTMtISHhiaFDh+6zkXmxzctDgajkiz5tFaK/J8Yvq3uunsYSP74zRcs9YusdR1Q8PMcVi/q2eH7zE5EUR+4jNXfmi6w3tykxR8gKRuSSzZH99re/atZYmKn544V/wNRFf0KHRrU5a0vvNk0hJMgxfiV29BQrasFxVFyEnTa21gyxRUL4vziIndROF7GjqfjerhAVW+KFdcXnEAiDmkBz4nrZPo8tUfGEvLJzc+HPvUe4ZR8Q3R/7/IjendXE4dmiwtwI/KAX3pD9bczLrsBiM6DF5ks/m/todj89khVvgBGWYhFhARuzuL+LREWKsFhE9yiZuAXlwd50/WxN9EwR5vlSmwwaNOj11157rcelS5eKJfquxYb4FdoQFFvzv0fGb0REhK4slewlrwj+c9wukQn2d/w8vckrc5x4LIDNuHLmPhalceUsWWmnZBX5ZeMuzRsNFdrGpKNciY2M4HYRDe3RHupXq+gSaUFFKlbytk6fUoRDareLrQKWuqfU+a4QFkHxC5+Oxm2xVy+pv90tL3SqzSko4vxTbtEn7PxXH+p9S10lsP5Qy/FH7D0nbya1VWS2istV83yxxN/FNtfW/H7oFElU5RbCAqK3cYvGMpdqZ4v4/gDWuFKZHazKRFQfS3BwcJEvtQcj0rUOHz48MDY29isJy0qxhJIqllLIvBw9Mn5DQ0N1Rf7Z/Cn4P4GtTLw8x8mqFhfHVLGd+dQhsoKOtS/InbB5/1G3N2BaZjZ8sWwdV5rVqg7DeraD+zs0h1JhoS4rantxQLQkG1rAHrlw9/3cJa/v12y3Lv3YAIlpvLKz9SdKJ0iYSw2Bah1+VGoXSixlQ1hE/y3SS32QtPCKx6cQFRXVQmZ83qaY7Cljm+/dJsfAwEBdkX/x+JaSDc4Ppzc97PU5rv7Oia6SMvVtpPK8DqCQtPDJj+d4tDH3HDvFlde/XgT3t78LHu3RDlrWraFrxW90aC2vw2cucG0ohS9ffkLp5+hYu9FsMlYiKQTjkShfJCtsfmjLnp2WKp01ZVi3i/sZ6QXL3VBLVmStKtczsuBaeqZXHuBmbh58v3orV+pUKc+Rloe7tIYyUaWodXWOuSs32z3W5c56Sj+foeBYSyTFxMBgWLhEYO+NnKBCIV4/B4X7/3ReeTS6B/zK2A3cXCFiyvEa2WNrniRJOw6LxSL4gFjQOqdkXSGyYgUGMuomd8L0n//UxcNgludxs3+GCd/9Bve2agzDeraHzk3qktVEh8gvLISF63dKHkNH6qBAxdhCW8wgByIpzqGoqIgb1ERYXFCIqedc2oXkX6kBBJSRzTKBsUKIrDhDJK2WFWESLJKbP3yFsKghK7KOtZbiYvjst9W6U4RLtuzhSuX4Mtz250e6t4GKcaVpFOgEy7f9w/kgSWHyk4pJT/851HL8diIpvou8vDxht4PDvgbsjf+2t5fssTWJ8OgMvtxOGFGY18+K/dtXCIsSWUHH2vFyJ+w9dlrXD3gu5TpMmr8MPlywHLo2qw/DerSDu1s0VvPmTnAj7C0BBQcGcgRTARON+tzOkhQ2cYfAfwka24M1nLzPIicnR4gfUWzPTC4jt6YS5+1lH8msYD6WzUwpbpVTmgS3kJO2fBs1Y6Wc1IsyO+cG30bYVjiJbGFtdcpsbWWxWLBvY0A3IeVAsa8TFiWygh1H1rH2qalzjNH4xcWwevdBrpSNieT8WjB2S81K5WiW8DDOXLkGm/b/K3nssxeGKr5Us7LeF0gKm3ixcz4F1mXY9tRz/kN6eroQmVMIViWWW132MdBBuTXlywP8NfC6W3hF0Y4k7hZyUo5vp4G8jNUQDUwM2pUvI/jrXGAfa1ipYZa2EllWAkCUzM+XCYsSWRksdzDzZi6cvJhiuIe+eiMT/m/xKq60qV+Tc8rt364ZhIUE0wziAaAztL24MH3bNVP6+UwjOdY6SVJ6gjUHVx9n72ubd0Tr3Cbexs2bN4WcJIU2cnuNlc4a3MKPCKLLGMTaZEP22JoFWvdvG2DQrWHufJCgzk9xRQz2XO62rDgcJt/MhEWOrKC5VPapv/1rk+EFsO3Qca68+uWPMLBjSy52S5M7qtI0465ByEjKD6u2Sh7DredhwUFKl/jTCM/pJEnB1O6zWBlAPUUeOTk5JYnycnNz7wgODp6pEUkhaIfhrLQNmXxkVN4bdddT/3YYTkVfNithkSMrXeV+iG/Gb3/zi2kEgVaib/7cyJWGiZW5JSKMlhsdEU5DRkOs+vsAl0pBCiqWgI6DzncBueCT0p+fyBOM2rZ+YVGa5aXBa8nh4sWLmKXVPysr6zFGVD4Da9I2kltYlN4er05gYODMwCnH3zR6//Yk+N1ATuf0MSNhsUdWIlj5UO6H/567bNqOciD5HIz94keOjPVt24wRl7bQrmFtGkEa4PtV9rlGLWX/oTcPtRyfYwZyYkNUMHXqOMNPJq0e4oq7YbFY8gYOHHgoPT19Snh4+JMkN92jDiuLafZznHu6OjeZibDYIyvopCQbDvaFz+aZvqfk5hfAwvU7uJJYPp6ztgzu1gYSYqNoGDmBlBsZsHL3AcljHzw5UE08nLVmIiqMpFTh3zbvpd6hHqmpqfvT0tK+iYyM7EjSIBB8g7D4y5AVu8hhSnznEd+K9ZN8KQXenbsY6j32Kgx+fyb8tWs/FFkol5wjWLBmGxQUSsc3eqynoi/jj4dajk8zC1HhQUTFCQQHB0dGR0cTUfl/9q4EPIpi+dfmDiEJcimCnEYggAEEueVQVOCBKGD4+zhUHgjqQ3mAJyCHBxJQOQUFxSAKCBgCiAoiRzgi96FGEg4JgUAgEELu3ey/a9IDw7g7M7s7u9mjft9X38Jmrq3u6fp1dXUVgeC6sarcYc2zojgQJCTt99mGx+rSG/ceFqRG5Urw7+7thaRzde+qRm+FCuKtLAE1uLu6lmKU33jTix8WlzrXVqJSeu44mFJ3M9kF5oLrwv//Mfuo1RT8Kt8jfKqkQ/dYRERENCK9eR/E9P/YPuaCHKHN5DCERrI2asLap7YQ54NtRdA2bnm6h8VgYQspEpgSpZOqPDFSMNoErkSDAR66v6GwTISp4oMDA0gpMuw6ngo937C8dXbrR28KVbQVgEULY8pzy7KesxNGVLBKo+YERca9K6AkeaVFI6s6G2k7EIK6jBCMgCNbl3ErtFIQqN7bONXup1Vvxds+E4xgeelNDvxN0m3lrtSbM7faWvptrujfSCqxnbC9Cj8bapHgOOu3OKJPtevn5OQk33HHHZjzB8e8Ar2KQupNWMq76rLiElBmdg4RFfmMgBG+7UdSBLkjPEzYRTTk0U7QpG5NUs5Nr4r1ooUtouqqnT61aWx8zkoPJymcqDTTSlRw8C5aM9GuQVxqCExHNvmUpwANVtGaCXaRFLnecBZPcA706N/YxngNJJXoNSN4r4fFZrKyYuseanEFYL2bhYlbBUFvAeZt6fdQay3LHF6L63kFsC7pgMW/jX7qUfBTD6xNKo+X2kn4VKuxxEFYFzJdkAPmczk+0df0TH5nbSmC4Dj07N8iQSV4N2GxRFYUIx2tpUkn/BMHTpwW5I3PV0G/Tq2ETLmY+MzXsGpbshCUbQmv9n9c7fQtTWPjT3gBSUGvyiD20UHtOGGmyAZzgm0gvVE7EbyXsFgiK9WVTsjKyaWW5qgQEgyVw8OELc7Z128I2VktIb+wSEgxj9Kodg2BtGBtoioRFX1CT9YCa6tGhmvRwRxPJykSjNXiGaCB3HaQ3qidCN5NWCyRFcWFvwgfXs4ICQqEf7VrAU+0bwkPNm4Ad1WOvPUSGk1w7HQ6/HLgd1i5LRlSz1lOmpdy9gK8vfg7mLL0e+jZNgaGPtYJujZvrCXHiEfiyMmzgljCkvGq+bwwsHaHs19YV4B7VZorHYOubG+q3+Mq6B0ES3AOqH8TYdGbrFxUOqGaDyZEw5iKoY93gjef6WM1IVxggD+0jKoryPiBvWD9nkNCBtzTF7IsHl9sNEJC0gFB7qleRdj+PKh7e6hZtbJX6S5eIWOthqzAczCw1tOJCscIpT9ifAQGhRJsA+qtePVEUoQHtBP1byIsDtlhC98lK53QqmE9n2pIXKZIeHcMfPLSIJsy1/Zu1wJ2z50EsV3bqh6bfukKvL88EZo+9yb0nzwH1u8+aDV5micB41S+2/abxb893aUNBPirbltLctbL6UqiEhaXinUEFGPBjHtXOrR7xVch6K0ghxThCe1E/dutCYu7w5JnZafSCV2bR/tMA2LStw0fjIV7a9pXewtjWj4b+zxUrxQOc7/frHo8xrxs3n9ckGrsHIxrwdwtUeo1c9wSuAMoJy/f4t+mPNdP7fT9TWPjf/OSl3KA2qwT1/IJts/WSW/UTgT9xkZ39rBYmtqigSi2dgLmDmndqL7XNxzGp3w3+b92ExUp3h02AJ7q1Nqmc7Ku5cKctT9Dq5GToMfrcfDNL3ugoKjYo3RoLbdKKNPt3VUqqarNi2YPj5B3wEmzddIbtRPBG8ZIu8gKQtH9Pn14rJbcGB6Nyc8+Bc3q65dIa+7owXB31TvsOnf376kw6uMvIWrwOBgzfzkcTvvb7fV38vwl9txpFv+2YMyzaqdfY7JNzxewnF9CxdxFGCBKsMMIkt6onQg+Q1iskRXFNQuMW5n6fH+vbayG99SAEf/qqus1MSncxMF9HbpGbn4hfLFpO3R+9T3oOHoafLbhV6vLLOWN+J92gtnKVu5ebVuonb5Ar8Da8n7xwuJScd3UatQ0ruM7ksHTV0F6o3YiOJewuBtpsVbE5nMmHyid+N8nuwuu/OEzl3hd9eGX2W/zt6FmgVZgGv5pyxLg/GXHiwcfO5UO4xd+K+w4eqLDAzDk0Q5adte4ZiZlKoVvrWQ67tK8sZbaSb940QxBMbAWi+vpBSzy5h/TQyjIh253IQW5i4IaGSm77f96ZpJ1ld78azaB0ux0XfWmVuPIlXqT3wu3EmM9HWf9Nr3bSTBYbQeWSzv5MmlxlzgWa1bjChMsxRKrdDKmkUdD+fP+Y/CfuMWQV1jk8Y2DhvTJjq2ccm0kQAM6Pwiz1/yk2zUxId3KX/cKUr9GdSEg95lH2tu0c0lv/LjvKFy8ajldz0cv/lvt9BSwsgzpoaXOFQO8cNDV5UW+vwcE9Z8mGF4RgV1HCPEC3pjbwpl6C+o53ulky1egVzshkQju9+5tNa6onXwLSu6DV5lkqb7s/n7Qs00MnF89F/6KjxPyhXgycIkrvILzEt9hAjhn4dSFSzD5q7UQ/exr8My7C+DH346Wi9drmUJulfo1qqmd/nbT2PhiL3rHHlAczHVwkeNsM3jwnNsMrugxQMIS3G+a9xlBJ+pNJHreqDePbCdGKNHzYakYJ7aTPZWeCbZ5V9ydrGAK1hdtuRhmdJ33ylC4sm4hbPxgrLD119OgZ1CtJUTXreX034DLMBv3HobYqfOgybNvCEtPZzKzXKK/81euCVuvLWHWqGe0ZOq1uASUXNtjDYdiVLWjlWLR0Ab1HKdqlFFoxi7RGzN8WvSGs3eC/dClf/dXfvfR60Lt5P2ERS0wYzWTT2xmwv5+QvxESvwMOPPtxzD26Z4e0yjOziDr6uWZC9nXYObKH6D58AnQ5+2PYPX236CoxOi0+y3fvMuqN+eZh9upnb7EmRlrXY2wuFRkZi2dOfPEmaUlz4AcQV1GeJcRdDBWIbDNQE16C2gbS5bKEVLpov6N7aTlOILnEhYtUaRj7CEsN6eV4WEwaUhfuLp+EeyYPUEo5OfO0JBV1TMHd7MZth9JgWFxi6HhkPHw+mcr4PczGbrf4+stlku1Y34eTJKnggRLX3qwV8Xp0BoUiJ4Ev1pNSWG26g2DbynwktqJUO6ERatlRsIy1aEbGQwQ06A2JC+YIsS3zHjBPd3SV3PznHr9fDcIQsbfuDBxK7R/eQp0+98H8NVPO+FGQaHD19159C+ry00Lxzyndjomj9lJw4GN75UNBMQQGkEKs0Nv/lEdSGEe0E5Exr2bsNjiRniHyWNMTjl60zA2w36hdze4tn4R7F84FTo2vc9tGgKTmTkTaRkX3arjHThxGkbPXQZRg8fDy7O/guQ/T9p9raU/WecaTdVjgd6ytAREXhVlUGZQO/VmwzKSXjtaCM7t3970LkRGRrbJyspy28GvPAiLrWsePzOJYYLZzX539OYYbIl1bzZOHwcX186HL14bDkEBAeXaCI4Yay3Y6+Tr2wv0+CzbvAseHf8htHnxHZiXsBmuXL+h+Xz01mzYc8ji314b2EtLxuMkINgM0xHt2UExrwZB1MUup+iYoHc77fbZ/l2lSpVnb9y4MZMIi31kBYEWbB0nLV2gzOPisLsAa/Fg3pashAXwx9LpNtfS0QvnsrKdms4+cfdBt39JUs5egLcXfweNhrwGQ6cvgq2H/rCajVbEil/3Wg3cfalvd7VbbmgaG3/2H8TRw70qeeOjUGmKDe6o61prKnNvKyTnKr0Z964g71V5thPTv1ai4o2ZcitUqDAiPz+forztJCs3+weT7VAWy4KpU9HbskePh8IdOV++PhwuJ3wKa6e+IgTpuhKYxt4Z+OPvDEg6dsJjOkex0QgJSQfgyYmfQLNhb8GH326AjMvZFo+1llsF27JSxQqqKvfid0wxXbGjcSQ4SKsN6DiIextZ0UNvajrBpaLiH2YCwZp+zrqkndSIJZJJb04MFxwcPJJ6m2NkRQqceqC3BcOxcavmaCgLmHQIgQH+8HDLJsL255PLZ8HIPt1cohTMBoveBb0xeelaVQ+FuyL90hV4f3kiNH3uTeg/eQ6s330QSowm4W8Y92JtZ9Fn455XuzT2kx3e5lWRQDFoWI/gzaI1E60SFiQqhV+P9jrvgB56QwNnjbCg3go+H0peFRUi4ZJ2Wj3R6r0EosL+7s31hwwGA0V4g/V0+47gEJd4Jg8xGcWkh6MXrRoZDh+OGAgfDI+F/X+dFmoSOSvRGSZVe3nOV7Bp+niBMOmBVduS4ad9xzy+w5QysoVJ31CqVQqH/+vWDk5nXrZ6fNvG96pdcoY35VaxAMURXa8dDAJhYTNQIQHc/T3KvCnJKzW70T1uAK+iT/JGJCwYv4I6w/pAaBS1eKsIZUtpBqY/1J21fqxH/0ZCgjWMsG8HtokVronfYSxR8bbPXFb/qjy7e1hcaoe88VG7fLm/GVw006/HpCMTnC7X0eui1/MKYMkP24UU884A1tmZO3qIw9fZl3IKer/9ERQUFftU5xr6WEeY819V/cUwsnLUWz0rbJDB2g0FSsfkT2rlcTN4zGkhTXPOBlJn6/Ef3zn7nnpArUBeeehN6hlxpJCh1t/rDf3b2TWIMAMvJsBTwJusr0x3w/HNofP9bCgY7KoMaKeZLIOyoNze3OviMCLCQmHMgMeFLdB75r8DLaPq6vrQ8T8nCdt50dNiL3YcSYGn3pntc0QF8fagvmqHJHkzUeHGCBPY7FccGGN6AMF2eFsJAW8F9W+CHnB1ulak1xtw0s0ECwf1Z5Lq6EVxC3R0nZrw68dvQeaaeYI3xN9Pn58mbufF4FhbgDtj3vt6HfSd+IngAfI1YC4dDaUFfCV6UXFbNrrRCXaQFdIbtROByIqLiMsaKPO2PMpkBpRti3YIocFBwvJNduJCOLL4PXisdTOHHxQDSDuNngYvfPSFEC+jBMxN8mniL9ByxASYsWJjuVQ9dgd8Pm6Y2iEYwbzNR9TxneLMM6o9pQq3Z8ZOeqN2IvgO6XWDZ0C3w2Yu7zPpymQSkxaOXrjuXdVg1Tv/FbbgYkAoBuXm2ZnuHpeCVmzdKwhWk27duD40uLs6hAQGCn8/f+UqHD+dIeRo8VWCIkX3VqqBdQt8JWNt3vio3WFxqeiaq2ntmMCuL1DSNjuAa/0Fs58kRbg5qH8TvIGsSIHGK4ELlujFoNyXwMGgXMyK26ttc6EmUWZ2Dry7LEFY3rEXWMk4cddB6j1WgN4sDZmIfa0OEHpXXlWafWKAnbflQ3E2cGdIcL9pwm4ogo26q3yPy+5F/dsxlJaWGsJnnTTkjm1g9tn+6sbPhgnmMPwal4n6MNElU9tdlSNh3itD4cq6hbDxg7GCl4SgLzQUqcRSDb6WXn+B+uxzBBVjs2fGhdu1KdjWZui1/VsrqH87RlawyZCwEFlxX6C3ZT0TzAgXzQT3dzmccC7A3w86NmsIKfEzhKRzY5/uSW+EHjMoPz9h+U0FE5vGxpt8SS9546MwkHyJovEIjYSQQXOET4JtQO+KytZPgqVx0IXBr9S/7YfRaPQT7bWvEhY/D3veP5l8Dre2QK/W46KYzn/SkL5wdf0i2DF7AjSqXYPeDjvxycuDtBy21UfV8yGTQrXZbsiIpTSg2wGMXwkePMflHgOPnly4OPCV+rdDZMXfA222z5IVEeIW6AFM7mTyNJNTDivDYICYBrUhecEUIb5Fw3IGQYYBnR9UO2SBl2estQruXfmfaj+s1RRC3/yFXOZ2egpCX/leIC6OGkQ05N6+vIS/z9WEhfq37SguLvbnZIWWgTwYl6AseBG9LejTXKDHRTFPyAu9uwkJ5/YvnAodm95Hb4wKMCkfbh1XwXpf1hEjLJ+yD9XobjS0otHVA760RIK6w99bYep+wdOCBlmrYcTj8FzMXoriaBCqJ2zZRR25un9I+7ceXhbUszeTn6KiogDwcc9KgBf9FszR8iOXt6AsxgX3wTZx6KUyGCCq1l2wcfo4KCwugY17D8PIj74UtkMTbsf8V1VTd6P3K4k0JdTLOqqVZKC3AGugYC0UW9OW4wCOBgEHc1/ciYG6k8ZlYB2Z0ux0C8YzwqKx8+YCeVLiIKR7bzNQqPeD+sGKymWfzq27I/TvtrFg3LtS6J+29m9cVsLnxuvoXTrAnVBSUuLP7bXvpUIX29pTqwBrJdxQtv25M5MXoWzJSBdkXM6GCUvWwNqd+8j0cqAXCsmdAp5uGhuvmCDNG/OsWEJYXComB7GpqJVQvI0X2kNjYi1vhTjLVCow501AQmEuuG6VcNgLNNT50+2v9I4kAL07BO0QyBJrT0EskCXUqV+tJoyk1C5bpvOR7LiJiYlP9+3bF2P98pCw5I5t4BbJvFxZG8jbyYoU6GvswuR1KMvhog/jNZpgx9EUGBa3GK7m5vnsIDN56FNCnSYV3MnIyiUiKzdf9LHgOyUHnAacUYvEDY2XsLSgQ5Bt0bLRgvG0e6YkK4ZHINjLm5lRb4TzQShbQSgsLfWOzKPuWMjQHYD+xXVMcBG5JZPRoMMW6MAAf3i4ZRNh+/PJ5bNgZJ9uPvk2DevZWe2Q1WpExdeQNz5qFvsYR5rQd2aO3hAkMI4QDUwy58j5wkyQzf4JBEeRnZ19DHw4VsUXyYoUh5jMhVsJ5zbpcdGqkeHw4YiBwhbozTPf0JJvxCuAvxMrYKuAppjWCctTTC6SNvQDelrQM4LEpfiHOM2xJ7jsIBCdvSscH1wr0xbqm+1hMpWQFuzD1q1bE7mtNviyHnxpGcgi2Gxf+Dy+ckg9KItvwXWIOnpdHysuL/lhO0z+aq3X6vDnuNehTeMGSoegBytGy5ZlX1oGkiIsLrU6+/iUExeCDZAuAykOdjzewT+qQxmZ4PEtGP+DMS9ivIReqPDGVsr5wrFs2bKFDz/8cPTdd9/9EGlDO5KTk7e0a9fufT6ZucIkF3x0GYjICicrIhhpwdiWTlCWw2WIXvdBPf959jy89MlXcDD1jFfpECtc+yt3uhFMz59rejl9lKwgMDNlSUnJQPYCTzcYDLSGoDNZcekgjLlEXvneLfW1adOmn00m08nu3bv3DQ4OdnoGzI0bN27p3bs3bkU7n5SU1KZVq1bPBwUFNaGeq4ytW7fueeSRRzAVxzkmmTjUAsWsECTkJYfJBia4Dw4LB/VnkurodXGXTHSdmvDrx29B5pp5MHf0EDUD7xHAGB0Nv4O2K2sAFikLDAxcsW/fvpaFhYXvsPHoAmnFMxHYJtbtnunKlSu548aNW96rV681ffr0Wf/444/Hnjp16h1GkLEOm9kZ9xszZswKRlRwGQNzPRg7duy4LiQkpNuFCxdeMBqNx6in/BOXL1++MWXKlERGVDBDewnXXamv64U8KzLPiiUcXzkEAzJwiegRKNsCXVGv+5/JzILXFq2An/Z55nub9vUsqFYpXOmQX5mONUcd+7JnRfSu8H/itvug9PT0wRUrVny4QoUKnRmRqW5XHztz5npeXl5xkyZNqur1nH/88celw4cPZzISXspmytWioqJqkmeFD6pV7hGWgJSwe/fuhOjo6HqVKlWKceazYDsdOXLkArvf2YSEhBMZGRlIgDHQ/TyULS1c5bP1EnZsx/Dw8C5sthvF+lo9RiruYX2vup33S2f3S2P3w3tkSe53RfQOIEE6dOhQp7p16w5j92obHBxs15oZI0UFv/3226XatWuH6dXHxf69a9eujC5dulR94IEH7qxfv35tZ7YT3o/p7RzT28nz58+jri5zj0om1yEuowtbl2kZiMiKFuKCy0RdmUxi0kKv58Akc5v3H4fhM5dAXmGRR+gOayrhLigVPMF0nEhkxWbCYuCEJZBJCGAIxBtvRHfu3LlDnTp1HoyMjKxXo0aNhpbOT0lJyWRGIGvnzp2Z69atu8hmsdihStnAUMJnauJsTZyx4b38JJ+qwXx8rDTzaxWya99gxCWXG6J8JlioEpNYhbMxJpwTfJQgsDEZ5bFjx55u3LhxHU8hK0LWXJX8H8wADtuxY8fvzEhnBQQEoO5MTKeBRqMx0mQyVWU6Q5KAn5H26k3STtjORbxtcrgRvMQ/c3h7lfB2D+X3Q6/yHf7+/lXYZyRvwzDJc0j7iUF2P2z7Ym5Y8frZ3NiiiNtvi3jfCxD79/Dhwxv06tWrU82aNWPuvPPORhEREXcxuY18FBQUFKWlpV0+d+7cDWbYL27atOniwYMHr/NrGSV93CjzSMj7tp8t/ZtJLtMF9u8c3s/xe3+mF9RJBG+nCP5bgsDGVQt+v1KutwIm12V6Q1KZy9sKyYpXGG4iK04kKzLi0o57XF4CHYNyM7Nz4N1lCbBs8y631t3qyaOheyvFJFyaA2uJrKgSlmAc0LnBQMMRwQbPSE4GKvK/4TFBbCCTGhNxEDRKiEqx5N+lEoMjFktTHcw5zJLr5XPjVMBFJCth3ACKn8H8Htq9d2lpo9is9j5PICuY2h8rQCuBkclj0dHRE9k/M7ghusHbJ4ATBWzXKvwz3F69SWDi5KCAG7wc7uHI4fcu4O1o4IZWbK8Ifv9wSfuF8Of0l5EVg+x+Jfy6ImG5KjO4Ilnxk/TvUGn/5n08ghMC7N8hSOhk9xf7oYnrsFgiamTFYGP/viEh40YJuQvj7RbG9RMI9oVYlErIUR4nLFK9iSSvxBfJSgAQHCE6uNa7h5EWDB7DKPcx3OviEO6qHAnzXhkKn7w8GPb+kQr/iVsCF7Kvud3v79K8sdoh8321aKGjwPgVRljEgV8cOKWko5DNwPP5gFZBYkiCLQzmJslsV20w10pU5IapiEshv7aZP0M+f6YQ2axcM0JCQgo9oc0wCZwaUUEsX758O9e9ievfJGmLAol+iriRsktvMiNolMzab/DrFvDvTLy9zBLSYpaQnHxJ3wri7eqvYPDNkvuJhlc08mK/E/uz2Vr/xvNYHxf7t0jGgzkZ8JeRcZOEWEjJuElqG20gKpb6dwF/riL+vR//Lp/rM5Q/myN21SghemJb5fH7msCHY1eIrOhDWtAgY4G+9Yy4NObelrcd9bYE+PtBx2YNISV+hpAdd+73m2HWqh/c4jc/1am1kBBPBRRYqx9hEQdzcUAvkgxmUkMiNSYG2cyzRCZGmWcFwIJrX2XmKRomo+y6Zn6tAImxDQQ7KscysuL266LoUQnqqZ7f78SJE2fee++9ozKyUir5d5HEABdy3dmlN1k7SY252HeKZM8hEodiyTOJBCdIYoj9ZZ47kD2bWUaQRRIrJUelsmPNMuIm798hkmcItEA4TLI+KO3fZpn30N7+bYnkB3KyItWPvTlR5IRN1FuhjFQSWSHoQlz+ZB9/MtKyCsq2QOOuov6OXhfjQyYN6QsTBj8Bx06lw4hZSyDlbPltFpn2fD+1Qw5xzxNBf8JilBkd6SAeYGHWKzUeRonIBz+pV0XrYC713Jhkhs9PNgv3t2cgZ2TFbYu3iRWetVYtnjZt2k+Smb9JRljMMk9BoQYvhi1GUNoHiiX9wCxrS7PMW1HEn0FOVOTPY5AZeZPkGkY5gRCXMvhSgLX7KvVvqWfFLOvXRgueCPmSlcHO/m2SECBLujGA/WRFTtqkxIs8KwSneVs2oDDigsFymI9+OpP6jlzXz2CAmAa1IXnBFCEQ9+vNu4TdRK5EcGAA1KpWWe2wqdQL9CMs+MlIS6nMEIgz0AAFY2K2YDxMMuNgbTC3ZYCVuvXNCrNZm5cySkpKdrKPDlb7Y793harUemSdtZWkYMVg/LcWLF26dNfy5ctP8jaTLlNIyUqJhJBqDgjVgFKZd65U3gck5KFU5sWTxjHJ41QMKgTJLPMclcoNrsJ9RU+QtG9LvYbWyJH8fnr3bymx9LOiG3th1tpOvgYKsHUgwNZWMNKCQVi4RNQbyrZA6wJsw7SMi/DqvK8h6fgJp/+Or98aCb3bt1Q6BF23teyJV6EAW2VItjZLB0olYwIKg20p/NOtbO+M0NK/QWH2rRnXrl3rFhER8aPqQ2BV6iObwJTxO5hSd/2jYq8us7v7ewixKf4xPTSTFMSBAwfSW7duvRzKtvCm809xd4y4G8esYFQdTbVutmKAwZoBZOTBoGLotXomNN3Pwn3lRECtf5tlxM9S/7ZHl2Yrvwmc0E6W7leqRW+eCNoN5KZkRUZccKTD/CNonXXL5lhYXAIb9x6GkR99KWyH1huhQYGQsXquWiK4GUyvr9tzfSIrdhMXLbNGufG46bVxZ4TFpWI8js1BtkhWSrPThRT6SGTEVPr4nRqRwQy0htAIoRgh1vjB/yNJsQfnzp273qdPn4TDhw+ncZKCGUlxDRe3pmIAKS51GH11xqyBtMhJgVof10yMCERWCNrhz70tnbm35U69LpxxORsmLFkDa3fu0+1hv3htOPR7qLXaYd2ZbKGmJeiIxUyGeeKDDxo0aNM333yDWR8zOVlBwTwnYkI2IXiSjCqByAqRFU8Belu6MEGvRDu9LlpiNMGOoykwLG6xsKvIXvynVxeYNeoZtcNwHSrGnpkwgaAADK7EXTSNPOWBjx8/nj169Ojd27ZtO83JiUhW8FPMcyLkOPGWjKQEApEV3wNmx0WPy1jQMeHc5ZxciFu5ERYmbrXpvP8N6CHsRsIaRyp4gkkiNR/BCUAi/6snPOiqVatODxw4cC/cnjH2AtxKnY6xKjfzZ5BXhUBkhciKpwO9LZhwbhSTHnpdtJS1+/6/Tgvp/bE+kTU0vKcGfDgiFrq2iNZy2RQo86oUU7MRnAR8Fz5lEu2OD5eWlnZ9/vz5f82ePRs9jJgoLFtCVi7CreUfMSmbsJWXyAqByAqRFW9CPSjztmAUqm7eltz8QvhuezLMT9gCRSVGiAwLheYN6kDv9i3g0dbNhO3SGoHZe7dRMxGcDIzzwqVSjPGq6Q4PhCRl0aJFqbNmCUlxxGRm12Rk5TLcCqoVs7mSV4VAZIXIitcCvS2YcG4AkyFu8kwTmLxHTUNw8XvQm78HSOIru/LmRUVFpn379mUlJiamz5w5Mw1uZXvF5R30nFzl5OQKl6syoiJsVyayQiCyQmTFVwbsR5h8wCSqnJ5hI5N/UVMQynHQ8ztx4kRMZGTko4GBgZ3CwsIeZJ/V9L5Pamrq1YMHD2Zt2bIlc8OGDZkXL14UiwCKqdHFOjFiETrRuyIWDcyDW7VlyKtCILJCZMXnEMpnl0hc0D1e0UX3xcH4fihLdEUglNegh2uVYvVeoXLxqFGjmt17770N77vvvvuDg4PvqFOnTj1GDvyqVq1auUqVKooZ3U6fPn3l+vXrxRkZGbl///137r59+y6vWbMmMzc3V1o3R1ovRiQqeRKycp2TlFy4VZFaWo+HcoAQiKwQWfFp4ECM8SOToGxXkbOAHpVRRFQIbkJWMJYliBOWMCYREgnn3wnVqdnx8sJzt5UpYCRCHqglzY4qrdsiEhUpWbkh8aLk8e8L4fYaL0RUCPTeElkhSID5WtDj8hLoF5SLs8NXmCwi9RLciKyIpANJSAgnJhUkEgq3qvdKK0FL686olSuQFoOUkhWx6J5ciuB2bwoRFQKByApBAeIW6DFQ5nWxFz9yonKCVEpwM7ICEtIhEpFgiQTJiEqAFaJiqaKwWGdGWghSSljEKtiFkv9LKxvfLKpHRIVAILJC0IbGUOZtwV0U3TUcj9su1zL5gcl6Uh/BzQmLuJzjLyEkgTKCYqmKr58VoiIlLNKlIBPcWtqRf0o9KURSCAQiKwQHEcWJC0onPgPFoNmDTHYySQLyohA8j7AA3F6V2g9u96DI41QsLf/IYZYRFilpkYuZSAqBQGSFQCAQ7CEu0mUeOUFRquxrtiJSYnJbxV8iKQQCkRUCgUBwhLwAWPekKHlWLJEXIHJCIBBZIRAIBAKB4MNkxY/URSAQCAQCwa2JDamAQCAQCASCO+P/BRgAbQeQzmpFvYUAAAAASUVORK5CYII='/>";

                    contenido += "<h4>";
                    contenido += "Bienvenido al programa de inclusión digital, su curp:<br>";
                    contenido += "<b>" + usuario.CURP + "</b><br>";
                    contenido += "Ha sido registrada con este correo.<br>";
                    contenido += "Para countinuar, acceda en la siguiente página:<br><br>";
                    contenido += "<a href='http://tabletasube.itleon.edu.mx/'  target='_blank'>http://tabletasube.itleon.edu.mx/</a><br>";
                    contenido += "<br>con la siguiente contraseña: ";
                    contenido += "<strong>";
                    contenido += password;
                    contenido += "</strong></h4><img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAATgAAACXCAYAAAH2RXdzAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAFxEAABcRAcom8z8AAIn0SURBVHhe7V0FWFTZF79up7u6rrEGbTe6thgM9hprd7dIGjDDgKKCEhYqKiHMEBZ2dyd2JzOgKJiY1Pmfc98bGGBAsP3v / L7v9937br03d84779xm / x9QqoHzs4Tm4ULVZTg / CTQPoU1CwDVggVeBuwHX9vIwjIv6qiZs + 6M9HDMcnL9aDToFd8YZwgN7U7hva1aAfyLjgWKcmTLuQZaH8z4KnBoIDwkHy / Rde6mibdrNmq4Q9 / fMN97sl1nbM9IkOZoW4OGWXsEbYu1oQNdEgmwzcGrgeQA4RdxvMBsSG89 / 483K2gRDufHBz9wcRsJTh4I8nHbteB2rleV6rBI4ESC3 + IY5rgbmGCXE2a41fdDYD4j8Wgeq9PZwIBeAFaredTqorQvyYASXbcOYfBtwuorUoO88ID61NwKQs69Yfz8rNnAREB++4cFqt5M1qdJ7FhiP8udp7tmamPKIAsF2BQhcvp7ZrnQRQzPR3q0Ba + cWLV5xvGzod / hBo3lB4qVOWFjIv9F + uMKeW4HII4Oifbj7RgxZCpVHVeWZ7FY1hGm7m8Ksgxa51khBQQ9XalIE / O6 + cfLXi44DvrnNxKh8oLvPvcqjqkG3OXWBHs59V5OO9HA + R5tlfUClqhVb / RjYmiRgUU + ArX4kxIffyd8PASi0e4DJe / vRORFy20j0vRWujSnoC5EXlOp9bPl91HuqZ + hPQVfNFCp7 / FrYsDBkFNbkKrEGRVQYNAcqDp6fQm6lAb4ZceRPtDOFRFtj7iage2KYCcT6j + aumOwtoYgJF30CItQjRF8WFJeuBE5ZVFoJp + UZNyW / tgJ + 4N4MEsSHJDf55vGJYtRbIDwuHR8wlq16ACzirsDlCSh7D7P86m8WHW1A7lcLD3X / dsEhQHAVQn7th3u11h2SUEWJlx8JAdF36VtK5A + G7jeh5 / j10T1R + IBmnKmP4m1SZzTOuBZz5wLz4d + KPt1oMbmy6Ms / LCe3ZBLnCcxKtpS1kEmYlUsf1sqlH15fZKzb1zyNROrLJLIF3J8r2k0qggUtYS1l / G / BTNdY26nAWknHszZTAK8fYiGnWctJfzArqTf6J7DWLt3wRm / +ayyl / 4g + ARLpdmbl7MJaiffS46NBERvF7bqwGyXEkE8IOXwl + gQo1MLDKWJr5jDRs6f9YNBYvtqMvEcGaBQLJDNdsH610677pRns / 6sHnDezzp + eQjVCipa + DGJIPqBU9898INWPGX6C99EobnTOPiz8rUHXOtFD7vuzM5wwHt74StVJoDKf9sab / YomejEfwUwnJRxva5LEI96IoBuZJjlhQUwR0UwfyGSboriJPnmH8HCzDmcx0QkJjebBg0bzdX7GNChnuwzITC87PmggPVyCY9m / xKg3wOfYal47c1CjE7Ka6CFkom / oWaIev3ZYvZM5oIk + aZ1QC43m52kJ10KzXPSymj1nooluArfHmqrEoHxC20Qn1y2zpUQmerG + 09PJTOfXoolOpAcjU52H60CtLlOhwqC5PP65vWG9JGujAr71tisiuIluE3mQ2UaeF0Ozov2UG6yb / BfxirEO00o8aOSnFq / yBD1cGZtlr8ifYaIHnu7B3TdiyJKrZKaTd9iy + iDb0hg891uA95Fmz3n8O6L8cK9iZW2WQTG3tfCDz64JbDGa6QVCNy9o7lIL + MNta / ycHi5HG0IRa4 / m0mS2Es0lMs / JXCe / Uv1ATJE3hvt / ixZKpQ9rpgfd + kH0CVDeMRd9bwQ + XOX3Z6YLNXMHTfMr6Cbx2gqLW4dfi / lYiyOZMq4VC4vNcTMyzysMnH264uC5GWZ6O + cwkExUcEX8yM6IK + PjA8vCiSEGcHqEGZwYavwBa1QLZKKXcImCkvKVbTVmesN5u6CO73Y0KoW / 9fXORVc1Jvp90Uyn8LeDMnYbcjf3K9TNsXHjhq47i4zPUui3C47V + mbhoUbk / 2bhYSDTnPyF / A6CWfj5zIeTVkH / +/ qLCwLRRGeKC / hNjub + 60 + TR5CrMcmT8K + lpJrrlxPe9kFby6uLvpxoKa0m + nTDUqpg7Yf / hOb5AdZSbozWbw + 0osPEuNXon4qWdhW0qufysCywks5hFvJiGNkBbfub6I7GsATWes73eN2LpyFTXeJsxf2EVnLhV1rJbFhrV8Ab5tlnwiGRDkSuxPIv8 + tWsnboP8o6yn / n13roUQBorHkNP4smx6dCxN1XWSpDFzW9ctkrTpsrErJqL6X6tSZO + WO99OXf1YWNhVvAjj87jRVTvD + EnkcNexqujygHV / CLfmW0yWsx5gMgLC7nj8 + NYbEzeJ4FZ6LYgrOQwUUXtXjOhKfRKteu + cTULUXawp6S3eBIuYFwynQ0XKhkD1erOcHNWq6bePp3wDdLo + GbRUeA + Z + Exw4mN5Psjau + nGjKv5WPHUzh5lizGmLS9wTqawu9LTIm53cu5PYDHs6JaZaD0EPkviuKue8BNk1DsaWmwaJLwObhd5bzNGDBhcQYjpja7kk0AhRf3wfuN5wDTxrPKy9GvRX + lK0CYnG39Vl + A1Uc8b6t6T0x6D2BfuB8lJj5ZwQ3O / zO + gjxIv3OleXhDquisMUIGbQOy1pxU7EyNY07YjZcazJvTKLYwhQ4v7UY9dYoP3g + aGg62I + PZpG0PXIoyHhgQTB9H7Dpe5EaV0PtcOQMrTb / kCXN2NAA0HBRf / O01xOMMwejCGNCIQtHC + 73o0LSMyvMD541FcZ58gsaH6KWMY0RUQPUeNTidDEqA / iqFn9sW6Zouty0sBj0AUB9DHnRaVOamDInus4EDQ + NrZNGXeQZFUfo4mPMunsD6 + 6D9IZnjTIr7HnTxW8lCebtZK41u7hrVVy2ig85BywEPxLE7Ag + VVr0vSOsIwRJQP5q7ZHlRvarGt6Sb28C0 / c0Be8jzcHnUIvaYtTnAfPh32oqjnoWSk6MOEjBFfr57K08YA6YDpwt4ekU + MEKPCk0D94r + s7nY740eEg9EzSASL0TNIhI3Sc0yqnppeA81LSDmLNgWP8C2NpnbyVlb0I5eyUfOS0mj4Lfp2 + CuqEnYUMfE04xyQdCe / dpxkObpGepuBUNZGIsm7WvqcvMA01viLwpBueEMjaOu8tv / MYUqhnc7lOot + FX + TYLU + 1iStUmNE + Gs / A4J55Ood7CzZb3DOqZOTXcBKKRYtBngjBVa7bsVgOmjHHiXU + hMQOwoqoxxe1G6A5hwaq / sVKs2fJ7v6AbjqbMdhaqXoNxK5kyrjELuV4cyxiHldgfyzAWS9WJ6v + 6j6zZ1R20Waur +/ 2aHeVAFJNxdB63iIe1G70AYsebQhySZtuQn5gwoSqNFnKT6qCjJWxEidRwQ89yFPcdL + iTIOzOXdH3zig3dnGLcuODwGB8cEYFlR63uIypQ7CJwbgAMESKwRwtZqzjYU3c18ATNEWeOGSVuCd2RpAU5Q43x6FRPNowS9wd5795eMqDuFZi0EdEWKzw6inUCrYyUXgw6l8kaaSmWKjKRTC6VY58ekBY3NMcTTQdKOy5Jajw7H1Q2GsHFJ65DX6btQ0Ke2zhFJNw1A46xsOqLTnMDV9Nj5sGyQdCIpOmN + eVmiStBS93LorOznSAP8XkXzACoi0zevuCotezwNN2LBiNdboOFHr + iNU23MBrChPTovt8fg / e85e + uC + 8WuEc / 2qCCb9OQilMsi0nVCz6U6bUgTT / XvB6El1 / pM7fT4rW8jKsvveP4lVONJWakYN6qzCyJA8TkNn8++OfX0VfAWHlnDmJxUomzPKSSN2420Jem0lkSmYlL4phtszSqRW6jsLkA9kiJpEX5 + mspO355AQruSn6F2IaYdCEJioQLJ3qM0spppceYpaykeie5OFW0v68h7SltBZrJRO + sl8WQPgHJPY / Z3EFZGmc6wbllwutBWwOcTc3yDFdfdsfaaSJdVv + NesmTit5dxTCP2c +/ im78Q / pzZ / DUmbBWso64p / ShFm5pGG4N8YreGor1yf4xwZgfDt + rYceeuihDe1eYZq0o0c + oa84HcjPnLMsFRePLQFxeQlx + Xnd7T0Qv97 / V6BJe + HYFNKuEF0MjxXGBnTFaaiNEFVV3sTC8J8Crqat + LYORP3UGHYVaff + pTQw + gC1GrrO8Ei / PNoUaN7KmaGmH3CtkEJ9I8ePz40aaCYhZkxGvJrpaqCMPa / JV8bzYPqqHxvApt + tYFfxLnC4bD84YTwCgGy594FlZ55SpRVbvAvu2pjCAzukvSncszWFS4PzO0WvIFCou2SpmLyoPRnI72zm0KD28OBicd6iMraDdr4NhS1pHBUOlO4Dxw2Hwdny1nC5ygS4UcPlvUgeDQ0SH9sYQIKdcUtsg66gtuhTZCJWoJjsPYIqQ7tyIu6msdCrhfHVbZ8lXGCEmCtzviSfQ4mN7ewQF4QRTabuST36V184XLp / m1OmY + ZcrOQI16vL4HatqRBbxwPu / O11SMz1Vvhl5jZnmqVKFIM4sNKuUOXRSJcY9B6hGS8lV8f0Mh4XIo67KrXiM8ZTkR4Hc + bLGFM9BaN7CzMZtaGuMwPu1vOCew1mw + NGuc / zzA + KS1fYasZVxSCOJw5mt6niqAtJDHpPWJrwa8ZKPWLg9RZiTCaCb2XGL9XSXzRdWCqu7HPdkfXBZh4tyacPe4gr / WbsHyPGZOBhg7mQ0HAuaMZWxeC3Bl8dSFOPkWWsg2TI40J / 3IeoOIL2YPT8M9fF0Exoxy / QGm7THoyemHX0nC1Hhc8HorcKdN0qjENo4QGvMKHSHjVe8M4 / rPzg + VczB6Tnwf7xjdOpwh6ifjsvr / wBusO1dZXA3mIM47ora9xDMSZzqSbROnOt1HMHw47cQ0s3aV44Ld8k2q925OGIQhPWZIytisx93FYH6nSeCtW6z8gxCN28h3RRje7TYWTfIekqa1O4a2sGcu0x3vcKj4MXsozW56DWqL42hgYCG6IZyQ + CO9bl4JmDEaRMFHtQR4Xe1IzcZ6f2oDSRpy8AqOKq9 / CASv19wWz4IjAYu8SWwu / aGxd / 5mCg3WH5geGKXyPN + lrNXI / s1 / JtWZe49pm3VLMGl6geW4ZPf8ioOMLgJRmT / TW0 + 2dBqnaloY7LuhowHzDvNCWj4kyHLQDDMQE5poc + mljB8KmdcWPx8gNg8vq7Oqc9ZBD1l9OWv8XUWdF11pOMKRD / eqUnO5SDVO2KI3T3SROmP / hA6X99skjaw8bz + 4mpCoTsFWcwdmnGUo9CIeeShekP5 / Bjdyrrerug6FWi7z0gY + E00g4 / 53YrHJjt8jPI7WxcZP5Ge9q7rWDt3O6wdlNmiSE5ULTdNA + ULhXyzKMm8955KgVW3LEMiRubOURY2HPbPc0i7d9mbO4mBqNBfqnAKiFviPNGOLUwMrRhBectTWHKziYw86AFsmmCGPXZoMqgeRkVV9pBkfH8VQbMhSoDUYUQAk / OYMGn33OlEWiZDfLrIXPS6zpUj6Wgfv5 / Pxwb3gAmrW8MbjuawIx9wgr5HCtcPgOYjViUUXElnFdCkSnrYn + etbXF1767z7OFR4APJX4QDPBH5T4f / uz3D / ztUAPaeZhDP / 96kFvFeR1 + y8pbdvEP0ffeUW7M4jGaiis6dT38OnMbbOhRlk + 4uRAy9ZSY7AOgmxf83HN0mq6Kc93epIfH / qaJmorLU + po0b5S / ZKPzIeoWjNFXCO2TOyGWnVXGEqk0agwdVcWqSrKwmN7YPz7rdA5m74nZ08 / A6C1WtfCPmTFETrNzCJxNisbZakgmqU062DT63y20iGLTKWrjfDYhkyhCsXKu8ivFerjwtJTVQRWVj2Mm8FC1QtZqCrTkFaqlou + 94pTQw2AZiupIz90xX0IhKnfPGZJa9c + AG6MMuQdmQkrp31mFadQ7eOuUrUWJQsZuxIlKYKFxrhi2C6UulkYvoUpYmbyLnNFTEcWGlsBK6oi + m0xzRqeXxGzhbt5oFZ3j4zpXTXI7TYdGvaczqdztR42O8uboZn6ddfGhE / ruufSEGLHGfPpXrFjjeDZ / tCzlO5FgmrCOtSD2tO8LoVN / 8CWQ / gtQ6a4Y4ASNYspY7vxGUjKGEu2LKYxC4upg + HVWbha2EuGXtew2OdYkauxsmnF5B3kSSGPejzvss8DFYf6AV85iSw / cM6hikMXcL / 5iPl8FWULe2z6aYHCKiIf2psIm59QT7CtESQiNcvdn + 8J4BugEC87WUDMvEF8pSVd398Vckks6iNDGfteN6IwoO4i6yAobb0kY8mg4filFapMUPJ5cA3laLhrgebMYUsC280mvDvp1Yl1m8Uolnp85cknWKE3xxjxeXCpSY9sxCiamPMdhd0YWTb3j98HhzJukujTjVC1UvS9EcWlKxM1u9aUkK2CEvK18Kd0xUnjqeuAlsbW8tqc5YdSGPGFozHvi3t9fNVWMYqDZsXfR6nTLJ3NWD4rLqFNsPnYG8toEH4nDfWXB1beKxasKo2v4hF8JR / xBc2hMUo + BqtUb + VLwhU3O6KO40Z3Xvhp5u6Sv87ceqewz24oPHM7n1j4lw + 6Hlug8qKs + y1oJhvmWnF2hlwSuTTu9j + RY2Lh9gVnxKQfGTQUGBZ3CCvnMVaMAQ9TqlXCynD8aCzHiguLS + SuMG94B0 + TC77xP9iQb0WkNSb77aIjYBJ6Er5deBi + 11r0fzbx + cxvFx7i66B1VRy + jkX4pEKUOopLORZ5XIz6PwS1M2mGJe / tODmYBUSfpWu7o7EZszEprviKK5nXyLjtAVg5ZqjrjOHlCqfH6WHW / JqYvNmLVxyRKjhtwb + QMqslPHcwgtfb5 + U + o / 6LAsBXbNnZLJXCGXgqvCpNX9UK2xL7eAVbmjm9VVNR2kzzbMolNO3uVXda7J4lHqUxOdLxE + m4zwlNnErh61noMIBhOkDmmq5ucmEMopVTKXLoi8qv3wkt5AXfyCs7zP3z3ixMg3cZD5A4GzErWTAyFEmzMGknguNMIu2Jrh9rLS / MJBhOMz4lLgv5fl00U7ON2wK + 2YCV1BPj1cxS5sLzSJybYjkX0O8r3iEPtJYLVrtEFslaOjfDG4zBAh9hYTbMUph4jAUdwgIn4fUR9B9E / 4mMcA1aySZwVyI9xl1LeTnuWk22565EeoZZyH / AdEdZK7qPTBjYpvsQNGV + MbByEZpNrd1oV7ZXWGlb8Uc9Zc2nmuC1LfqvMCvn1qy5s7Di2UqawlrL6mIFb + DXBCvZEtFHFWHB96bIqDhNxUgXYSVWRNcZ73kZyz2N5W / jcZq5I3S / d4HV1LJYPs3x3SVckyQ6zxT8PFxYXmXp3Bh / Q2fBLx2KzxLF / QUGSUIOaE3HkszSnkidCdprQwPNBGiN7uiabQq95h6a + EwI98lryn1 + YSWbj9I8AiviPlbSfuQNJL6KVDnSzay1tBbGLeOV2EpeGyuwH76eHvhHRool6KGHHnro8TlBcacpNlnlyFBszu5mCvV1bN4m86ELvtlZ / H94Ex093j / y2myIOqH + 07s26fH + oRc4PXIFjV4pYnbyLTFoK34N6fOXbybS + nx3scS8BY5IPcq0cic7ab2 / poc5N9DiFNq6gz8vCi89a9RjsLCOgJXf / Q2R39a5uuLHel3E1F8GFpwtwoKiA1hgdDoLOQvfBZ + CIBsrODrU5OyRgcY553B / cQhVrWER + IfpEoa3IQmLNpbdjGLLbgELQWpcXcwZ91QsISsUMSs0K6C0Wd51R + qaQjWA9vxb / WMjWPdLc9j8e2vYXuwf2F3iXzjwV284WnbAe94p7T2BhltCzr7O7Ow + DSPdndJujzUGmuqs2V6FqOauGVwcYSrsjvTFQKHen / 1Pey9UqM + JdxCw + GIU86eFTAUgpV90oa9YggBl3A6d98OXZe3XtWHtzxaw8TcJbCvaDnb92Rn2luoBh8r2hSPlBsNJ4 + Fw2mwMnKtgAxcrOcCVyhOTxVI / PQKjPfkmIRoGnYFtE5qn0wo1WohAa4ZooJMWXtFueeR / hOE0Qhw33uSZWMpnDn5yjY4 / LwtjgYXHCTuNZl8MpoukdZbdKMfmXM3s / STMORnFF1PM1cEF54D57DdmIad / 5py / +xdOzzVZNwpRqMQ1VRqK + 1KisO341TJ9R7GOqMW6wcHSqMXKDYTjhkPhpMkoFLJxcL6CLVyqPAGuVp3MF5fRCWS3a0 + F + DqeEF9v2ie1H7 + fecDkh3kH4Ie5 + zJY1Hdb + uPxBiFikgw8ty9f9 + VEGqoTxjxJAEkg74w3yeyq / yyhVMflEBZthsVdEVPmhEK1TGceDcmGyw6PA1HCyW4Hc9L7WM4NS7Nj0YWbzP8Car2c7Dwk6PXRMv1eiilz4Fo15wM3asjhVq0pEGM + DWhRXlzdmXCnnhfE1 / eGhw3FydefCL9N3TCviPtGyOQG + H3qRt2mBOKpg0mARuCIpO3u25gWaMXSx0cIags6Y4TzukjxOviGjk3TsyHgpgULuqmVX8ulRYjLbmRd6OK2PYq57QBOWtyj8RPd9 + YtcPITP7F5Z4D5HkeicPogyeU8Dn + 57nzj4JX671kt7tXz5gJ2r4Ev31VXWNBI56TQGpYFumfsfgQUswktVWpSJJSaGA6lJiDJFfw5lvb + 7LiyefJE2m8rU + Docxs / 3vgzHoeiz92SK2Qf5cKLb27R + Z8vl2sZZHstuNhTTClg0too3YugkM6bgVmvzV3gph / 9gy / Lc9 + dC3fJxZS5Asz9v33UaD4kNBKETLNyNoMNFwwVk34SGI1aPMR41GLarDcb / QV39GIwGLUEXjkKc6 + IZMuRHXfftqDn8XwK0Cpesqk4T4oUr0mbzDlRX0yZE7PIzkK7KyNPNpd2PsgOm8goZrMcdNI + Cv4cPT / v9ZOT16czpw3AJiOzu84bUXDXZz30TRvY + jvX0h8FK5uQiXzWdDGgQP4kpn7vqN3R9UKtTlMqiZd5olIfr9gqfb2gcja6DO2b / sAuc6IzChk8sDODSyMqGIpZP3N4HyrKV0HPPJQ76agunubgSbS1DvBrr3zk8TzUR7xLJkYEe7ORy0AnRylhXX / D1zcHG / L + tqd2Bp2e2hs9T59snLmr9Yjg0sx2JbDxkbmThJdWRFpH3EJGa64DOyxJ0SVoGiY2mZ + xje + HAC1Upi3Haelold4z + YK9CoPmHDMbtUDn7rRJjsYtye3adbx0WNf + h88MN4Lzo0zh / EgT9eVRJgGn + 1b / +fkE09wVwucLKMRmoNE + dRewKUhys5PCc4vLTvrsyXfkPt2OVqYPXCgy60LqCf3bpKjGluWLrjWkxddZtlEfjlpoZCiwYVjOsMA3c3gQXLRYkKZLyIiPmyyEhAZ + ncTSPwjM29sVyypws / h6Zdp5nFaQ0ifTcFwg7UAezzd3FPFkklF5 + u3pjhV4S51Oi7s1wOAHkFf + 7qV9 + Xc6qPfTwX7rz3wHEemmd6PLVvysbZoilpo3es5 + xXr6Qg72mAtHRlZOy1PgNOgzrzPr5wes91xgfXSz9r9zU5 / qEDLiw8YLyF0tlvZBUbedyzDzjm55CNxiMByzZLiYPAPgb / 4tOLJfYfzvTfm1 / c / FYTjjsxDBhuW5bzdHwInuou8zgV1UWTYRDXbNJiEF4YQ1tGNLGnNYOU0sreDoON2NdZkJrNP0THYkzoBvOrnDn11doHZ3G + F0r7zQ2cOBdfEEgR5QtqNn2vNGfllO3SABe9l0CSQ29IsELS3yMVC3gyyidjuZec3OU7pW7 + GpU8MZjQ8GE7tw4ZgWLRQKOJmMhOxkS0 / ME5NkReShH1nwmScs8HjmRjKfDazDnjPrcLR3sjMCfhg / z1lMlSukmxtVmbKjcddpuy26ztzXOPfz7fTIgiq9vZJzfFLHBvBDPcrYKw + LyThKOyrHl7FXYHgmy05EW1X7pfE / 8S0LOnWDrwsJPLVCDP0MMVoJbERQJtHeYSNCocqYGmA + 0fw3MVUGhgbX2zMusiE / YISW7dMhI7TnAR00QvseaA4b8T7SDGYeaJp1WEuPLKjed1ZxWoKbXeBobwY6EKW486oXf8jXZGy4WOEfx1 / rdpBWES / ZN4sOS7 / iC49OA1 + 1FXTq7b80Hw39FzhlGu1oxA9YBL / 1H5RMJ9jQ9gaWU8yh7YzaST396vFtDuhQFtoj4k0Cl4WHm10T7 / ZxQIPfkXeEfXo07mcOtOFiaemytsDRhiB0Cg9tCkIn8fw6czvUDDoG2xdIYdsiOWzzd4WtyFsH1ue5BvPzQ09fP9ZrNmhYrM8 / aRqBo9N / aE8NOgEoN4GjfYToDKpcBQ4582DTXIecckXIPVOmVO9loepJbNXVMujfhvRkCtU0FqZuIeyaEOvDlDFFGB1ZQghVhbCAq3 + yqCdCHyDNIiaExllh3oOYryu / JoSq5jBFTA + 2JuFXvMdSMfSTo5S9wrrkxIiEkvK1UMxtHRSdtgl + 99wOv3rvTreIOH1iU69ysKmvSQYvhn6Je4QQunjeJ4ObdZmRXmVUlbwFbjUJXKMc56zjZzRVl8B5HW4Gngcb836lfIPOtAlVdUZheszC4jpz4aEdckLuFmerHuIn5FZFvvI6RGXK04bHluXXpNXoetPV7 / kSdjqpaTWmD1HX4PFhMcJnKpwLITBFXG8s / 5OOo + YXAFD58CADODzEBA4RB5vAzfAvVeC08FU7ef +/ HapdaS6rmdLWwzyt6 + w6KX0X / X1xaFD9nJ25nwIK1UnkSi5Ibwv69NK + CkosK / z9nYj1IUECd3GEIVygDmAkuXdXfG6b + ehBAjqbRdy7L159EMix1Vi7vbx37Q6yk3U6SBNqt5e + MG8nfWreXnqzdgdpYJ2OU3mXx + kLt31PnrvtderCbS9yz19R5bom++LV2Lnaaa + pHwbFjDWC2 + NMM / hwlfsJFMTSL2 + emp60X3n8ydYFp5MORh55pTrnkGVBvRZowf2zOzf7X1 + 76MLBKT1eHnbpBAekneDE7LGJd49v3YLxFcSkXzBoozi++Yi6P34m4 / FTNhEF4SW3w / jJjip3jHuJ9pQXi7iThtfTMC6JhdxuKM7Bm8SnrIfESfgnUKlui / kC0I1FhrHwu / fxE7wY / bex / HBxQ6dgLOMO2m42GHaUhd15iOUGo + 22EsMbsIh4 / Gyrx / PnektU7uUxtkqfWVClF / WvzczCqtmvMV29gT5QtacHVENW7TkDJGOxwZYL / nEI4ukEzoDWtgGQaGsC92xMIR6pcTXUviY / vx5vAs8W9kEZgh / jD6w + trtvGdg70CRfPD + zF + XL3wYLnyVC + dZoD1lojLBWXxG7E68fsJCbNTDOBgVkM1PeaoBCEc38 + TGjj1jo7ZboPuVCqlSPYOExTVBg9vD8tDW4Qn0Mw2NRMI3QnYpx89BFoY7ZjGlN + P2UcUPx07kK / f2wrBSMO8rzK2ND8PoJPkcPfl1AGI5YWMNkpD + YjFiYQePhi1aK0RkwGbEozWTUYjAbvQSqj10MpsP8MthkUuamqNlh5boiS9pm0kigTbVosqWGT52qwou9QcdQm3VGmqWnpg59GTHpxUMbQx5PM0keS2vDxXGV4SJ9hkeacvfa + Crw6sE9YRMKEShc39xf4xV / cSQ2RjDNRUx7y9GchE739gL / l + DHs6qqiVefFUrYhxiVmRgBpR2U8BeytCO6jmEpJR2Uo8rZhJaqLF + eYx + YCrIVUNo + NIN1p63NVeCaem / Jkra + 50Z47mgiTDHnNMmx55Y2kj0s4Kk9Cp7CAWJR28URx6Pmm1AVXl3eHfDqwq5lnOe13Iu7Ax7MkGSkjbU2glc3TuS9Odv / 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'/>";
                    contenido += "</center>";

                    string conversionFechaNac = usuario.fecha_nac.Value.ToString("dd/MM/yyyy");

                    try
                    {
                        objAdmin.registraUsuario(usuario.CURP, usuario.nombre, usuario.apellido_pat, usuario.apellido_mat,
                            lugar_nac, sexo, fecha_nac,
                              usuario.codigopostal, usuario.estado_r, usuario.municipio, usuario.colonia, usuario.calle, usuario.num_int,
                                usuario.num_ext, usuario.telefono, usuario.celular, usuario.correo, password, usuario.tipo, "E",
                                  usuario.pertenece);
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }

                    try
                    {
                        Metodos.sendMail(contenido, usuario.correo, subject);
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }


                    TempData["error"] = "alert alert-success";
                    TempData["message"] = "Registrado Correctamente";
                }
                else
                {
                    TempData["error"] = "alert alert-danger";
                    TempData["message"] = "No se pudo validar la CURP";
                }

                return RedirectToAction("Usuarios");
            }
            else
            {
                TempData["error"] = "alert alert-danger";
                TempData["message"] = "Uno o mas campos no son validos.";
            }

            return View(usuario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        protected Hashtable rellenaCodigoP(String cct)
        {
            Conexion con = new Conexion();
            Hashtable consulta = new Hashtable();

            if (con.consulta(objAdmin.regresaConsulta(cct)).Rows.Count > 0)
            {
                foreach (DataRow dbRow in con.consulta(objAdmin.regresaConsulta(cct)).Rows)
                {
                    consulta["municipio"] = dbRow["municipio"].ToString();
                    consulta["colonia"] = dbRow["colonia"].ToString();
                    consulta["calle"] = dbRow["calle"].ToString();
                    consulta["numero"] = dbRow["numero"].ToString();
                    consulta["codigopostal"] = dbRow["codigopostal"].ToString();

                    consulta["found"] = "true";
                }
            }
            else
            {
                consulta["found"] = "false";
            }
            return consulta;
        }

        public ActionResult InstitucionAdmin()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }

            Session["cctTemp"] = null;
            return View(objAdmin.consultaInstitucionesRegistradas());
        }

        public ActionResult ListaBeneficiario()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (string.IsNullOrEmpty((string)Session["cctTemp"]))
            {
                TempData["error"] = "alert alert-danger";
                TempData["message"] = "Debe ingresar como institución";
                return RedirectToAction("InstitucionAdmin", "Administrador");
            }

            var CCT = (String)Session["cctTemp"];
            string pertenece = (string)Session["pertenece"];
            string ruta = Server.MapPath(@"~\Content\mi" + pertenece + "archivo4.xlsx");

            objExcel.cargaBeneficiario(ruta, objInstitucion.consultaBenefReg(CCT));
            return View(objInstitucion.consultaBenefReg(CCT));

        }

        public ActionResult ListaSinsesion()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }

            string pertenece = (string)Session["pertenece"];
            string ruta = Server.MapPath(@"~\Content\mi" + pertenece + "archivo4.xlsx");

            objExcel.cargaBeneficiariosinsesion(ruta, objAdmin.consultaBeneficiariosSinSesion());
            return View(objAdmin.consultaBeneficiariosSinSesion());

        }


        [HttpPost]
        public ActionResult ListaBeneficiario(FormCollection collection)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (string.IsNullOrEmpty((string)Session["cctTemp"]))
            {
                Session["cctTemp"] = collection["cct"];
            }
            var CCT = (String)Session["cctTemp"];
            string pertenece = (string)Session["pertenece"];
            string ruta = Server.MapPath(@"~\Content\mi" + pertenece + "archivo4.xlsx");

            objExcel.cargaBeneficiario(ruta, objInstitucion.consultaBenefReg(CCT));
            return View(objInstitucion.consultaBenefReg(CCT));
        }

        //[HttpPost]
        //public ActionResult EliminaBeneficiario(FormCollection collection)
        //{
        //    if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
        //    if (!Session["tipo"].ToString().Equals("A"))
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    string update = collection["update"];
        //    var CCT = (String)Session["cctTemp"];

        //    objAdmin.actualizaEstatusBeneficiario(update, CCT);

        //    return View(objAdmin.consultaBeneficiariosActvos(CCT));

        //}

        public ActionResult RegistroUnico()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }
            string CCT = Session["cctTemp"].ToString();//aqui va el CCT que se desea usar

            return View(objInstitucion.consultaErroresBenefMensaje(CCT));
        }

        [HttpPost]
        public ActionResult RegistroUnico(FormCollection collection, string curp, string matricula, string parametro)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }
            string CCT = Session["cctTemp"].ToString();//aqui va el CCT que se desea usar
            List<Models.Alumno> lista = new List<Models.Alumno>();
            Models.Alumno dato = new Models.Alumno();

            if (parametro == "consulta")
            {
                if (matricula == "" && curp == "")
                {
                    dato.no = 1;
                    dato.curp = curp;
                    dato.matricula = matricula;
                    dato.nombre = "";
                    dato.status = "Campos: CURP y Matricula necesarios";
                }

                else if (curp != "" && matricula != "")
                {
                    string validaCurp = curp.ToString().ToUpper();
                    try
                    {
                        CURP objCurp = new CURP();
                        string[] str = objCurp.get_Curp(validaCurp.ToString().ToUpper()).Split(',');

                        if (str.Length >= 7)
                        {
                            string CURP = str[0];
                            string ape_pat = str[1];
                            string ape_mat = str[2];
                            string nombre = str[3];
                            string sexo = str[4];
                            string fecha_nac = str[5];
                            string lugar_nac = str[6];

                            dato.no = 1;
                            dato.curp = validaCurp;
                            dato.matricula = matricula;
                            dato.nombre = ape_pat + " " + ape_mat + " " + nombre;
                            dato.status = "No Registrado";

                            objInstitucion.consultaEstatusBeneficiario(CURP);
                            if (objInstitucion.getCURP() != "")
                            {

                                objInstitucion.consultaBeneficiarioRegistrado(CURP, CCT);

                                if (objInstitucion.getCURP() != "")
                                {
                                    dato.no = 1;
                                    dato.curp = objInstitucion.getCURP();
                                    dato.matricula = objInstitucion.getMatricula();
                                    dato.nombre = ape_pat + " " + ape_mat + " " + nombre;
                                    dato.status = "Registrado en su Institución";
                                }
                                else
                                {

                                    objInstitucion.consultaBenefEnOtraInst(CURP, CCT);
                                    if (objInstitucion.getCURP() != "")
                                    {
                                        dato.no = 1;
                                        dato.curp = objInstitucion.getCURP();
                                        dato.matricula = objInstitucion.getMatricula();
                                        dato.nombre = ape_pat + " " + ape_mat + " " + nombre;
                                        dato.status = "Registrado en otra institución";
                                    }
                                }

                            } //Validación

                            if (objInstitucion.getStatus() == "I")
                            {
                                objInstitucion.consultaBeneficiariosInactivos(CURP, CCT);

                                if (objInstitucion.getCURP() != "")
                                {
                                    dato.no = 1;
                                    dato.curp = objInstitucion.getMatricula();
                                    dato.matricula = matricula;
                                    dato.nombre = ape_pat + " " + ape_mat + " " + nombre;
                                    dato.status = "Dado de baja de su institución";
                                }
                                else
                                {
                                    dato.no = 1;
                                    dato.curp = curp;
                                    dato.matricula = matricula;
                                    dato.nombre = ape_pat + " " + ape_mat + " " + nombre;
                                    dato.status = "No Registrado";
                                }
                            }
                        }
                        else
                        {
                            dato.no = 1;
                            dato.curp = "";
                            dato.matricula = "";
                            dato.nombre = "";
                            dato.status = "Verifique el CURP";
                        }
                    }
                    catch (WebException ex)
                    {
                        dato.status = "El Servicio no esta disponible actualmente";
                    }

                }//Fin del IF CURP Y MATRICULA
                else
                {
                    dato.status = "No se permiten campos vacíos";

                }

                lista.Add(dato);

            } //Fin del IF Parametro

            else if (parametro.ToString() == "No Registrado")
            {
                string cct = Session["pertenece"].ToString();
                string clave = cct + curp.ToString();
                string datetime = DateTime.Now.ToString("yyyy/MM/dd");
                string validaCurp = curp.ToString().ToUpper();

                try
                {
                    if (curp.ToString() != "" && matricula.ToString() != "")
                    {
                        CURP objCurp = new CURP();
                        string[] str = objCurp.get_Curp(validaCurp.ToString().ToUpper()).Split(',');

                        if (str.Length >= 7)
                        {
                            string CURP = str[0];
                            string ape_pat = str[1];
                            string ape_mat = str[2];
                            string nombre = str[3];
                            string sexo = str[4];
                            string fecha_nac = str[5];
                            string lugar_nac = str[6];

                            objInstitucion.consultaBenenficiaio(CURP);

                            if (objInstitucion.getCURP() != "")
                            {
                                objInstitucion.ingresoBeneficiario(clave, CURP, CCT, matricula, datetime);

                                dato.no = 1;
                                dato.curp = CURP;
                                dato.matricula = matricula;
                                dato.nombre = ape_pat + " " + ape_mat + " " + nombre;
                                dato.status = "Alumno Registrado";
                                lista.Add(dato);

                            }
                            else
                            {
                                objInstitucion.registraNuevoBenef(clave, CCT, CURP, matricula, datetime, nombre, ape_pat, ape_mat, datetime, sexo, fecha_nac);

                                dato.no = 1;
                                dato.curp = CURP;
                                dato.matricula = matricula;
                                dato.nombre = ape_pat + " " + ape_mat + " " + nombre;
                                dato.status = "Alumno Registrado";
                                lista.Add(dato);
                            }

                            //  } //Conexion

                        }
                        else
                        {
                            dato.no = 1;
                            dato.curp = "";
                            dato.matricula = "";
                            dato.nombre = "";
                            dato.status = "Error al Realizar el Registro";
                            lista.Add(dato);
                        }
                    }
                    else
                    {
                        dato.no = 1;
                        dato.curp = validaCurp;
                        dato.matricula = matricula;
                        dato.nombre = "No Disponible";
                        dato.status = "No se permiten campos vacios";
                        lista.Add(dato);
                    }
                }
                catch (WebException ex)
                {
                    dato.status = "El Servicio no esta disponible actualmente";
                }
            } // Parametro

            else if (parametro.ToString() == "Registrado en su Institución")
            {
                dato.no = null;
                dato.curp = "";
                dato.matricula = "";
                dato.nombre = "";
                dato.status = "Error: Ya se encuentra registrado en su institución";
                lista.Add(dato);
            }//Parametro
            else if (parametro.ToString() == "Registrado en otra institución")
            {
                dato.no = null;
                dato.curp = "";
                dato.matricula = "";
                dato.nombre = "";
                dato.status = "Error: Debera ser dado de baja de la otra institución";
                lista.Add(dato);

            }//Parametro

            else if (parametro.ToString() == "Dado de baja de su institución")
            {
                string cct = Session["pertenece"].ToString();
                string clave = cct + curp.ToString();
                string datetime = DateTime.Now.ToString("yyyy/MM/dd");
                string validaCurp = curp.ToString().ToUpper();

                try
                {
                    if (curp.ToString() != "" && matricula.ToString() != "")
                    {
                        CURP objCurp = new CURP();
                        string[] str = objCurp.get_Curp(validaCurp.ToString().ToUpper()).Split(',');

                        if (str.Length >= 7)
                        {
                            string CURP = str[0];
                            string ape_pat = str[1];
                            string ape_mat = str[2];
                            string nombre = str[3];
                            string sexo = str[4];
                            string fecha_nac = str[5];
                            string lugar_nac = str[6];

                            objInstitucion.consultaBenenficiaio(CURP);

                            if (objInstitucion.getCURP() != "")
                            {

                                objInstitucion.trasladoBeneficiario(clave, CURP, matricula, CCT);

                                dato.no = 1;
                                dato.curp = CURP;
                                dato.matricula = matricula;
                                dato.nombre = ape_pat + " " + ape_mat + " " + nombre;
                                dato.status = "Registrado Nuevamente";
                                lista.Add(dato);
                            }
                        }
                        else
                        {
                            dato.no = 1;
                            dato.curp = "";
                            dato.matricula = "";
                            dato.nombre = "";
                            dato.status = "Error al Realizar el Registro";
                            lista.Add(dato);
                        }
                    }
                    else
                    {
                        dato.no = 1;
                        dato.curp = validaCurp;
                        dato.matricula = matricula;
                        dato.nombre = "No Disponible";
                        dato.status = "No se permiten campos vacios";
                        lista.Add(dato);
                    }
                }
                catch (WebException ex)
                {
                    dato.status = "El Servicio no esta disponible actualmente";
                }

            }//Parametro

            else if (parametro.ToString() == "CURP no valida")
            {
                dato.no = 1;
                dato.curp = "";
                dato.matricula = "";
                dato.nombre = "";
                dato.status = "Ingrese un CURP valido";
                lista.Add(dato);
            } //Parametro

            return View(lista);
        }//HTTP POST

        public ActionResult RegistroBeneficiario()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }
            string CCT = Session["cctTemp"].ToString();
            string pertenece = (string)Session["pertenece"];
            string dato1 = Server.MapPath(@"~\Content\mi" + pertenece + "archivo.xlsx");
            string dato2 = Server.MapPath(@"~\Content\miarchivo2.xlsx");


            //objExcel.cargaarexcel(dato1, objInstitucion.consultaErroresBenef(CCT));
            //objExcel.cargaFormato(dato2);
            //return View(objInstitucion.consultaErroresBenef(CCT));


            List<Alumno> lista = new List<Alumno>();
            lista = objInstitucion.consultaErroresBenef(CCT);
            InstitucionTransacciones objInstTransac = new InstitucionTransacciones();
            objInstTransac.eliminaErroresBeneficiario(CCT);


            objExcel.cargaarexcel(dato1, lista);
            objExcel.cargaFormato(dato2);

            return View(lista);
        }


        [HttpPost]
        public ActionResult RegistroBeneficiario(HttpPostedFileBase file)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }
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
                        //string CCT = Session["pertenece"].ToString();
                        string CCT = Session["cctTemp"].ToString();

                        string validaCurp = ds.Tables[0].Rows[i][0].ToString().ToUpper();
                        string matricula = ds.Tables[0].Rows[i][1].ToString();
                        string clave = CCT + validaCurp;
                        string datetime = DateTime.Now.ToString("yyyy/MM/dd");

                        CURP objCurp = new CURP();
                        string[] str = objCurp.get_Curp(validaCurp.ToString().ToUpper()).Split(',');

                        if (str.Length >= 7)
                        {
                            string CURP = str[0];
                            string ape_pat = str[1];
                            string ape_mat = str[2];
                            string nombre = str[3];
                            string sexo = str[4];
                            string fecha_nac = str[5];
                            string lugar_nac = str[6];

                            objInstitucion.consultaEstatusBeneficiario(CURP.Trim()); //Usuario
                            string estatus = objInstitucion.getStatus();


                            if (estatus.Equals("I"))
                            {
                                objInstitucion.consultaBeneficiariosInactivos(CURP, CCT);

                                if (objInstitucion.getCURP() != "")
                                {
                                    objInstitucion.trasladoBeneficiario(clave, CURP, matricula, CCT);
                                }
                                else
                                {
                                    objInstitucion.consultaBeneficiariosActivos(CURP, CCT);

                                    if (objInstitucion.getCURP() != "")
                                    {
                                        // MENSAJE DE ERROR EL BENEFICIARIO 
                                        string error = "Se registro en su institución";
                                        objInstitucion.ingresaErrores(clave, CCT, CURP, matricula, ape_pat, ape_mat, nombre, datetime, error);
                                    }
                                }
                            }
                            else if (estatus.Equals("A"))
                            {
                                objInstitucion.consultaBeneficiariosActivos(CURP, CCT);

                                if (objInstitucion.getCURP() != "")
                                {
                                    string error = "Se registro en su institución anteriormente";
                                    objInstitucion.ingresaErrores(clave, CCT, CURP, matricula, ape_pat, ape_mat, nombre, datetime, error);
                                }
                                else
                                {
                                    objInstitucion.consultaBenefEnOtraInst(CURP, CCT);

                                    if (objInstitucion.getCURP() != "")
                                    {
                                        string mensaje = "Registrado en otra institución";
                                        objInstitucion.ingresaErrores(clave, CCT, CURP, matricula, ape_pat, ape_mat, nombre, datetime, mensaje);

                                    }
                                }
                            } //Validación

                            else if (estatus.Equals("E"))
                            {
                                objInstitucion.consultaBeneficiarioRegistrado(CURP, CCT);

                                if (objInstitucion.getCURP() != "")
                                {
                                    string error = "Se registro en su institución anteriormente";
                                    objInstitucion.ingresaErrores(clave, CCT, CURP, matricula, ape_pat, ape_mat, nombre, datetime, error);
                                }
                                else
                                {
                                    objInstitucion.consultaBenefEnOtraInst(CURP, CCT);

                                    if (objInstitucion.getCURP() != "")
                                    {
                                        string mensaje = "Registrado en otra institución";
                                        objInstitucion.ingresaErrores(clave, CCT, CURP, matricula, ape_pat, ape_mat, nombre, datetime, mensaje);

                                    }
                                }
                            }
                            else
                            {
                                objInstitucion.registraNuevoBenef(clave, CCT, CURP, matricula, datetime, nombre, ape_pat, ape_mat, lugar_nac, sexo, fecha_nac);
                            }

                        } // str.lengh = 7
                        else
                        {
                            objInstitucion.registraCURPNoValido(clave, CCT, validaCurp, matricula, datetime);
                        }

                    } //for
                }
                else
                {
                    string CCT = Session["pertenece"].ToString();
                    objInstitucion.registraErrorFormato(CCT);

                }
            }
            catch
            {
                string CCT = Session["pertenece"].ToString();
                objInstitucion.archivoNoAdmitido(CCT);
            }

            TempData["Message"] = "Archivo Cargado";
            TempData["error"] = "alert alert-success";
            return RedirectToAction("RegistroBeneficiario");
        }

        public ActionResult TabletAdmin()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }

            return View(objAdmin.consultaInstitucionesRegistradas());
        }

        public ActionResult EliminaTablet(FormCollection collection)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }
            string update = collection["update"];
            var CCT = (String)Session["cctTemp"];
            string datetime = DateTime.Now.ToString("yyyy/MM/dd");

            objAdmin.registraTabletBenefInst(update, datetime);

            return View(objAdmin.consultaInfoBeneficiarioYTablet(CCT));
        }

        public ActionResult RegistroTablet()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }
            string CCT = Session["cctTemp"].ToString();//aqui va el CCT que se desea usar

            return View(objInstitucion.consultaErroresBeneficiario(CCT));
        }

        [HttpPost]
        public ActionResult RegistroTablet(FormCollection collection, string curp, string matricula, string parametro, string serie)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }
            string CCT = Session["cctTemp"].ToString();
            string grado = (String)Session["grado"];  //Session["grado"].ToString();
            string datetime = DateTime.Now.ToString("dd/MM/yyyy");

            List<Models.Alumno> lista = new List<Models.Alumno>();
            Models.Alumno dato = new Models.Alumno();

            if (parametro == "consulta")
            {
                if (matricula == "" && curp == "")
                {
                    dato.curp = curp;
                    dato.matricula = matricula;
                    dato.nombre = "No Disponible";
                    lista.Add(dato);
                }

                else if (matricula != "" && curp == "")
                {
                    objInstitucion.compExistBenefUsuario(curp, matricula);
                }

                else if (curp != "" && matricula == "")
                {
                    objInstitucion.beneficiarioActivo(curp);
                }

                else if (curp != "" && matricula != "")
                {
                    objInstitucion.consultaAlumnoYMatricula(curp, matricula);
                }
            }
            else if (serie != "")
            {
                if (curp.ToString() != "" && matricula.ToString() != "")
                {
                    objInstitucion.consultaTabletasAsignadas(curp, CCT, serie, datetime, grado);
                } //Validacion
            } // Parametro

            TempData["Message"] = "Se asigno la tablet";
            TempData["error"] = "alert alert-success";
            return View(lista);
        }//HTTP POST

        [HttpPost]
        public ActionResult ListaBenefTablet(FormCollection collection)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }
            string pertenece = (string)Session["pertenece"];
            Session["cctTemp"] = collection["cct"];
            var CCT = (String)Session["cctTemp"];
            string dato1 = Server.MapPath(@"~\Content\mi" + pertenece + "archivo3.xlsx");
            List<Models.Alumno> lista = new List<Models.Alumno>();

            objAdmin.consultaGradoInst(CCT);
            Session["grado"] = objAdmin.getGrado();

            objExcel.cargaBeneficiarioTablet(dato1, objInstitucion.consultaAlumnoTablet(CCT));
            return View(objInstitucion.consultaAlumnoTablet(CCT));
        }


        public ActionResult UsuarioDetails(string id)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return RedirectToAction("Institucion");
            }

            usuario usuario = new usuario();

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "SELECT ISNULL(P.CURP, ''), ISNULL(P.nombre, ''), ISNULL(P.apellido_pat, ''), ISNULL(P.apellido_mat, ''), ISNULL(P.codigopostal, ''), " +
                "ISNULL(P.estado_r, ''), ISNULL(P.municipio, ''), ISNULL(P.colonia, ''), ISNULL(P.calle, ''), ISNULL(P.num_int, ''), " +
                "ISNULL(P.num_ext, ''), ISNULL(P.telefono, ''), ISNULL(P.celular, ''), ISNULL(P.correo, ''), ISNULL(U.tipo, ''), ISNULL(U.pertenece, '') " +
                "FROM Persona AS P, usuario AS U " +
                "WHERE P.CURP = U.CURP AND U.CURP = '{0}'", id), conexion);
                SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    usuario.CURP = reader.GetString(0);
                    usuario.nombre = reader.GetString(1);
                    usuario.apellido_pat = reader.GetString(2);
                    usuario.apellido_mat = reader.GetString(3);
                    usuario.codigopostal = reader.GetString(4);
                    usuario.estado_r = reader.GetString(5);
                    usuario.municipio = reader.GetString(6);
                    usuario.colonia = reader.GetString(7);
                    usuario.calle = reader.GetString(8);
                    usuario.num_int = reader.GetString(9);
                    usuario.num_ext = reader.GetString(10);
                    usuario.telefono = reader.GetString(11);
                    usuario.celular = reader.GetString(12);
                    usuario.correo = reader.GetString(13);
                    usuario.tipo = reader.GetString(14);
                    usuario.pertenece = reader.GetString(15);
                }
                conexion.Close();
            }

            return View(usuario);
        }


        public ActionResult UsuarioEdit(string id)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return RedirectToAction("Institucion");
            }

            usuario usuario = new usuario();

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "SELECT ISNULL(P.CURP, ''), ISNULL(P.nombre, ''), ISNULL(P.apellido_pat, ''), ISNULL(P.apellido_mat, ''), ISNULL(P.codigopostal, ''), " +
                "ISNULL(P.estado_r, ''), ISNULL(P.municipio, ''), ISNULL(P.colonia, ''), ISNULL(P.calle, ''), ISNULL(P.num_int, ''), " +
                "ISNULL(P.num_ext, ''), ISNULL(P.telefono, ''), ISNULL(P.celular, ''), ISNULL(P.correo, ''), ISNULL(U.tipo, ''), ISNULL(U.pertenece, '') " +
                "FROM Persona AS P, usuario AS U " +
                "WHERE P.CURP = U.CURP AND U.CURP = '{0}'", id), conexion);
                SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    usuario.CURP = reader.GetString(0);
                    usuario.nombre = reader.GetString(1);
                    usuario.apellido_pat = reader.GetString(2);
                    usuario.apellido_mat = reader.GetString(3);
                    usuario.codigopostal = reader.GetString(4);
                    usuario.estado_r = reader.GetString(5);
                    usuario.municipio = reader.GetString(6);
                    usuario.colonia = reader.GetString(7);
                    usuario.calle = reader.GetString(8);
                    usuario.num_int = reader.GetString(9);
                    usuario.num_ext = reader.GetString(10);
                    usuario.telefono = reader.GetString(11);
                    usuario.celular = reader.GetString(12);
                    usuario.correo = reader.GetString(13);
                    usuario.tipo = reader.GetString(14);
                    usuario.pertenece = reader.GetString(15);
                }
                conexion.Close();
            }
            return View(usuario);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsuarioEdit([Bind(Include = "CURP,nombre,apellido_pat,apellido_mat,estado_nac,sexo,fecha_nac,codigopostal,estado_r,municipio,colonia,calle,num_int,num_ext,telefono,celular,correo,password,tipo,estatus,pertenece")] usuario usuario)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                objAdmin.actualizaDatosUsuario(usuario);
                TempData["error"] = "alert alert-success";
                TempData["message"] = "La institución se modificó correctamente";
            }

            TempData["error"] = "alert alert-danger";
            TempData["message"] = "Uno o mas campos no son validos";
            return View(usuario);
        }

        public PropiedadesExcel DownloadFile(string id)
        {
            return new PropiedadesExcel
            {
                FileName = "BeneficiariosNoRegistrados.xlsx",
                Path = @"~/Content/mi" + id + "archivo.xlsx"
            };
        }

        public PropiedadesExcel DownloadTablet(string id)
        {
            return new PropiedadesExcel
            {
                FileName = "BeneficiariosConTablet.xlsx",
                Path = @"~/Content/mi" + id + "archivo3.xlsx"
            };
        }

        public PropiedadesExcel DownloadBeneficiario(string id)
        {

            return new PropiedadesExcel
            {
                FileName = "BeneficiariosRegistrados.xlsx",
                Path = @"~/Content/mi" + id + "archivo4.xlsx"
            };
        }

        public PropiedadesExcel DownloadFormato()
        {

            return new PropiedadesExcel
            {
                FileName = "FormatoBeneficiario.xlsx",
                Path = @"~/Content/miarchivo2.xlsx"
            };
        }

        //Institucion


        public ActionResult registroMasivoProveedor()
        {
            if (Session["CURP"] == null) { return RedirectToAction("Index", "Home"); }
            string ruta = Server.MapPath(@"~\Content\Registo_De_Proveedores.xlsx");
            string ruta2 = Server.MapPath(@"~\Content\Registo_De_Proveedores_Errores.xlsx");

            List<InstitucionMasivoModel> lista = new List<InstitucionMasivoModel>();

            try
            {
                lista = objAdmin.consultaListaErrores();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            AdministradorTransacciones objAdmn = new AdministradorTransacciones();

            try
            {
                objAdmin.eliminaErroresEducafin();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            objExcel.cargaProveedorMasivoErrores(ruta2, lista);
            objExcel.cargaProveedorMasivo(ruta);


            return View(lista);

        }

        public PropiedadesExcel FormatoMasivoProveedor()
        {
            return new PropiedadesExcel
            {
                FileName = "Registo_De_Proveedores.xlsx",
                Path = @"~/Content/Registo_De_Proveedores.xlsx"
            };
        }

        public PropiedadesExcel FormatoMasivoProveedorError()
        {
            return new PropiedadesExcel
            {
                FileName = "Registo_De_Proveedores_Errores.xlsx",
                Path = @"~/Content/Registo_De_Proveedores_Errores.xlsx"
            };
        }


        [HttpPost]
        public ActionResult registroMasivoProveedor(HttpPostedFileBase file)
        {
            if (Session["CURP"] == null) { return RedirectToAction("Index", "Home"); }

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
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    String RFC = ds.Tables[0].Rows[i][0].ToString();
                    String nombre = ds.Tables[0].Rows[i][1].ToString();
                    String representante = ds.Tables[0].Rows[i][2].ToString();
                    String codigo_postal = ds.Tables[0].Rows[i][3].ToString();
                    String estado = ds.Tables[0].Rows[i][4].ToString();
                    String municipio = ds.Tables[0].Rows[i][5].ToString();
                    String colonia = ds.Tables[0].Rows[i][6].ToString();
                    String calle = ds.Tables[0].Rows[i][7].ToString();
                    String num_ext = ds.Tables[0].Rows[i][8].ToString();
                    String num_int = ds.Tables[0].Rows[i][8].ToString();
                    String telefono = ds.Tables[0].Rows[i][10].ToString();
                    String correo = ds.Tables[0].Rows[i][11].ToString();

                    if (objAdmin.consultaRFCProveedor(RFC).Equals(null) || objAdmin.consultaRFCProveedor(RFC).Equals(""))
                    {
                        try
                        {
                            objAdmin.registraProveedores(RFC, nombre, representante, codigo_postal, estado, municipio, colonia, calle, num_ext, num_int, telefono, correo);
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                    }
                    else //if (resultado != "" || resultado != null)
                    {
                        try
                        {
                            objAdmin.registraErrorProveedor(RFC, nombre);
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }

                    }

                }//for excel
            }//Fin del Excel

            TempData["Message"] = "Archivo Cargado";
            TempData["error"] = "alert alert-success";
            return RedirectToAction("registroMasivoProveedor");
        }


        public ActionResult registroMasivoInstitucion()
        {
            if (Session["CURP"] == null) { return RedirectToAction("Index", "Home"); }
            string ruta = Server.MapPath(@"~\Content\Registo_De_Instituciones.xlsx");
            string ruta2 = Server.MapPath(@"~\Content\Registo_De_Instituciones_Errores.xlsx");


            List<InstitucionMasivoModel> lista = new List<InstitucionMasivoModel>();
            try
            {
                lista = objAdmin.consultaListaErrores();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            AdministradorTransacciones objAdmn = new AdministradorTransacciones();

            try
            {
                objAdmin.eliminaErroresEducafin();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            objExcel.cargaInstitucionMasivoErrores(ruta2, lista);
            objExcel.cargaInstitucionMasivo(ruta);

            return View(lista);
        }


        public PropiedadesExcel FormatoMasivoInstitucion()
        {
            return new PropiedadesExcel
            {
                FileName = "Registo_De_Instituciones.xlsx",
                Path = @"~/Content/Registo_De_Instituciones.xlsx"
            };
        }

        public PropiedadesExcel FormatoMasivoInstitucionError()
        {
            return new PropiedadesExcel
            {
                FileName = "Registo_De_Instituciones_Errores.xlsx",
                Path = @"~/Content/Registo_De_Instituciones_Errores.xlsx"
            };
        }

        [HttpPost]
        public ActionResult registroMasivoInstitucion(HttpPostedFileBase file)
        {
            if (Session["CURP"] == null) { return RedirectToAction("Index", "Home"); }
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
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    String CCT = ds.Tables[0].Rows[i][0].ToString();
                    String nombre = ds.Tables[0].Rows[i][1].ToString();
                    String grado = ds.Tables[0].Rows[i][2].ToString();
                    String estado = ds.Tables[0].Rows[i][3].ToString();
                    String municipio = ds.Tables[0].Rows[i][4].ToString();
                    String colonia = ds.Tables[0].Rows[i][5].ToString();
                    String calle = ds.Tables[0].Rows[i][6].ToString();
                    String numero = ds.Tables[0].Rows[i][7].ToString();
                    String codigo_postal = ds.Tables[0].Rows[i][8].ToString();
                    String telefono = ds.Tables[0].Rows[i][9].ToString();
                    String correo = ds.Tables[0].Rows[i][10].ToString();
                    String medioproveedor = ds.Tables[0].Rows[i][11].ToString();
                    String dependencia = ds.Tables[0].Rows[i][12].ToString();
                    String mensaje = "";

                    try
                    {

                        objAdmin.consultaExistenciaInstitucion(CCT); //RETORNA objAdmin.getCCTInst();
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }

                    if (objAdmin.getCCTInst().Equals(null) || objAdmin.getCCTInst().Equals(""))
                    {
                        if (grado.Equals("S") || grado.Equals("s") || grado.Equals("M") || grado.Equals("m"))
                        {
                            if (medioproveedor.Equals("SI") || medioproveedor.Equals("si") || medioproveedor.Equals("Si") || medioproveedor.Equals("sI"))
                            {
                                medioproveedor = ("True");
                                try
                                {
                                    objAdmin.registraInstitucion(CCT, nombre, grado, estado, municipio, colonia, calle, numero, codigo_postal, telefono, correo, medioproveedor, dependencia);
                                }
                                catch (Exception ex) { Console.WriteLine(ex.Message); }

                            }
                            else if (medioproveedor.Equals("NO") || medioproveedor.Equals("no") || medioproveedor.Equals("No") || medioproveedor.Equals("nO"))
                            {
                                medioproveedor = ("False");
                                try
                                {
                                    objAdmin.registraInstitucion(CCT, nombre, grado, estado, municipio, colonia, calle, numero, codigo_postal, telefono, correo, medioproveedor, dependencia);
                                }
                                catch (Exception ex) { Console.WriteLine(ex.Message); }
                            }
                            else
                            {
                                mensaje = nombre + " MedioProveedor escrito incorrecto. Escriba: (SI o No)";
                                try
                                {
                                    objAdmin.registraErrorRegInst(CCT, mensaje);
                                }
                                catch (Exception ex) { Console.WriteLine(ex.Message); }
                            }

                        }
                        else
                        {
                            mensaje = nombre + " Grado escrito incorrecto. Escriba: (S o M)";
                            try
                            {
                                objAdmin.registraErrorRegInst(CCT, mensaje);
                            }
                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                        }

                    }
                    else if (objAdmin.getCCTInst() != null || objAdmin.getCCTInst() != "")
                    {
                        mensaje = "Clave:" + nombre + " Ya fue registrada anteriormente";
                        try
                        {
                            objAdmin.registraErrorRegInst(CCT, mensaje);
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                    }

                } //for
            }
            TempData["Message"] = "Archivo Cargado";
            TempData["error"] = "alert alert-success";
            return RedirectToAction("registroMasivoInstitucion");
        }


        public ActionResult registroMasivoUsuarios()
        {
            if (Session["CURP"] == null) { return RedirectToAction("Index", "Home"); }
            string ruta = Server.MapPath(@"~\Content\Registo_De_Usuarios.xlsx");
            string ruta2 = Server.MapPath(@"~\Content\Registo_De_Usuarios_Errores.xlsx");

            List<InstitucionMasivoModel> lista = new List<InstitucionMasivoModel>();
            try
            {
                lista = objAdmin.consultaListaErrores();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            AdministradorTransacciones objAdmn = new AdministradorTransacciones();
            try
            {
                objAdmin.eliminaErroresEducafin();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            objExcel.cargaUsuariosMasivoErrores(ruta2, lista);
            objExcel.cargaUsuariosMasivo(ruta);
            return View(lista);


        }


        public PropiedadesExcel FormatoMasivoUsuarios()
        {
            return new PropiedadesExcel
            {
                FileName = "Registo_De_Usuarios.xlsx",
                Path = @"~/Content/Registo_De_Usuarios.xlsx"
            };
        }

        public PropiedadesExcel FormatoMasivoUsuariosError()
        {
            return new PropiedadesExcel
            {
                FileName = "Registo_De_Usuarios_Errores.xlsx",
                Path = @"~/Content/Registo_De_Usuarios_Errores.xlsx"
            };
        }




        [HttpPost]
        public ActionResult registroMasivoUsuarios(HttpPostedFileBase file)
        {
            if (Session["CURP"] == null) { return RedirectToAction("Index", "Home"); }
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
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    String CURPExcel = ds.Tables[0].Rows[i][0].ToString();
                    String pertenece = ds.Tables[0].Rows[i][1].ToString();
                    String codigo_postal = ds.Tables[0].Rows[i][2].ToString();
                    String estado_recid = ds.Tables[0].Rows[i][3].ToString();
                    String municipio = ds.Tables[0].Rows[i][4].ToString();
                    String colonia = ds.Tables[0].Rows[i][5].ToString();
                    String calle = ds.Tables[0].Rows[i][6].ToString();
                    String numero_int = ds.Tables[0].Rows[i][7].ToString();
                    String numero_ext = ds.Tables[0].Rows[i][8].ToString();
                    String telefono = ds.Tables[0].Rows[i][9].ToString();
                    String tel_cel = ds.Tables[0].Rows[i][10].ToString();
                    String correo = ds.Tables[0].Rows[i][11].ToString();



                    string tipo = "", estatus = "", password = "";
                    CURPExcel = CURPExcel.ToString().ToUpper();
                    // objAdmin.consultaCURP(CURPExcel.Trim());

                    if (objAdmin.consultaCURP(CURPExcel.Trim()) == null || objAdmin.consultaCURP(CURPExcel.Trim()) == "")
                    {
                        try
                        {
                            password = Metodos.generatePassword(6);
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }

                        if (objAdmin.consultaInstitucion(pertenece) != null || objAdmin.consultaInstitucion(pertenece) != "")
                        {
                            if (objAdmin.consultaRFCProveedor(pertenece) != null || objAdmin.consultaRFCProveedor(pertenece) != "")
                            {
                                System.Diagnostics.Debug.WriteLine("Entro a Proveedor");
                                CURP objCurp = new CURP();
                                string[] str = objCurp.get_Curp(CURPExcel.ToString()).Split(',');

                                if (str.Length >= 7)
                                {
                                    string CURP = str[0];
                                    string ape_pat = str[1];
                                    string ape_mat = str[2];
                                    string nombre = str[3];
                                    string sexo = str[4];
                                    string fecha_nac = str[5];
                                    string lugar_nac = str[6];

                                    System.Diagnostics.Debug.WriteLine("Entro a Usuario");

                                    if (objAdmin.consultaInstitucion(pertenece) != null || objAdmin.consultaInstitucion(pertenece) != "") { tipo = "I"; }
                                    else if (objAdmin.consultaRFCProveedor(pertenece) != null || objAdmin.consultaRFCProveedor(pertenece) != "") { tipo = "P"; }
                                    estatus = "A";
                                    if (numero_int.Equals("") || numero_int.Equals(null)) { numero_int = ""; }

                                    try
                                    {
                                        objAdmin.registraUsuario(CURP.Trim(), nombre, ape_pat, ape_mat, lugar_nac, sexo, fecha_nac, codigo_postal,
                                        estado_recid, municipio, colonia, calle, numero_int, numero_ext, telefono, tel_cel, correo, password,
                                        tipo, estatus, pertenece);
                                    }
                                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                                }

                            }
                            else
                            {
                                try
                                {
                                    objAdmin.registraErrorRegInst(CURPExcel, "El RFC no existe");
                                }
                                catch (Exception ex) { Console.WriteLine(ex.Message); }
                            }
                        }
                        else
                        {
                            try
                            {
                                objAdmin.registraErrorRegInst(CURPExcel, "El CCT no existe");
                            }
                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                        }
                    }
                    else
                    {
                        try
                        {
                            objAdmin.registraErrorRegInst(CURPExcel, "El usuario ya existe");
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                    }
                    // objAdmin.setCURP(null);
                } //for
            }
            TempData["Message"] = "Archivo Cargado";
            TempData["error"] = "alert alert-success";
            return RedirectToAction("registroMasivoUsuarios");
        }


        public ActionResult periodoFinalizado()
        {
            if (Session["CURP"] == null) { return RedirectToAction("Index", "Home"); }
            return View();
        }

        [HttpPost]
        public ActionResult periodoFinalizado(string baja)
        {
            if (Session["CURP"] == null) { return RedirectToAction("Index", "Home"); }

            try
            {
                objAdmin.bajasBeneficiarios();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            TempData["Message"] = "Los Beneficiarios fueron inhabilitados";
            return View();
        }



        public ActionResult RegistroEntregasPasadas()
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }

            string dato1 = Server.MapPath(@"~\Content\Registo_De_Entregas_AnterioresError.xlsx");
            string dato2 = Server.MapPath(@"~\Content\Registo_De_Entregas_Anteriores.xlsx");

            List<Alumno> lista = new List<Alumno>();
            lista = objInstitucion.consultaErroresBenefAdmin("EDUCAFIN");
            InstitucionTransacciones objInstTransac = new InstitucionTransacciones();
            objInstTransac.EliminaErroresBD();


            objExcel.cargaarexcel(dato1, lista);
            objExcel.cargaFormatoEntregasAnteriores(dato2);

            return View(lista);

        }


        public PropiedadesExcel FormatoMasivoAnteriores()
        {
            return new PropiedadesExcel
            {
                FileName = "Registo_De_Entregas_Anteriores.xlsx",
                Path = @"~/Content/Registo_De_Entregas_Anteriores.xlsx"
            };
        }

        public PropiedadesExcel FormatoMasivoAnterioresError()
        {
            return new PropiedadesExcel
            {
                FileName = "Registo_De_Entregas_AnterioresError.xlsx",
                Path = @"~/Content/Registo_De_Entregas_AnterioresError.xlsx"
            };
        }






        [HttpPost]
        public ActionResult RegistroEntregasPasadas(HttpPostedFileBase file)
        {
            if (string.IsNullOrEmpty((string)Session["CURP"])) { return RedirectToAction("Index", "Home"); }
            if (!Session["tipo"].ToString().Equals("A"))
            {
                return RedirectToAction("Index", "Home");
            }
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
                        string CCT = ds.Tables[0].Rows[i][1].ToString();
                        string matricula = ds.Tables[0].Rows[i][2].ToString();
                        string no_Serie = ds.Tables[0].Rows[i][3].ToString();

                        string clave = CCT + validaCurp;
                        string datetime = DateTime.Now.ToString("yyyy/MM/dd");

                        CURP objCurp = new CURP();
                        string[] str = objCurp.get_Curp(validaCurp.ToString().ToUpper()).Split(',');

                        if (str.Length >= 7)
                        {
                            string CURP = str[0];
                            string ape_pat = str[1];
                            string ape_mat = str[2];
                            string nombre = str[3];
                            string sexo = str[4];
                            string fecha_nac = str[5];
                            string lugar_nac = str[6];

                            string ConsultaCurp = "";

                            using (SqlConnection conexion = Conexion.conexion())
                            {
                                SqlCommand comando2 = new SqlCommand(string.Format("select CURP from Usuario WHERE CURP='{0}'", CURP.Trim()), conexion);

                                SqlDataReader reader = comando2.ExecuteReader();
                                while (reader.Read())
                                {
                                    ConsultaCurp = reader.GetString(0);
                                }
                                reader.Close();

                            }

                            if (ConsultaCurp.Equals("") || ConsultaCurp.Equals(null))
                            {

                                string CURP_folio = "";
                                using (SqlConnection conex = Conexion.conexion())
                                {
                                    string query = "Insert into institucion_beneficiario(clave,CCT,CURP,matricula,estatus,fecha_reg) Values('" + clave + "','" + CCT + "','" + CURP.Trim() + "','" + matricula + "','I','" + datetime + "')";
                                    string query2 = "Insert into persona(CURP,nombre,apellido_pat,apellido_mat,estado_nac,sexo,fecha_nac) values('" + CURP.Trim() + "','" + nombre + "','" + ape_pat + "','" + ape_mat + "'," + lugar_nac + "," + sexo + ",'" + fecha_nac + "')";
                                    string query3 = "Insert into Usuario(CURP,password,tipo,estatus,pertenece) values('" + CURP.Trim() + "','','B','A','" + CCT + "')";

                                    SqlCommand cmd = new SqlCommand(query2, conex);
                                    cmd.ExecuteNonQuery();
                                    SqlCommand cmd2 = new SqlCommand(query, conex);
                                    cmd2.ExecuteNonQuery();
                                    SqlCommand cmd3 = new SqlCommand(query3, conex);
                                    cmd3.ExecuteNonQuery();
                                    conex.Close();
                                }


                                bool medioproveedor = false;

                                using (SqlConnection conexion = Conexion.conexion())
                                {

                                    SqlCommand comando2 = new SqlCommand(string.Format("select medioprovedor from institucion where CCT='{0}' ", CCT), conexion);
                                    SqlDataReader reader = comando2.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        medioproveedor = reader.GetBoolean(0);
                                    }
                                    reader.Close();
                                    conexion.Close();
                                }

                                System.Diagnostics.Debug.WriteLine(medioproveedor);

                                if (medioproveedor.Equals(false))
                                {
                                    using (SqlConnection conex = Conexion.conexion())
                                    {
                                        string query4 = "INSERT INTO tablet(proveedor, serie, intitucion, medioprovedor, beneficiario, fecha, estatus) VALUES('EDUCAFIN','" + no_Serie + "','" + CCT + "','EDUCAFIN','" + CURP.Trim() + "','" + datetime + "','B')";
                                        SqlCommand cmd4 = new SqlCommand(query4, conex);
                                        cmd4.ExecuteNonQuery();
                                        conex.Close();
                                    }
                                }
                                else if (medioproveedor.Equals(true))
                                {
                                    using (SqlConnection conex = Conexion.conexion())
                                    {
                                        string query = "INSERT INTO tablet(proveedor, serie, medioprovedor, beneficiario, fecha, estatus) VALUES('EDUCAFIN','" + no_Serie + "','" + CCT + "','" + CURP.Trim() + "','" + datetime + "','B')";
                                        SqlCommand cmd = new SqlCommand(query, conex);
                                        cmd.ExecuteNonQuery();
                                        conex.Close();
                                    }
                                }



                                using (SqlConnection conexion = Conexion.conexion())
                                {

                                    SqlCommand comando2 = new SqlCommand(string.Format("select CURP from folio WHERE CURP='{0}' ", CURP.Trim()), conexion);
                                    SqlDataReader reader = comando2.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        CURP_folio = reader.GetString(0);
                                    }
                                    reader.Close();
                                    conexion.Close();
                                }

                                if (CURP_folio.Equals(null) || CURP_folio.Equals(""))
                                {

                                    using (SqlConnection conex = Conexion.conexion())
                                    {
                                        string query4 = "INSERT INTO folio(CURP) VALUES('" + CURP.Trim() + "')";
                                        SqlCommand cmd4 = new SqlCommand(query4, conex);
                                        cmd4.ExecuteNonQuery();
                                        conex.Close();
                                    }
                                }


                            }
                            else
                            {
                                //Imprimir mensaje de error (El beneficiario Ya existe)
                                objInstitucion.registraCURPDuplicado(clave, "EDUCAFIN", validaCurp, matricula, datetime);
                            }

                        } // str.lengh = 7

                        else
                        {
                            objInstitucion.registraCURPNoValido(clave, "EDUCAFIN", validaCurp, matricula, datetime);
                        }

                    } //for
                }
                else
                {
                    objInstitucion.registraErrorFormato("EDUCAFIN");

                }
            }
            catch
            {
                objInstitucion.archivoNoAdmitido("EDUCAFIN");
            }

            TempData["Message"] = "Archivo Cargado";
            TempData["error"] = "alert alert-success";
            return RedirectToAction("RegistroEntregasPasadas");

        }


















    } //Fin de la Clase
}
