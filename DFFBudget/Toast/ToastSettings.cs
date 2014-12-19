using System;
using System.Drawing;
using MonoTouch.UIKit;

namespace DFFBudget
{
	public class ToastSettings
	{
		public ToastSettings()
		{
			this.Duration = 500;
			this.Gravity = ToastGravity.Bottom;
		}

		public int Duration
		{
			get;
			set;
		}

		public double DurationSeconds
		{
			get { return (double)Duration / 1000; }

		}

		public ToastGravity Gravity
		{
			get;
			set;
		}

		public PointF Position
		{
			get;
			set;
		}
	}
}