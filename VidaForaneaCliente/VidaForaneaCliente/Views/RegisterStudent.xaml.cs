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
            Connection.initializeConnection();
        }

        private async void btAdd_Click(object sender, RoutedEventArgs e)
        {
            Student student = new Student();
            tbName.Text = student.nombre;
            tbEnrollment.Text = student.matricula;
            tbDegree.Text = student.licenciatura;
            pbPassword.Password = student.contrasenia;
            Connection.PostStudent(student);   
            if (Connection.latestStatusCode == HttpStatusCode.OK)
            {
                MessageBox.Show("Yupi", "Estudiante registrado", MessageBoxButton.OK);
            }
            else if (Connection.latestStatusCode == HttpStatusCode.NotFound)
            {
                MessageBox.Show("No se ha encontrado el estudiante con la matrícula y contraseña ingresada", "Estudiante no encontrado", MessageBoxButton.OK);
            }

        }
    }
}
