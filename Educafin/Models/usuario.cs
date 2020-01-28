namespace Educafin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

   // [Table("usuario")]
    public partial class usuario
    {
        [Key]
        [Required]
        [StringLength(18)]
        public string CURP { get; set; }

        [Required]
        [StringLength(50)]
        public string nombre { get; set; }

        [StringLength(50)]
        public string apellido_pat { get; set; }

        [StringLength(50)]
        public string apellido_mat { get; set; }

        public int? estado_nac { get; set; }

        public int? sexo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_nac { get; set; }
        [Required]
        [StringLength(5)]
        public string codigopostal { get; set; }

        [StringLength(50)]
        public string estado_r { get; set; }

        [StringLength(50)]
        public string municipio { get; set; }

        [StringLength(100)]
        public string colonia { get; set; }

        [StringLength(100)]
        public string calle { get; set; }

        [StringLength(20)]
        public string num_int { get; set; }

        [StringLength(20)]
        public string num_ext { get; set; }

        [StringLength(20)]
        public string telefono { get; set; }

        [StringLength(20)]
        public string celular { get; set; }

        [Required]
        [StringLength(50)]
        public string correo { get; set; }

        [StringLength(50)]
        public string password { get; set; }

        [StringLength(2)]
        public string tipo { get; set; }

        [StringLength(2)]
        public string estatus { get; set; }
        [Required]
        [StringLength(15)]
        public string pertenece { get; set; }
    }
}
