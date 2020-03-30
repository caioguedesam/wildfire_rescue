using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource source;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        source = GetComponent<AudioSource>();
    }

    public void StopMusic() {
        if(source.isPlaying) {
            source.Pause();
        }
    }

    public void ContinueMusic() {
        if (!source.isPlaying) {
            source.Play();
        }
        source.volume = 0.5f;
    }
}
