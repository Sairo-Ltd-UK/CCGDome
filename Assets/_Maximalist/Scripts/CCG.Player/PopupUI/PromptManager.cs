using UnityEngine;
using System.Collections.Generic;

namespace CCG.Player.Prompt
{
	public class PromptManager : MonoBehaviour 
	{
		[SerializeField] private PromptUI promptUIPrefab;
		[SerializeField] private PromptUI promptUI;
		[Space]
		[SerializeField] private Transform targetUIPosition;
		[SerializeField] private Transform mainCameraTransform;

		[Header("Design-time prompts")]
		[SerializeField] private List<PromptContent> initialPrompts = new List<PromptContent>();

		private Queue<PromptContent> promptQueue = new Queue<PromptContent>();
		private PromptContent currentPrompt;

		private void Start()
		{
			foreach (var prompt in initialPrompts)
			{
				promptQueue.Enqueue(prompt);
			}

			if (promptUI != null)
				return;

			if (promptUIPrefab == null)
				return;

			promptUI = Instantiate(promptUIPrefab, transform.position, transform.rotation);

			promptUI.TargetUIPosition = targetUIPosition;
			promptUI.MainCameraTransform = mainCameraTransform;
		}

		private void FixedUpdate()
		{
			if (currentPrompt == null && promptQueue.Count > 0)
			{
				StartNextPrompt();
			}

			if (currentPrompt != null && currentPrompt.IsComplete())
			{
				EndCurrentPrompt();
			}
		}

		public void EnqueuePrompt(PromptContent prompt)
		{
			promptQueue.Enqueue(prompt);
		}

		private void StartNextPrompt()
		{
			Debug.Log("[PM] StartNextPrompt");
			currentPrompt = promptQueue.Dequeue();
			currentPrompt.Begin();
			promptUI.PushMessageToUI(currentPrompt.Message);
		}

		private void EndCurrentPrompt()
		{
			Debug.Log($"[PM] EndCurrentPrompt, queue count: {promptQueue.Count}");
			currentPrompt = null;

			if (promptQueue.Count == 0)
			{
				Debug.Log("[PM] promptQueue.Count == 0");
				promptUI.DisableUI();
			}
		}
	}
}
