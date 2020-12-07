using System;
using System.Collections.Generic;

namespace Xsolla.Core
{
	public partial class Error
	{
		public static readonly Dictionary<string, ErrorType> ItemsListErrors =
			new Dictionary<string, ErrorType>()
			{
				{"422", ErrorType.InvalidData},
			};

		public static readonly Dictionary<string, ErrorType> ConsumeItemErrors =
			new Dictionary<string, ErrorType>()
			{
				{"422", ErrorType.InvalidData},
			};

		public static readonly Dictionary<string, ErrorType> CouponErrors =
			new Dictionary<string, ErrorType>()
			{
				{"401", ErrorType.InvalidToken},
				{"403", ErrorType.AuthorizationHeaderNotSent},
				{"404", ErrorType.InvalidCoupon},
				{"422", ErrorType.InvalidData},
			};
	}
}