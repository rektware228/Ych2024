using EducationalPracticeAutumn2024.DB;
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

namespace EducationalPracticeAutumn2024.Windowws
{
    /// <summary>
    /// Логика взаимодействия для AddServiceWindoww.xaml
    /// </summary>
    public partial class AddServiceWindoww : Window
    {
        public static List<Service> services { get; set; }
        public static Service service = new Service();

        bool availabilityMainIMG = false;
        public AddServiceWindoww()
        {
            InitializeComponent();
            services = new List<Service>(DBConnection.clientsServiceEntities.Service.ToList());
        }

        private void NumericOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(e.Text);

        }

        private static bool IsTextNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            return reg.IsMatch(str);

        }

        private void AddMainIMGBTN_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImagePath = $"/Услуги школы/{openFileDialog.SafeFileName}";

                MainMG.Source = new BitmapImage(new Uri(selectedImagePath, UriKind.Relative));

                service.MainImagePath = selectedImagePath;

                availabilityMainIMG = true;

            }
            //DBConnection.clientsServiceEntities.SaveChanges();
        }

        private void OKBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder error = new StringBuilder();
                if (string.IsNullOrWhiteSpace(NameServiceTB.Text) || CostServiceTB.Text.Trim() == "" || TimeServiceTB.Text.Trim() == "")

                {
                    error.AppendLine("Заполните все поля!");
                }
                else if (error.Length > 0)
                {
                    MessageBox.Show(error.ToString());
                }
                else if (int.Parse(TimeServiceTB.Text) > 240)
                {
                    MessageBox.Show("Занятие не может идти больше 4 часов!", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (int.Parse(SaleServiceTB.Text) < 0)
                {
                    MessageBox.Show("Скидка не может быть меньше 0!", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (int.Parse(SaleServiceTB.Text) > 100)
                {
                    MessageBox.Show("Скидка не может быть больше 100!", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (int.Parse(TimeServiceTB.Text) < 0)
                {
                    MessageBox.Show("Время занятия не может быть отрицательным!", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (int.Parse(CostServiceTB.Text) < 0)
                {
                    MessageBox.Show("Цена не может быть ниже нуля!", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var existingService = DBConnection.clientsServiceEntities.Service.FirstOrDefault(s => s.Title.Equals(NameServiceTB.Text, StringComparison.OrdinalIgnoreCase));

                    if (existingService != null && existingService.ID != service.ID)
                    {
                        MessageBox.Show("Услуга с таким наименованием уже существует.", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                                  


                }

                if (availabilityMainIMG == false)
                {
                    MessageBox.Show("Добавьте главную фотографию услуги!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    var result = MessageBox.Show($"Проверьте верность введенных данных:\nНаименование: {NameServiceTB.Text}, Стоимость: {CostServiceTB.Text}, " +
                        $"Скидка:, {SaleServiceTB.Text}, Длительность: {TimeServiceTB.Text} минут, \nОписание: {DegrServiceTB.Text}", "",
                        MessageBoxButton.YesNo, MessageBoxImage.Asterisk);


                    if (result == MessageBoxResult.Yes)
                    {

                        service.Title = NameServiceTB.Text.Trim();
                        service.Description = DegrServiceTB.Text.Trim();
                        service.Cost = int.Parse(CostServiceTB.Text.Trim());
                        service.Discount = int.Parse(SaleServiceTB.Text.Trim());
                        service.DurationInSeconds = int.Parse(TimeServiceTB.Text.Trim()) * 60;

                        DBConnection.clientsServiceEntities.Service.Add(service);
                        DBConnection.clientsServiceEntities.SaveChanges();

                        this.Close();
                    }
                }


            }
            catch 
            {
                MessageBox.Show("ОШИБКА!", "", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}
