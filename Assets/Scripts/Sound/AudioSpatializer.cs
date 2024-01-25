using UnityEngine;

public class AudioSpatializer : MonoBehaviour
{
    private Transform playerTransform;
    public float maxDistance = 10f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (playerTransform == null)
        {
            Debug.Log("Pplayer transform is nul");
        }
        if (audioSource == null)
        {
            Debug.Log("audio not assigned");
        }

        // Calculate distance between player and audio source
        float distance = Vector3.Distance(playerTransform.position, transform.position);

        // Adjust volume based on distance
        float volume = 1f - Mathf.Clamp01(distance / maxDistance);
        audioSource.volume = volume;

        // Calculate panning based on player's position relative to the audio source
        Vector3 relativePosition = playerTransform.position - transform.position;
        float pan = Mathf.Clamp(relativePosition.x / maxDistance, -1f, 1f);
        audioSource.panStereo = pan;
    }
}
