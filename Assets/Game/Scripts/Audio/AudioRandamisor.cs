using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRandamisor : MonoBehaviour
{
    [SerializeField] List<AudioClip> clips;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.clip = GetRandomClip();
    }

    AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Count - 1)];
    }
}
