using UnityEngine;
using UnityEngine.InputSystem;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    AudioSource sfxObjectPrefab;

    [SerializeField]
    public float masterVolume = 0.5f;
    public float sfxVolume = 1f;
    public bool musicPaused = false;

    public AudioSource PlaySFX(AudioClip audioClip, bool makeChild, Transform spawnTransform, float volume, float spatialBlend = 1f, bool loop = false, float pitch = 1f, bool ignoreListenerPause = false)
    {
        AudioSource audioSource = makeChild ? Instantiate(sfxObjectPrefab, spawnTransform) : Instantiate(sfxObjectPrefab, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume * sfxVolume * masterVolume;
        audioSource.Play();
        audioSource.spatialBlend = spatialBlend;
        audioSource.loop = loop;
        audioSource.ignoreListenerPause = ignoreListenerPause;
        audioSource.pitch = pitch;

        if (!loop)
        {
            float clipLength = audioSource.clip.length;
            Destroy(audioSource.gameObject, clipLength);
        }

        return audioSource;
    }
}