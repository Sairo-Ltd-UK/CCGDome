// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     23/06/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using UnityEngine;

namespace CCG.MiniGames.Duckhunt
{
	public class DuckSpawner : MonoBehaviour
	{
		public GameObject[] ducks;
		public Transform[] spawnPoints;

		//Dont destroy or spawn objects, just enable/disable 
		public void SpawnDuck() { /* random point + velocity */ }
		
	}
}

