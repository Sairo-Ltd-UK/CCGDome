using UnityEngine;

public class VideoPlayerComponent: MonoBehaviour
{
    [SerializeField] private RenderTexture renderTexture;

    [ContextMenu("SetRenderTextureAnisoLevelTo0")]
    void Start()
    {
        renderTexture.anisoLevel = 0;
    }


}
