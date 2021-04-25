using System;

namespace LD48.Utils
{
	public static class TimeUtils
	{
		public static string GetCurrentDateTime => DateTime.Now.ToString("HH:mm dd MMMM, yyyy");
	}
}