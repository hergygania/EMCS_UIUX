using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace App
{
	public class ArgumentNullEmptyException : SystemException
	{
		public ArgumentNullEmptyException()
		{
		}
		public ArgumentNullEmptyException(string message) : base(message)
		{
		}
		protected ArgumentNullEmptyException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
		public ArgumentNullEmptyException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
