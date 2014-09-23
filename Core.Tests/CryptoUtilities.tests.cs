using System;
using System.Security.Cryptography;
using Xunit;

namespace Core.Tests {
	public class CryptoUtilitiesCreateHashTests {
		[Fact]
		public void TestDefaultBehavior() {
			// arrange/act
			var hash = CryptoUtilities.CreateHash("password");

			// assert
			var tokens = hash.Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries);

			Assert.Equal(3, tokens.Length);
			Assert.Equal(Constants.Cryptography.HASH_ITERATIONS, int.Parse(tokens[0]));
			Assert.Equal(Constants.Cryptography.SALT_BYTE_SIZE, Convert.FromBase64String(tokens[1]).Length);
			Assert.Equal(Constants.Cryptography.HASH_BYTE_SIZE, Convert.FromBase64String(tokens[2]).Length);
		}
	}

	public class CryptoUtilitiesEncryptTests {
		[Fact]
		public void TestDefaultBehavior() {
			// arrange
			var obj = new {
				plaintext = "secret"
			};

			string ciphertext;

			// act
			using (var algorithm = TripleDES.Create()) {
				ciphertext = CryptoUtilities.Encrypt(obj.plaintext, algorithm.Key, algorithm.IV);
			}

			// assert
			Assert.NotEqual(obj.plaintext, ciphertext);
		}
	}

	public class CryptoUtilitiesDecryptTests {
		[Fact]
		public void TestDefaultBehavior() {
			// arrange
			var obj = new {
				plaintext = "secret"
			};

			string plaintext;

			// act
			using (var algorithm = TripleDES.Create()) {
				var ciphertext = CryptoUtilities.Encrypt(obj.plaintext, algorithm.Key, algorithm.IV);

				plaintext = CryptoUtilities.Decrypt(ciphertext, algorithm.Key, algorithm.IV);
			}

			// assert
			Assert.Equal(obj.plaintext, plaintext);
		}
	}

	public class CryptoUtilitiesValidatePasswordTests {
		[Fact]
		public void TestValidPassword() {
			// arrange
			var obj = new {
				password = "password"
			};

			var hash = CryptoUtilities.CreateHash(obj.password);

			// act
			var valid = CryptoUtilities.ValidatePassword(obj.password, hash);

			// assert
			Assert.True(valid);
		}

		[Fact]
		public void TestInvalidPassword() {
			// arrange
			var hash = CryptoUtilities.CreateHash("password");

			// act
			var valid = CryptoUtilities.ValidatePassword("something else", hash);

			// assert
			Assert.False(valid);
		}
	}
}