using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Xsolla.Core;
using Xsolla.Inventory;

public class SubscriptionItemUI : MonoBehaviour
{
	[SerializeField]
	Image itemImage;
	[SerializeField]
	GameObject loadingCircle;
	[SerializeField]
	Text itemName;
	[SerializeField]
	Text itemDescription;
	[SerializeField]
	Text itemStatus;
	[SerializeField]
	Text itemExpiration;

	SubscriptionItem _itemInformation;

	public void Initialize(SubscriptionItem itemInformation)
	{
		_itemInformation = itemInformation;

		itemName.text = _itemInformation.name;
		itemDescription.text = _itemInformation.description;

		itemStatus.text = _itemInformation.status.ToUpper();

		switch (itemInformation.Status)
		{
			case SubscriptionStatusType.None:
				itemStatus.text = "Subscription not purchased";
				break;
			case SubscriptionStatusType.Active:
				itemStatus.text = "Subscription active until";
				break;
			case SubscriptionStatusType.Expired:
				itemStatus.text = "Subscription expired at";
				break;
		}

		if (_itemInformation.expired_at != null && _itemInformation.Status != SubscriptionStatusType.None)
		{
			itemExpiration.text = UnixTimeToDateTime(_itemInformation.expired_at.Value).ToString("dd/MM/yyyy hh:mm:tt");
			itemExpiration.gameObject.SetActive(true);
		}

		ChangeImageUrl(_itemInformation);
	}

	private void ChangeImageUrl(SubscriptionItem itemInformation)
	{
		if (!string.IsNullOrEmpty(_itemInformation.image_url))
			ImageLoader.Instance.GetImageAsync(_itemInformation.image_url, LoadImageCallback);
		else
		{
			Debug.LogError($"Subscription item with sku = '{itemInformation.sku}' without image!");
			loadingCircle.SetActive(false);
			itemImage.sprite = null;
		}
	}

	void LoadImageCallback(string url, Sprite image)
	{
		loadingCircle.SetActive(false);
		itemImage.sprite = image;
	}
	
	private DateTime UnixTimeToDateTime(long unixTime)
	{
		DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		dtDateTime = dtDateTime.AddSeconds(unixTime).ToLocalTime();
		return dtDateTime;
	}
}