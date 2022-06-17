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
using VidaForaneaCliente.Models;

namespace VidaForaneaCliente.Views
{
    /// <summary>
    /// Lógica de interacción para ForumAdmin.xaml
    /// </summary>
    public partial class ForumAdmin : Window
    {
        Student loggedStudent;
        MainWindow menu;
        public ForumAdmin(Student student, MainWindow menu)
        {
            InitializeComponent();
            this.loggedStudent = student;
            this.menu = menu;
            lblUser.Content = student.name;
        }

        private void BtExit_Click(object sender, RoutedEventArgs e)
        {
            menu.Show();
            this.Close();
        }

        private void BtStatistics_Click(object sender, RoutedEventArgs e)
        {
            ForumView forum = new ForumView(loggedStudent, menu, 1);
            forum.Show();
            this.Close();
        }

        private void BtNetwork_Click(object sender, RoutedEventArgs e)
        {
            ForumView forum = new ForumView(loggedStudent, menu, 3);
            forum.Show();
            this.Close();
        }

        private void BtTec_Click(object sender, RoutedEventArgs e)
        {
            ForumView forum = new ForumView(loggedStudent, menu, 2);
            forum.Show();
            this.Close();
        }

        private void BtSoftware_Click(object sender, RoutedEventArgs e)
        {
            ForumView forum = new ForumView(loggedStudent, menu, 4);
            forum.Show();
            this.Close();
        }
    }
}
