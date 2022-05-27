using System;
using System.Collections.Generic;
using System.IO;
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
    /// Lógica de interacción para PlaceList.xaml
    /// </summary>
    public partial class PlaceList : Window
    {
        Student loggedStudent;
        Admin loggedAdmin;
        bool isAdmin = false;
        string category;
        public ObservableCollection<String> PlacesCollection{ get; set; }
        public ListBox listPlaces { get { return lstbCategory; } set { lstbCategory = value; } }
        List<Place> places = new List<Place>();
        public PlaceList(String category, Student loggedStudent)
        {
            InitializeComponent();
            this.loggedStudent = loggedStudent;
            lbCategory.Content = category;
            lbUser.Content = loggedStudent.name;
            this.category = category;
            InitializePlaces(category);

        }
        public async void InitializePlaces(String category)
        {
            if (category.Equals("Papeleria"))
            {
                category = "Herramientas";
                imgPlace.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("..\\..\\Images\\stationary.png"), UriKind.Absolute));

            }
            else if (category.Equals("Ocio"))
            {
                imgPlace.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("..\\..\\Images\\leisure.png"), UriKind.Absolute));
            }
            else if (category.Equals("Comida"))
            {
                imgPlace.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("..\\..\\Images\\food.png"), UriKind.Absolute));
            }
            string status = "aprobado";
            if (category.Equals(""))
            {
                 status = "pendiente";
            }
            places = await Connection.GetPlacesByCategory(status, category);
               
            PlacesCollection = new ObservableCollection<String>();
           
            if (places != null)
            {
                PlacesCollection.Clear();
                foreach (var place in places)
                {
                    PlacesCollection.Add(place.name);
                }
            }
            
            listPlaces.ItemsSource = PlacesCollection;
           
        }
    
        public PlaceList(String category, Admin loggedAdmin)
        {
            InitializeComponent();
            isAdmin = true;
            this.loggedAdmin = loggedAdmin;
            lbCategory.Content = category;
            this.category = category;
            lbUser.Content = loggedAdmin.nombre;
            InitializePlaces(category);
        }
        private void btReturn_Click(object sender, RoutedEventArgs e)
        {
            if (isAdmin)
            {
                Menu menu = new Menu(loggedAdmin);
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

        private void lstbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String placeSelected= lstbCategory.SelectedValue.ToString();
            Place place = searchPlace(placeSelected);

            if (isAdmin)
            {
                ModifyPlace modifyPlace = new ModifyPlace(loggedAdmin, place,category);
                modifyPlace.Show();
                this.Close();
            }
            else
            {
                PlaceView placeview = new PlaceView(place, loggedStudent,category);
                placeview.Show();
                this.Close();
            }
            
        }

        private Place searchPlace(String name)
        {
            Place place = new Place();
            foreach (Place p in places)
            {
                if (p.name.Equals(name))
                {
                    place = p;
                }
            }
            return place;
        }

        public static BitmapImage ConvertirArrayAImagen(byte[] arrayDeImagen)
        {
            BitmapImage imagen = new BitmapImage();
            using (MemoryStream memStream = new MemoryStream(arrayDeImagen))
            {
                imagen.BeginInit();
                imagen.CacheOption = BitmapCacheOption.OnLoad;
                imagen.StreamSource = memStream;
                imagen.EndInit();
                imagen.Freeze();
            }
            return imagen;
        }
    }
}
