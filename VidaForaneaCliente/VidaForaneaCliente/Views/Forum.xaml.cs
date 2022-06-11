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
    /// Lógica de interacción para Forum.xaml
    /// </summary>

    public partial class ForumView : Window
    {
        Student loggedStudent;
        MainWindow menu;
        int idDegree;
        bool isAdmin;
        List<Comment> comments;

        public TextBox ContenedorDelMensaje
        {
            get { return ContenidoDelMensaje; }
            set { ContenidoDelMensaje = value; }
        }

        public ForumView(Student student, MainWindow menu)
        {
            InitializeComponent();
            this.loggedStudent = student;
            lblForum.Content = this.loggedStudent.degree;
            this.menu = menu;
            lblUser.Content = student.name;
            isAdmin = false;
            InitializeComments();
        }

        public ForumView(Student student, MainWindow menu, int idDegree)
        {
            InitializeComponent();
            this.idDegree = idDegree;
            isAdmin = true;
            this.loggedStudent = student;
            lblForum.Content = this.loggedStudent.name;
            this.menu = menu;
            lblUser.Content = student.name;
            btSend.IsEnabled = false;
            ContenidoDelMensaje.IsEnabled = false;
            InitializeComments();
        }

        private async void InitializeComments()
        {
            PlantillaMensaje.Items.Clear();
            if (!isAdmin)
            {
                if (loggedStudent.degree == "Lic. Estadística")
                {
                    idDegree = (int)degreeId.estadistica + 1;
                }
                else if (loggedStudent.degree == "Lic. en Tecnologías Computacionales")
                {
                    idDegree = (int)degreeId.tecnologiasComputacionales + 1;
                }
                else if (loggedStudent.degree == "Lic. en Redes y Servicios de Cómputo")
                {
                    idDegree = (int)degreeId.redes + 1;
                }
                else
                {
                    idDegree = (int)degreeId.software + 1;
                }
            }
            Forum forum = new Forum() { Id = idDegree };
            comments = await Connection.GetCommentsByForum(forum);
            foreach (var comment in comments)
            {
                string name;
                Student student = await Connection.GetStudentByMatricula(comment.student);
                name = student.name;
               
                if (isAdmin || loggedStudent.enrollment == comment.student)
                {
                    PlantillaMensaje.Items.Add(new { Posicion = "Center", FondoElemento = "White", FondoCabecera = "#e6dded", Nombre = name, idComentario = comment.Id, TiempoDeEnvio = comment.date, MensajeEnviado = comment.description });

                } else
                {
                    PlantillaMensaje.Items.Add(new { Posicion = "Center", FondoElemento = "White", FondoCabecera = "#e6dded", Nombre = name, TiempoDeEnvio = comment.date, MensajeEnviado = comment.description });
                   

                }
                ContenidoDelMensaje.Clear();
            }
        }

        private async void btSend_Click(object sender, RoutedEventArgs e)
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
                    Comment comment = new Comment();
                    comment.id_forum = idDegree;
                    comment.student = loggedStudent.enrollment;
                    comment.date = DateTime.Now.Date.ToString();
                    comment.hour = DateTime.Now.Hour.ToString();
                    comment.description = mensajeFinal;
                    bool correcto = await Connection.PostComment(new Forum() { Id = idDegree }, comment); ;
                    if (Connection.latestStatusCode == HttpStatusCode.Created)
                    {
                        PlantillaMensaje.Items.Add(new { Posicion = "Center", FondoElemento = "White", FondoCabecera = "#e6dded", Nombre = loggedStudent.name, TiempoDeEnvio = comment.date, MensajeEnviado = mensaje });
                        ContenidoDelMensaje.Clear();
                    }
                }
                catch (Exception exception) when (exception is TimeoutException)
                {

                    this.Close();
                }
            }

        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            menu.Show();
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            btBack_Click(new object(), new RoutedEventArgs());
        }

        private async void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Label label = (Label)sender;
            if (label.Content != null)
            {
                try
            {
                Comment comment = new Comment();
                comment.id_forum = idDegree;
                String id = label.Content.ToString();
                comment.Id = Int32.Parse(id);
                bool correcto = await Connection.DeleteComment(new Forum() { Id = idDegree }, comment);
                if (Connection.latestStatusCode == HttpStatusCode.OK)
                {
                    MessageBox.Show("Comentario del foro eliminado", "Comentario eliminado", MessageBoxButton.OK);
                    InitializeComments();
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
