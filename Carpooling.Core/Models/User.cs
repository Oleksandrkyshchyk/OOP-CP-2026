namespace Carpooling.Core.Models
{
    // Абстрактний клас
    public abstract class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }

        // Метод-заглушка
        public virtual string GetRoleName()
        {
            throw new NotImplementedException();
        }

        // Метод для зміни пароля
        public virtual bool ChangePassword(string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}