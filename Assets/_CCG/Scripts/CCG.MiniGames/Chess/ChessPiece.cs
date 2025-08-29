// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     13/06/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using System;
using UnityEngine;

namespace CCG.MiniGames.Chess
{
	public abstract class PieceBehaviourSO : ScriptableObject
	{
		public Mesh whiteMesh; // assign the visual mesh asset in inspector
		public Mesh blackMesh; // assign the visual mesh asset in inspector

		public abstract bool[,] PossibleMoves(ChessGame chessGame, int x, int y, bool isWhitePiece);

		protected bool IsInsideBoard(int x, int y)
		{
			return x >= 0 && x < 8 && y >= 0 && y < 8;
		}

		protected void ScanDirection(ChessGame chessGame, bool[,] possibleMoves, int startX, int startY, int dx, int dy, bool isWhitePiece)
		{
			for (int i = 1; i < 8; i++)
			{
				int newX = startX + dx * i;
				int newY = startY + dy * i;

				if (!IsInsideBoard(newX, newY)) break;

				var target = chessGame.boardPieceTwoDArrays[newX, newY];

				if (target == null)
				{
					possibleMoves[newX, newY] = true;
				}
				else
				{
					bool targetWhite = target.GetComponent<ChessPiece>().IsWhitePiece;
					if (isWhitePiece != targetWhite)
					{
						possibleMoves[newX, newY] = true;
					}
					break;
				}
			}
		}

		protected bool CanMoveTo(ChessGame chessGame, int x, int y, bool isWhitePiece)
		{
			if (!IsInsideBoard(x, y))
				return false;

			var target = chessGame.boardPieceTwoDArrays[x, y];
			return target == null || target.GetComponent<ChessPiece>().IsWhitePiece != isWhitePiece;
		}
	}

	public class ChessPiece : MonoBehaviour
	{
		[SerializeField] private PieceBehaviourSO pieceBehaviourSO;
		[SerializeField] private bool isWhitePiece;
		[Space]
		[SerializeField] private MeshFilter meshFilter;
		[SerializeField] private MeshRenderer meshRenderer;
		[Space]
		[SerializeField] private Material whiteMat;
		[SerializeField] private Material blackMat;

		public bool IsKing { get { return pieceBehaviourSO is KingBehaviourSO; } }
		public bool IsWhitePiece { get => isWhitePiece; set => isWhitePiece = value; }

		private void Reset()
		{
			meshRenderer = GetComponent<MeshRenderer>();
			meshFilter = GetComponent<MeshFilter>();
		}

		public virtual bool[,] PossibleMoves(ChessGame chessGame, int x, int y)
		{
			return pieceBehaviourSO.PossibleMoves(chessGame, x, y, IsWhitePiece);
		}

		public void SetPieceBehaviour(PieceBehaviourSO pieceBehaviourSO, bool isWhite)
		{
			this.pieceBehaviourSO = pieceBehaviourSO;
			this.isWhitePiece = isWhite;

			if(isWhite)
				meshFilter.mesh = pieceBehaviourSO.whiteMesh;
			else
				meshFilter.mesh = pieceBehaviourSO.blackMesh;

			if(isWhitePiece)
				meshRenderer.sharedMaterial = whiteMat;
			else
				meshRenderer.sharedMaterial = blackMat;

			gameObject.name = pieceBehaviourSO.name;
		}

		internal void PromotePiece(PieceBehaviourSO queenBehaviour)
		{
			throw new NotImplementedException();
		}
	}
}
