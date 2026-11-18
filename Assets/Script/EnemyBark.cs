using System.Collections;
using UnityEngine;

public class EnemyBark : MonoBehaviour
{
    [Header("Audio Setup")]
    public AudioSource audioSource;
    [Tooltip("Add 1 or more bark/growl clips to pick from randomly")]
    public AudioClip[] barkClips;

    [Header("Random Timing")]
    [Tooltip("Minimum seconds between bark checks")]
    public float minDelay = 4f;
    [Tooltip("Maximum seconds between bark checks")]
    public float maxDelay = 10f;

    [Range(0f, 1f)]
    [Tooltip("Chance to bark when the timer triggers (0.5 = 50% chance)")]
    public float barkChance = 0.5f;

    [Header("Optional Pitch Variation")]
    public float minPitch = 0.9f;
    public float maxPitch = 1.15f;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        StartCoroutine(RandomBarkRoutine());
    }

    private IEnumerator RandomBarkRoutine()
    {
        while (true)
        {
            // Wait a random amount of time before trying to bark
            float waitTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime);

            // Roll the dice: only bark sometimes based on barkChance
            if (Random.value <= barkChance)
            {
                PlayRandomBark();
            }
        }
    }

    private void PlayRandomBark()
    {
        if (audioSource == null || barkClips == null || barkClips.Length == 0) return;
        if (audioSource.isPlaying) return; // Avoid overlapping sounds

        // Pick a random clip from the array
        AudioClip chosenClip = barkClips[Random.Range(0, barkClips.Length)];

        // Slightly randomize pitch so every bark sounds natural
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(chosenClip);
    }
}