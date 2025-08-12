// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     13/06/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using CCG.Core;
using CCG.CustomInput;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CCG.MiniGames
{
	public class GenericMiniGameInteractor : NetworkBehaviour
	{
		[Header("Generic MiniGame Interactor")]
		[SerializeField] private LayerMask interactableLayer;
		[SerializeField] private CustomInputActionData fireRayAction;
		[Space]
		[SerializeField] private Transform raycastOrigin;
        [Tooltip("Only needs setting client side on the local player")]
		[SerializeField] private MiniGameInteractable currentMiniGame;
		[SerializeField] private LineRenderer debugRayLineRenderer;

		public override void OnStartLocalPlayer()
		{
            base.OnStartLocalPlayer();

			if(isServerOnly == true)
				return;

			if (fireRayAction != null)
			{
				fireRayAction.AddToInputActionReference(RequestRaycast);
            }

            if (debugRayLineRenderer == null)
                return;

			debugRayLineRenderer.positionCount = 2;
        }

		private void RequestRaycast()
		{
			if (isLocalPlayer == false)
				return;

            Vector3 rayOrigin;
			Vector3 rayDirection;

#if UNITY_ANDROID && !UNITY_EDITOR
			// Mobile build – use raycast origin transform
			rayOrigin = raycastOrigin.position;
			rayDirection = raycastOrigin.forward;
#else
			Vector2 screenPosition = Mouse.current.position.ReadValue();
			Ray ray = Camera.main.ScreenPointToRay(screenPosition);

			rayOrigin = ray.origin;
			rayDirection = ray.direction;
#endif

			Debug.Log("[GMGI] RequestRaycast");
			CmdRequestRaycast(rayOrigin, rayDirection);

			if (debugRayLineRenderer == null)
				return;

			debugRayLineRenderer.SetPosition(0, rayOrigin);
			debugRayLineRenderer.SetPosition(1, rayOrigin + rayDirection * 500f);
		}

		[Command]
		private void CmdRequestRaycast(Vector3 origin, Vector3 direction)
		{
			Debug.Log("[GMGI] CmdRequestRaycast");

            if (currentMiniGame == null)
            {
                Debug.Log("[GMGI] currentMiniGame is null");
                return;
            }

			currentMiniGame.OnFireActionPressed();

            Debug.Log("[GMGI] currentMiniGame != null");

			if (RayCastHitProvider.ProvideRaycastHit(origin, direction, out RaycastHit hit, interactableLayer, 500))
			{
				Debug.Log("[GMGI] ProvideRaycastHit");
				currentMiniGame.OnReciveRaycastHit(hit);
			}
			else
			{
				Debug.Log("[GMGI] ProvideRaycastHit found nothing");

			}
		}

		public void SetCurrentMiniGame(MiniGameInteractable newMiniGame)
		{
			if (newMiniGame == null)
				return;

			currentMiniGame = newMiniGame;

			if (!isLocalPlayer)
				return;

			CmdSetCurrentMiniGame(newMiniGame.netIdentity);
		}

		[Command]
		public void CmdSetCurrentMiniGame(NetworkIdentity miniGameIdentity)
		{
			if (miniGameIdentity == null)
				return;

			currentMiniGame = miniGameIdentity.GetComponent<MiniGameInteractable>();
			Debug.Log("[GMGI] currentMiniGame set");
		}

		public void ClearCurrentMiniGame(MiniGameInteractable newMiniGame)
		{
			if (currentMiniGame != newMiniGame)
				return;
			
			currentMiniGame = null;
			CmdClearCurrentMiniGame();
		}

		[Command]
		public void CmdClearCurrentMiniGame()
		{
			currentMiniGame = null;
		}
	}
}
