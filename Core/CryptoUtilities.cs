using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Core {
	public static class CryptoUtilities {
		public static string CreateHash(string password) {
			var salt = ComputeRandomSalt(Constants.Cryptography.SALT_BYTE_SIZE);
			var hash = ComputePseudoRandomHash(password, salt, Constants.Cryptography.HASH_ITERATIONS, Constants.Cryptography.HASH_BYTE_SIZE);

			return
				Constants.Cryptography.HASH_ITERATIONS + ":" +
				Convert.ToBase64String(salt) + ":" +
				Convert.ToBase64String(hash);
		}

		private static byte[] ComputeRandomSalt(int saltByteSize) {
			using (var provider = new RNGCryptoServiceProvider()) {
				var salt = new byte[saltByteSize];

				provider.GetBytes(salt);

				return salt;
			}
		}

		private static byte[] ComputePseudoRandomHash(string password, byte[] salt, int iterations, int hashByteSize) {
			using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations)) {
				return deriveBytes.GetBytes(hashByteSize);
			}
		}

		public static string Encrypt(string plaintext, byte[] key, byte[] iv) {
			using (var algorithm = TripleDES.Create()) {
				using (var stream = new MemoryStream()) {
					using (var cryptoStream = new CryptoStream(stream, algorithm.CreateEncryptor(key, iv), CryptoStreamMode.Write)) {
						var bytes = Encoding.UTF8.GetBytes(plaintext);

						cryptoStream.Write(bytes, 0, bytes.Length);
					}

					return Convert.ToBase64String(stream.ToArray());
				}
			}
		}

		public static string Decrypt(string ciphertext, byte[] key, byte[] iv) {
			using (var algorithm = TripleDES.Create()) {
				using (var stream = new MemoryStream()) {
					using (var cryptoStream = new CryptoStream(stream, algorithm.CreateDecryptor(key, iv), CryptoStreamMode.Write)) {
						var bytes = Convert.FromBase64String(ciphertext);

						cryptoStream.Write(bytes, 0, bytes.Length);
					}

					return Encoding.UTF8.GetString(stream.ToArray());
				}
			}
		}

		public static bool ValidatePassword(string password, string correctHash) {
			var split = correctHash.Split(new[] {':'});
			var iterations = Int32.Parse(split[0]);
			var salt = Convert.FromBase64String(split[1]);
			var hash = Convert.FromBase64String(split[2]);

			var testHash = ComputePseudoRandomHash(password, salt, iterations, hash.Length);

			return SlowEquals(hash, testHash);
		}

		/// <summary>
		/// Compares two byte arrays in length-constant time. 
		/// This comparison method is used so that password hashes cannot be extracted from on-line systems using a timing attack and then attacked off-line.
		/// </summary>
		/// <param name="a">The first byte array.</param>
		/// <param name="b">The second byte array.</param>
		/// <returns>True if both byte arrays are equal. False otherwise.</returns>
		private static bool SlowEquals(byte[] a, byte[] b) {
			var diff = (uint) a.Length ^ (uint) b.Length;

			for (var i = 0; i < a.Length && i < b.Length; i++) {
				diff |= (uint) (a[i] ^ b[i]);
			}

			return diff == 0;
		}
	}
}