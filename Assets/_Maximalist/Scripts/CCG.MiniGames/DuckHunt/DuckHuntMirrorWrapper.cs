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
   
        public override void OnFireActionPressed()
        {
            if (duckHuntGameManager == null)
                return;

            if (duckHuntGameManager.HasShotsRemaining == false)
                return;

            FireServer();
        }

        private void FireServer()
        {
            if(isClient == false)
                Fire();

            FireServerRpc();
        }

        [ClientRpc]
        private void FireServerRpc()
        {
            Fire();
        }

        private void Fire()
        {
            duckHuntGameManager.Fire();
        }

        public override void OnReciveRaycastHit(RaycastHit hit)
        {
            if (duckHuntGameManager.HasShotsRemaining == false)
                return;

            Duck duck = hit.collider.GetComponent<Duck>();

            if (duck != null)
                HitDuckServer(duck.Index);
        }

        public void HitDuckServer(int index)
		{
            if (isClient == false)
                HitDuck(index);

			HitDuckRpc(index);
        }

		[ClientRpc]
		public void HitDuckRpc(int index)
		{
			Debug.Log("HitDuck");
			HitDuck(index);
        }

		public void HitDuck(int index)
		{
            duckHuntGameManager.HitDuck(index);
        }
    }
}
