using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using InterfacesLib.Shared;

namespace ModelsLib
{
    public class Medecin:IEntity

    {
        public Medecin()
        {
            Examen=new HashSet<Examen>();
        }
        [Key]
        public int Id { get; set; }
        public string Nom { get; set; }

        public virtual ICollection<Examen> Examen { get; set; }
    }
}