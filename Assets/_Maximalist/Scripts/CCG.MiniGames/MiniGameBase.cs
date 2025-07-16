using UnityEngine;

namespace CCG.MiniGames
{
    public class MiniGameBase : MonoBehaviour
    {
        [Tooltip("The switch to change game instance on player")]
        [SerializeField] MiniGameInteractionSwitch interactableSwitch;
        
        private void OnValidate()
        {
            if (interactableSwitch == null)
            {
                Debug.LogWarning("MiniGameInteractionSwitch is not assigned!", this);
            }
        }
    }
}
