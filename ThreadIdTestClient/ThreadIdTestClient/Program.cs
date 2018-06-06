using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ThreadIdTestClient
{
	public class Model
	{
		public TimeSpan Duration { get; set; }

		public DateTime StopTime { get; set; }

		public DateTime StartTime { get; set; }
	}

	public class SeperateClass
	{
		static int numberOfRequest = 300;
		public static Model LoadUrlsAsync()
		{
			var startTime = DateTime.Now;
			Console.WriteLine("LoadUrlsAsync Start - {0}", startTime);

			var tasks = new List<Task<string>>();

			for (int i = 0; i < numberOfRequest; i++)
			{
				var request = WebRequest.Create(@"http://localhost:51006/api/ThreadIdTest/async") as HttpWebRequest;
				request.Method = "GET";

				var task = LoadUrlAsync(request);
				tasks.Add(task);
			}

			var results = Task.WhenAll(tasks);
			var stopTime = DateTime.Now;
			var duration = (stopTime - startTime);

			return new Model { Duration = duration, StopTime = stopTime, StartTime = startTime };
		}

		async static Task<string> LoadUrlAsync(WebRequest request)
		{
			string value = string.Empty;
			using (var response = await request.GetResponseAsync())
			using (var responseStream = response.GetResponseStream())
			using (var reader = new StreamReader(responseStream))
			{
				value = reader.ReadToEnd();
				Console.WriteLine("{0} - Bytes: {1}. Thread Ids: {2}", request.RequestUri, value.Length, value);
			}

			return value;
		}
	}

	public class SeperateClassSync
	{
		static int numberOfRequest = 300;
		public static Model LoadUrlsSync()
		{
			var startTime = DateTime.Now;
			Console.WriteLine("LoadUrlsSync Start - {0}", startTime);

			var tasks = new List<Task<string>>();

			for (int i = 0; i < numberOfRequest; i++)
			{
				var request = WebRequest.Create(@"http://localhost:51006/api/ThreadIdTest/non-async") as HttpWebRequest;
				request.Method = "GET";

				var task = LoadUrlSync(request);
			}

			var stopTime = DateTime.Now;
			var duration = (stopTime - startTime);

			return new Model { Duration = duration, StopTime = stopTime, StartTime = startTime };
		}

		static string LoadUrlSync(WebRequest request)
		{
			string value = string.Empty;
			using (var response = request.GetResponse())//Still async FW, just changed to Sync call here
			using (var responseStream = response.GetResponseStream())
			using (var reader = new StreamReader(responseStream))
			{
				value = reader.ReadToEnd();
				Console.WriteLine("{0} - Bytes: {1}. Thread Ids: {2}", request.RequestUri, value.Length, value);
			}

			return value;
		}
	}


	class Program
	{
		static void Main(string[] args)
		{
			var test1 = SeperateClass.LoadUrlsAsync();
			Console.ReadLine();
			var test2 = SeperateClassSync.LoadUrlsSync();

			Console.WriteLine("LoadUrlsAsync Started - {0}", test1.StartTime);
			Console.WriteLine("LoadUrlsAsync Complete - {0}", test1.StopTime);
			Console.WriteLine("LoadUrlsAsync Duration - {0}ms", test1.Duration);

			Console.WriteLine("LoadUrlsSync Started - {0}", test1.StartTime);
			Console.WriteLine("LoadUrlsSync Complete - {0}", test2.StopTime);
			Console.WriteLine("LoadUrlsSync Duration - {0}ms", test2.Duration);

			Console.ReadLine();
		}
	}
}
