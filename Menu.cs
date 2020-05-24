using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject volume;
    public AudioMixer mainMixer;
    public void volumeTurnOn(){
        if(volume.activeInHierarchy) volume.SetActive(false);
        else volume.SetActive(true);
    }
    public void SetVolume(float vol){
        mainMixer.SetFloat("MainVolume",vol);
    }
}
