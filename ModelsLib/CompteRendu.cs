using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using InterfacesLib.Shared;
using ModelsLib.Annotations;
using PropertyChanged;

namespace ModelsLib
{
    [Serializable]
    public class CompteRendu:IEntity,INotifyPropertyChanged
    {
        //private string _contenu;
        private List<string> _valeursPossible;

        [Key]
        
        public int Id { get; set; }
        public string Nom { get; set; }

        [XmlIgnore]
        public string Contenu { get; set; }
        [XmlIgnore]
        public virtual Examen Examen { get; set; }

        [NotMapped]
        public List<string> ValeursPossible
        {
            get { return _valeursPossible; }
            set
            {
             
                _valeursPossible = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}