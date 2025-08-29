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
	[CreateAssetMenu(fileName = "KingBehaviourSO", menuName = "Chess/PieceBehaviours/King")]
	public class KingBehaviourSO : PieceBehaviourSO
	{
		public override bool[,] PossibleMoves(ChessGame chessGame, int x, int y, bool isWhitePiece)
		{
			bool[,] possibleMoves = new bool[8, 8];

			int[,] moves = new int[,]
			{
				{ 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 },
				{ 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 }
			};

			for (int i = 0; i < moves.GetLength(0); i++)
			{
				int newX = x + moves[i, 0];
				int newY = y + moves[i, 1];

				if (CanMoveTo(chessGame, newX, newY, isWhitePiece))
				{
					possibleMoves[newX, newY] = true;
				}
			}

			return possibleMoves;
		}
	}
}
