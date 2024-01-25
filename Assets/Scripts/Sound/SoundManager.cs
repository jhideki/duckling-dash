using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public GameObject swim;
    private AudioSource swimAudioSource;
    public AudioSource underWaterAudioSource;
    public AudioSource appearAudioSource;
    private Hiding hiding;
    private PlayerAnimation playerAnimation;
    // Start is called before the first frame update
    void Start()
    {
        swim.SetActive(false);
        swimAudioSource = swim.GetComponent<AudioSource>();
        hiding = GetComponent<Hiding>();
        playerAnimation = GetComponent<PlayerAnimation>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            swim.SetActive(true);
            swimAudioSource.loop = true;
        }
        else
        {
            StartCoroutine(StopSound());
        }

        if (Input.GetKeyDown(KeyCode.Space) && !hiding.GetUnderWater())
        {
            underWaterAudioSource.Play();
        }

        if (playerAnimation.isTimingAppear)
        {
            appearAudioSource.Play();
        }


    }
    IEnumerator StopSound()
    {
        swimAudioSource.loop = false;
        while (swimAudioSource.isPlaying)
        {
            yield return null;
        }
        swim.SetActive(false);
    }
}
