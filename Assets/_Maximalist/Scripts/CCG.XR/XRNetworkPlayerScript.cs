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
using Mirror;
using System.Collections.Generic;
using TMPro;

namespace CCG.XR
{
	public class XRNetworkPlayerScript : NetworkBehaviour
	{
		public Transform rHandTransform;
		public Transform lHandTransform;
		public Transform headTransform;
		public GameObject headModel;
		public GameObject rHandModel;
		public GameObject lHandModel;
		[Space]
		public XRPlayerRig xrPlayerRig;

		[SyncVar(hook = nameof(OnRightObjectChangedHook))]
		public NetworkIdentity rightHandObject;

		[SyncVar(hook = nameof(OnLeftObjectChangedHook))]
		public NetworkIdentity leftHandObject;

		public override void OnStartLocalPlayer()
		{
			// create a link to local vr rig, so that rig can sync to our local network players transforms
			xrPlayerRig = FindObjectOfType<XRPlayerRig>();
			xrPlayerRig.localVRNetworkPlayerScript = this;

			// we dont need to see our network representation of hands, or our own headset, it also covers camera without using layers or some repositioning
			headModel.SetActive(false);
			rHandModel.SetActive(false);
			lHandModel.SetActive(false);
			CmdSetupPlayer();
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

		[Command]
		public void CmdSetupPlayer()
		{
			//player info sent to server, then server updates sync vars which handles it on all clients

		}

		void OnRightObjectChangedHook(NetworkIdentity _old, NetworkIdentity _new)
		{
			if (rightHandObject)
			{

			}
			else
			{

			}
		}

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