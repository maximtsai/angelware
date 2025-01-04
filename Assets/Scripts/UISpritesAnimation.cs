using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UISpritesAnimation : MonoBehaviour
{
    public float duration;
    public bool playOnAwake;
    public bool loop;
    public int currentSpriteSequence;

    [SerializeField] private List<ListWrapper> spriteSequences = new List<ListWrapper>();
    
    private Image image;
    private int index = 0;
    private float timer = 0;
    private bool isPlaying = false;

    void Start()
    {
        image = GetComponent<Image>();
        if (playOnAwake) {
            isPlaying = true;
        }
    }
    private void Update()
    {
        List<Sprite> sprites = spriteSequences[currentSpriteSequence].myList;
        if (isPlaying) {
            if ( (timer+=Time.deltaTime) >= (duration / sprites.Count) )
            {
                timer = 0;
                image.sprite = sprites[index];
                index += 1;
                if (loop) {
                    index %= sprites.Count;
                }
                else if (index >= sprites.Count) {
                    isPlaying = false;
                    index = 0;
                }
            }
        }   
    }

    public void Play()
    {
        index = 0;
        isPlaying = true;
    }

    public void Stop()
    {
        isPlaying = false;
    }
}

[System.Serializable]
public class ListWrapper
{
     public List<Sprite> myList;
}