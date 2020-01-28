using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication8.Models
{
    public class Tableta
    {
        public Nullable<int> No { get; set; }
        public string serie { get; set; }
        public string nombre { get; set; }
        public string estado { get; set; }
        public string institucion { get; set; }
        public string nombre_institucion { get; set; }
        public string institucion_planteles { get; set; }
        public string nombre_institucion_planteles { get; set; }

    }
}