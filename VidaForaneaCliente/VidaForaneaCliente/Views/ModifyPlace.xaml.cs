using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace VidaForaneaCliente.Views
{
    /// <summary>
    /// Lógica de interacción para ModifyPlace.xaml
    /// </summary>
    public partial class ModifyPlace : Window
    {
        Student loggedStudent;
        Place place;
        string category;
        string imageSource = "NO";
        public ModifyPlace(Student loggedStudent, Place place, string category)
        {
            InitializeComponent();
            this.loggedStudent = loggedStudent;
            this.place = place;
            this.category = category;
            InitializePlace();
        }

        private void InitializePlace()
        {
            tbName.Text = place.name;
            tbLocation.Text = place.address;
            tbServices.Text = place.services;
            image.Source = Utils.ConvertBytesToImage(Convert.FromBase64String(place.image));
            if (place.type_place == "Papelería")
            {
                cbType.SelectedIndex = 0;
            } else if (place.type_place == "Comida")
            {
                cbType.SelectedIndex = 1;
            } else
            {
                cbType.SelectedIndex = 2;
            }
            if (place.status == StatusPlace.aprobado)
            {
                cbStatus.SelectedIndex = 0;
            }
            else
            {
                cbStatus.SelectedIndex = 1;
            } 
        }

        private void BtBack_Click(object sender, RoutedEventArgs e)
        {
            PlaceList placeList = new PlaceList(category, loggedStudent);
            placeList.Show();
            this.Close();
        }

        private async void BtSave_Click(object sender, RoutedEventArgs e)
        {
            string time = GenerateScheduleString();
            if (String.IsNullOrWhiteSpace(cbType.Text) || String.IsNullOrWhiteSpace(tbName.Text) || String.IsNullOrWhiteSpace(tbLocation.Text) || String.IsNullOrWhiteSpace(time))
            {
                MessageBox.Show("Existen campos vacíos, por favor revise los campos", "Campos vacíos", MessageBoxButton.OK);
            }
            else
            {
                string imagePlace;
                if (imageSource == "NO")
                {
                    imagePlace = place.image;
                } else
                {
                    imagePlace = Convert.ToBase64String(Utils.ConvertImageToBytes(imageSource));
                }
                StatusPlace newStatus;
                if (cbStatus.SelectedIndex == 0)
                {
                    newStatus = StatusPlace.aprobado;
                } else
                {
                    newStatus = StatusPlace.pendiente;

                }
                Place placeModified = new Place()
                {
                    name = tbName.Text,
                    address = tbLocation.Text,
                    services = tbServices.Text,
                    schedule = time,
                    status = newStatus,
                    type_place = cbType.Text,
                    image = imagePlace
                    //image.Source = Utils.ConvertBytesToImage(Convert.FromBase64String(String recibida del servidor)):

                };
                bool respuesta = await Connection.PutPlace(placeModified,place.id);
                if (Connection.latestStatusCode == HttpStatusCode.OK)
                {
                    MessageBox.Show("Se ha modificado la solicitud del lugar", "Solicitud regismodificadatrada", MessageBoxButton.OK);
                    BtBack_Click(new object(), new RoutedEventArgs());
                }
                else if (Connection.latestStatusCode == HttpStatusCode.BadRequest)
                {
                    MessageBox.Show("Error en el registro de la solicitud", "Solciitud no registrada", MessageBoxButton.OK);
                }
                Console.WriteLine(Connection.latestStatusCode);
            }

        }

        private string GenerateScheduleString()
        {
            return place.schedule;
        }

        private void BtSelectImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".jpg";
            dialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                var bitmap = new BitmapImage();
                imageSource = dialog.FileName;
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(dialog.FileName);
                bitmap.DecodePixelHeight = 200;
                bitmap.EndInit();
                image.Source = bitmap;
            }

        }
    }
}
