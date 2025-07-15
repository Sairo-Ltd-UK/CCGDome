//---------------------------------------------------------------
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
	public class XRPlayerRig : MonoBehaviour
	{
		public Transform rHandTransform;
		public Transform lHandTransform;
		public Transform headTransform;

		public Transform canvasUIPosition;

		public XRNetworkPlayerScript localVRNetworkPlayerScript;

		// switch to Late/Fixed Update if weirdness happens
		private void Update()
		{
			if (localVRNetworkPlayerScript)
			{
				// presuming you want a head object to sync, optional, same as hands.
				localVRNetworkPlayerScript.headTransform.position = headTransform.position;
				localVRNetworkPlayerScript.headTransform.rotation = headTransform.rotation;
				localVRNetworkPlayerScript.rHandTransform.position = rHandTransform.position;
				localVRNetworkPlayerScript.rHandTransform.rotation = rHandTransform.rotation;
				localVRNetworkPlayerScript.lHandTransform.position = lHandTransform.position;
				localVRNetworkPlayerScript.lHandTransform.rotation = lHandTransform.rotation;
			}
		}

		// Simple movement for testing on PC/Editor/Controller joystick
		// helps if you cannot use headset directly in Unity Editor  (W A S D)
		private void FixedUpdate()
		{
			HandleMovement();
		}

		private void HandleMovement()
		{
			float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 100.0f;
			float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 4f;

			transform.Rotate(0, moveX, 0);
			transform.Translate(0, 0, moveZ);
		}

	}
}