namespace Educafin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("proveedor")]
    public partial class proveedor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public proveedor()
        {
            Proveedor_Institucion = new HashSet<Proveedor_Institucion>();
        }

        [Key]
        [Required]
        [StringLength(15)]
        public string RFC { get; set; }

        [StringLength(50)]
        [Required]
        public string nombre { get; set; }

        [StringLength(100)]
        [Required]
        public string representante { get; set; }

        [StringLength(5)]
        [Required]
        public string codigopostal { get; set; }

        [StringLength(50)]
        [Required]
        public string estado { get; set; }

        [StringLength(50)]
        [Required]
        public string municipio { get; set; }

        [StringLength(50)]
        [Required]
        public string colonia { get; set; }

        [StringLength(100)]
        [Required]
        public string calle { get; set; }

        [StringLength(10)]
        [Required]
        public string num_ext { get; set; }

        [StringLength(10)]
        public string num_int { get; set; }

        [StringLength(20)]
        [Required]
        public string telefono { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "El campo correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo invalido")]

        [StringLength(50)]
        public string correo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Proveedor_Institucion> Proveedor_Institucion { get; set; }
    }
}
