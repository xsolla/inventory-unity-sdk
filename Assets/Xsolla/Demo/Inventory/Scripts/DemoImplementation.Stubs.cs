using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Xsolla.Core;
using Xsolla.Inventory;

public partial class DemoImplementation : MonoBehaviour, IDemoImplementation
{
	public void GetCatalogVirtualItems(Action<List<CatalogVirtualItemModel>> onSuccess, Action<Error> onError = null)
	{
		onSuccess?.Invoke(new List<CatalogVirtualItemModel>());
	}

	public void GetCatalogVirtualCurrencyPackages(Action<List<CatalogVirtualCurrencyModel>> onSuccess, Action<Error> onError = null)
	{
		onSuccess?.Invoke(new List<CatalogVirtualCurrencyModel>());
	}

	public void GetCatalogSubscriptions(Action<List<CatalogSubscriptionItemModel>> onSuccess, Action<Error> onError = null)
	{
		onSuccess?.Invoke(new List<CatalogSubscriptionItemModel>());
	}

	public void GetCatalogBundles(Action<List<CatalogBundleItemModel>> onSuccess, Action<Error> onError = null)
	{
		onSuccess?.Invoke(new List<CatalogBundleItemModel>());
	}

	private static void FillItemModel(ItemModel model, StoreItem item)
	{
		model.Sku = item.sku;
		model.Name = item.name;
		model.Description = item.description;
		model.ImageUrl = item.image_url;
	}

	public List<string> GetCatalogGroupsByItem(ItemModel item)
	{
		return _itemsGroups.ContainsKey(item.Sku) ? _itemsGroups[item.Sku] : new List<string>();
	}

	public void PurchaseForRealMoney(CatalogItemModel item, Action<CatalogItemModel> onSuccess = null, Action<Error> onError = null)
	{
		onSuccess?.Invoke(item);
	}

	public void PurchaseForVirtualCurrency(CatalogItemModel item, Action<CatalogItemModel> onSuccess = null, Action<Error> onError = null)
	{
		onSuccess?.Invoke(item);
	}

	public void PurchaseCart(List<UserCartItem> items, Action<List<UserCartItem>> onSuccess, Action<Error> onError = null)
	{
		onSuccess?.Invoke(new List<UserCartItem>());
	}

	public void GetUserFriends(Action<List<FriendModel>> onSuccess = null, Action<Error> onError = null)
	{
		onSuccess?.Invoke(new List<FriendModel>());
	}

	public void GetBlockedUsers(Action<List<FriendModel>> onSuccess = null, Action<Error> onError = null)
	{
		onSuccess?.Invoke(new List<FriendModel>());
	}

	public void GetPendingUsers(Action<List<FriendModel>> onSuccess = null, Action<Error> onError = null)
	{
		onSuccess?.Invoke(new List<FriendModel>());
	}

	public void GetRequestedUsers(Action<List<FriendModel>> onSuccess = null, Action<Error> onError = null)
	{
		onSuccess?.Invoke(new List<FriendModel>());
	}

	public void BlockUser(FriendModel user, Action<FriendModel> onSuccess = null, Action<Error> onError = null)
	{
		onSuccess?.Invoke(user);
	}

	public void UnblockUser(FriendModel user, Action<FriendModel> onSuccess = null, Action<Error> onError = null)
	{
		onSuccess?.Invoke(user);
	}

	public void SendFriendshipInvite(FriendModel user, Action<FriendModel> onSuccess = null, Action<Error> onError = null)
	{
		onSuccess?.Invoke(user);
	}

	public void RemoveFriend(FriendModel user, Action<FriendModel> onSuccess = null, Action<Error> onError = null)
	{
		onSuccess?.Invoke(user);
	}

	public void AcceptFriendship(FriendModel user, Action<FriendModel> onSuccess = null, Action<Error> onError = null)
	{
		onSuccess?.Invoke(user);
	}

	public void DeclineFriendship(FriendModel user, Action<FriendModel> onSuccess = null, Action<Error> onError = null)
	{
		onSuccess?.Invoke(user);
	}

	public void CancelFriendshipRequest(FriendModel user, Action<FriendModel> onSuccess = null, Action<Error> onError = null)
	{
		onSuccess?.Invoke(user);
	}

	public void ForceUpdateFriendsFromSocialNetworks(Action onSuccess = null, Action<Error> onError = null)
	{
		onSuccess?.Invoke();
	}

	public void GetFriendsFromSocialNetworks(Action<List<FriendModel>> onSuccess = null, Action<Error> onError = null)
	{
		onSuccess?.Invoke(new List<FriendModel>());
	}
}