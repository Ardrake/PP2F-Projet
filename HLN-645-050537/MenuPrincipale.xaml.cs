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

        NHL_DataDataContext context = new NHL_DataDataContext("Data Source = CABANONS00171; Initial Catalog = NLH-645-050537; Integrated Security = True");
        //NHL_DataDataContext context = new NHL_DataDataContext("Data Source = ANDRE-PC; Initial Catalog = NLH-645-050537; Integrated Security = True");

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
            PreposerGestionPatient.Visibility = Visibility.Collapsed;
            DocteurCongePatient.Visibility = Visibility.Collapsed;
            InfirmiereListeDesPatients.Visibility = Visibility.Collapsed;
            PreposerAdmissionPatient.Visibility = Visibility.Collapsed;
            
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

            var query = from a in context.AdmissionRecords
                        where a.Facture == false
                        select a;

            ObservableCollection<AdmissionRecord> myFactureList = new ObservableCollection<AdmissionRecord>();

            var getlist = query;
            foreach (var item in getlist)
            {
                myFactureList.Add(item);
            }

            var queryTemine = from a in context.AdmissionRecords
                              where a.Facture == true
                              select a;

            ObservableCollection<AdmissionRecord> myFactureListTermnine = new ObservableCollection<AdmissionRecord>();

            var getlisttermine = queryTemine;
            foreach (var item in getlisttermine)
            {
                myFactureListTermnine.Add(item);
            }

            ListAFacture.DataContext = myFactureList;
            ListAFactureTermine.DataContext = myFactureListTermnine;
        }

        // Bouton genere Facture
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        // Bouton re-impression facture
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void PreposerPatient_Click(object sender, RoutedEventArgs e)
        {
            PreposerGestionPatient.Visibility = Visibility.Visible;
            PreposerAdmissionPatient.Visibility = Visibility.Collapsed;
            DocteurListe.ItemsSource = GetAllDoctors();
            ListeDesPatients.DataContext = GetAllPatient();

        }

        private void PreposerAdmission_Click(object sender, RoutedEventArgs e)
        {
            PreposerAdmissionPatient.Visibility = Visibility.Visible;
            PreposerGestionPatient.Visibility = Visibility.Collapsed;
            SelectionPatientAile.ItemsSource = context.Wards;

            var selecPatientAdmission = from p in context.GetPatientAdmission()
                                        select p;

            SelectionPatientAdmission.ItemsSource = selecPatientAdmission;
        }

        // Section Liste des patients (Menu Infirmiere)
        private void InfirmiereListePatient_Click(object sender, RoutedEventArgs e)
        {
            InfirmiereListeDesPatients.Visibility = Visibility.Visible;
            LVListeDesPatient.DataContext = GetPatientAdmis();
        }

        private void SelectionPatientAdmission_DropDownClosed(object sender, EventArgs e)
        {
            int Age = 0;
            try
            {
                DateTime myDob = new DateTime();
                myDob = (DateTime)((GetPatientAdmissionResult)SelectionPatientAdmission.SelectedItem).DateOfBirth;
                Age = DateTime.MinValue.AddDays(DateTime.Now.Subtract(myDob).TotalHours / 24).Year - 1;
            }
            catch
            {
                Age = 0;
            }

            if (Age < 18)
            {
                SelectionPatientAile.SelectedIndex = 2;


                Ward myWard = null;

                myWard = (Ward)SelectionPatientAile.SelectedValue;

                var selectionChambre = from b in context.Beds
                                       where b.WardName == myWard.WARD1 && b.Occupied == false
                                       select b;

                if (selectionChambre.Count() > 0)
                {
                    SelectionPatientChambre.ItemsSource = selectionChambre;
                }

            }

        }

        private void SelectionPatientAile_DropDownClosed(object sender, EventArgs e)
        {
            Ward myWard = null;

            myWard = (Ward)SelectionPatientAile.SelectedValue;

            var selectionChambre = from b in context.Beds
                                   where b.WardName == myWard.WARD1 && b.Occupied == false
                                   select b;

            if (selectionChambre.Count() > 0)
            {
                SelectionPatientChambre.ItemsSource = selectionChambre;
            }
        }

        // ****************************************************//
        // Creer le dossier d'admission dans la base de donnée //
        // ****************************************************//
        private void AdmissionAcceter_Click(object sender, RoutedEventArgs e)
        {
            // Besoin de rajouter de la validation pour validé que les combobos sont selectionnez
            string myHealthNumber = "";
            myHealthNumber = ((GetPatientAdmissionResult)SelectionPatientAdmission.SelectedItem).HealthNumber;
            Bed myAdmissionBed = (Bed)SelectionPatientChambre.SelectedValue;
            string admissionId = DateTime.Now.ToString("M/d/yyyy") + "-" + myHealthNumber;

            if (myHealthNumber != "" && myAdmissionBed != null)
            {
                context.InsereAdmission(admissionId, myHealthNumber, myAdmissionBed.BedNumber, DateTime.Now);
                MessageBox.Show("Admission complété" );
                myAdmissionBed = null;
                myHealthNumber = "";
                SelectionPatientAdmission.SelectedIndex = -1;
                SelectionPatientAile.SelectedIndex = -1;
                SelectionPatientChambre.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Veuilles faire les selection");
            }
        }

        // Load la liste des patient pouvant avoir un congé quand on click sur Docteur / congé
        private void btnDocteurCongePatient_Click(object sender, RoutedEventArgs e)
        {
            DocteurCongePatient.Visibility = Visibility.Visible;

            var query = from p in context.AdmissionRecords
                        where p.DischargeDate == null
                        select p;

            ListePatientHositaliser.DataContext = query;
        }
        
        // ****************************************************//
        // Trait le congé d'un patient                         //
        // ****************************************************//
        private void Button_Click(object sender, RoutedEventArgs e)  // Congé Patient
        {
            AdmissionRecord MyPatienConge = new AdmissionRecord();
            MyPatienConge = (AdmissionRecord)ListePatientHositaliser.SelectedItem;

            try
            {
                context.CongePatient(MyPatienConge.AdmissionID, MyPatienConge.BedNumber);
                MessageBox.Show("Le congé du patient a été traité");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                var query = from p in context.AdmissionRecords
                            where p.DischargeDate == null
                            select p;

                ListePatientHositaliser.DataContext = query;
            }
        }

        // ****************************************************//
        /// Insertion d'un Docteur dans le DB 
        // ****************************************************//
        // Rajout de validation a faire sur champ a entré
        // ****************************************************//
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

        // Genere collection de tous les Docteur
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

        //Genere collection des patient presentement admis a l'hopitale
        private ObservableCollection<ViewListeDesPatient> GetPatientAdmis()
        {
            ObservableCollection<ViewListeDesPatient> myPatientAdmisList = new ObservableCollection<ViewListeDesPatient>();
            try
            {
                var getlist = context.ViewListeDesPatients;
                foreach (var item in getlist)
                {
                    myPatientAdmisList.Add(item);
                }
            }
            catch
            {
                MessageBox.Show("Erreur de connection a la base de donnée");
            }
            return myPatientAdmisList;
        }

        // genere la collection de tous les patients de l'hopital
        private ObservableCollection<Patient> GetAllPatient()
        {
            ObservableCollection<Patient> myPatientList = new ObservableCollection<Patient>();
            try
            {
                var getlist = context.Patients;
                foreach (var item in getlist)
                {
                    myPatientList.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connection a la base de donnée " + ex.Message);
            }
            return myPatientList;
        }

        // ****************************************************//
        // Trait la modification d'un docteur                  //
        // ****************************************************//
        private void btnModifDocteur_Click(object sender, RoutedEventArgs e)
        {
            Doctor mySelectedDoc = (Doctor)ListeDesDocteurs.SelectedItem;
            
            var query = from doc in context.Doctors
                        where doc.DoctorID == mySelectedDoc.DoctorID
                        select doc;
            if (query != null)
            {
                if (query.Count() == 1)
                {
                    query.FirstOrDefault().DoctorID = tbAdminGereDocId.Text;
                    query.FirstOrDefault().FirstName = tbAdminGereDocPrenom.Text;
                    query.FirstOrDefault().LastName = tbAdminGereDocNom.Text;
                    query.FirstOrDefault().Specialty = ((Specialty)comboSpecialtyListe.SelectedItem).SpecialtyID;

                    try
                    {
                        //comit les changement et re-init le formulaire
                        context.SubmitChanges();

                        tbAdminGereDocId.Text = "";
                        tbAdminGereDocPrenom.Text = "";
                        tbAdminGereDocNom.Text = "";
                        comboSpecialtyListe.SelectedIndex = -1;

                        MessageBox.Show("Doctueur modifié");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                if (query.Count() > 1)
                {
                    MessageBox.Show("Erreur plus d'un docteur a avec le meme ID a été trouvé");
                }
            }
            else
            {
                MessageBox.Show("Vous devez selectionner un docteur a modifier");
            }
        }

        // ****************************************************//
        // Trait la suprresion d'un docteur                    //
        // ****************************************************//
        private void btnSuprimeDocteur_Click(object sender, RoutedEventArgs e)
        {
            Doctor mySelectedDoc = (Doctor)ListeDesDocteurs.SelectedItem;

            var query = from doc in context.Doctors
                        where doc.DoctorID == mySelectedDoc.DoctorID
                        select doc;

            if (query != null)
            {
                foreach (var item in query)
                {
                    context.Doctors.DeleteOnSubmit(item);
                }
            }
            try
            {
                context.SubmitChanges();
                MessageBox.Show("Le docteur a été supprimé");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ListeDesDocteurs.DataContext = GetAllDoctors();
            }
           
        }

        // ****************************************************//
        // Trait la l'insertion d'un patient                   //
        // ****************************************************//
        private void PreposerInserePatient_Click(object sender, RoutedEventArgs e)
        {
            // Validation des champs a faire, longueur pertinence etc etc
            Doctor mySelectedDoc = (Doctor)DocteurListe.SelectedItem;
            string LienParente = "";
            try
            {
                LienParente = ((ComboBoxItem)ComboLienParente.SelectedItem).Content.ToString();
            }
            catch
            {
                MessageBox.Show("Veuillez choisir un lien de parenté");
            }

            Patient myNewPatient = new Patient();
            myNewPatient.HealthNumber = tbPatienHealthNumber.Text.TrimEnd();
            myNewPatient.DateOfBirth = DOB.SelectedDate;
            myNewPatient.FirstName = tbPrenomPatient.Text.TrimEnd();
            myNewPatient.LastName = tbNomPatient.Text.TrimEnd();
            myNewPatient.Address = tbAddressePatient.Text.TrimEnd();
            myNewPatient.City = tbVillePatient.Text.TrimEnd();
            myNewPatient.Province = tbProvincePatient.Text.TrimEnd();
            myNewPatient.PostalCode = tbCPPatient.Text.TrimEnd();
            myNewPatient.Phone = tbtelPatient.Text.TrimEnd();
            myNewPatient.InsuranceCompany = tbCieAssurance.Text.TrimEnd();
            myNewPatient.InsuranceNumber = tbNumeroAssurance.Text.TrimEnd();
            myNewPatient.NextOfKin = tbContactPatient.Text.TrimEnd();
            myNewPatient.NextOfKinRelationship = LienParente.TrimEnd();
            myNewPatient.Doctor = mySelectedDoc.DoctorID.TrimEnd();

            
            try
            {
                context.Patients.InsertOnSubmit(myNewPatient);
                context.SubmitChanges();
                MessageBox.Show("Le patient a été inséré");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                myNewPatient = null;
                ListeDesPatients.DataContext = GetAllPatient();
            }
        }

        // ****************************************************//
        // Trait la la modification d'un patient               //
        // ****************************************************//
        private void PreposerModifiePatient_Click(object sender, RoutedEventArgs e)
        {
            //validation des champs a faire !! pareil comme Insere client
            var query = (from myModifiedPatient in context.Patients
                         where myModifiedPatient.HealthNumber == tbPatienHealthNumber.Text.TrimEnd()
                         select myModifiedPatient).First();

                Doctor mySelectedDoc = (Doctor)DocteurListe.SelectedItem;
                string LienParente = "";
                try
                {
                    LienParente = ((ComboBoxItem)ComboLienParente.SelectedItem).Content.ToString();
                }
                catch
                {
                    MessageBox.Show("Veuillez choisir un lien de parenté");
                }
                query.DateOfBirth = DOB.SelectedDate;
                query.FirstName = tbPrenomPatient.Text.TrimEnd();
                query.LastName = tbNomPatient.Text.TrimEnd();
                query.Address = tbAddressePatient.Text.TrimEnd();
                query.City = tbVillePatient.Text.TrimEnd();
                query.Province = tbProvincePatient.Text.TrimEnd();
                query.PostalCode = tbCPPatient.Text.TrimEnd();
                query.Phone = tbtelPatient.Text.TrimEnd();
                query.InsuranceCompany = tbCieAssurance.Text.TrimEnd();
                query.InsuranceNumber = tbNumeroAssurance.Text.TrimEnd();
                query.NextOfKin = tbContactPatient.Text.TrimEnd();
                query.NextOfKinRelationship = LienParente.TrimEnd();
                query.Doctor = mySelectedDoc.DoctorID.TrimEnd();

            try
            {
                context.SubmitChanges();
                MessageBox.Show("Le patient a été modifié");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ListeDesPatients.DataContext = GetAllPatient();
            }

        }

        // ****************************************************//
        // Trait la la supression d'un patient                 //
        // ****************************************************//
        private void PreposerSupprimePatient_Click(object sender, RoutedEventArgs e)
        {
            var query = (from myDeletedPatient in context.Patients
                         where myDeletedPatient.HealthNumber == tbPatienHealthNumber.Text.TrimEnd()
                         select myDeletedPatient).First();

            context.Patients.DeleteOnSubmit(query);

            try
            {
                context.SubmitChanges();
                MessageBox.Show("Le patient a été suprimé");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ListeDesPatients.DataContext = GetAllPatient();
            }

        }

        // function bypass pour remplir le combobox par programation (binding ne fonctionne pas)
        private void ListeDesPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Patient mySelectedPatient = (Patient)ListeDesPatients.SelectedItem;
            if (mySelectedPatient != null)
            {
                DocteurListe.SelectedItem = mySelectedPatient.Doctor1;
            }
        }

      }

    // ****************************************************//
    // Converter pour effacé les espaces dans les combobox //
    // ****************************************************//
    [ValueConversion(typeof(string), typeof(string))]
    public class StringTrimmingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString().Trim();
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}

