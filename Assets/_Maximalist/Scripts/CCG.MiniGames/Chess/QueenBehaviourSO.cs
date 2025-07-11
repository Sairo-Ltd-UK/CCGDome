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
	[CreateAssetMenu(fileName = "QueenBehaviourSO", menuName = "Chess/PieceBehaviours/Queen")]
	public class QueenBehaviourSO : PieceBehaviourSO
	{
		public override bool[,] PossibleMoves(ChessGame chessGame, int x, int y, bool isWhitePiece)
		{
			bool[,] possibleMoves = new bool[8, 8];

			ScanDirection(chessGame, possibleMoves, x, y, 1, 0, isWhitePiece);  // Right
			ScanDirection(chessGame, possibleMoves, x, y, -1, 0, isWhitePiece); // Left
			ScanDirection(chessGame, possibleMoves, x, y, 0, 1, isWhitePiece);  // Up
			ScanDirection(chessGame, possibleMoves, x, y, 0, -1, isWhitePiece); // Down

			ScanDirection(chessGame, possibleMoves, x, y, 1, 1, isWhitePiece);   // Up-right
			ScanDirection(chessGame, possibleMoves, x, y, 1, -1, isWhitePiece);  // Down-right
			ScanDirection(chessGame, possibleMoves, x, y, -1, 1, isWhitePiece);  // Up-left
			ScanDirection(chessGame, possibleMoves, x, y, -1, -1, isWhitePiece); // Down-left

			return possibleMoves;
		}
	}
}