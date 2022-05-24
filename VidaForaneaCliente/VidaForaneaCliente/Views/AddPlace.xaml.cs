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
    /// Lógica de interacción para AddPlace.xaml
    /// </summary>
    public partial class AddPlace : Window
    {
        Student student;
        public AddPlace(Student student)
        {
            InitializeComponent();
            this.student = student;
            //lbUser.Content = student.name;
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            PlaceList placeList = new PlaceList();
            placeList.Show();
            this.Close();
        }

        private async void btSend_Click(object sender, RoutedEventArgs e)
        {
            string time = GenerateScheduleString();
            if (String.IsNullOrWhiteSpace(cbType.Text) || String.IsNullOrWhiteSpace(tbName.Text) || String.IsNullOrWhiteSpace(tbLocation.Text) || String.IsNullOrWhiteSpace(time))
            {
                MessageBox.Show("Existen campos vacíos, por favor revise los campos", "Campos vacíos", MessageBoxButton.OK);
            } else
            {
                Place place = new Place()
                {
                    name = tbName.Text,
                    address = tbLocation.Text,
                    services = tbServices.Text,
                    schedule = time,
                    status = StatusPlace.pendiente,
                    type_place = cbType.Text

                };
                bool correcto = await Connection.PostPlace(place);
                if (Connection.latestStatusCode == HttpStatusCode.Created)
                {
                    MessageBox.Show("Se ha registrado la solicitud del lugar", "Solicitud registrada", MessageBoxButton.OK);
                    MainWindow mainWindow = new MainWindow(student);
                    mainWindow.Show();
                    this.Close();
                }
                else if (Connection.latestStatusCode == HttpStatusCode.BadRequest)
                {
                    MessageBox.Show("Error en el registro de la solicitud", "Solciitud no registrada", MessageBoxButton.OK);
                }
            }
        }

        private string GenerateScheduleString()
        {
            string time = "";
            if (cbMonday.IsChecked == true && !String.IsNullOrWhiteSpace(tpMondayStart.Value.ToString()) && !String.IsNullOrWhiteSpace(tpMondayEnd.Value.ToString()))
            {
                time += "Lunes: " + tpMondayStart.Value.ToString().Substring(11,14) + " - " + tpMondayEnd.Value.ToString().Substring(11, 14) + Environment.NewLine;
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
    }
}
