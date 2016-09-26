using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using Ophtalmo.Helpers;
using Ophtalmo.ViewModel;
using Ophtalmo.ViewModel.Accueil;

namespace Ophtalmo
{
    /// <summary>
    /// Interaction logic for PrintWindow.xaml
    /// </summary>
    public partial class PrintWindow : Window
    {
        public PrintDialog PrintDlg { get; set; }

        public PrintWindow()
        {
            InitializeComponent();
        }

        public void Print()
        {
            var idocument = BigFlowDocument as IDocumentPaginatorSource;


            if (idocument != null)
            {
                //var count = idocument.DocumentPaginator.PageCount;
                //while (!FlowDocReader.CanGoToPage(count+1))
                //{
                //    top += 5;
                //    Debug.WriteLine(top);
                //    var thick = FooterContainer.Margin;
                //    thick.Top = top;
                //    FooterContainer.Margin = thick;
                //}
                //FooterContainer.Margin = new Thickness { Top = top - 5 };

                PrintDlg.PrintDocument(idocument.DocumentPaginator, "Compte rendu Complet");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Print();
            Thread.Sleep(500);
            SimpleIoc.Default.GetInstance<EditableExamenViewModel>().ResetForms();
        }

        private void TheBigList_Loaded(object sender, RoutedEventArgs e)
        {
            var FooterContainer = new BlockUIContainer();
            //BigFlowDocument.PagePadding.
            var vm = SimpleIoc.Default.GetInstance<PrintViewModel>();
            var cr = vm.Examen.ComptesRendus;
            var titres = 0;
            var lignes = 0;

            foreach (var compteRendu in cr)
            {
                if (string.IsNullOrEmpty(compteRendu.Contenu) || compteRendu.Contenu.Length < 3)
                    continue;
                var paragrapheTitreCompterendu = new Paragraph {FontSize = 16};
                paragrapheTitreCompterendu.Inlines.Add(compteRendu.Nom + ":");
                titres++;

                var paragrapheContenu = new Paragraph {FontSize = 12};

                paragrapheContenu.Inlines.Add(compteRendu.Contenu);
                var line = Regex.Matches(compteRendu.Contenu, "\r", RegexOptions.IgnoreCase).Count;
                if (line == 0)
                    line = 1;
                lignes += line;

                TheBigList.ListItems.Add(new ListItem {Blocks = {paragrapheTitreCompterendu, paragrapheContenu}});
            }


            PrintDlg = new PrintDialog();
            BigFlowDocument.PageHeight = PrintDlg.PrintableAreaHeight;
            BigFlowDocument.PageWidth = PrintDlg.PrintableAreaWidth;
            var diff = 500 - (titres*22 + lignes*18);
            if (lignes + titres >= 30)
                diff = 860 - (titres*22 + lignes*18) + (30*19);
            if (diff > 0)
                FooterContainer.Margin = new Thickness {Top = diff};
            Print();
            var prvm = SimpleIoc.Default.GetInstance<PrintViewModel>();
            MessageBox.Show("L'impression du compte rendu de Mr/Mmme:\n"+prvm.Examen.Malade.Nom+" "+ prvm.Examen.Malade.Prenom + " a été ajoutée à votre liste d'impression ", "Impression Confirmée !");
            Thread.Sleep(500);
            var editableexam = SimpleIoc.Default.GetInstance<EditableExamenViewModel>();
            var newmalade = SimpleIoc.Default.GetInstance<NewMaladeViewModel>();
            if (newmalade!=null && newmalade.SelectedMalade!=null)
                newmalade.Attentes_malades.Remove(newmalade.SelectedMalade);
            editableexam.ResetForms();
            Close();
        }
    }
}
