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

		public XRPlayerRig xrPlayerRig;

		public override void OnStartLocalPlayer()
		{
			// create a link to local vr rig, so that rig can sync to our local network players transforms
			xrPlayerRig = FindObjectOfType<XRPlayerRig>();
			xrPlayerRig.localVRNetworkPlayerScript = this;

			// we dont need to see our network representation of hands, or our own headset, it also covers camera without using layers or some repositioning
			headModel.SetActive(false);
			rHandModel.SetActive(false);
			lHandModel.SetActive(false);

			// if no customisation is set, create one.
			if (XRStaticVariables.playerName == "")
			{
				CmdSetupPlayer("Player: " + netId);
			}
			else
			{
				CmdSetupPlayer(XRStaticVariables.playerName);
			}
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

		[SyncVar(hook = nameof(OnNameChangedHook))]
		public string playerName = "";
		public TMP_Text textPlayerName;

		void OnNameChangedHook(string _old, string _new)
		{
			//Debug.Log("OnNameChangedHook: " + playerName);
			textPlayerName.text = playerName;
		}

		[Command]
		public void CmdSetupPlayer(string _name)
		{
			//player info sent to server, then server updates sync vars which handles it on all clients
			playerName = _name; //+ connectionToClient.connectionId;
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

		// switch to Late/Fixed Update if weirdness happens
		//private void Update()
		//{
		//    if (rightHandObject)
		//    {
		//         rightHandObject.transform.position = rHandTransform.position;
		//    }
		//}

		public void Fire(int _hand)
		{

		}


		[Command]
		private void CmdFire(int _hand)
		{
			// 0 both, 1 right, 2 left
			//Debug.Log("Mirror CmdFire");
			RpcOnFire(_hand);
			if (isServerOnly)
			{
				OnFire(_hand);
			}
		}

		[ClientRpc]
		private void RpcOnFire(int _hand)
		{
			//Debug.Log("Mirror RpcOnFire");
			OnFire(_hand);
		}

		private void OnFire(int _hand)
		{

		}
	}
}