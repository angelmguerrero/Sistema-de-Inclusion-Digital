using Educafin.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Educafin.Clases.Transacciones
{
    public class AdministradorTransacciones
    {

        private string grado = "", CCTinstitucion = "", nombre = "", RFCProveedor = "", CURP = "";


        public void setGrado(String grado) { this.grado = grado; }
        public string getGrado() { return this.grado; }
        public void setCCTInst(String CCTinstitucion) { this.CCTinstitucion = CCTinstitucion; }
        public string getCCTInst() { return this.CCTinstitucion; }
        public void setNombre(String nombre) { this.nombre = nombre; }
        public string getNombre() { return this.nombre; }


        public void setRFCProveedor(String RFCProveedor) { this.RFCProveedor = RFCProveedor; }
        public string getRFCProveedor() { return this.RFCProveedor; }
        public void setCURP(String CURP) { this.CURP = CURP; }
        public string getCURP() { return this.CURP; }

        public List<Models.Alumno> consultaInstitucionesRegistradas()
        {

            List<Models.Alumno> lista = new List<Models.Alumno>();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format("select CCT,nombre from institucion"), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                int j = 1;
                while (reader.Read())
                {
                    Models.Alumno dato = new Models.Alumno();
                    dato.no = j;
                    dato.cct = reader.GetString(0);
                    dato.cctNombre = reader.GetString(1);
                    lista.Add(dato);
                    j++;
                }
                conexion.Close();
            }
            return lista;
        }


        public void consultaGradoInst(string CCT)
        {

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format("SELECT grado FROM institucion WHERE CCT='{0}'", CCT), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setGrado(reader.GetString(0));
                }
            }
        }

        public List<BeneficiarioSinsesion> consultaBeneficiariosSinSesion()
        {

            List<BeneficiarioSinsesion> lista = new List<BeneficiarioSinsesion>();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                // "SELECT  CURP, correo, password  FROM usuario where estatus= 'E' and correo is not null and tipo='B'"
                "SELECT  B.CURP, A.correo, B.password  FROM persona as A, usuario as B where A.CURP = B.CURP AND B.estatus = 'E' and A.correo is not null and B.tipo = 'B'"), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                int j = 1;
                while (reader.Read())
                {
                    BeneficiarioSinsesion dato = new BeneficiarioSinsesion();
                    dato.no = j;
                    dato.curp = reader.GetString(0);
                    dato.correo = reader.GetString(1);
                    dato.password = reader.GetString(2);
                    lista.Add(dato);
                    j++;
                }
                conexion.Close();
            }
            return lista;
        }


        public void actualizaEstatusBeneficiario(string CURP, string CCT)
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


        public List<Models.Alumno> consultaBeneficiariosActvos(string CCT)
        {

            List<Models.Alumno> lista = new List<Models.Alumno>();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "SELECT I.CURP, I.matricula, U.apellido_pat,U.apellido_mat, U.nombre FROM institucion_beneficiario AS I, usuario AS U WHERE I.CURP = U.CURP AND I.CCT = '{0}' AND I.estatus='A'", CCT), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                int j = 1;
                while (reader.Read())
                {
                    Models.Alumno dato = new Models.Alumno();
                    dato.no = j;
                    dato.curp = reader.GetString(0);
                    dato.matricula = reader.GetString(1);
                    dato.nombre = reader.GetString(2) + " " + reader.GetString(3) + " " + reader.GetString(4);
                    dato.cct = CCT;
                    lista.Add(dato);
                    j++;
                }
                conexion.Close();
            }
            return lista;
        }


        public void registraTabletBenefInst(string CURP, string datetime)
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
                comando = new SqlCommand(string.Format("Insert into tablet_baja values ('" + datos[0] + "','" + datos[1] + "','" + datos[2] + "','" + datetime + "','" + datos[3] + "')"), conect);
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

        public List<Models.Alumno> consultaInfoBeneficiarioYTablet(string CCT)
        {

            List<Models.Alumno> lista = new List<Models.Alumno>();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                // "SELECT I.CURP, I.matricula, U.apellido_pat, U.apellido_mat, U.nombre,T.serie,T.fecha FROM institucion_beneficiario AS I, usuario AS U, tablet AS T WHERE I.CURP = T.beneficiario and I.CURP = U.CURP AND T.intitucion = '{0}' AND I.matricula != '' ORDER BY apellido_pat, apellido_mat, nombre"
                "SELECT U.CURP, I.matricula, P.apellido_pat, P.apellido_mat, P.nombre, T.serie, T.fecha " +
                 "FROM institucion_beneficiario AS I, usuario AS U, tablet AS T, persona AS P " +
                 "WHERE I.CURP = T.beneficiario and I.CURP = U.CURP AND U.CURP = P.CURP AND T.intitucion = '{0}' " +
                 "AND I.matricula != '' ORDER BY apellido_pat, apellido_mat, nombre", CCT), conexion);
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


        public List<InstitucionMasivoModel> consultaListaErrores()
        {

            List<InstitucionMasivoModel> lista = new List<InstitucionMasivoModel>();

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando2 = new SqlCommand(string.Format("SELECT CCT,mensaje FROM benefi_error WHERE CURP='EDUCAFIN'"), conexion);
                SqlDataReader reader = comando2.ExecuteReader();

                int j = 0;
                while (reader.Read())
                {
                    InstitucionMasivoModel dato = new InstitucionMasivoModel();

                    dato.no = j;
                    dato.CCT = reader.GetString(0);
                    dato.nombre = reader.GetString(1);
                    lista.Add(dato);
                    j++;
                }
                reader.Close();
                conexion.Close();
            }
            return lista;
        }

        public void eliminaErroresEducafin() {

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format("DELETE FROM benefi_error WHERE CURP='EDUCAFIN' "), conexion);
                comando.ExecuteNonQuery();
                conexion.Close();
            }
        }




        public void consultaExistenciaInstitucion(string CCT)
        {

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando2 = new SqlCommand(string.Format("SELECT CCT FROM institucion WHERE CCT='{0}'", CCT), conexion);
                SqlDataReader reader = comando2.ExecuteReader();

                while (reader.Read())
                {
                    setCCTInst(reader.GetString(0));
                }
                reader.Close();
                conexion.Close();

            }
        }

        public string consultaInstitucion(string CCT)
        {
            string consultaCCT = "";
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando2 = new SqlCommand(string.Format("SELECT CCT FROM institucion WHERE CCT='{0}'", CCT), conexion);
                SqlDataReader reader = comando2.ExecuteReader();

                while (reader.Read())
                {
                    consultaCCT = reader.GetString(0);
                }
                reader.Close();
                conexion.Close();
            }

            return consultaCCT;
        }


        public void registraInstitucion(string CCT, string nombre, string grado, string estado, string municipio, string colonia, string calle, string numero, string codigo_postal, string telefono, string correo, string medioproveedor, string dependencia)
        {
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format("INSERT INTO institucion VALUES('" + CCT + "','" + nombre + "','"
                    + grado + "','" + estado + "','" + municipio + "','" + colonia + "','" + calle + "','" + numero + "','"
                    + codigo_postal + "','" + telefono + "','" + correo + "','" + medioproveedor + "','" + dependencia + "')"), conexion);
                comando.ExecuteNonQuery();
                conexion.Close();
            }
        }

        public void registraErrorRegInst(string CCT, string nombre)
        {
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format("INSERT INTO benefi_error(CCT,CURP,mensaje) VALUES('" + CCT + "','EDUCAFIN','" + nombre + "')"), conexion);
                comando.ExecuteNonQuery();
                conexion.Close();
            }
        }

        public void bajasBeneficiarios()
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "UPDATE usuario set estatus='I' WHERE tipo='B'";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }



        public string regresaConsulta(string CCT)
        {
            string consulta = "SELECT * from institucion WHERE CCT = '" + CCT + "'";

            return consulta;
        }

        public string regresaConsultaUsuario()
        {
            string consulta = "SELECT P.CURP, P.nombre, P.apellido_pat, P.apellido_mat, U.password, U.estatus, U.tipo, U.pertenece " +
                              "FROM persona AS P, usuario AS U " +
                              "WHERE P.CURP = U.CURP AND P.CURP COLLATE Latin1_General_CS_AS = '{0}'";

            return consulta;
        }

        public Boolean realizaRegistro(string CURP)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(regresaConsultaUsuario(), CURP), conex);
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {
                    read.Close();
                    conex.Close();
                    return false;
                }
                else
                {
                    read.Close();
                    conex.Close();
                    return true;
                }
            }
        }


        public List<string> consultaModeloUsuario(string lugar, string clave, string pertenece)
        {
            string nombre = "";

            List<string> nombres = new List<string>();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format("select nombre from {0} where {1} = '{2}'", lugar, clave, pertenece), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                nombre = "";

                if (reader.Read())
                {
                    nombre = reader.GetString(0);
                }
                else
                {
                    nombre = "";
                }
                nombres.Add(nombre);
                reader.Close();
                conexion.Close();
            }
            return nombres;
        }

        public void borraProveedorInst(string CCT, string RFC)
        {

            using (SqlConnection conexion = Conexion.conexion())
            {
                string query = "delete from Proveedor_Institucion where cct_int = '" + CCT + "' and rfc_prov = '" + RFC + "'";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.ExecuteNonQuery();
            }
        }


        public void registraProveedorInst(string CCT, string RFC, string cantidad)
        {

            using (SqlConnection conexion = Conexion.conexion())
            {
                string query = "Insert into Proveedor_Institucion Values('" + CCT + "','" + RFC + "'," + cantidad + ")";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.ExecuteNonQuery();
            }
        }


        public List<Proveedor_Institucion> consultaEntregaProvInst(string RFC)
        {

            List<Proveedor_Institucion> lista = new List<Proveedor_Institucion>();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format("select i.CCT, i.nombre, p2.cantidad from Proveedor_Institucion p2 join institucion i on p2.cct_int = i.CCT where p2.rfc_prov='{0}'", RFC), conexion);

                SqlDataReader reader = comando.ExecuteReader();
                int j = 0;

                while (reader.Read())
                {
                    Proveedor_Institucion dato = new Proveedor_Institucion();
                    dato.no = j;
                    dato.cct_int = reader.GetString(0);
                    dato.nombre = reader.GetString(1);
                    dato.cantidad = reader.GetInt32(2);
                    lista.Add(dato);
                    j++;
                }
                reader.Close();
            }
            return lista;
        }


        public void consultaNombreProveedor(string RFC)
        {
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand com = new SqlCommand(string.Format("select nombre from proveedor where RFC LIKE '" + RFC + "%'", RFC), conexion);
                SqlDataReader reader1 = com.ExecuteReader();
                while (reader1.Read())
                {
                    setNombre(reader1.GetString(0));
                }
            }
        }


        public string consultaCURP(string CURP)
        {
            string consultaCURP = "";
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando2 = new SqlCommand(string.Format("SELECT CURP FROM usuario WHERE CURP='{0}'", CURP), conexion);
                SqlDataReader reader = comando2.ExecuteReader();

                while (reader.Read())
                {
                    consultaCURP = reader.GetString(0);
                }
                reader.Close();
                conexion.Close();
            }
            return consultaCURP;
        }


        public string consultaRFCProveedor(string RFC)
        {
            string consultaRFC = "";

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando2 = new SqlCommand(string.Format("SELECT RFC FROM proveedor WHERE RFC='{0}'", RFC), conexion);
                SqlDataReader reader = comando2.ExecuteReader();

                while (reader.Read())
                {
                    consultaRFC = reader.GetString(0);
                }
                reader.Close();
                conexion.Close();
            }
            return consultaRFC;
        }

        public void registraProveedores(string RFC, string nombre, string representante, string codigo_postal, string estado,
            string municipio, string colonia, string calle, string num_ext, string num_int, string telefono, string correo)
        {

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format("INSERT INTO proveedor VALUES('" + RFC + "','" + nombre + "','"
                    + representante + "','" + codigo_postal + "','" + estado + "','" + municipio + "','" + colonia + "','" + calle + "','"
                    + num_ext + "','" + num_int + "','" + telefono + "','" + correo + "')"), conexion);
                comando.ExecuteNonQuery();
                conexion.Close();
            }
        }

        public void registraErrorProveedor(string RFC, string nombre)
        {
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format("INSERT INTO benefi_error(CCT,CURP,mensaje) VALUES('" + RFC + "','EDUCAFIN','El proveedor: " + nombre + " ya fue registrado')"), conexion);
                comando.ExecuteNonQuery();
                conexion.Close();
            }
        }



        public void registraUsuario(string CURP, string nombre, string ape_pat, string ape_mat, string lugar_nac, string sexo, string fecha_nac,
                    string codigo_postal, string estado_recid, string municipio, string colonia, string calle, string numero_int,
            string numero_ext, string telefono, string tel_cel, string correo, string password, string tipo, string estatus, string pertenece)
        {

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format("INSERT INTO persona VALUES('" + CURP + "','" + nombre + "','"
                    + ape_pat + "','" + ape_mat + "'," + lugar_nac + "," + sexo + ",'" + fecha_nac + "','" + codigo_postal + "','"
                    + estado_recid + "','" + municipio + "','" + colonia + "','" + calle + "','" + numero_int + "','"
                    + numero_ext + "','" + telefono + "','" + tel_cel + "','" + correo + "')"), conexion);
                comando.ExecuteNonQuery();
                conexion.Close();
            }

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format("INSERT INTO usuario VALUES('" + CURP + "','" + password + "','" + tipo + "','" + estatus + "','" + pertenece + "',NULL)"), conexion);
                comando.ExecuteNonQuery();
                conexion.Close();
            }
        }

        public List<Models.institucion> consultaListaDeInstituciones()
        {


            List<Models.institucion> lista = new List<Models.institucion>();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "SELECT CCT, nombre, telefono, coreo, medioprovedor FROM institucion"), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                int j = 1;
                while (reader.Read())
                {
                    Models.institucion dato = new Models.institucion();
                    dato.CCT = reader.GetString(0);
                    dato.nombre = reader.GetString(1);
                    dato.telefono = reader.GetString(2);
                    dato.coreo = reader.GetString(3);
                    dato.medioprovedor = reader.GetBoolean(4);
                    lista.Add(dato);
                    j++;
                }
                conexion.Close();
            }
            return lista;
        }


        public void eliminaDependenciaCCT(string cctI)
        {
            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "UPDATE institucion SET dependencia =  null  WHERE CCT = '" + cctI + "'";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
            }
        }





        public void asignaDependenciaCCT(string cct, string cctI)
        {
            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "UPDATE institucion SET dependencia = '" + cct + "' WHERE CCT = '" + cctI + "'";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Models.institucion> consultaDependencia(string cct)
        {

            List<Models.institucion> lista = new List<Models.institucion>();
            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format("select CCT,nombre from institucion where dependencia='{0}'", cct), conex);
                SqlDataReader reader = comando.ExecuteReader();
                int j = 0;

                while (reader.Read())
                {
                    institucion dato = new institucion();
                    dato.CCT = reader.GetString(0);
                    dato.nombre = reader.GetString(1);
                    lista.Add(dato);
                    j++;
                }

                reader.Close();
                conex.Close();
            }


            return lista;
        }


        public String consultaNombreDependencia(string cct)
        {
            String nombre = "";

            List<Models.institucion> lista = new List<Models.institucion>();
            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand com = new SqlCommand(string.Format("select nombre from institucion where CCT='{0}'", cct), conex);
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    nombre = reader.GetString(0);
                }
                reader.Close();
                conex.Close();
            }
            return nombre;
        }


        public institucion consultaDatosInstitucion(string id)
        {
            institucion institucion = new institucion();

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
               "SELECT ISNULL(CCT, '') CCT, ISNULL(nombre, '') Nombre, ISNULL(grado, '') Grado, ISNULL(estado, '') Estado, ISNULL(municipio, '') Municipio, " +
               "ISNULL(colonia, '') Colonia, ISNULL(calle, '') Calle, ISNULL(numero, '') Numero, ISNULL(codigopostal, '') CodigoPostal, " +
               "ISNULL(telefono, '') Telefono, ISNULL(coreo, '') Correo, ISNULL(medioprovedor, '') MedioProveedor, " +
               "ISNULL(dependencia, '') Dependencia  FROM institucion WHERE CCT = '{0}'", id), conexion);
                SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    institucion.CCT = reader.GetString(0);
                    institucion.nombre = reader.GetString(1);
                    institucion.grado = reader.GetString(2);
                    institucion.estado = reader.GetString(3);
                    institucion.municipio = reader.GetString(4);
                    institucion.colonia = reader.GetString(5);
                    institucion.calle = reader.GetString(6);
                    institucion.numero = reader.GetString(7);
                    institucion.codigopostal = reader.GetString(8);
                    institucion.telefono = reader.GetString(9);
                    institucion.coreo = reader.GetString(10);
                    institucion.medioprovedor = reader.GetBoolean(11);
                    institucion.dependencia = reader.GetString(12);
                }
                conexion.Close();
            }
            return institucion;
        }



        public void actualizaDatosInstitucion(institucion institucion)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "UPDATE institucion SET CCT='" + institucion.CCT + "', nombre='" + institucion.nombre +
                "', grado='" + institucion.grado + "', estado='" + institucion.grado + "', municipio='" + institucion.municipio +
                "',colonia='" + institucion.colonia + "', calle='" + institucion.calle + "', numero='" + institucion.numero + "'," +
                "codigopostal='" + institucion.codigopostal + "', telefono = '" + institucion.telefono + "', coreo = '" + institucion.coreo +
                "', medioprovedor = '" + institucion.medioprovedor + "', dependencia = '" + institucion.dependencia + "' WHERE CCT = '" + institucion.CCT + "'";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
            }
        }




        public List<Models.proveedor> consultaProveedoresRegistrados()
        {

            List<Models.proveedor> lista = new List<Models.proveedor>();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "SELECT RFC, nombre, telefono, correo FROM proveedor"), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                int j = 1;
                while (reader.Read())
                {
                    Models.proveedor dato = new Models.proveedor();
                    dato.RFC = reader.GetString(0);
                    dato.nombre = reader.GetString(1);
                    dato.telefono = reader.GetString(2);
                    dato.correo = reader.GetString(3);
                    lista.Add(dato);
                    j++;
                }
                conexion.Close();
            }
            return lista;
        }


        public proveedor consultaDatosDeProveedor(string id)
        {

            proveedor proveedor = new proveedor();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "SELECT ISNULL(RFC, '') RFC, ISNULL(nombre, '') Nombre, ISNULL(representante, '') Representante, " +
                "ISNULL(codigopostal, '') CodPostal, ISNULL(estado, '') Estado_Recidencia, ISNULL(municipio, '') Municipio, " +
                "ISNULL(colonia, '') Colonia, ISNULL(calle, '') Calle, ISNULL(num_ext, '') Num_Ext, ISNULL(num_int, '') Num_Int, " +
                "ISNULL(telefono, '') Telefono, ISNULL(correo, '') Correo FROM proveedor WHERE RFC = '{0}'", id), conexion);
                SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    proveedor.RFC = reader.GetString(0);
                    proveedor.nombre = reader.GetString(1);
                    proveedor.representante = reader.GetString(2);
                    proveedor.codigopostal = reader.GetString(3);
                    proveedor.estado = reader.GetString(4);
                    proveedor.municipio = reader.GetString(5);
                    proveedor.colonia = reader.GetString(6);
                    proveedor.calle = reader.GetString(7);
                    proveedor.num_ext = reader.GetString(8);
                    proveedor.num_int = reader.GetString(9);
                    proveedor.telefono = reader.GetString(10);
                    proveedor.correo = reader.GetString(11);
                }
                conexion.Close();
            }
            return proveedor;
        }


        public proveedor registraProveedor(string id)
        {

            proveedor proveedor = new proveedor();

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "SELECT ISNULL(RFC, '') RFC, ISNULL(nombre, '') Nombre, ISNULL(representante, '') Representante, " +
                "ISNULL(codigopostal, '') CodPostal, ISNULL(estado, '') Estado_Recidencia, ISNULL(municipio, '') Municipio, " +
                "ISNULL(colonia, '') Colonia, ISNULL(calle, '') Calle, ISNULL(num_ext, '') Num_Ext, ISNULL(num_int, '') Num_Int, " +
                "ISNULL(telefono, '') Telefono, ISNULL(correo, '') Correo FROM proveedor WHERE RFC = '{0}'", id), conexion);
                SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    proveedor.RFC = reader.GetString(0);
                    proveedor.nombre = reader.GetString(1);
                    proveedor.representante = reader.GetString(2);
                    proveedor.codigopostal = reader.GetString(3);
                    proveedor.estado = reader.GetString(4);
                    proveedor.municipio = reader.GetString(5);
                    proveedor.colonia = reader.GetString(6);
                    proveedor.calle = reader.GetString(7);
                    proveedor.num_ext = reader.GetString(8);
                    proveedor.num_int = reader.GetString(9);
                    proveedor.telefono = reader.GetString(10);
                    proveedor.correo = reader.GetString(11);

                }
                conexion.Close();
            }
            return proveedor;
        }


        public void actualizaDatosProveedor(proveedor proveedor)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "UPDATE proveedor SET RFC='" + proveedor.RFC + "', nombre='" + proveedor.RFC + "', representante='" + proveedor.representante +
                    "',codigopostal='" + proveedor.codigopostal + "', estado = '" + proveedor.estado + "', municipio = '" + proveedor.municipio +
                    "', colonia = '" + proveedor.colonia + "', calle = '" + proveedor.calle + "',num_ext = '" + proveedor.num_ext +
                    "', num_int = '" + proveedor.num_int + " ',telefono = '" + proveedor.telefono + "', " + "correo = '" + proveedor.correo +
                    "' Where RFC = '" + proveedor.RFC + "'";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();

            }
        }

        public List<Models.usuario> consultaUsuariosRegistrados()
        {
            List<Models.usuario> lista = new List<Models.usuario>();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "SELECT ISNULL(P.CURP, '') CURP, ISNULL(P.nombre, '') Nombre, ISNULL(P.apellido_pat, '') Apellido_Paterno, "+
                "ISNULL(P.apellido_mat, '') Apellido_Materno, ISNULL(P.correo, '') Correo, ISNULL(U.pertenece, '') Pertenece, " +
                "ISNULL(U.Tipo, '') TIPO " +
                "FROM persona P LEFT JOIN usuario U ON P.CURP = U.CURP " +
                "WHERE U.tipo = 'P' OR U.tipo = 'I' OR U.tipo = 'T'"), conexion);
                SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    Models.usuario dato = new Models.usuario();
                    dato.CURP = reader.GetString(0);
                    dato.nombre = reader.GetString(1);
                    dato.apellido_pat = reader.GetString(2);
                    dato.apellido_mat = reader.GetString(3);
                    dato.correo = reader.GetString(4);
                    dato.pertenece = reader.GetString(5);
                    dato.tipo = reader.GetString(6);

                    if (dato.tipo.Equals("P"))
                    {
                        using (SqlConnection conec = Conexion.conexion())
                        {
                            SqlCommand comando2 = new SqlCommand(string.Format("select nombre from proveedor where RFC = '" + dato.pertenece + "'"), conec);
                            SqlDataReader read = comando2.ExecuteReader();
                            while (read.Read())
                            {
                                dato.calle = read.GetString(0);
                            }
                            read.Close();
                            conec.Close();
                        }
                    }
                    else if (dato.tipo.Equals("I") || dato.tipo.Equals("T"))
                    {
                        using (SqlConnection conex = Conexion.conexion())
                        {
                            SqlCommand cmd = new SqlCommand(string.Format("select nombre from institucion where CCT = '" + dato.pertenece + "'"), conex);
                            SqlDataReader reading = cmd.ExecuteReader();
                            while (reading.Read())
                            {
                                dato.calle = reading.GetString(0);
                            }
                            reading.Close();
                            conex.Close();
                        }
                    }
                    lista.Add(dato);
                }
                conexion.Close();
            }
            return lista;
        }


        public void actualizaDatosUsuario(usuario usuario) {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "UPDATE persona set CURP='" + usuario.CURP + "', nombre='" + usuario.nombre + "', apellido_pat='" + usuario.apellido_pat + "', apellido_mat='" + usuario.apellido_mat +
                "', codigopostal='" + usuario.codigopostal + "',estado_r='" + usuario.estado_r + "', municipio='" + usuario.municipio +
                "', colonia='" + usuario.colonia + "'," + "calle = '" + usuario.calle + "', num_int = '" + usuario.num_int + "', num_ext = '" + usuario.num_ext + "', telefono = '" + usuario.telefono + "', celular = '" + usuario.celular + "', correo = '" + usuario.correo + "' WHERE CURP = '" + usuario.CURP + "'";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
            }



        }






    }//Fin Clase
}