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
using System.Windows.Forms;
using TextBox = System.Windows.Controls.TextBox;
using MessageBox = System.Windows.Forms.MessageBox;
using Label = System.Windows.Controls.Label;

namespace VidaForaneaCliente.Views
{
    /// <summary>
    /// Lógica de interacción para PlaceView.xaml
    /// </summary>
    public partial class PlaceView : Window
    {
        Student loggedStudent;
        String category;
        Place place;
        public TextBox ContenedorDelMensaje
        {
            get { return ContenidoDelMensaje; }
            set { ContenidoDelMensaje = value; }
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
        
        private async void intializeOpinions( )
        {
            PlantillaMensaje.Items.Clear();
            List<Opinion> opinions = await Connection.GetOpinionsByPlace(place);
            foreach (var opinion in opinions) {
                string name;
                string puntuacion = "";
                if (opinion.score == 5)
                {
                    puntuacion = "✪✪✪✪✪";
                }
                if (opinion.score == 4)
                {
                    puntuacion = " ✪✪✪✪";
                }
                if (opinion.score == 3)
                {
                    puntuacion = "  ✪✪✪";
                }
                if (opinion.score == 2)
                {
                    puntuacion = "   ✪✪";
                }
                if (opinion.score == 1)
                {
                    puntuacion = "     ✪";
                }
                if (((opinion.student).Substring(0, 2).ToUpper() == "ZS"))
                {
                    Student student = await Connection.GetStudentByMatricula(opinion.student);
                    name = student.name;
                }
                else
                {
                    name = "Administrador";
                }
                string user;
               
                    user = loggedStudent.enrollment;

                    
                
                if (loggedStudent.rol == "administrador" || opinion.student == user) {
                    PlantillaMensaje.Items.Add(new { Posicion = "Right", FondoElemento = "White", FondoCabecera = "#7f4ca5", Nombre = name, TiempoDeEnvio = opinion.date, MensajeEnviado = opinion.description, Puntuacion = "Puntuacion: " + puntuacion, idOpinion = opinion.id });
                }
                else
                {
                    PlantillaMensaje.Items.Add(new { Posicion = "Right", FondoElemento = "White", FondoCabecera = "#7f4ca5", Nombre = name, TiempoDeEnvio = opinion.date, MensajeEnviado = opinion.description, Puntuacion = "Puntuacion: " + puntuacion });
                }
                ContenidoDelMensaje.Clear();
            }
            
        }

        private async void intializePlace()
        {
            lbSchedule.Text = place.schedule;
            cbStar.SelectedIndex = 0;
            lbName.Content = place.name;
            lbLocate.Text = place.address;
            lbServices.Text = place.services;
            imgPlace.Source = Utils.ConvertBytesToImage(Convert.FromBase64String(place.image));
            intializeOpinions();
        }
        private void btReturn_Click(object sender, RoutedEventArgs e)
        {
                PlaceList placeList = new PlaceList(category, loggedStudent);
                placeList.Show();
                this.Close();
            
            

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
                   
                        opinion.student = loggedStudent.enrollment;
                    
                        
                    opinion.date = DateTime.Now.Date.ToString();
                    opinion.hour = DateTime.Now.Hour.ToString();
                    opinion.description = mensajeFinal;
                    int puntuacionNum = 0;
                    if (puntuacion.Length == 5)
                    {
                        puntuacionNum = 5;
                    }
                    if (puntuacion.Length == 4)
                    {
                        puntuacionNum = 4;
                    }
                    if (puntuacion.Length == 3)
                    {
                        puntuacionNum = 3;
                    }
                    if (puntuacion.Length == 2)
                    {
                        puntuacionNum = 2;
                    }
                    if (puntuacion.Length == 1)
                    {
                        puntuacionNum = 1;
                    }
                    opinion.score = puntuacionNum;
                    bool correcto = await Connection.PostOpinion(place,opinion);
                    if (Connection.latestStatusCode == HttpStatusCode.OK)
                    {
                        intializeOpinions();
                        ContenidoDelMensaje.Clear();
                    }
                    else if (Connection.latestStatusCode == HttpStatusCode.BadRequest)
                    {
                        MessageBox.Show("Ya existe una opinion", "Ya existe una opinionn", MessageBoxButtons.OK);
                    }

                    
                        

                }
                catch (Exception exception) when (exception is TimeoutException)
                {
    
                    this.Close();
                }
            }
        }

        private async void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Label label = (Label)sender;
            if(label.Content != null)
            {

                try
                {
                    Opinion opinion = new Opinion();
                    String id = label.Content.ToString();
                    opinion.id = Int32.Parse(id);


                    var result = MessageBox.Show("Eliminar opinion", "Eliminar opinion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        bool correcto = await Connection.DeleteOpinion(place, opinion);
                        if (Connection.latestStatusCode == HttpStatusCode.NoContent)
                        {
                            PlantillaMensaje.Items.Clear();

                            intializeOpinions();
                        }
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
