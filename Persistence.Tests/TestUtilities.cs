using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Persistence.Tests {
	public static class TestUtilities {
		// NOTE: This should no longer be needed once Xunit version 2 comes out
		public static async Task DoesNotThrowAsync(Func<Task> testCode) {
			await testCode();
			Assert.DoesNotThrow(() => { }); // Use xUnit's default behavior
		}

		// NOTE: This should no longer be needed once Xunit version 2 comes out
		public static async Task<T> ThrowsAsync<T>(Func<Task> testCode) where T : Exception {
			try {
				await testCode();
				Assert.Throws<T>(() => { }); // Use xUnit's default behavior
			}
			catch (T exception) {
				return exception;
			}

			return null;
		}

		public static Mock<DbSet<T>> CreateDbSetMock<T>(List<T> data) where T : class {
			var query = data.AsQueryable();
			var set = new Mock<DbSet<T>>();

			set
				.As<IDbAsyncEnumerable<T>>()
				.Setup(x => x.GetAsyncEnumerator())
				.Returns(new TestDbAsyncEnumerator<T>(data.GetEnumerator()));

			set
				.As<IQueryable<T>>()
				.Setup(x => x.Provider)
				.Returns(new TestDbAsyncQueryProvider<T>(query.Provider));

			set
				.As<IQueryable<T>>()
				.Setup(x => x.Expression)
				.Returns(query.Expression);

			set
				.As<IQueryable<T>>()
				.Setup(x => x.ElementType)
				.Returns(query.ElementType);

			set
				.As<IQueryable<T>>()
				.Setup(x => x.GetEnumerator())
				.Returns(data.GetEnumerator());

			return set;
		}
	}
}