namespace CCG.Player.Prompt
{
	[System.Serializable]
	public class PromptContent
	{
		public string Message;
		public CompletionCondition Condition;

		public void Begin()
		{
			Condition?.OnBegin();
		}

		public bool IsComplete()
		{
			return Condition != null && Condition.CheckComplete();
		}
	}
}
