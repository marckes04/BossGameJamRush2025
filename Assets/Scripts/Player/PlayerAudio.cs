using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField]
    private AudioClip walkSound; // Sonido de caminar

    [SerializeField]
    private AudioClip runSound; // Sonido de correr

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing.");
        }
    }

    // Método para reproducir el sonido según el estado
    public void PlayFootstepSound(bool isRunning)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = isRunning ? runSound : walkSound;
            audioSource.loop = true; // Activa el bucle para los pasos
            audioSource.Play();
        }
    }

    // Detiene los sonidos
    public void StopFootstepSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
