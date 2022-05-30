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
            cbDegree.SelectedIndex = 1;
        }

        private async void btAdd_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbName.Text) || String.IsNullOrWhiteSpace(tbEnrollment.Text)
               || String.IsNullOrWhiteSpace(pbPassword.Password))
            {
                MessageBox.Show("Existen campos vacíos, por favor revise los campos", "Campos vacíos", MessageBoxButton.OK);
            }
            else
            {
                Student student = new Student();
                student.name = tbName.Text;
                student.enrollment = tbEnrollment.Text;
                ComboBoxItem comboItem = (ComboBoxItem)cbDegree.SelectedItem;
                string degreeString = comboItem.Content.ToString();
                student.degree = degreeString;
                student.password = pbPassword.Password;
                bool correcto = await Connection.PostStudent(student);
                if (Connection.latestStatusCode == HttpStatusCode.Created)
                {
                    MessageBox.Show("Se ha registrado el estudiante correctamente", "Estudiante registrado", MessageBoxButton.OK);
                    MainWindow mainWindow = new MainWindow(student);
                    mainWindow.Show();
                    this.Close();
                }
                else if (Connection.latestStatusCode == HttpStatusCode.BadRequest)
                {
                    MessageBox.Show("El estudiante ya se encuentra registrado", "Estudiante ya existente", MessageBoxButton.OK);
                }
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
