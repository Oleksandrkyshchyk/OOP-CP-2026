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

        public void UpdateUI()
        {
            if (_currentUser == null)
            {
                btnAccount.Content = "Увійти / Реєстрація";
                btnCreateTrip.Visibility = Visibility.Collapsed;
                btnAdminPanel.Visibility = Visibility.Collapsed; // Ховаємо панель адміна для гостей
            }
            else
            {
                btnAccount.Content = $"{_currentUser.FullName} (Профіль)";

                // Відображення кнопки створення поїздки для водіїв
                btnCreateTrip.Visibility = (_currentUser is Driver) ? Visibility.Visible : Visibility.Collapsed;

                // Відображення кнопки панелі адміністратора
                if (btnAdminPanel != null) // Перевірка, чи ви вже додали її в XAML
                {
                    btnAdminPanel.Visibility = (_currentUser is Admin) ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        // Обробник натискання
        private void btnCreateTrip_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser is Driver driver)
            {
                CreateTripWindow createTripWin = new CreateTripWindow(driver, this);
                createTripWin.Show();
                this.Hide(); // Ховаємо головне вікно, поки створюється поїздка
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
                ProfileWindow profile = new ProfileWindow(_currentUser, this);
                profile.Show();
                this.Hide();
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

        private void btnAdminPanel_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser is Admin admin)
            {
                // Припустимо, вікно називається AdminWindow
                AdminWindow adminWin = new AdminWindow(admin, this);
                adminWin.Show();
                this.Hide();
            }
        }
    }
}