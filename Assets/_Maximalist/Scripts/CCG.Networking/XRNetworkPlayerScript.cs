// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     11/07/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using CCG.XR;
using Mirror;
using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using UnityEngine;

namespace CCG.Networking
{
	public class XRNetworkPlayerScript : NetworkBehaviour
	{
		public Transform rHandTransform;
		public Transform lHandTransform;
		public Transform headTransform;
		public GameObject headModel;
		public GameObject rHandModel;
		public GameObject lHandModel;

		public XRPlayerRig xrPlayerRig;

		public override void OnStartLocalPlayer()
		{
			xrPlayerRig = GameObject.FindObjectOfType<XRPlayerRig>();
			xrPlayerRig.localVRNetworkPlayerScript = this;

			headModel.SetActive(false);

			CmdSendPlayerIdToServer(AuthenticationService.Instance.PlayerId);
		}

		[Command]
		public void CmdSendPlayerIdToServer(string playerId)
		{
			ServerQueryReporter.RegisterPlayerId(connectionToClient, playerId);
		}

		[Command]
		public void CmdTeleportToPosition(Vector3 targetPosition)
		{
			// 'connectionToClient' is the client's connection who called this command
			TargetTeleport(connectionToClient, targetPosition);
		}

		[TargetRpc]
		private void TargetTeleport(NetworkConnection target, Vector3 targetPosition)
		{
			transform.position = targetPosition;

			if (xrPlayerRig != null)
			{
				xrPlayerRig.transform.position = targetPosition;
			}
		}
	}
}