using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Educafin.Clases.Transacciones
{
    public class HomeTransacciones
    {

        private string CURP = "", nombre = "", password = "", estatus = "", tipo = "", pertenece = "", grado = "", dependencia = "";
        private bool medioProveedor = false;
        private string fecha_limite = "", correo = "";
        private DateTime date_limit;
        private DateTime date_Time = DateTime.Now;

        public void setCorreo(String correo) { this.correo = correo; }
        public string getCorreo() { return this.correo; }

        public void setFechalimite(String fecha_limite) { this.fecha_limite = fecha_limite; }
        public string getFechaLimite() { return this.fecha_limite; }

        public void setDateLimit(DateTime date_limit) { this.date_limit = date_limit; }
        public DateTime getDateLimit() { return this.date_limit; }

        public void setDateTime(DateTime date_Time) { this.date_Time = date_Time; }
        public DateTime getDateTime() { return this.date_Time; }



        public void setCURP(String CURP) { this.CURP = CURP; }
        public string getCURP() { return this.CURP; }

        public void setPassword(String password) { this.password = password; }
        public string getPassword() { return this.password; }
        public void setEstaus(String estatus) { this.estatus = estatus; }
        public string getEstatus() { return this.estatus; }
        public void setTipo(String tipo) { this.tipo = tipo; }
        public string getTipo() { return this.tipo; }
        public void setPertenece(String pertenece) { this.pertenece = pertenece; }
        public string getPertenece() { return this.pertenece; }
        public void setNombre(String nombre) { this.nombre = nombre; }
        public string getNombre() { return this.nombre; }


        public void setGrado(String grado) { this.grado = grado; }
        public string getGrado() { return this.grado; }

        public void setMedioProveedor(bool medioProveedor) { this.medioProveedor = medioProveedor; }
        public bool getMedioProveedor() { return this.medioProveedor; }

        public void setDependencia(String dependencia) { this.dependencia = dependencia; }
        public string getDependencia() { return this.dependencia; }

        public void consultaDatosUsuario(string txtNombre)
        {
            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format("SELECT A.CURP, A.nombre, A.apellido_pat, A.apellido_mat, B.password, B.estatus, B.tipo, B.pertenece FROM persona AS A, usuario AS B WHERE A.CURP = B.CURP AND A.CURP COLLATE Latin1_General_CS_AS = '{0}'", txtNombre), conex);
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {
                    this.setCURP(read.GetString(0));
                    this.setNombre(read.GetString(1) + " " + read.GetString(2) + " " + read.GetString(3));
                    if (read.GetString(4) != null)
                        this.setPassword(read.GetString(4));
                    this.setEstaus(read.GetString(5));
                    this.setTipo(read.GetString(6));
                    this.setPertenece(read.GetString(7));
                }
                read.Close();
                conex.Close();
            }
        }


        public void consultaInstitucion(string CCT)
        {
            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando2 = new SqlCommand(string.Format("Select grado, medioprovedor, ISNULL(dependencia,'') FROM institucion WHERE CCT = '{0}'", CCT), conex);
                SqlDataReader read2 = comando2.ExecuteReader();
                if (read2.Read())
                {
                    this.setGrado(read2.GetString(0));
                    this.setMedioProveedor(read2.GetBoolean(1));
                    this.setDependencia(read2.GetString(2));
                    System.Diagnostics.Debug.WriteLine("Grado: " + this.getGrado() +  " MedioProveedor: " + this.getMedioProveedor() + " Dependencia: " + this.getDependencia());

                }
                read2.Close();
                conex.Close();
            }
        }


        public String consultaMedioP(string CCT)
        {
            string medioP = "";

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando2 = new SqlCommand(string.Format("Select medioprovedor FROM institucion WHERE CCT = '{0}'", CCT), conex);
                SqlDataReader read2 = comando2.ExecuteReader();
                if (read2.Read())
                {
                    medioP = read2.GetString(0).ToString();
                }
                read2.Close();
                conex.Close();
            }
            return medioP;
        }


        public void consultaFechaLimite(string CURP)
        {
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comand = new SqlCommand(string.Format(
                "SELECT fecha_limite FROM inst_temporal WHERE CURP='{0}'", CURP), conexion);
                SqlDataReader reader = comand.ExecuteReader();

                while (reader.Read())
                {
                    this.setFechalimite(reader.GetString(0));
                }
                conexion.Close();
                this.setDateLimit(Convert.ToDateTime(this.getFechaLimite()));
            }
        }

        public void bloqueoUsuarioTemp(String CURP)
        {
            using (SqlConnection conexion = Conexion.conexion())
            {
                string query = "UPDATE usuario SET estatus='I' WHERE CURP='" + CURP + "'";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
        }

        public void cambiaContraseña(string pass, string CURP)
        {
            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format("UPDATE Usuario SET password = '{0}', estatus = 'A' WHERE CURP = '{1}'", pass, CURP), conex);
                SqlDataReader read = comando.ExecuteReader();
                read.Close();
                conex.Close();
            }
        }

        public void consultaAvisoPrivacidad(string CURP)
        {
            string consulta = "";

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "SELECT CURP FROM usuario WHERE avisoPrivacidad='Acepto' AND CURP='{0}'", CURP), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    consulta = reader.GetString(0);
                }
                conexion.Close();
            }

            if (consulta.Equals(null) || consulta.Equals(""))
            {

                using (SqlConnection conexion = Conexion.conexion())
                {
                    string query = "UPDATE usuario SET avisoPrivacidad='Acepto' WHERE CURP='" + CURP + "'";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.ExecuteNonQuery();
                    conexion.Close();
                }
            }
        }

        public void cambiaUsuarioEspera(string curp, string pass)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand("UPDATE Usuario SET password=@pass, estatus='E' where CURP=@curp2;", conex);
                comando.Parameters.AddWithValue("@pass", pass.ToString());
                comando.Parameters.AddWithValue("@curp2", curp.ToString());
                SqlDataReader read = comando.ExecuteReader();

                read.Close();
                conex.Close();
            }
        }

        public void consultaCorreo(string CURP)
        {
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "SELECT correo FROM Persona WHERE CURP='{0}'", CURP), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    this.setCorreo(reader.GetString(0));
                }
                conexion.Close();
            }

        }





    }//Fin Clase
}