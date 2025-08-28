// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Jay A Hunt
//  Company:     Maximalist Ltd
//  Created:     11/08/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using UnityEngine;

namespace CCG.Firebase
{
	/// <summary>
	/// Appends a LocationData entry when:
	///  - an object on the specified layer enters this trigger collider, or
	///  - you call FireNow() manually.
	/// </summary>
	public class LocationEventTrigger : MonoBehaviour
	{
		[Header("Trigger Behaviour")]
		[Tooltip("Layer to detect. Default is 'LocalPlayer'.")]
		[SerializeField] private LayerMask targetLayer = 1 << 8; // Unity layer 8 by default; set to LocalPlayer in inspector

		[Tooltip("Only allow firing once until cooldown expires.")]
		[SerializeField] private float cooldownSeconds = 120f; // 2 minutes

		[Tooltip("Send immediately when the scene starts.")]
		[SerializeField] private bool fireOnStart = false;

		[Header("Event Data")]
		[Tooltip("If set, this SO provides the LocationName. If null, the string below is used.")]
		[SerializeField] private LocationEventSO eventAsset;
		

		[Header("Initialisation")]
		[Tooltip("If Firebase is not initialised yet, attempt to initialise before sending.")]
		[SerializeField] private bool initialiseIfNeeded = true;

		private float _lastFireTime = -Mathf.Infinity;

		private void Reset()
		{
			// Make sure the collider is a trigger by default
			var col = GetComponent<Collider>();
			if (col) col.isTrigger = true;
		}

		private async void Start()
		{
			if (fireOnStart)
				await FireNow();
		}

		private async void OnTriggerEnter(Collider other)
		{
			if (!enabled) return;
			if (((1 << other.gameObject.layer) & targetLayer) == 0) return;

			await FireNow();
		}

		/// <summary>
		/// Public entry point for buttons or other scripts.
		/// </summary>
		public async System.Threading.Tasks.Task FireNow()
		{
			if (Time.time - _lastFireTime < cooldownSeconds)
			{
				Debug.Log($"[LocationEventTrigger] Cooldown active. Try again in {cooldownSeconds - (Time.time - _lastFireTime):F1}s.");
				return;
			}

			// Ensure Firebase
			if (!FirebaseManager.IsInitialised)
			{
				if (!initialiseIfNeeded)
				{
					Debug.LogWarning("[LocationEventTrigger] Firebase not initialised. Aborting.");
					return;
				}

				Debug.Log("[LocationEventTrigger] Initialising Firebase on demand...");
				await FirebaseManager.InitialiseAsync();
			}

			var sent = false;

			if (eventAsset != null)
			{
				sent = await FirebaseManager.LocationData.AppendEntryAsync(eventAsset);
			}
			else
			{
				Debug.LogWarning("[LocationEventTrigger] No SO and no Location Name provided. Entry rejected.");
				return;
			}

			if (sent)
			{
				_lastFireTime = Time.time;
#if UNITY_EDITOR
				Debug.Log($"[LocationEventTrigger] Sent LocationData for UID {FirebaseManager.GetDeviceUid()}");
#endif
			}
		}

#if UNITY_EDITOR
		private void OnDrawGizmosSelected()
		{
			var col = GetComponent<Collider>();
			if (col == null) return;

			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.color = new Color(0f, 0.6f, 1f, 0.25f);

			if (col is BoxCollider bc)
				Gizmos.DrawCube(bc.center, bc.size);
			else if (col is SphereCollider sc)
				Gizmos.DrawSphere(sc.center, sc.radius);
			else if (col is CapsuleCollider cc)
				DrawCapsuleGizmo(cc);
		}

		private void DrawCapsuleGizmo(CapsuleCollider cc)
		{
			var up = Vector3.up * (cc.height * 0.5f - cc.radius);
			var centre = cc.center;
			Gizmos.DrawSphere(centre + up, cc.radius);
			Gizmos.DrawSphere(centre - up, cc.radius);
		}
#endif
	}
}
