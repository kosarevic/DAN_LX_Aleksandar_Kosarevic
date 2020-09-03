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

namespace Zadatak_1
{
    /// <summary>
    /// Interaction logic for AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        public AddEmployeeWindow()
        {
            InitializeComponent();
        }

        private void LostFocus_TextBox(object sender, RoutedEventArgs e)
        {
        }

        private void Btn_Ok(object sender, RoutedEventArgs e)
        {
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            MainWindow MainPage = new MainWindow();
            MainPage.Show();
            this.Close();
        }
    }
}
