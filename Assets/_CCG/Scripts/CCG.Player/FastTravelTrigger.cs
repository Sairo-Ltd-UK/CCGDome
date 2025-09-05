// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     04/08/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using TMPro;
using UnityEditor;
using UnityEngine;

namespace CCG.Player
{
	public class FastTravelTrigger : MonoBehaviour
	{
		[Tooltip("Target position to teleport player to")]
		[SerializeField] private FastTravelDestination fastTravelTargetLocation;
		[SerializeField] private MeshRenderer teleporterRenderer;
		[SerializeField] private TextMeshPro destinationNameText;
		[SerializeField] private int id = 0;

		private static Color[] teleporterColours = new Color[]
		{
			Color.yellow,
			Color.green, //Tutorial
			new Color(0.247f, 0.553f, 0.757f), // Communal
			new Color(0.918f, 0.506f, 0.212f), // Educational
			new Color(0.439f, 0.667f, 0.341f), // Health
			new Color(0.773f, 0.169f, 0.420f), // Nostalgia
			Color.red,
			Color.black,
			Color.blue,
			Color.cyan,
			Color.magenta,
			Color.grey,

		};

		public int Id { get => id; }

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (Id == 0 || fastTravelTargetLocation != null)
				return;

			// Find all destinations in the scene
			var destinations = GameObject.FindObjectsOfType<FastTravelDestination>(true);
			foreach (var d in destinations)
			{
				if (d.Id == Id)
				{
					fastTravelTargetLocation = d;
					EditorUtility.SetDirty(this); // mark scene dirty so Unity saves link
					break;
				}
			}
		}
#endif

		private void Start()
		{
			SetTeleporterRendererColour();
			SetDestinationText();
		}

		private void SetTeleporterRendererColour()
		{
			if (teleporterRenderer == null)
				return;

			int index = (Id > 0) ? (Id % teleporterColours.Length) : 0;
			teleporterRenderer.material.color = teleporterColours[index];
		}

		private void SetDestinationText()
		{
			if (destinationNameText == null)
				return;

			int index = (Id > 0) ? (Id % teleporterColours.Length) : 0;
			destinationNameText.text = fastTravelTargetLocation.DestinationName;
			destinationNameText.color = teleporterColours[index];
		}

		private void OnTriggerEnter(Collider other)
		{
			Debug.Log($"FastTravelTrigger: OnTriggerEnter with {other.name}");
			fastTravelTargetLocation.PlaySound();

			if (other.TryGetComponent(out FastTravelProvider fastTravelProvider) == false)
				return;

			fastTravelProvider.GenerateTeleportRequest(fastTravelTargetLocation.transform.position, fastTravelTargetLocation.transform.rotation);
		}




		private void OnDrawGizmos()
		{
			if(fastTravelTargetLocation == null)
				return;

			int index = (Id > 0) ? (Id % teleporterColours.Length) : 0;
			Gizmos.color = teleporterColours[index];
		
			Gizmos.DrawLine(transform.position, fastTravelTargetLocation.transform.position);
		}

	}
}
