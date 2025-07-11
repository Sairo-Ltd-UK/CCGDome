// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     13/06/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CCG.Core.Debuging
{
	public class DebugConsole : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI logText;
		[SerializeField] private int maxLines = 20;

		private Queue<string> logQueue = new();

		private void OnEnable()
		{
			DebugLogger.OnLog += HandleLog;
		}

		private void OnDisable()
		{
			DebugLogger.OnLog -= HandleLog;
		}

		private void HandleLog(DebugLogger.LogMessage log)
		{
			if (logQueue.Count >= maxLines)
			{
				logQueue.Dequeue(); // Remove oldest log entry
			}

			logQueue.Enqueue($"{log.Message}");
			UpdateLogText();
		}

		private void UpdateLogText()
		{
			logText.text = string.Join("\n", logQueue);
		}
	}
}