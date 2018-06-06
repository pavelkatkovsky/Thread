using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ThreadIdTest.Controllers
{
	[Produces("application/json")]
	[Route("api/ThreadIdTest")]
	public class ThreadIdTestController : Controller
	{
		[Route("async")]
		public async Task<IEnumerable<string>> GetAsync()
		{
			var list = new List<string>();
			var path = $"{Directory.GetCurrentDirectory()}\\text.txt";

			list.Add(System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
			using (var reader = System.IO.File.OpenText(path))
			{
				var fileText = await reader.ReadToEndAsync();
				list.Add(System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
			}

			return list;
		}

		[Route("non-async")]
		public IEnumerable<string> Get()
		{
			var list = new List<string>();
			var path = $"{Directory.GetCurrentDirectory()}\\text.txt";

			list.Add(System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
			using (var reader = System.IO.File.OpenText(path))
			{
				var fileText = reader.ReadToEnd();
				list.Add(System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
			}

			return list;
		}
	}
}