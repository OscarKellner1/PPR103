using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        [Header("Send Off")]
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

    public void SendOff()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + -1);
       

    }
}
