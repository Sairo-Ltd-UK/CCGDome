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
			// create a link to local vr rig, so that rig can sync to our local network players transforms
			xrPlayerRig = GameObject.FindObjectOfType<XRPlayerRig>();
			xrPlayerRig.localVRNetworkPlayerScript = this;

			// we dont need to see our network representation of hands, or our own headset, it also covers camera without using layers or some repositioning
			headModel.SetActive(false);
			rHandModel.SetActive(false);
			lHandModel.SetActive(false);

			CmdSendPlayerIdToServer(AuthenticationService.Instance.PlayerId);
		}

		public override void OnStopLocalPlayer()
		{
			base.OnStopLocalPlayer();
			CmdUnregisterPlayer();
		}

		[Command]
		public void CmdSendPlayerIdToServer(string playerId)
		{
			ServerQueryReporter.RegisterPlayerId(connectionToClient, playerId);
		}

		[Command]
		public void CmdUnregisterPlayer()
		{
			ServerQueryReporter.UnregisterPlayer(connectionToClient);
		}



		// a static global list of players that can be used for a variery of features, one being enemies
		public readonly static List<XRNetworkPlayerScript> playersList = new List<XRNetworkPlayerScript>();

		public override void OnStartServer()
		{
			playersList.Add(this);
		}

		public override void OnStopServer()
		{
			playersList.Remove(this);
		}



		[SyncVar(hook = nameof(OnRightObjectChangedHook))]
		public NetworkIdentity rightHandObject;

		void OnRightObjectChangedHook(NetworkIdentity _old, NetworkIdentity _new)
		{
			if (rightHandObject)
			{

			}
			else
			{

			}
		}

		[SyncVar(hook = nameof(OnLeftObjectChangedHook))]
		public NetworkIdentity leftHandObject;

		void OnLeftObjectChangedHook(NetworkIdentity _old, NetworkIdentity _new)
		{
			if (leftHandObject)
			{

			}
			else
			{

			}
		}


	}
}