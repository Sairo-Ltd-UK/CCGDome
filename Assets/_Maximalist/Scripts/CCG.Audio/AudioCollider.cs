using UnityEngine;

namespace CCG.Audio
{
    public class RoomMusicTrigger : MonoBehaviour
    {
        public AudioClip musicClip;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("LocalPlayer") == false)

                return;

            if (musicClip != null)
                MusicManager.Instance.PlayMusic(musicClip);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("LocalPlayer") == false)
                return;

            MusicManager.Instance.PlayMainRoomMusic();
        }
    }
}
