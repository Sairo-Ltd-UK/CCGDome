// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     11/07/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using UnityEngine;

namespace CCG.XR
{
	public class XRLookAtCamera : MonoBehaviour
	{
		// There may be a few different ways/axis we want objects to look at cameera
		// Stick them all in one script with a number to trigger which type
		private Transform mainCameraTransform;
		public int lookAtMethod = 1;

		void LateUpdate()
		{
			if (mainCameraTransform == null)
			{
				mainCameraTransform = Camera.main.transform;
			}
			else
			{
				if (lookAtMethod == 1)
				{
					transform.rotation = Quaternion.LookRotation(transform.position - mainCameraTransform.position);
				}
				else if (lookAtMethod == 2)
				{
					transform.forward = mainCameraTransform.forward;
				}
				else if (lookAtMethod == 3)
				{
					transform.LookAt(mainCameraTransform);
				}
			}
		}
	}
}
