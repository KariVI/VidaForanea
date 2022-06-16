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
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        bool connected = true;
        public Login()
        {
            Connection.token = null;
            Connection.CleanConnection();
            InitializeComponent();
        }

        private async void btLogin_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtUsuario.Text)) {
                MessageBox.Show("Existen campos vacíos", "Campos vacíos", MessageBoxButton.OK);
            }   else {
                String usuario = txtUsuario.Text;
                Token token = new Token();
                token = await Connection.Login(txtUsuario.Text, txtPassword.Password);
                    if (Connection.latestStatusCode == HttpStatusCode.OK && token.accessToken != "null")
                    {
                        Connection.matricula = txtUsuario.Text;
                        Connection.password = txtPassword.Password;
                        Student student;
                        Connection.token = token;
                        Connection.AutheticateWithToken();
                        student = await Connection.GetStudentByMatricula(txtUsuario.Text);
                        MainWindow mainWindow;
                        mainWindow = new MainWindow(student);
                        mainWindow.Show();
                        this.Close();
                       
                    }
                    else if (Connection.latestStatusCode == HttpStatusCode.Unauthorized)
                    {
                        MessageBox.Show("No se ha encontrado el estudiante con la matrícula y contraseña ingresada", "Estudiante no encontrado", MessageBoxButton.OK);
                    }
            }
        }
        private void btRegister_Click(object sender, RoutedEventArgs e)
        {
              RegisterStudent registerStudent = new RegisterStudent();
              registerStudent.Show();
              this.Close();

        }
    }
}
