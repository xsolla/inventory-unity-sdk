using System.Collections.Generic;
using UnityEngine;
using Xsolla.Store;

namespace Xsolla.Demo
{
	public class SubscriptionItemContainer : MonoBehaviour, IContainer
	{
		[SerializeField]
		GameObject itemPrefab = default;

		[SerializeField]
		Transform itemParent = default;

		private List<GameObject> _items;

		private void Awake()
		{
			_items = new List<GameObject>();
		}

		public void AddItem(SubscriptionItem itemInformation)
		{
			var newItem = Instantiate(itemPrefab, itemParent);
			newItem.GetComponent<SubscriptionItemUI>().Initialize(itemInformation);
			_items.Add(newItem);
		}

		private void ClearItems()
		{
			_items.ForEach(Destroy);
			_items.Clear();
		}

		public void Refresh()
		{
			ClearItems();
			UserSubscriptions.Instance.GetItems().ForEach(AddItem);
		}
	}
}
