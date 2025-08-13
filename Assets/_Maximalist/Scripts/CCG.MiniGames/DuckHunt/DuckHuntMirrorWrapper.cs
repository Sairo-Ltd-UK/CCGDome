// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     23/06/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using Mirror;
using UnityEngine;

namespace CCG.MiniGames.Duckhunt
{
	public class DuckHuntMirrorWrapper : MiniGameInteractable
	{
        [SerializeField] private DuckHuntGameManager duckHuntGameManager;
		[SerializeField] private LayerMask duckLayer;
		[SerializeField] private Duck[] ducks;

        private void Start()
        {
			for (int i = 0; i < ducks.Length; i++)
			{
				ducks[i].Index = i;
            }
        }

        public override void OnFireActionPressed() 
		{
			if(duckHuntGameManager == null)
				return;

			if (duckHuntGameManager.HasShotsRemaining == false)
				return;

			duckHuntGameManager.Fire();
        }

        public override void OnReciveRaycastHit(RaycastHit hit)
		{
            if (duckHuntGameManager.HasShotsRemaining == false)
                return;

            Duck duck = hit.collider.GetComponent<Duck>();

			if (duck != null)
				HitDuckRpc(duck.Index);
        }

		[ClientRpc]
		public void HitDuckRpc(int index)
		{
			Debug.Log("HitDuck");

			if (ducks[index])
				ducks[index].OnHit();
        }
	}
}
