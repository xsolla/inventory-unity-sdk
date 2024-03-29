using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using Xsolla.Core.Popup;

namespace Xsolla.Demo
{
	public class InventoryPageStoreItemsController : BasePageStoreItemsController
	{
		[SerializeField] private SimpleButton RefreshInventoryButton = default;
	
		private string _lastGroup;

		protected override bool IsShowContent => UserInventory.Instance.HasVirtualItems || UserInventory.Instance.HasPurchasedSubscriptions;

		protected override void Initialize()
		{
			UserInventory.Instance.RefreshEvent += OnUserInventoryRefresh;

			if (RefreshInventoryButton)
				RefreshInventoryButton.onClick += RefreshInventory;
		}

		private void OnDestroy()
		{
			//If UserInventory is not in destroying state (on app exit)
			if (UserInventory.Instance)
			{
				UserInventory.Instance.RefreshEvent -= OnUserInventoryRefresh;

				if (RefreshInventoryButton)
					RefreshInventoryButton.onClick -= RefreshInventory;
			}
		}

		private void RefreshInventory()
		{
			var isDone = false;
			PopupFactory.Instance.CreateWaiting().SetCloseCondition(() => isDone);

			UserInventory.Instance.Refresh(
				() => isDone = true,
				error =>
				{
					isDone = true;
					StoreDemoPopup.ShowError(error);
				}
			);
		}

		private void OnUserInventoryRefresh()
		{
			base.UpdatePage(_lastGroup);
		}

		protected override void InitializeItemUI(GameObject item, ItemModel model)
		{
			item.GetComponent<InventoryItemUI>().Initialize(model);
		}

		protected override List<ItemModel> GetItemsByGroup(string groupName)
		{
			_lastGroup = groupName;

			var predicate = groupName.Equals(GROUP_ALL) ? new Func<string, bool>(_ => true) : g => g.Equals(groupName);

			return UserInventory.Instance.AllItems.Where(i =>
			{
				if (i.IsVirtualCurrency())
					return false;
				else if (i.IsSubscription())
				{
					if (!UserCatalog.Instance.Subscriptions.Any(sub => sub.Sku.Equals(i.Sku)))
					{
						Debug.Log($"User subscription with sku = '{i.Sku}' have no equal catalog item!");
						return false;
					}

					var model = UserInventory.Instance.Subscriptions.First(x => x.Sku.Equals(i.Sku));
					if (!(model.Status != UserSubscriptionModel.SubscriptionStatusType.None && model.Expired.HasValue))
					{
						return false; //This is a non-purchased subscription
					}
				}
				else
				{
					if (!UserCatalog.Instance.VirtualItems.Any(cat => cat.Sku.Equals(i.Sku)))
					{
						Debug.Log($"Inventory item with sku = '{i.Sku}' have no equal catalog item!");
						return false;
					}
				}

				var catalogItem = UserCatalog.Instance.AllItems.First(cat => cat.Sku.Equals(i.Sku));
				var itemGroups = SdkCatalogLogic.Instance.GetCatalogGroupsByItem(catalogItem);

				if (itemGroups.Count == 1 && itemGroups[0] == BattlePassConstants.BATTLEPASS_GROUP)
					return false; //This is battlepass util item

				if (base.CheckHideInAttribute(i, HideInFlag.Inventory))
					return false; //This item must be hidden by attribute

				return itemGroups.Any(predicate);
			}).ToList();
		}

		protected override List<string> GetGroups()
		{
			var items = UserInventory.Instance.AllItems;

			var itemGroups = new HashSet<string>();

			foreach (var item in items)
			{
				if (item.IsSubscription() && item is UserSubscriptionModel subscription && subscription.Status == UserSubscriptionModel.SubscriptionStatusType.None)
				{
					//Do nothing, skip this item
					continue;
				}
				else
				{
					var currentItemGroups = SdkCatalogLogic.Instance.GetCatalogGroupsByItem(item);

					foreach (var group in currentItemGroups)
						itemGroups.Add(group);
				}
			}

			return itemGroups.ToList();
		}
	}
}
