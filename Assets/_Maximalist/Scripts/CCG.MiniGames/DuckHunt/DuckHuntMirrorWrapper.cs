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

namespace CCG.MiniGames.Duckhunt
{
	public class DuckHuntMirrorWrapper : MiniGameInteractable
	{
		[SerializeField] private float shootCooldown = 0.25f;
		[SerializeField] private LayerMask duckLayer;

		public override void OnReciveRaycastHit(RaycastHit hit, string ownerID)
		{
			var duck = hit.collider.GetComponent<Duck>();

			if (duck != null)
				duck.OnHit();
		}
	}
}
