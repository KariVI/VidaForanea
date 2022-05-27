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
        Admin admin;
        MainWindow menu;
        public ForumAdmin(Admin admin, MainWindow menu)
        {
            InitializeComponent();
            this.admin = admin;
            this.menu = menu;
            lblUser.Content = admin.nombre;
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            menu.Show();
            this.Close();
        }

        private void btStatistics_Click(object sender, RoutedEventArgs e)
        {
            ForumView forum = new ForumView(admin, menu, 1);
            forum.Show();
            this.Close();
        }

        private void btNetwork_Click(object sender, RoutedEventArgs e)
        {
            ForumView forum = new ForumView(admin, menu, 3);
            forum.Show();
            this.Close();
        }

        private void btTec_Click(object sender, RoutedEventArgs e)
        {
            ForumView forum = new ForumView(admin, menu, 2);
            forum.Show();
            this.Close();
        }

        private void btSoftware_Click(object sender, RoutedEventArgs e)
        {
            ForumView forum = new ForumView(admin, menu, 4);
            forum.Show();
            this.Close();
        }
    }
}
