using System;
using System.Collections.Generic;
using MonoTouch.Dialog;

namespace DFFBudget
{
	public class Global
	{
		public static List<Category> Categories;
		public static List<Operation> Incomes;
		public static List<Operation> Outcomes;
		public static RootElement CategoriesRoot;
		public static string Token;

		public Global()
		{
		}

		static public void PrepareCategories()
		{
			Categories = DataLayer.GetCategories();
			CategoriesRoot = new RootElement("Категория");
			var section = new Section();
			foreach (Category cCategory in Categories)
			{
				section.Add(new RadioElement(cCategory.Title){ Value = cCategory.Id.ToString() });
			}
			CategoriesRoot.Add(section);
		}
	}
}

