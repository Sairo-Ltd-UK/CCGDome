//------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     11 / 07 / 2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using Mirror;
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

		[SerializeField] private XRPlayerRig xrPlayerRig;
		[Space]
		[SerializeField] private AudioClip onConnectedToServer;
		[SerializeField] private AudioClip onDisconnectedFromServer;
		[SerializeField] private AudioSource playerAudioSource;

		[Space]
		[SerializeField] private Color[] generatedColours;
		[SerializeField] private MeshRenderer pillRenderer;

		// Server picks this once — automatically syncs to all clients
		[SyncVar(hook = nameof(OnColorIndexChanged))]
		private int colorIndex = 0;

		public override void OnStartServer()
		{
			base.OnStartServer();

			// Assign a color index based on connection ID — wraps around list length
			colorIndex = connectionToClient.connectionId % 50; //CW num needs to match generated colours length but for some reason the list is stripped server side.
		}

		public override void OnStartClient()
		{
			base.OnStartClient();

			if (onConnectedToServer)
			{
				Debug.Log("OnStartClient");
				playerAudioSource.clip = onConnectedToServer;
				playerAudioSource.Play();
			}

			if(generatedColours == null)
				return;

			if (colorIndex >= generatedColours.Length || colorIndex < 0)
				colorIndex = 0;

			// Apply immediately for the initial state
			ApplyColor(generatedColours[colorIndex]);
		}

		public override void OnStopClient()
		{
			base.OnStopClient();

			if (onDisconnectedFromServer)
			{
				Debug.Log("OnStopClient");
				playerAudioSource.clip = onDisconnectedFromServer;
				playerAudioSource.Play();
			}
		}

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

		private void ApplyColor(Color color)
		{
			if(isServer)
				return;

			if (pillRenderer != null)
				pillRenderer.material.color = color;
		}

		//Called automatically when colorIndex changes on any client
		private void OnColorIndexChanged(int oldIndex, int newIndex)
		{
			Debug.Log($"Color index changed: old={oldIndex}, new={newIndex}, arrayLen={generatedColours?.Length}");

			if (isServer == true || generatedColours == null || generatedColours.Length == 0)
				return;

			colorIndex = ((newIndex % generatedColours.Length) + generatedColours.Length) % generatedColours.Length;

			if (colorIndex < 0 || colorIndex >= generatedColours.Length)
				colorIndex = 0;

			ApplyColor(generatedColours[colorIndex]);
		}
	}
}
