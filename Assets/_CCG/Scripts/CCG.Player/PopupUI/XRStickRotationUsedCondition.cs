// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     25/08/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

namespace CCG.Player.Prompt
{
	public class XRStickRotationUsedCondition : CompletionCondition
	{
		[SerializeField] private SnapTurnProvider provider;
		private bool rotated = false;

		public override void OnBegin()
		{
			rotated = false;

			if (provider != null)
			{
				provider.locomotionEnded += OnTurn;
			}
		}

		private void OnTurn(LocomotionProvider eventProvider)
		{
			Debug.Log("[RUC] OnTurn");

			if (eventProvider != provider)
				return;

			rotated = true;

			if (provider != null)
				provider.locomotionEnded -= OnTurn;
		}

		[ContextMenu("TestCheckComplete")]
		public void TestCheckComplete()
		{
			rotated = true;
		}

		public override bool CheckComplete()
		{
			return rotated;
		}
	}
}
