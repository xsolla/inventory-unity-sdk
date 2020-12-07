using System.Collections.Generic;
using UnityEngine;
using Xsolla.Inventory;

public class SubscriptionItemContainer : MonoBehaviour, IContainer
{
	[SerializeField]
	GameObject itemPrefab;

	[SerializeField]
	Transform itemParent;

	private IDemoImplementation _demoImplementation;
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

	public void SetStoreImplementation(IDemoImplementation demoImplementation)
	{
		_demoImplementation = demoImplementation;
	}

	public void Refresh()
	{
		ClearItems();
		UserSubscriptions.Instance.GetItems().ForEach(AddItem);
	}
}
	