using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Ioc;
using Ophtalmo.ViewModel.Login;

namespace Ophtalmo
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LoginViewModel Loginvm ;
        public static LoginWindow Logwin;
        public LoginWindow()
        {
            InitializeComponent();
            Loginvm = SimpleIoc.Default.GetInstance<LoginViewModel>();
            Loginvm.PropertyChanged += loginvm_PropertyChanged;
            Logwin=this;

        }

        private void loginvm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsEnabled")
            {
                 if (Loginvm.IsEnabled)
                     this.Hide();
            }
        }
    }
}
