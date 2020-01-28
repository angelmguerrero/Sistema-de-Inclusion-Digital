using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Educafin.Models
{
    public class EntregaInstitucion
    {
        public Nullable<int> No { get; set; }
        public string nombre { get; set; }
        public Nullable<int> cantidad { get; set; }
        public Nullable<System.DateTime> fecha { get; set; }
        public string responsable { get; set; }
    }
}