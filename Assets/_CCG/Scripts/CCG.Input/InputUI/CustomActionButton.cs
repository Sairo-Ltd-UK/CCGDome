// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     25/06/2025
//
//  Copyright @ 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using CCG.Core.Debuging;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CCG.CustomInput
{
	public class CustomActionButton : MonoBehaviour
	{
		[SerializeField] private CustomInputActionData actionData;
		[SerializeField] private UnityEvent onActionPressed;
		[Space]
		[SerializeField] private TextMeshProUGUI actionTextName;
		[SerializeField] private Image actionSprite;

		public void OnEnable()
		{
			if (actionData == null)
				return;

			DebugLogger.Log($"{name} is adding Invoke event to action");
			actionData.AddToInputActionReference(InvokeEvent);

			if(actionTextName)
				actionTextName.text = actionData.InputActionTextName;

			if(actionSprite)
				actionSprite.sprite = actionData.InputActionSprite;
		}

		public void OnDisable()
		{
			if (actionData == null)
				return;

			DebugLogger.Log($"{name} is removing Invoke event to action");
			actionData.RemoveFromInputActionReference(InvokeEvent);
		}

		private void InvokeEvent()
		{
			if(onActionPressed != null)
				onActionPressed.Invoke();
		}
	}
}

