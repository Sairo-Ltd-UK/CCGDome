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
	[CreateAssetMenu(fileName = "RookBehaviourSO", menuName = "Chess/PieceBehaviours/Rook")]
	public class RookBehaviourSO : PieceBehaviourSO
	{
		public override bool[,] PossibleMoves(ChessGame chessGame, int x, int y, bool isWhitePiece)
		{
			bool[,] possibleMoves = new bool[8, 8];

			ScanDirection(chessGame, possibleMoves, x, y, 1, 0, isWhitePiece);  // Right
			ScanDirection(chessGame, possibleMoves, x, y, -1, 0, isWhitePiece); // Left
			ScanDirection(chessGame, possibleMoves, x, y, 0, 1, isWhitePiece);  // Up
			ScanDirection(chessGame, possibleMoves, x, y, 0, -1, isWhitePiece); // Down

			return possibleMoves;
		}
	}
}
