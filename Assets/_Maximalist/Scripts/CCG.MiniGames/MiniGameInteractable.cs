// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     13/06/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using Mirror;
using UnityEngine;

namespace CCG.MiniGames
{
	public abstract class MiniGameInteractable: NetworkBehaviour
	{
		public abstract void OnReciveRaycastHit(RaycastHit hit);
	}
}
