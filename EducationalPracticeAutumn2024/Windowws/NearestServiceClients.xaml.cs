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
using EducationalPracticeAutumn2024.DB;

namespace EducationalPracticeAutumn2024.Windowws
{
    /// <summary>
    /// Логика взаимодействия для NearestServiceClients.xaml
    /// </summary>
    public partial class NearestServiceClients : Window
    {
        public static List<Service> services { get; set; }
        public static List<Client> clients { get; set; }
        public static List<ClientService> clientService { get; set; } 
        public NearestServiceClients()
        {
            InitializeComponent();
            Rehresh();
        }

        public async void Rehresh()
        {
            while (true)
            {
                DateTime today = DateTime.Now;
                DateTime tomorrow = today.AddDays(1);

                services = new List<Service>(DBConnection.clientsServiceEntities.Service.ToList());
                clients = new List<Client>(DBConnection.clientsServiceEntities.Client.ToList());
                clientService = new List<ClientService>(DBConnection.clientsServiceEntities.ClientService.
                    Where(x => (DateTime)x.StartTime >= today && (DateTime)x.StartTime <= tomorrow).OrderBy(x => (DateTime)x.StartTime).ToList());

                NearestSCLV.ItemsSource = clientService;
                await Task.Delay(30000);
            }
        }
    }
}
