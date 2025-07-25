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
		[Space]
		public XRPlayerRig xrPlayerRig;

		//SyncVar(hook = nameof(OnRightObjectChangedHook))]
		public NetworkIdentity rightHandObject;

		//[SyncVar(hook = nameof(OnLeftObjectChangedHook))]
		public NetworkIdentity leftHandObject;

		// switch to Late/Fixed Update if weirdness happens
		private void Update()
		{
			if (xrPlayerRig == false)
				return;

			transform.position = xrPlayerRig.transform.position;
			transform.rotation = xrPlayerRig.transform.rotation;

			headTransform.position = xrPlayerRig.HeadTransform.position;
			headTransform.rotation = xrPlayerRig.HeadTransform.rotation;

			rHandTransform.position = xrPlayerRig.RHandTransform.position;
			rHandTransform.rotation = xrPlayerRig.RHandTransform.rotation;
			lHandTransform.position = xrPlayerRig.LHandTransform.position;
			lHandTransform.rotation = xrPlayerRig.LHandTransform.rotation;
		}

		public override void OnStartLocalPlayer()
		{
			// create a link to local vr rig, so that rig can sync to our local network players transforms
			xrPlayerRig = FindObjectOfType<XRPlayerRig>();

			// we dont need to see our network representation of hands, or our own headset, it also covers camera without using layers or some repositioning
			headModel.SetActive(false);
			rHandModel.SetActive(false);
			lHandModel.SetActive(false);
		}
	
		[Command]
		public void CmdSetupPlayer()
		{
			//player info sent to server, then server updates sync vars which handles it on all clients

		}
	}
}