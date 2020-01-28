namespace Educafin.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModeloRegistroUsuario : DbContext
    {
        public ModeloRegistroUsuario()
            : base("name=ModeloRegistroUsuario")
        {
        }

        public virtual DbSet<usuario> usuario { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
