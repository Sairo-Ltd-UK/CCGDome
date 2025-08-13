using UnityEngine;

public class RoomMusicTrigger : MonoBehaviour
{
    public AudioClip musicClip;

    private void OnTriggerEnter(Collider other)
    {
        // if (!other.CompareTag("Player")) return;
        if (musicClip != null)
            MusicManager.Instance.PlayMusic(musicClip);
    }

    private void OnTriggerExit(Collider other)
    {
        // if (!other.CompareTag("Player")) return;
        MusicManager.Instance.PlayMainRoomMusic();
    }
}
