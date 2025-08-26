using UnityEngine;

namespace CCG.Player.Prompt
{
	public abstract class CompletionCondition : MonoBehaviour
	{
		public abstract void OnBegin();
		public abstract bool CheckComplete();
	}
}
