using Educafin.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Educafin.Clases.Transacciones
{
    public class InstitucionTransacciones
    {

        private string CURP = "", tipo = "", status = "", CCT = "", matricula="";
        public void setCURP(String CURP) { this.CURP = CURP; }
        public string getCURP() { return this.CURP; }
        public void setTipo(String tipo) { this.tipo = tipo; }
        public string getTipo() { return this.tipo; }
        public void setStatus(String status) { this.status = status; }
        public string getStatus() { return this.status; }
        public void setCCT(String CCT) { this.CCT = CCT; }
        public string getCCT() { return this.CCT; }
        public void setMatricula(String matricula) { this.matricula = matricula; }
        public string getMatricula() { return this.matricula; }


        public List<Models.Alumno> consultaErroresBenef(string CCT)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: Metodo para leer los errores que se generaron al intentar hacer algun registro
            // Fecha Modificación: 11/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------
            List<Models.Alumno> lista = new List<Models.Alumno>();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select CURP,MATRICULA,ESTATUS,MENSAJE from benefi_error WHERE CCT = '{0}' ORDER BY CURP", CCT), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                int j = 1;
                while (reader.Read())
                {
                    Models.Alumno dato = new Models.Alumno();
                    dato.no = j;
                    dato.curp = reader.GetString(0);
                    dato.matricula = reader.GetString(1);
                    dato.nombre = reader.GetString(2);
                    dato.status = reader.GetString(3);
                    lista.Add(dato);
                    j++;
                }
                conexion.Close();
            }

            return lista;
        }

        public void eliminaErroresBeneficiario(string CCT) {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "delete from benefi_error where CCT like'" + CCT + "'";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }


        public List<Models.Alumno> consultaErroresBenefAdmin(string CCT)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: Metodo para leer los errores que se generaron al intentar hacer algun registro
            // Fecha Modificación: 11/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------
            List<Models.Alumno> lista = new List<Models.Alumno>();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select CURP,MATRICULA,ESTATUS,MENSAJE from benefi_error WHERE CCT = 'EDUCAFIN' ORDER BY CURP", CCT), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                int j = 1;
                while (reader.Read())
                {
                    Models.Alumno dato = new Models.Alumno();
                    dato.no = j;
                    dato.curp = reader.GetString(0);
                    dato.matricula = reader.GetString(1);
                    dato.nombre = reader.GetString(2);
                    dato.status = reader.GetString(3);
                    lista.Add(dato);
                    j++;
                }
                reader.Close();
                conexion.Close();
            }  
            return lista;   
        }


        public void EliminaErroresBD() {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "delete from benefi_error where CCT='EDUCAFIN'";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }



        public List<Models.Alumno> consultaBenefReg(String CCT)
        {

            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: Metodo para leer los beneficiarios que ya estan Activos 
            // (Cambiaron su contraseña Temporal).
            // Fecha Modificación: 11/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------
            List<Models.Alumno> lista = new List<Models.Alumno>();

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                    // "SELECT I.CURP, I.matricula, U.apellido_pat,U.apellido_mat, U.nombre FROM institucion_beneficiario AS I, Usuario AS U WHERE I.CURP = U.CURP AND CCT = '{0}' AND I.estatus = 'A' ORDER BY apellido_pat,apellido_mat,nombre"
                    "SELECT I.CURP, I.matricula, P.apellido_pat, P.apellido_mat, P.nombre " +
                    "FROM institucion_beneficiario AS I, persona AS P " +
                    "WHERE I.CURP = P.CURP AND CCT = '{0}' AND I.estatus = 'A' " +
                    "ORDER BY apellido_pat, apellido_mat, nombre", CCT), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                int j = 1;
                while (reader.Read())
                {
                       Models.Alumno dato = new Models.Alumno();
                    dato.no = j;
                    dato.curp = reader.GetString(0);
                    dato.matricula = reader.GetString(1);
                    dato.nombre = reader.GetString(2) + " " + reader.GetString(3) + " " + reader.GetString(4);
                    dato.apePat = reader.GetString(2);
                    dato.apeMat = reader.GetString(3);
                    dato.nombreTemp = reader.GetString(4);
                    lista.Add(dato);
                    j++;
                }
                conexion.Close();
            }
            return lista;
        }

        public List<Models.Alumno> consultaAlumnoTablet(string CCT)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: Metodo para leer los beneficiarios que ya tienen una tablet entregada 
            // (Se busca por el numero de serie y el Beneficiario).
            // Fecha Modificación: 11/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------
            List<Models.Alumno> lista = new List<Models.Alumno>();

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                //"SELECT I.CURP, I.matricula, U.apellido_pat, U.apellido_mat, U.nombre,T.serie,T.fecha,U.correo, I.folio FROM institucion_beneficiario AS I, Usuario AS U, tablet AS T WHERE I.CURP = T.beneficiario and I.CURP = U.CURP AND T.intitucion = '{0}' AND I.matricula != '' ORDER BY apellido_pat, apellido_mat, nombre"
                "SELECT I.CURP, I.matricula, P.apellido_pat, P.apellido_mat, P.nombre, T.serie, T.fecha, ISNULL(P.correo, ''), ISNULL(F.folio, '') " +
                "FROM institucion_beneficiario AS I, persona AS P, tablet AS T, folio as F " +
                "WHERE I.CURP = T.beneficiario and I.CURP = P.CURP AND F.CURP = P.CURP AND F.CURP = T.beneficiario " +
                "AND T.intitucion = '{0}' AND I.matricula != '' " +
                "ORDER BY apellido_pat, apellido_mat, nombre", CCT), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                int j = 1;
                while (reader.Read())
                {
                    Models.Alumno dato = new Models.Alumno();
                    dato.no = j;
                    dato.curp = reader.GetString(0);
                    dato.matricula = reader.GetString(1);
                    dato.nombre = reader.GetString(2) + " " + reader.GetString(3) + " " + reader.GetString(4);
                    dato.apePat = reader.GetString(2);
                    dato.apeMat = reader.GetString(3);
                    dato.nombreTemp = reader.GetString(4);
                    dato.status = reader.GetString(5);
                    dato.fecha = reader.GetString(6);
                    dato.correo = reader.GetString(7);
                    dato.folio = reader.GetString(8);

                    if (reader.GetValue(7).ToString() != "")
                    {
                        dato.correo = reader.GetString(7);
                    }
                    else
                    {
                        dato.correo = "No Disponible";
                    }
                    lista.Add(dato);
                    j++;
                }
                conexion.Close();
            }
            return lista;
        }

        public List<Models.Alumno> consultaErroresBeneficiario(string CCT)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: Metodo para leer los errores al momento de hacer un registro 
            // Fecha Modificación: 11/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            List<Models.Alumno> lista = new List<Models.Alumno>();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select CURP,MATRICULA,CCT from benefi_error WHERE CCT = '{0}' ", CCT), conexion);
                SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    Models.Alumno dato = new Models.Alumno();
                    dato.curp = reader.GetString(0);
                    dato.matricula = reader.GetString(1);
                    dato.nombre = reader.GetString(2);
                    lista.Add(dato);

                }
                conexion.Close();
            }
            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "delete from benefi_error where CCT like'" + CCT + "'";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
            return lista;
        }

        public List<Models.Alumno> consultaErroresBenefMensaje(string CCT)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: Metodo para leer los errores al momento de hacer un registro 
            // Fecha Modificación: 11/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            List<Models.Alumno> lista = new List<Models.Alumno>();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select CURP,MATRICULA,CCT,MENSAJE from benefi_error WHERE CCT = '{0}' ", CCT), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                int j = 1;
                while (reader.Read())
                {
                    Models.Alumno dato = new Models.Alumno();
                    dato.no = j;
                    dato.curp = reader.GetString(0);
                    dato.matricula = reader.GetString(1);
                    dato.nombre = reader.GetString(2);
                    dato.status = reader.GetString(3);
                    lista.Add(dato);
                    j++;
                }
                conexion.Close();
            }
            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "delete from benefi_error where CCT like'" + CCT + "'";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
            return lista;
        }

        public List<MvcApplication8.Models.Tableta> consultaTabletNoEntregada(string CCT)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: Metodo para leer las tabletas que no han sido entregadas a un beneficiario.
            // Fecha Modificación: 11/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            List<MvcApplication8.Models.Tableta> lista = new List<MvcApplication8.Models.Tableta>();

            bool medioproveedor = false;

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select medioprovedor from institucion WHERE CCT = '{0}'", CCT), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                   medioproveedor = reader.GetBoolean(0);      
                }
                reader.Close();
                conexion.Close();
            }

            if (medioproveedor == true)
            {
                using (SqlConnection conexion = Conexion.conexion())
                {
                    SqlCommand comando = new SqlCommand(string.Format(
                    "SELECT DISTINCT A.serie, ISNULL(A.medioprovedor, '') CCT_MEDIO_PROVEEDOR, ISNULL(B.nombre, '') Nombre_Institución_Planteles " +
                    "FROM tablet as A, institucion AS B " +
                    "WHERE A.medioprovedor = B.CCT " +
                    "AND beneficiario IS NULL AND A.medioprovedor = '{0}' AND A.estatus = 'M'", CCT), conexion);
                    SqlDataReader reader = comando.ExecuteReader();
                    int j = 1;
                    while (reader.Read())
                    {
                        MvcApplication8.Models.Tableta dato = new MvcApplication8.Models.Tableta();
                        dato.No = j;
                        dato.serie = reader.GetString(0);
                        dato.institucion = reader.GetString(1);
                        dato.nombre_institucion = reader.GetString(2);
                        lista.Add(dato);
                        j++;
                    }
                    reader.Close();
                    conexion.Close();
                }

            } else
            {
                using (SqlConnection conexion = Conexion.conexion())
                {
                    SqlCommand comando = new SqlCommand(string.Format(
                    "SELECT DISTINCT A.serie, ISNULL(A.intitucion,'') CCT_INSTITUCION, ISNULL(B.nombre, '') Nombre_Institución " +
                    "FROM tablet as A, institucion AS B " +
                    "WHERE A.intitucion = B.CCT " +
                    "AND beneficiario IS NULL AND A.intitucion = '{0}' AND A.estatus = 'I'", CCT), conexion);
                    SqlDataReader reader = comando.ExecuteReader();
                    int j = 1;
                    while (reader.Read())
                    {
                        MvcApplication8.Models.Tableta dato = new MvcApplication8.Models.Tableta();
                        dato.No = j;
                        dato.serie = reader.GetString(0);
                        dato.institucion = reader.GetString(1);
                        dato.nombre_institucion = reader.GetString(2);
                        lista.Add(dato);
                        j++;
                    }
                    reader.Close();
                    conexion.Close();
                }
            }

            return lista;
        }

        public List<Alumno> consultaUsuariosTemporales(string CCT, string datetime)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: Metodo para leer los usuarios temporales de tipo Institucion para la.
            // entrega de tablets unicamente.
            // Fecha Modificación: 11/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            List<Alumno> lista = new List<Alumno>();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                //"SELECT ISNULL(A.CURP, ''), ISNULL(A.nombre, ''), ISNULL(A.apellido_pat, ''), ISNULL(A.apellido_mat, '') FROM Usuario AS A, inst_Temporal AS B WHERE A.CURP = B.CURP AND A.pertenece = '{0}'AND B.fecha_limite between '{1}' AND B.fecha_limite"
                "SELECT ISNULL(P.CURP, '') CURP, ISNULL(P.nombre, '') Nombre, ISNULL(P.apellido_pat, '') Ape_Pat, ISNULL(P.apellido_mat, '') Ape_Mat " +
                "FROM persona AS P, inst_Temporal AS B, usuario as U " +
                "WHERE P.CURP = B.CURP AND U.CURP = P.CURP AND U.CURP = B.CURP AND U.pertenece = '{0}' " +
                "AND B.fecha_limite between '{1}' AND B.fecha_limite"
                , CCT, datetime), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                int j = 1;
                while (reader.Read())
                {
                    Alumno dato = new Alumno();
                    dato.no = j;
                    dato.curp = reader.GetString(0);
                    dato.nombre = reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3);
                    lista.Add(dato);
                    j++;
                }
                conexion.Close();
            }
            return lista;
        }

        public void consultaUsuariosRegistrados(string Curp)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: Metodo para leer buscar si existe un Usuario temporal en la Base de Datos.
            // Fecha Modificación: 11/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "SELECT ISNULL(CURP, '') CURP, ISNULL(tipo, '') TIPO, ISNULL(estatus, '') ESTATUS, ISNULL(pertenece, '') PERTENECE " +
                "FROM Usuario WHERE CURP='{0}'"
            
                , Curp), conexion);
                SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    setCURP(reader.GetString(0));
                    setTipo(reader.GetString(1));
                    setStatus(reader.GetString(2));
                    setCCT(reader.GetString(3));
                }
                conexion.Close();
            }
        }

        public void registraUsuariosTemporales(string CURP, string nombre, string ape_Pat, string ape_Mat, string lugar_Nac, string sexo, string fecha_Nac, string CCT, string fecha_Final)
        {
            //-------------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: Metodo para insertar un Usuario temporal, primero lo registra en la tabla Usuario
            // posteriormente registra en una segunda tabla el CURP y la fecha maxima que tendra el Usuario para
            // ingresar al sistema.
            // Fecha Modificación: 11/10/2017
            // Autor Modificación: Irving Lahera
            //-------------------------------------------------------------------------------------------------

            using (SqlConnection conex = Conexion.conexion())
            {
                //string query = "Insert into Usuario(CURP, nombre, apellido_pat, apellido_mat, estado_nac, sexo, fecha_nac, password, tipo, estatus, pertenece) VALUES('" + CURP.Trim() + "','" + nombre + "','" + ape_Pat + "','" + ape_Mat + "'," + lugar_Nac + "," + sexo + ",'" + fecha_Nac + "','" + CCT + "','T','A','" + CCT + "')";
                //SqlCommand cmd = new SqlCommand(query, conex);
                //cmd.ExecuteNonQuery();

                string query = "Insert into Persona(CURP, nombre, apellido_pat, apellido_mat, estado_nac, sexo, fecha_nac) VALUES('" + CURP.Trim() + "','" + nombre + "','" + ape_Pat + "','" + ape_Mat + "'," + lugar_Nac + "," + sexo + ",'" + fecha_Nac + "')";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();

                string queryP = "Insert into Usuario(CURP, password, tipo, estatus, pertenece) VALUES('" + CURP.Trim() + "','" + CCT + "','T','A','" + CCT + "')";
                SqlCommand cmdP = new SqlCommand(queryP, conex);
                cmdP.ExecuteNonQuery();

                string query2 = "Insert into inst_Temporal(CURP, fecha_limite) Values('" + CURP.Trim() + "','" + fecha_Final.Trim() + "')";
                SqlCommand cmd2 = new SqlCommand(query2, conex);
                cmd2.ExecuteNonQuery();
                conex.Close();

            }
        }

        public string actualizaEstatusUsuarioTemp(string CURP, string fecha_Final)
        {
            //-------------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: Metodo para actualizar el estatus de "I" por "A" de un Usuario temporal 
            // y volver a actualizar la fecha limite que tendra el Usuario para ingresar al sistema.
            // Fecha Modificación: 11/10/2017
            // Autor Modificación: Irving Lahera
            //-------------------------------------------------------------------------------------------------

            string respuesta = "";
            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "UPDATE Usuario SET estatus='A' WHERE CURP='" + CURP.Trim() + "'";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();

                string query2 = "UPDATE inst_Temporal SET fecha_limite='" + fecha_Final.Trim() + "' WHERE CURP='" + CURP.Trim() + "'";
                SqlCommand cmd2 = new SqlCommand(query2, conex);
                cmd2.ExecuteNonQuery();

                respuesta = "El CURP ha sido activado nuevamente";
            }
            return respuesta;
        }


        public void compExistBenefUsuario(string curp, string matricula)
        {
            //-------------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: Metodo para verificar si existe algun beneficiario a traves de su CURP ó matricula
            // Fecha Modificación: 11/10/2017
            // Autor Modificación: Irving Lahera
            //-------------------------------------------------------------------------------------------------

            List<Models.Alumno> lista = new List<Models.Alumno>();
            Models.Alumno dato = new Models.Alumno();
            using (SqlConnection conexion = Conexion.conexion())
            {
                
                SqlCommand comando = new SqlCommand(string.Format(
                //"SELECT I.CURP, I.matricula, U.apellido_pat, U.apellido_mat, U.nombre FROM institucion_beneficiario AS I, Usuario AS U WHERE I.CURP = U.CURP AND I.matricula LIKE '" + matricula + "%' AND I.estatus='A'"
                "SELECT I.CURP, I.matricula, P.apellido_pat, P.apellido_mat, P.nombre " +
                "FROM institucion_beneficiario AS I, persona AS P " +
                "WHERE I.CURP = P.CURP AND I.matricula LIKE '" + matricula + "%' AND I.estatus = 'A'" ), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    dato.curp = reader.GetString(0);
                    dato.matricula = reader.GetString(1);
                    dato.nombre = reader.GetString(2) + " " + reader.GetString(3) + " " + reader.GetString(4);
                }
                reader.Close();
                conexion.Close();

                if (dato.nombre == null)
                {
                    dato.curp = curp.ToString();
                    dato.matricula = matricula.ToString();
                    dato.nombre = "No Disponible";
                }
            }
            lista.Add(dato);
        }

        public string consultaFoliosSinAsignar(string CCT)
        {
            //-------------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: Metodo que devuelve una consulta de los beneficiarios que no tienen folio. 
            // Fecha Modificación: 11/10/2017
            // Autor Modificación: Ivan Ibarra
            //-------------------------------------------------------------------------------------------------

            //string consulta = "SELECT curp, apellido_pat, apellido_mat, nombre FROM Usuario " +
            //                     "where curp IN(select curp from institucion_beneficiario " +
            //                     "where CCT = '" + CCT + "' AND estatus = 'A' AND folio IS NULL) " +
            //                     "ORDER BY apellido_pat, apellido_mat, nombre;";

            string consulta = "SELECT A.curp, A.apellido_pat, A.apellido_mat, A.nombre " +
                              "FROM PERSONA A LEFT JOIN institucion_beneficiario B " +
                              "ON A.CURP = B.CURP " +
                              "LEFT JOIN folio as F " +
                              "ON B.CURP = F.CURP " +
                              "WHERE B.CCT = '" + CCT + "' AND F.folio IS NULL " +
                              "ORDER BY A.apellido_pat, A.apellido_mat, A.nombre";

            return consulta;
        }

        public string consultaFoliosAsignados(string CCT)
        {
            //-------------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: Metodo que devuelve una consulta de los beneficiarios que ya tienen folio asignado. 
            // Fecha Modificación: 11/10/2017
            // Autor Modificación: Ivan Ibarra
            //-------------------------------------------------------------------------------------------------

            //string consulta = "SELECT curp FROM Usuario " +
            //                    "where curp IN(select curp from institucion_beneficiario " +
            //                    "where CCT = '" + CCT + "' AND estatus = 'A' AND folio IS NOT NULL);";

            string consulta = "SELECT A.curp, A.apellido_pat, A.apellido_mat, A.nombre " +
                              "FROM PERSONA A LEFT JOIN institucion_beneficiario B " +
                              "ON A.CURP = B.CURP " +
                              "LEFT JOIN folio as F " +
                              "ON B.CURP = F.CURP " +
                              "WHERE B.CCT = '" + CCT + "' AND F.folio IS NOT NULL " +
                              "ORDER BY A.apellido_pat, A.apellido_mat, A.nombre";
            return consulta;
        }

        public string consultaUltimoFolio(string CCT)
        {
            //-------------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: Metodo que devuelve una consulta con el ultimo folio que se genero en el sistema. 
            // Fecha Modificación: 11/10/2017
            // Autor Modificación: Ivan Ibarra
            //-------------------------------------------------------------------------------------------------
            string consulta = "SELECT TOP 1 F.folio FROM folio F " +
                              "LEFT JOIN institucion_beneficiario B on F.CURP = B.CURP " +
                              "where B.CCT = '" + CCT + "' " +
                              "ORDER BY folio DESC; ";

            return consulta;
        }


        public void beneficiarioActivo(string CURP)
        {
            //-------------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: Metodo que consulta si un beneficiario esta registrado en el sistema y su estatus
            // esta asignado como activo.
            // Fecha Modificación: 11/10/2017
            // Autor Modificación: Irving Lahera
            //-------------------------------------------------------------------------------------------------

            List<Models.Alumno> lista = new List<Models.Alumno>();
            Models.Alumno dato = new Models.Alumno();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                    // "SELECT I.CURP, I.matricula, U.apellido_pat, U.apellido_mat, U.nombre FROM institucion_beneficiario AS I, Usuario AS U WHERE I.CURP = U.CURP AND I.CURP LIKE '" + CURP + "%' AND I.estatus='A'"
                    "SELECT I.CURP, I.matricula, P.apellido_pat, P.apellido_mat, P.nombre " +
                    "FROM institucion_beneficiario AS I, persona AS P " +
                    "WHERE I.CURP = P.CURP AND I.CURP LIKE '" + CURP + "%' AND I.estatus = 'A'"), conexion);
                SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    dato.curp = reader.GetString(0);
                    dato.matricula = reader.GetString(1);
                    dato.nombre = reader.GetString(2) + " " + reader.GetString(3) + " " + reader.GetString(4);
                }
                reader.Close();
                conexion.Close();
            }
            lista.Add(dato);
        }

        public void consultaAlumnoYMatricula(string CURP, string matricula)
        {

            //-------------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: Metodo que consulta si un beneficiario esta registrado en el sistema y su estatus
            // esta asignado como activo y tiene una matricula.
            // Fecha Modificación: 11/10/2017
            // Autor Modificación: Irving Lahera
            //-------------------------------------------------------------------------------------------------

            List<Models.Alumno> lista = new List<Models.Alumno>();
            Models.Alumno dato = new Models.Alumno();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
               // "SELECT I.CURP, I.matricula, U.apellido_pat, U.apellido_mat, U.nombre FROM institucion_beneficiario AS I, Usuario AS U WHERE I.CURP = U.CURP AND I.CURP LIKE '" + CURP + "%' AND I.estatus='A' AND I.matricula LIKE '" + matricula + "%'"
                "SELECT I.CURP, I.matricula, P.apellido_pat, P.apellido_mat, P.nombre " +
                "FROM institucion_beneficiario AS I, persona AS P " +
                "WHERE I.CURP = P.CURP AND I.CURP LIKE '" + CURP + "%' AND I.estatus = 'A' AND I.matricula LIKE '" + matricula + "%'" ), conexion);
                SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    dato.curp = reader.GetString(0);
                    dato.matricula = reader.GetString(1);
                    dato.nombre = reader.GetString(2) + " " + reader.GetString(3) + " " + reader.GetString(4);
                }
                reader.Close();
                conexion.Close();
            }
            lista.Add(dato);
        }


        public void consultaTabletasAsignadas(string CURP, string CCT, string serie, string datetime, string grado)
        {

            //-------------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: Metodo que consulta si un beneficiario tiene asignada alguna tableta en caso 
            // de no tener alguna registra esa tableta al beneficiario correspondiente.
            // Fecha Modificación: 11/10/2017
            // Autor Modificación: Irving Lahera
            //-------------------------------------------------------------------------------------------------

            List<Models.Alumno> lista = new List<Models.Alumno>();
            Models.Alumno dato = new Models.Alumno();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
               "select beneficiario from tablet where beneficiario='{0}' ", CURP), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                string beneficiario = "";
                while (reader.Read())
                {
                    beneficiario = reader.GetString(0);
                }
                reader.Close();

                if (beneficiario != "")
                {
                    dato.status = CURP + " ya cuenta con una tablet";
                }
                else
                {
                    comando = new SqlCommand(string.Format(
                    "select serie from tablet where intitucion='{0}'AND serie='{1}' AND beneficiario is not null ", CCT, serie), conexion);
                    reader = comando.ExecuteReader();
                    string validacion2 = "";
                    while (reader.Read())
                    {
                        validacion2 = reader.GetString(0);
                    }
                    reader.Close();

                    if (validacion2 != "")
                    {
                        dato.status = "No de serie ya se asignado";
                    }
                    else
                    {
                        comando = new SqlCommand(string.Format(
                        "select serie from tablet where intitucion='{0}' AND serie='{1}' and beneficiario is null ", CCT, serie), conexion);
                        reader = comando.ExecuteReader();
                        string validacion = "";
                        while (reader.Read())
                        {
                            validacion = reader.GetString(0);
                        }
                        reader.Close();

                        if (validacion != "")
                        {
                            string update = "update tablet set beneficiario ='" + CURP + "', fecha = '" + datetime + "' WHERE serie='" + serie + "'";
                            SqlCommand updatecmd = new SqlCommand(update, conexion);
                            updatecmd.ExecuteNonQuery();
                            dato.status = "Tableta Registrada";
                        }
                        else
                        {
                            dato.status = "No. de serie no disponible";
                        }
                    }
                    conexion.Close();
                }
            }
            lista.Add(dato);
        }







        public void bajaBeneficiarios(string CURP, string CCT)
        {

            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: 
            // Fecha Modificación: 10/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "UPDATE institucion_beneficiario SET estatus='I', matricula='' WHERE CURP='" + CURP + "' and CCT='{0}'", CCT), conex);
                comando.ExecuteNonQuery();

                comando = new SqlCommand(string.Format(
                "UPDATE Usuario SET pertenece=null, estatus='I' WHERE CURP='" + CURP + "'"), conex);
                comando.ExecuteNonQuery();
            }
        }


        public void respaldoBajaBenTablet(string CURP, string datetime)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: 
            // Fecha Modificación: 10/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            using (SqlConnection conect = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select serie,intitucion,beneficiario,grado from tablet where beneficiario='{0}'", CURP), conect);
                SqlDataReader reader = comando.ExecuteReader();
                string[] datos = null;
                while (reader.Read())
                {
                    datos = new string[]{
                   reader.GetString(0) , reader.GetString(1), reader.GetString(2), reader.GetString(3) };
                }
                reader.Close();
                comando = new SqlCommand(string.Format("Insert into tablet_baja values ('" + datos[0] + "','" + datos[1] + "','" + datos[2] + "','" + datetime + "','" + datos[3] + "')"), conect);
                comando.ExecuteNonQuery();
                conect.Close();
            }
        }

        public void eliminaTablet(string CURP)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: 
            // Fecha Modificación: 10/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "update tablet set beneficiario=null, fecha=null where beneficiario = '" + CURP + "'"), conex);
                comando.ExecuteNonQuery();
            }
        }

        public string consultaDependencia(string CCT)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: 
            // Fecha Modificación: 10/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            string dependencia = "";
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando2 = new SqlCommand(string.Format("select dependencia from institucion WHERE CCT='{0}' ", CCT), conexion);
                SqlDataReader reader = comando2.ExecuteReader();

                while (reader.Read())
                {
                    if (reader.GetString(0) != null)
                    {
                        dependencia = reader.GetString(0);
                    }
                }
                reader.Close();
                conexion.Close();
            }
            return dependencia;
        }

        public void consultaEstatusBeneficiario(string CURP)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: 
            // Fecha Modificación: 10/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando2 = new SqlCommand(string.Format("select CURP,estatus from Usuario WHERE CURP='{0}' ", CURP), conexion);
                SqlDataReader reader = comando2.ExecuteReader();
                while (reader.Read())
                {
                    setCURP(reader.GetString(0));
                    setStatus(reader.GetString(1));
                }
                reader.Close();
            }
        }

        public void consultaBeneficiariosInactivos(string CURP, string CCT)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: 
            // Fecha Modificación: 10/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando2 = new SqlCommand(string.Format("select CURP from institucion_beneficiario WHERE CURP='{0}' AND estatus='I' AND CCT='{1}'", CURP, CCT), conexion);
                SqlDataReader reader = comando2.ExecuteReader();


                while (reader.Read())
                {
                    setCURP(reader.GetString(0));
                }
                reader.Close();
                conexion.Close();
            }

        }

        public void consultaBeneficiariosActivos(string CURP, string CCT)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: 
            // Fecha Modificación: 10/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando2 = new SqlCommand(string.Format("select CURP from institucion_beneficiario WHERE CURP='{0}' AND estatus='A' AND CCT='{1}'", CURP, CCT), conexion);
                SqlDataReader reader = comando2.ExecuteReader();


                while (reader.Read())
                {
                    setCURP(reader.GetString(0));
                }
                reader.Close();
                conexion.Close();
            }

        }


        public void consultaBenenficiaio(string CURP)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: 
            // Fecha Modificación: 10/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format("select CURP from Usuario WHERE CURP='{0}' ", CURP), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setCURP(reader.GetString(0));
                }
                reader.Close();
            }
        }

         public void trasladoBeneficiario(string clave, string CURP, string matricula, string CCT )
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: 
            // Fecha Modificación: 10/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            using (SqlConnection conex = Conexion.conexion())
            {
                string update = "UPDATE institucion_beneficiario SET estatus='A', matricula='" + matricula + "', CCT='" + CCT + "' WHERE clave='" + clave + "' ";
                SqlCommand updatecmd = new SqlCommand(update, conex);
                updatecmd.ExecuteNonQuery();

                string update2 = "UPDATE Usuario SET pertenece='" + CCT + "', estatus='E' WHERE CURP='" + CURP.Trim() + "'";
                SqlCommand updatecmd2 = new SqlCommand(update2, conex);
                updatecmd2.ExecuteNonQuery();
            }
        }

        public void ingresoBeneficiario(string clave, string CURP, string CCT, string matricula, string fecha_actual)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: 
            // Fecha Modificación: 10/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into institucion_beneficiario(clave,CCT,CURP,matricula,estatus,fecha_reg) Values('" + clave + "','" + CCT + "','" + CURP + "','" + matricula + "','A','" + fecha_actual + "')";
                string query2 = "UPDATE Usuario SET pertenece='" + CCT + "', estatus='E' WHERE CURP='" + CURP + "'";

                SqlCommand cmd = new SqlCommand(query2, conex);
                cmd.ExecuteNonQuery();
                SqlCommand cmd2 = new SqlCommand(query, conex);
                cmd2.ExecuteNonQuery();
                conex.Close();
            }
        }


        public void consultaBeneficiarioRegistrado(string CURP, string CCT)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: 
            // Fecha Modificación: 10/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando2 = new SqlCommand(string.Format("select CURP,matricula from institucion_beneficiario WHERE CURP='{0}' AND estatus='A' AND CCT='{1}'", CURP, CCT), conex);
                SqlDataReader reader = comando2.ExecuteReader();

                while (reader.Read())
                {
                    setCURP(reader.GetString(0));
                    setMatricula(reader.GetString(1));
                }
                reader.Close();
                conex.Close();
            }
        }

        public void ingresaErrores(string clave, string CCT, string CURP, string matricula, string ape_pat, string ape_mat, string nombre, string fecha_actual, string error)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: 
            // Fecha Modificación: 10/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into benefi_error Values('" + clave + "','" + CCT + "','" + CURP + "','" + matricula + "','" + ape_pat + " " + ape_mat + " " + nombre + "','" + fecha_actual + "','" + error + "')";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }

        public void consultaBenefEnOtraInst(string CURP, string CCT)
        {

            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: 
            // Fecha Modificación: 10/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando2 = new SqlCommand(string.Format("select CURP from institucion_beneficiario WHERE CURP='{0}' AND estatus='A' AND CCT!='{1}'", CURP, CCT), conex);
                SqlDataReader reader = comando2.ExecuteReader();

                while (reader.Read())
                {
                    setCURP(reader.GetString(0));
                }
                reader.Close();
                conex.Close();
            }
        }

        public void registraNuevoBenef(string clave, string CCT, string CURP, string matricula, string fecha_actual, string nombre, string ape_pat, string ape_mat, string lugar_nac, string sexo, string fecha_nac)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: 
            // Fecha Modificación: 10/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            string CURP_folio = "";

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into institucion_beneficiario(clave,CCT,CURP,matricula,estatus,fecha_reg) Values('" + clave + "','" + CCT + "','" + CURP.Trim() + "','" + matricula + "','A','" + fecha_actual + "')";
                //string query2 = "Insert into Usuario(CURP,nombre,apellido_pat,apellido_mat,estado_nac,sexo,fecha_nac,password,tipo,estatus,pertenece) values('" + CURP.Trim() + "','" + nombre + "','" + ape_pat + "','" + ape_mat + "'," + lugar_nac + "," + sexo + ",'" + fecha_nac + "','','B','E','" + CCT + "')";
                string query2 = "Insert into persona(CURP,nombre,apellido_pat,apellido_mat,estado_nac,sexo,fecha_nac) values('" + CURP.Trim() + "','" + nombre + "','" + ape_pat + "','" + ape_mat + "'," + lugar_nac + "," + sexo + ",'" + fecha_nac + "')";
                string query3 = "Insert into Usuario(CURP,password,tipo,estatus,pertenece) values('" + CURP.Trim() + "','','B','E','" + CCT + "')";

                SqlCommand cmd = new SqlCommand(query2, conex);
                cmd.ExecuteNonQuery();
                SqlCommand cmd2 = new SqlCommand(query, conex);
                cmd2.ExecuteNonQuery();
                SqlCommand cmd3 = new SqlCommand(query3, conex);
                cmd3.ExecuteNonQuery();
                conex.Close();
            }

            using (SqlConnection conexion = Conexion.conexion())
            {
                
                SqlCommand comando2 = new SqlCommand(string.Format("select CURP from folio WHERE CURP='{0}' ", CURP), conexion);
                SqlDataReader reader = comando2.ExecuteReader();
                while (reader.Read())
                {
                    CURP_folio = reader.GetString(0);
                }
                reader.Close();
                conexion.Close();
            }

            if (CURP_folio.Equals(null) || CURP_folio.Equals("")) {

                using (SqlConnection conex = Conexion.conexion())
                {
                    string query4 = "INSERT INTO folio(CURP) VALUES('" + CURP.Trim() + "')";
                    SqlCommand cmd4 = new SqlCommand(query4, conex);
                    cmd4.ExecuteNonQuery();
                    conex.Close();
                }

            }


        }

        public void registraCURPNoValido(string clave, string CCT, string CURP, string matricula, string fecha_actual)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: 
            // Fecha Modificación: 10/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into benefi_error Values('" + clave + "','" + CCT + "','" + CURP + "','" + matricula + "','','" + fecha_actual + "','Verifique el CURP" + "')";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
            }
        }


        public void registraCURPDuplicado(string clave, string CCT, string CURP, string matricula, string fecha_actual)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: 
            // Fecha Modificación: 10/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into benefi_error Values('" + clave + "','" + CCT + "','" + CURP + "','" + matricula + "','','" + fecha_actual + "','El CURP ya fue registrado anteriormente')";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
            }
        }

        public void registraErrorDependecia(string CCT, string CURP, string matricula)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: 
            // Fecha Modificación: 10/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into benefi_error(clave,cct,curp,matricula,estatus,fecha_reg,mensaje) Values('','" + CCT + "','" + CURP + "','" + matricula + "','','','El CCT del alumno no pertenece a su CCT" + "')";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
            }
        }

        public void registraErrorFormato(string CCT)
        {

            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: 
            // Fecha Modificación: 10/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into benefi_error(clave,cct,curp,matricula,estatus,fecha_reg,mensaje) Values('','" + CCT + "','','','','','Archivo No Tiene Formato Correcto" + "')";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
            }
        }

        public void registraErrorAbsoluto()
        {

            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: 
            // Fecha Modificación: 10/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into benefi_error(clave,cct,curp,matricula,estatus,fecha_reg,mensaje) Values('','" + "','" + "','" + "','','','Ocurrio un error insesperado, vuelva a intentar')";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
            }
        }

        public void archivoNoAdmitido(string CCT) {


            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: 
            // Fecha Modificación: 10/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into benefi_error(clave,cct,curp,matricula,estatus,fecha_reg,mensaje) Values('','" + CCT + "','','','','','Archivo no Admitido" + "')";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
            }
        }


        public void registrBenefDatos(string clave, string CCT, string CURP, string matricula, 
        string calle, string num_int, string num_ext, string colonia, string estado, string municipio, string codigo_postal, string telefono_casa,
        string celular, string tutor, string credencial,
        string fecha_actual, string nombre, string ape_pat, string ape_mat, string lugar_nac, string sexo, string fecha_nac, string correo)
        {
            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: 
            // Fecha Modificación: 10/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

        string CURP_folio = "";

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into institucion_beneficiario(clave,CCT,CURP,matricula,estatus,fecha_reg) Values('" + clave + "','" + CCT + "','" + CURP.Trim() + "','" + matricula + "','A','" + fecha_actual + "')";
                string query2 = "Insert into persona(CURP,nombre,apellido_pat,apellido_mat,estado_nac,sexo,fecha_nac,estado_r,municipio,colonia,calle,num_int,num_ext,telefono,celular, correo) values('" 
                  + CURP.Trim() + "','" + nombre + "','" + ape_pat + "','" + ape_mat + "'," + lugar_nac + "," + sexo + ",'" + fecha_nac +
                   "','" + estado + "','" + municipio + "','" + colonia  + "','" + calle + "','" + num_int +
                   "','" + num_ext + "','" + telefono_casa + "','" + celular + "','" + correo + "')";

                string query3 = "Insert into Usuario(CURP,password,tipo,estatus,pertenece) values('" + CURP.Trim() + "','','B','E','" + CCT + "')";

                SqlCommand cmd = new SqlCommand(query2, conex);
                cmd.ExecuteNonQuery();
                SqlCommand cmd2 = new SqlCommand(query, conex);
                cmd2.ExecuteNonQuery();
                SqlCommand cmd3 = new SqlCommand(query3, conex);
                cmd3.ExecuteNonQuery();
                conex.Close();
            }

            using (SqlConnection conexion = Conexion.conexion())
            {

                SqlCommand comando2 = new SqlCommand(string.Format("select CURP from folio WHERE CURP='{0}' ", CURP), conexion);
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
                    string query4 = "INSERT INTO folio(CURP, tutor, credencial) VALUES('" + CURP.Trim() + "','" + tutor + "','" + credencial  + "')";
                    SqlCommand cmd4 = new SqlCommand(query4, conex);
                    cmd4.ExecuteNonQuery();
                    conex.Close();
                }

            }
        }

        public void CCTNoAdmitido(string CCT)
        {

            //---------------------------------------------------------------------------------------------
            // Autor: Irving Lahera
            // Descripción: 
            // Fecha Modificación: 10/10/2017
            // Autor Modificación: Irving Lahera
            //---------------------------------------------------------------------------------------------

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into benefi_error(clave,cct,curp,matricula,estatus,fecha_reg,mensaje) Values('','" + CCT + "','','','','','El CCT no depende de su institución'" + "')";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
            }
        }



    }//Fin Clase
}