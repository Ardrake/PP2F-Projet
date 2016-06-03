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

namespace HLN_645_050537
{
    public partial class Facture : Window
    {
        public Facture(AdmissionRecord myfacture, string ladescitem, string lemontant)
        {
            InitializeComponent();

            string nomClient = myfacture.Patient.FirstName.TrimEnd() + " " + myfacture.Patient.LastName.TrimEnd();

            LabelNomPatient.Content = nomClient;
            LabelAddressPatient.Content = myfacture.Patient.Address;
            LabelVillePatient.Content = myfacture.Patient.City;

            lableDescFacture.Content = ladescitem;
            LabelMontantFacture.Content = lemontant;
        }
    }
}
