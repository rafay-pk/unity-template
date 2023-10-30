using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Enhancements.ExtensionMethods
{
	public static class StringExtensions
	{
		public static string Join(this IEnumerable<string> list)
		{
			var stringBuilder = new StringBuilder();
			foreach (var str in list)
			{
				stringBuilder.Append(str);
			}
			return stringBuilder.ToString();
		}

		public static string[] GetFileNames(this IEnumerable<string> list)
		{
			return list.Select(Path.GetFileNameWithoutExtension).ToArray();
		}
	}
}