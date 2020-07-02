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
            Password = password;
            passwordHash = PasswordHasher.GenerateHash(password);
        }
        public string Name { get; set; }
        private string passwordHash = null;
        public string PasswordHash 
        {
            get => passwordHash ?? PasswordHasher.GenerateHash(Password);
        }
        public string Password { get; set; }

        ///<summary>
        ///Create the AuthModel by name and hash of a user entity
        ///</summary>
        public static AuthModel CreateFromUser(string name, string hash)
        {
            return new AuthModel
            {
                Name = name,
                passwordHash = hash
            };
        } 
    }
}