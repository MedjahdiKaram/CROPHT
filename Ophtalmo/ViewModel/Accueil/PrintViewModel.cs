using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using GalaSoft.MvvmLight;
using ModelsLib;

namespace Ophtalmo.ViewModel.Accueil
{
    public class PrintViewModel : ViewModelBase
    {
        #region Properties

        private Examen _examen;

        public Examen Examen
        {
            get { return _examen; }
            set
            {
                _examen = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public PrintViewModel()
        {
        }
    }
}
