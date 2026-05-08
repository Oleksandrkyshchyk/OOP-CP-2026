namespace Carpooling.Core.Models
{
    public class Admin : User
    {
        public int AccessLevel { get; set; }

        public Admin()
        {
            throw new NotImplementedException();
            // Role = "Адміністратор";
            // AccessLevel = 1;
        }
    }
}