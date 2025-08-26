using CCG.MiniGames;
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

		private bool hasPrepared = true;

		private void Start()
		{
			renderTexture.anisoLevel = 0;

			if (loadingSpinner != null)
				loadingSpinner.SetActive(true);

			hasPrepared = false;

			videoPlayer.prepareCompleted += OnVideoPrepared;
			videoPlayer.loopPointReached += OnVideoEnded;
		}

		private void Update()
		{
			// Rotate loading sprite if preparing
			if (hasPrepared == false && loadingSpinner != null)
			{
				loadingSpinner.transform.Rotate(Vector3.forward, -spinSpeed * Time.deltaTime);
			}
		}

		private void OnVideoPrepared(VideoPlayer vp)
		{
			hasPrepared = true;

			// Hide spinner
			if (loadingSpinner != null)
				loadingSpinner.SetActive(false);

			videoPlayer.Play();
		}

		private void OnVideoEnded(VideoPlayer vp)
		{

		}


		private void OnTriggerEnter(Collider other)
		{
			if (!other.TryGetComponent(out VideoViewer viewer))
				return;

			if (hasPrepared)
				videoPlayer.Play();
			else
				videoPlayer.Prepare();
		}

		private void OnTriggerExit(Collider other)
		{
			if (!other.TryGetComponent(out VideoViewer viewer))
				return;

			videoPlayer.Pause();
		}
	}
}
