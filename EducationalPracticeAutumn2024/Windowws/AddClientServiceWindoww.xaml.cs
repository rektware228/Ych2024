using EducationalPracticeAutumn2024.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace EducationalPracticeAutumn2024.Windowws
{
    /// <summary>
    /// Логика взаимодействия для AddClientService.xaml
    /// </summary>
    public partial class AddClientService : Window
    {
        public static List<Client> clients {  get; set; }
        public static List<Service> services { get; set; }
        public static List<ClientService> clientServices { get; set; }
        Service contextservice;

        public AddClientService(Service service)
        {
            InitializeComponent();
            contextservice = service;

            services = new List<Service>(DBConnection.clientsServiceEntities.Service.ToList());
            clients = new List<Client>(DBConnection.clientsServiceEntities.Client.ToList());
            clientServices = new List<ClientService>(DBConnection.clientsServiceEntities.ClientService.ToList());

            DateClientServiceDP.DisplayDateStart = DateTime.Now;

            MainMG.Source = new BitmapImage(new Uri(service.MainImagePath, UriKind.Relative));
            TitleServiceTBL.Text = contextservice.Title;
            TimeServiceTBL.Text = (contextservice.DurationInSeconds / 60).ToString();

            this.DataContext = this;

        }

        private void OKBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder error = new StringBuilder();

                if (ClientsCB.SelectedIndex == -1 || DateClientServiceDP.SelectedDate == null || TimeClientServiceTB.Text.Trim() == "")
                {
                    error.AppendLine("Заполните все поля!");
                }

                int hour = -1;
                int minute = -1;

                if (TimeClientServiceTB.Text.Length == 5 && TimeClientServiceTB.Text[2] == ':' &&
                    int.TryParse(TimeClientServiceTB.Text.Substring(0, 2), out hour) &&
                    int.TryParse(TimeClientServiceTB.Text.Substring(3, 2), out minute))
                {
                    if (hour < 0 || hour > 23 || minute < 0 || minute > 59)
                    {
                        error.AppendLine("Введите корректное время в формате Час:Минута!");
                    }
                }
                else
                {
                    error.AppendLine("Введите корректное время в формате Час:Минута!");
                }


                if (error.Length > 0)
                {
                    MessageBox.Show(error.ToString());
                }
                else
                {
                    DateTime selectedDate = (DateTime)DateClientServiceDP.SelectedDate;
                    DateTime startTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hour, minute, 0);
                    var selectedClient = ClientsCB.SelectedItem as Client;


                    var result = MessageBox.Show($"Проверьте верность введенных данных:\nНаименование услуги: {TitleServiceTBL.Text}" + 
                        $"Дата:, {selectedDate.Day}.{selectedDate.Month}.{selectedDate.Year}, Время: {hour}:{minute}, " +
                        $"\nКлиент: {selectedClient.LastName} {selectedClient.FirstName} {selectedClient.Patronymic}", "",MessageBoxButton.YesNo, MessageBoxImage.Asterisk);


                    if (result == MessageBoxResult.Yes)
                    {

                        ClientService clientService = new ClientService();

                        clientService.StartTime = startTime;
                        clientService.ServiceID = contextservice.ID;


                        clientService.ClientID = selectedClient.ID;

                        DateTime endTime = startTime.AddSeconds(contextservice.DurationInSeconds);

                        DBConnection.clientsServiceEntities.ClientService.Add(clientService);
                        DBConnection.clientsServiceEntities.SaveChanges();

                        this.Close();
                    }

                }
            }
            catch
            {
                MessageBoxResult result = MessageBox.Show("Произошла ошибка!\nНеобходимо перезагрузить программу!", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);

                if (result == MessageBoxResult.OK)
                {
                    //ПЕРЕЗАПУСК ПРОГРАММЫ
                    string exePath = Process.GetCurrentProcess().MainModule.FileName;
                    Process.Start(exePath);
                    Application.Current.Shutdown();
                }
            }
        }

        private void CancelBTN_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите выйти?", "", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);

            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void TimeTbx_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1) && e.Text != ":")
            {
                e.Handled = true;
            }
        }

        private void TimeTbx_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TimeServiceTBL.Text.Length == 5 && TimeServiceTBL.Text[2] == ':')
            {
                int hour, minute;
                if (int.TryParse(TimeServiceTBL.Text.Substring(0, 2), out hour) && int.TryParse(TimeServiceTBL.Text.Substring(3, 2), out minute))
                {
                    if (hour < 0 || hour > 23 || minute < 0 || minute > 59)
                    {
                        MessageBox.Show("Введите корректное время в формате Час:Минута!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        TimeServiceTBL.Text = string.Empty;
                    }
                }
                else
                {
                    MessageBox.Show("Введите корректное время в формате Час:Минута!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    TimeServiceTBL.Text = string.Empty;
                }
            }
        }

    }
}
