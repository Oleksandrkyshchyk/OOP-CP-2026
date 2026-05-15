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
        private User _currentUser;

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
                ProfileWindow profile = new ProfileWindow(_currentUser, this);
                profile.Show();
                this.Hide();
            }
        }

        // Обробник кнопки "Забронювати"
        private void btnBook_Click(object sender, RoutedEventArgs e)
        {
            // 1. Перевірка на авторизацію
            if (_currentUser == null)
            {
                MessageBox.Show("Будь ласка, увійдіть, щоб забронювати місце.", "Авторизація");
                return;
            }

            // 2. Блокування бронювань від водіїв (та інших ролей, крім Пасажира)
            if (_currentUser.Role != "Пасажир")
            {
                MessageBox.Show("Тільки користувачі з роллю 'Пасажир' можуть бронювати поїздки.", "Обмеження ролі");
                return;
            }

            var button = sender as FrameworkElement;
            var selectedTrip = button?.Tag as Trip;

            if (selectedTrip != null)
            {
                // Заборона бронювати власну поїздку (якщо водій раптом має роль пасажира)
                if (selectedTrip.DriverLogin == _currentUser.Login)
                {
                    MessageBox.Show("Ви не можете забронювати місце у власній поїздці.", "Помилка");
                    return;
                }

                // Перевірка на повторне бронювання одним і тим самим пасажиром
                bool alreadyBooked = selectedTrip.Bookings != null &&
                                     selectedTrip.Bookings.Any(b => b.Passenger != null && b.Passenger.Login == _currentUser.Login);

                if (alreadyBooked)
                {
                    MessageBox.Show("Ви вже забронювали місце у цій поїздці. Повторне бронювання неможливе.", "Повторна дія");
                    return;
                }

                // 5. Перевірка наявності вільних місць
                if (selectedTrip.CalculateFreeSeats() > 0)
                {
                    // Створення бронювання
                    var newBooking = new Booking
                    {
                        Passenger = _currentUser as Passenger,
                        Trip = selectedTrip,
                        SeatsCount = 1,
                        BookingDate = DateTime.Now
                    };

                    if (selectedTrip.Bookings == null) selectedTrip.Bookings = new List<Booking>();
                    selectedTrip.Bookings.Add(newBooking);

                    // Збереження в JSON через TripManager
                    if (_tripManager.UpdateTrip(selectedTrip))
                    {
                        MessageBox.Show($"Місце успішно заброньовано!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                        PerformSearch(); // Оновлюємо список результатів
                    }
                    else
                    {
                        MessageBox.Show("Сталася помилка при збереженні даних у базу.");
                    }
                }
                else
                {
                    MessageBox.Show("На жаль, вільних місць більше немає.", "Помилка");
                }
            }
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            // Приховуємо деталі від гостей
            if (_currentUser == null)
            {
                MessageBox.Show("Детальна інформація про водія та автомобіль доступна лише авторизованим користувачам.",
                                "Доступ обмежено", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var button = sender as FrameworkElement;
            var selectedTrip = button?.Tag as Trip;

            if (selectedTrip != null)
            {
                TripDetailsWindow detailsWindow = new TripDetailsWindow(selectedTrip, _currentUser, this);
                detailsWindow.Show();
                this.Hide();
            }
        }
    }
}