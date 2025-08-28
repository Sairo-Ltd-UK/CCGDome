// ------------------------------------------------------------------------------
//  Project:     CCG Dome
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     25/08/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using UnityEngine.Events;

namespace CCG.Player.Prompt
{
	[System.Serializable]
	public class PromptContent
	{
		public string Message;
		public CompletionCondition Condition;
		public UnityEvent onCompleteEvent;

		public void Begin()
		{
			Condition?.OnBegin();
		}

		public bool IsComplete()
		{
			return Condition != null && Condition.CheckComplete();
		}

		public void OnCompleted()
		{ 
			if( onCompleteEvent != null )
				onCompleteEvent.Invoke();
		}
	}
}
