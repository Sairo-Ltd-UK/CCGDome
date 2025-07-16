using UnityEngine;

namespace CCG.MiniGames
{
    public class MiniGameInteractionSwitch : MonoBehaviour
    {
        [Tooltip("Interactable instance of game to switch to")]
        [SerializeField]
        MiniGameInteractable targetInteractable;
        private void OnTriggerEnter(Collider other)
        {

            if (!other.TryGetComponent<GenericMiniGameInteractor>(out GenericMiniGameInteractor interactor))
            {
                return;
            }

            // if (!interactor.isLocalPlayer)
            // {
            //     return;
            // }

            interactor.SetCurrentMiniGame(targetInteractable);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<GenericMiniGameInteractor>(out var interactor))
            {
                return;
            }
            
            // if (!interactor.isLocalPlayer)
            //     return;

            interactor.ClearCurrentMiniGame(targetInteractable);
        }
    }
}
