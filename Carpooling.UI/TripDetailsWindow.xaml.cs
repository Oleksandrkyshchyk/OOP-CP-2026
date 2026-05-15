using System;
using System.Collections.Generic;
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
        private readonly TripManager _tripManager;

        public TripDetailsWindow(Trip trip, User currentUser, Window previousWindow)
        {
            InitializeComponent();
            _trip = trip;
            _currentUser = currentUser;
            _previousWindow = previousWindow;

            // Ініціалізуємо менеджери
            var storage = new JsonDataStorage();
            _userManager = new UserManager(storage);
            _tripManager = new TripManager(storage);

            PopulateFields();
        }

        private void PopulateFields()
        {
            // Заповнення основних даних поїздки
            lblRoute.Text = $"{_trip.DepartureCity} → {_trip.ArrivalCity}";
            lblDate.Text = $"Дата: {_trip.DepartureTime:dd.MM.yyyy HH:mm}";
            lblPrice.Text = $"Ціна: {_trip.Price} грн";

            int freeSeats = _trip.CalculateFreeSeats();
            lblSeats.Text = $"Вільних місць: {freeSeats} з {_trip.TotalSeats}";

            // Пошук конкретного водія, який створив цю поїздку
            var driver = _userManager.GetAllUsers()
                .FirstOrDefault(u => u.Login == _trip.DriverLogin) as Driver;

            if (driver != null)
            {
                lblDriver.Text = $"Водій: {driver.FullName}";
                lblCar.Text = $"Авто: {driver.CarModel ?? "Не вказано"} ({driver.LicensePlate ?? "---"})";
            }
            else
            {
                lblDriver.Text = "Водій: Дані відсутні";
                lblCar.Text = "Авто: Не вказано";
            }

            // Блокуємо кнопку, якщо місць немає
            if (freeSeats <= 0)
            {
                btnBook.IsEnabled = false;
                btnBook.Content = "Місць немає";
            }
        }

        private void btnBook_Click(object sender, RoutedEventArgs e)
        {
            // 1. Перевірка авторизації
            if (_currentUser == null)
            {
                MessageBox.Show("Бронювання доступне лише зареєстрованим користувачам!", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Перевірка ролі
            if (_currentUser.Role != "Пасажир")
            {
                MessageBox.Show("Тільки пасажири можуть бронювати поїздки.", "Обмеження ролі");
                return;
            }

            // 3. Заборона власного бронювання
            if (_trip.DriverLogin == _currentUser.Login)
            {
                MessageBox.Show("Ви не можете забронювати місце у власній поїздці.", "Помилка");
                return;
            }

            // 4. Перевірка на повторне бронювання
            bool alreadyBooked = _trip.Bookings != null &&
                                 _trip.Bookings.Any(b => b.Passenger != null && b.Passenger.Login == _currentUser.Login);

            if (alreadyBooked)
            {
                MessageBox.Show("Ви вже забронювали місце у цій поїздці.", "Повторна дія");
                return;
            }

            // 5. Бронювання
            if (_trip.CalculateFreeSeats() > 0)
            {
                var newBooking = new Booking
                {
                    Passenger = _currentUser as Passenger,
                    Trip = _trip,
                    SeatsCount = 1,
                    BookingDate = DateTime.Now
                };

                if (_trip.Bookings == null) _trip.Bookings = new List<Booking>();
                _trip.Bookings.Add(newBooking);

                // Збереження змін у JSON
                if (_tripManager.UpdateTrip(_trip))
                {
                    MessageBox.Show($"Ви успішно забронювали місце!", "Успіх");
                    PopulateFields(); // Оновлюємо UI
                }
                else
                {
                    MessageBox.Show("Помилка при збереженні бронювання.");
                }
            }
            else
            {
                MessageBox.Show("На жаль, вільних місць більше немає.", "Помилка");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (_previousWindow != null)
            {
                _previousWindow.Show();
            }
            this.Close();
        }
    }
}