// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     23/06/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using CCG.CustomInput;
using UnityEngine;

namespace CCG.MiniGames.Duckhunt
{
	public class DuckShooterSpatialWrapper : MonoBehaviour
	{
		[SerializeField] private float shootCooldown = 0.25f;
		[SerializeField] private CustomInputActionData mainPointerAction;
		[SerializeField] private LayerMask duckLayer;

		private void OnEnable()
		{
			mainPointerAction.AddToInputActionReference(TryShoot);
		}

		private void OnDisable()
		{
			mainPointerAction.RemoveFromInputActionReference(TryShoot);
		}


		private void TryShoot()
		{
			RaycastHit hit;

			if (RayCastHitProvider.ProvideRaycastHit(out hit, duckLayer, 100))
			{
				var duck = 
					hit.collider.GetComponent<Duck>();

				if (duck != null) 
					duck.OnHit();
			}
		}
	}
}

