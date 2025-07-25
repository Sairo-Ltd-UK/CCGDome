using UnityEngine;
using Mirror;

namespace CCG.MiniGames
{
	public class MiniGameSpawner : NetworkBehaviour
	{
		[SerializeField] private GameObject chessGamePrefab;
		[SerializeField] private Transform spawnLocation;

		public override void OnStartServer()
		{
			base.OnStartServer();
			SpawnChessGame();
		}

		[Server]
		private void SpawnChessGame()
		{
			if (chessGamePrefab == null || spawnLocation == null)
			{
				Debug.LogWarning("MinigameSpawner: Missing chess prefab or spawn location.");
				return;
			}

			GameObject chessInstance = Instantiate(chessGamePrefab, spawnLocation.position, spawnLocation.rotation);
			NetworkServer.Spawn(chessInstance);
		}
	}
}
