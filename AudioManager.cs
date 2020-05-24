using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    static AudioManager current;
    public AudioClip BgmClip;
    public AudioClip putNumberClip;
    public AudioMixerGroup mainMixer,numberMixer;
    AudioSource BgmSource;
    AudioSource NumberSource;
    private void Awake() {
        if(current != null)
        {
            Destroy(gameObject);
            return;
        }
        current = this;
        DontDestroyOnLoad(gameObject);
        BgmSource = gameObject.AddComponent<AudioSource>();
        NumberSource = gameObject.AddComponent<AudioSource>();
        BgmSource.outputAudioMixerGroup = mainMixer;
        NumberSource.outputAudioMixerGroup = numberMixer;
        PlayBgm();
    }

    public static void PlayBgm(){
        current.BgmSource.clip = current.BgmClip;
        current.BgmSource.loop = true;
        current.BgmSource.Play();
    }
    public static void PauseBgm(){
        current.BgmSource.Pause();
    }
    public static void PlayPutNumberAudio(){
        current.NumberSource.clip =current.putNumberClip;
        current.NumberSource.Play();
    }
}
