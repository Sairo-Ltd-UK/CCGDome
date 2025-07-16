// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     23/06/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using System;
using UnityEngine;

namespace CCG.MiniGames.Duckhunt
{
	public class Duck : MonoBehaviour
	{
		public static event Action OnDuckDied;
		
		public float speed = 2f;
		public bool isHit = false;

		private void Update() { Move(); }

		private void Move() { }

		public void OnHit()
		{
			if (isHit) return;
			isHit = true;
			Die();
		}

		private void Die()
		{
			OnDuckDied?.Invoke(); /* animation + notify game manager */
		}
	}

}

