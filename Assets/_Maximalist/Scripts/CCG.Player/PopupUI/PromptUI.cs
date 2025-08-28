// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     25/08/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using UnityEngine;
using TMPro;
using System.Collections;

namespace CCG.Player.Prompt
{
	public class PromptUI : MonoBehaviour
	{
		[Header("PromptUI")]
		[SerializeField] private Transform UICanvasHolder;
		[SerializeField] private Canvas UICanvas;
		[SerializeField] private float UICanvasMoveSpeed;
		[Space]
		[SerializeField] private float startTolerance = 0.1f;
		[SerializeField] private float stopTolerance = 0.01f;
		[SerializeField] private float scaleDuration = 0.25f;
		[SerializeField] private float maxSnapDistance = 2.0f;
		[Space]
		[SerializeField] private TextMeshProUGUI messageText;
		[Space]
		[Header("Scene variables")]
		[SerializeField] private Transform targetUIPosition;
		[SerializeField] private Transform mainCameraTransform;

		private bool isMoving = false;
		private Vector3 defaultScale;
		private Coroutine scaleCoroutine;

		public Transform TargetUIPosition { get => targetUIPosition; set => targetUIPosition = value; }
		public Transform MainCameraTransform { get => mainCameraTransform; set => mainCameraTransform = value; }

		private void Awake()
		{
			defaultScale = UICanvasHolder.transform.localScale;

			if (MainCameraTransform == null)
			{
				MainCameraTransform = Camera.main.transform;
			}

			DisableUIInstant();
		}

		public void PushMessageToUI(string message)
		{
			messageText.text = message;
			EnableUI();
		}

		private void EnableUI()
		{
			if (UICanvas == null || UICanvas.enabled == true || isActiveAndEnabled == false) 
				return;
		
			UICanvas.enabled = true;

			// Snap to target immediately
			if (targetUIPosition != null && UICanvasHolder != null)
				UICanvasHolder.position = targetUIPosition.position;

			// Start scale-up animation
			if (scaleCoroutine != null)
				StopCoroutine(scaleCoroutine);

			scaleCoroutine = StartCoroutine(ScaleY(0f, defaultScale.y, scaleDuration));
		}

		public void DisableUI()
		{
			if (UICanvas == null)
				return;

			// Stop following
			isMoving = false;

			// Start scale-down animation
			if (scaleCoroutine != null)
				StopCoroutine(scaleCoroutine);

			scaleCoroutine = StartCoroutine(ScaleDownAndDisable(scaleDuration));
		}

		private void DisableUIInstant()
		{
			if (UICanvas != null)
				UICanvas.enabled = false;

			if (UICanvasHolder != null)
				UICanvasHolder.localScale = new Vector3(defaultScale.x, 0f, defaultScale.z);
		}


		private IEnumerator ScaleY(float from, float to, float duration)
		{
			float elapsed = 0f;

			while (elapsed < duration)
			{
				Debug.Log("Test");
				elapsed += Time.deltaTime;
				float scaleY = Mathf.Lerp(from, to, elapsed / duration);
				UICanvasHolder.localScale = new Vector3(defaultScale.x, scaleY, defaultScale.z);
				yield return null;
			}

			UICanvasHolder.localScale = new Vector3(defaultScale.x, to, defaultScale.z);
		}

		private IEnumerator ScaleDownAndDisable(float duration)
		{
			yield return ScaleY(defaultScale.y, 0f, duration);

			if (UICanvas != null)
				UICanvas.enabled = false;
		}

		private void FixedUpdate()
		{
			if (UICanvas == null || !UICanvas.enabled || targetUIPosition == null) return;

			if (MainCameraTransform != null)
				transform.rotation = Quaternion.LookRotation(transform.position - MainCameraTransform.position);

			float distance = Vector3.Distance(UICanvasHolder.position, targetUIPosition.position);

			if (distance > maxSnapDistance)
			{
				isMoving = false;
				UICanvasHolder.position = targetUIPosition.position;
				return;
			}

			if (!isMoving && distance > startTolerance)
			{
				isMoving = true;
			}
			else if (isMoving && distance <= stopTolerance)
			{
				isMoving = false;
				UICanvasHolder.position = targetUIPosition.position;
				return;
			}

			if (isMoving)
			{
				Vector3 direction = (targetUIPosition.position - UICanvasHolder.position).normalized;
				UICanvasHolder.position += direction * UICanvasMoveSpeed * Time.fixedDeltaTime;
			}
		}
	}
}
