// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     12/08/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------


using UnityEngine;

namespace CCG.MiniGames
{
	[RequireComponent(typeof(LineRenderer))]
	public class LineRendererController : MonoBehaviour
	{
		[Header("Ray Settings")]
		[SerializeField] private float maxDistance = 100f;
		[SerializeField] private LayerMask hitMask = Physics.DefaultRaycastLayers;
		[SerializeField] private LineRenderer lineRenderer;

		private void Reset()
		{
			lineRenderer = GetComponent<LineRenderer>();
			lineRenderer.positionCount = 2;

			lineRenderer.enabled = false;
			enabled = false;
		}

		private void OnEnable()
		{
			lineRenderer.enabled = true;

			lineRenderer.SetPosition(0, Vector3.zero);
			lineRenderer.SetPosition(1, Vector3.zero);
		}

		private void OnDisable()
		{
			lineRenderer.enabled = false;

			lineRenderer.SetPosition(0, Vector3.zero);
			lineRenderer.SetPosition(0, Vector3.zero);
		}

		private void FixedUpdate()
		{
			FireRay();
		}

		[ContextMenu("FireRay")]
		private void FireRay()
		{
			Vector3 rayOrigin = transform.position;
			Vector3 rayDirection = transform.forward;

			RaycastHit hit;
			Vector3 endPoint;

			if (Physics.Raycast(rayOrigin, rayDirection, out hit, maxDistance, hitMask))
			{
				endPoint = hit.point;
			}
			else
			{
				endPoint = rayOrigin + rayDirection * maxDistance;
			}

			lineRenderer.SetPosition(0, rayOrigin);
			lineRenderer.SetPosition(1, endPoint);
		}
	}
}
