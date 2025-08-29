// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     13/06/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the  client.
// ------------------------------------------------------------------------------

using System;

namespace CCG.Core.Debuging
{
	public static class DebugLogger
	{
		public enum LogType { Info, Warning, Error }

		public struct LogMessage
		{
			public string Message;
			public LogType Type;
			public DateTime Timestamp;
		}

		public static event Action<LogMessage> OnLog;

		public static void Log(string message)
		{
			Emit(message, LogType.Info);
		}

		public static void LogWarning(string message)
		{
			Emit(message, LogType.Warning);
		}

		public static void LogError(string message)
		{
			Emit(message, LogType.Error);
		}

		private static void Emit(string message, LogType type)
		{
			var log = new LogMessage
			{
				Message = message,
				Type = type,
				Timestamp = DateTime.UtcNow
			};

			UnityEngine.Debug.Log(message);
			OnLog?.Invoke(log);
		}
	}
}
