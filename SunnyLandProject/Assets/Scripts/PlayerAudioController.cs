using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    // keep track of the jumping state ... 
    bool isJumping = false;
    bool hasJumped = false;
    // make sure to keep track of the movement as well !

    Rigidbody2D rb; // note the "2D" prefix 

    private AudioSource[] allAudioSources;
    private AudioSource playerAudio;
    private AudioSource objectsAudio;
    private AudioSource bckgAudio;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip land;
    [SerializeField] private AudioClip crouch;
    [SerializeField] private AudioClip run;
    [SerializeField] private AudioClip pickUpCherry;
    private float currentPitch;
    [SerializeField] private TriggerBox tB;
    [SerializeField] private AudioClip impactAudio;
    [SerializeField] private AudioClip rumbleAudio;

    // Start is called before the first frame update
    void Start()
    {
	    rb = GetComponent<Rigidbody2D>();
        // get the references to your audio sources here !
        allAudioSources = GetComponents<AudioSource>();
        playerAudio = allAudioSources[0];
        objectsAudio = allAudioSources[1];
        bckgAudio = allAudioSources[2];
        currentPitch = playerAudio.pitch;
    }

    // FixedUpdate is called whenever the physics engine updates
    void FixedUpdate()
    {
        // Use the ridgidbody instance to find out if the fox is
        // moving, and play the respective sound !
        // Make sure to trigger the movement sound only when
        // the movement begins ...

        // Use a magnitude threshold of 1 to detect whether the
        // fox is moving or not !
        // i.e.
        // if ( ??? > 1 && ???) {
        //    play sound here !
        // } else if ( ??? < 1 &&) {
        //   stop sound here !
        // }
        float v = rb.velocity.magnitude;
        float pY = Mathf.Abs(rb.position.y);
        if (v > 1 && !playerAudio.isPlaying && !isJumping)
        {
            playerAudio.pitch = currentPitch;
            playerAudio.clip = run;
            playerAudio.Play();
        }
        else if (v < 1 && playerAudio.clip == run && playerAudio.isPlaying)
        {
            playerAudio.Stop();
        }
        if (tB.impact)
        { 
            objectsAudio.clip = impactAudio;
            objectsAudio.Play();
        }
        else if (tB.rumble && !objectsAudio.isPlaying)
        {
            objectsAudio.clip = rumbleAudio;
            objectsAudio.Play();
        }

    }
    
    // trigger your landing sound here !
    public void OnLanding() {
        isJumping = false;
        print("the fox has landed");
        // to keep things cleaner, you might want to
        // play this sound only when the fox actually jumoed ...
        if(hasJumped)
        {
            if (Random.Range(0, 100) > 50)
            {
                playerAudio.pitch = currentPitch + 2.0f;
            }
            else
            {
                playerAudio.pitch = currentPitch;
            }
            playerAudio.clip = land;
            playerAudio.Play();
            hasJumped = false;
        }
    }

    // trigger your crouching sound here
    public void OnCrouching() {
        print("the fox is crouching");
        playerAudio.clip = crouch;
        playerAudio.Play();
        playerAudio.pitch = currentPitch;
    }
 
    // trigger your jumping sound here !
    public void OnJump() {
        isJumping = true;
        print("the fox has jumped");
        if (Random.Range(0, 100) > 50)
        {
            playerAudio.pitch = currentPitch + 1.0f;
        }
        else
        {
            playerAudio.pitch = currentPitch;
        }
        playerAudio.clip = jump;
        playerAudio.Play();
        hasJumped = true;
    }

    // trigger your cherry collection sound here !
    public void OnCherryCollect() {
        print("the fox has collected a cherry");
        objectsAudio.clip = pickUpCherry;
        objectsAudio.Play();
        playerAudio.pitch = currentPitch;
    }
}
