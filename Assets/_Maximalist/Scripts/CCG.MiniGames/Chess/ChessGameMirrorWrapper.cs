//------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     13/06/2025

//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using CCG.CustomInput;
using CCG.Core.Debuging;
using Mirror;
using UnityEngine;

namespace CCG.MiniGames.Chess
{
	public class ChessGameMirrorWrapper : NetworkBehaviour
	{
		[SerializeField] private ChessGame game;
		[SerializeField] private CustomInputActionData mainPointerAction;

		public bool WhiteTurn => game.WhiteTurn;

		private void OnEnable()
		{
			mainPointerAction.AddToInputActionReference(TryMovePiece);
		}

		private void OnDisable()
		{
			mainPointerAction.RemoveFromInputActionReference(TryMovePiece);
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			game.LoadListsToDictionarys();
			game.Setup(transform.localScale.y);
		}

		private void Start()
		{
			if (isServer)
			{
				game.LoadListsToDictionarys();
				game.Setup(transform.localScale.y);
			}
		}

		public void TryMovePiece()
		{
			if (RayCastHitProvider.ProvideRaycastHit(out RaycastHit hit, game.ChessTileLayer, 50f))
			{
				DebugLogger.Log($"[CGMW] has hit {hit.collider}");

				var singleTile = hit.collider.GetComponent<SingleTile>();
				if (singleTile == null) return;

				if (game.selectedPiece == null)
				{
					game.selectedX = singleTile.posX;
					game.selectedY = singleTile.posY;

					var selected = game.boardPieceTwoDArrays[game.selectedX, game.selectedY];
					if (selected == null) return;

					game.selectedPiece = selected;
					game.possibleMoves = selected.GetComponent<ChessPiece>().PossibleMoves(game, game.selectedX, game.selectedY);
					game.SetSelectedPiece(1);

					if (game.WhiteTurn != selected.GetComponent<ChessPiece>().IsWhitePiece)
						game.SetSelectedPiece(0);
				}
				else
				{
					game.currentX = singleTile.posX;
					game.currentY = singleTile.posY;

					if (game.possibleMoves[game.currentX, game.currentY])
					{
						var targetPiece = game.boardPieceTwoDArrays[game.currentX, game.currentY];
						bool isWhite = game.selectedPiece.GetComponent<ChessPiece>().IsWhitePiece;

						if (targetPiece != null)
						{
							if (targetPiece.GetComponent<ChessPiece>().IsKing)
							{
								CmdSomeoneWonGame(!isWhite);
								CmdResetChessGame();
								return;
							}
							else
							{
								if (isWhite)
									ChessGameUIManager.p1PieceCount--;
								else
									ChessGameUIManager.p2PieceCount--;
							}

							CmdRemovePiece(game.currentX, game.currentY);
						}

						CmdSetBoardPiece(game.selectedX, game.selectedY, game.currentX, game.currentY);
						CmdSwitchTurns();
					}

					game.SetSelectedPiece(0);
				}
			}
		}

		#region Commands (Run on Server)

		[Command]
		private void CmdSetBoardPiece(int fromX, int fromY, int toX, int toY)
		{
			RpcSetBoardPiece(fromX, fromY, toX, toY);
		}

		[Command]
		private void CmdRemovePiece(int x, int y)
		{
			RpcRemovePiece(x, y);
		}

		[Command]
		private void CmdSwitchTurns()
		{
			RpcSwitchTurns();
		}

		[Command]
		private void CmdResetChessGame()
		{
			RpcResetChessGame();
		}

		[Command]
		private void CmdSomeoneWonGame(bool whiteWon)
		{
			RpcSomeoneWonGame(whiteWon);
		}

		#endregion

		#region ClientRpc (Run on All Clients)

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
		private void RpcResetChessGame()
		{
			game.ResetChessGame();
		}

		[ClientRpc]
		private void RpcSomeoneWonGame(bool whiteWon)
		{
			game.SomeoneWonGame(whiteWon);
		}

		#endregion
	}
}
