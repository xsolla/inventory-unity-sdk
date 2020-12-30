using UnityEngine;
using UnityEngine.UI;
using Xsolla.Core;

namespace Xsolla.Demo
{
	public class BundleItemUI : MonoBehaviour
	{
		[SerializeField] private Image itemImage = default;
		[SerializeField] private Text itemName = default;
		[SerializeField] private Text itemDescription = default;
		[SerializeField] private Text itemQuantity = default;

		public void Initialize(BundleContentItem item)
		{
			itemName.text = item.Name;
			itemDescription.text = item.Description;
			itemQuantity.text = item.Quantity.ToString();

			if (!string.IsNullOrEmpty(item.ImageUrl))
			{
				ImageLoader.Instance.GetImageAsync(item.ImageUrl, LoadImageCallback);
			}
			else
			{
				Debug.LogError($"Bundle content item with sku = '{item.Sku}' without image!");
			}
		}

		void LoadImageCallback(string url, Sprite image)
		{
			if (itemImage)
				itemImage.sprite = image;
		}
	}
}
