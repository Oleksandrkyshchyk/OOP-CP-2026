using System;
using System.Windows;
using System.Windows.Controls;
using Carpooling.Core.Managers;
using Carpooling.Core.Models;
using Carpooling.Core.Validators;
using Carpooling.Core.Services;

namespace Carpooling.UI
{
    public partial class RegistrationWindow : Window
    {
        // Тільки оголошуємо змінну
        private UserManager _userManager;

        public RegistrationWindow()
        {
            InitializeComponent();

            // Ініціалізуємо менеджер, передаючи йому сховище (виправляє CS1729)
            // Використовуємо JsonDataStorage з вашого файлу JsonDataStorage.cs
            _userManager = new UserManager(new JsonDataStorage());
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Перевіряємо чи поля існують в XAML (x:Name="txtFullName" тощо)
            string fullName = txtFullName.Text.Trim();
            string login = txtLogin.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string password = txtPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;

            // Отримуємо текст обраної ролі
            string selectedRole = (cmbRole.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Валідація через ваш стат. клас UserValidator
            if (!UserValidator.IsValidLogin(login))
            {
                MessageBox.Show("Логін має бути від 3 символів без пробілів!", "Помилка");
                return;
            }

            if (!UserValidator.IsValidPassword(password))
            {
                MessageBox.Show("Пароль має містити від 8 символів, велику літеру та цифру!", "Слабкий пароль");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Паролі не збігаються!", "Помилка");
                return;
            }

            // Створення об'єкта через поліморфізм
            User newUser;
            if (selectedRole == "Водій")
            {
                newUser = new Driver { FullName = fullName, Login = login, Phone = phone, Password = password };
            }
            else
            {
                newUser = new Passenger { FullName = fullName, Login = login, Phone = phone, Password = password };
            }

            // Виклик реєстрації
            if (_userManager.Register(newUser))
            {
                MessageBox.Show("Реєстрація успішна!", "Успіх");
                btnBackToLogin_Click(null, null);
            }
            else
            {
                MessageBox.Show("Цей логін вже зайнятий!", "Помилка реєстрації");
            }
        }

        private void btnBackToLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}