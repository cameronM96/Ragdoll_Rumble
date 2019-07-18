using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteAudioTest : MonoBehaviour
{

    public bool isMuted = false;
    public Sprite mute1; 
    public Sprite mute2; 

    private SpriteRenderer spriteRenderer;

    //Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // we are accessing the SpriteRenderer that is attached to the Gameobject
        if (spriteRenderer.sprite == null) // if the sprite on spriteRenderer is null then
            spriteRenderer.sprite = mute1; // set the sprite to sprite1
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (isMuted == false)
            {
                AudioListener.pause = true;
                AudioListener.volume = 0;
                isMuted = true;
                ChangeSprite();

                Debug.Log("Clicked");

            }
            else
            {
                AudioListener.pause = false;
                AudioListener.volume = 1;
                isMuted = false;
                ChangeSprite();

            }
        }
    }

    void ChangeSprite()
    {
        if (spriteRenderer.sprite == mute1) // if the spriteRenderer sprite = sprite1 then change to sprite2
        {
            spriteRenderer.sprite = mute2;
        }
        else
        {
            spriteRenderer.sprite = mute1; // otherwise change it back to sprite1
        }
    }


    public void muteAudio()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            AudioListener.pause = true;
            AudioListener.volume = 0;
            isMuted = true;
            ChangeSprite();

            Debug.Log("Clicked");
        }
    }
}
