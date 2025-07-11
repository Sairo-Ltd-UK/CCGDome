// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Jay Alexander Andrade-Hunt
//  Company:     Maximalist Ltd
//  Created:     13/06/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using CCG.Core.Debuging;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

// ======
// =============
// This is minimal viable code to test functionality
// ============
// ======

// ===============================
// 🔧 AUTHENTICATION MANAGER
// ===============================

namespace CCG.Firebase
{
	public static class AuthManager
	{
		private static string _playerId;
		private static bool _initialized;

		public static string PlayerId
		{
			get
			{
				if (!_initialized)
					TryInitialize();

				return _playerId;
			}
		}

		private static void TryInitialize()
		{
#if SPATIAL_SDK
					if (SpatialSys.UnitySDK.SpatialBridge.actorService.localActor != null)
					{
						_playerId = SpatialSys.UnitySDK.SpatialBridge.actorService.localActor.userID;
						DebugLogger.Log("🌟 Spatial user ID detected: " + _playerId);
					}
					else
#endif
			{
				_playerId = "spatial_user_" + Guid.NewGuid();
				DebugLogger.LogWarning("⚠️ SpatialBridge.localActor not found — using fallback ID: " + _playerId);
			}

			_initialized = true;
		}
	}

	// ===============================
	// 📂 FIRESTORE MANAGER
	// ===============================
	public static class FirestoreManager
	{
		private static string endpoint = "https://us-central1-testproject-1937d.cloudfunctions.net/api";

		public static IEnumerator UploadData(string path, string json)
		{
			UnityWebRequest request = new UnityWebRequest(endpoint + path, "POST");
			byte[] body = Encoding.UTF8.GetBytes(json);
			request.uploadHandler = new UploadHandlerRaw(body);
			request.downloadHandler = new DownloadHandlerBuffer();
			request.SetRequestHeader("Content-Type", "application/json");

			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.Success)
				DebugLogger.Log("Data uploaded successfully.");
			else
				DebugLogger.LogError("Upload failed: " + request.error);
		}
	}

	// ===============================
	// 🧠 SCORE TRACKING MANAGER
	// ===============================
	public enum MiniGameID { PuzzleOne, PuzzleTwo, MemoryMatch }

	[Serializable]
	public class ScorePayload
	{
		public string userId;
		public string miniGame;
		public int score;
		public string timestamp;

		public ScorePayload(string userId, string miniGame, int score, string timestamp)
		{
			this.userId = userId;
			this.miniGame = miniGame;
			this.score = score;
			this.timestamp = timestamp;
		}
	}

	public static class GameTracker
	{
		public static IEnumerator TrackScore(MiniGameID game, int score)
		{
			var payload = new ScorePayload(
				AuthManager.PlayerId,
				game.ToString(),
				score,
				DateTime.UtcNow.ToString("o")
			);

			string json = JsonUtility.ToJson(payload);
			DebugLogger.Log("[TrackScore] Payload: " + json);
			yield return FirestoreManager.UploadData("/score", json);
		}
	}

	// ===============================
	// 📍 ZONE TRACKING MANAGER
	// ===============================
	public enum ZoneID { Lobby, GameRoom, ExitDoor }

	[Serializable]
	public class ZoneEntryPayload
	{
		public string userId;
		public string zone;
		public string enteredAt;

		public ZoneEntryPayload(string userId, string zone, string enteredAt)
		{
			this.userId = userId;
			this.zone = zone;
			this.enteredAt = enteredAt;
		}
	}

	public static class ZoneTracker
	{
		public static IEnumerator TrackZoneEntry(ZoneID zone)
		{
			var payload = new ZoneEntryPayload(
				AuthManager.PlayerId,
				zone.ToString(),
				DateTime.UtcNow.ToString("o")
			);

			string json = JsonUtility.ToJson(payload);
			DebugLogger.Log("[TrackZoneEntry] Payload: " + json);
			yield return FirestoreManager.UploadData("/zone", json);
		}
	}
}
