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
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Student loggedStudent;
        Admin loggedAdmin;
        bool isAdmin = false;
        public MainWindow(Student student)
        {
            this.loggedStudent = student;
            InitializeComponent();
            lblUser.Content = loggedStudent.nombre;
        }

        public MainWindow(Admin admin)
        {
            this.loggedAdmin = admin;
            InitializeComponent();
            lblUser.Content = loggedAdmin.nombre;
            isAdmin = true;
        }


        private void btPlace_Click(object sender, RoutedEventArgs e)
        {
            if(isAdmin)
            {
                Menu menu =  new Menu(loggedAdmin);
                menu.Show();
                this.Close();
            }
            else
            {
                Menu menu = new Menu(loggedStudent);
                menu.Show();
                this.Close();
            }

        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
           
                Login login = new Login();
                login.Show();
                this.Close();
            
        }
    }
}
