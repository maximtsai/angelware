using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoEndDetector : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign this in the Inspector

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the VideoPlayer is assigned
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // Subscribe to the loopPointReached event
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene("MainScene");
        // Add any additional logic here (e.g., load a new scene, display UI, etc.)
    }

    // Optional: Clean up the event subscription when the object is destroyed
    void OnDestroy()
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
    }
}
