using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Carpooling.Core.Models;
using Carpooling.Core.Managers;
using Carpooling.Core.Services;

namespace Carpooling.UI
{
    public partial class SearchResultsWindow : Window
    {
        private readonly TripManager _tripManager;
        private List<Trip> _results;
        private User _currentUser; // Поле для зберігання поточного користувача

        // Конструктор з параметром currentUser
        public SearchResultsWindow(string from, string to, DateTime? date, User currentUser = null)
        {
            InitializeComponent();
            _currentUser = currentUser; // Зберігаємо користувача

            var storage = new JsonDataStorage();
            _tripManager = new TripManager(storage);

            txtFrom.Text = from;
            txtTo.Text = to;
            dateTrip.SelectedDate = date;

            this.Loaded += (s, e) => UpdateAccountButton(); // Оновлюємо кнопку після завантаження UI
            PerformSearch();
        }

        private void UpdateAccountButton()
        {
            if (_currentUser == null)
            {
                btnAccount.Content = "Увійти / Реєстрація"; // Текст для гостя
            }
            else
            {
                btnAccount.Content = "Мій профіль"; // Текст для авторизованого
            }
        }

        private void PerformSearch()
        {
            string from = txtFrom.Text.Trim();
            string to = txtTo.Text.Trim();
            DateTime? date = dateTrip.SelectedDate;

            _results = _tripManager.GetAllTrips()
                .Where(t => t.Status == "Активна" &&
                            (string.IsNullOrEmpty(from) || t.DepartureCity.Contains(from, StringComparison.OrdinalIgnoreCase)) &&
                            (string.IsNullOrEmpty(to) || t.ArrivalCity.Contains(to, StringComparison.OrdinalIgnoreCase)) &&
                            (!date.HasValue || t.DepartureTime.Date == date.Value.Date))
                .ToList();

            // Оновлюємо інтерфейс
            lstTrips.ItemsSource = _results;

            // Керуємо видимістю повідомлення про відсутність результатів
            if (_results.Count == 0)
            {
                txtNoResults.Visibility = Visibility.Visible;
                txtResultSummary.Text = "Поїздок не знайдено";
            }
            else
            {
                txtNoResults.Visibility = Visibility.Collapsed;
                txtResultSummary.Text = $"Знайдено поїздок: {_results.Count}";
            }
        }

        // Обробник кнопки "Оновити"
        private void btnUpdateSearch_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        // Обробник кнопки "Мій профіль"
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

        // Обробник кнопки "Забронювати"
        private void btnBook_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as FrameworkElement;
            var selectedTrip = button?.Tag as Trip;

            if (selectedTrip != null)
            {
                if (selectedTrip.CalculateFreeSeats() > 0)
                {
                    MessageBox.Show($"Ви забронювали місце у поїздці: {selectedTrip.DepartureCity} - {selectedTrip.ArrivalCity}",
                                    "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);

                    PerformSearch();
                }
                else
                {
                    MessageBox.Show("На жаль, вільних місць більше немає.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as FrameworkElement;
            var selectedTrip = button?.Tag as Trip;

            if (selectedTrip != null)
            {
                // Створюємо вікно деталей, передаючи:
                // 1. Об'єкт обраної поїздки
                // 2. Поточного користувача (щоб знати, хто бронює)
                // 3. Посилання на це вікно (this), щоб працювала кнопка "Назад"
                TripDetailsWindow detailsWindow = new TripDetailsWindow(selectedTrip, _currentUser, this);

                detailsWindow.Show();
                this.Hide(); // Ховаємо список результатів, поки ми в деталях
            }
        }
    }
}