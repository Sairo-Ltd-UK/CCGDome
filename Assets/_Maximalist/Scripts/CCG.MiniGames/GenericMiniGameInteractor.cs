// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     13/06/2025
//
//  Copyright � 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using CCG.CustomInput;
using Mirror;
using UnityEngine;

namespace CCG.MiniGames
{
	public class GenericMiniGameInteractor : NetworkBehaviour
	{
		[Header("Generic MiniGame Interactor")]
		[SerializeField] private LayerMask interactableLayer;
		[SerializeField] private CustomInputActionData fireRayAction;
		[Space]
		[Tooltip("Only needs setting client side on the local player")]
		[SerializeField] private MiniGameInteractable currentMiniGame;

		public override void OnStartLocalPlayer()
		{
			base.OnStartLocalPlayer();

			if(isClientOnly == false)
				return;

			if (fireRayAction != null)
			{
				Debug.Log("AddToInputActionReference");
				fireRayAction.AddToInputActionReference(RequestRaycast);
			}
		}

		private void RequestRaycast()
		{
			if (isLocalPlayer == false)
				return;

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			CmdRequestRaycast(ray.origin, ray.direction);
		}

		[Command]
		private void CmdRequestRaycast(Vector3 origin, Vector3 direction)
		{
			//CW Placeholder, will later replace with a selector system. This will do for now.
			if(currentMiniGame == null)
				currentMiniGame = FindObjectOfType<MiniGameInteractable>();

			if (RayCastHitProvider.ProvideRaycastHit(out RaycastHit hit, interactableLayer, 500))
			{
				if (currentMiniGame != null)
				{
					currentMiniGame.OnReciveRaycastHit(hit);
				}
			}
		}

		public void SetCurrentMiniGame(MiniGameInteractable newMiniGame)
		{
			if (newMiniGame != null)
			{
				currentMiniGame = newMiniGame;
			}
		}
		public void ClearCurrentMiniGame(MiniGameInteractable newMiniGame)
		{
			if (currentMiniGame == newMiniGame)
			{
				currentMiniGame = null;
			}
		} 
	}
}
