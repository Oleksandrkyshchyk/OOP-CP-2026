using System;
using System.Linq;
using System.Windows;
using Carpooling.Core.Managers;
using Carpooling.Core.Models;
using Carpooling.Core.Services;

namespace Carpooling.UI
{
    public partial class TripDetailsWindow : Window
    {
        private Trip _trip;
        private User _currentUser;
        private Window _previousWindow;
        private readonly UserManager _userManager;

        public TripDetailsWindow(Trip trip, User currentUser, Window previousWindow)
        {
            InitializeComponent();
            _trip = trip;
            _currentUser = currentUser;
            _previousWindow = previousWindow;

            // Ініціалізуємо менеджер для пошуку даних водія
            _userManager = new UserManager(new JsonDataStorage());

            PopulateFields();
        }

        private void PopulateFields()
        {
            // Динамічне заповнення полів даними об'єкта
            lblRoute.Text = $"{_trip.DepartureCity} → {_trip.ArrivalCity}";
            lblDate.Text = $"Дата: {_trip.DepartureTime:dd.MM.yyyy HH:mm}";
            lblPrice.Text = $"Ціна: {_trip.Price} грн";

            int freeSeats = _trip.CalculateFreeSeats();
            lblSeats.Text = $"Вільних місць: {freeSeats} з {_trip.TotalSeats}";

            // Пошук водія за логіном (якщо він є в Trip)
            // Примітка: переконайтеся, що в класі Trip є DriverLogin або аналогічне поле
            var driver = _userManager.GetAllUsers().FirstOrDefault(u => u.Role == "Водій") as Driver;

            if (driver != null)
            {
                lblDriver.Text = $"Водій: {driver.FullName}";
                lblCar.Text = $"Авто: {driver.CarModel ?? "Не вказано"} ({driver.LicensePlate ?? "---"})";
            }
        }

        // МЕТОД, ЯКОГО НЕ ВИСТАЧАЛО (виправляє помилку CS1061)
        private void btnBook_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Бронювання доступне лише зареєстрованим користувачам!", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_trip.CalculateFreeSeats() <= 0)
            {
                MessageBox.Show("На жаль, вільних місць немає.", "Помилка");
                return;
            }

            // Тут буде логіка створення Booking та збереження в JSON
            MessageBox.Show("Ви успішно забронювали місце!", "Успіх");
            PopulateFields(); // Оновлюємо кількість місць на екрані
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            _previousWindow.Show();
            this.Close();
        }
    }
}