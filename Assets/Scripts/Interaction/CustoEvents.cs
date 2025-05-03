using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustoEvents : MonoBehaviour
{
    public AudioSource audioSource;
    [Space(10)]

        [Header("MofuHappyEvent")]
        public GameObject Mofu;
        public AudioClip Happynoise;

    [Space(10)]
    [Header("Fill Up Bucket")]
    public Animation WaterTap;

        [Space(10)]
        [Header("Third Event")]
        public AudioClip backgroundMusic;
       
    
    public void MofuHappyEvent()
    {
        Mofu.GetComponent<DialogueTrigger>().AlternateSprites = true;
        audioSource.PlayOneShot(Happynoise);
    }
    public void WaterTheBucket()
    {
        
        WaterTap.Play();
    }
}
