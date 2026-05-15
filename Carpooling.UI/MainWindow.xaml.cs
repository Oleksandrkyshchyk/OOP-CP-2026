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
                btnCreateTrip.Visibility = Visibility.Collapsed; // Гість не бачить кнопку
            }
            else
            {
                btnAccount.Content = $"{_currentUser.FullName} (Профіль)";

                // Перевіряємо, чи є користувач водієм
                if (_currentUser is Driver)
                {
                    btnCreateTrip.Visibility = Visibility.Visible;
                }
                else
                {
                    btnCreateTrip.Visibility = Visibility.Collapsed; // Пасажир не бачить кнопку
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
    }
}