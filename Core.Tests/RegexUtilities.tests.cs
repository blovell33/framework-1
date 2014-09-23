using Xunit;

namespace Core.Tests {
	public class RegexUtilitiesValidateEmailTests {
		[Fact]
		public void TestValidCase1() {
			// arrange/act
			var valid = RegexUtilities.ValidateEmail("david.jones@proseware.com");

			// assert
			Assert.True(valid);
		}

		[Fact]
		public void TestValidCase2() {
			// arrange/act
			var valid = RegexUtilities.ValidateEmail("d.j@server1.proseware.com");

			// assert
			Assert.True(valid);
		}

		[Fact]
		public void TestValidCase3() {
			// arrange/act
			var valid = RegexUtilities.ValidateEmail("jones@ms1.proseware.com");

			// assert
			Assert.True(valid);
		}

		[Fact]
		public void TestValidCase4() {
			// arrange/act
			var valid = RegexUtilities.ValidateEmail("j@proseware.com9");

			// assert
			Assert.True(valid);
		}

		[Fact]
		public void TestValidCase5() {
			// arrange/act
			var valid = RegexUtilities.ValidateEmail("js#internal@proseware.com");

			// assert
			Assert.True(valid);
		}

		[Fact]
		public void TestValidCase6() {
			// arrange/act
			var valid = RegexUtilities.ValidateEmail("j_9@[129.126.118.1]");

			// assert
			Assert.True(valid);
		}

		[Fact]
		public void TestValidCase7() {
			// arrange/act
			var valid = RegexUtilities.ValidateEmail("js@proseware.com9");

			// assert
			Assert.True(valid);
		}

		[Fact]
		public void TestValidCase8() {
			// arrange/act
			var valid = RegexUtilities.ValidateEmail("j.s@server1.proseware.com");

			// assert
			Assert.True(valid);
		}

		[Fact]
		public void TestValidCase9() {
			// arrange/act
			var valid = RegexUtilities.ValidateEmail("\"j\\\"s\\\"\"@proseware.com");

			// assert
			Assert.True(valid);
		}

		[Fact]
		public void TestValidCase10() {
			// arrange/act
			var valid = RegexUtilities.ValidateEmail("js@contoso.中国");

			// assert
			Assert.True(valid);
		}

		[Fact]
		public void TestInvalidCase1() {
			// arrange/act
			var valid = RegexUtilities.ValidateEmail("j.@server1.proseware.com");

			// assert
			Assert.False(valid);
		}

		[Fact]
		public void TestInvalidCase2() {
			// arrange/act
			var valid = RegexUtilities.ValidateEmail("j..s@proseware.com");

			// assert
			Assert.False(valid);
		}

		[Fact]
		public void TestInvalidCase3() {
			// arrange/act
			var valid = RegexUtilities.ValidateEmail("js*@proseware.com");

			// assert
			Assert.False(valid);
		}

		[Fact]
		public void TestInvalidCase4() {
			// arrange/act
			var valid = RegexUtilities.ValidateEmail("js@proseware..com");

			// assert
			Assert.False(valid);
		}

		[Fact]
		public void TestInvalidCase5() {
			// arrange/act
			var valid = RegexUtilities.ValidateEmail(null);

			// assert
			Assert.False(valid);
		}

		[Fact]
		public void TestInvalidCase6() {
			// arrange/act
			var valid = RegexUtilities.ValidateEmail(new string(' ', 10));

			// assert
			Assert.False(valid);
		}

		[Fact]
		public void TestInvalidCase7() {
			// arrange/act
			var valid = RegexUtilities.ValidateEmail("proseware.com");

			// assert
			Assert.False(valid);
		}
	}

	public class RegexUtilitiesValidateUrlPart {
		[Fact]
		public void TestValidCase1() {
			// arrange/act
			var valid = RegexUtilities.ValidateUrlPart("TH1Sis_a.test~");

			// assert
			Assert.True(valid);
		}

		[Fact]
		public void TestInvalidCase1() {
			// arrange/act
			var valid = RegexUtilities.ValidateUrlPart("TH1S is_a.test~");

			// assert
			Assert.False(valid);
		}

		[Fact]
		public void TestInvalidCase2() {
			// arrange/act
			var valid = RegexUtilities.ValidateUrlPart("TH1S-is_a.test~");

			// assert
			Assert.False(valid);
		}

		[Fact]
		public void TestInvalidCase3() {
			// arrange/act
			var valid = RegexUtilities.ValidateUrlPart("TH1S@is_a.test~");

			// assert
			Assert.False(valid);
		}

		[Fact]
		public void TestInvalidCase4() {
			// arrange/act
			var valid = RegexUtilities.ValidateUrlPart("TH1S?is_a.test~");

			// assert
			Assert.False(valid);
		}

		[Fact]
		public void TestInvalidCase5() {
			// arrange/act
			var valid = RegexUtilities.ValidateUrlPart("TH1S&is_a.test~");

			// assert
			Assert.False(valid);
		}
	}
}