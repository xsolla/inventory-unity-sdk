using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xsolla.UIBuilder;

namespace Xsolla.Demo
{
	public abstract class BasePageStoreItemsController : MonoBehaviour
	{
		protected const string GROUP_ALL = "ALL";

		[SerializeField] protected WidgetProvider ItemPrefabProvider = new WidgetProvider();
		[SerializeField] protected GroupsController groupsController = default;
		[SerializeField] protected ItemContainer itemsContainer = default;
		
		private GameObject ItemPrefab => ItemPrefabProvider.GetValue();

		private void Start()
		{
			Initialize();

			groupsController.GroupSelectedEvent += ShowGroupItems;
			StartCoroutine(FillGroups());
		}

		protected void ShowGroupItems(string groupName)
		{
			var items = GetItemsByGroup(groupName);

			itemsContainer.Clear();
			items.ForEach(item =>
			{
				var itemGameObject = itemsContainer.AddItem(ItemPrefab);
				InitializeItemUI(itemGameObject, item);
			});
		}

		protected IEnumerator FillGroups()
		{
			yield return new WaitUntil(() => UserCatalog.Instance.IsUpdated);
			yield return new WaitUntil(() => UserInventory.Instance.IsUpdated);

			var groups = GetGroups();

			//Hide BattlePass group if any
			groups.Remove(BattlePassConstants.BATTLEPASS_GROUP);

			groupsController.RemoveAll();
			groupsController.AddGroup(GROUP_ALL);
			groups.ForEach(g => groupsController.AddGroup(g));

			groupsController.SelectDefault();
		}

		protected abstract void Initialize();
		protected abstract void InitializeItemUI(GameObject item, ItemModel model);
		protected abstract List<ItemModel> GetItemsByGroup(string groupName);
		protected abstract List<string> GetGroups();
	}
}
