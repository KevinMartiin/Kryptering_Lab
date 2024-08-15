using System.Security.Cryptography;
using System.Text;

namespace GruppLabKryptering
{
    public partial class Form1 : Form
    {
        private const string password = "my-ian-password";
        //private readonly byte[] salt = Encoding.UTF8.GetBytes("your-hardcoded_salt");
        private static readonly int SaltSize = 16; // 16 bytes �r vanligt f�r AES
        private byte[] salt;


        public Form1()
        {
            InitializeComponent();
            salt = GenerateSalt();
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private void EncryptButton_Click(object sender, EventArgs e)
        {
            string plaintext = textBoxInput.Text;  // Input fr�n anv�ndaren
            string encryptedText = EncryptString(plaintext, password);
            textBoxOutput.Text = encryptedText; // Visa det krypterade resultatet
        }
        public void decryptButton_Click(object sender, EventArgs e)
        {
            string encryptedText = textBoxInput.Text;
            string decryptedText = DecryptString(encryptedText, password);
            textBoxOutput.Text = decryptedText;

        }
        //private string EncryptString(string plaintext, string password)
        //{
        //    byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
        //    byte[] salt = GenerateSalt();

        //    using (var passwordBytes = new Rfc2898DeriveBytes(password, salt, 10000))
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        encryptor.Key = passwordBytes.GetBytes(32);
        //        encryptor.IV = passwordBytes.GetBytes(16);

        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            ms.Write(salt, 0, salt.Length); // Skriv saltet i b�rjan av str�mmen

        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(plaintextBytes, 0, plaintextBytes.Length);
        //                cs.FlushFinalBlock();
        //            }

        //            return Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //}
        //private string DecryptString(string encryptedText, string password)
        //{
        //    byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

        //    using (MemoryStream ms = new MemoryStream(encryptedBytes))
        //    {
        //        byte[] salt = new byte[SaltSize];
        //        ms.Read(salt, 0, SaltSize); // L�s saltet fr�n b�rjan av str�mmen

        //        using (var passwordBytes = new Rfc2898DeriveBytes(password, salt, 10000))
        //        using (Aes decryptor = Aes.Create())
        //        {
        //            decryptor.Key = passwordBytes.GetBytes(32);
        //            decryptor.IV = passwordBytes.GetBytes(16);

        //            using (CryptoStream cs = new CryptoStream(ms, decryptor.CreateDecryptor(), CryptoStreamMode.Read))
        //            using (StreamReader sr = new StreamReader(cs, Encoding.UTF8))
        //            {
        //                return sr.ReadToEnd();
        //            }
        //        }
        //    }
        //}

        private string EncryptString(string plaintext, string password)
        {
            // Convert the plaintext string to a byte array
            byte[] plaintextBytes = System.Text.Encoding.UTF8.GetBytes(plaintext);

            // Derive a new password using the PBKDF2 algorithm and a random salt
            Rfc2898DeriveBytes passwordBytes = new Rfc2898DeriveBytes(password, salt, 10000);

            // Use the password to encrypt the plaintext
            Aes encryptor = Aes.Create();
            encryptor.Key = passwordBytes.GetBytes(32);
            encryptor.IV = passwordBytes.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(plaintextBytes, 0, plaintextBytes.Length);
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
        private string DecryptString(string encryptedText, string password)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

            // Derive the password using the PBKDF2 algorithm
            Rfc2898DeriveBytes passwordBytes = new Rfc2898DeriveBytes(password, salt, 10000);

            // Use the password to decrypt the encrypted string
            Aes encryptor = Aes.Create();
            encryptor.Key = passwordBytes.GetBytes(32);
            encryptor.IV = passwordBytes.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(encryptedBytes, 0, encryptedBytes.Length);
                }
                return System.Text.Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}
