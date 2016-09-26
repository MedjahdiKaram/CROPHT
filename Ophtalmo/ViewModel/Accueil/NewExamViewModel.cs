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
using Ophtalmo.Helpers;


namespace Ophtalmo.ViewModel.Accueil
{
    public class NewExamViewModel : ViewModelBase
    {
        #region Properties

        private Examen _selectedExamen;
        private ObservableCollection<string> _medecinsNames;
        private string _selectedMedecin;
        private ObservableCollection<Examen> _examenBasics;
        private Medecin _medecin;
        public Repository<Examen> ExamensRepository { get; set; }
        public XmlRepository<Examen> ExamensBasicsRepository { get; set; }

        public Repository<CompteRendu> ComptesRendusRepository { get; set; }
        public Repository<Medecin> MedecinRepository { get; set; }

        public ObservableCollection<Examen> ExamenBasics
        {
            get { return _examenBasics; }
            set
            {
                _examenBasics = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand NewExamCommand { get; set; }

        public Medecin Medecin
        {
            get { return _medecin; }
            set
            {
                _medecin = value;
                RaisePropertyChanged();
            }
        }


        public Examen SelectedExamen
        {
            get { return _selectedExamen; }
            set
            {
                _selectedExamen = value;
                ResetComptesRendus(_selectedExamen);
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<string> MedecinsNames
        {
            get { return _medecinsNames; }
            set
            {
                _medecinsNames = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedMedecin
        {
            get { return _selectedMedecin; }
            set
            {
                _selectedMedecin = value;
                FindMedecin();
                RaisePropertyChanged();
            }
        }

        public bool CanEditExam { get; set; }
        #endregion

        public void FindMedecin()
        {
            try
            {
                var list = new ObservableCollection<string>();
                var toubibs = MedecinRepository.Get(n => n.Nom.ToUpper().StartsWith(SelectedMedecin.ToUpper())).ToList();
                if (toubibs == null || toubibs.Count == 0)
                    toubibs = MedecinRepository.Get().ToList();
                foreach (var medecin in toubibs)
                {
                    list.Add(medecin.Nom);
                }
                MedecinsNames = list;
                
            }
            catch (Exception e)
            {
                e.SaveException();
            }
        }

        private void LoadRepos()
        {
            try
            {
                IUnitOfWork ctx = SimpleIoc.Default.GetInstance<OphtalmoContext>();
       
                ExamensRepository = new Repository<Examen>(ctx);
                ComptesRendusRepository = new Repository<CompteRendu>(ctx);
                MedecinRepository = new Repository<Medecin>(ctx);
                ExamensBasicsRepository = new XmlRepository<Examen>("exams.config");
                ExamenBasics = new ObservableCollection<Examen>(ExamensBasicsRepository.Get().ToList());
                FindMedecin();
            }
            catch (Exception e)
            {
                e.SaveException();
            }
        }

        public void NewMedecin()
        {
            try
            {
                Medecin = MedecinRepository.Get(n => n.Nom == SelectedMedecin.ToUpper()).FirstOrDefault();
                if (Medecin == null)
                {
                    MedecinRepository.Add(new Medecin {Nom = SelectedMedecin});
                    Medecin = MedecinRepository.Get(n => n.Nom == SelectedMedecin.ToUpper()).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                e.SaveException();
            }
        }
        private void NewExam()
        {
            try
            {
                if (!CanEditExam)
                    return;
                var editableexamvm = SimpleIoc.Default.GetInstance<EditableExamenViewModel>();
                var newmaladevm = SimpleIoc.Default.GetInstance<NewMaladeViewModel>();
                
                NewMedecin();
                CanEditExam = false;
                var exam = SelectedExamen;
                exam.Moment = DateTime.Now;
                exam.Malade = newmaladevm.SelectedMalade;
                exam.ComptesRendus = new List<CompteRendu>();
                foreach (var compterendu in SelectedExamen.ComtpesRendusPresents)
                {
                    var temp = new CompteRendu();
                    temp.Examen = exam;
                    temp.Contenu = compterendu.Contenu;
                    temp.Nom = compterendu.Nom;
                    temp.ValeursPossible = compterendu.ValeursPossible;

                    // remove all above, add compterendu directly
                    exam.ComptesRendus.Add(temp);
                }
                exam.Medecin = Medecin;
                editableexamvm.Examen = exam;
            }
            catch (Exception e)
            {
                e.SaveException();
            }

        }

        private void ResetComptesRendus(Examen exam)
        {
            try
            {
                if (exam!=null && exam.ComptesRendus!=null)
                    foreach (var comptesRendu in exam.ComptesRendus)
                    {
                        comptesRendu.Contenu=string.Empty;
                    }
            }
            catch (Exception e)
            {
                e.SaveException();
            }
        }

        public NewExamViewModel()
        {
            try
            {
                if (IsInDesignMode) return;
                LoadRepos();
                NewExamCommand = new RelayCommand(NewExam);
                CanEditExam = true;

            }
            catch (Exception e)
            {
                e.SaveException();
            }
        }
    }
}
