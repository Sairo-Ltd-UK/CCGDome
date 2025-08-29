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
using System.Collections.Generic;
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
		private int colorIndex;

		[ContextMenu("GenerateColorPalette")]
		public void GenerateColorPalette()
		{
			List<Color> colors = new List<Color>(50);

			int totalColors = 50;
			int satSteps = 5; // 5 saturation levels
			int valSteps = 2; // 2 brightness levels
			float goldenRatio = 0.61803398875f; // fraction of hue per step

			int index = 0;

			for (int i = 0; i < totalColors; i++)
			{
				// Step hue using golden ratio to maximize distance
				float baseHue = (i * goldenRatio) % 1f;

				// Alternate saturation and brightness to add contrast
				int satIndex = i % satSteps;
				int valIndex = i % valSteps;

				float sat = Mathf.Lerp(0.6f, 1.0f, (float)satIndex / (satSteps - 1));
				float val = Mathf.Lerp(0.8f, 1.0f, (float)valIndex / (valSteps - 1));

				// Add small jitter for natural variation
				float hue = Mathf.Clamp01(baseHue + Random.Range(-0.2f, 0.2f));
				sat = Mathf.Clamp01(sat + Random.Range(-0.05f, 0.05f));
				val = Mathf.Clamp01(val + Random.Range(-0.05f, 0.05f));

				colors.Add(Color.HSVToRGB(hue, sat, val));
				index++;
			}

			// Shuffle list so consecutive colours aren’t predictable
			for (int i = colors.Count - 1; i > 0; i--)
			{
				int j = Random.Range(0, i + 1);
				(colors[i], colors[j]) = (colors[j], colors[i]);
			}

			generatedColours = colors.ToArray();
		}

		public override void OnStartServer()
		{
			base.OnStartServer();

			// Assign a color index based on connection ID — wraps around list length
			if (generatedColours != null && generatedColours.Length > 0)
				colorIndex = connectionToClient.connectionId % generatedColours.Length;
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
			if (pillRenderer != null)
				pillRenderer.material.color = color;
		}

		//Called automatically when colorIndex changes on any client
		private void OnColorIndexChanged(int oldIndex, int newIndex)
		{
			if (generatedColours != null && generatedColours.Length > 0)
				ApplyColor(generatedColours[newIndex]);
		}
	}
}
