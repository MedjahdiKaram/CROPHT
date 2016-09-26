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
    
    public class Examen:IEntity,INotifyPropertyChanged
    {
        public Examen()
        {
            ComptesRendus=new HashSet<CompteRendu>();
        }
        private ICollection<CompteRendu> _comptesRendus;

        [Key]
        public int Id { get; set; }
        public string Nom { get; set; }


        [XmlIgnore]
        //[Column(TypeName = "datetime2")]
        public DateTime Moment { get; set; }
        [XmlIgnore]
        public virtual Malade Malade { get; set; }
        [XmlIgnore]
        public virtual Medecin Medecin { get; set; }

        [XmlIgnore]
        public virtual ICollection<CompteRendu> ComptesRendus
        {
            get { return _comptesRendus; }
            set
            {
                if (Equals(value, _comptesRendus)) return;
                _comptesRendus = value;
                OnPropertyChanged();
            }
        }

        [NotMapped]
        public List<CompteRendu> ComtpesRendusPresents { get; set; }
        //public int? MedecinId { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}