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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EducationalPracticeAutumn2024.Windowws
{
    /// <summary>
    /// Логика взаимодействия для EditServiceWindoww.xaml
    /// </summary>
    public partial class EditServiceWindoww : Window
    {
        public static List<Service> services { get; set; }
        Service contextservice;

        public EditServiceWindoww(Service services)
        {
            InitializeComponent();
            contextservice = services;
            this.DataContext = this;


            MainMG.Source = new BitmapImage(new Uri(services.MainImagePath, UriKind.Relative));
            IDTB.Text = $"ID: {contextservice.ID}";
            NameServiceTB.Text = contextservice.Title;
            CostServiceTB.Text = contextservice.Cost.ToString();
            TimeServiceTB.Text = (contextservice.DurationInSeconds / 60).ToString();
            SaleServiceTB.Text = contextservice.Discount.ToString();
            DegrServiceTB.Text += contextservice.Description;

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

        private void OKBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder error = new StringBuilder();
                Service service = contextservice;
                if (string.IsNullOrWhiteSpace(NameServiceTB.Text) || CostServiceTB.Text.Trim() == "" || TimeServiceTB.Text.Trim() == "")
                {
                    error.AppendLine("Заполните все поля!");
                    return;
                }
                if (error.Length > 0)
                {
                    MessageBox.Show(error.ToString());
                }

                if (int.Parse(TimeServiceTB.Text) > 240)
                {
                    MessageBox.Show("Занятие не может идти больше 4 часов!", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (int.Parse(SaleServiceTB.Text) < 0)
                {
                    MessageBox.Show("Скидка не может быть меньше 0!", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (int.Parse(SaleServiceTB.Text) > 100)
                {
                    MessageBox.Show("Скидка не может быть больше 100!", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (int.Parse(TimeServiceTB.Text) < 0)
                {
                    MessageBox.Show("Время занятия не может быть отрицательным!", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if(int.Parse(CostServiceTB.Text) < 0)
                {
                    MessageBox.Show("Цена не может быть ниже нуля!", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var existingService = DBConnection.clientsServiceEntities.Service.FirstOrDefault(s => s.Title.Equals(NameServiceTB.Text, StringComparison.OrdinalIgnoreCase));

                if (existingService != null && existingService.ID != service.ID)
                {
                    MessageBox.Show("Услуга с таким наименованием уже существует.", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                    var result = MessageBox.Show($"Проверьте верность введенных данных:\nНаименование: {NameServiceTB.Text}, \nСтоимость: {CostServiceTB.Text}, " +
                        $"Скидка:, {SaleServiceTB.Text}, \nДлительность: {TimeServiceTB.Text} минут, \nОписание: {DegrServiceTB.Text}", "",
                        MessageBoxButton.YesNo, MessageBoxImage.Asterisk);


                    if (result == MessageBoxResult.Yes)
                    {
                        service.Title = NameServiceTB.Text;
                        service.Description = DegrServiceTB.Text;
                        service.Cost = int.Parse(CostServiceTB.Text);
                        service.Discount = int.Parse(SaleServiceTB.Text);
                        service.DurationInSeconds = int.Parse(TimeServiceTB.Text) * 60;
                        DBConnection.clientsServiceEntities.SaveChanges();

                        NameServiceTB.Text = String.Empty;
                        SaleServiceTB.Text = String.Empty;
                        DegrServiceTB.Text = String.Empty;
                        CostServiceTB.Text = String.Empty;
                        TimeServiceTB.Text = String.Empty;

                        DBConnection.clientsServiceEntities.SaveChanges();
                        Close();
                    
                }      
            }
            catch
            {
                MessageBox.Show("Произошла ошибка!", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void EditMainIMGBTN_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImagePath = $"/Услуги школы/{openFileDialog.SafeFileName}";

                MainMG.Source = new BitmapImage(new Uri(selectedImagePath, UriKind.Relative));

                contextservice.MainImagePath = selectedImagePath;

                DBConnection.clientsServiceEntities.SaveChanges();
            }
        }

        private void MoreIMGBTN_Click(object sender, RoutedEventArgs e)
        {
            var allPhotoWindow = new AddPhotoPathWindoww(contextservice);
            allPhotoWindow.Show();
        }
    }
}
