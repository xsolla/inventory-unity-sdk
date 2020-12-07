﻿using UnityEngine;
using UnityEngine.UI;

public class OnInputFieldsButtonEnabler : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField] InputField[] InputFields;
	[SerializeField][Tooltip("Make sure number of rules matches number of input fields, each rule itself may be zero")] int[] MinSymbolsRules;
	[SerializeField] SimpleTextButtonDisableable Button;
#pragma warning restore 0649

	bool IsAllInputFieldsAreFilled
	{
		get
		{
			for (int i = 0; i < InputFields.Length; i++)
			{
				var currentCredentialText = InputFields[i].text;

				if(string.IsNullOrEmpty(currentCredentialText))
					return false;

				if(currentCredentialText.Length < MinSymbolsRules[i])
					return false;
			}

			/*else*/
			return true;
		}
	}

	private void Awake()
	{
		for (int i = 0; i < InputFields.Length; i++)
			InputFields[i].onValueChanged.AddListener(_ => UpdateButtonState());
	}

	private void Start()
	{
		UpdateButtonState();
	}

	private void UpdateButtonState()
	{
		if (IsAllInputFieldsAreFilled)
			Button.Enable();
		else
			Button.Disable();
	}
}
