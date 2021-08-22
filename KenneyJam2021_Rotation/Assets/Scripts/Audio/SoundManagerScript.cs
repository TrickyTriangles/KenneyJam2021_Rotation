using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    public static AudioClip BulletSound;
    static AudioSource audioSrc;


    // Start is called before the first frame update
    void Start()

    {
        BulletSound = Resources.Load<AudioClip> ("Debug");
        audioSrc = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public static void PlaySound (string clip)
    {
            switch (clip){
            case "Debug":
                audioSrc.PlayOneShot (BulletSound);
                break;
     } 
    }
}
