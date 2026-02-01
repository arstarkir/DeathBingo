using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PotControllerUI : MonoBehaviour
{
    public GameObject front;
    public GameObject backSettings;
    public GameObject backCredits;

    public Material main;
    public Material play;
    public Material settings;
    public Material credits;
    public Material quit;
    public Material back;

    public List<Transform> targets;

    MeshRenderer meshRenderer;
    Coroutine rotAllZ = null;

    public void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        SetMainActive();
    }

    public void SetMainActive()
    {
        meshRenderer.material = main;
    }

    public void SetPlayActive()
    {
        meshRenderer.material = play;
    }

    public void SetSettingsActive()
    {
        meshRenderer.material = settings;
    }

    public void SetCreditsActive()
    {
        meshRenderer.material = credits;
    }

    public void SetQuitActive()
    {
        meshRenderer.material = quit;
    }

    public void SetBackActive()
    {
        meshRenderer.material = back;
    }

    public void StartSettings(float degrees)
    {
        StartRotateAllZ(degrees);
        StartBackSettings();
    }

    public void StartCredits(float degrees)
    {
        StartRotateAllZ(degrees);
        StartBackCredits();
    }

    public void StartBack(float degrees)
    {
        StartRotateAllZ(degrees);
        front.SetActive(true);
        backCredits.SetActive(false);
        backSettings.SetActive(false);
    }

    public void StartBackSettings()
    {
        front.SetActive(false);
        backCredits.SetActive(true);
    }

    public void StartBackCredits()
    {
        front.SetActive(false);
        backSettings.SetActive(true);
    }

    public void StartRotateAllZ(float degrees)
    {
        if(rotAllZ == null)
            rotAllZ = StartCoroutine(RotateAllZ(degrees));
    }

    public IEnumerator RotateAllZ(float degrees)
    {
        float time = 0f;
        var startRot = new List<Quaternion>(targets.Count);
        var endRot = new List<Quaternion>(targets.Count);

        for (int i = 0; i < targets.Count; i++)
        {
            var t = targets[i];
            var s = t.rotation;
            startRot.Add(s);
            endRot.Add(s * Quaternion.Euler(0f, 0f, degrees));
        }

        while (time < 1f)
        {
            time += Time.deltaTime;
            float a = Mathf.Clamp01(time / 1f);
            for (int i = 0; i < targets.Count; i++)
                targets[i].rotation = Quaternion.Slerp(startRot[i], endRot[i], a);
            yield return null;
        }
        rotAllZ = null;
    }

    public void Play(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
}
