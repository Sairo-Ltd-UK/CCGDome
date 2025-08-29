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
	[CreateAssetMenu(fileName = "KnightBehaviourSO", menuName = "Chess/PieceBehaviours/Knight")]
	public class KnightBehaviourSO : PieceBehaviourSO
	{
		public override bool[,] PossibleMoves(ChessGame chessGame, int x, int y, bool isWhitePiece)
		{
			bool[,] possibleMoves = new bool[8, 8];

			int[,] knightMoves = new int[,]
			{
				{  2,  1 }, {  2, -1 },
				{  1,  2 }, {  1, -2 },
				{ -1,  2 }, { -1, -2 },
				{ -2,  1 }, { -2, -1 }
			};

			for (int i = 0; i < knightMoves.GetLength(0); i++)
			{
				int newX = x + knightMoves[i, 0];
				int newY = y + knightMoves[i, 1];

				if (CanMoveTo(chessGame, newX, newY, isWhitePiece))
				{
					possibleMoves[newX, newY] = true;
				}
			}

			return possibleMoves;
		}
	}
}
