namespace Educafin.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<institucion> institucion { get; set; }
        public virtual DbSet<proveedor> proveedor { get; set; }
        public virtual DbSet<Proveedor_Institucion> Proveedor_Institucion { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<institucion>()
            //    .HasMany(e => e.Proveedor_Institucion)
            //    .WithRequired(e => e.institucion)
            //    .HasForeignKey(e => e.cct_int)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<proveedor>()
            //    .HasMany(e => e.Proveedor_Institucion)
            //    .WithRequired(e => e.proveedor)
            //    .HasForeignKey(e => e.rfc_prov)
            //    .WillCascadeOnDelete(false);
        }
    }
}
