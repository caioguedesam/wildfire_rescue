using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    private AudioSource source;
    public AudioClip passBucketClip;
    public AudioClip bucketTossClip;
    public AudioClip winClip;
    public AudioClip loseClip;
    private bool hasFinished = false;

    private void Awake() {
        source = GetComponent<AudioSource>();
    }

    public void PassBucket() {
        if(!hasFinished) {
            source.clip = passBucketClip;
            source.Play();
        }
    }

    public void TossBucket() {
        if(!hasFinished) {
            Debug.Log("Playing toss clip " + bucketTossClip);
            source.clip = bucketTossClip;
            source.Play();
        }
    }

    public void WinLevel() {
        source.clip = winClip;
        source.Play();
        hasFinished = true;
    }

    public void LoseLevel() {
        source.clip = loseClip;
        source.Play();
        hasFinished = true;
    }
}
