using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Xsolla.Core;
using Xsolla.Core.Popup;
using Xsolla.Inventory;

public partial class DemoImplementation : MonoBehaviour, IDemoImplementation
{
	private readonly Dictionary<string, List<string>> _itemsGroups = new Dictionary<string, List<string>>();

	public void GetInventoryItems(Action<List<InventoryItemModel>> onSuccess, Action<Error> onError = null)
	{
		XsollaInventory.Instance.GetInventoryItems(XsollaSettings.ProjectId, items =>
		{
			var inventoryItems = items.items.Where(i => !i.IsVirtualCurrency() && !i.IsSubscription()).ToList();
			inventoryItems.ToList().ForEach(i => AddItemGroups(i));
			var inventoryItemModels = inventoryItems.Select(i => new InventoryItemModel
			{
				Sku = i.sku,
				Description = i.description,
				Name = i.name,
				ImageUrl = i.image_url,
				IsConsumable = i.IsConsumable(),
				InstanceId = i.instance_id,
				RemainingUses = (uint?) i.quantity
			}).ToList();

			// Determine item groups for subscriptions separately. Aclual subscription items will be added in scope of GetUserSubscriptions method
			var subscriptionItems = items.items.Where(i => i.IsSubscription()).ToList();
			subscriptionItems.ToList().ForEach(i => AddItemGroups(i));

			onSuccess?.Invoke(inventoryItemModels);
		}, WrapErrorCallback(onError));
	}

	private void AddItemGroups(InventoryItem item)
	{
		var groups = item.groups.Select(g => g.name).ToList();
		if (!_itemsGroups.ContainsKey(item.sku))
			_itemsGroups.Add(item.sku, new List<string>());
		else
			groups = groups.Except(_itemsGroups[item.sku]).ToList();
		if (groups.Any())
			_itemsGroups[item.sku].AddRange(groups);
	}

	public void GetVirtualCurrencies(Action<List<VirtualCurrencyModel>> onSuccess, Action<Error> onError = null)
	{
		XsollaInventory.Instance.GetVirtualCurrencyList(XsollaSettings.ProjectId, items =>
		{
			var currencies = items.items.ToList();
			if (currencies.Any())
			{
				var result = currencies.Select(c =>
				{
					var model = new VirtualCurrencyModel();
					FillItemModel(model, c);
					return model;
				}).ToList();
				onSuccess?.Invoke(result);
			}
			else onSuccess?.Invoke(new List<VirtualCurrencyModel>());
		}, WrapErrorCallback(onError));
	}

	public void GetVirtualCurrencyBalance(Action<List<VirtualCurrencyBalanceModel>> onSuccess, Action<Error> onError = null)
	{
		XsollaInventory.Instance.GetVirtualCurrencyBalance(XsollaSettings.ProjectId, balances =>
		{
			var result = balances.items.ToList().Select(b => new VirtualCurrencyBalanceModel
			{
				Sku = b.sku,
				Description = b.description,
				Name = b.name,
				ImageUrl = b.image_url,
				IsConsumable = false,
				Amount = b.amount
			}).ToList();
			onSuccess?.Invoke(result);
		}, WrapErrorCallback(onError));
	}

	public void GetUserSubscriptions(Action<List<UserSubscriptionModel>> onSuccess, Action<Error> onError = null)
	{
		XsollaInventory.Instance.GetSubscriptions(XsollaSettings.ProjectId, items =>
		{
			var subscriptionItems = items.items.Select(i => new UserSubscriptionModel
			{
				Sku = i.sku,
				Description = i.description,
				Name = i.name,
				ImageUrl = i.image_url,
				IsConsumable = false,
				Status = GetSubscriptionStatus(i.status),
				Expired = i.expired_at.HasValue ? UnixTimeToDateTime(i.expired_at.Value) : (DateTime?) null
			}).ToList();
			onSuccess?.Invoke(subscriptionItems);
		}, WrapErrorCallback(onError));
	}

	private static UserSubscriptionModel.SubscriptionStatusType GetSubscriptionStatus(string status)
	{
		if (string.IsNullOrEmpty(status)) return UserSubscriptionModel.SubscriptionStatusType.None;
		switch (status)
		{
			case "active": return UserSubscriptionModel.SubscriptionStatusType.Active;
			case "expired": return UserSubscriptionModel.SubscriptionStatusType.Expired;
			default: return UserSubscriptionModel.SubscriptionStatusType.None;
		}
	}

	private DateTime UnixTimeToDateTime(long unixTime)
	{
		DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		dtDateTime = dtDateTime.AddSeconds(unixTime).ToLocalTime();
		return dtDateTime;
	}

	public void ConsumeVirtualCurrency(InventoryItemModel currency, uint count, Action onSuccess, Action onFailed = null)
	{
		StoreDemoPopup.ShowConsumeConfirmation(currency.Name, count, () =>
		{
			if (count > int.MaxValue)
			{
				var errorMessage = "Count exceeds max possible value";
				Debug.LogError("Count exceeds MaxValue");
				StoreDemoPopup.ShowError(new Error(errorMessage: errorMessage));
				onFailed?.Invoke();
				return;
			}

			var convertedCount = (int?) count;

			var isFinished = false;
			PopupFactory.Instance.CreateWaiting().SetCloseCondition(() => isFinished);
			SendConsumeItemRequest(currency, convertedCount,
				onSuccess: () =>
				{
					isFinished = true;
					onSuccess?.Invoke();
				},
				onError: WrapErrorCallback(_ =>
				{
					isFinished = true;
					onFailed?.Invoke();
				}));
		}, onFailed);
	}

	public void ConsumeInventoryItem(InventoryItemModel item, uint count, Action<InventoryItemModel> onSuccess, Action<InventoryItemModel> onFailed = null)
	{
		StoreDemoPopup.ShowConsumeConfirmation(item.Name, count, () => { StartCoroutine(ConsumeCoroutine(item, count, onSuccess, onFailed)); }, () => onFailed?.Invoke(item));
	}

	public void RedeemCouponCode(string couponCode, Action<List<CouponRedeemedItemModel>> onSuccess, Action<Error> onError)
	{
		var isFinished = false;
		PopupFactory.Instance.CreateWaiting()
			.SetCloseCondition(() => isFinished);

		SendRedeemCouponCodeRequest(couponCode, (redeemedItems) =>
		{
			isFinished = true;
			onSuccess?.Invoke(redeemedItems);
		}, WrapRedeemCouponErrorCallback(error =>
		{
			isFinished = true;
			onError?.Invoke(error);
		}));
	}

	IEnumerator ConsumeCoroutine(InventoryItemModel item, uint count, Action<InventoryItemModel> onSuccess, Action<InventoryItemModel> onFailed = null)
	{
		var isFinished = false;
		PopupFactory.Instance.CreateWaiting()
			.SetCloseCondition(() => isFinished);
		while (count-- > 0)
		{
			var busy = true;
			SendConsumeItemRequest(item, 1, () => busy = false, WrapErrorCallback(_ => onFailed?.Invoke(item)));
			yield return new WaitWhile(() => busy);
		}

		isFinished = true;
		onSuccess?.Invoke(item);
	}

	private void SendConsumeItemRequest(InventoryItemModel item, int? count, Action onSuccess, Action<Error> onError)
	{
		XsollaInventory.Instance.ConsumeInventoryItem(XsollaSettings.ProjectId, new ConsumeItem
		{
			sku = item.Sku,
			instance_id = item.InstanceId,
			quantity = count
		}, onSuccess, onError);
	}

	private void SendRedeemCouponCodeRequest(string couponCode, Action<List<CouponRedeemedItemModel>> onSuccess, Action<Error> onError)
	{
		XsollaInventory.Instance.RedeemCouponCode(XsollaSettings.ProjectId, new CouponCode {coupon_code = couponCode}, redeemedItems =>
		{
			var redeemedItemModels = redeemedItems.items.Select(
				i => new CouponRedeemedItemModel
				{
					Sku = i.sku,
					Description = i.description,
					Name = i.name,
					ImageUrl = i.image_url,
					Quantity = i.quantity,
				}).ToList();
			onSuccess?.Invoke(redeemedItemModels);
		}, onError);
	}

	private Action<Error> WrapRedeemCouponErrorCallback(Action<Error> onError)
	{
		return error =>
		{
			if (error.ErrorType != ErrorType.InvalidCoupon)
			{
				StoreDemoPopup.ShowError(error);
			}

			onError?.Invoke(error);
		};
	}
}