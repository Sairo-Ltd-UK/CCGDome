using UnityEngine;
using UnityEngine.Video;

namespace CCG.Video 
{
    public class VideoPlayerComponent : MonoBehaviour
    {
        [Header("Video Setup")]
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private RenderTexture renderTexture;

        [Header("Loading UI")]
        [SerializeField] private GameObject loadingSpinner; 
        [SerializeField] private float spinSpeed = 180f; 

        private bool isPreparing = true;

        private void Start()
        {
            renderTexture.anisoLevel = 0;

            if (loadingSpinner != null)
                loadingSpinner.SetActive(true);

            isPreparing = true;

            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.loopPointReached += OnVideoEnded;

            videoPlayer.Prepare();
        }

        private void Update()
        {
            // Rotate loading sprite if preparing
            if (isPreparing && loadingSpinner != null)
            {
                loadingSpinner.transform.Rotate(Vector3.forward, -spinSpeed * Time.deltaTime);
            }
        }

        private void OnVideoPrepared(VideoPlayer vp)
        {
            isPreparing = false;

            // Hide spinner
            if (loadingSpinner != null)
                loadingSpinner.SetActive(false);

            vp.Play();
        }

        private void OnVideoEnded(VideoPlayer vp)
        {
            // Optional: show spinner again, or handle UI
        }
    }
}
