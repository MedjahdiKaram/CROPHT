using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesLib.Shared;
namespace ModelsLib
{
    public class Login:IEntity
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string MotDePasse { get; set; }
        public string Serial { get; set; }

        public string Secret { get; set; }
    }
}
