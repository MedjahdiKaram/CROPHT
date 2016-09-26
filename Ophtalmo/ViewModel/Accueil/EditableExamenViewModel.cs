using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using ModelsLib;
using Ophtalmo.Helpers;

namespace Ophtalmo.ViewModel.Accueil
{
    public class EditableExamenViewModel : ViewModelBase
    {
        #region Properties

        private Examen _examen;
        private CompteRendu _selectedCompteRendu;
        private ObservableCollection<string> _valeursParDefaut;
        private string _selectedEye;
        private string _selectedValeur;

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
                if (value == null) return;
                _selectedCompteRendu = value;
                if (value.ValeursPossible == null)
                    return;
                ValeursParDefaut = new ObservableCollection<string>(value.ValeursPossible);
                RaisePropertyChanged();
            }
        }

        public string SelectedEye
        {
            get { return _selectedEye; }
            set
            {
                _selectedEye = value;
                RaisePropertyChanged();
            }
        }

        public string LastEye { get; set; }



        public string SelectedValeur
        {
            get { return _selectedValeur; }
            set
            {
                _selectedValeur = value;
                SetValeurInCompteRendu();
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<string> ValeursParDefaut
        {
            get { return _valeursParDefaut; }
            set
            {
                _valeursParDefaut = value;
                RaisePropertyChanged();
            }
        }

        //private PrintWindow _printWindow;
        private int _selectedCrIndex;
        

        public RelayCommand ValidateCommand { get; set; }

        public int SelectedCrIndex
        {
            get { return _selectedCrIndex; }
            set
            {
                _selectedCrIndex = value; 
                RaisePropertyChanged();
            }
        }

        #endregion

        private void SetValeurInCompteRendu()
        {
            try
            {
                if (string.IsNullOrEmpty(SelectedValeur)) return;
                string result;
                if (string.IsNullOrEmpty(SelectedEye) || SelectedEye.Contains("vide"))
                {
                    result = SelectedValeur + "\n";
                }
                else if (!string.IsNullOrEmpty(SelectedCompteRendu.Contenu) && LastEye==SelectedEye)
                {
                    result = SelectedValeur + "\n";
                }
                else
                {
                    result = SelectedEye + " :" + SelectedValeur + "\n";
                    LastEye = SelectedEye;
                }
                SelectedCompteRendu.Contenu += result;
            }
            catch (Exception e)
            {
                e.SaveException();
            }
        }
        private void Validate()
        {
            try
            {
           
                var newExamVm = SimpleIoc.Default.GetInstance<NewExamViewModel>();
                var newMaladeVm = SimpleIoc.Default.GetInstance<NewMaladeViewModel>();
             
                
                Examen.Malade = newMaladeVm.SelectedMalade;
                newExamVm.ExamensRepository.Add(Examen);

                //
                var printvm = SimpleIoc.Default.GetInstance<PrintViewModel>();
                var exam = newExamVm.ExamensRepository.Get().ToList().FirstOrDefault(n => n.Moment == Examen.Moment);
                printvm.Examen = exam;
                var printwin = new PrintWindow();
                printwin.Show();
                SelectedEye = "(vide)";
                LastEye = string.Empty;
                //Thread.Sleep(1000);
                //ResetForms();           

            }
            catch (Exception e)
            {
                e.SaveException();
                ResetForms();  
            }
        }

        public void ResetForms()
        {
            try
            {

                SelectedCompteRendu = new CompteRendu
                {
                    ValeursPossible = new List<string>()


                };
                Examen = null;
                var newExamVm = SimpleIoc.Default.GetInstance<NewExamViewModel>();
                var newMaladeVm = SimpleIoc.Default.GetInstance<NewMaladeViewModel>();
                
                newMaladeVm.SelectedName = string.Empty;
                newMaladeVm.SelectedLastName = string.Empty;
                newMaladeVm.Names = new List<string>();
                newMaladeVm.LastNames = new ObservableCollection<string>();
                newMaladeVm.SelectedMalade = null;
                newMaladeVm.SelectedAge = 1;
                newExamVm.SelectedMedecin = string.Empty;
                newExamVm.Medecin = new Medecin();


                var examComptesRendusPresnts = newExamVm.SelectedExamen.ComtpesRendusPresents;
                var examName = newExamVm.SelectedExamen.Nom;
                
                newExamVm.SelectedExamen = new Examen();
                newExamVm.SelectedExamen.ComtpesRendusPresents = examComptesRendusPresnts;
                newExamVm.SelectedExamen.Nom = examName;
             
                
                newExamVm.ExamenBasics = new ObservableCollection<Examen>(newExamVm.ExamensBasicsRepository.Get().ToList());



                newExamVm.CanEditExam = true;



            }
            catch (Exception e)
            {
                e.SaveException();
            }
            
        }

        
        public EditableExamenViewModel()
        {
            try
            {
                ValidateCommand = new RelayCommand(Validate);
           
                
               
            }
            catch (Exception e)
            {
                e.SaveException();
            }
        }
    }
}
