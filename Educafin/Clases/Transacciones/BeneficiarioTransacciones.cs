using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Educafin.Clases.Transacciones
{
    public class BeneficiarioTransacciones
    {
        string CURP = "", nombre = "", apellido_pat = "", apellido_mat = "", fecha_nac = "", estado_r = "", municipio = "",
        colonia = "", calle = "", numero_int = "", numero_ext = "", telefono = "", correo = "", codigo_postal = "",
        celular = "", tutor = "", credencial = "", estatus = "";


        public void setCURP(String CURP) { this.CURP = CURP; }
        public string getCURP() { return this.CURP; }
        public void setNombre(String nombre) { this.nombre = nombre; }
        public string getNombre() { return this.nombre; }
        public void setApePat(String apellido_pat) { this.apellido_pat = apellido_pat; }
        public string getApePat() { return this.apellido_pat; }
        public void setApeMat(String apellido_mat) { this.apellido_mat = apellido_mat; }
        public string getApeMat() { return this.apellido_mat; }
        public void setFechaNac(String fecha_nac) { this.fecha_nac = fecha_nac; }
        public string getFechaNac() { return this.fecha_nac; }
        public void setEdoRec(String estado_r) { this.estado_r = estado_r; }
        public string getEdoRec() { return this.estado_r; }
        public void setMunicipio(String municipio) { this.municipio = municipio; }
        public string getMunicipio() { return this.municipio; }
        public void setColonia(String colonia) { this.colonia = colonia; }
        public string getColonia() { return this.colonia; }
        public void setCalle(String calle) { this.calle = calle; }
        public string getCalle() { return this.calle; }
        public void setNumInt(String numero_int) { this.numero_int = numero_int; }
        public string getNumInt() { return this.numero_int; }
        public void setNumExt(String numero_ext) { this.numero_ext = numero_ext; }
        public string getNumExt() { return this.numero_ext; }
        public void setTelefono(String telefono) { this.telefono = telefono; }
        public string getTelefono() { return this.telefono; }
        public void setCorreo(String correo) { this.correo = correo; }
        public string getCorreo() { return this.correo; }
        public void setCodigoPostal(String codigo_postal) { this.codigo_postal = codigo_postal; }
        public string getCodigoPostal() { return this.codigo_postal; }
        public void setCelular(String celular) { this.celular = celular; }
        public string getCelular() { return this.celular; }
        public void setTutor(String tutor) { this.tutor = tutor; }
        public string getTutor() { return this.tutor; }
        public void setCredencial(String credencial) { this.credencial = credencial; }
        public string getCredencial() { return this.credencial; }
        public void setEstatus(String estatus) { this.estatus = estatus; }
        public string getEstatus() { return this.estatus; }


        public void beneficiarioContraseña(string CURP, string correo, string password)
        {

            //using (SqlConnection conex = Conexion.conexion())
            //{
            //    SqlCommand comando = new SqlCommand("UPDATE usuario SET password=@pass, correo=@correo2, estatus='E' where CURP=@curp2;", conex);
            //    comando.Parameters.AddWithValue("@pass", password.ToString());
            //    comando.Parameters.AddWithValue("@curp2", CURP.ToString());
            //    comando.Parameters.AddWithValue("@correo2", correo.ToString());

            //    SqlDataReader read = comando.ExecuteReader();
            //    read.Close();
            //    conex.Close();
            //}

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand("UPDATE persona SET correo=@correo2 where CURP=@curp2;", conex);
                comando.Parameters.AddWithValue("@correo2", correo.ToString());
                comando.Parameters.AddWithValue("@curp2", CURP.ToString());

                SqlDataReader read = comando.ExecuteReader();
                read.Close();
                conex.Close();
            }

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand("UPDATE usuario SET password=@pass, estatus='E' where CURP=@curp2;", conex);
                comando.Parameters.AddWithValue("@pass", password.ToString());
                comando.Parameters.AddWithValue("@curp2", CURP.ToString());

                SqlDataReader read = comando.ExecuteReader();
                read.Close();
                conex.Close();
            }
        }

        public void consultaDatosBeneficiario()
        {
            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format("Select correo FROM persona WHERE CURP = '{0}'", CURP), conex);
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {
                    this.setCorreo(read.GetString(0));
                }
                read.Close();
                conex.Close();
            }

        }


        public void consultaCorreo(string CURP)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                //"SELECT A.curp, A.nombre, A.apellido_pat, A.apellido_mat, A.fecha_nac, A.estado_r, A.municipio, A.colonia, A.calle, A.num_int, A.num_ext, A.telefono, A.correo, A.codigopostal, A.celular, B.estatus, B.credencial, B.tutor from usuario as A, institucion_beneficiario as B WHERE A.CURP = B.CURP AND A.CURP = '" + CURP + "'"
                "SELECT P.curp, P.nombre, P.apellido_pat, P.apellido_mat, P.fecha_nac, P.estado_r, P.municipio, P.colonia," +
                "P.calle, P.num_int, P.num_ext, P.telefono, P.correo, A.codigopostal, A.celular, I.estatus, I.credencial, I.tutor " +
                "from persona as P, institucion_beneficiario as I " +
                "WHERE P.CURP = I.CURP AND P.CURP = '" + CURP + "'"), conex);
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {
                    this.setCURP(read.GetString(0).ToString());
                    this.setNombre(read.GetString(1).ToString());
                    this.setApePat(read.GetString(2).ToString());
                    this.setApeMat(read.GetString(3).ToString());
                    this.setFechaNac(read.GetString(4).ToString());
                    this.setEdoRec(read.GetString(5).ToString());
                    this.setMunicipio(read.GetString(6).ToString());
                    this.setColonia(read.GetString(7).ToString());
                    this.setCalle(read.GetString(8).ToString());
                    this.setNumInt(read.GetString(9).ToString());
                    this.setNumExt(read.GetString(10).ToString());
                    this.setTelefono(read.GetString(11).ToString());
                    this.setCorreo(read.GetString(12).ToString());
                    this.setCodigoPostal(read.GetString(13).ToString());
                    this.setCelular(read.GetString(14).ToString());
                    this.setEstatus(read.GetString(15).ToString());
                    this.setCredencial(read.GetString(16).ToString());
                    this.setTutor(read.GetString(17).ToString());
                }
                read.Close();
                conex.Close();
            }
        }


        public string consultaDomicilio(string CURP) {
            //string consulta = "SELECT u.codigopostal, u.calle, i.Credencial " +
            //                "from usuario u join institucion_beneficiario i " +
            //                "on u.CURP = i.CURP WHERE u.CURP = '" + CURP + "' AND i.estatus = 'A'";

            string consulta = "SELECT P.codigopostal, P.calle, I.Credencial " +
                    "from persona P join institucion_beneficiario I " +
                    "on P.CURP = I.CURP WHERE P.CURP = '" + CURP + "' AND I.estatus = 'A'";

            return consulta;
        }



        public string consultaCredencial(string CURL) {
            string consulta = "SELECT Credencial from institucion_beneficiario " +
                             "WHERE CURP = '" + CURP + "' AND estatus = 'A'";

          return consulta;
        }

        public string consultaDatosInstitucion(string CURP) {
            string consulta= "SELECT nombre from institucion " +
                                 "WHERE CCT = (SELECT dependencia from institucion " +
                                 "WHERE CCT = (SELECT CCT from institucion_beneficiario " +
                                 "WHERE CURP = '" + CURP + "' AND estatus = 'A')) ";

            return consulta;
        }

        public string consultaDatosBeneficiario(string CURP) {
            string consulta = "SELECT * from persona WHERE curp = '" + CURP + "'";

            return consulta;
        }

        public string consultaBeneficiarioActivo(string CURP)
        {
            string consulta = "SELECT * from institucion_beneficiario "
                    + "WHERE CURP = '" + CURP + "' AND estatus = 'A'";

            return consulta;
        }

        public string consultaTabletaAsignada(string CURP)
        {
            string consulta = "SELECT * from tablet "
                    + "WHERE beneficiario = '" + CURP + "'";

            return consulta;
        }

        public string consultaInstitucionDelBeneficiario(string CURP)
        {
            string consulta = "SELECT * from institucion "
                    + "WHERE CCT = (SELECT CCT from institucion_beneficiario "
                    + "WHERE CURP = '" + CURP + "' AND estatus = 'A')";

            return consulta;
        }

        public string consultaBeneficiarioEInstitucion(string CURP)
        {
            //AL PRINCIPIO string consulta = "SELECT * from usuario u inner join institucion_beneficiario i on u.curp = i.curp WHERE u.curp = '" + CURP + "'";
            // string consulta = "SELECT * from persona P inner join institucion_beneficiario I on P.curp = I.curp WHERE P.curp = '" + CURP + "'";
            string consulta = "SELECT A.nombre, A.apellido_pat, A.apellido_mat, A.fecha_nac, A.estado_r, A.municipio, " +
            "A.colonia, A.calle, A.num_int, A.num_ext, A.telefono, A.correo, A.codigopostal, " +
            "A.celular, C.tutor, C.Credencial, B.estatus FROM persona A " +
            "LEFT JOIN folio C ON C.CURP = A.CURP "+
            "LEFT JOIN institucion_beneficiario B ON C.CURP = B.CURP " +
            "WHERE A.CURP = '" + CURP + "'";
            return consulta;
        }


    }
}