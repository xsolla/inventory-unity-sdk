﻿using System;

namespace Xsolla.Inventory
{
	[Serializable]
	public class RedeemPromocodeRequest
	{
		public class Cart
		{
			public string id;
		}
		
		public string coupon_code;
		public Cart cart;
	}
}