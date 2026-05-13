using System.Windows;
using Carpooling.Core.Models;

namespace Carpooling.UI
{
    public partial class MainWindow : Window
    {
        private User _currentUser;

        // Використовуємо один конструктор з необов'язковим параметром
        public MainWindow(User user = null)
        {
            InitializeComponent();
            _currentUser = user;

            if (this.IsLoaded)
            {
                UpdateUI();
            }
            else
            {
                this.Loaded += (s, e) => UpdateUI();
            }
        }

        private void UpdateUI()
        {
            // Тепер btnAccount точно не буде null під час виконання Loaded
            if (_currentUser == null)
            {
                btnAccount.Content = "Увійти / Реєстрація";
            }
            else
            {
                btnAccount.Content = $"{_currentUser.FullName} (Профіль)";
            }
        }

        private void btnAccount_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                LoginWindow login = new LoginWindow();
                login.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show($"Вітаємо, {_currentUser.FullName}! Вікно профілю в розробці.");
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string from = txtFrom.Text.Trim();
            string to = txtTo.Text.Trim();
            DateTime? selectedDate = dateTrip.SelectedDate; // Отримуємо дату з DatePicker

            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
            {
                MessageBox.Show("Будь ласка, вкажіть міста для пошуку!", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SearchResultsWindow resultsWindow = new SearchResultsWindow(from, to, selectedDate, _currentUser);
            resultsWindow.Show();
            this.Close();
        }
    }
}