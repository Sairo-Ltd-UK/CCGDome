// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     25/07/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using UnityEngine;
using Mirror;

namespace CCG.MiniGames
{
	public class MiniGameSpawner : NetworkBehaviour
	{
		[SerializeField] private GameObject chessGamePrefab;
		[SerializeField] private Transform spawnLocation;

		public override void OnStartServer()
		{
			base.OnStartServer();
			SpawnChessGame();
		}

		[Server]
		private void SpawnChessGame()
		{
			if (chessGamePrefab == null || spawnLocation == null)
			{
				Debug.LogWarning("MinigameSpawner: Missing chess prefab or spawn location.");
				return;
			}

			GameObject chessInstance = Instantiate(chessGamePrefab, spawnLocation.position, spawnLocation.rotation);
			NetworkServer.Spawn(chessInstance);
		}
	}
}
