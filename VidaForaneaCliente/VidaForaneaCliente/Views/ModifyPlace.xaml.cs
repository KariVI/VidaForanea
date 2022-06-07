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

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            PlaceList placeList = new PlaceList(category, loggedStudent);
            placeList.Show();
            this.Close();
        }

        private async void btSave_Click(object sender, RoutedEventArgs e)
        {
            string time = "ok";
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
                    btBack_Click(new object(), new RoutedEventArgs());
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
            string time = "";
            if (cbMonday.IsChecked == true && !String.IsNullOrWhiteSpace(tpMondayStart.Value.ToString()) && !String.IsNullOrWhiteSpace(tpMondayEnd.Value.ToString()))
            {
                time += "Lunes: " + tpMondayStart.Value.ToString().Substring(11, 14) + " - " + tpMondayEnd.Value.ToString().Substring(11, 14) + Environment.NewLine;
            }
            if (cbTuesday.IsChecked == true && !String.IsNullOrWhiteSpace(tpTuesdayStart.Value.ToString()) && !String.IsNullOrWhiteSpace(tpTuesdayEnd.Value.ToString()))
            {
                time += "Martes: " + tpTuesdayStart.Value.ToString().Substring(11, 14) + " - " + tpTuesdayEnd.Value.ToString().Substring(11, 14) + Environment.NewLine;
            }
            if (cbWednesday.IsChecked == true && !String.IsNullOrWhiteSpace(tpWednesdayStart.Value.ToString()) && !String.IsNullOrWhiteSpace(tpWednesdayEnd.Value.ToString()))
            {
                time += "Miércoles: " + tpWednesdayStart.Value.ToString().Substring(11, 14) + " - " + tpWednesdayEnd.Value.ToString().Substring(11, 14) + Environment.NewLine;
            }
            if (cbThursday.IsChecked == true && !String.IsNullOrWhiteSpace(tpThursdayStart.Value.ToString()) && !String.IsNullOrWhiteSpace(tpThursdayEnd.Value.ToString()))
            {
                time += "Jueves: " + tpThursdayStart.Value.ToString().Substring(11, 14) + " - " + tpThursdayEnd.Value.ToString().Substring(11, 14) + Environment.NewLine;
            }
            if (cbFriday.IsChecked == true && !String.IsNullOrWhiteSpace(tpFridayEnd.Value.ToString()) && !String.IsNullOrWhiteSpace(tpFridayStart.Value.ToString()))
            {
                time += "Viernes: " + tpFridayStart.Value.ToString().Substring(11, 14) + " - " + tpFridayEnd.Value.ToString().Substring(11, 14) + Environment.NewLine;
            }
            if (cbSaturday.IsChecked == true && !String.IsNullOrWhiteSpace(tpSaturdayStart.Value.ToString()) && !String.IsNullOrWhiteSpace(tpSaturdayEnd.Value.ToString()))
            {
                time += "Sábado: " + tpSaturdayStart.Value.ToString().Substring(11, 14) + " - " + tpSaturdayEnd.Value.ToString().Substring(11, 14) + Environment.NewLine;
            }
            if (cbSunday.IsChecked == true && !String.IsNullOrWhiteSpace(tpSundayStart.Value.ToString()) && !String.IsNullOrWhiteSpace(tpSundayEnd.Value.ToString()))
            {
                time += "Domingo: " + tpSundayStart.Value.ToString().Substring(11, 14) + " - " + tpSundayEnd.Value.ToString().Substring(11, 14) + Environment.NewLine;
            }
            return time;
        }

        private void btSelectImage_Click(object sender, RoutedEventArgs e)
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
