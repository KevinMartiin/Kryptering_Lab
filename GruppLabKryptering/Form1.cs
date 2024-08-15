using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;

namespace GruppLabKryptering
{
    public partial class Form1 : Form
    {
        private const string password = "my-ian-password";
        //private readonly byte[] salt = Encoding.UTF8.GetBytes("your-hardcoded_salt");
        private static readonly int SaltSize = 16; // 16 bytes är vanligt för AES
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
            string plaintext = textBoxInput.Text;  // Input från användaren
            string encryptedText = EncryptString(plaintext, password);
            textBoxOutput.Text = encryptedText; // Visa det krypterade resultatet
            SaveEncryptedDataToDatabase(encryptedText, salt);
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
        //            ms.Write(salt, 0, salt.Length); // Skriv saltet i början av strömmen

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
        //        ms.Read(salt, 0, SaltSize); // Läs saltet från början av strömmen

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

        private void ShowAllDataButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlite("Data Source=encryptiondb.db").Options))
                {
                    var allData = context.message.ToList();

                    StringBuilder sb = new StringBuilder();
                    foreach (var data in allData)
                    {
                        // Konvertera byte-array till en Base64-sträng för att kunna visa den i MessageBox
                        string saltBase64 = Convert.ToBase64String(data.Salt);
                        sb.AppendLine($"ID: {data.Id}");
                        sb.AppendLine($"EncryptedText: {data.EncryptedText}");
                        sb.AppendLine($"Salt (Base64): {saltBase64}");
                        sb.AppendLine();  // Lägg till en tom rad mellan poster
                    }

                    MessageBox.Show(sb.ToString(), "All Data");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }
            //string connectionString = "Data Source=encryptiondb.db";

            //try
            //{
            //    using (var connection = new SQLiteConnection(connectionString))
            //    {
            //        connection.Open();

            //        DataTable schemaTable = connection.GetSchema("Tables");
            //        string tables = "Tables in the database:\n";

            //        foreach (DataRow row in schemaTable.Rows)
            //        {
            //            tables += row["TABLE_NAME"].ToString() + "\n";
            //        }

            //        MessageBox.Show(tables, "Database Tables");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Error: {ex.Message}", "Error");
            //}
        }
        private void SaveEncryptedDataToDatabase(string encryptedText, byte[] salt)
        {
            try
            {
                using (var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlite("Data Source=encryptiondb.db").Options))
                {
                    // Skapa en ny instans av EncryptedData
                    var encryptedData = new EncryptedData
                    {
                        EncryptedText = encryptedText,
                        Salt = salt
                    };

                    // Lägg till instansen i DbContext
                    context.message.Add(encryptedData);

                    // Spara ändringarna till databasen
                    context.SaveChanges();
                }
                MessageBox.Show("Data saved successfully.", "Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }
        }
    }
}
