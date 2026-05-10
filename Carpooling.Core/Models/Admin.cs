namespace Carpooling.Core.Models
{
    public class Admin : User
    {
        public int AccessLevel { get; set; }

        public Admin()
        {
            Role = "Адміністратор";
            AccessLevel = 1;
        }
    }
}