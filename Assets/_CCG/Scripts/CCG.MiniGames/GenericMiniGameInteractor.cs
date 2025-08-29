// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     25/08/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

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
		[SerializeField] private CustomInputActionData rightFireAciton;
		[SerializeField] private CustomInputActionData leftFireAciton;
		[Space]
		[SerializeField] private Transform rightRaycastOrigin;
		[SerializeField] private Transform leftRaycastOrigin;
		[Space]
		[Tooltip("Only needs setting client side on the local player")]
		[SerializeField] private MiniGameInteractable currentMiniGame;
		[Space]
		[SerializeField] private LineRenderer debugLine;

		private Transform rightRaycastOverride;
		private Transform leftRaycastOverride;

		public Transform RightRaycastOrigin
		{
			get
			{
				if (rightRaycastOverride)
					return rightRaycastOverride;

				return rightRaycastOrigin;
			}
		}

		public Transform LeftRaycastOrigin
		{
			get
			{
				if (leftRaycastOverride)
					return leftRaycastOverride;

				return leftRaycastOrigin;
			}
		}

		public Transform RightRaycastOverride { get => rightRaycastOverride; set => rightRaycastOverride = value; }
		
		public Transform LeftRaycastOverride { get => leftRaycastOverride; set => leftRaycastOverride = value; }


		public override void OnStartLocalPlayer()
		{
			base.OnStartLocalPlayer();

			if(isServerOnly == true)
				return;

			if (rightFireAciton != null)
			{
				rightFireAciton.AddToInputActionReference(RequestRaycastRightHand);
			}

			if (leftFireAciton != null)
			{
				leftFireAciton.AddToInputActionReference(RequestRaycastLeftHand);
			}
		}

		private void RequestRaycastRightHand()
		{
			if (RightRaycastOrigin)
				RequestRaycast(RightRaycastOrigin.position, RightRaycastOrigin.forward);
		}

		private void RequestRaycastLeftHand()
		{
			if(LeftRaycastOrigin)
				RequestRaycast(LeftRaycastOrigin.position, LeftRaycastOrigin.forward);
		}

		private void RequestRaycast(Vector3 rayOrigin, Vector3 rayDirection)
		{
			if (isLocalPlayer == false)
				return;

			DrawDebugLine(rayOrigin, rayDirection);

#if !UNITY_ANDROID || UNITY_EDITOR
			Vector2 screenPosition = Mouse.current.position.ReadValue();
			Ray ray = Camera.main.ScreenPointToRay(screenPosition);

			rayOrigin = ray.origin;
			rayDirection = ray.direction;
#endif

			Debug.Log("[GMGI] RequestRaycast");
			CmdRequestRaycast(rayOrigin, rayDirection);
		}

		private void DrawDebugLine(Vector3 rayOrigin, Vector3 rayDirection)
		{
			if (debugLine == null)
				return;

			debugLine.SetPosition(0, rayOrigin);
			debugLine.SetPosition(1, rayOrigin + rayDirection * 10);
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
