using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesLib.Shared;

namespace ModelsLib
{
    public class Malade : IEntity
    {
        public Malade()
        {
            Examens=new HashSet<Examen>();
        }
        [Key]
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int Age { get; set; }
        public virtual ICollection<Examen> Examens { get; set; }


    }
}
