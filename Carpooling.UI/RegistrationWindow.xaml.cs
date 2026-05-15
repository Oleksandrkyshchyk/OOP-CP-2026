using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Carpooling.Core.Managers;
using Carpooling.Core.Models;
using Carpooling.Core.Validators;
using Carpooling.Core.Services;

namespace Carpooling.UI
{
    public partial class RegistrationWindow : Window
    {
        private UserManager _userManager;

        public RegistrationWindow()
        {
            InitializeComponent();
            _userManager = new UserManager(new JsonDataStorage());

            txtFullName.Focus();

            SetupEnterNavigation();
        }

        private void SetupEnterNavigation()
        {
            // Список полів для зручного переходу
            txtFullName.KeyDown += MoveFocusOnEnter;
            txtLogin.KeyDown += MoveFocusOnEnter;
            txtPhone.KeyDown += MoveFocusOnEnter;
            txtPassword.KeyDown += MoveFocusOnEnter;
            txtConfirmPassword.KeyDown += (s, e) => { if (e.Key == Key.Enter) btnRegister_Click(null, null); };
        }

        private void MoveFocusOnEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var request = new TraversalRequest(FocusNavigationDirection.Next);
                (sender as FrameworkElement)?.MoveFocus(request);
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string login = txtLogin.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string password = txtPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;
            string selectedRole = (cmbRole.SelectedItem as ComboBoxItem)?.Content.ToString();


            // Перевірка імені 
            if (string.IsNullOrWhiteSpace(fullName) || fullName.Length < 2)
            {
                MessageBox.Show("Будь ласка, введіть коректне ім'я та прізвище!", "Помилка");
                txtFullName.Focus();
                return;
            }

            if (!UserValidator.IsValidLogin(login))
            {
                MessageBox.Show("Логін має бути від 3 символів без пробілів!", "Помилка");
                txtLogin.Focus();
                return;
            }

            // 2. Перевірка телефону
            if (!UserValidator.IsValidPhone(phone))
            {
                MessageBox.Show("Введіть коректний номер телефону (наприклад, +380991234567)!", "Помилка");
                txtPhone.Focus();
                return;
            }

            if (!UserValidator.IsValidPassword(password))
            {
                MessageBox.Show("Пароль має містити від 8 символів, велику латинську літеру та цифру!", "Слабкий пароль");
                txtPassword.Focus();
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Паролі не збігаються!", "Помилка");
                return;
            }

            // Створення об'єкта
            User newUser;
            if (selectedRole == "Водій")
            {
                newUser = new Driver { FullName = fullName, Login = login, Phone = phone, Password = password };
            }
            else
            {
                newUser = new Passenger { FullName = fullName, Login = login, Phone = phone, Password = password };
            }

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