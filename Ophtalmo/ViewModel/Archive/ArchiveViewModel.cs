using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using InterfacesLib.Repository;
using ModelsLib;
using Ophtalmo.ViewModel.Accueil;
using System.Globalization;
using Ophtalmo.Helpers;

namespace Ophtalmo.ViewModel.Archive
{
    public class ArchiveViewModel:ViewModelBase
    {
        #region Properties
        private string _searchTerm;
        private bool _isName;
        private bool _isDate;
        private ObservableCollection<Malade> _malades;
        private ObservableCollection<Examen> _examens;
        private Examen _examen;
        private Repository<Malade> _maladesRepository;
        private Repository<Examen> _examensRepository;
        private Malade _malade;
        private Repository<CompteRendu> _compteRenduRepository;

        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                _searchTerm = value;
                RaisePropertyChanged();
            }
        }
        public bool IsName
        {
            get { return _isName; }
            set
            {
                _isName = value;
                RaisePropertyChanged();
            }
        }
        public bool IsDate
        {
            get { return _isDate; }
            set
            {
                _isDate = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Malade> Malades
        {
            get { return _malades; }
            set
            {
                _malades = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Examen> Examens
        {
            get { return _examens; }
            set
            {
                _examens = value;
                RaisePropertyChanged();
            }
        }
        public Repository<Malade> MaladesRepository
        {
            get { return _maladesRepository; }
            set
            {
                _maladesRepository = value;
                RaisePropertyChanged();
            }
        }
        public Repository<Examen> ExamensRepository
        {
            get { return _examensRepository; }
            set
            {
                _examensRepository = value;
                RaisePropertyChanged();
            }
        }
        public Examen Examen
        {
            get { return _examen; }
            set
            {
                _examen = value;
                RaisePropertyChanged();
            }
        }

        public Malade Malade
        {
            get { return _malade; }
            set
            {
                _malade = value;
                RaisePropertyChanged();
                SearchExamsBySelectedMalade();
            }
        }

        public RelayCommand SearchCommand { get; set; }
        public RelayCommand PrintCommand { get; set; }
        public RelayCommand ResetCommand { get; set; }
        public Repository<CompteRendu> CompteRenduRepository
        {
            get { return _compteRenduRepository; }
            set
            {
                _compteRenduRepository = value;
                RaisePropertyChanged();
            }
        }
        
        #endregion
        private void LoadRepos()
        {
            try
            {
                
                IUnitOfWork ctx = SimpleIoc.Default.GetInstance<OphtalmoContext>();
                
                MaladesRepository = new Repository<Malade>(ctx);
                ExamensRepository = new Repository<Examen>(ctx);
                CompteRenduRepository = new Repository<CompteRendu>(ctx);
                var liste = MaladesRepository.Get();
                if (liste!=null)
                    Malades = new ObservableCollection<Malade>(liste.ToList());
            }
            catch (Exception e)
            {
                e.SaveException();
            }
        }



        public void Search()
        {

            try
            {
                Malades = new ObservableCollection<Malade>();
                if (IsName)
                {
                    var malades = MaladesRepository.Get(n => (n.Nom.ToUpper() + " " + n.Prenom.ToUpper()).StartsWith(SearchTerm.ToUpper())).ToList();
                    Malades=new ObservableCollection<Malade>(malades);
                }
                else if (IsDate)
                {
                    DateTime date;
             
                    date = DateTime.ParseExact(SearchTerm, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                
                    Examens =
                        new ObservableCollection<Examen>((ExamensRepository.Get()).ToList().Where(n => n.Moment.ToShortDateString() == date.ToShortDateString()).ToList());
                    foreach (var exam in Examens)
                    {
                        Malades.Add(exam.Malade);
                    }
                }
            }
            catch (Exception e)
            {
                e.SaveException();
            }
        }

        public void Print()
        {
            try
            {
                if (Examen == null) return;
                var printvm = SimpleIoc.Default.GetInstance<PrintViewModel>();               
                printvm.Examen = Examen;
                var printwin = new PrintWindow();
                printwin.Show();
            }
            catch (Exception e)
            {
                e.SaveException();
            }
        }

        public void SearchExamsBySelectedMalade()
        {
            try
            {
                if (!IsDate && Malade!=null)
                {
                    if (Malade.Examens!=null)
                    {
                        Examens = new ObservableCollection<Examen>(Malade.Examens);
                    }
                }
            }
            catch (Exception e)
            {
                e.SaveException();
            }
        }

        public void Reset()
        {
            try
            {
                Malades = new ObservableCollection<Malade>(MaladesRepository.Get().ToList());
                SearchTerm = string.Empty;
                IsName = true;
                Examens = null;
                Examen = null;
            }
            catch (Exception e)
            {
                e.SaveException();
            }
        }
        public ArchiveViewModel()
        {
            try
            {
                LoadRepos();
                SearchCommand=new RelayCommand(Search);
                PrintCommand=new RelayCommand(Print);
                ResetCommand = new RelayCommand(Reset);
                IsName = true;
            }
            catch (Exception e)
            {
                e.SaveException();
            }
        }
    }
}
