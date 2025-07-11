// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     13/06/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using UnityEngine;

namespace CCG.MiniGames.Chess
{
	[CreateAssetMenu(fileName = "PawnBehaviourSO", menuName = "Chess/PieceBehaviours/Pawn")]
	public class PawnBehaviourSO : PieceBehaviourSO
	{
		public override bool[,] PossibleMoves(ChessGame chessGame, int x, int y, bool isWhitePiece)
		{
			bool[,] possibleMoves = new bool[8, 8];

			int direction = isWhitePiece ? -1 : 1;
			int startRow = isWhitePiece ? 6 : 1;
			int nextRow = x + direction;
			int doubleRow = x + 2 * direction;

			if (IsInsideBoard(nextRow, y) && chessGame.boardPieceTwoDArrays[nextRow, y] == null)
			{
				possibleMoves[nextRow, y] = true;

				if (x == startRow && IsInsideBoard(doubleRow, y) && chessGame.boardPieceTwoDArrays[doubleRow, y] == null)
				{
					possibleMoves[doubleRow, y] = true;
				}
			}

			int[] dy = { -1, 1 };
			foreach (int offsetY in dy)
			{
				int diagX = x + direction;
				int diagY = y + offsetY;

				if (CanMoveTo(chessGame,diagX, diagY, isWhitePiece))
				{
					var target = chessGame.boardPieceTwoDArrays[diagX, diagY];
					if (target != null)
					{
						possibleMoves[diagX, diagY] = true;
					}
				}
			}

			return possibleMoves;
		}
	}
}
