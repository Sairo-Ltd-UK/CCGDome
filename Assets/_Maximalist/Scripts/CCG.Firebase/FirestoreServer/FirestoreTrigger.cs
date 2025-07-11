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
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace CCG.Firebase
{
	public class FirestoreTrigger : MonoBehaviour
	{
		// ====== This code is just for testing 

		[FormerlySerializedAs("improvementHandler")]
		[SerializeField] private DataFeedback dataFeedback;

		public MiniGameID gameToTrack = MiniGameID.PuzzleOne;
		public ZoneID zoneToTrack = ZoneID.Lobby;

		private int min = 0;
		private int max = 5;
		public void TriggerScore()
		{
			int score = Random.Range(min, max);
			min += 5;
			max += 5;

			DebugLogger.Log($"[TriggerScore] Game: {gameToTrack}, Score: {score}, Range: {min - 5}-{max - 1}");
			dataFeedback.UpdateScoreText(score);
			StartCoroutine(GameTracker.TrackScore(gameToTrack, score));
		}

		public void TriggerZone()
		{
			DebugLogger.Log($"[TriggerZone] Entered zone: {zoneToTrack}");
			dataFeedback.EnteredZone(zoneToTrack.ToString());
			StartCoroutine(ZoneTracker.TrackZoneEntry(zoneToTrack));
		}
	}
}