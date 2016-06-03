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

        //NHL_DataDataContext context = new NHL_DataDataContext("Data Source = CABANONS00171; Initial Catalog = NLH-645-050537; Integrated Security = True");
        NHL_DataDataContext context = new NHL_DataDataContext("Data Source = ANDRE-PC; Initial Catalog = NLH-645-050537; Integrated Security = True");

        public MenuPrincipale(String user)
        {
            InitializeComponent();

            if (user == "admin")
            {
                HideAll(); 
                ZoneAdmin.Visibility = Visibility.Visible;
            }
            if (user == "Docteur")
            {
                HideAll();
                ZoneDocteur.Visibility = Visibility.Visible;
            }
            if (user == "Infirmière")
            {
                HideAll();
                ZoneInfirmiere.Visibility = Visibility.Visible;
            }
            if (user == "Preposé")
            {
                HideAll();
                ZonePreposer.Visibility = Visibility.Visible;
            }
            if (user == "Cooke")
            {
                HideAll();
                SectionCooke.Visibility = Visibility.Visible;
            }
            sbUser.Text = user;
        }

        private void SwtichAdmin_Click(object sender, RoutedEventArgs e)
        {
            HideAll();
            SectionCooke.Visibility = Visibility.Visible;
            ZoneAdmin.Visibility = Visibility.Visible;
        }

        private void SwtichDocteur_Click(object sender, RoutedEventArgs e)
        {
            HideAll();
            SectionCooke.Visibility = Visibility.Visible;
            ZoneDocteur.Visibility = Visibility.Visible;
        }

        private void SwtichInfirmiere_Click(object sender, RoutedEventArgs e)
        {
            HideAll();
            SectionCooke.Visibility = Visibility.Visible;
            ZoneInfirmiere.Visibility = Visibility.Visible;
        }

        private void SwtichPreposer_Click(object sender, RoutedEventArgs e)
        {
            HideAll();
            SectionCooke.Visibility = Visibility.Visible;
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
            SectionCooke.Visibility = Visibility.Collapsed;
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
                        where a.Facture == false && a.DischargeDate != null
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
            AdmissionRecord mySelectedAfacture = (AdmissionRecord)ListAFacture.SelectedItem;
            String DescItem = "";
            String MontantFacture = "0.00";
            String DescTV = "";
            String DescTel = "";
            String DescTarif = " tarif chambre Standard";
            decimal extTelRate = 0;
            decimal extTVRate = 0;
            decimal extRoomRate = 0;
            decimal Total = 0;

            TimeSpan difference = (TimeSpan)(mySelectedAfacture.DischargeDate - mySelectedAfacture.AdmitDate);
            int totjours = Convert.ToInt32(difference.TotalDays);
            

            if (mySelectedAfacture.Extra.Phone == true)
            {
                decimal telrate = 0;
                
                var gettelrate = (from p in context.Extra_Rates
                                  where p.AmenityName == "PHONE"
                                  select p).First().DailyCost;

                DescTel = " - Option téléphone - " + gettelrate + "$ par jours ";
                telrate = Convert.ToDecimal(gettelrate);
                extTelRate = totjours * telrate;
            }

            if (mySelectedAfacture.Extra.TV == true)
            {
                decimal tvrate = 0;

                var gettvrate = (from p in context.Extra_Rates
                                  where p.AmenityName == "TV"
                                  select p).First().DailyCost;

                DescTV = " - Option télévision - " + gettvrate + "$ par jours ";
                tvrate = Convert.ToDecimal(gettvrate);
                extTVRate = totjours * tvrate;
            }

            if (mySelectedAfacture.Extra.SemiPriivate == true)
            {
                decimal roomrate = 0;

                var getroomrate = (from p in context.Extra_Rates
                                 where p.AmenityName == "SEMI"
                                 select p).First().DailyCost;

                roomrate = Convert.ToDecimal(getroomrate);
                extRoomRate = roomrate * totjours;

                DescTarif = " tarif chambre Semi-Privé " + getroomrate + "$";
            }

            if (mySelectedAfacture.Extra.Private == true)
            {
                decimal roomrate = 0;

                var getroomrate = (from p in context.Extra_Rates
                                   where p.AmenityName == "PRIVÉ"
                                   select p).First().DailyCost;

                roomrate = Convert.ToDecimal(getroomrate);
                extRoomRate = roomrate * totjours;

                DescTarif = " tarif chambre Privé " + getroomrate + "$";
            }

            DescItem = "Hospitalisation de " + totjours.ToString() + " jours" + DescTel + DescTV + DescTarif;

            Total = extRoomRate + extTelRate + extTVRate;
            MontantFacture = Total.ToString()+"$";

            Facture myfacture = new Facture(mySelectedAfacture, DescItem, MontantFacture);
            myfacture.ShowDialog();

            mySelectedAfacture.Facture = true;
            context.SubmitChanges();

            var query = from a in context.AdmissionRecords
                        where a.Facture == false && a.DischargeDate != null
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

        // Bouton re-impression facture
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            AdmissionRecord mySelectedAfacture = (AdmissionRecord)ListAFactureTermine.SelectedItem;
            String DescItem = "";
            String MontantFacture = "0.00";
            String DescTV = "";
            String DescTel = "";
            String DescTarif = " tarif chambre Standard";
            TimeSpan difference = new TimeSpan();
            decimal extTelRate = 0;
            decimal extTVRate = 0;
            decimal extRoomRate = 0;
            decimal Total = 0;

            difference = (TimeSpan)(mySelectedAfacture.DischargeDate - mySelectedAfacture.AdmitDate);
            int totjours = Convert.ToInt32(difference.TotalDays);


            if (mySelectedAfacture.Extra.Phone == true)
            {
                decimal telrate = 0;

                var gettelrate = (from p in context.Extra_Rates
                                  where p.AmenityName == "PHONE"
                                  select p).First().DailyCost;

                DescTel = " - Option téléphone - " + gettelrate + "$ par jours ";
                telrate = Convert.ToDecimal(gettelrate);
                extTelRate = totjours * telrate;
            }

            if (mySelectedAfacture.Extra.TV == true)
            {
                decimal tvrate = 0;

                var gettvrate = (from p in context.Extra_Rates
                                 where p.AmenityName == "TV"
                                 select p).First().DailyCost;

                DescTV = " - Option télévision - " + gettvrate + "$ par jours ";
                tvrate = Convert.ToDecimal(gettvrate);
                extTVRate = totjours * tvrate;
            }

            if (mySelectedAfacture.Extra.SemiPriivate == true)
            {
                decimal roomrate = 0;

                var getroomrate = (from p in context.Extra_Rates
                                   where p.AmenityName == "SEMI"
                                   select p).First().DailyCost;

                roomrate = Convert.ToDecimal(getroomrate);
                extRoomRate = roomrate * totjours;

                DescTarif = " tarif chambre Semi-Privé " + getroomrate + "$";
            }

            if (mySelectedAfacture.Extra.Private == true)
            {
                decimal roomrate = 0;

                var getroomrate = (from p in context.Extra_Rates
                                   where p.AmenityName == "PRIVÉ"
                                   select p).First().DailyCost;

                roomrate = Convert.ToDecimal(getroomrate);
                extRoomRate = roomrate * totjours;

                DescTarif = " tarif chambre Privé " + getroomrate + "$";
            }

            DescItem = "Hospitalisation de " + totjours.ToString() + " jours" + DescTel + DescTV + DescTarif;

            Total = extRoomRate + extTelRate + extTVRate;
            MontantFacture = Total.ToString() + "$";

            Facture myfacture = new Facture(mySelectedAfacture, DescItem, MontantFacture);
            myfacture.ShowDialog();

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
            bool ExtraTV = false;
            bool ExtraTel = false;
            bool semiPrive = false;
            bool prive = false;

            if (checkTelevision.IsChecked == true)
            {
                ExtraTV = true;
            }
            else
            { ExtraTV = false; }

            if (checkTelephone.IsChecked == true)
            {
                ExtraTel = true;
            }
            else
            { ExtraTel = false; }

            if (RateText.Text == "Semi-Privé")
            {
                semiPrive = true;
            }
            else
            { semiPrive = false; }

            if (RateText.Text == "Privé")
            {
                prive = true;
            }
            else
            { prive = false; }

            if (myHealthNumber != "" && myAdmissionBed != null)
            {
                try
                {
                    context.InsereAdmission(admissionId, myHealthNumber, myAdmissionBed.BedNumber, DateTime.Now, ExtraTV, ExtraTel, semiPrive, prive);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                MessageBox.Show("Admission complété" );
                myAdmissionBed = null;
                myHealthNumber = "";
                SelectionPatientAdmission.SelectedIndex = -1;
                SelectionPatientAile.SelectedIndex = -1;
                SelectionPatientChambre.SelectedIndex = -1;
                checkTelephone.IsChecked = false;
                checkTelevision.IsChecked = false;
            }
            else
            {
                MessageBox.Show("Veuilles faire les selection");
            }
        }

        private void SelectionPatientChambre_DropDownClosed(object sender, EventArgs e)
        {
            Bed myAdmissionBed = (Bed)SelectionPatientChambre.SelectedValue;
            if(myAdmissionBed != null)
            {
                int totChambreStandard = (from b in context.Beds
                                          where b.WardName == myAdmissionBed.WardName &&
                                                b.BedType == "STANDARD" &&
                                                b.Occupied == false
                                          select b).Count();

                int totChambreSemiPrive = (from b in context.Beds
                                           where b.WardName == myAdmissionBed.WardName &&
                                                 b.BedType == "SEMI-PRIVÉ" &&
                                                 b.Occupied == false
                                           select b).Count();

                if (myAdmissionBed.BedType.TrimEnd() == "STANDARD")
                {
                    RateText.Text = "Standard";
                }
                if (myAdmissionBed.BedType.TrimEnd() == "SEMI-PRIVÉ")
                {
                    RateText.Text = "Semi-Privé";
                }
                if (myAdmissionBed.BedType.TrimEnd() == "PRIVÉ")
                {
                    RateText.Text = "Privé";
                }
                // Override frais aucune chambre standard de dispo
                if (myAdmissionBed.BedType.TrimEnd() == "SEMI-PRIVÉ" && totChambreStandard == 0)
                {
                    RateText.Text = "Standard";
                }
                // Override frais aucune chambre semi-privé de dispo
                if (myAdmissionBed.BedType.TrimEnd() == "PRIVÉ" && totChambreSemiPrive == 0)
                {
                    RateText.Text = "Semi-Privé";
                }
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
        // Traite la modification d'un docteur                  //
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
                        MessageBox.Show("Docteur modifié");
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
        // Traite la suprresion d'un docteur                    //
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
        // Traite la l'insertion d'un patient                   //
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
        // Traite la la modification d'un patient               //
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
        // Traite la la supression d'un patient // NON ACTIF   //
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

        private void quitter_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.Shutdown();
        }

        private void aPropos_Click(object sender, RoutedEventArgs e)
        {
            APropos fenetreAPropos = new APropos();
            fenetreAPropos.ShowDialog();
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

