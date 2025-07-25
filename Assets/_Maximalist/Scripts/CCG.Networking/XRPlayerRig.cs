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

namespace CCG.Networking
{
	public class XRPlayerRig : MonoBehaviour
	{
		[Header("XRPlayerRig")]
		[SerializeField] private Transform rHandTransform;
		[SerializeField] private Transform lHandTransform;
		[SerializeField] private Transform headTransform;
		[Space]
		[SerializeField] private Transform canvasUIPosition;

		public Transform RHandTransform { get => rHandTransform; set => rHandTransform = value; }
		public Transform LHandTransform { get => lHandTransform; set => lHandTransform = value; }
		public Transform HeadTransform { get => headTransform; set => headTransform = value; }

		public Transform CanvasUIPosition { get => canvasUIPosition; set => canvasUIPosition = value; }

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