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
using EducationalPracticeAutumn2024.Pages.AdminPages;

namespace EducationalPracticeAutumn2024.Pages.MainPages
{
    /// <summary>
    /// Логика взаимодействия для PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        public PasswordWindow()
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordTextBox.Password == "0000")
            {
                // Здесь можно открыть новую страницу или окно
                MessageBox.Show("Пароль верный! Открываем новую страницу.");
                this.Close(); // Закрываем текущее окно
            }
            else
            {
                MessageBox.Show("Неверный пароль. Попробуйте снова.");
            }
        }
    }
}
