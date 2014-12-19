using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using MonoTouch.UIKit;
using System.Threading;

namespace DFFBudget
{
	public static class DataLayer
	{
		static HttpClient client;

		public static void Prepare()
		{
			client = new HttpClient();
			client.BaseAddress = new Uri("http://dutyfreeflowers.ru/api/");
			client.Timeout = new TimeSpan(0, 0, 5);
		}

		public static string Login(string email, string password)
		{
			string token = null;
			var response = client.GetAsync(String.Format("auth?method={0}&login={1}&pass={2}", "GET", email, password)).Result;

			Console.WriteLine("Login: " + response.Content.ReadAsStringAsync().Result);
			if (response.IsSuccessStatusCode)
			{
				token = response.Content.ReadAsStringAsync().Result;
			}
			else
			{
				Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.StatusCode);
				ReportError(response.Content.ReadAsStringAsync().Result);
			}

			return token;
		}

		public static List<Operation> GetOperations(string type)
		{
			List<Operation> incomes = null;

			var response = client.GetAsync(String.Format("{0}s?method={1}&token={2}", type, "GET", Global.Token)).Result;

			Console.WriteLine("GetOperations: " + response.Content.ReadAsStringAsync().Result);
			if (response.IsSuccessStatusCode)
			{
				var json = response.Content.ReadAsStringAsync().Result;
				incomes = JsonConvert.DeserializeObject<List<Operation>>(json);
			}
			else
			{
				Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.StatusCode);
				ReportError(response.Content.ReadAsStringAsync().Result);
			}

			return incomes;
		}

		public static List<Category> GetCategories()
		{
			List<Category> categories = null;

			var response = client.GetAsync(String.Format("categories?method={0}&token={1}", "GET", Global.Token)).Result;
			Console.WriteLine("GetCategories: " + response.Content.ReadAsStringAsync().Result);

			if (response.IsSuccessStatusCode)
			{
				var json = response.Content.ReadAsStringAsync().Result;
				categories = JsonConvert.DeserializeObject<List<Category>>(json);
			}
			else
			{
				Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.StatusCode);
				ReportError(response.Content.ReadAsStringAsync().Result);
			}

			return categories;
		}

		public static bool PutOperation(Operation operation, string type)
		{
			var result = client.PutAsync(String.Format("{0}s/{1}?method={2}&sum={3}&categoryId={4}&comment={5}&datetime={6}-{7}-{8}&token={9}", type, operation.Id, "PUT", operation.Sum, operation.CategoryId, operation.Comment, operation.Datetime.Year, operation.Datetime.Month, operation.Datetime.Day, Global.Token), null).Result;
			Console.WriteLine(result.Content.ReadAsStringAsync().Result);

			return true;
		}

		public static bool DeleteOperation(Operation operation, string type)
		{
			var result = client.PutAsync(String.Format("{0}s/{1}?method={2}&token={3}", type, operation.Id, "DELETE", Global.Token), null).Result;
			Console.WriteLine(result.Content.ReadAsStringAsync().Result);

			return true;
		}

		public static int AddOperation(Operation operation, string type)
		{
			var result = client.PutAsync(String.Format("{0}s?method={1}&sum={2}&categoryId={3}&datetime={4}-{5}-{6}&token={7}", type, "POST", operation.Sum, operation.CategoryId, operation.Datetime.Year, operation.Datetime.Month, operation.Datetime.Day, Global.Token), null).Result;
			Console.WriteLine(result.Content.ReadAsStringAsync().Result);

			JToken token = JObject.Parse(result.Content.ReadAsStringAsync().Result);

			int id = (int)token.SelectToken("id");

			return id;
		}

		public static bool PutCategory(Category category)
		{
			var result = client.PutAsync(String.Format("categories/{0}?method={1}&title={2}&token={3}", category.Id, "PUT", category.Title, Global.Token), null).Result;
			Console.WriteLine(result.Content.ReadAsStringAsync().Result);

			return true;
		}

		public static bool DeleteCategory(Category category)
		{
			var result = client.PutAsync(String.Format("categories/{0}?method={1}&token={2}", category.Id, "DELETE", Global.Token), null).Result;
			Console.WriteLine(result.Content.ReadAsStringAsync().Result);

			return true;
		}

		public static int AddCategory()
		{
			var result = client.PutAsync(String.Format("categories?method={0}&title={1}&token={2}", "POST", "Новая категория", Global.Token), null).Result;
			Console.WriteLine(result.Content.ReadAsStringAsync().Result);

			JToken token = JObject.Parse(result.Content.ReadAsStringAsync().Result);

			int id = (int)token.SelectToken("id");

			return id;
		}

		public static void ReportError(string msg)
		{
			var result = client.PutAsync(String.Format("log?method={0}&msg={1}&token={2}", "POST", msg, Global.Token), null).Result;
			Console.WriteLine(result.Content.ReadAsStringAsync().Result);
		}
	}
}

