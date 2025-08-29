// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     23/06/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using CCG.Core.Debuging;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace CCG.CustomInput
{
	[CreateAssetMenu(fileName = "CustomInputActionData", menuName = "CustomInput/CustomInputActionData")]
	public class CustomInputActionData : ScriptableObject
	{
		[SerializeField] private InputActionReference inputActionReference;
		[Space]
		[SerializeField] private InputActionPlatformSpecificDataEntry pc;
		[SerializeField] private InputActionPlatformSpecificDataEntry vr;

		public string InputActionTextName { get => pc.InputActionTextName; }
		public Sprite InputActionSprite { get => pc.InputActionSprite; }

		private Dictionary<UnityAction, Action<InputAction.CallbackContext>> actionMap = new();

		public void AddToInputActionReference(UnityAction actionToAdd)
		{
			if (inputActionReference == null || actionToAdd == null)
				return;

			var inputAction = inputActionReference.action;

			if (inputAction == null)
				return;

			if (!inputAction.enabled)
			{
				inputAction.Enable();
			}

			if (actionMap.ContainsKey(actionToAdd))
				return;

			Action<InputAction.CallbackContext> wrapped = ctx => actionToAdd.Invoke();
			actionMap[actionToAdd] = wrapped;
			inputAction.performed += wrapped;
		}

		public void RemoveFromInputActionReference(UnityAction actionToRemove)
		{
			if (inputActionReference == null || actionToRemove == null)
				return;

			var inputAction = inputActionReference.action;

			if (inputAction == null)
				return;

			if (actionMap.TryGetValue(actionToRemove, out var wrapped))
			{
				inputAction.performed -= wrapped;
				actionMap.Remove(actionToRemove);
			}

			if (actionMap.Count == 0 && inputAction.enabled)
			{
				inputAction.Disable();
			}
		}

		private void OnDestroy()
		{
			//DisableAction();
		}

		private void OnDisable()
		{
			//DisableAction();
		}

		private void DisableAction()
		{
			if (inputActionReference)
			{
				inputActionReference.action.Disable();
			}
		}
	}
}

