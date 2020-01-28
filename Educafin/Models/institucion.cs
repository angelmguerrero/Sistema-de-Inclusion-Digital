namespace Educafin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("institucion")]
    public partial class institucion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public institucion()
        {
            Proveedor_Institucion = new HashSet<Proveedor_Institucion>();
        }

        [Key]
        [Required]
        [StringLength(15)]
        public string CCT { get; set; }
        [Required]
        [StringLength(80)]
        public string nombre { get; set; }

        [Required]
        [StringLength(1)]
        public string grado { get; set; }
        [Required]
        [StringLength(50)]
        public string estado { get; set; }
        [Required]
        [StringLength(50)]
        public string municipio { get; set; }
        [Required]
        [StringLength(100)]
        public string colonia { get; set; }
        [Required]
        [StringLength(100)]
        public string calle { get; set; }
        [Required]
        [StringLength(10)]
        public string numero { get; set; }
        [Required]
        [StringLength(5)]
        public string codigopostal { get; set; }
        [Required]
        [StringLength(20)]
        public string telefono { get; set; }


        [Display(Name = "Email address")]
        [Required(ErrorMessage = "El campo correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo invalido")]
        [StringLength(50)]
        public string coreo { get; set; }

        public bool medioprovedor { get; set; }


        [StringLength(15)]
        public string dependencia { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Proveedor_Institucion> Proveedor_Institucion { get; set; }
    }
}
