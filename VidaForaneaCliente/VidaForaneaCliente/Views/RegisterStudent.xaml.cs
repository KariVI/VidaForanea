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

namespace VidaForaneaCliente.Views

{
    /// <summary>
    /// Lógica de interacción para RegisterStudent.xaml
    /// </summary>
    public partial class RegisterStudent : Window
    {
        public RegisterStudent()
        {
            InitializeComponent();
        }

        private async void btAdd_Click(object sender, RoutedEventArgs e)
        {
            Student student = new Student();
            student.nombre = tbName.Text  ;
            student.matricula = tbEnrollment.Text ;
            student.licenciatura = tbDegree.Text;
            student.contrasenia = pbPassword.Password ;
            bool correcto = await Connection.PostStudent(student);   
            if (Connection.latestStatusCode == HttpStatusCode.Created)
            {
                MessageBox.Show("Se ha registrado el estudiante correctamente", "Estudiante registrado", MessageBoxButton.OK);
                MainWindow mainWindow =  new MainWindow(student);
                mainWindow.Show();
                this.Close();
            }
            else if (Connection.latestStatusCode == HttpStatusCode.BadRequest)
            {
                MessageBox.Show("El estudiante ya se encuentra registrado", "Estudiante ya existente", MessageBoxButton.OK);
            }
           
        }

        private void btReturn_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }
}
