
namespace CRM.Core {
	using System;
	using System.Text;
	using System.Security.Cryptography;
	using System.IO;
	using System.Web.Security;

	public class SecurityManager {
		const string Password = "lottery";
		public static string Encrypt(string text) {
			var rijndaelCipher = new RijndaelManaged();
			byte[] plainText = Encoding.Unicode.GetBytes(text);
			byte[] salt = Encoding.ASCII.GetBytes(Password.Length.ToString());
			var secretKey = new PasswordDeriveBytes(Password, salt);

			//Creates a symmetric encryptor object.
			ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));
			var memoryStream = new MemoryStream();
			//Defines a stream that links data streams to cryptographic transformations
			var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
			cryptoStream.Write(plainText, 0, plainText.Length);
			//Writes the final state and clears the buffer
			cryptoStream.FlushFinalBlock();
			byte[] cipherBytes = memoryStream.ToArray();
			memoryStream.Close();
			cryptoStream.Close();
			string encryptedData = Convert.ToBase64String(cipherBytes);

			return encryptedData;

		}
		public static string EncryptQueryString(string text) {
			string q = Encrypt(text);

			return q.Replace("+", "_");
		}
		public static string DecryptQueryString(string text) {
			string q = text.Replace("_", "+");

			return Decrypt(q);
		}
		
		public static string Decrypt(string text) {
			string decryptedData = null;

			if (!string.IsNullOrEmpty(text)) {
				var rijndaelCipher = new RijndaelManaged();
				byte[] encryptedData = Convert.FromBase64String(text);
				byte[] salt = Encoding.ASCII.GetBytes(Password.Length.ToString());

				//Making of the key for decryption
				var secretKey = new PasswordDeriveBytes(Password, salt);

				//Creates a symmetric Rijndael decryptor object.
				ICryptoTransform decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));
				var memoryStream = new MemoryStream(encryptedData);

				//Defines the cryptographics stream for decryption.THe stream contains decrpted data
				var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
				byte[] plainText = new byte[encryptedData.Length];
				int decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);
				memoryStream.Close();
				cryptoStream.Close();

				//Converting to string
				decryptedData = Encoding.Unicode.GetString(plainText, 0, decryptedCount);
			}

			return decryptedData;
		}

		public static string GeneratePassword() {
			return Membership.GeneratePassword(6, 3);
		}

		public static string GenerateVerificationCode() {
			return Membership.GeneratePassword(8, 0);
		}

		public static bool IsExpired() {
			var targetDate = DateTime.Parse("15-jun-2012");
			return DateTime.Now.CompareTo(targetDate) > 0;
		}
	}
}
