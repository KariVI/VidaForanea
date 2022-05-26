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
        String category;
        Place place;
        public TextBox ContenedorDelMensaje
        {
            get { return ContenidoDelMensaje; }
            set { ContenidoDelMensaje = value; }
        }
        public PlaceView(Place place, Admin loggedAdmin,String category)
        {
            InitializeComponent();
            this.loggedAdmin = loggedAdmin;
            lbUser.Content = loggedAdmin.nombre;
            this.category = category;
            this.place = place;
            isAdmin = true;
            intializePlace();
            
        }
        public PlaceView(Place place, Student loggedStudent,String category)
        {
            InitializeComponent();
            this.loggedStudent = loggedStudent;
            this.category = category;
            lbUser.Content = loggedStudent.name;
            btUpdate.IsEnabled = false;
            btUpdate.Opacity = 0;
            this.place = place;
            intializePlace();
        }
        
        private async void intializePlace( )
        {
            lbSchedule.Text = place.schedule;
            List<Opinion> opinions = await Connection.GetOpinionsByPlace(place);
            foreach(var opinion in opinions){
                string name;
                if (((opinion.user).Substring(0, 2).ToUpper() == "ZS"))
                {
                    Student student = await Connection.GetStudentByMatricula(opinion.user);
                    name = student.name;
                }
                else
                {
                    name = "Administrador";
                }

                PlantillaMensaje.Items.Add(new { Posicion = "Right", FondoElemento = "White", FondoCabecera = "#7f4ca5", Nombre = name, TiempoDeEnvio = opinion.date, MensajeEnviado = opinion.description, Puntuacion = "Puntuacion: " + opinion.score });
                ContenidoDelMensaje.Clear();
            }
            cbStar.SelectedIndex = 0;
            lbName.Content = place.name;
            lbLocate.Text = place.address;
            lbServices.Text = place.services;
            imgPlace.Source = Utils.ConvertBytesToImage(Convert.FromBase64String(place.image));
        }

        private void btReturn_Click(object sender, RoutedEventArgs e)
        {
            if (isAdmin)
            {
                PlaceList placeList = new PlaceList(category, loggedAdmin);
               
                placeList.Show();
                this.Close();
            }
            else
            {
                PlaceList placeList = new PlaceList(category, loggedStudent);
                placeList.Show();
                this.Close();
            }
            

        }

        private async void BotonEnviar_Click(object sender, RoutedEventArgs e)
        {
            ScrollerContenido.ScrollToBottom();
            if (!string.IsNullOrWhiteSpace(ContenidoDelMensaje.Text))
            {
                string mensajeFinal;
                if (ContenedorDelMensaje.Text.Length > 36)
                {
                    int tamanioMensaje = ContenedorDelMensaje.Text.Length;
                    mensajeFinal = ContenedorDelMensaje.Text.Substring(0, 30);
                    mensajeFinal += System.Environment.NewLine;
                    mensajeFinal += ContenedorDelMensaje.Text.Substring(31, tamanioMensaje - 32);

                }
                else
                {
                    mensajeFinal = ContenedorDelMensaje.Text;
                }
                try
                {
                    string mensaje = mensajeFinal;
                    ComboBoxItem comboItem = (ComboBoxItem)cbStar.SelectedItem;
                    string puntuacion = comboItem.Content.ToString();
                    Opinion opinion = new Opinion();
                    opinion.id_place = place.id; 
                    if (isAdmin)
                    {
                        opinion.user = loggedAdmin.usuario;
                    }
                    else
                    {
                        opinion.user = loggedStudent.enrollment;
                    }
                        
                    opinion.date = DateTime.Now.Date.ToString();
                    opinion.hour = DateTime.Now.Hour.ToString();
                    opinion.description = mensajeFinal;
                    opinion.score = Convert.ToInt32(puntuacion);
                    bool correcto = await Connection.PostOpinion(place,opinion);
                    if (Connection.latestStatusCode == HttpStatusCode.Created)
                    {
                         PlantillaMensaje.Items.Add(new { Posicion = "Right", FondoElemento = "White", FondoCabecera = "#7f4ca5", Nombre = opinion.user, TiempoDeEnvio = DateTime.Now, MensajeEnviado = mensaje, Puntuacion = "Puntuacion: " + puntuacion });
                        ContenidoDelMensaje.Clear();
                    }
                    else if (Connection.latestStatusCode == HttpStatusCode.BadRequest)
                    {
                        MessageBox.Show("Ya existe una opinion", "Ya existe una opinionn", MessageBoxButton.OK);
                    }

                    
                        

                }
                catch (Exception exception) when (exception is TimeoutException)
                {
    
                    this.Close();
                }
            }
        }
    }
}
