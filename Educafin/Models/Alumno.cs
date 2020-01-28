using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Educafin.Models
{
    public class Alumno
    {
        public Nullable<int> no { get; set; }
        public string curp { get; set; }
        public string matricula { get; set; }
        public string nombre { get; set; }
        public string status { get; set; }
        public string alerta { get; set; }

        public string cct { get; set; }
        public string cctNombre { get; set; }
        public string apePat { get; set; }
        public string apeMat { get; set; }
        public string nombreTemp { get; set; }
        public string fecha { get; set; }
        public string correo { get; set; }
        public string folio { get; set; }
    }
}