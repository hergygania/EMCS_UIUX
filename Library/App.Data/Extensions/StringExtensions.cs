using System.Collections.Generic;

namespace App.Data
{
	public static class StringExtensions
	{
		public static string ConvertPascalCaseToFriendlyString(this string stringToConvert)
		{
			var cFriendlyName = new List<char>();
			var cName = stringToConvert.ToCharArray();
			for(var cIdx = 0; cIdx < cName.Length; cIdx++)
			{
				var c = cName[cIdx];
				if(cIdx > 0 && char.IsUpper(c))
				{
					cFriendlyName.Add(' ');
				}
				cFriendlyName.Add(c);
			}
			return new string(cFriendlyName.ToArray());
		}
	}
}
