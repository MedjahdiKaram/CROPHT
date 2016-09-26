/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Ophtalmo"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using DAL;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Ophtalmo.ViewModel.Accueil;
using Ophtalmo.ViewModel.Archive;
using Ophtalmo.ViewModel.Config;
using Ophtalmo.ViewModel.Login;

namespace Ophtalmo.ViewModel
{

    public class ViewModelLocator
    {

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<NewExamViewModel>();
            SimpleIoc.Default.Register<NewMaladeViewModel>();
            SimpleIoc.Default.Register<EditableExamenViewModel>();
            SimpleIoc.Default.Register<PrintViewModel>();
            SimpleIoc.Default.Register<ArchiveViewModel>();
            SimpleIoc.Default.Register<OphtalmoContext>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<ConfigurationViewModel>();
        }

        public OphtalmoContext DbOphtalmoContext
        {
            get { return ServiceLocator.Current.GetInstance<OphtalmoContext>(); }
        }
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public NewExamViewModel NewExamVm
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NewExamViewModel>();
            }
        }
        public NewMaladeViewModel NewMaladeVm 
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NewMaladeViewModel>();
            }
        }
        public EditableExamenViewModel EditableExamVm
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditableExamenViewModel>();
            }
        }
        public PrintViewModel PrintVm
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PrintViewModel>();
            }
        }

        public ArchiveViewModel ArchiveVm
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ArchiveViewModel>();
            }
        }       
        public ConfigurationViewModel ConfigVm
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ConfigurationViewModel>();
            }
        }
        public LoginViewModel LoginVm
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginViewModel>();
            }
        }
        public static void Cleanup()
        {
            
        }
    }
}