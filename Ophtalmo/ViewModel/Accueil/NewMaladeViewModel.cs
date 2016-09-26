using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DAL;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using InterfacesLib.Repository;
using ModelsLib;
using Ophtalmo.Helpers;
using System.Diagnostics;

namespace Ophtalmo.ViewModel.Accueil
{
    public class NewMaladeViewModel : ViewModelBase

    {
        #region Properties

        private bool _cansearch = true;
        private Malade _selectedMalade;
        private string _selectedName;
        private string _selectedLastName;
        private int _selectedAge;
        private List<string> _names;
        private ObservableCollection<string> _lastNames;
        public Repository<Malade> MaladesRepository { get; set; }
        public RelayCommand NewMaladeCommand { get; set; }
        public RelayCommand NewExamCommand { get; set; }
        public RelayCommand EmptyWaitingList { get; set; }



        public Malade SelectedMalade
        {
            get { return _selectedMalade; }
            set
            {
                _selectedMalade = value;
                RaisePropertyChanged();
            }
        }

        
        public string SelectedName
        {
            get { return _selectedName; }
            set
            {
                _selectedName = value;
                RaisePropertyChanged();
                if (value.Length > 2 && _cansearch)
                    FindMaladesByName();         
            

            }
        }


        public string SelectedLastName
        {
            get { return _selectedLastName; }
            set
            {
                _selectedLastName = value;
                RaisePropertyChanged();
                if (value.Length > 2 && _cansearch)
                    FindMaladesByLastName();
            }
        }


        private ObservableCollection<Malade> attentes_malades;
        public ObservableCollection<Malade> Attentes_malades
        {

            get
            {
                return attentes_malades;
            }
            set
            {
                attentes_malades = value;
                RaisePropertyChanged();

            }
        }
        public int SelectedAge
        {
            get { return _selectedAge; }
            set
            {
                _selectedAge = value;
                NewMaladeCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }



        public ObservableCollection<string> LastNames
        {
            get { return _lastNames; }
            set
            {
                _lastNames = value;

                RaisePropertyChanged();
            }
        }

        public List<string> Names
        {
            get { return _names; }
            set
            {
                _names = value;
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
            }
            catch (Exception e)
            {
                e.SaveException();
            }
        }

        public void FindMaladesByName()
        {
         
            try
            {
                if (string.IsNullOrEmpty(SelectedName))
                    return;

                if (!_cansearch)
                    return;
                var T = new Thread(() =>
                {
                    _cansearch = false;
                    var malades = MaladesRepository.Get(m => m.Nom.ToUpper().StartsWith(SelectedName.ToUpper())).ToList();

                    Names = new List<string>();
                    foreach (var malade in malades)
                    {
                        Names.Add(malade.Nom);
                    }
                
                    _cansearch = true;

                });
                T.Start();
            }
            catch (Exception e)
            {
                e.SaveException();

            }
        }

        public void FindMaladesByLastName()
        {
            try
            {
                _cansearch = false;
                var malades =
                    MaladesRepository.Get(
                        m => m.Nom.ToUpper() == SelectedName && m.Prenom.ToUpper().StartsWith(SelectedLastName.ToUpper()))
                        .ToList();
                LastNames = new ObservableCollection<string>();
                foreach (var malade in malades)
                {
                    LastNames.Add(malade.Prenom);
                }
                SearchMalade();
                _cansearch = true;
            }
            catch (Exception e)
            {
                e.SaveException();
            }
        }

        public void SearchMalade()
        {
            try
            {
                var malade =
                    MaladesRepository.Get(n => n.Nom.ToUpper() == SelectedName && n.Prenom.ToUpper() == SelectedLastName)
                        .ToList();
                if (malade.Count <= 0)
                {
                    SelectedMalade = null;
                    SelectedAge = 0;
    
                }
                else
                {
                    //SelectedMalade = malade.FirstOrDefault();
                    SelectedAge = malade.FirstOrDefault().Age;
                    RaisePropertyChanged();
                }
            }
            catch (Exception e)
            {
                e.SaveException();

            }
        }

        private void AddMalade()
        {
            try
            {
               // SelectedMalade = new Malade();
                Malade malade = new Malade {Nom = SelectedName, Prenom = SelectedLastName, Age = SelectedAge};
                this.Attentes_malades.Add(malade);
                SelectedName = string.Empty;
                SelectedLastName = string.Empty;
                SelectedAge = 0;

                RaisePropertyChanged();
            }
            catch (Exception e)
            {
               e.SaveException();

            }
        }


        private bool CanAddMalade()
        {
            try
            {
                var malade =
                    MaladesRepository.Get(n => n.Nom == SelectedName && n.Prenom == SelectedLastName && n.Age == SelectedAge)
                        .FirstOrDefault();
                return SelectedAge > 0;
            }
            catch (Exception e)
            {
                e.SaveException();
                return false;

            }
        }

        public NewMaladeViewModel()
        {
            try
            {
                if (!IsInDesignMode)
                {
                    LoadRepos();
                    NewMaladeCommand = new RelayCommand(AddMalade, CanAddMalade);
                    Attentes_malades = new ObservableCollection<Malade>();
                    EmptyWaitingList = new RelayCommand(ResetWaitingList);

                   

                }
            }
            catch (Exception e)
            {
                e.SaveException();

            }
        }

        private void ResetWaitingList()
        {
            Attentes_malades.Clear();
        }
    }
}
