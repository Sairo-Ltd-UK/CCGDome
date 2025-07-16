// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     13/06/2025
//
//  Copyright � 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using CCG.Core.Debuging;
using UnityEngine;

namespace CCG.MiniGames.Chess
{
	[System.Serializable]
	public class ChessGame: MiniGameBase
	{
		private const int width = 8;
		private const int height = 8;
		private const int boardLength = 8;
		private const float individualTileLength = 9;

		private static readonly Vector3 originOffset = new Vector3(-40.5299988f, 7.4000001f, -31.5f);

		[Header("Chess game")]
		[SerializeField] private ChessGameUIManager chessGameUiManager;
		[SerializeField] private GameObject tilePrefab;
		[SerializeField] private GameObject piecePrefab;
		[Space]
		[SerializeField] private KingBehaviourSO kingBehaviour;
		[SerializeField] private QueenBehaviourSO queenBehaviour;
		[SerializeField] private BishopBehaviourSO bishopBehaviour;
		[SerializeField] private KnightBehaviourSO knightBehaviour;
		[SerializeField] private RookBehaviourSO rookBehaviour;
		[SerializeField] private PawnBehaviourSO pawnBehaviour;
		[Space]
		[SerializeField] private GameObject[] tiles; // Unity-serializable backing fields
		[SerializeField] private GameObject[] boardPieces; // Unity-serializable backing fields
		[Space]
		[Header("Materials")]
		[SerializeField] private Material selectedMat;
		[SerializeField] private Material enemyMat;
		[SerializeField] private Material potentialMat;
		[SerializeField] private Material transparentMat;

		private bool whiteTurn = true;

		public GameObject[,] tilesTwoDArray; //Does not show in inspector anyway.
		public GameObject[,] boardPieceTwoDArrays; //Does not show in inspector anyway.

		[HideInInspector] public GameObject selectedPiece;
		[HideInInspector] public bool[,] possibleMoves;

		[HideInInspector] public int selectedX;
		[HideInInspector] public int selectedY;
		[HideInInspector] public int currentX;
		[HideInInspector] public int currentY;

		[HideInInspector] public float localScaleY;

		public bool WhiteTurn { get => whiteTurn; set => whiteTurn = value; }

		public Vector3 OriginOffset { get => originOffset * localScaleY; }
		public float IndividualTileLength { get => individualTileLength * localScaleY; }

		private int Index(int x, int y) => y * width + x;

		private void Reset()
		{
			chessGameUiManager = FindAnyObjectByType<ChessGameUIManager>();
		}

		public void Setup(float localScaley)
		{
			this.localScaleY = localScaley;
		}

		public void LoadListsToDictionarys()
		{
			tilesTwoDArray = new GameObject[width, height];
			boardPieceTwoDArrays = new GameObject[width, height];

			if (tiles == null)
				return;

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					int i = Index(x, y);

					if (i < tiles.Length)
						tilesTwoDArray[x, y] = tiles[i];

					if (i < boardPieces.Length)
						boardPieceTwoDArrays[x, y] = boardPieces[i];
				}
			}
		}

		[ContextMenu("GenerateBoard")]
		private void GenerateBoard()
		{
			localScaleY = transform.localScale.y;

			// Initialise board tiles, piecePrefab and boolean arrays
			tilesTwoDArray = new GameObject[8, 8];
			boardPieceTwoDArrays = new GameObject[8, 8];

			possibleMoves = new bool[8, 8];

			// Procedurally generate tile game objects, and assign their positions to the SingleTile component
			Vector3 origin = transform.position + OriginOffset;
			Vector3 startOrigin = origin;

			GameObject tilesHolder = new GameObject("TilesHolder");
			tilesHolder.transform.parent = transform;
			tilesHolder.transform.localPosition = Vector3.zero;

			for (int x = 0; x < tilesTwoDArray.GetLength(0); x++)
			{
				for (int y = 0; y < tilesTwoDArray.GetLength(1); y++)
				{
					tilePrefab.name = "Tile[" + x + "," + y + "]";
					tilePrefab.GetComponent<SingleTile>().posX = x;
					tilePrefab.GetComponent<SingleTile>().posY = y;
					tilesTwoDArray[x, y] = GameObject.Instantiate(tilePrefab, new Vector3(origin.x + IndividualTileLength, origin.y, origin.z), Quaternion.identity);
					tilesTwoDArray[x, y].transform.SetParent(tilesHolder.transform);
					tilesTwoDArray[x, y].transform.localScale = new Vector3(transform.localScale.x * 8, .5f, transform.localScale.y * 8);
					origin = tilesTwoDArray[x, y].transform.position;
				}

				startOrigin.z += IndividualTileLength;
				origin = startOrigin;
			}

			// Load chess piecePrefab onto board
			LoadPieces();
		}

		[ContextMenu("LoadPieces")]
		private void LoadPieces()
		{
			GameObject piecesHolder = new GameObject("piecesHolder");
			piecesHolder.transform.parent = transform;
			piecesHolder.transform.localPosition = Vector3.zero;

			Transform piecesHolderTransfrom = piecesHolder.transform;

			selectedPiece = null;

			CreatePiece(0, 4, kingBehaviour, false, piecesHolderTransfrom);
			CreatePiece(0, 3, queenBehaviour, false, piecesHolderTransfrom);

			CreatePiece(0, 5, bishopBehaviour, false, piecesHolderTransfrom);
			CreatePiece(0, 2, bishopBehaviour, false, piecesHolderTransfrom);

			CreatePiece(0, 6, knightBehaviour, false, piecesHolderTransfrom);
			CreatePiece(0, 1, knightBehaviour, false, piecesHolderTransfrom);

			CreatePiece(0, 7, rookBehaviour, false, piecesHolderTransfrom);
			CreatePiece(0, 0, rookBehaviour, false, piecesHolderTransfrom);

			for (int i = 0; i < 8; i++)
				CreatePiece(1, i, pawnBehaviour, false, piecesHolderTransfrom);

			CreatePiece(7, 4, kingBehaviour, true, piecesHolderTransfrom);
			CreatePiece(7, 3, queenBehaviour, true, piecesHolderTransfrom);

			CreatePiece(7, 5, bishopBehaviour, true, piecesHolderTransfrom);
			CreatePiece(7, 2, bishopBehaviour, true, piecesHolderTransfrom);

			CreatePiece(7, 6, knightBehaviour, true, piecesHolderTransfrom);
			CreatePiece(7, 1, knightBehaviour, true, piecesHolderTransfrom);

			CreatePiece(7, 7, rookBehaviour, true, piecesHolderTransfrom);
			CreatePiece(7, 0, rookBehaviour, true, piecesHolderTransfrom);

			for (int i = 0; i < 8; i++)
				CreatePiece(6, i, pawnBehaviour, true, piecesHolderTransfrom);
		}

		public void CreatePiece(int x, int y, PieceBehaviourSO pieceBehaviour, bool isWhite, Transform piecesHolderTransfrom)
		{
			boardPieceTwoDArrays[x, y] = GameObject.Instantiate(piecePrefab, tilesTwoDArray[x, y].transform.position, Quaternion.identity, piecesHolderTransfrom);
			boardPieceTwoDArrays[x, y].GetComponent<ChessPiece>().SetPieceBehaviour(pieceBehaviour, isWhite);
			boardPieceTwoDArrays[x, y].transform.localScale = Vector3.one * localScaleY;
		}

		public void SomeoneWonGame(bool playerOneWon)
		{
			if (playerOneWon == true)
				chessGameUiManager.PlayerOneWin();
			else
				chessGameUiManager.PlayerTwoWin();
		}

		public void ResetChessGame()
		{
			SetSelectedPiece(0);

			for (int x = 0; x < boardLength; x++)
			{
				for (int y = 0; y < boardLength; y++)
				{
					int i = Index(x, y);
					boardPieceTwoDArrays[x, y] = boardPieces[i];

					if (boardPieceTwoDArrays[x, y] == null)
						continue;

					boardPieceTwoDArrays[x, y].transform.position = tilesTwoDArray[x, y].transform.position;
					boardPieceTwoDArrays[x, y].SetActive(true);
				}
			}
		}

		public void SwitchTurns()
		{
			DebugLogger.Log("Switch Turns");
			WhiteTurn = !WhiteTurn;
		}

		public void SetSelectedPiece(int type)
		{
			switch (type)
			{
				case 0:
					selectedPiece = null;
					ClearHighlights();
					break;
				case 1:
					BoardHighlight(possibleMoves, selectedX, selectedY);
					break;
				default:
					ClearHighlights();
					break;
			}
		}

		public void SetBoardPiece(int fromX, int fromY, int toX, int toY)
		{
			boardPieceTwoDArrays[toX, toY] = boardPieceTwoDArrays[fromX, fromY];
			boardPieceTwoDArrays[fromX, fromY].transform.position = tilesTwoDArray[toX, toY].transform.position;
			boardPieceTwoDArrays[fromX, fromY] = null;
		}

		public void RemovePiece(int x, int y)
		{
			boardPieceTwoDArrays[x, y].SetActive(false);
			boardPieceTwoDArrays[x, y] = null;
		}

		public void BoardHighlight(bool[,] array, int sX, int sY)
		{
			// Handles board highlighting for potential, enemy and player positions
			for (int x = 0; x < 8; x++)
			{
				for (int y = 0; y < 8; y++)
				{
					if (array[x, y])
					{
						if (boardPieceTwoDArrays[x, y] == null)
						{
							tilesTwoDArray[x, y].GetComponent<Renderer>().material = potentialMat;
						}
						else
						{
							tilesTwoDArray[x, y].GetComponent<Renderer>().material = enemyMat;
						}
					}
				}
			}

			tilesTwoDArray[sX, sY].GetComponent<Renderer>().material = selectedMat;
		}

		public void ClearHighlights()
		{
			foreach (GameObject tile in tilesTwoDArray)
			{
				tile.GetComponent<Renderer>().material = transparentMat;
			}
		}

		public void PromotePiece(ChessPiece chessPiece, int promotionTo)
		{
			switch (promotionTo)
			{
				case 0:
					chessPiece.PromotePiece(queenBehaviour);
					break;
				case 1:
					chessPiece.PromotePiece(bishopBehaviour);
					break;
				case 2:
					chessPiece.PromotePiece(knightBehaviour);
					break;
				case 3:
					chessPiece.PromotePiece(rookBehaviour);
					break;
			}
		}
	}
}
