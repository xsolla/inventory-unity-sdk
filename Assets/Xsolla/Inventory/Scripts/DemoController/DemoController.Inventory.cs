using Xsolla.Core;

namespace Xsolla.Demo
{
	public partial class DemoController : MonoSingleton<DemoController>
	{
		private TutorialManager _tutorialManager = default;

		partial void InitTutorial()
		{
			if (DemoMarker.IsInventoryDemo)
				_tutorialManager = GetComponent<TutorialManager>();

			if (_tutorialManager != null)
				IsTutorialAvailable = true;
		}

		partial void AutoStartTutorial()
		{
			if (IsTutorialAvailable && !IsAccessTokenAuth)
			{
				if (!_tutorialManager.IsTutorialCompleted())
					_tutorialManager.ShowTutorial();
				else
					Debug.Log("Skipping tutorial since it was already completed.");
			}
			else
				Debug.Log("Tutorial is not available for this demo.");
		}

		partial void ManualStartTutorial(bool showWelcomeMessage)
		{
			_tutorialManager.ShowTutorial(showWelcomeMessage);
		}

		partial void UpdateInventory()
		{
			if (!UserInventory.IsExist)
				UserInventory.Instance.Init();

			if (!DemoMarker.IsStorePartAvailable)
			{
				UserCatalog.Instance.Init();
				UserCatalog.Instance.UpdateItems(
					onSuccess: () => UserInventory.Instance.Refresh(onError: StoreDemoPopup.ShowError),
					onError: error =>
					{
						Debug.LogError($"InventorySDK init failure: {error}");
						StoreDemoPopup.ShowError(error);
					});
			}
		}

		partial void DestroyInventory()
		{
			if (UserInventory.IsExist)
				Destroy(UserInventory.Instance.gameObject);
		}
	}
}
