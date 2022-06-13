using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VidaForaneaCliente.Models;
using VidaForaneaCliente.ServerConnection;
namespace VidaForaneaCliente.Views
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Connection.InitializeConnection();
        }
        
    }
}
