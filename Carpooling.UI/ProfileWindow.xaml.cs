using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Carpooling.Core.Models;
using Carpooling.Core.Managers;
using Carpooling.Core.Services;
using Carpooling.Core.Validators;
using System.Windows.Controls;

namespace Carpooling.UI
{
    public partial class ProfileWindow : Window
    {
        private User _currentUser;
        private MainWindow _mainWindow;
        private UserManager _userManager;
        private TripManager _tripManager;

        public ProfileWindow(User user, Window mainWindow)
        {
            InitializeComponent();
            _currentUser = user;
            _mainWindow = mainWindow as MainWindow;

            var storage = new JsonDataStorage();
            _userManager = new UserManager(storage);
            _tripManager = new TripManager(storage);

            LoadUserData();
        }

        private void LoadUserData()
        {
            // Оновлюємо текстові поля профілю зліва
            txtUserFullName.Text = _currentUser.FullName;
            txtUserRole.Text = _currentUser.Role;
            lblLogin.Text = _currentUser.Login;
            lblPhone.Text = _currentUser.Phone;

            // Скидаємо видимість іконок та панелей перед перевіркою
            iconDriver.Visibility = Visibility.Collapsed;
            iconPassenger.Visibility = Visibility.Collapsed;
            panelDriver.Visibility = Visibility.Collapsed;
            panelPassenger.Visibility = Visibility.Collapsed;

            // Логіка для Водія
            if (_currentUser.Role == "Водій")
            {
                iconDriver.Visibility = Visibility.Visible;
                panelDriver.Visibility = Visibility.Visible;

                // Заповнення полів авто (якщо дані є)
                if (_currentUser is Driver driver)
                {
                    txtCarModel.Text = driver.CarModel;
                    txtCarNumber.Text = driver.LicensePlate;
                }

                // Завантажуємо поїздки, де цей користувач є водієм
                lstDriverTrips.ItemsSource = _tripManager.GetAllTrips()
                    .Where(t => t.DriverLogin == _currentUser.Login)
                    .ToList();
            }
            // Логіка для Пасажира
            else if (_currentUser.Role == "Пасажир")
            {
                iconPassenger.Visibility = Visibility.Visible;
                panelPassenger.Visibility = Visibility.Visible;

                // Шукаємо поїздки, у яких в списку бронювань є логін пасажира
                lstPassengerBookings.ItemsSource = _tripManager.GetAllTrips()
                    .Where(t => t.Bookings != null &&
                                t.Bookings.Any(b => b.Passenger != null && b.Passenger.Login == _currentUser.Login))
                    .ToList();
            }
        }

        private void btnSaveProfile_Click(object sender, RoutedEventArgs e)
        {
            string name = editFullName.Text.Trim();
            string phone = editPhone.Text.Trim();
            string pass = editPassword.Password;

            if (!UserValidator.IsValidFullName(name)) { MessageBox.Show("Некоректне ім'я!"); return; }
            if (!UserValidator.IsValidPhone(phone)) { MessageBox.Show("Некоректний телефон!"); return; }
            if (!string.IsNullOrWhiteSpace(pass) && !UserValidator.IsValidPassword(pass)) { MessageBox.Show("Слабкий пароль!"); return; }

            var allUsers = _userManager.GetAllUsers();
            var userInDb = allUsers.FirstOrDefault(u => u.Login == _currentUser.Login);

            if (userInDb != null)
            {
                userInDb.FullName = name;
                userInDb.Phone = phone;
                if (!string.IsNullOrWhiteSpace(pass)) userInDb.Password = pass;

                _userManager.SaveChanges();

                _currentUser.FullName = name;
                _currentUser.Phone = phone;

                if (_mainWindow != null) _mainWindow.UpdateUI();

                MessageBox.Show("Профіль оновлено!");
                ExitEditMode();
            }
        }

        private void btnSaveCar_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser is Driver driver)
            {
                var allUsers = _userManager.GetAllUsers();
                var driverInDb = allUsers.FirstOrDefault(u => u.Login == driver.Login) as Driver;

                if (driverInDb != null)
                {
                    driverInDb.CarModel = txtCarModel.Text.Trim();
                    driverInDb.LicensePlate = txtCarNumber.Text.Trim();
                    _userManager.SaveChanges();

                    driver.CarModel = driverInDb.CarModel;
                    driver.LicensePlate = driverInDb.LicensePlate;

                    MessageBox.Show("Дані авто збережено!");
                }
            }
        }

        private void btnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            panelDriver.Visibility = Visibility.Collapsed;
            panelPassenger.Visibility = Visibility.Collapsed;
            panelEditProfile.Visibility = Visibility.Visible;

            editFullName.Text = _currentUser.FullName;
            editPhone.Text = _currentUser.Phone;
            editPassword.Password = "";
        }

        private void btnCancelEdit_Click(object sender, RoutedEventArgs e) => ExitEditMode();

        private void ExitEditMode()
        {
            panelEditProfile.Visibility = Visibility.Collapsed;
            LoadUserData();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Ви дійсно бажаєте вийти з акаунту?", "Вихід", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MainWindow loginEntry = new MainWindow(null);
                loginEntry.Show();

                foreach (Window window in Application.Current.Windows)
                {
                    if (window != loginEntry)
                        window.Close();
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (_mainWindow != null)
            {
                _mainWindow.Show();
                _mainWindow.Focus();

                if (_mainWindow is MainWindow main)
                {
                    main.UpdateUI();
                }
            }
            this.Close();
        }

        private void btnCancelBooking_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            // Отримуємо об'єкт поїздки прямо з Tag, якщо в XAML ми прив'язали саму поїздку
            var trip = button?.Tag as Trip;

            if (trip == null) return;

            var result = MessageBox.Show("Ви впевнені, що хочете скасувати це бронювання?",
                                         "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // 1. Шукаємо бронювання саме цього користувача в цій поїздці
                var bookingToRemove = trip.Bookings?.FirstOrDefault(b => b.Passenger != null && b.Passenger.Login == _currentUser.Login);

                if (bookingToRemove != null)
                {
                    // 2. Видаляємо бронювання
                    trip.Bookings.Remove(bookingToRemove);

                    // 3. Оновлюємо дані в JSON через менеджер
                    if (_tripManager.UpdateTrip(trip))
                    {
                        MessageBox.Show("Бронювання успішно скасовано!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);

                        // 4. Перезавантажуємо дані, щоб поїздка зникла зі списку
                        LoadUserData();
                    }
                    else
                    {
                        MessageBox.Show("Помилка при збереженні змін у базі даних.");
                    }
                }
            }
        }
    }
}
