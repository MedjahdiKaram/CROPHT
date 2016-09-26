using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using InterfacesLib.Repository;
using ModelsLib;

namespace DAL
{
    public class OphtalmoContext:DbContext,IUnitOfWork
    {
        public DbSet<Medecin> Medecins { get; set; }
        public DbSet<Examen> Examens { get; set; }
        public DbSet<CompteRendu> CompteRendus { get; set; }
        public DbSet<Malade> Malades { get; set; }

        public OphtalmoContext()
            : base("name=OphtalmoDbCe")
        {
            Database.CreateIfNotExists();
            
        }
        
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Examen>().HasOptional(p => p.Medecin)
        //        .WithMany(b => b.Examen)
        //        .HasForeignKey(p => p.ExamenId);
        //}

       
    }
}
