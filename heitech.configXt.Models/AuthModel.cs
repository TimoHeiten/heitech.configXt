namespace heitech.configXt.Models
{
    public class AuthModel
    {
        ///<summary>
        ///for serialization
        ///</summary>
        public  AuthModel()
        {
            
        }
        public AuthModel(string name, string password)
        {
            Name = name;
            PasswordHash = PasswordHasher.GenerateHash(password);
        }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
    }
}