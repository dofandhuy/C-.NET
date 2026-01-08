using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Prn212TrialContext context = new Prn212TrialContext();
        public MainWindow()
        {
            InitializeComponent();
            LoadEmployee();
        }
        private void LoadEmployee()
        {
            var list = context.Employees.ToList();
            dgvDisplay.ItemsSource = list;
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgvDisplay_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var obj = dgvDisplay.SelectedItem as Employee;
            if (obj != null)
            {
                txtID.Text = obj.Id.ToString();
                txtName.Text = obj.Name;
                if (obj.Gender.Equals("Male")) 
                {
                    rbnMale.IsChecked = true;
                }
                else 
                {
                    rbnFemale.IsChecked = true;
                }
                dpkDob.SelectedDate = DateTime.Parse(obj.Dob.ToString());
                txtPhone.Text = obj.Phone.ToString();
                txtIDNumber.Text = obj.Idnumber.ToString();

            }

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}