using UnityEngine;
using Xsolla.Core.Popup;

public class InventoryMainMenuController : BaseMenuController
{
	[SerializeField] private SimpleButton inventoryButton;
	[SerializeField] private SimpleButton webStoreButton;
	[SerializeField] private SimpleButton documentationButton;
	[SerializeField] private SimpleButton publisherAccountButton;
	[SerializeField] private SimpleButton logoutButton;
	[SerializeField] private SimpleButton tutorialButton;

	private void Start()
	{
		AttachButtonCallback(inventoryButton,
			() =>
			{
				if (UserInventory.Instance.IsUpdated)
				{
					UserInventory.Instance.Refresh(() => SetMenuState(MenuState.Inventory));
					PopupFactory.Instance.CreateWaiting().SetCloseCondition(() => UserInventory.Instance.IsUpdated);
				}
				else
				{
					SetMenuState(MenuState.Inventory, () => UserInventory.Instance.IsUpdated);
				}
			});
		AttachButtonCallback(logoutButton,
			() => SetMenuState(MenuState.Authorization));

		AttachUrlToButton(documentationButton, DemoController.Instance.UrlContainer.GetUrl(UrlType.DocumentationUrl));
		AttachUrlToButton(publisherAccountButton, DemoController.Instance.UrlContainer.GetUrl(UrlType.PublisherUrl));

		AttachUrlToButton(webStoreButton, DemoController.Instance.GetWebStoreUrl());

		AttachButtonCallback(tutorialButton,
			() =>
			{
				if (DemoController.Instance.IsTutorialAvailable)
					DemoController.Instance.TutorialManager.ShowTutorial(false);
			});
	}
}