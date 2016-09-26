using System.ComponentModel.DataAnnotations;

namespace InterfacesLib.Shared
{
    public interface IEntity
    {      
        int Id { get; set; }
        string Nom { get; set; }

    }
}