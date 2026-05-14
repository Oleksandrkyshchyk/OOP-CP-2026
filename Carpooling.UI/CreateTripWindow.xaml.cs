using System;
using System.Windows;
using Carpooling.Core.Managers;
using Carpooling.Core.Models;
using Carpooling.Core.Services;
using Carpooling.Core.Validators;

namespace Carpooling.UI
{
    public partial class CreateTripWindow : Window
    {
        private readonly TripManager _tripManager;
        private readonly Driver _currentDriver;
        private readonly Window _parentWindow;

        public CreateTripWindow(Driver driver, Window parentWindow)
        {
            InitializeComponent();

            _currentDriver = driver;
            _parentWindow = parentWindow;
            _tripManager = new TripManager(new JsonDataStorage());

            InitializeFields();
        }

        private void InitializeFields()
        {
            // Встановлюємо сьогоднішню дату як мінімальну
            datePicker.SelectedDate = DateTime.Now;

            // Підтягуємо дані авто з профілю водія
            if (!string.IsNullOrWhiteSpace(_currentDriver.CarModel))
            {
                lblCarInfo.Text = $"Авто: {_currentDriver.CarModel} ({_currentDriver.LicensePlate})";
            }
            else
            {
                lblCarInfo.Text = "Увага: Дані про авто не вказані у профілі!";
                btnCreate.IsEnabled = false;
                MessageBox.Show("Для створення поїздки необхідно вказати автомобіль у вашому профілі.",
                                "Дані відсутні", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            // 1. Збір текстових даних
            string from = txtFrom.Text.Trim();
            string to = txtTo.Text.Trim();
            string timeStr = txtTime.Text.Trim();

            // 2. ВАЛІДАЦІЯ: Порожні поля (Маршрут)
            if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
            {
                MessageBox.Show("Будь ласка, вкажіть місто відправлення та місто прибуття!", "Помилка заповнення", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Зупиняємо виконання
            }

            // 3. ВАЛІДАЦІЯ: Формат дати та часу
            if (!datePicker.SelectedDate.HasValue ||
                string.IsNullOrWhiteSpace(timeStr) ||
                !timeStr.Contains(":") || // Перевіряємо наявність двокрапки, щоб "36" не пройшло
                !TimeSpan.TryParse(timeStr, out TimeSpan time) ||
                time.Days > 0) // Перевіряємо, щоб значення не виходило за межі 24 годин
            {
                MessageBox.Show("Введіть час у форматі ГГ:ХХ (наприклад, 14:30). Значення не може перевищувати 23:59.",
                                "Помилка формату", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Формуємо повну дату виїзду
            DateTime departureDateTime = datePicker.SelectedDate.Value.Date.Add(time);

            // 4. ВАЛІДАЦІЯ: Числові дані (Місця та Ціна)
            if (!int.TryParse(txtSeats.Text, out int seats) || !decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Кількість місць та ціна мають бути числовими значеннями.", "Помилка вводу", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Зупиняємо виконання
            }

            // 5. БІЗНЕС-ВАЛІДАЦІЯ (через TripValidator)

            // Перевірка на однакові міста
            if (!TripValidator.AreCitiesValid(from, to))
            {
                MessageBox.Show("Міста відправлення та прибуття не можуть збігатися.", "Валідація", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Перевірка на час у минулому
            if (!TripValidator.IsDateValid(departureDateTime))
            {
                MessageBox.Show("Дата поїздки не може бути у минулому.", "Валідація", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Перевірка кількості місць (1-50)
            if (!TripValidator.IsValidSeats(seats))
            {
                MessageBox.Show("Кількість місць повинна бути від 1 до 50.", "Валідація", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Перевірка ціни (>0)
            if (!TripValidator.IsValidPrice(price))
            {
                MessageBox.Show("Ціна повинна бути більшою за 0.", "Валідація", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 6. СТВОРЕННЯ ОБ'ЄКТА (якщо всі перевірки пройдено)
            var newTrip = new Trip
            {
                DriverLogin = _currentDriver.Login,
                DepartureCity = from,
                ArrivalCity = to,
                DepartureTime = departureDateTime,
                TotalSeats = seats,
                Price = price,
                Status = "Активна"
            };

            // 7. ЗБЕРЕЖЕННЯ
            if (_tripManager.CreateTrip(newTrip))
            {
                MessageBox.Show("Поїздку успішно опубліковано!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                _parentWindow.Show();
            }
            else
            {
                MessageBox.Show("Не вдалося зберегти поїздку. Перевірте з'єднання з файлом даних.", "Помилка збереження", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            _parentWindow.Show();
        }
    }
}