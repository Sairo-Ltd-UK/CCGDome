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
using UnityEngine;

namespace CCG.CustomInput
{
	public static class RayCastHitProvider
	{
		public static bool ProvideRaycastHit(out RaycastHit hit, LayerMask layerMask, float distance)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			return Physics.Raycast(ray, out hit, distance, layerMask);
		}

		public static bool ProvideRaycastHit(Vector3 origin, Vector3 direction, out RaycastHit hit, LayerMask layerMask, float distance)
		{
			Ray ray = new Ray(origin, direction);
			return Physics.Raycast(ray, out hit, distance, layerMask);
		}
	}
}
