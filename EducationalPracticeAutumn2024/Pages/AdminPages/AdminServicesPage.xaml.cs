using EducationalPracticeAutumn2024.DB;
using EducationalPracticeAutumn2024.Windowws;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EducationalPracticeAutumn2024.Pages.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        public static List<Service> services { get; set; }
        public static List<ClientService> clientServices { get; set; }

        public ServicesPage()
        {
            InitializeComponent();

            Refresh(0);
            SaleFLTRCB.SelectedIndex = 0;

            services = new List<Service>(DBConnection.clientsServiceEntities.Service.ToList());
            clientServices = new List<ClientService>(DBConnection.clientsServiceEntities.ClientService.ToList());

            ServicesLV.ItemsSource = services;
            this.DataContext = this;
        }

        private void Refresh(int i)
        {

            var allService = DBConnection.clientsServiceEntities.Service.ToList();
            var filtred = allService.AsQueryable();

            var searchText = SearchTB.Text.ToLower();

            if (SaleFLTRCB != null && SaleFLTRCB.SelectedIndex == 1)
            {
                var min_discountValue = 0;
                var max_discountValue = 5;

                filtred = filtred.Where(x => x.Discount.HasValue &&
                                               x.Discount.Value >= min_discountValue &&
                                               x.Discount.Value < max_discountValue);
            }
            if (SaleFLTRCB != null && SaleFLTRCB.SelectedIndex == 2)
            {
                var min_discountValue = 5;
                var max_discountValue = 15;

                filtred = filtred.Where(x => x.Discount.HasValue &&
                                               x.Discount.Value >= min_discountValue &&
                                               x.Discount.Value < max_discountValue);
            }
            if (SaleFLTRCB != null && SaleFLTRCB.SelectedIndex == 3)
            {
                var min_discountValue = 15;
                var max_discountValue = 30;

                filtred = filtred.Where(x => x.Discount.HasValue &&
                                               x.Discount.Value >= min_discountValue &&
                                               x.Discount.Value < max_discountValue);
            }
            if (SaleFLTRCB != null && SaleFLTRCB.SelectedIndex == 4)
            {
                var min_discountValue = 30;
                var max_discountValue = 70;

                filtred = filtred.Where(x => x.Discount.HasValue &&
                                               x.Discount.Value >= min_discountValue &&
                                               x.Discount.Value < max_discountValue);
            }
            if (SaleFLTRCB != null && SaleFLTRCB.SelectedIndex == 5)
            {
                var min_discountValue = 70;
                var max_discountValue = 100;

                filtred = filtred.Where(x => x.Discount.HasValue &&
                                               x.Discount.Value >= min_discountValue &&
                                               x.Discount.Value < max_discountValue);
            }

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filtred = filtred.Where(x => x.Title.ToLower().Contains(searchText) ||
                                                (x.Description != null && x.Description.ToLower().Contains(searchText)));
            }

            if (LessBTN.IsEnabled == true && LargerBTN.IsEnabled == false)
                filtred = filtred.OrderByDescending(x => x.NewCost);
            else if (LessBTN.IsEnabled == false && LargerBTN.IsEnabled == true)
                filtred = filtred.OrderBy(x => x.NewCost);

            ServicesLV.ItemsSource = filtred.ToList();
            CountRecordTBL.Text = $"{filtred.Count()} из {allService.Count}";
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void PART_ContentHost_TextChanged(object sender, TextChangedEventArgs e) //Поиск текста
        {
            Refresh(0);
        }

        private void SaleFLTRCB_SelectionChanged(object sender, SelectionChangedEventArgs e) //Комбобокс со скидкой
        {
            Refresh(0);
        }

        private void LargerBTN_Click(object sender, RoutedEventArgs e) //По возрастанию
        {
            LargerBTN.Background = new SolidColorBrush(Color.FromRgb(0, 28, 112));
            LessBTN.Background = new SolidColorBrush(Color.FromRgb(4, 160, 255));


            LargerBTN.IsEnabled = false;
            LessBTN.IsEnabled = true;

            Refresh(0);
        }

        private void LessBTN_Click(object sender, RoutedEventArgs e) //По убыванию
        {
            LessBTN.Background = new SolidColorBrush(Color.FromRgb(0, 28, 112));
            LargerBTN.Background = new SolidColorBrush(Color.FromRgb(4, 160, 255));


            LessBTN.IsEnabled = false;
            LargerBTN.IsEnabled = true;

            Refresh(0);
        }

        private void ResetBTN_Click(object sender, RoutedEventArgs e) //Очистить
        {
            LessBTN.Background = new SolidColorBrush(Color.FromRgb(4, 160, 255));
            LargerBTN.Background = new SolidColorBrush(Color.FromRgb(4, 160, 255));


            LessBTN.IsEnabled = true;
            LargerBTN.IsEnabled = true;

            SearchTB.Text = "";

            SaleFLTRCB.SelectedIndex = 0;

            var allService = DBConnection.clientsServiceEntities.Service.ToList();
            var filtred = allService.AsQueryable();


            ServicesLV.ItemsSource = filtred.ToList();
            CountRecordTBL.Text = $"{filtred.Count()} из {allService.Count}";
        }



        private void EdidServiceBTN_Click(object sender, RoutedEventArgs e) //Редач
        {
            if (sender is Button button && button.DataContext is Service service)
            {
                Service selectedService = service;

                EditServiceWindoww editServiceWindow = new EditServiceWindoww(selectedService);
                editServiceWindow.ShowDialog();

                Refresh(0);
            }
        }

        private void DeliteServiceBNT_Click(object sender, RoutedEventArgs e) //Удалить
        {
            try
            {
                if (sender is Button button && button.DataContext is Service service)
                {
                    var result = MessageBox.Show($"Вы действительно хотите удалить услугу {service.Title}?", "", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);

                    if (result == MessageBoxResult.Yes)
                    {
                        DBConnection.clientsServiceEntities.Service.Remove(service);
                        DBConnection.clientsServiceEntities.SaveChanges();
                        Refresh(0);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Невозможно удалить запись!\nНа данную услугу существует запись или дополнительная фотография", "ОШИБКА!", MessageBoxButton.OK, MessageBoxImage.Error);
                MessageBox.Show("После удаления записей на данную услугу или дополнительных фотографий услуга удалиться автоматически", "Подсказка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddServiceBTN_Click(object sender, RoutedEventArgs e)
        {
                AddServiceWindoww addServiceWindow = new AddServiceWindoww();
                addServiceWindow.ShowDialog();
        }

        private void NearestServiceClientsBTN_Click(object sender, RoutedEventArgs e)
        {
            NearestServiceClients nearestServiceClients = new NearestServiceClients();
            nearestServiceClients.Show(); //Окно специально открыто без блокировки основных окон!!!
        }
    }
}