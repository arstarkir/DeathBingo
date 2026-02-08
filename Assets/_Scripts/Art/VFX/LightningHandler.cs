using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class LightningHandler : MonoBehaviour
{
    [SerializeField] float duration = 5;
    [SerializeField] GameObject lightningVFX;
    [SerializeField] List<ParticleSystem> particles = new List<ParticleSystem>();
    [SerializeField] ParticleSystem warningLights;
    [SerializeField] GameObject trigger;

    private void Start()
    {
        lightningVFX.SetActive(false);
        StartLightning(duration);
    }

    public void StartLightningJustDelay(float delay)
    {
        foreach (ParticleSystem particle in particles)
        {
            ParticleSystem.MainModule main = particle.main;
            main.startDelay = delay;
        }

        ParticleSystem.MainModule warningMain = warningLights.main;
        warningMain.duration = delay;
        lightningVFX.SetActive(true);
        StartCoroutine(EnableDmgTrigger(delay, 0.2f));
    }

    public void StartLightning(float delay, float dmgTimeWindow = 0.2f)
    {
        foreach (ParticleSystem particle in particles)
        {
            ParticleSystem.MainModule main = particle.main;
            main.startDelay = delay;
        }

        ParticleSystem.MainModule warningMain = warningLights.main;
        warningMain.duration = delay;
        lightningVFX.SetActive(true);
        StartCoroutine(EnableDmgTrigger(delay, dmgTimeWindow));
    }

    IEnumerator EnableDmgTrigger(float delay, float dmgTimeWindow = 0.2f)
    {
        yield return new WaitForSeconds(delay);
        if(trigger != null)
        {
            trigger.GetComponent<Collider>().enabled = true;
            if(trigger.TryGetComponent<DealEffect>(out DealEffect dealEffect))
                dealEffect.enabled = true;
            if (trigger.TryGetComponent<DealDamage>(out DealDamage dealDamage))
                dealDamage.enabled = true;
        }
        yield return new WaitForSeconds(dmgTimeWindow);
        if (trigger != null)
        {
            trigger.GetComponent<Collider>().enabled = false;
            if (trigger.TryGetComponent<DealEffect>(out DealEffect dealEffect))
                dealEffect.enabled = false;
            if (trigger.TryGetComponent<DealDamage>(out DealDamage dealDamage))
                dealDamage.enabled = false;
        }
        Destroy(gameObject);
    }
}