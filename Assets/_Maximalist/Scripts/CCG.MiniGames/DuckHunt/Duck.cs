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
using Random = UnityEngine.Random;

namespace CCG.MiniGames.Duckhunt
{
	public class Duck : MonoBehaviour
	{
		public static event Action<int> OnDuckDied;
        [SerializeField] private int index;
        [SerializeField] private int points = 2;
        [SerializeField] private bool isHit = false;

		[SerializeField] private MeshRenderer duckRenderer;
		private Color startingColor;

        public int Index { get => index; set => index = value; }

        private void Start()
        {
			if(duckRenderer != null)
				startingColor = duckRenderer.material.color;
        }

        public void OnHit()
		{
			if (isHit)
				return;

			isHit = true;
			Die();

            if (duckRenderer != null)
                duckRenderer.material.color = Color.blue;
        }

    
        private void Die()
		{
			OnDuckDied?.Invoke(points); /* animation + notify game manager */
		}

        public void ResetDuck()
        {
            isHit = false;

            if (duckRenderer != null)
                duckRenderer.material.color = startingColor;
        }
    }
}

