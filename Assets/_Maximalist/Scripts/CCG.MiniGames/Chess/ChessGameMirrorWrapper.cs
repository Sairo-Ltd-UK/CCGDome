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

namespace CCG.MiniGames.Chess
{
	public class ChessGameMirrorWrapper : MiniGameInteractable
	{
		[SerializeField] private ChessGame game;

		public override void OnStartServer()
		{
			game.LoadListsToDictionarys();
			game.Setup(transform.localScale.y);
		}

		[Server]
		public override void OnReciveRaycastHit(RaycastHit hit)
		{
			TryMovePeice(hit);
		}

		[Server]
		private void TryMovePeice(RaycastHit hit)
		{
			if(hit.collider == null) 
				return;

			Debug.Log($"NAME: {hit.collider.name}");

			var tile = hit.collider.GetComponent<SingleTile>();

			if (tile == null)
				return;

			Debug.Log($"Tile: {tile.name}");

			int posX = tile.posX;
			int posY = tile.posY;

			if (game.selectedPiece == null)
			{
				game.selectedX = posX;
				game.selectedY = posY;

				var selected = game.boardPieceTwoDArrays[posX, posY];
				if (selected == null) return;

				game.selectedPiece = selected;
				game.possibleMoves = selected.GetComponent<ChessPiece>().PossibleMoves(game, posX, posY);

				if (game.WhiteTurn != selected.GetComponent<ChessPiece>().IsWhitePiece)
				{
					game.SetSelectedPiece(0);
					return;
				}

				game.SetSelectedPiece(1);
			}
			else
			{
				game.currentX = posX;
				game.currentY = posY;

				if (!game.possibleMoves[posX, posY]) return;

				var targetPiece = game.boardPieceTwoDArrays[posX, posY];

				if (targetPiece != null)
				{
					bool isWhite = game.selectedPiece.GetComponent<ChessPiece>().IsWhitePiece;
					bool isKing = targetPiece.GetComponent<ChessPiece>().IsKing;

					if (isKing)
					{
						RpcSomeoneWonGame(!isWhite);
						RpcResetGame();
						return;
					}
					else
					{
						if (isWhite) ChessGameUIManager.p1PieceCount--;
						else ChessGameUIManager.p2PieceCount--;
					}

					RpcRemovePiece(posX, posY);
				}

				RpcSetBoardPiece(game.selectedX, game.selectedY, posX, posY);
				RpcSwitchTurns();

				game.SetSelectedPiece(0);
			}
		}

		[ClientRpc]
		private void RpcSetBoardPiece(int fromX, int fromY, int toX, int toY)
		{
			game.SetBoardPiece(fromX, fromY, toX, toY);
		}

		[ClientRpc]
		private void RpcRemovePiece(int x, int y)
		{
			game.RemovePiece(x, y);
		}

		[ClientRpc]
		private void RpcSwitchTurns()
		{
			game.SwitchTurns();
		}

		[ClientRpc]
		private void RpcResetGame()
		{
			game.ResetChessGame();
		}

		[ClientRpc]
		private void RpcSomeoneWonGame(bool whiteWon)
		{
			game.SomeoneWonGame(whiteWon);
		}

	}
}
