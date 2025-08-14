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
		public static event Action<int> OnDuckDied;
        [SerializeField] private int index;
        [SerializeField] private int points = 2;
        [SerializeField] private bool isHit = false;
        [SerializeField] private Vector3 rotationOnHit = new Vector3(90, 0, 0);
		private Quaternion startingRotation;

        public int Index { get => index; set => index = value; }

        private void Start()
        {
            startingRotation = transform.rotation;
        }

        [ContextMenu("OnHit")]
        public void OnHit()
		{
			if (isHit)
				return;

			isHit = true;
			Die();

        }
    
        private void Die()
		{
			OnDuckDied?.Invoke(points); /* animation + notify game manager */
            transform.localEulerAngles -= rotationOnHit;
        }

        public void ResetDuck()
        {
            isHit = false;
            transform.rotation = startingRotation;
        }
    }
}

