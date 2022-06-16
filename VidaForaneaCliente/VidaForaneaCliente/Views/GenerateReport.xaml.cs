using System;
using System.Collections.Generic;
using System.IO;
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
    /// Lógica de interacción para GenerateReport.xaml
    /// </summary>
    public partial class GenerateReport : Window
    {
        Student loggedStudent;
        bool isAdmin = false;
        public ObservableCollection<String> PlacesCollection { get; set; }
        public ObservableCollection<float> ScoresCollection { get; set; }
        public ListView Places { get { return lvPlace; } set { lvPlace = value; } }
        public ListView Scores { get { return lvScore; } set { lvScore = value; } }
        Dictionary<string, float> placeScores;

        List<Place> places = new List<Place>();
        public GenerateReport(Student loggedStudent)
        {
            InitializeComponent();
           this.loggedStudent = loggedStudent;
            if (loggedStudent.rol == "administrador")
            {
                isAdmin = true;
            }
            lbUser.Content = loggedStudent.name;
            PlacesCollection = new ObservableCollection<String>();
            ScoresCollection = new ObservableCollection<float>();

            InitializePlaces();
           
        }
       
        public async void InitializePlaces()
        {
            placeScores = new Dictionary<string, float>();
            string status = "aprobado";
            places = await Connection.GetPlacesByCategory(status, "");
               
            PlacesCollection = new ObservableCollection<String>();
           
            if (places != null)
            {
                PlacesCollection.Clear();
                ScoresCollection.Clear();
                foreach (var place in places)
                {
                    List<Opinion> opinions = await Connection.GetOpinionsByPlace(place);
                    int numOpinion = 0;
                    float score = 0;
                    foreach(var opinion in opinions)
                    {
                        score = score + opinion.score;
                        numOpinion++;
                    }
                    score = score / numOpinion;
                    placeScores.Add(place.name, score);
                }
                initializeColection();
            }          
        }

        private void initializeColection()
        {
            placeScores = placeScores.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            foreach (var placescore in placeScores)
            {
                PlacesCollection.Add(placescore.Key);
                ScoresCollection.Add(placescore.Value);
            }
           
            Places.ItemsSource = PlacesCollection;
            Scores.ItemsSource = ScoresCollection;
        }

        private void btReturn_Click(object sender, RoutedEventArgs e)
        {
            
                Menu menu = new Menu(loggedStudent);
                menu.Show();
                this.Close();
            
        }

        private void btPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();
                if(printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(print, "report");
                }
            }
            finally
            {
                this.IsEnabled = true;
            }
        }
    }
}
