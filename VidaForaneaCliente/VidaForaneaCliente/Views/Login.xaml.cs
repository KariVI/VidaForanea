﻿using System;
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
        public Login()
        {
            InitializeComponent();
            Connection.initializeConnection();
        }

        private async void btLogin_Click(object sender, RoutedEventArgs e)
        {
            Student student = await Connection.Login(txtMatricula.Text,txtPassword.Password);
            if (Connection.latestStatusCode == HttpStatusCode.OK)
            {
                Menu menu = new Menu(student);
                menu.Show();
                this.Close();
            } else if (Connection.latestStatusCode == HttpStatusCode.NotFound)
            {
                MessageBox.Show("No se ha encontrado el estudiante con la matrícula y contraseña ingresada", "Estudiante no encontrado",MessageBoxButton.OK);
            }
        }

    }
}
