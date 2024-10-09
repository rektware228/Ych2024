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
    /// Логика взаимодействия для AddPhotoPathWindoww.xaml
    /// </summary>
    public partial class AddPhotoPathWindoww : Window
    {
        public static List<Service> services { get; set; }
        public static List<ServicePhoto> servicesPhotos { get; set; }
        public static ServicePhoto servicePhoto = new ServicePhoto();
        Service contextService;
        public AddPhotoPathWindoww(Service service)
        {
            InitializeComponent();
            contextService = service;
            services = new List<Service>(DBConnection.clientsServiceEntities.Service.ToList());
            servicesPhotos = new List<ServicePhoto>(DBConnection.clientsServiceEntities.ServicePhoto.Where(p => p.ServiceID == contextService.ID).ToList());

            this.DataContext = this;
        }

        private void CancelBTN_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите выйти?", "", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);

            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }


        private void AddIMGBTN_Click(object sender, RoutedEventArgs e)
        {
            try 
            { 
                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (openFileDialog.ShowDialog() == true)
                {
                    string selectedImagePath = $"/Услуги школы/{openFileDialog.SafeFileName}";

                    var newPhoto = new ServicePhoto
                    {
                        ServiceID = contextService.ID,
                        PhotoPath = selectedImagePath
                    };
                    DBConnection.clientsServiceEntities.ServicePhoto.Add(newPhoto);
                    DBConnection.clientsServiceEntities.SaveChanges();

                    servicesPhotos.Add(newPhoto);
                    ServicesPhotosLV.ItemsSource = null;
                    ServicesPhotosLV.ItemsSource = servicesPhotos;
                }
            }
            catch 
            {
                MessageBox.Show("Произошла ошибка!", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void DeliteIMGBTN_Click(object sender, RoutedEventArgs e)
        {
            if (ServicesPhotosLV.SelectedItem is ServicePhoto servicePhoto)
            {
                try
                {
                    DBConnection.clientsServiceEntities.ServicePhoto.Remove(servicePhoto);
                    DBConnection.clientsServiceEntities.SaveChanges();

                    ServicesPhotosLV.ItemsSource = DBConnection.clientsServiceEntities.ServicePhoto.ToList();
                }
                catch
                {
                    MessageBoxResult result = MessageBox.Show("Произошла ошибка!\nНеобходимо перезагрузить программу!", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);

                    if (result == MessageBoxResult.OK)
                    {
                        // Получаем путь к текущему исполняемому файлу
                        string exePath = Process.GetCurrentProcess().MainModule.FileName;

                        // Запускаем новый экземпляр приложения
                        Process.Start(exePath);

                        // Завершаем текущее приложение
                        Application.Current.Shutdown();
                    }
                }

            }
        }
    }
}