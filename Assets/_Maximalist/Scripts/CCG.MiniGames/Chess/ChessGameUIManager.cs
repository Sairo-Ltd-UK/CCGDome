// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     13/06/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------


using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace CCG.MiniGames.Chess
{
	public class ChessGameUIManager : NetworkBehaviour
	{
		[Header("ChessGameUIManager")]
		[SerializeField] private Button resetGame;
		[SerializeField] private ChessGame game;

		private void Start()
		{
			// Only hook up the button for local player
			if (resetGame != null)
			{
				resetGame.onClick.AddListener(OnResetButtonPressed);
			}
		}

		private void OnDestroy()
		{
			if (resetGame != null)
				resetGame.onClick.RemoveListener(OnResetButtonPressed);
		}

		[ContextMenu("OnResetButtonPressed")]
		private void OnResetButtonPressed()
		{
			CmdRequestReset();
		}

		[Command(requiresAuthority = false)]
		private void CmdRequestReset()
		{
			RpcDoReset();

			if(isServerOnly)
				DoReset();
		}


		[ClientRpc]
		private void RpcDoReset()
		{
			DoReset();
		}

		private void DoReset()
		{
			if (game != null)
				game.ResetChessGame();
		}
	}
}