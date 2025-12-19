using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKMusic : MonoBehaviour
{
    private static BKMusic instance;
    public static BKMusic Instance => instance;

    private AudioSource AudioSource;

    private void Awake()
    {
        instance = this;
        AudioSource = GetComponent<AudioSource>();
        if(AudioSource == null)
        {
            AudioSource = gameObject.AddComponent<AudioSource>(); 
        }

        MusicData musicData = GameDataMgr.Instance.musicData;
        //≥ı ºªØ“Ù¿÷
        SetIsOpen(musicData.isMusicOn);
        SetVolume(musicData.musicVolume);
    }
    
    public void SetIsOpen(bool isOpen)
    {
        AudioSource.mute = !isOpen;
    }
    public void SetVolume(float volume)
    {
        AudioSource.volume = volume;
    }
}
