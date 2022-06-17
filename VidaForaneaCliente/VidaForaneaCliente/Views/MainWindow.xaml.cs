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
        public MainWindow(Student student)
        {
            this.loggedStudent = student;
            InitializeComponent();
            lblUser.Content = loggedStudent.name;
        }


        private void BtPlace_Click(object sender, RoutedEventArgs e)
        {
           
                Menu menu = new Menu(loggedStudent);
                menu.Show();
                this.Close();
            

        }

        private void BtExit_Click(object sender, RoutedEventArgs e)
        {
           
                Login login = new Login();
                login.Show();
                this.Close();
            
        }

        private void BtForo_Click(object sender, RoutedEventArgs e)
        {
            if (loggedStudent.rol == "administrador")
            {
                ForumAdmin forum = new ForumAdmin(loggedStudent, this);
                forum.Show();
                this.Hide();
            }
            else
            {
                ForumView forum = new ForumView(loggedStudent, this);
                forum.Show();
                this.Hide();
            }
        }
    }
}
