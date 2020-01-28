using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Educafin.Clases.Transacciones
{
    public class InstitucionPlantelesTransacciones
    {
        List<Models.Alumno> lista = new List<Models.Alumno>();

        string matricula = "";
        public void setMatricula(String matricula) { this.matricula = matricula; }
        public string getMatricula() { return this.matricula; }

        string CURP = "", estatus = "";
        public void setCURP(String CURP) { this.CURP = CURP; }
        public string getCURP() { return this.CURP; }
        public void setEstatus(String estatus) { this.estatus = estatus; }
        public string getEstatus() { return this.estatus; }

        public List<Models.Alumno> consultaErroresBeneneficiarios(string CCT)
        {

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
            borarDatosTemporales(CCT);
            return lista;
        }

        public List<Models.Alumno> consultaBeneficiariosRegistrados(string CCT)
        {
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                //"SELECT I.CURP, I.matricula, U.apellido_pat,U.apellido_mat, U.nombre FROM institucion_beneficiario AS I, usuario AS U WHERE I.CURP = U.CURP AND CCT = '{0}' AND I.estatus = 'A' ORDER BY apellido_pat,apellido_mat,nombre"
                "SELECT I.CURP, I.matricula, P.apellido_pat, P.apellido_mat, P.nombre " +
                "FROM institucion_beneficiario AS I, persona AS P " +
                "WHERE I.CURP = P.CURP AND I.CCT = '{0}' AND I.estatus = 'A' ORDER BY apellido_pat, apellido_mat, nombre", CCT), conexion);
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

        public void bajaBeneficiario(string CURP, string CCT)
        {
            using (SqlConnection conex = Conexion.conexion())
            {

                SqlCommand comando = new SqlCommand(string.Format(
                "UPDATE institucion_beneficiario SET estatus='I', matricula='' WHERE CURP='" + CURP + "' and CCT='{0}'", CCT), conex);
                comando.ExecuteNonQuery();

                comando = new SqlCommand(string.Format(
                "UPDATE usuario SET pertenece=null, estatus='I' WHERE CURP='" + CURP + "'"), conex);
                comando.ExecuteNonQuery();
            }
        }

        public List<Models.Alumno> consultaBenefConTablet(string CCT)
        {

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "SELECT I.CURP, I.matricula, U.apellido_pat, U.apellido_mat, U.nombre,T.serie,T.fecha FROM institucion_beneficiario AS I, usuario AS U, [ tablet] AS T WHERE I.CURP = T.beneficiario and I.CURP = U.CURP AND T.intitucion = '{0}' AND I.matricula != '' ORDER BY apellido_pat, apellido_mat, nombre", CCT), conexion);
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
                    lista.Add(dato);
                    j++;
                }
                conexion.Close();
            }
            return lista;
        }


        public void bajaBenefConTablet(string CURP, string fecha_actual)
        {
            using (SqlConnection conect = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select serie,intitucion,beneficiario,grado from tablet where beneficiario='{0}'", CURP), conect);
                SqlDataReader reader = comando.ExecuteReader();
                string[] datos = null;
                while (reader.Read())
                {
                    datos = new string[]{
                   reader.GetString(0) , reader.GetString(1), reader.GetString(2), reader.GetString(3)
                   };
                }
                reader.Close();

                comando = new SqlCommand(string.Format("Insert into tablet_baja values ('" + datos[0] + "','" + datos[1] + "','" + datos[2] + "','" + fecha_actual + "','" + datos[3] + "')"), conect);
                comando.ExecuteNonQuery();
                conect.Close();
            }

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "update tablet set beneficiario=null, fecha=null where beneficiario = '" + CURP + "'"), conex);
                comando.ExecuteNonQuery();
            }
        }

        public List<Models.Alumno> consultaErrorBeneficiario(string CCT)
        {

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
            borarDatosTemporales(CCT);

            return lista;
        }

        public List<Models.Alumno> consultaErrorTablets(string CCT)
        {

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
            borarDatosTemporales(CCT);

            return lista;
        }

        public void borarDatosTemporales(string CCT)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "delete from benefi_error where CCT like'" + CCT + "'";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }

        public string consultaDependencia(string CCT)
        {

            string dependencia = "";
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando2 = new SqlCommand(string.Format("select ISNULL(dependencia,'') Dependencia from institucion WHERE CCT='{0}' ", CCT), conexion);
                SqlDataReader reader = comando2.ExecuteReader();

                while (reader.Read())
                {
                    if (reader.GetString(0) != "" || reader.GetString(0) != null)
                    {
                        dependencia = reader.GetString(0);
                    }
                }
                reader.Close();
                conexion.Close();
            }
            return dependencia;
        }


        public void consultaestatusBeneficiario(string CURP)
        {

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando2 = new SqlCommand(string.Format("select CURP,estatus from persona WHERE CURP='{0}'", CURP), conexion);
                SqlDataReader reader = comando2.ExecuteReader();

                while (reader.Read())
                {
                    setCURP(reader.GetString(0));
                    setEstatus(reader.GetString(1));
                }
                reader.Close();
                conexion.Close();
            }
        }

        public void consultaBeneficiariosInactivos(string CURP, string CCT)
        {

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando2 = new SqlCommand(string.Format("select CURP, estatus from institucion_beneficiario WHERE CURP='{0}' AND estatus='I' AND CCT='{1}'", CURP, CCT), conexion);
                SqlDataReader reader = comando2.ExecuteReader();

                while (reader.Read())
                {
                    setCURP(reader.GetString(0));
                    setEstatus(reader.GetString(1));

                }
                reader.Close();
                conexion.Close();
            }
        }

        public void trasladoBeneficiario(string clave, string CURP, string matricula, string CCT )
        {
            using (SqlConnection conex = Conexion.conexion())
            {
                string update = "UPDATE institucion_beneficiario SET estatus='A', matricula='" + matricula + "', CCT='" + CCT + "' WHERE clave='" + clave + "' ";
                SqlCommand updatecmd = new SqlCommand(update, conex);
                updatecmd.ExecuteNonQuery();

                string update2 = "UPDATE usuario SET pertenece='" + CCT + "', estatus='E' WHERE CURP='" + CURP + "'";
                SqlCommand updatecmd2 = new SqlCommand(update2, conex);
                updatecmd2.ExecuteNonQuery();
            }
        }



        public void ingresoBeneficiario(string clave, string CURP, string CCT, string matricula, string fecha_actual)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into institucion_beneficiario(clave,CCT,CURP,matricula,estatus,fecha_reg) Values('" + clave + "','" + CCT + "','" + CURP.Trim() + "','" + matricula + "','A','" + fecha_actual + "')";
                string query2 = "UPDATE usuario SET pertenece='" + CCT + "', estatus='E' WHERE CURP='" + CURP.Trim() + "'";

                SqlCommand cmd = new SqlCommand(query2, conex);
               // cmd.ExecuteNonQuery();
                SqlCommand cmd2 = new SqlCommand(query, conex);
                //cmd2.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine(query);
                conex.Close();
            }
        }



        public void consultaBeneficiarioRegistrado(string CURP, string CCT)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando2 = new SqlCommand(string.Format("select CURP,matricula from institucion_beneficiario WHERE CURP='{0}' AND estatus='A'", CURP, CCT), conex);
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

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando2 = new SqlCommand(string.Format("select CURP, estatus from institucion_beneficiario WHERE CURP='{0}' AND estatus='I' AND CCT!='{1}'", CURP, CCT), conex);
                SqlDataReader reader = comando2.ExecuteReader();

                while (reader.Read())
                {
                    setCURP(reader.GetString(0));
                    setEstatus(reader.GetString(1));
                }
                reader.Close();
                conex.Close();
            }
        }

        public void registraNuevoBenef(string clave, string CCT, string CURP, string matricula, string fecha_actual, string nombre, string ape_pat, string ape_mat, string lugar_nac, string sexo, string fecha_nac)
        {
            string CURP_folio = "";

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into institucion_beneficiario(clave,CCT,CURP,matricula,estatus,fecha_reg) Values('" + clave + "','" + CCT + "','" + CURP + "','" + matricula + "','A','" + fecha_actual + "')";
                string query2 = "Insert into persona(CURP,nombre,apellido_pat,apellido_mat,estado_nac,sexo,fecha_nac) values('" + CURP + "','" + nombre + "','" + ape_pat + "','" + ape_mat + "','" + lugar_nac + "','" + sexo + "','" + fecha_nac + "')";
                string query3 = "Insert into usuario(CURP,password,tipo,estatus,pertenece) values('" + CURP + "','','B','E','" + CCT + "')";
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
                        string query4 = "INSERT INTO folio(CURP) VALUES('" + CURP.Trim() + "')";
                        SqlCommand cmd4 = new SqlCommand(query4, conex);
                        cmd4.ExecuteNonQuery();
                        conex.Close();
                    }

                }          
        }

        public void registraCURPNoValido(string clave, string CCT, string CURP, string matricula, string fecha_actual)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into benefi_error Values('" + clave + "','" + CCT + "','" + CURP + "','" + matricula + "','','" + fecha_actual + "','Verifique el CURP" + "')";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
            }
        }

        public void registraErrorDependecia(string CCT, string CURP, string matricula)
        {
            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into benefi_error(clave,cct,curp,matricula,estatus,fecha_reg,mensaje) Values('','" + CCT + "','" + CURP + "','" + matricula + "','','','El CCT no depende de su Institución" + "')";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
            }
        }


        public void registraErrorFormato(string CCT)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into benefi_error(clave,cct,curp,matricula,estatus,fecha_reg,mensaje) Values('','" + CCT + "','','','','','Archivo No Tiene Formato Correcto" + "')";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
            }
        }

        public void registraErrorAbsoluto()
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into benefi_error(clave,cct,curp,matricula,estatus,fecha_reg,mensaje) Values('','" + "','" + "','" + "','','','Ocurrio un error insesperado, vuelva a intentar')";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
            }
        }

        public void consultaBeneficiarioMatricula(string CURP)
        {
            List<Models.Alumno> lista = new List<Models.Alumno>();
            Models.Alumno dato = new Models.Alumno();

            using (SqlConnection conexion = Conexion.conexion())
            {

                SqlCommand comando = new SqlCommand(string.Format(
               // "SELECT I.CURP, I.matricula, U.apellido_pat, U.apellido_mat, U.nombre FROM institucion_beneficiario AS I, usuario AS U WHERE I.CURP = U.CURP AND I.matricula LIKE '" + matricula + "%' AND I.estatus='A'"
               "SELECT I.CURP, I.matricula, P.apellido_pat, P.apellido_mat, P.nombre " +
               "FROM institucion_beneficiario AS I, persona AS P " +
               "WHERE I.CURP = P.CURP AND I.matricula LIKE '" + matricula + "%' AND I.estatus = 'A'"), conexion);
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
                    dato.curp = CURP.ToString();
                    dato.matricula = matricula.ToString();
                    dato.nombre = "No Disponible";
                }
            }
        }

        public void consultaBenefPorCURP(string CURP)
        {
            List<Models.Alumno> lista = new List<Models.Alumno>();
            Models.Alumno dato = new Models.Alumno();

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                //"SELECT I.CURP, I.matricula, U.apellido_pat, U.apellido_mat, U.nombre FROM institucion_beneficiario AS I, usuario AS U WHERE I.CURP = U.CURP AND I.CURP LIKE '" + CURP + "%' AND I.estatus='A'"
                "SELECT I.CURP, I.matricula, P.apellido_pat, P.apellido_mat, P.nombre " + 
                "FROM institucion_beneficiario AS I, persona AS P " +
                "WHERE I.CURP = P.CURP AND I.CURP LIKE '" + CURP + "%' AND I.estatus = 'A'" ), conexion);
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
        }


        public void consultaBenefPorCURPYMatricula(string CURP, string matricula)
        {
            List<Models.Alumno> lista = new List<Models.Alumno>();
            Models.Alumno dato = new Models.Alumno();

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                //"SELECT I.CURP, I.matricula, U.apellido_pat, U.apellido_mat, U.nombre FROM institucion_beneficiario AS I, usuario AS U WHERE I.CURP = U.CURP AND I.CURP LIKE '" + CURP + "%' AND I.estatus='A' AND I.matricula LIKE '" + matricula + "%'"
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
        }


        public String consultaBeneficiarioTablet(string CURP)
        {
            string beneficiario = "";
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select beneficiario from tablet where beneficiario='{0}' ", CURP), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    beneficiario = reader.GetString(0);
                }
                reader.Close();
            }
            return beneficiario;
        }
        public string consultaNoSerie(string CCT, string serie)
        {
            string no_serie = "";

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format("select serie from tablet where intitucion='{0}'AND serie='{1}' AND beneficiario is not null ", CCT, serie), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    no_serie = reader.GetString(0);
                }
                reader.Close();
            }
            return no_serie;
        }

        public string consultaSerieNoAsignado(string CCT, string serie)
        {
            string no_serie = "";

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format("select serie from tablet where intitucion='{0}' AND serie='{1}' and beneficiario is null ", CCT, serie), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    no_serie = reader.GetString(0);
                }
                reader.Close();
            }
            return no_serie;
        }

        public void asignaSerieABeneficiario(string CURP, string fecha_actual, string grado, string serie)
        {
            List<Models.Alumno> lista = new List<Models.Alumno>();
            Models.Alumno dato = new Models.Alumno();

            using (SqlConnection conexion = Conexion.conexion())
            {
                string update = "update[ tablet] set beneficiario ='" + CURP + "', fecha = '" + fecha_actual + "', grado='" + grado + "' WHERE serie='" + serie + "'";
                SqlCommand updatecmd = new SqlCommand(update, conexion);
                updatecmd.ExecuteNonQuery();
                dato.status = "Tableta Registrada";
            }
        }

        public void consultaEstatusBeneficiario(string CURP)
        {

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando2 = new SqlCommand(string.Format("select CURP,estatus from usuario WHERE CURP='{0}' ", CURP), conexion);
                SqlDataReader reader = comando2.ExecuteReader();
                while (reader.Read())
                {
                    setCURP(reader.GetString(0));
                    setEstatus(reader.GetString(1));
                }
                reader.Close();
            }
        }

        public void consultaBeneficiarioEnInst(string CURP, string CCT)
        {

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando2 = new SqlCommand(string.Format("select CURP,matricula from institucion_beneficiario WHERE CURP='{0}' AND estatus='A' AND CCT='{1}'", CURP, CCT), conexion);
                SqlDataReader reader = comando2.ExecuteReader();
                while (reader.Read())
                {
                    setCURP(reader.GetString(0));
                    setMatricula(reader.GetString(1));
                }
                reader.Close();
            }
        }

        public void consultaBenenficiaio(string CURP)
        {

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format("select CURP from usuario WHERE CURP='{0}' ", CURP), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setCURP(reader.GetString(0));
                }
                reader.Close();
            }
        }


    }//FIN CLASE
}