// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     12/08/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using UnityEngine;

namespace CCG.MiniGames
{
	public class MiniGameBase : MonoBehaviour
	{
		[Tooltip("The switch to change game instance on player")]
		[SerializeField] MiniGameInteractionSwitch interactableSwitch;
		
		private void OnValidate()
		{
			if (interactableSwitch == null)
			{
				Debug.LogWarning("MiniGameInteractionSwitch is not assigned!", this);
			}
		}
	}
}
