using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace ModelLayer.Models
{
    public class User
    {
        // Private backing field
        private string password;

        // Public properties
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name="Username")]
        public string UserName { get; set; }
        [Required]
        [Display(Name="Email Address")]
        [EmailAddress(ErrorMessage ="Email must be valid.")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        public string Password { get { return password; } set { this.password = value; } }
        [Display(Name="First Name")]
        public string FirstName { get; set; }
        [Display(Name="Last Name")]
        public string LastName { get; set; }
        [Display(Name="Profile Picture")]
        public byte[] ProfilePicture { get; set; }

        public string Description { get; set; }
        
        /// <summary>
        /// Takes a string, cryptographically secures the string through salting and hashing, and returns the secure string.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string HashPassword(string password)
        {
            int SaltByteSize = 24;
            int HashByteSize = 24;
            int HasingIterationsCount = 10101;
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, SaltByteSize, HasingIterationsCount))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(HashByteSize);
            }
            byte[] dst = new byte[(SaltByteSize + HashByteSize) + 1];
            Buffer.BlockCopy(salt, 0, dst, 1, SaltByteSize);
            Buffer.BlockCopy(buffer2, 0, dst, SaltByteSize + 1, HashByteSize);
            return Convert.ToBase64String(dst);
        }

        public User() { }
        public User(string userN, string passw, string email)
        {
            this.UserName = userN;
            this.Password = passw;
            this.Email = email;
        }
    }
}
