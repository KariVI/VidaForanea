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
        bool isAdmin = false;
        public Menu(Student student)
        {
            this.loggedStudent = student;
            InitializeComponent();
            lblUser.Content = loggedStudent.name;
            btRequest.Opacity = 0;
            btRequest.IsEnabled = false;
        }

        public Menu(Admin admin)
        {
            this.loggedAdmin = admin;
            InitializeComponent();
            lblUser.Content = loggedAdmin.nombre;
            isAdmin = true;
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            if (isAdmin)
            {
                MainWindow mainWindow= new MainWindow(loggedAdmin);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MainWindow mainWindow = new MainWindow(loggedStudent);
                mainWindow.Show();
                this.Close();
            }

        }

        private void btLeisure_Click(object sender, RoutedEventArgs e)
        {
            if (isAdmin)
            {
                PlaceList placeList = new PlaceList("Ocio",loggedAdmin);
                placeList.Show();
                this.Close();
            }
            else
            {
                PlaceList placeList = new PlaceList("Ocio", loggedStudent);
                placeList.Show();
                this.Close();
            }
        }

        private void btFood_Click(object sender, RoutedEventArgs e)
        {
            if (isAdmin)
            {
                PlaceList placeList = new PlaceList("Comida", loggedAdmin);
                placeList.Show();
                this.Close();
            }
            else
            {
                PlaceList placeList = new PlaceList("Comida", loggedStudent);
                placeList.Show();
                this.Close();
            }
        }

        private void btStationary_Click(object sender, RoutedEventArgs e)
        {
            if (isAdmin)
            {
                PlaceList placeList = new PlaceList("Papeleria", loggedAdmin);
                placeList.Show();
                this.Close();
            }
            else
            {
                PlaceList placeList = new PlaceList("Papeleria", loggedStudent);
                placeList.Show();
                this.Close();
            }
        }
    }
}
