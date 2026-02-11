using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreenUI : Singleton<PauseScreenUI>
{
    public GameObject pauseScreen;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public AudioSource musicSource;

    private void Start()
    {
        float master = PlayerPrefs.GetFloat("masterVol", 1f);
        float music = PlayerPrefs.GetFloat("musicVol", 1f);
        float sfx = PlayerPrefs.GetFloat("sfxVol", 1f);

        masterSlider.value = master;
        musicSlider.value = music;
        sfxSlider.value = sfx;

        AudioManager.instance.masterVolume = master;
        musicSource.volume = music * master;
        AudioManager.instance.sfxVolume = sfx;

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

    public void OnMasterSliderChanged(float value)
    {
        PlayerPrefs.SetFloat("masterVol", value);
        PlayerPrefs.Save();
        AudioManager.instance.masterVolume = value;
        musicSource.volume = PlayerPrefs.GetFloat("musicVol", value) * AudioManager.instance.masterVolume;
    }

    public void OnMusicSliderChanged(float value)
    {
        PlayerPrefs.SetFloat("musicVol", value);
        PlayerPrefs.Save();
        musicSource.volume = value * AudioManager.instance.masterVolume;
    }

    public void OnSfxSliderChanged(float value)
    {
        PlayerPrefs.SetFloat("sfxVol", value);
        PlayerPrefs.Save();
        AudioManager.instance.sfxVolume = value;
    }
}
