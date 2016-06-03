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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HLN_645_050537
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int nbressai = 0;
        //NHL_DataDataContext context = new NHL_DataDataContext("Data Source = CABANONS00171; Initial Catalog = NLH-645-050537; Integrated Security = True");
        NHL_DataDataContext context = new NHL_DataDataContext("Data Source = ANDRE-PC; Initial Catalog = NLH-645-050537; Integrated Security = True");

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            var query = from u in context.Logins
                        where u.UserName == USERNAME.Text
                        select u;
            if (query.Count() > 0)
            {
                Login myUserLogin = query.First();

                if (Password.Password.TrimEnd() == myUserLogin.Password.TrimEnd())
                {
                    this.Hide();
                    MenuPrincipale myMenuPrincipale = new MenuPrincipale(myUserLogin.UserName.TrimEnd());
                    myMenuPrincipale.Show();
                }
                else
                {
                    MessageBox.Show("Erreur usager ou mot de passe", "Login Incorrect", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    nbressai += 1;
                }

                if (nbressai == 3)
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Usager non trouvé");
            }
            

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
