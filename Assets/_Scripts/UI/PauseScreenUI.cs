using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreenUI : Singleton<PauseScreenUI>
{
    public GameObject pauseScreen;
    public Slider musicSlider;
    public Slider sfxSlider;

    public AudioSource musicSource;
    public AudioSource[] sfxSources;

    private void Start()
    {
        float music = PlayerPrefs.GetFloat("musicVol", 1f);
        float sfx = PlayerPrefs.GetFloat("sfxVol", 1f);

        musicSlider.value = music;
        sfxSlider.value = sfx;

        ApplyMusicVolume(music);
        ApplySfxVolume(sfx);

        pauseScreen.SetActive(false);
    }

    public void OpenPause()
    {
        pauseScreen.SetActive(true);

        Time.timeScale = 0f;
    }

    public void ClosePause()
    {
        pauseScreen.SetActive(false);

        CharacterController.instance.isPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void ExitToMenu(string name)
    {
        CharacterController.instance.isPaused = false;
        Time.timeScale = 1f;

        SceneManager.LoadScene(name);
    }

    public void ExitGame()
    {
        CharacterController.instance.isPaused = false;
        Time.timeScale = 1f;

        Application.Quit();
    }

    public void OnMusicSliderChanged(float value)
    {
        PlayerPrefs.SetFloat("musicVol", value);
        PlayerPrefs.Save();
        ApplyMusicVolume(value);
    }

    public void OnSfxSliderChanged(float value)
    {
        PlayerPrefs.SetFloat("sfxVol", value);
        PlayerPrefs.Save();
        ApplySfxVolume(value);
    }

    void ApplyMusicVolume(float v)
    {
        if (musicSource) 
            musicSource.volume = v;
    }

    void ApplySfxVolume(float v)
    {
        if (sfxSources == null) 
            return;

        for (int i = 0; i < sfxSources.Length; i++)
            sfxSources[i].volume = v;
    }
}
