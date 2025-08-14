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

        [SerializeField] private AudioClip onConnectedToServer;
        [SerializeField] private AudioClip onDisconnectedFromServer;
        [Space]
        [SerializeField] private Color[] generatedColours;
        [SerializeField] private MeshRenderer pillRenderer;

        [ContextMenu("Generate Colours")]
        private void GenerateColours()
        {
            int count = 50;
            generatedColours = new Color[count];

            // Generate evenly spaced hues
            for (int i = 0; i < count; i++)
            {
                float hue = (float)i / count; // evenly spaced around the color wheel
                generatedColours[i] = Color.HSVToRGB(hue, 0.8f, 1f); // High saturation, full brightness
            }

            // Shuffle with Fisher-Yates
            for (int i = generatedColours.Length - 1; i > 0; i--)
            {
                int swapIndex = Random.Range(0, i + 1);
                Color temp = generatedColours[i];
                generatedColours[i] = generatedColours[swapIndex];
                generatedColours[swapIndex] = temp;
            }

            Debug.Log("Generated and shuffled 50 distinct colours.");
        }

        private void Start()
        {
            int ownerId = netIdentity.connectionToClient != null ? netIdentity.connectionToClient.connectionId : 0; // 0 for host/server objects
            Color assignedColor = generatedColours[ownerId % generatedColours.Length];
            ApplyColor(assignedColor);
        }

        private void ApplyColor(Color color)
        {
            if (pillRenderer == null)
                return;

            pillRenderer.material.color = color;
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

        public override void OnStartClient()
        {
            base.OnStartClient();
            // Make sure the initial color is applied when the client starts

            if (onConnectedToServer)
				AudioSource.PlayClipAtPoint(onConnectedToServer, transform.position);
        }

        public override void OnStopClient()
        {
            base.OnStopClient();

            if (onDisconnectedFromServer)
                AudioSource.PlayClipAtPoint(onDisconnectedFromServer, transform.position);
        }

    }
}