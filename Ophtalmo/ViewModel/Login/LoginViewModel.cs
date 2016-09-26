using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DAL;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ophtalmo.Helpers;

namespace Ophtalmo.ViewModel.Login
{
    public class LoginViewModel : ViewModelBase
    {

        #region Properties

        private string _username;
        private string _password;
        private bool _isEnabled;
        private ModelsLib.Login _login;
        private string _message;

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                RaisePropertyChanged();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged();
            }
        }

        public ModelsLib.Login Login
        {
            get { return _login; }
            set
            {
                _login = value;
                RaisePropertyChanged();
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand LoginCommand { get; set; }

        #endregion


        private void LoadConfigLogin()
        {
            try
            {
                var confRepo=new XmlRepository<ModelsLib.Login>("config.xml");
                Login = confRepo.Get().FirstOrDefault();
            }
            catch (Exception e)
            {
                e.SaveException();
            }

        }

        private void GrantAccess()
        {
            try
            {
                IsEnabled = true;
                var win = new MainWindow();

                win.Show();
            }
            catch (Exception e)
            {
                e.SaveException();

            }

        }



        private void LoginFunction()
        {
            try
            {
                if (Security.ToMd5(Password + Username + DateTime.Today.Month+DateTime.Today.Year) == Login.Secret)
                    GrantAccess();
                else if (Username == Login.Nom)
                {
                    if (Security.ToMd5(Password) == Login.MotDePasse )
                    {
                        var macEncrypted=Security.ToMd5(Security.GetMacAddress());
                        if (macEncrypted == Login.Serial)                        
                            GrantAccess(); 
                        else
                            Message = "Vérifiez votre numéro de série";
                    }
                }
                else

                {
                    IsEnabled = false;                           
                    Message = "Vérifiez votre nom / mot de passe";
                }
            }
            catch (Exception e)
            {
                e.SaveException();
            }
        }
        public LoginViewModel()
        {
            try
            {
                Username = "admin";
                Password = "admin";
                IsEnabled = false;
                LoginCommand = new RelayCommand(LoginFunction);
                LoadConfigLogin();
                Message = "Entrez votre nom & mot de passe.";
            }
            catch (Exception e)
            {
                e.SaveException();
            }
        }
    }
}
