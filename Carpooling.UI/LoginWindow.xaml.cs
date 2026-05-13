using System;
using System.Windows;
using Carpooling.Core.Managers;
using Carpooling.Core.Services;
using Carpooling.Core.Models;

namespace Carpooling.UI
{
    public partial class LoginWindow : Window
    {
        private UserManager _userManager;

        public LoginWindow()
        {
            InitializeComponent();
            try
            {
                _userManager = new UserManager(new JsonDataStorage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка ініціалізації сховища: {ex.Message}", "Критична помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (_userManager == null) return;

            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password.Trim();

            // Перевірка на порожні поля
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Будь ласка, заповніть усі поля (логін та пароль)!", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = _userManager.Login(login, password);

            if (user != null)
            {
                // Успішний вхід
                MessageBox.Show($"Авторизація успішна! Вітаємо, {user.FullName}.\nВаша роль: {user.GetRoleName()}", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);

                // Тут буде відкриття MainWindow і передача туди об'єкта user
                MainWindow main = new MainWindow(user);
                main.Show();
                this.Close();
            }
            else
            {
                // Невірні дані
                MessageBox.Show("Користувача з такими даними не знайдено. Перевірте правильність логіна та пароля.", "Помилка входу", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Перехід до реєстрації
            RegistrationWindow regWindow = new RegistrationWindow();
            regWindow.Show();
            this.Close();
        }

        // Кнопка для гостьового входу
        private void btnGuest_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ви увійшли як Гість. Вам доступний лише загальний перегляд поїздок. Для бронювання, будь ласка, зареєструйтесь.", "Гостьовий доступ", MessageBoxButton.OK, MessageBoxImage.Information);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}