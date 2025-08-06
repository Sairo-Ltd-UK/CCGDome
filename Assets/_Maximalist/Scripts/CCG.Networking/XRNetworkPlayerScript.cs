// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     11/07/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

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
			rHandModel.SetActive(false);
			lHandModel.SetActive(false);

            CmdSendPlayerIdToServer(Unity.Services.Authentication.AuthenticationService.Instance.PlayerId);
		}

		[Command]
		public void CmdSendPlayerIdToServer(string playerId)
		{
			ServerQueryReporterService.RegisterPlayerId(connectionToClient, playerId);
		}


	}
}