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
using VidaForaneaCliente.Models;

namespace VidaForaneaCliente.Views
{
    /// <summary>
    /// Lógica de interacción para Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        Student loggedStudent;
        Admin loggedAdmin;
        public Menu(Student student)
        {
            this.loggedStudent = student;
            InitializeComponent();
            lblUser.Content = loggedStudent.nombre;
            btRequest.Opacity = 0;
            btRequest.IsEnabled = false;
        }

        public Menu(Admin admin)
        {
            this.loggedAdmin = admin;
            InitializeComponent();
            lblUser.Content = loggedAdmin.nombre;

        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
