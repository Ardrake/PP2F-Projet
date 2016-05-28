using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;

namespace HLN_645_050537
{
    /// <summary>
    /// Interaction logic for MenuPrincipale.xaml
    /// </summary>
    public partial class MenuPrincipale : Window
    {
        public string SelectedSpecialty { get; set; }

        NHL_DataDataContext context = new NHL_DataDataContext("Data Source = CABANONS00006V; Initial Catalog = NLH-645-050537; Integrated Security = True");

        public MenuPrincipale()
        {
            InitializeComponent();
        }

        private void SwtichAdmin_Click(object sender, RoutedEventArgs e)
        {
            HideAll();
            ZoneAdmin.Visibility = Visibility.Visible;
        }

        private void SwtichDocteur_Click(object sender, RoutedEventArgs e)
        {
            HideAll();
            ZoneDocteur.Visibility = Visibility.Visible;
        }

        private void SwtichInfirmiere_Click(object sender, RoutedEventArgs e)
        {
            HideAll();
            ZoneInfirmiere.Visibility = Visibility.Visible;
        }

        private void SwtichPreposer_Click(object sender, RoutedEventArgs e)
        {
            HideAll();
            ZonePreposer.Visibility = Visibility.Visible;
        }

        private void HideAll()
        {
            ZoneAdmin.Visibility = Visibility.Collapsed;
            ZoneDocteur.Visibility = Visibility.Collapsed;
            ZoneInfirmiere.Visibility = Visibility.Collapsed;
            ZonePreposer.Visibility = Visibility.Collapsed;
            ZoneAdminGereDocteur.Visibility = Visibility.Collapsed;
            ZoneAdminGereFacture.Visibility = Visibility.Collapsed;
        }

        private void SwtichAll_Click(object sender, RoutedEventArgs e)
        {
            ZoneAdmin.Visibility = Visibility.Visible;
            ZoneDocteur.Visibility = Visibility.Visible;
            ZoneInfirmiere.Visibility = Visibility.Visible;
            ZonePreposer.Visibility = Visibility.Visible;
        }

        private void AdminGereDocteur_Click(object sender, RoutedEventArgs e)
        {
            ZoneAdminGereFacture.Visibility = Visibility.Collapsed;
            ZoneAdminGereDocteur.Visibility = Visibility.Visible;
            comboSpecialtyListe.ItemsSource = context.Specialties;
            ListeDesDocteurs.DataContext = GetAllDoctors();
            
        }

        private void AdminGereFacture_Click(object sender, RoutedEventArgs e)
        {
            ZoneAdminGereDocteur.Visibility = Visibility.Collapsed;
            ZoneAdminGereFacture.Visibility = Visibility.Visible;
        }

        private void btnInsereDocteur_Click(object sender, RoutedEventArgs e)
        {
            bool validSpecialty = false;
            bool validDoctorID = false;
            Specialty mySelectedSpecialy = new Specialty();
            int value = -1;

            try
            { 
                mySelectedSpecialy = (Specialty)((Specialty)comboSpecialtyListe.SelectedItem);
                value = mySelectedSpecialy.SpecialtyID;
                validSpecialty = true;
            }
            catch
            {
                MessageBox.Show("Vous devez choisir une spécialté");
            }


            if (tbAdminGereDocId.Text != "")
            {
                var findDoc = from d in context.Doctors
                              where d.DoctorID == tbAdminGereDocId.Text
                              select d;

                if (findDoc.Count() == 0)
                {
                    validDoctorID = true;
                }
                else
                {
                    MessageBox.Show("Erreur Idenification du doctor deja existante");
                }
            }
            else
            {
                MessageBox.Show("Idenification du doctor ne peut pas etre vide");
            }

            if (validSpecialty && validDoctorID) 
            { 
                try
                {
                    // insere docteur
                    context.InsereDocteur(tbAdminGereDocId.Text, tbAdminGereDocNom.Text, tbAdminGereDocPrenom.Text, value);
                    MessageBox.Show("Nouveau docteur inséré");
                }
                catch (Exception ex)
                {
                    //affiche message d'erreur
                    MessageBox.Show("Erreur " + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                finally
                {
                    //re-init formulaire
                    ListeDesDocteurs.DataContext = GetAllDoctors();
                }
            }
        }

        private ObservableCollection<Doctor> GetAllDoctors()
        {
            ObservableCollection<Doctor> myDoctorList = new ObservableCollection<Doctor>();
            try
            {
                var getlist = context.Doctors;
                foreach (var item in getlist)
                {
                    myDoctorList.Add(item);
                }
            }
            catch
            {
                MessageBox.Show("Erreur de connection a la base de donnée");
            }
            return myDoctorList;
        }
    }
}
