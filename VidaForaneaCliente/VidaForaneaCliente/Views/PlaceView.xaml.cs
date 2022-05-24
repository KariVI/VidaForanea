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
using VidaForaneaCliente.ServerConnection;
using System.Net;
using System.Collections.ObjectModel;

namespace VidaForaneaCliente.Views
{
    /// <summary>
    /// Lógica de interacción para PlaceView.xaml
    /// </summary>
    public partial class PlaceView : Window
    {
        Student loggedStudent;
        Admin loggedAdmin;
        bool isAdmin = false;
        public PlaceView(Place place, Admin loggedAdmin)
        {
            InitializeComponent();
            this.loggedAdmin = loggedAdmin;
            lbUser.Content = loggedAdmin.nombre;
            intializePlace(place);
        }
        public PlaceView(Place place, Student loggedStudent)
        {
            InitializeComponent();
            this.loggedStudent = loggedStudent;
            lbUser.Content = loggedStudent.name;
            intializePlace(place);
        }
        
        private void intializePlace(Place place)
        {
            lbName.Content = place.name;
        }
    }
}
