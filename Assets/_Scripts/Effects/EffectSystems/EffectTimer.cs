using System;

// Old Script I wrote. Don't expect this amount of comments
public class EffectTimer
{
    // The total duration of the effect
    public float duration { get; set; }

    // The current elapsed time for the effect
    public float curTime { get; private set; }

    // Action to invoke when the timer completes
    public Action OnTimerComplete { get; private set; }

    // Reference to the effect handler associated with this timer
    public EffectHandler effectHendler { get; private set; }

    // Constructor to initialize the timer with a duration, completion action, and optional effect handler
    public EffectTimer(float duration, Action onTimerComplete, EffectHandler effect = null, int effectID = -1)
    {
        this.duration = duration;
        OnTimerComplete = onTimerComplete;
        effectHendler = effect;
        curTime = 0f;
    }

    // Checks if this timer corresponds to a specific effect on a specific entity
    public bool IsThisEffectTimer(EffectSO effect, Entity entity)
    {
        // Returns true if the timer's effect matches the provided effect and entity
        if (effectHendler != null && effectHendler.effectSO.effectName == effect.effectName)
            return true;

        return false;
    }

    public bool IsThisEffectUseTimer(EffectUse use, Entity entity)
    {
        if (effectHendler != null && effectHendler.effectSO.effectUse == use)
            return true;

        return false;
    }

    // Updates the timer; returns true if the timer has finished
    public bool UpdateTimer(float deltaTime)
    {
        // Trigger the effect start if it's the first update and the handler exists
        if (curTime == 0 && effectHendler != null)
            effectHendler.effectSO.OnEffectStart();

        // Update the current elapsed time
        curTime += deltaTime;

        // Trigger the effect while the timer is active
        if (effectHendler != null)
            effectHendler.effectSO.TriggerEffect(deltaTime);

        // Check if the timer has completed
        if (curTime >= duration)
        {
            // End the effect and invoke the completion action
            if (effectHendler != null)
                effectHendler.effectSO.OnEffectEnd();
            else
                OnTimerComplete?.Invoke();
            // Timer has completed
            return true; 
        }
        // Timer is still running
        return false; 
    }
}
