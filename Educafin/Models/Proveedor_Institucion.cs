namespace Educafin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Proveedor_Institucion
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string cct_int { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string rfc_prov { get; set; }

        public int? cantidad { get; set; }

        public virtual institucion institucion { get; set; }

        public virtual proveedor proveedor { get; set; }
        public Nullable<int> no { get; set; }
        public string nombre { get; set; }
    }
}
