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
        bool isAdmin = false;
        public Menu(Student student)
        {
            this.loggedStudent = student;
            InitializeComponent();
            lblUser.Content = loggedStudent.name;
            if (student.rol == "administrador")
            {
                isAdmin = true;
            }
            else
            {
                btRequest.Opacity = 0;
                btRequest.IsEnabled = false;
                btReport.Opacity = 0;
                btReport.IsEnabled = false;
            } 
        }

       

        private void BtExit_Click(object sender, RoutedEventArgs e)
        {
            
                MainWindow mainWindow = new MainWindow(loggedStudent);
                mainWindow.Show();
                this.Close();
            

        }


        private void BtLeisure_Click(object sender, RoutedEventArgs e)
        {
            
                PlaceList placeList = new PlaceList("Ocio", loggedStudent);
                placeList.Show();
                this.Close();
            
        }

        private void BtFood_Click(object sender, RoutedEventArgs e)
        {
                PlaceList placeList = new PlaceList("Comida", loggedStudent);
                placeList.Show();
                this.Close();
            
        }

        private void BtStationary_Click(object sender, RoutedEventArgs e)
        {
          
                PlaceList placeList = new PlaceList("Papeleria", loggedStudent);
                placeList.Show();
                this.Close();
            
        }
        private void BtAddPlace_Click(object sender, RoutedEventArgs e)
        {
           
                AddPlace addPlace = new AddPlace(this, loggedStudent);
                addPlace.Show();
                this.Hide();

           
        }

        private void BtRequest_Click(object sender, RoutedEventArgs e)
        {
            PlaceList placeList = new PlaceList("", loggedStudent);
            placeList.Show();
            this.Close();
        }

        private void BtReport_Click(object sender, RoutedEventArgs e)
        {
            GenerateReport generateReport = new GenerateReport(loggedStudent);
            generateReport.Show();
            this.Close();
        }
    }
}
