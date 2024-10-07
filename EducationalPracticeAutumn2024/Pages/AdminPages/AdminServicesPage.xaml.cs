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

        public ServicesPage()
        {
            InitializeComponent();

            services = new List<Service>(DBConnection.clientsServiceEntities.Service.ToList());
            ServicesLV.ItemsSource = services;

            FiltrCb.Items.Add("По умолчанию");
            FiltrCb.Items.Add("По убыванию");
            FiltrCb.Items.Add("По возрастанию");

            FiltrSaleCb.Items.Add("от 0 до 5%");
            FiltrSaleCb.Items.Add("от 5% до 15%");
            FiltrSaleCb.Items.Add("от 15% до 30%");
            FiltrSaleCb.Items.Add("от 30% до 70%");
            FiltrSaleCb.Items.Add("от 70% до 100%");
            FiltrSaleCb.Items.Add("Все");
            FiltrSaleCb.SelectedIndex = 5;

            Refresh();

            this.DataContext = this;
        }

        private void Refresh()
        {
            List<Service> services = new List<Service>(DBConnection.clientsServiceEntities.Service.ToList());
            services = services.Where(i => i.Title.ToLower().StartsWith(SearchTbx.Text.Trim().ToLower())
                   || i.Description.ToLower().StartsWith(SearchTbx.Text.Trim().ToLower())).ToList();

            if (FiltrCb.SelectedIndex == 0)
            {
                ServicesLV.ItemsSource = services;
            }
            else if (FiltrCb.SelectedIndex == 1)
            {
                ServicesLV.ItemsSource = services.OrderByDescending(x => x.Cost).ToList();
            }
            else if (FiltrCb.SelectedIndex == 2)
            {
                ServicesLV.ItemsSource = services.OrderBy(x => x.Cost).ToList();
            }

            if (FiltrSaleCb.SelectedIndex == 0)
            {
                ServicesLV.ItemsSource = services.Where(x => x.Discount >= 0 && x.Discount < 5).ToList();
            }
            else if (FiltrSaleCb.SelectedIndex == 1)
            {
                ServicesLV.ItemsSource = services.Where(x => x.Discount >= 5 && x.Discount < 15).ToList();
            }
            else if (FiltrSaleCb.SelectedIndex == 2)
            {
                ServicesLV.ItemsSource = services.Where(x => x.Discount >= 15 && x.Discount < 30).ToList();
            }
            else if (FiltrSaleCb.SelectedIndex == 3)
            {
                ServicesLV.ItemsSource = services.Where(x => x.Discount >= 30 && x.Discount < 70).ToList();
            }
            else if (FiltrSaleCb.SelectedIndex == 4)
            {
                ServicesLV.ItemsSource = services.Where(x => x.Discount >= 70 && x.Discount < 100).ToList();
            }
            else if (FiltrSaleCb.SelectedIndex == 5)
            {
                ServicesLV.ItemsSource = services;
            }

            ServicesLV.ItemsSource = services;

        }

        private void FiltrCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void FiltrSaleCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void SearchTbx_TextChanged(object sender, TextChangedEventArgs e)
        {
            Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e) //Редач
        {
            if (sender is Button button && button.DataContext is Service service)
            {
                Service selectedService = service;

                EditServiceWindoww editServiceWindow = new EditServiceWindoww(selectedService);
                editServiceWindow.ShowDialog();

                Refresh();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) //Удалить
        {
            if (sender is Button button && button.DataContext is Service service)
            {
                DBConnection.clientsServiceEntities.Service.Remove(service);
                DBConnection.clientsServiceEntities.SaveChanges();

                Refresh();
            }
        }
    }
}