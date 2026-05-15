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
    public partial class AdminWindow : Window
    {
        private UserManager _userManager;
        private TripManager _tripManager;
        private User _currentAdmin;
        private Window _parentWindow;

        public AdminWindow(User admin, Window parentWindow)
        {
            InitializeComponent();
            _currentAdmin = admin;
            _parentWindow = parentWindow;

            var storage = new JsonDataStorage();
            _userManager = new UserManager(storage);
            _tripManager = new TripManager(storage);

            RefreshData();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            _parentWindow?.Show();
            this.Close();
        }

        private void RefreshData()
        {
            try
            {
                var users = _userManager.GetAllUsers();
                var trips = _tripManager.GetAllTrips();

                dgUsers.ItemsSource = null;
                dgUsers.ItemsSource = users;

                dgTrips.ItemsSource = null;
                dgTrips.ItemsSource = trips;

                txtStatUsers.Text = users.Count.ToString();
                txtStatTrips.Text = trips.Count(t => t.Status == "Активна" && t.DepartureTime >= DateTime.Now).ToString();

                decimal totalRevenue = trips.Sum(t => (t.Bookings?.Sum(b => b.SeatsCount) ?? 0) * t.Price);
                txtStatMoney.Text = $"{totalRevenue:N0} грн";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка оновлення даних: {ex.Message}");
            }
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            var user = (sender as FrameworkElement)?.Tag as User;
            if (user == null) return;

            string newPass = Microsoft.VisualBasic.Interaction.InputBox(
                $"Новий пароль для {user.Login} (мін. 8 символів, цифра та велика літера):",
                "Зміна пароля",
                user.Password);

            if (!string.IsNullOrWhiteSpace(newPass))
            {
                // ВИПРАВЛЕНО: Використовуємо твій валідатор
                if (UserValidator.IsValidPassword(newPass))
                {
                    user.Password = newPass;
                    _userManager.SaveChanges();
                    RefreshData();
                    MessageBox.Show($"Пароль для {user.Login} успішно змінено та збережено.", "Успіх");
                }
                else
                {
                    MessageBox.Show("Пароль не відповідає вимогам безпеки!\n" +
                                    "- Мінімум 8 символів\n" +
                                    "- Хоча б одна велика літера\n" +
                                    "- Хоча б одна цифра",
                                    "Помилка валідації", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var user = (sender as FrameworkElement)?.Tag as User;
            if (user == null) return;

            if (user.Login == _currentAdmin.Login)
            {
                MessageBox.Show("Ви не можете видалити власний акаунт!");
                return;
            }

            if (user.Role == "Водій")
            {
                bool hasActive = _tripManager.GetAllTrips()
                    .Any(t => t.DriverLogin == user.Login && t.Status == "Активна" && t.DepartureTime >= DateTime.Now);
                if (hasActive)
                {
                    MessageBox.Show("Неможливо видалити водія з активними поїздками!");
                    return;
                }
            }

            if (MessageBox.Show($"Видалити {user.Login}?", "Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _userManager.GetAllUsers().Remove(user);
                _userManager.SaveChanges();
                RefreshData();
            }
        }

        private void btnAdminCancelTrip_Click(object sender, RoutedEventArgs e)
        {
            var trip = (sender as FrameworkElement)?.Tag as Trip;
            if (trip == null) return;

            if (MessageBox.Show("Скасувати поїздку?", "Модерація", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                trip.ChangeStatus("Скасована");
                _tripManager.UpdateTrip(trip);
                RefreshData();
            }
        }

        private void btnAdminFullDeleteTrip_Click(object sender, RoutedEventArgs e)
        {
            var trip = (sender as FrameworkElement)?.Tag as Trip;
            if (trip == null) return;

            if (MessageBox.Show("Видалити поїздку назавжди?", "Повне видалення", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _tripManager.GetAllTrips().Remove(trip);
                _tripManager.SaveChanges();
                RefreshData();
            }
        }

        private void btnSaveJson_Click(object sender, RoutedEventArgs e)
        {
            _userManager.SaveChanges();
            _tripManager.SaveChanges();
            MessageBox.Show("Дані збережені в JSON.");
        }

        private void btnLoadJson_Click(object sender, RoutedEventArgs e)
        {
            var storage = new JsonDataStorage();
            _userManager = new UserManager(storage);
            _tripManager = new TripManager(storage);
            RefreshData();
            MessageBox.Show("Дані завантажені.");
        }

        private void btnViewUsers_Click(object sender, RoutedEventArgs e)
        {
            panelUsers.Visibility = Visibility.Visible;
            panelTrips.Visibility = Visibility.Collapsed;
        }

        private void btnViewTrips_Click(object sender, RoutedEventArgs e)
        {
            panelUsers.Visibility = Visibility.Collapsed;
            panelTrips.Visibility = Visibility.Visible;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }
    }
}