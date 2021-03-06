using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Xsolla.Core;

namespace Xsolla.Demo
{
	public partial class InventoryItemUI : MonoBehaviour
	{
		[SerializeField] private Image itemImage = default;
		[SerializeField] private GameObject loadingCircle = default;
		[SerializeField] private Text itemName = default;
		[SerializeField] private Text itemDescription = default;
		[SerializeField] private GameObject itemQuantityImage = default;
		[SerializeField] private Text itemQuantityText = default;
		[SerializeField] private ConsumeButton consumeButton = default;
		[SerializeField] private Text purchasedStatusText = default;
		[SerializeField] private GameObject remainingTimeObject = default;
		[SerializeField] private Text remainingTimeTimeText = default;
		[SerializeField] private GameObject expiredTimeObject = default;
		[SerializeField] private Text expiredTimeText = default;
#pragma warning disable 0414
		[SerializeField] private SimpleTextButton renewSubscriptionButton = default;
#pragma warning restore 0414

		private ItemModel _itemInformation;
		private IInventoryDemoImplementation _demoImplementation;

		private void Awake()
		{
			DisableConsumeButton();
		}

		public void Initialize(ItemModel itemInformation, IInventoryDemoImplementation demoImplementation)
		{
			_demoImplementation = demoImplementation;
			_itemInformation = itemInformation;

			itemName.text = _itemInformation.Name;
			itemDescription.text = _itemInformation.Description;

			LoadImage(_itemInformation.ImageUrl);

			if (_itemInformation.IsSubscription())
				AttachRenewSubscriptionHandler();
		}

		partial void AttachRenewSubscriptionHandler();

		private void LoadImage(string url)
		{
			if (!string.IsNullOrEmpty(url))
				ImageLoader.Instance.GetImageAsync(url, LoadImageCallback);
			else
			{
				Debug.LogError($"Inventory item with sku = '{_itemInformation.Sku}' have not image!");
				LoadImageCallback(string.Empty, null);
			}
		}

		private void LoadImageCallback(string url, Sprite image)
		{
			loadingCircle.SetActive(false);
			itemImage.gameObject.SetActive(true);
			itemImage.sprite = image;

			RefreshUi();
		}

		private void RefreshUi()
		{
			DisableQuantityImage();
			DisableConsumeButton();
			DisableExpirationText();
			DisablePurchasedStatusText();

			if (_itemInformation.IsSubscription())
				DrawSubscriptionItem();
			else
			{
				if (_itemInformation.IsConsumable)
					DrawConsumableVirtualItem();
				else
					DrawNonConsumableVirtualItem();
			}
		}

		private void DrawSubscriptionItem()
		{
			var model = UserInventory.Instance.Subscriptions.First(i => i.Sku.Equals(_itemInformation.Sku));
			if (model.Status != UserSubscriptionModel.SubscriptionStatusType.None && model.Expired.HasValue)
			{
				var isExpired = model.Status == UserSubscriptionModel.SubscriptionStatusType.Expired || model.Expired <= DateTime.Now;
				if (isExpired)
					EnableExpiredTimeText(GetPassedTime(model.Expired.Value));
				else
					EnableRemainingTimeText(GetRemainingTime(model.Expired.Value));
			}
			else
				EnablePurchasedStatusText(isPurchased: false);
		}

		private void EnableRemainingTimeText(string text)
		{
			remainingTimeObject.SetActive(true);
			expiredTimeObject.SetActive(false);

			remainingTimeTimeText.text = text;
		}

		private void EnableExpiredTimeText(string text)
		{
			remainingTimeObject.SetActive(false);
			expiredTimeObject.SetActive(true);

			expiredTimeText.text = $"Expired {text}";
		}

		private string GetRemainingTime(DateTime expiredDateTime)
		{
			var timeLeft = expiredDateTime - DateTime.Now;
			StartCoroutine(RemainingTimeCoroutine(timeLeft.TotalSeconds > 60 ? 60 : 1));
			if (timeLeft.TotalDays >= 30)
				return $"{(int) (timeLeft.TotalDays / 30)} month{(timeLeft.TotalDays > 60 ? "s" : "")} remaining";
			if (timeLeft.TotalDays > 1)
				return $"{(uint) (timeLeft.TotalDays)} day{(timeLeft.TotalDays > 1 ? "s" : "")} remaining";
			if (timeLeft.TotalHours > 1)
				return $"{(uint) (timeLeft.TotalHours)} hour{(timeLeft.TotalHours > 1 ? "s" : "")} remaining";
			if (timeLeft.TotalMinutes > 1)
				return $"{(uint) (timeLeft.TotalMinutes)} minute{(timeLeft.TotalMinutes > 1 ? "s" : "")} remaining";
			return $"{(uint) (timeLeft.TotalSeconds)} second{(timeLeft.TotalSeconds > 1 ? "s" : "")} remaining";
		}

		private string GetPassedTime(DateTime expiredDateTime)
		{
			var timePassed = DateTime.Now - expiredDateTime;
			StartCoroutine(RemainingTimeCoroutine(timePassed.TotalSeconds > 60 ? 60 : 1));
			if (timePassed.TotalDays >= 30)
				return $"{(int) (timePassed.TotalDays / 30)} month{(timePassed.TotalDays > 60 ? "s" : "")} ago";
			if (timePassed.TotalDays > 1)
				return $"{(uint) (timePassed.TotalDays)} day{(timePassed.TotalDays > 1 ? "s" : "")} ago";
			if (timePassed.TotalHours > 1)
				return $"{(uint) (timePassed.TotalHours)} hour{(timePassed.TotalHours > 1 ? "s" : "")} ago";
			if (timePassed.TotalMinutes > 1)
				return $"{(uint) (timePassed.TotalMinutes)} minute{(timePassed.TotalMinutes > 1 ? "s" : "")} ago";
			return $"{(uint) (timePassed.TotalSeconds)} second{(timePassed.TotalSeconds > 1 ? "s" : "")} ago";
		}

		private IEnumerator RemainingTimeCoroutine(float waitSeconds)
		{
			yield return new WaitForSeconds(waitSeconds);
			RefreshUi();
		}

		private void DrawConsumableVirtualItem()
		{
			var model = UserInventory.Instance.VirtualItems.First(i => i.Sku.Equals(_itemInformation.Sku));
			DrawItemsCount(model);
			EnableConsumeButton();
		}

		private void DrawNonConsumableVirtualItem()
		{
			var model = UserInventory.Instance.VirtualItems.First(i => i.Sku.Equals(_itemInformation.Sku));
			DrawItemsCount(model);
			EnablePurchasedStatusText(isPurchased: true);
		}

		private void DrawItemsCount(InventoryItemModel model)
		{
			if (model.RemainingUses == null || itemQuantityImage == null) return;
			EnableQuantityImage();
			itemQuantityText.text = model.RemainingUses.Value.ToString();
		}

		private void EnableQuantityImage()
		{
			itemQuantityImage.SetActive(true);
		}

		private void DisableQuantityImage()
		{
			itemQuantityImage.SetActive(false);
		}

		private void EnablePurchasedStatusText(bool isPurchased)
		{
			if (purchasedStatusText != null)
			{
				purchasedStatusText.text = isPurchased ? "Purchased" : "Not purchased";
				purchasedStatusText.gameObject.SetActive(true);
			}
		}

		private void DisablePurchasedStatusText()
		{
			if (purchasedStatusText != null)
				purchasedStatusText.gameObject.SetActive(false);
		}

		private void DisableExpirationText()
		{
			remainingTimeObject.SetActive(false);
		}

		private void EnableConsumeButton()
		{
			consumeButton.gameObject.SetActive(true);
			consumeButton.onClick = ConsumeHandler;
			if (consumeButton.counter < 1)
				consumeButton.counter.IncreaseValue(1 - consumeButton.counter.GetValue());
			consumeButton.counter.ValueChanged += Counter_ValueChanged;
		}

		private void DisableConsumeButton()
		{
			consumeButton.counter.ValueChanged -= Counter_ValueChanged;
			consumeButton.gameObject.SetActive(false);
		}

		private void ConsumeHandler()
		{
			var model = UserInventory.Instance.VirtualItems.First(i => i.Sku.Equals(_itemInformation.Sku));
			DisableConsumeButton();
			_demoImplementation.ConsumeInventoryItem(model, consumeButton.counter.GetValue(),
				_ => UserInventory.Instance.Refresh(), _ => EnableConsumeButton());
		}

		private void Counter_ValueChanged(int newValue)
		{
			var model = UserInventory.Instance.VirtualItems.First(i => i.Sku.Equals(_itemInformation.Sku));
			if (newValue > model.RemainingUses)
			{
				var delta = (int) model.RemainingUses - newValue;
				StartCoroutine(ChangeConsumeQuantityCoroutine(delta));
			}
			else
			{
				if (newValue == 0)
					StartCoroutine(ChangeConsumeQuantityCoroutine(1));
			}
		}

		private IEnumerator ChangeConsumeQuantityCoroutine(int deltaValue)
		{
			yield return new WaitForEndOfFrame();
			if (deltaValue < 0)
				consumeButton.counter.DecreaseValue(-deltaValue);
			else
				consumeButton.counter.IncreaseValue(deltaValue);
		}
	}
}
