using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DAL;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using ModelsLib;
using Ophtalmo.Helpers;
using Ophtalmo.ViewModel.Accueil;

namespace Ophtalmo.ViewModel.Config
{
    public class ConfigurationViewModel:ViewModelBase
    {
        #region Properties

        private ObservableCollection<Examen> _examens;
        private CompteRendu _selectedCompteRendu;
        private string _newDefaultValue;
        private string _newPassword;
        private Examen _examen;
        private string _newUserName;
        private string _selectedDefaultValue;
        public XmlRepository<Examen> ExamensRepository { get; set; }
        public XmlRepository<ModelsLib.Login> ConfigRepository { get; set; }

        public ObservableCollection<Examen> Examens
        {
            get { return _examens; }
            set
            {
                _examens = value;
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

        public CompteRendu SelectedCompteRendu
        {
            get { return _selectedCompteRendu; }
            set
            {
                _selectedCompteRendu = value;
                RaisePropertyChanged();
            }
        }

        public string NewDefaultValue
        {
            get { return _newDefaultValue; }
            set
            {
                _newDefaultValue = value;
                RaisePropertyChanged();
                AddDfaultValueCommand.RaiseCanExecuteChanged();
            }
        }

        public string SelectedDefaultValue
        {
            get { return _selectedDefaultValue; }
            set
            {
                _selectedDefaultValue = value;
                RaisePropertyChanged();
                DeleteDefaultValueCommand.RaiseCanExecuteChanged();
            }
        }

        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                _newPassword = value;
                RaisePropertyChanged();
            }
        }

        public string NewUserName
        {
            get { return _newUserName; }
            set
            {
                _newUserName = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand AddDfaultValueCommand { get; set; }
        public RelayCommand DeleteDefaultValueCommand { get; set; }
        public RelayCommand SetNewCredentialCommand { get; set; }
       
        #endregion


        public void LoadRepo()
        {
            try
            {
                ExamensRepository=new XmlRepository<Examen>("exams.config");
                ConfigRepository=new XmlRepository<ModelsLib.Login>("config.xml");
                Examens = new ObservableCollection<Examen>(ExamensRepository.Get());
            }
            catch (Exception e)
            {
                e.SaveException();
            }
        }
        public void AddDefaultValue()
        {
            try
            {
             
                SelectedCompteRendu.ValeursPossible.Add(NewDefaultValue);
                var edi = SimpleIoc.Default.GetInstance<EditableExamenViewModel>();
                if (edi.SelectedCompteRendu!=null)
                    edi.SelectedCompteRendu.ValeursPossible.Add(NewDefaultValue);


                ExamensRepository.Update();     
                SelectedCompteRendu.ValeursPossible.Add(NewDefaultValue);
                //AddDefaultValueRaisePropertyChanged("SelectedCompteRendu");                 
                LoadRepo();
                NewDefaultValue=string.Empty;

            }
            catch (Exception e)
            {
                e.SaveException();
            }
        }



        public void DeleteDefaultValue()
        {
            try
            {
                if (!string.IsNullOrEmpty(SelectedDefaultValue))
                {
                    SelectedCompteRendu.ValeursPossible.Remove(SelectedDefaultValue);
                    ExamensRepository.Update();
                    LoadRepo();
                    NewDefaultValue = string.Empty;
                }
            }
            catch (Exception e)
            {
                e.SaveException();
            }
        }

  
        public void SetNewCredential()
        {
            try
            {
                var conf = ConfigRepository.Get().FirstOrDefault();
                if (conf == null) return;
                if (!string.IsNullOrEmpty(NewUserName))
                {
                    conf.Nom = NewUserName;
                    ConfigRepository.Update();
                    DisplaySuccessBox("nom d'utilisateur");
                }
                if (!string.IsNullOrEmpty(NewPassword))
                {
                    conf.MotDePasse = Security.ToMd5(NewPassword);
                    ConfigRepository.Update();
                    DisplaySuccessBox();
                }
            }
            catch (Exception e)
            {
                e.SaveException();
            }
        }

        public ConfigurationViewModel()
        {
            try
            {
                LoadRepo();
                AddDfaultValueCommand=new RelayCommand(AddDefaultValue,()=> !string.IsNullOrEmpty(NewDefaultValue));
                SetNewCredentialCommand = new RelayCommand(SetNewCredential);
                DeleteDefaultValueCommand=new RelayCommand(DeleteDefaultValue,()=> !string.IsNullOrEmpty(SelectedDefaultValue));
            }
            catch (Exception e)
            {
                e.SaveException();
            }
          
        }
      private void DisplaySuccessBox(string content="mot de passe")
        {
             MessageBox.Show("Le "+content+" a été changé avec succès","Opération réussite");
        }
    }
}
