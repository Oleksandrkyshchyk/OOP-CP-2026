using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Carpooling.Core.Models;
using Carpooling.Core.Managers;
using Carpooling.Core.Services;
using Carpooling.Core.Validators;

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
            txtUserFullName.Text = _currentUser.FullName;
            txtUserRole.Text = _currentUser.Role;
            lblLogin.Text = _currentUser.Login;
            lblPhone.Text = _currentUser.Phone;

            iconDriver.Visibility = Visibility.Collapsed;
            iconPassenger.Visibility = Visibility.Collapsed;
            panelDriver.Visibility = Visibility.Collapsed;
            panelPassenger.Visibility = Visibility.Collapsed;
            txtEmptyDriver.Visibility = Visibility.Collapsed;
            txtEmptyPassenger.Visibility = Visibility.Collapsed;

            if (_currentUser.Role == "Водій")
            {
                iconDriver.Visibility = Visibility.Visible;
                panelDriver.Visibility = Visibility.Visible;

                if (_currentUser is Driver driver)
                {
                    txtCarModel.Text = driver.CarModel;
                    txtCarNumber.Text = driver.LicensePlate;
                }

                var driverTrips = _tripManager.GetAllTrips()
                    .Where(t => t.DriverLogin == _currentUser.Login)
                    .ToList();

                lstDriverTrips.ItemsSource = driverTrips;
                if (driverTrips.Count == 0) txtEmptyDriver.Visibility = Visibility.Visible;
            }
            else if (_currentUser.Role == "Пасажир")
            {
                iconPassenger.Visibility = Visibility.Visible;
                panelPassenger.Visibility = Visibility.Visible;

                var bookings = _tripManager.GetAllTrips()
                    .Where(t => t.Bookings != null &&
                                t.Bookings.Any(b => b.Passenger?.Login == _currentUser.Login))
                    .ToList();

                lstPassengerBookings.ItemsSource = bookings;
                if (bookings.Count == 0) txtEmptyPassenger.Visibility = Visibility.Visible;
            }
        }

        private void btnCancelBooking_Click(object sender, RoutedEventArgs e)
        {
            var trip = (sender as FrameworkElement)?.Tag as Trip;
            if (trip == null) return;

            if (MessageBox.Show("Скасувати це бронювання?", "Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var booking = trip.Bookings.FirstOrDefault(b => b.Passenger?.Login == _currentUser.Login);
                if (booking != null)
                {
                    trip.Bookings.Remove(booking);
                    if (_tripManager.UpdateTrip(trip))
                    {
                        MessageBox.Show("Бронювання скасовано.");
                        LoadUserData();
                    }
                }
            }
        }

        private void btnDeleteTrip_Click(object sender, RoutedEventArgs e)
        {
            var trip = (sender as FrameworkElement)?.Tag as Trip;
            if (trip == null) return;

            if (trip.DepartureTime < DateTime.Now)
            {
                MessageBox.Show("Неможливо видалити поїздку з минулого. Вона вже в історії.", "Обмеження");
                return;
            }

            if (MessageBox.Show("Видалити цю поїздку?", "Увага", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _tripManager.GetAllTrips().Remove(trip);
                if (_tripManager.SaveChanges())
                {
                    MessageBox.Show("Поїздку видалено.");
                    LoadUserData();
                }
            }
        }

        // --- Методи для редагування ---
        private void btnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            panelDriver.Visibility = Visibility.Collapsed;
            panelPassenger.Visibility = Visibility.Collapsed;
            panelEditProfile.Visibility = Visibility.Visible;
            editFullName.Text = _currentUser.FullName;
            editPhone.Text = _currentUser.Phone;
        }

        private void btnSaveProfile_Click(object sender, RoutedEventArgs e)
        {
            if (!UserValidator.IsValidFullName(editFullName.Text)) { MessageBox.Show("Некоректне ім'я!"); return; }

            var userInDb = _userManager.GetAllUsers().FirstOrDefault(u => u.Login == _currentUser.Login);
            if (userInDb != null)
            {
                userInDb.FullName = editFullName.Text;
                userInDb.Phone = editPhone.Text;
                if (!string.IsNullOrWhiteSpace(editPassword.Password)) userInDb.Password = editPassword.Password;

                _userManager.SaveChanges();
                _currentUser.FullName = userInDb.FullName;
                _currentUser.Phone = userInDb.Phone;

                _mainWindow?.UpdateUI();
                ExitEditMode();
            }
        }

        private void btnSaveCar_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser is Driver driver)
            {
                var driverInDb = _userManager.GetAllUsers().FirstOrDefault(u => u.Login == driver.Login) as Driver;
                if (driverInDb != null)
                {
                    driverInDb.CarModel = txtCarModel.Text;
                    driverInDb.LicensePlate = txtCarNumber.Text;
                    _userManager.SaveChanges();
                    driver.CarModel = driverInDb.CarModel;
                    driver.LicensePlate = driverInDb.LicensePlate;
                    MessageBox.Show("Дані авто збережено!");
                }
            }
        }

        private void btnCancelEdit_Click(object sender, RoutedEventArgs e) => ExitEditMode();
        private void ExitEditMode() { panelEditProfile.Visibility = Visibility.Collapsed; LoadUserData(); }
        private void btnBack_Click(object sender, RoutedEventArgs e) { _mainWindow?.Show(); this.Close(); }
        private void btnLogout_Click(object sender, RoutedEventArgs e) { new MainWindow(null).Show(); this.Close(); }
    }
}