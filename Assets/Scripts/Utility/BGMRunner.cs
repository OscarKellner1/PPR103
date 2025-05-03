using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMRunner : MonoBehaviour
{
    [Header("Looping Ambient")]
    public AudioSource loopSource;       // your always-looping AudioSource
    public AudioClip loopClip;           // main ambient track

    [Header("Sequenced Bonus Clips")]
    public AudioSource bonusSource;      // AudioSource for playing bonus clips
    public AudioClip[] bonusClips;       // drag in your 3 clips here

    [Header("Interval Settings (seconds)")]
    [Tooltip("Time to wait before each bonus clip")]
    public float minInterval = 60f;
    public float maxInterval = 120f;

    // internal index for cycling through bonusClips
    private int bonusIndex = 0;

    void Awake()
    {
        // auto-add sources if you forgot to hook them up
        if (loopSource == null) loopSource = gameObject.AddComponent<AudioSource>();
        if (bonusSource == null) bonusSource = gameObject.AddComponent<AudioSource>();
        bonusIndex = Random.Range(0, bonusClips.Length);
    }

    void Start()
    {
        // start the ambient loop
        loopSource.clip = loopClip;
        loopSource.loop = true;
        loopSource.Play();

        // begin cycling through bonus clips
        StartCoroutine(BonusClipRoutine());
    }

    IEnumerator BonusClipRoutine()
    {
        while (true)
        {
            // 1) wait a random 60–120 s
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            // 2) pick the next clip in your list
            if (bonusClips.Length > 0)
            {
                AudioClip clip = bonusClips[bonusIndex];
                bonusSource.PlayOneShot(clip);

                // advance & wrap the index
                bonusIndex = (bonusIndex + 1) % bonusClips.Length;

                // 3) include the clip’s own length before looping back
                yield return new WaitForSeconds(clip.length);
            }
        }
    }
}
