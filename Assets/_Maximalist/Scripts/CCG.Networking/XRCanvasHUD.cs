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
using UnityEngine.UI;

namespace CCG.Networking
{
	public class XRCanvasHUD : MonoBehaviour
	{
		private const string ipToConnectTo = "145.190.54.4";

		[SerializeField] private Button localHostButton;
		[SerializeField] private Button connectToServerButton;

		private void Start()
		{
			localHostButton.onClick.AddListener(OnLocalHostButtonPressed);
			connectToServerButton.onClick.AddListener(OnConnectToServerButtonPressed);
		}

		private void OnDestroy()
		{
			localHostButton.onClick.RemoveListener(OnLocalHostButtonPressed);
			connectToServerButton.onClick.RemoveListener(OnConnectToServerButtonPressed);
		}

		private void OnLocalHostButtonPressed()
		{
			localHostButton.interactable = false;
			connectToServerButton.interactable = false;
			NetworkManager.singleton.StartHost();
		}

		private void OnConnectToServerButtonPressed()
		{
			Debug.Log("Attempting to connect to server at " + ipToConnectTo);

			localHostButton.interactable = false;
			connectToServerButton.interactable = false;
			NetworkManager.singleton.networkAddress = ipToConnectTo;
			NetworkManager.singleton.StartClient();
		}

	}
}