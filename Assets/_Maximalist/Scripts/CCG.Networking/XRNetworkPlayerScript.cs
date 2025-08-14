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

        public XRPlayerRig xrPlayerRig;

        [SerializeField] private AudioClip onConnectedToServer;
        [SerializeField] private AudioClip onDisconnectedFromServer;
        [Space]
        [SerializeField] private Color[] generatedColours;
        [SerializeField] private MeshRenderer pillRenderer;

        // Server picks this once — automatically syncs to all clients
        [SyncVar(hook = nameof(OnColorIndexChanged))]
        private int colorIndex;

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
                AudioSource.PlayClipAtPoint(onConnectedToServer, transform.position);

            // Apply immediately for the initial state
            ApplyColor(generatedColours[colorIndex]);
        }

        public override void OnStopClient()
        {
            base.OnStopClient();

            if (onDisconnectedFromServer)
                AudioSource.PlayClipAtPoint(onDisconnectedFromServer, transform.position);
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
