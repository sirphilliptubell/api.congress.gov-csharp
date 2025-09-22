using Congress.Gov.CSharp.Clients.Congress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Congress.Gov.CSharp.Tests.Clients.Congress {
	internal class CongressesClientTests {

		private static ICongressesClient GetSut() => new CongressClient().Congresses;

		[Test]
		public async Task Test() {
			var sut = GetSut();
			int count = 0;
			await foreach (var items in sut.ListAsync(1000)) {
				Console.WriteLine(items.ToString());
				count++;
				if (count > 20) break; // Prevents sending too many requests and avoids reusing request objects
			}
		}
	}
}
