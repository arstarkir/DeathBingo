using UnityEngine;

public class AudioSFXTrigger : MonoBehaviour
{
    public AudioClip ifNoAudioGive;
    public Transform theObj;
    public bool isLooping = false;

    public void StartAudio(AudioClip audioClip = null)
    {
        if(audioClip != null || ifNoAudioGive != null)
            AudioManager.instance.PlaySFX(audioClip != null ? audioClip : ifNoAudioGive, true, theObj, 2, loop: isLooping);
    }
}
