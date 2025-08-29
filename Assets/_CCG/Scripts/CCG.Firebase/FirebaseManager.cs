// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Jay A Hunt
//  Company:     Maximalist Ltd
//  Created:     11/08/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;

namespace CCG.Firebase
{
	/// <summary>
	/// Static facade for Firebase. Initialises and exposes modules.
	/// Uses the device ID as a temporary UID until real auth is added.
	/// </summary>
	public static class FirebaseManager
	{
		public static bool IsInitialised { get; private set; }
		public static FirebaseApp App { get; private set; }
		public static FirebaseAuth Auth { get; private set; }
		public static FirebaseFirestore Db { get; private set; }

		// Modules
		public static FirestoreLocationData LocationData { get; private set; }

		// Flag you can flip in code or via a menu later. We log if unsupported.
		public static bool UseEmulators { get; set; } =
#if UNITY_EDITOR
			true;
#else
			false;
#endif

		private static readonly object _initLock = new object();
		private static Task _initialiseTask;
		private static string _deviceUid;

		/// <summary>
		/// Idempotent initialiser. Safe to call more than once.
		/// </summary>
		public static Task InitialiseAsync(CancellationToken ct = default)
		{
			lock (_initLock)
			{
				if (_initialiseTask == null || _initialiseTask.IsFaulted || _initialiseTask.IsCanceled)
					_initialiseTask = DoInitialiseAsync(ct);

				return _initialiseTask;
			}
		}

		private static async Task DoInitialiseAsync(CancellationToken ct)
		{
			if (IsInitialised) return;

			try
			{
				var depStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
				if (depStatus != DependencyStatus.Available)
					throw new Exception($"Firebase dependencies not available: {depStatus}");

				App = FirebaseApp.DefaultInstance;

				// Auth: Unity SDK does not expose a public UseEmulator API.
				Auth = FirebaseAuth.DefaultInstance;

				// Firestore: in newer Unity SDKs Settings may be get-only.
				// We take the default instance to guarantee compile-time compatibility.
				Db = FirebaseFirestore.DefaultInstance;

				if (UseEmulators)
				{
#if UNITY_EDITOR
					Debug.Log("[Firebase] Emulator mode requested. Unity Auth has no public emulator API. Firestore settings may be read-only in this SDK, so no host will be set here.");
#else
					Debug.Log("[Firebase] Emulator mode requested but not applied in player builds via code. Ensure you point to real project data or wire emulators using supported platform APIs.");
#endif
				}

				// Modules
				LocationData = new FirestoreLocationData(Db, GetDeviceUid);

				IsInitialised = true;
				Debug.Log($"[Firebase] Initialised. UID={GetDeviceUid()}");
			}
			catch (Exception ex)
			{
				Debug.LogError($"[Firebase] Initialisation failed: {ex}");
				throw;
			}
		}

		/// <summary>
		/// Temporary UID derived from device. Falls back to a persisted GUID if unavailable.
		/// </summary>
		public static string GetDeviceUid()
		{
			if (!string.IsNullOrEmpty(_deviceUid)) return _deviceUid;

			var sysId = SystemInfo.deviceUniqueIdentifier;
			if (!string.IsNullOrEmpty(sysId) && !sysId.Equals("Unknown", StringComparison.OrdinalIgnoreCase))
			{
				_deviceUid = sysId;
				return _deviceUid;
			}

			const string key = "ccg_device_uid_fallback";
			if (PlayerPrefs.HasKey(key))
			{
				_deviceUid = PlayerPrefs.GetString(key);
			}
			else
			{
				_deviceUid = Guid.NewGuid().ToString("N");
				PlayerPrefs.SetString(key, _deviceUid);
				PlayerPrefs.Save();
			}
			return _deviceUid;
		}

		public static void EnsureInitialised()
		{
			if (!IsInitialised)
				throw new InvalidOperationException("FirebaseManager not initialised. Call InitialiseAsync() first.");
		}
	}
}
