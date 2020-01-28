using MvcApplication8.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Educafin.Clases.Transacciones
{
    public class ProveedorTransacciones
    {
        string dependencia = "", RFC = "", institucion = "", nombre = "", estatus = "", serie = "", CCTMedioP = "", grado = "";
        int cantidad = 0, cantidadProveedor = 0, total = 0, clave = 0, claveEntrega = 0;
        DateTime fecha;
        bool medioProveedor = false;

        public void setMedioProveedor(bool medioProveedor) { this.medioProveedor = medioProveedor; }
        public bool getMedioProveedor() { return this.medioProveedor; }

        public void setDependencia(String dependencia) { this.dependencia = dependencia; }
        public string getDependencia() { return this.dependencia; }
        public void setRFC(String RFC) { this.RFC = RFC; }
        public string getRFC() { return this.RFC; }
        public void setCantidadProveedor(int cantidadProveedor) { this.cantidadProveedor = cantidadProveedor; }
        public int getCantidadProveedor() { return this.cantidadProveedor; }
        public void setCantidad(int cantidad) { this.cantidad = cantidad; }
        public int getCantidad() { return this.cantidad; }
        public void setTotal(int total) { this.total = total; }
        public int getTotal() { return this.total; }
        public void setInstitucion(String institucion) { this.institucion = institucion; }
        public string getInstitucion() { return this.institucion; }
        public void setFecha(DateTime fecha) { this.fecha = fecha; }
        public DateTime getFecha() { return this.fecha; }
        public void setNombre(String nombre) { this.nombre = nombre; }
        public string getNombre() { return this.nombre; }
        public void setEstatus(String estatus) { this.estatus = estatus; }
        public string getEstatus() { return this.estatus; }
        public void setSerie(String serie) { this.serie = serie; }
        public string getSerie() { return this.serie; }
        public void setCCTMedioP(String CCTMedioP) { this.CCTMedioP = CCTMedioP; }
        public string getCCTMedioP() { return this.CCTMedioP; }

        public void setClaveEntrega(int clave) { this.clave = clave; }
        public int getClaveEntrega() { return this.clave; }

        public void setGrado(String grado) { this.grado = grado; }
        public string getGrado() { return this.grado; }



        public void consultaDependencia(string CCT)
        {
            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select [medioprovedor],[dependencia] from institucion where CCT like '{0}' ", CCT), conex);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setMedioProveedor(reader.GetBoolean(0));
                    if (reader.GetValue(1) != null)
                    {
                        setDependencia(reader.GetValue(1).ToString());
                    }
                }
                conex.Close();
            }
        }

        public string regresaConsulta()
        {
            // string consulta = "select nombre,fecha,cantidad,responsable from entregainstitucion, proveedor where institucion like '{0}' AND RFC like proveedor";
            string consulta = "select B.nombre, ISNULL(C.fecha_MP, '') Fecha_MP, ISNULL(A.Cantidad_Destino, '') Cantidad_Recibida,  A.responsable " +
                              "from entrega AS A, proveedor AS B, entrega_Proveedor as C " +
                              "where A.clave = C.clave AND C.CCT like '{0}' AND B.RFC like C.RFC AND C.RFC = B.RFC " +
                              " AND C.fecha_MP IS NOT NULL";
            return consulta;
        }

        public string regresaConsulta2()
        {
            // string consulta = "select nombre,fecha,cantidad,responsable from entregainstitucion, institucion where institucion like '{0}' AND cct like proveedor";
            string consulta = "select B.nombre, ISNULL(C.fecha_Inst, '') Fecha_Inst, ISNULL(A.Cantidad_Destino, '') Cantidad_Recibida,  A.responsable " +
                              "from entrega AS A, institucion AS B, entrega_MP as C " +
                              "where A.clave = C.clave AND C.CCT_MP like '{0}' AND B.CCT like C.CCT_MP " +
                              "AND C.fecha_Inst IS NOT NULL";
            return consulta;
        }




        public List<Educafin.Models.EntregaInstitucion> consultaEntregaInstitucion(string consulta, string CCT)
        {
            List<Educafin.Models.EntregaInstitucion> lista = new List<Educafin.Models.EntregaInstitucion>();

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(consulta, CCT), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                int j = 1;
                while (reader.Read())
                {
                    Educafin.Models.EntregaInstitucion dato = new Educafin.Models.EntregaInstitucion();
                    dato.No = j;
                    dato.nombre = reader.GetString(0);
                    dato.fecha = reader.GetDateTime(1);
                    dato.cantidad = reader.GetInt32(2);
                    dato.responsable = reader.GetString(3);
                    lista.Add(dato);
                    j++;
                }
                conexion.Close();
            }
            return lista;
        }

        public List<Educafin.Models.EntregaInstitucion> consultaEntregaInstitucion2(string consulta, string consulta2, string CCT, string dependencia)
        {
            List<Educafin.Models.EntregaInstitucion> lista = new List<Educafin.Models.EntregaInstitucion>();

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(consulta, CCT), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                int j = 1;
                while (reader.Read())
                {
                    Educafin.Models.EntregaInstitucion dato = new Educafin.Models.EntregaInstitucion();
                    dato.No = j;
                    dato.nombre = reader.GetString(0);
                    dato.fecha = reader.GetDateTime(1);
                    dato.cantidad = reader.GetInt32(2);
                    dato.responsable = reader.GetString(3);
                    lista.Add(dato);
                    j++;
                }
                conexion.Close();
            }


            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(consulta2, dependencia), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                int j = 1;
                while (reader.Read())
                {
                    Educafin.Models.EntregaInstitucion dato = new Educafin.Models.EntregaInstitucion();
                    dato.No = j;
                    dato.nombre = reader.GetString(0);
                    dato.fecha = reader.GetDateTime(1);
                    dato.cantidad = reader.GetInt32(2);
                    dato.responsable = reader.GetString(3);
                    lista.Add(dato);
                    j++;
                }
                conexion.Close();
            }
            return lista;
        }

        public void consultaProveedor(string proveedor)
        {
            using (SqlConnection cone = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select rfc from proveedor where nombre like '{0}'", proveedor), cone);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setRFC(reader.GetString(0));
                }
                cone.Close();
            }
        }

        public void consultaCatidadEntrega(string clave)
        {

            using (SqlConnection cone = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select cantidadproveedor from entrega where clave like '{0}'", clave), cone);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setCantidadProveedor(reader.GetInt32(0));
                }
                cone.Close();
            }
        }

        public void registraEntrega(string fecha, int cantidad, string clave)
        {

            using (SqlConnection conexion = Conexion.conexion())
            {
                string query = "update entrega set  fecha_institucion='" + fecha + "',cantidadinstitucion=" + cantidad + ",estatusinstitucion ='Entregado' where clave like " + clave;
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
        }

        public List<Tableta> consultaTabletasNombres(string CCT)
        {
            List<Tableta> listatablet = new List<Tableta>();

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                " select serie,nombre from tablet T ,institucion I where T.medioprovedor like '{0}' and CCT like T.medioprovedor and beneficiario is null", CCT), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                int j = 1;
                while (reader.Read())
                {
                    Tableta dato = new Tableta();
                    dato.No = j;
                    dato.serie = reader.GetString(0);
                    dato.nombre = reader.GetString(1);
                    dato.estado = "registrado";
                    listatablet.Add(dato);
                    j++;
                }
                conexion.Close();
            }
            return listatablet;
        }

        public List<Tableta> consultaTabletaSinBeneficiario(string CCT)
        {
            List<Tableta> listatablet = new List<Tableta>();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select serie,nombre from tablet T ,institucion I where proveedor like '{0}' and beneficiario is null and ( CCT like T.medioprovedor or CCT = T.intitucion) and estatus = 'P'", CCT), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                int j = 1;
                while (reader.Read())
                {
                    Tableta dato = new Tableta();
                    dato.No = j;
                    dato.serie = reader.GetString(0);
                    dato.nombre = reader.GetString(1);
                    dato.estado = "registrado";
                    listatablet.Add(dato);
                    j++;
                }
                conexion.Close();
            }
            return listatablet;
        }

        public void eliminaTablet(string serieTablet)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "delete from tablet where serie like '" + serieTablet + "' and estatus ='P'";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }

        public void eliminaTabletEstatusPendiente(string CCT, string proveedor)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "delete from tablet where beneficiario is null and (intitucion like '" + CCT + "' or medioprovedor like '" + CCT + "') and proveedor = '" + proveedor + "' and estatus = 'P'";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }

        public void consultaFechaEntregas(string id_entrega)
        {

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select institucion, cantidadproveedor, fecha_proveedor from entrega where clave like '{0}'", id_entrega), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setInstitucion(reader.GetString(0));
                    setCantidadProveedor(reader.GetInt32(1));
                    setFecha(reader.GetDateTime(2));
                }
                conexion.Close();
            }
        }


        public void consultarEntregasProvInst(string RFC, string CCT)
        {

            using (SqlConnection cone = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select cantidad from Proveedor_Institucion where rfc_prov like '" + RFC + "' and cct_int like '" + CCT + "'"), cone);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setCantidad(reader.GetInt32(0));
                }
                cone.Close();
            }
        }

        public void consultaCantidadTab(string CCT, string RFC, int clave)
        {
            int cantidadpro = 0;
            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select cantidadproveedor from entrega where  institucion='" + CCT + "' and proveedor like '" + RFC + "'and clave != " + clave + ""), conex);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    cantidadpro += reader.GetInt32(0);
                }
                conex.Close();
            }
            setCantidadProveedor(cantidadpro);
        }

        public void actualizaTablet(string fecha, int cantidad, string CCT, int clave)
        {

            using (SqlConnection conexion = Conexion.conexion())
            {
                string query = "update entrega set  fecha_proveedor='" + fecha + "',cantidadproveedor=" + cantidad + ",institucion ='" + CCT + "' where clave like " + clave;
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
        }

        public List<Entregas> consultaTabletasEntregadas(string RFC)
        {
            List<Entregas> lista = new List<Entregas>();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                //"select nombre,cantidadproveedor,fecha_proveedor,estatusproveedor,responsable,clave from entrega ,institucion where proveedor like '{0}' AND CCT like institucion"
                "select B.nombre, A.cantidad_Origen, C.fecha_proveedor, A.responsable, A.clave " +
                "from entrega A " +
                "LEFT JOIN " +
                "entrega_Proveedor C ON A.clave = C.clave " +
                "LEFT JOIN " +
                "institucion B ON B.CCT = C.CCT " +
                "where C.RFC like '{0}'", RFC), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                int j = 1;
                while (reader.Read())
                {
                    Entregas dato = new Entregas();
                    dato.No = j;
                    dato.nombre = reader.GetString(0);
                    dato.cantidad = reader.GetInt32(1);
                    dato.fecha_hora = reader.GetDateTime(2);
                    dato.estatus = "Registrado";
                    dato.responsable = reader.GetString(3);
                    dato.clave = reader.GetInt32(4);
                    lista.Add(dato);
                    j++;
                }
                conexion.Close();
            }
            return lista;
        }

        public void consultaClaveInst(string RFC, string CCT)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select cct_int from Proveedor_Institucion where cct_int like '{0}' and rfc_prov like '" + RFC + "'", CCT), conex);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setInstitucion(reader.GetString(0));
                }
                conex.Close();
            }
        }

        public void consultaClaveInstConNombre(string nombre)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select CCT from institucion where nombre ='" + nombre + "'"), conex);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setInstitucion(reader.GetString(0));
                }
                conex.Close();
            }
        }

        public void consultaTotalTablets()
        {

            using (SqlConnection cone = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select COUNT(*)CLAVE from entrega"), cone);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setCantidad(reader.GetInt32(0));
                }
                cone.Close();
            }
        }

        public void consultaCantidadProveedor(string RFC, string CCT)
        {

            using (SqlConnection cone = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select cantidad from Proveedor_Institucion where rfc_prov like '" + RFC + "' and cct_int like '" + CCT + "'"), cone);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setCantidadProveedor(reader.GetInt32(0));
                }
                cone.Close();
            }
        }

        public void consultaCantProvInst(string CCT, string RFC)
        {
            int cantidpro = 0;
            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select cantidadproveedor from entrega where institucion='" + CCT + "' and proveedor like '" + RFC + "'"), conex);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    cantidpro += reader.GetInt32(0);
                }
                conex.Close();
            }
            setTotal(cantidpro);
        }

        public void realizaEntrega(int CLAVE, string RFC, string CCT, DateTime fecha, int Cantidad, string responsable)
        {

            int claveEntrega = 0;

            // var date = Convert.ToDateTime(fecha);

            DateTime dt = Convert.ToDateTime(fecha);
            string date = Convert.ToDateTime(dt).ToString("yyyyMMdd");




            using (SqlConnection cone = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select COUNT(*)Entregas FROM entrega"), cone);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    claveEntrega = reader.GetInt32(0);
                }
                cone.Close();

            }

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand
                ( //"insert INTO entrega values (" + (CLAVE + 1) + ",'" + RFC + "','" + CCT + "','" + fecha.ToString("yyyy-MM-dd HH:mm ") + "',null ," + Cantidad + ",'Registrado',0,'Sin Registro','" + responsable + "')"
                "Insert into entrega(clave, cantidad_Origen, Cantidad_Destino, fecha_Registro, responsable) " +
                "VALUES(" + (claveEntrega + 1) + ", " + Cantidad + ", NULL, '" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ") + "', '" + responsable + "')"
                , conexion);
                comando.ExecuteNonQuery();
                conexion.Close();
            }

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand
                (
                "insert into entrega_Proveedor(clave, RFC, CCT, fecha_Proveedor, fecha_MP) " +
                "VALUES(" + (claveEntrega + 1) + ",'" + RFC + "', '" + CCT + "','" + date + "', NULL )"
                , conexion);
                comando.ExecuteNonQuery();
                conexion.Close();
            }
        }


        public void consultaInstitucion(string CCT)
        {
            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select CCT,nombre from institucion where CCT like '{0}' ", CCT), conex);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setInstitucion(reader.GetString(0));
                    setNombre(reader.GetString(1));
                }
                conex.Close();
            }
        }



        public List<Tableta> consultaErrores(string RFC)
        {
            List<Tableta> listatablet = new List<Tableta>();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select serie,intitucion,mensaje from tabletaerro where proveedor like '{0}'", RFC), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                int j = 1;
                while (reader.Read())
                {
                    Tableta dato = new Tableta();
                    dato.No = j;
                    dato.serie = reader.GetString(0);
                    dato.nombre = reader.GetString(1);
                    dato.estado = reader.GetString(2);
                    listatablet.Add(dato);
                    j++;
                }
                reader.Close();
                conexion.Close();
            }
          
            return listatablet;
        }

            public void eliminaErroresTabletas(string RFC) {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "delete from tabletaerro where proveedor like '" + RFC + "'";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
    }



        public List<Tableta> consultaSeiesConNombre(string RFC)
        {

            List<Tableta> listatablet = new List<Tableta>();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select serie,nombre from tablet T ,institucion I where T.medioprovedor like '{0}' and CCT like T.medioprovedor and beneficiario is null and estatus ='M'", RFC), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                int j = 1;
                while (reader.Read())
                {
                    Tableta dato = new Tableta();
                    dato.No = j;
                    dato.serie = reader.GetString(0);
                    dato.nombre = reader.GetString(1);
                    dato.estado = "registrado";
                    listatablet.Add(dato);
                    j++;
                }
                conexion.Close();
            }
            return listatablet;
        }


        public List<Tableta> tabletasEntregadasAMP(string CCT)
        {

            List<Tableta> listatablet = new List<Tableta>();
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
                    // "select serie,nombre from [tablet] T ,institucion I where t.medioprovedor like '{0}' and CCT like t.intitucion and beneficiario is null and estatus = 'M'"
                    "SELECT T.SERIE, I.nombre FROM tablet T " +
                    "LEFT JOIN institucion I ON T.intitucion = I.CCT OR T.medioprovedor = I.CCT " +
                    "WHERE T.medioprovedor = '{0}' " +
                    "AND T.beneficiario IS NULL AND T.estatus = 'M'"
                    , CCT), conexion);
                    SqlDataReader reader = comando.ExecuteReader();
                    int j = 1;
                    while (reader.Read())
                    {
                        Tableta dato = new Tableta();
                        dato.No = j;
                        dato.serie = reader.GetString(0);
                        dato.nombre = reader.GetString(1);
                        dato.estado = "Recibida";
                        listatablet.Add(dato);
                        j++;
                    }
                    conexion.Close();
                }
            }
            else
            {

                using (SqlConnection conexion = Conexion.conexion())
                {
                    SqlCommand comando = new SqlCommand(string.Format(
                    // "select serie,nombre from [tablet] T ,institucion I where t.medioprovedor like '{0}' and CCT like t.intitucion and beneficiario is null and estatus = 'M'"
                    "SELECT T.SERIE, I.nombre FROM tablet T " +
                    "LEFT JOIN institucion I ON T.intitucion = I.CCT OR T.medioprovedor = I.CCT " +
                    "WHERE T.intitucion = '{0}' " +
                    "AND T.beneficiario IS NULL AND T.estatus = 'I'"
                    , CCT), conexion);
                    SqlDataReader reader = comando.ExecuteReader();
                    int j = 1;
                    while (reader.Read())
                    {
                        Tableta dato = new Tableta();
                        dato.No = j;
                        dato.serie = reader.GetString(0);
                        dato.nombre = reader.GetString(1);
                        dato.estado = "Recibida";
                        listatablet.Add(dato);
                        j++;
                    }
                    conexion.Close();
                }
            }
            return listatablet;
        }

        public void bajaTabletasMP(string no_serie)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "UPDATE tablet SET grado=NULL,estatus='E' WHERE serie='" + no_serie + "' and estatus ='M' and beneficiario is null";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }

        public void eliminaInstitucionMP(string CCT, string RFC)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "UPDATE tablet SET grado=NULL,estatus='E' WHERE medioprovedor='" + CCT + "' AND proveedor='" + RFC + "' AND beneficiario IS NULL AND estatus='M'";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }

        public List<Entregas> consultaTabletsEntMP(string MP)
        {

            List<Entregas> lista = new List<Entregas>();
            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select B.nombre, C.cantidad_Origen, A.fecha_MP, C.responsable, C.clave " +
                "FROM entrega C " +
                "LEFT JOIN entrega_MP A ON A.clave = C.clave " +
                "LEFT JOIN institucion B ON A.CCT_MP = B.CCT " +
                "where A.CCT_MP = '{0}' AND A.fecha_Inst IS NULL", MP), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                int j = 1;
                while (reader.Read())
                {
                    Entregas dato = new Entregas();
                    dato.No = j;
                    dato.nombre = reader.GetString(0);
                    dato.cantidad = reader.GetInt32(1);
                    dato.fecha_hora = reader.GetDateTime(2);
                    dato.estatus = "Registrado";
                    dato.responsable = reader.GetString(3);
                    dato.clave = reader.GetInt32(4);
                    lista.Add(dato);
                    j++;
                }
                conexion.Close();
            }
            return lista;
        }


        public void consultaDependenciaInst(string CCT)
        {
     
            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select dependencia from institucion where cct like '{0}'", CCT), conex);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setDependencia(reader.GetString(0));
                }
                conex.Close();
            }
        }


        public void consultaTotalTabletsMP()
        {

            using (SqlConnection cone = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select COUNT(*)CLAVE from entrega_MP"), cone);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setCantidad(reader.GetInt32(0));
                }
                cone.Close();
            }
        }

        public void consultaTabletasMPNoEntreg(string RFC)
        {

            using (SqlConnection cone = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select COUNT(*)tableta from tablet where medioprovedor = '" + RFC + "' and beneficiario is null"), cone);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setTotal(reader.GetInt32(0));
                }
                cone.Close();
            }
        }

        public void consultaTabAEntregarMP(string RFC)
        {
            int cantidpro = 0;
            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select cantidadproveedor from entregaMP where medioproveedor like '" + RFC + "'"), conex);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    cantidpro += reader.GetInt32(0);
                }
                conex.Close();
            }
            setCantidadProveedor(cantidpro);
        }


        public void registraEntregaMP(string MP, string CCT, DateTime fecha, int cantidad, string responsable)
        {

            DateTime dt = Convert.ToDateTime(fecha);
            string date = Convert.ToDateTime(dt).ToString("yyyy-MM-dd");

            int claveEntrega = 0;
            using (SqlConnection cone = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select COUNT(*)Entregas FROM entrega"), cone);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    claveEntrega = reader.GetInt32(0);
                }
                cone.Close();
            }

            if (claveEntrega <= 0 || claveEntrega.Equals(null))
            {
                claveEntrega = 1;
            }
            else
            {
                string hora = "T00:00:00";
                using (SqlConnection conexion = Conexion.conexion())
                {
                    // string datetime = DateTime.Now.ToString("yyyy/MM/dd");


                    System.Diagnostics.Debug.WriteLine("insert INTO entrega(clave, cantidad_Origen, cantidad_Destino, fecha_Registro, responsable) values(" + (claveEntrega + 1) + ",'" + cantidad + "', NULL ,'" + DateTime.Now.ToString("yyyy-MM-dd") + hora + "','" + responsable + "')");
                    SqlCommand comando = new SqlCommand
                     (
                     "insert INTO entrega(clave, cantidad_Origen, cantidad_Destino, fecha_Registro, responsable) values(" + (claveEntrega + 1) + ",'" + cantidad + "', NULL ,'" + DateTime.Now.ToString("yyyy-MM-dd") + hora + "','" + responsable + "')", conexion);
                    comando.ExecuteNonQuery();
                    conexion.Close();
                }


                using (SqlConnection conexion = Conexion.conexion())
                {
                    System.Diagnostics.Debug.WriteLine("insert INTO entrega_MP(clave, CCT_MP, CCT_Inst, fecha_MP, fecha_Inst) values(" + (claveEntrega + 1) + ",'" + MP + "','" + CCT + "','" + date + hora + "', NULL )");
                    SqlCommand comando = new SqlCommand
                     (
                        //"insert INTO entregaMP values (" + (CLAVE + 1) + ",'" + RFC + "','" + CCT + "','" + fecha.ToString("yyyy-MM-dd HH:mm") + "',null ," + Cantidad + ",'Registrado',0,'Sin Registro','" + responsable + "')", conexion);
                        "insert INTO entrega_MP(clave, CCT_MP, CCT_Inst, fecha_MP, fecha_Inst) values(" + (claveEntrega + 1) + ",'" + MP + "','" + CCT + "','" + date + hora + "', NULL )", conexion);
                    comando.ExecuteNonQuery();

                    conexion.Close();
                }
            }
        }

        public void consultaFechaParaEntrMP(string clave)
        {

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select A.CCT, B.cantidad_Origen, A.fecha_proveedor " +
                "from entrega_Proveedor AS A, ENTREGA AS B " +
                "where A.clave = B.clave AND B.clave = '{0}'", clave), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setInstitucion(reader.GetString(0));
                    setCantidad(reader.GetInt32(1));
                    setFecha(reader.GetDateTime(2));
                }
                conexion.Close();
            }
        }


        public void consultaCantProvYEntregaMP(string RFC, int clave)
        {
            int cantidpro = 0;

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select cantidadproveedor from entregaMP where proveedor like '" + RFC + "'and clave != " + clave + ""), conex);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    cantidpro += reader.GetInt32(0);
                }
                conex.Close();
            }
            setCantidadProveedor(cantidpro);
        }

        public void actualizaEntregaInstYMP(string fechadata, int Cantidad, string CCT, int clave)
        {

            using (SqlConnection conexion = Conexion.conexion())
            {
                // string query = "update entregaMP set  fecha_proveedor='" + fechadata + "',cantidadproveedor=" + Cantidad + ",institucion ='" + CCT + "' where clave like " + clave;
                string query = "update entrega_Proveedor set fecha_proveedor = '" + fechadata + "', CCT = '" + CCT + "' where clave = " + clave + "";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.ExecuteNonQuery();
                conexion.Close();
            }


            using (SqlConnection conexion = Conexion.conexion())
            {
                string query = "UPDATE entrega SET cantidad_Origen='" + Cantidad + "' WHERE clave =" + clave + "";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.ExecuteNonQuery();
                conexion.Close();
            }

        }

        public void consultaRFCdelProv(string RFC)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select rfc_prov from Proveedor_Institucion where rfc_prov like '{0}' ", RFC), conex);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setRFC(reader.GetString(0));
                }
                conex.Close();
            }
        }

        public void consultaEstatusTablet(string CCT, string RFC)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select distinct estatus from tablet where medioprovedor = '" + CCT + "' and (proveedor = '{0}' or intitucion = '{0}' )and  intitucion is not null", RFC), conex);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setEstatus(reader.GetString(0));
                }
                conex.Close();
            }
        }

        public void registraEntregaInst(string CCT, string RFC, DateTime fecha, int Cantidad, string responsable)
        {

            //using (SqlConnection conexion = Conexion.conexion())
            //{
            //    SqlCommand comando = new SqlCommand(
            //        "insert INTO entregainstitucion values ('" + CCT + "','" + RFC + "','" + fecha.ToString("yyyy-MM-dd HH:mm") + "'," + Cantidad + ",'" + responsable + "')"
            //        //"Insert into entrega(cantidad_Origen, Cantidad_Destino, fecha_Registro, responsable) " +
            //        // "VALUES(1500, 1200, '2017/02/02 00:00:00', 'Juan Lopez')"
            //        , conexion);
            //    comando.ExecuteNonQuery();
            //    conexion.Close();
            //}

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(
                    "insert INTO entregainstitucion values ('" + CCT + "','" + RFC + "','" + fecha.ToString("yyyy-MM-dd HH:mm") + "'," + Cantidad + ",'" + responsable + "')"
                    //"Insert into entrega_MP(CCT_MP, CCT_Inst, fecha_MP, fecha_Inst) " +
                    //"Values('TEC-000', 'TEC-001', '2017/02/02 00:00:00', '2017/02/10 00:00:00')"
                    , conexion);
                comando.ExecuteNonQuery();
                conexion.Close();
            }


        }

        public void consultaEstatusMedioProv(string RFC, string CCT)
        {
            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select distinct estatus from tablet where medioprovedor = '" + RFC + "' and intitucion = '{0}' and  intitucion is not null", CCT), conex);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setEstatus(reader.GetString(0));
                }
                conex.Close();
            }
        }


        public void consultaEntregaRecibidaInst(string RFC, string CCT)
        {
            int count = 0;
            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "Select clave from entrega_MP where CCT_MP='" + RFC + "' AND CCT_Inst='" + CCT + "' AND fecha_Inst IS NULL"), conex);
                SqlDataReader reader = comando.ExecuteReader();


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        count++;
                        setClaveEntrega(reader.GetInt32(0));
                        System.Diagnostics.Debug.WriteLine(reader.GetInt32(0));
                    }
                }
                else
                {
                    setClaveEntrega(0);
                }

                conex.Close();
            }


        }

        public void consultaEntregaRecibidaProveedor(string RFC, string CCT)
        {
            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "Select clave from entrega_Proveedor where RFC='" + RFC + "' AND CCT='" + CCT + "' AND fecha_MP IS NULL"), conex);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setClaveEntrega(reader.GetInt32(0));
                }
                conex.Close();
            }
        }

        public void registraEntregaProveedor(DateTime fecha_MP, int clave)
        {

            DateTime dt = Convert.ToDateTime(fecha_MP);
            string date = Convert.ToDateTime(dt).ToString("yyyyMMdd");

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "UPDATE entrega_Proveedor set fecha_MP='" + date + "' WHERE clave=" + clave;
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }


        public void registraEntregaMP(DateTime fecha_Inst, int clave)
        {
            DateTime dt = Convert.ToDateTime(fecha_Inst);
            string date = Convert.ToDateTime(dt).ToString("yyyyMMdd");

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "UPDATE entrega_MP set fecha_Inst='" + date + "' WHERE clave=" + clave + "";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }

            System.Diagnostics.Debug.WriteLine("UPDATE entrega_MP set fecha_Inst='" + date + "' WHERE clave=" + clave + "");
        }

        public void actualizaFechaEntregaRecibida(int Cantidad, int clave)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "UPDATE entrega set Cantidad_Destino=" + Cantidad + " WHERE clave=" + clave + "";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }

            System.Diagnostics.Debug.WriteLine("UPDATE entrega set Cantidad_Destino=" + Cantidad + " WHERE clave=" + clave + "");

        }


        public string retornaConsultaInst(string estatus, string RFC, string CCT)
        {
            string consulta = "UPDATE tablet SET ESTATUS ='" + estatus + "' WHERE proveedor = '" + RFC + "' AND intitucion ='" + CCT + "'"; ;
            return consulta;
        }

        public string retornaConsultaMP(string estatus, string RFC, string CCT)
        {
            string consulta = "UPDATE tablet SET ESTATUS ='" + estatus + "' WHERE medioprovedor = '" + RFC + "' AND intitucion ='" + CCT + "'";
            return consulta;
        }

        public void realizaRegistro(string query)
        {
            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }

        public void retornaConsultaMP2(string estatus, string RFC, string CCT)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "UPDATE tablet SET ESTATUS ='" + estatus + "' WHERE proveedor = '" + RFC + "' AND medioprovedor ='" + CCT + "'";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }

        public List<MvcApplication8.Models.Institucion> autoCompletarEntrega(string CCT)
        {

            List<MvcApplication8.Models.Institucion> lis = new List<MvcApplication8.Models.Institucion>();
            using (SqlConnection cone = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select i.nombre,p.cct_int from Proveedor_Institucion p, institucion i where cct_int like '{0}%' and cct_int = CCT", CCT), cone);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    MvcApplication8.Models.Institucion mde = new MvcApplication8.Models.Institucion();
                    mde.nombre = reader.GetString(0);
                    mde.CCT = reader.GetString(1);
                    mde.direccion = reader.GetString(0);
                    lis.Add(mde);
                }
                cone.Close();
            }
            return lis;
        }

        public void consultaProvInst(string CCT)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select cct_int from Proveedor_Institucion where cct_int like '{0}' ", CCT), conex);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setInstitucion(reader.GetString(0));
                }
                conex.Close();
            }
        }

        public void consultaSerieInstProv(string no_Serie)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select serie, intitucion, medioprovedor from tablet where serie like '{0}' ", no_Serie), conex);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setSerie(reader.GetString(0));
                    if (reader.GetString(1) != "")
                    {
                        setInstitucion(reader.GetString(1));
                    }
                    else
                    {
                        setCCTMedioP(reader.GetString(2));
                    }
                }
                conex.Close();
            }
        }


        public void consultaSerieProv(string no_Serie, string CCT_MP)
        {
            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select serie from tablet where serie='{0}' AND medioprovedor='{1}'", no_Serie, CCT_MP), conex);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setSerie(reader.GetString(0));
                }
                conex.Close();
            }
        }




        public void consultaNombreMP(string CCT)
        {
            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select nombre,medioprovedor from institucion where CCT like '{0}' ", CCT), conex);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setNombre(reader.GetString(0));
                    setMedioProveedor(reader.GetBoolean(1));
                }
                conex.Close();
            }

        }

        public void registraNuevaTablet(string no_Serie, string RFC, string CCT)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into tablet Values('" + no_Serie + "','" + RFC + "','" + CCT + "',NULL,'',NULL,NULL,'P')";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }

        public void registraNuevaTablet2(string no_Serie, string RFC, string CCT)
        {
            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into tablet Values('" + no_Serie + "','" + RFC + "','',NULL,'" + CCT + "',NULL,NULL,'P')";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }


        public void registraErrorTablet(string no_Serie, string RFC, string nomintitucion, string medioprovedor)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into tabletaerro Values('" + no_Serie + "','" + RFC + "','" + nomintitucion + medioprovedor + "','','',NULL,'','El numero de serie no esta asignado a su institución')";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }

        public void registraTabletDuplicada(string no_Serie, string RFC, string nomintitucion)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into tabletaerro Values('" + no_Serie + "','" + RFC + "','" + nomintitucion + "','','',NULL,'','El numero de serie se registro anteriormente')";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }


        public void registraErrorSinAsignacion(string no_Serie, string RFC, string nom, string CCT)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into tabletaerro Values('" + no_Serie + "','" + RFC + "','" + nom + "','','',null,'','La Institucion: " + CCT + " no esta asignada a su RFC')";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }

        public void registraErrorSinAsignacion(string no_Serie, string pRFC, string nombre)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into tabletaerro Values('" + no_Serie + "','" + pRFC + "','" + nombre + "','','',null,null,'Institucion no Asignada')";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }

        public void errorCCTNoExiste(string no_Serie, string RFC, string CCT)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into tabletaerro Values('" + no_Serie + "','" + RFC + "','" + CCT + "','','',null,'','El CCT: " + CCT + "' no existe)";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }

        public void consultaSerieEInstitucion(string no_Serie, string CCT)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select serie, intitucion, medioprovedor from tablet WHERE serie='{0}' AND intitucion='{1}'", no_Serie, CCT), conex);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setSerie(reader.GetString(0));
                    setInstitucion(reader.GetValue(1).ToString());
                    setCCTMedioP(reader.GetString(2));
                }
                conex.Close();
            }
        }


        public void consultaSerieMP(string no_Serie, string CCT_MP)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select serie, medioprovedor, proveedor from tablet where serie='{0}' AND medioprovedor='{1}'", no_Serie, CCT_MP), conex);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setSerie(reader.GetString(0));
                    setCCTMedioP(reader.GetString(1));
                    setRFC(reader.GetString(2));
                }
                conex.Close();
            }
        }


        public void registraErrorTabAsignada(string no_Serie, string RFC, string exitinstitucion, string intitucion)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "Insert into tabletaerro Values('" + no_Serie + "','" + RFC + "','" + exitinstitucion + "','','',NULL,NULL,' La tableta no esta asignada para entrega')";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }

        public void registraTabletaNoEncontrada(string CCT, string CCT_MP, string no_Serie, string grado)
        {

            DateTime fecha_actual = DateTime.Now;

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "UPDATE tablet set grado='" + grado + "', estatus='I', fecha='" + fecha_actual +  "' WHERE medioprovedor='" + CCT_MP + "' AND intitucion='" + CCT + "' AND beneficiario IS NULL";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }


        public void registraInstitucionPorMP(string RFC, string CCT_MP, string CCT, string no_Serie)
        {

            DateTime fecha_actual = DateTime.Now;

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "UPDATE tablet set intitucion='" + CCT + "', fecha='" + fecha_actual + "' WHERE medioprovedor='" + CCT_MP + "' AND proveedor='" + RFC + "' AND beneficiario IS NULL";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
                System.Diagnostics.Debug.WriteLine("UPDATE tablet set intitucion='" + CCT + "', fecha='" + fecha_actual + "' WHERE medioprovedor='" + CCT_MP + "' AND proveedor='" + RFC + "' AND beneficiario IS NULL");
            }

        }

        public void registraTabletaNoEncontradaMP( string RFC, string CCT_MP, string no_Serie, string grado)
        {

            DateTime fecha_actual = DateTime.Now;

            using (SqlConnection conex = Conexion.conexion())
            {
                string query =
                "UPDATE tablet set grado='" + grado + "', estatus='M', fecha='" + fecha_actual + "' WHERE proveedor='" + RFC + "' AND medioprovedor='" + CCT_MP + "' AND beneficiario IS NULL";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
               // System.Diagnostics.Debug.WriteLine("UPDATE tablet set grado='" + grado + "', estatus='M', fecha='" + fecha_actual + "' WHERE proveedor='" + RFC + "' AND medioprovedor='" + CCT_MP + "' AND beneficiario IS NULL");
            }
        }




        public void consultaClaveEntregaProveedor(string RFC, string CCT)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select clave from entrega_Proveedor where RFC='{0}' AND CCT='{1}' AND fecha_MP IS NULL", RFC, CCT), conex);
                SqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows)
                {
                    int count = 0;
                    while (reader.Read())
                    {
                        count++;
                        setClaveEntrega(reader.GetInt32(0));
                    }
                }
                else
                {
                    setClaveEntrega(0);
                }

                conex.Close();
            }
            System.Diagnostics.Debug.WriteLine(getClaveEntrega());
        }


        public void comparaCantidadEntrega(int clave)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "SELECT cantidad_Origen from entrega WHERE clave='{0}'", clave), conex);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setCantidadProveedor(reader.GetInt32(0));
                }
                conex.Close();
            }
            System.Diagnostics.Debug.WriteLine("Cantidad Proveedor" + getCantidadProveedor());
        }

        public void consultaGradoDeInstitucion(string CCT)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select grado from institucion where CCT='{0}'", CCT), conex);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setGrado(reader.GetString(0));
                }
                conex.Close();
            }
        }

        public void actualizaDatosDeEntregaEnTablet(string grado, string RFC, string CCT)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "UPDATE tablet set grado='" + grado + "', estatus='I' WHERE proveedor='" + RFC + "' AND intitucion='" + CCT + "' AND beneficiario IS NULL";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }


        public void actualizaDatosDeEntregaEnTablet2(string grado, string RFC, string CCT)
        {

            using (SqlConnection conex = Conexion.conexion())
            {
                string query = "UPDATE tablet set grado='" + grado + "', estatus='M' WHERE proveedor='" + RFC + "' AND medioprovedor='" + CCT + "' AND beneficiario IS NULL";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.ExecuteNonQuery();
                conex.Close();
            }
        }


        public void consultaMPdeInstitucion(string CCT) {

            using (SqlConnection conexion = Conexion.conexion())
            {
                SqlCommand comando = new SqlCommand(string.Format(
                "select medioprovedor from institucion WHERE CCT = '{0}'", CCT), conexion);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    setMedioProveedor( reader.GetBoolean(0));
                }
                reader.Close();
                conexion.Close();
            }

        }



    }//Fin Clase
}