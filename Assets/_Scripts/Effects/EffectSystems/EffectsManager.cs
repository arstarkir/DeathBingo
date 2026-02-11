using System;
using System.Collections.Generic;
using UnityEngine;

// Old Script I wrote. Don't expect this amount of comments

/// <summary>
/// Manages all active effects and their timers
/// Handles the lifecycle of effect handlers and updates them over time
/// </summary>
public class EffectsManager : Singleton<EffectsManager>
{
    // List of all active effect timers
    public List<EffectTimer> activeTimers = new List<EffectTimer>();
    // List to store timers that should be Added
    private List<EffectTimer> timersToAdd = new List<EffectTimer>();
    // List to store timers that should be removed
    private List<EffectTimer> timersToRemove = new List<EffectTimer>();
    CharacterController characterController;
    [HideInInspector]
    public EffectListSO effectsList;

    public override void Awake()
    {
        base.Awake();
        effectsList = Resources.Load<EffectListSO>("SO/EffectList");
    }

    public void Start()
    {
        characterController = CharacterController.instance;
    }

    private void Update()
    {
        if (characterController.isPaused)
            return;

        // To avoid modifications OnEffectEnd or OnTimerComplete
        List<EffectTimer> timersToRemoveCopy = new List<EffectTimer>(timersToRemove);

        // Remove all marked timers from the active timers list
        foreach (EffectTimer timer in timersToRemoveCopy)
        {
            timer.effectHendler.effectSO.OnEffectEnd();
            timer.OnTimerComplete?.Invoke();
            Entity entity = timer.effectHendler ? timer.effectHendler.GetComponent<Entity>() : null;
            if (entity != null)
            {
                int effectId = effectsList.GetEffectId(timer.effectHendler.effectSO);
                if (effectId >= 0)
                {
                    foreach (EffectHandler handler in entity.GetComponents<EffectHandler>())
                    {
                        if (handler.effectId == effectId)
                        {
                            Destroy(handler);
                            break;
                        }
                    }
                }
            }

            // Destroy the effect handler and mark the timer for removal
            if (timer.effectHendler != null)
                DestroyEffectHendler(timer.effectHendler);

            activeTimers.Remove(timer);
        }

        // Removes all timers that were fully removed
        timersToRemove.RemoveAll(timer => !activeTimers.Contains(timer));

        // Adds all marked timers to the active timers list
        if (timersToAdd.Count > 0)
            activeTimers.AddRange(timersToAdd);

        timersToAdd.Clear();

        // Update all active effect timers
        float delta = Time.deltaTime;
        for (int i = 0; i < activeTimers.Count; i++)
        {
            var timer = activeTimers[i];
            if (timer.UpdateTimer(delta))
                timersToRemove.Add(timer);
        }
    }

    /// <summary>
    /// Adds an effect to the entity for a specific duration
    /// </summary>
    /// <param name="effect">The effect to be applied</param>
    /// <param name="entity">The entity the effect is applied to</param>
    /// <param name="time">The duration the effect will last</param>
    public EffectHandler AddEffectToEntityForTime(EffectSO effect, Entity entity, float time, GameObject caster = null, Action? action = null)
    {
        if (time < 0)
            time = float.MaxValue - 100;

        effect = Instantiate(effect);
        effect.thisEntity = entity;

        // Add the EffectHandler component to the entity's GameObject
        EffectHandler temp = entity.gameObject.AddComponent<EffectHandler>();
        temp.effectSO = effect;
        temp.effectId = effectsList.GetEffectId(effect);

        // Create a new effect timer and add it to the active timers list
        if (!temp.effectSO.isTimeStacked || !temp.effectSO.isResetStacked || !IsActiveEffectOnEntity(effect,entity))
        {
            EffectTimer effectTimer = new EffectTimer(time, (action != null ? () => { action.Invoke(); DestroyEffectHendler(temp); } : () => DestroyEffectHendler(temp)), temp);
            timersToAdd.Add(effectTimer);
        }
        else
        {
            foreach (EffectTimer timer in activeTimers)
            {
                if (timer.IsThisEffectTimer(effect, entity))
                {
                    if (temp.effectSO.isTimeStacked)
                        timer.duration += effect.effectDurationTime;
                    if (temp.effectSO.isResetStacked)
                        timer.duration = effect.effectDurationTime;
                }
            }
        }

        return temp;
    }

    /// <summary>
    /// Removes all effects of the effect from the entity
    /// </summary>
    /// <param name="effect">The effect to be removed</param>
    /// <param name="entity">The entity the effects should be removed from</param>
    public void RemoveAllEffectOfKindFromEntity(EffectSO effect, Entity entity)
    {
        // Iterate through active timers and find the ones that match the effect and entity
        foreach (EffectTimer timer in activeTimers)
        {
            if (timer.IsThisEffectTimer(effect, entity))
                timersToRemove.Add(timer);
        }
    }

    public void TryRemoveFirstEffectOfKindFromEntity(EffectSO effect, Entity entity)
    {
        foreach (EffectTimer timer in activeTimers)
        {
            if (timer.IsThisEffectTimer(effect, entity))
            {
                timersToRemove.Add(timer);
                return;
            }
        }
    }

    public void RemoveAllEffectOfUseFromEntity(EffectUse use, Entity entity)
    {
        foreach (EffectTimer timer in activeTimers)
        {
            if (timer.IsThisEffectUseTimer(use, entity))
                timersToRemove.Add(timer);
        }
    }

    /// <summary>
    /// Removes all effects from the entity
    /// </summary>
    /// <param name="entity">The entity the effects should be removed from</param>
    public void RemoveAllEffectFromEntity(Entity entity)
    {
        foreach (EffectTimer timer in activeTimers)
        {
            if (entity == timer.effectHendler.GetComponent<Entity>())
                timersToRemove.Add(timer);
        }
    }

    /// <summary>
    /// Checks if the specified effect is currently active on the entity
    /// </summary>
    /// <param name="effect">The effect to check for</param>
    /// <param name="entity">The entity to check on</param>
    /// <returns>True if the effect is active on the entity, false otherwise</returns>
    public bool IsActiveEffectOnEntity(EffectSO effect, Entity entity)
    {
        // Iterate through active timers to see if the effect is active on the entity
        foreach (EffectTimer timer in activeTimers)
        {
            if (timer.IsThisEffectTimer(effect, entity))
                // Effect is active
                return true;
        }
        // Effect is not active
        return false;
    }

    public int GetActiveEffectOnEntityAmount(EffectSO effect, Entity entity)
    {
        int amount = 0;

        foreach (EffectTimer timer in activeTimers)
        {
            if (timer.IsThisEffectTimer(effect, entity))
                amount++;
        }

        return amount;
    }

    public float GetLongestRemainingEffectOnEntity(EffectSO effect, Entity entity)
    {
        float longest = -1f;

        foreach (EffectTimer timer in activeTimers)
        {
            if (!timer.IsThisEffectTimer(effect, entity))
                continue;

            float left = timer.duration - timer.curTime;

            if (left > longest)
                longest = left;
        }

        return longest;
    }


    /// <summary>
    /// Removes the effect handler from the entity
    /// </summary>
    /// <param name="effectHendler">The effect handler to be destroyed</param>
    private void DestroyEffectHendler(EffectHandler effectHendler)
    {
        Destroy(effectHendler);
    }
}
