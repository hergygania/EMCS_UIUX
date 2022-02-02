using App;

namespace System
{
	public static class StringExtension
	{
		public static bool EqualsTo(this string current, string other)
		{
			return Fix(current).Equals(Fix(other), StringComparison.CurrentCultureIgnoreCase);
		}

		public static bool StartsWithTo(this string current, string other)
		{
			return Fix(current).StartsWith(Fix(other), StringComparison.CurrentCultureIgnoreCase);
		}

		public static string SubstringTo(this string current, int startIndex, int length)
		{

			if(startIndex < 0)
				throw new ArgumentException("startIndex must be equal or more than zero");
			if(length < 0)
				throw new ArgumentException("length must be equal or more than zero");

			if(current == null || current.Length <= startIndex)
				return string.Empty;
			if(current.Length < startIndex + length)
				return current.Substring(startIndex);

			return current.Substring(startIndex, length);
		}

		public static string SubstringToEnd(this string current, int startIndex)
		{
			if(startIndex < 0)
				throw new ArgumentException("startIndex must be equal or more than zero");
			if(current == null || current.Length <= startIndex)
				return string.Empty;

			return current.Substring(startIndex);
		}

		private static string Fix(string str)
		{
			return str == null ? string.Empty : str.Trim();
		}

		public static string Validate(this string current, string errMsg)
		{
			if(string.IsNullOrEmpty(current))
				throw new NullReferenceException(errMsg);
			return current;
		}

		public static string ValidateNullEmptyArg(this string arg, string errMsg)
		{
			if(string.IsNullOrEmpty(arg))
				throw new ArgumentNullEmptyException(errMsg);
			return arg;
		}
	}
}
