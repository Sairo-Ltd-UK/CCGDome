// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     23/06/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using UnityEngine;

namespace CCG.MiniGames
{
	public class MiniGameInteractionSwitch : MonoBehaviour
	{
		[Tooltip("Intractable instance of game to switch to")]
		[SerializeField] private MiniGameInteractable targetInteractable;

		private void OnTriggerEnter(Collider other)
		{
			//Debug.Log("Name: " + other.name);

			if (!other.TryGetComponent(out GenericMiniGameInteractor interactor))
				return;

			interactor.SetCurrentMiniGame(targetInteractable);
		}

		private void OnTriggerExit(Collider other)
		{
			//Debug.Log("Name: " + other.name);

			if (!other.TryGetComponent(out GenericMiniGameInteractor interactor))
				return;

			interactor.ClearCurrentMiniGame(targetInteractable);
		}
	}
}
