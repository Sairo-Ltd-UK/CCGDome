using UnityEngine;
using CCG.CustomInput;

namespace CCG.MiniGames
{
    public class PlayerEquipment : MonoBehaviour
    {
        //This is technically dupication from the interactor, but i want to keep them seprated. 
        [SerializeField] private CustomInputActionData fireAction;
        [SerializeField] private AudioSource gunAudioSource;
        [SerializeField] private AudioClip gunFireSound;

        public void OnEquiped()
        {
            if (fireAction != null)
            {
                fireAction.AddToInputActionReference(OnFireActionPressed);
            }
        }

        public void OnFireActionPressed()
        {
            if (gunAudioSource == null)
                return;

            if(gunFireSound == null)
                return;

            gunAudioSource.clip = gunFireSound;
            gunAudioSource.Play();
        }

        public void OnUnEquiped()
        {
            if (fireAction != null)
            {
                fireAction.RemoveFromInputActionReference(OnFireActionPressed);
            }
        }
    }
}
