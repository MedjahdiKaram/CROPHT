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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ophtalmo.ViewModel.Accueil;

namespace Ophtalmo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
  
        public MainWindow()
        {
            InitializeComponent();
            this.Closing+=MainWindow_Closing;
            ListValeursParDefaut.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
            

            //var fen = new PrintWindow();
            //fen.Show();
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            LoginWindow.Logwin.Close();
            
    
        }

        private void ListValeursParDefaut_MouseLeave(object sender, MouseEventArgs e)
        {
            ListValeursParDefaut.SelectedIndex = -1;
        }

        private void GridDataTemplate_Loaded(object sender, RoutedEventArgs e)
        {
            var x = (Grid)sender;
            x.Width = 350;
        }

        private void AgeBox_GotFocus(object sender, RoutedEventArgs e)
        {
            AgeBox.Text = string.Empty;
        }
    }
}
