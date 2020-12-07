using System;
using UnityEngine;
using Xsolla.Core;
using Xsolla.Login;
using Xsolla.Inventory;

public partial class DemoImplementation : MonoBehaviour, IDemoImplementation
{
	private void Start()
	{
		DemoController.Instance.StateChangingEvent += (state, newState) =>
		{
			if (newState == MenuState.Main)
			{
				XsollaLogin.Instance.Token = XsollaInventory.Instance.Token = GetUserToken();
			}
		};
		ValidateXsollaSettings();
	}

	private void ValidateXsollaSettings()
	{
		if (string.IsNullOrEmpty(XsollaSettings.LoginId))
		{
			var errorMessage = "Please copy the Login project ID from your Publisher Account and add it to your project settings";
			GenerateLoginSettingsError(errorMessage);
			return;
		}

		if (string.IsNullOrEmpty(XsollaSettings.ProjectId))
		{
			var errorMessage = "Please copy the Store project ID from your Publisher Account and add it to your project settings";
			GenerateLoginSettingsError(errorMessage);
			return;
		}

		var isDefaultLoginID = XsollaSettings.LoginId == Constants.DEFAULT_LOGIN_ID || XsollaSettings.LoginId == Constants.DEFAULT_PLATFORM_LOGIN_ID;
		var isDefaultProjectID = XsollaSettings.ProjectId == Constants.DEFAULT_PROJECT_ID;

		if (isDefaultLoginID && !isDefaultProjectID)
		{
			var errorMessage = $"You changed [XsollaSettings->ProjectID] to '{XsollaSettings.ProjectId}', but did not change LoginID. Change LoginID from '{XsollaSettings.LoginId}' to correct value.";
			GenerateLoginSettingsError(errorMessage);
		}
		else if (!isDefaultLoginID && isDefaultProjectID)
		{
			var errorMessage = $"You changed [XsollaSettings->LoginID] to '{XsollaSettings.LoginId}', but did not change ProjectID. Change ProjectID from '{XsollaSettings.ProjectId}' to correct value.";
			GenerateLoginSettingsError(errorMessage);
		}
	}

	private void GenerateLoginSettingsError(string errorMessage)
	{
		var proxyObject = new GameObject();
		var proxyScript = proxyObject.AddComponent<LoginSettingsErrorHolder>();
		proxyScript.LoginSettingsError = errorMessage;
		DemoController.Instance.SetState(MenuState.LoginSettingsError);
	}

	public Token GetUserToken()
	{
		if (Token == null || Token.IsNullOrEmpty())
			Debug.LogError("Token was not obtained before use");

		return Token;
	}

	private Action<Error> WrapErrorCallback(Action<Error> onError)
	{
		return error =>
		{
			StoreDemoPopup.ShowError(error);
			onError?.Invoke(error);
		};
	}
}
