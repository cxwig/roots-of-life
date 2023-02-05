using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerChanger : MonoBehaviour
{
    public AudioSource AudioSource;
    private float musicVolume = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        AudioSource.volume = musicVolume;
    }

    public void UpdateVolume(float volume)
    {
        musicVolume = volume;
    }
}
