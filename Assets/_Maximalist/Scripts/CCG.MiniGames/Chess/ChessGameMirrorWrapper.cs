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

		private void Start()
		{
			Debug.Log("[CGMW] Start");
			game.LoadListsToDictionarys();
			game.Setup(transform.localScale.y);
		}

		[Server]
		public override void OnReciveRaycastHit(RaycastHit hit, string ownerID)
		{
			Debug.Log("[CGMW] OnReciveRaycastHit");
			TryMovePeice(hit);
		}

		[Server]
		private void TryMovePeice(RaycastHit hit)
		{
			Debug.Log("[CGMW] TryMovePeice");

			if (hit.collider == null) 
				return;

			Debug.Log("[CGMW] hit.collider != null");

			var tile = hit.collider.GetComponent<SingleTile>();

			if (tile == null)
				return;

			Debug.Log("[CGMW] tile != null");

			int posX = tile.posX;
			int posY = tile.posY;

			if (game.selectedPiece == null)
			{
				Debug.Log("[CGMW] game.selectedPiece == null");
				SelectPieceServer(posX, posY);
			}
			else
			{
				Debug.Log("[CGMW] game.selectedPiece != null");

				SetGameCurrentXYServer(posX, posY);

				if (game.possibleMoves[posX, posY] == false)
				{
					SetSelectedPieceToNothingServer();
					return;
				}

				var targetPiece = game.boardPieceTwoDArrays[posX, posY];

				if (targetPiece != null)
				{
					Debug.Log("[CGMW] targetPiece != null");

					bool isWhite = game.selectedPiece.GetComponent<ChessPiece>().IsWhitePiece;
					bool isKing = targetPiece.GetComponent<ChessPiece>().IsKing;

					if (isKing)
					{
						SomeoneWonServer(isWhite);
						ResetGameServer();
						return;
					}
					else
					{
						if (isWhite)
							ChessGameUIManager.p1PieceCount--;
						else
							ChessGameUIManager.p2PieceCount--;
					}

					RemovePieceServer(posX, posY);
				}

				SetBoardPieceServer(posX, posY);
				SwitchTurnsServer();
				SetSelectedPieceToNothingServer();
			}
		}

		[Server]
		private void SetGameCurrentXYServer(int posX, int posY)
		{
			if(isClient == false)
				SetGameCurrentXY(posX, posY);

			RpcSetGameCurrentXY(posX, posY);
		}

		[ClientRpc]
		private void RpcSetGameCurrentXY(int posX, int posY)
		{
			SetGameCurrentXY(posX, posY);
		}

		private void SetGameCurrentXY(int posX, int posY)
		{
			game.currentX = posX;
			game.currentY = posY;
		}

		[Server]
		private void SetSelectedPieceToNothingServer()
		{
			if (isClient == false)
				SetSelectedPieceToNothing();

			RpcSetSelectedPieceToNothing();
		}

		[ClientRpc]
		private void RpcSetSelectedPieceToNothing()
		{
			SetSelectedPieceToNothing();
		}

		private void SetSelectedPieceToNothing()
		{
			Debug.Log("[CGMW] SetSelectedPiece");
			game.SetSelectedPiece(0);
		}

		[Server]
		private void SelectPieceServer(int posX, int posY)
		{
			if (isClient == false)
				SelectPiece(posX, posY);

			RpcSelectPiece(posX, posY);
		}

		[ClientRpc]
		private void RpcSelectPiece(int posX, int posY)
		{
			SelectPiece(posX, posY);
		}

		private void SelectPiece(int posX, int posY)
		{
			Debug.Log("[CGMW] SelectPiece");

			game.selectedX = posX;
			game.selectedY = posY;

			GameObject selected = game.boardPieceTwoDArrays[posX, posY];

			if (selected == null)
				return;

			game.selectedPiece = selected;
			game.possibleMoves = selected.GetComponent<ChessPiece>().PossibleMoves(game, posX, posY);

			if (game.WhiteTurn != selected.GetComponent<ChessPiece>().IsWhitePiece)
			{
				game.SetSelectedPiece(0);
				return;
			}

			game.SetSelectedPiece(1);
		}

		[Server]
		private void SetBoardPieceServer(int posX, int posY)
		{
			if (isClient == false)
				SetBoardPiece(game.selectedX, game.selectedY, posX, posY);

			RpcSetBoardPiece(game.selectedX, game.selectedY, posX, posY);
		}

		[ClientRpc]
		private void RpcSetBoardPiece(int fromX, int fromY, int toX, int toY)
		{
			SetBoardPiece(fromX, fromY, toX, toY);
		}

		private void SetBoardPiece(int fromX, int fromY, int toX, int toY)
		{
			Debug.Log("[CGMW] SetBoardPiece");
			game.SetBoardPiece(fromX, fromY, toX, toY);
		}

		[Server]
		private void RemovePieceServer(int posX, int posY)
		{
			if (isClient == false)
				RemovePiece(posX, posY);

			RpcRemovePiece(posX, posY);
		}

		[ClientRpc]
		private void RpcRemovePiece(int x, int y)
		{
			RemovePiece(x, y);
		}
		
		private void RemovePiece(int x, int y)
		{
			Debug.Log("[CGMW] RemovePiece");
			game.RemovePiece(x, y);
		}

		[Server]
		private void SwitchTurnsServer()
		{
			if(isClient == false)
				SwitchTurns();

			RpcSwitchTurns();
		}

		[ClientRpc]
		private void RpcSwitchTurns()
		{
			SwitchTurns();
		}

		private void SwitchTurns()
		{
			Debug.Log("[CGMW] SwitchTurns");
			game.SwitchTurns();
		}

		[Server]
		[ContextMenu("ResetGameServer")]
		private void ResetGameServer()
		{
			if (isClient == false)
				ResetGame();

			RpcResetGame();
		}

		[ClientRpc]
		private void RpcResetGame()
		{
			ResetGame();
		}

		private void ResetGame()
		{
			Debug.Log("[CGMW] ResetGame");
			game.ResetChessGame();
		}

		[Server]
		private void SomeoneWonServer(bool isWhite)
		{
			if (isClient == false)
				SomeoneWonGame(!isWhite);

			RpcSomeoneWonGame(!isWhite);
		}

		[ClientRpc]
		private void RpcSomeoneWonGame(bool whiteWon)
		{
			SomeoneWonGame(whiteWon);
		}

		private void SomeoneWonGame(bool whiteWon)
		{
			game.SomeoneWonGame(whiteWon);
		}
	}
}
