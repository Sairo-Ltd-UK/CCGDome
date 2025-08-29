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
using UnityEngine.UI;

namespace CCG.Core.Debuging
{
	public class DebugConsole : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI logText;
		[SerializeField] private ScrollRect scrollRect;
		[SerializeField] private int maxLines = 20;

		private Queue<string> logQueue = new();

		private void OnEnable()
		{
			Application.logMessageReceived += HandleUnityLog;
		}

		private void OnDisable()
		{
			Application.logMessageReceived -= HandleUnityLog;
		}

		private void HandleUnityLog(string message, string stackTrace, LogType type)
		{
			string prefix = type switch
			{
				LogType.Warning => "[Warning] ",
				LogType.Error => "[Error] ",
				LogType.Exception => "[Exception] ",
				_ => ""
			};

			if (type == LogType.Warning)
				return;

			EnqueueLog(prefix + message);
		}

		private void EnqueueLog(string message)
		{
			if (logQueue.Count >= maxLines)
				logQueue.Dequeue(); // Remove oldest entry

			var tempList = new List<string>(logQueue);
			tempList.Insert(0, message);
			logQueue = new Queue<string>(tempList); 
			
			UpdateLogText();
		}

		private void UpdateLogText()
		{
			logText.text = string.Join("\n", logQueue);
		}
	}
}