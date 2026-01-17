using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerProgressTracker : Singleton<PlayerProgressTracker>
{
    [HideInInspector] public ProgressData curProgressData;
    [HideInInspector] public EffectListSO effectsList;
    [HideInInspector] public RuleListSO ruleList;
    [HideInInspector] public AttackListSO attackList;
    [HideInInspector] public int newEffectsDone = 0;
    [HideInInspector] public int newAttacksDone = 0;
    [HideInInspector] public int newRuelsDone = 0;

    public override void Awake()
    {
        base.Awake();

        effectsList = Resources.Load<EffectListSO>("SO/EffectList");
        ruleList = Resources.Load<RuleListSO>("SO/RuleList");
        attackList = Resources.Load<AttackListSO>("SO/AttackList");

        if(!PlayerSaveSystem.TryLoad(out ProgressData data))
        {
            curProgressData = data;

            foreach (EffectSO item in effectsList.effects)
                if (!curProgressData.effectProgress.ContainsKey(item.dataName))
                    curProgressData.effectProgress.Add(item.dataName, false);
            foreach (RuleSO item in ruleList.rules)
                if (!curProgressData.ruleProgress.ContainsKey(item.dataName))
                    curProgressData.ruleProgress.Add(item.dataName, false);
            foreach (AttackSO item in attackList.attacks)
                if (!curProgressData.attackProgress.ContainsKey(item.dataName))
                    curProgressData.attackProgress.Add(item.dataName, false);
        }
        curProgressData = data;

        PlayerSaveSystem.Save(curProgressData);
    }

    public void UpdateProgressData(string name, bool val, DataType dataType)
    {
        if (dataType == DataType.Effect)
        {
            if (!curProgressData.effectProgress.ContainsKey(name))
                curProgressData.effectProgress.Add(name, val);

            if (curProgressData.effectProgress[name] == false && val)
                newEffectsDone++;
            curProgressData.effectProgress[name] = val;
        }
        if (dataType == DataType.Rule)
        {
            if (!curProgressData.ruleProgress.ContainsKey(name))
                curProgressData.ruleProgress.Add(name, val);

            if (curProgressData.ruleProgress[name] == false && val)
                newRuelsDone++;
            curProgressData.ruleProgress[name] = val;
        }
        if (dataType == DataType.Attack)
        {
            if (!curProgressData.attackProgress.ContainsKey(name))
                curProgressData.attackProgress.Add(name, val);

            if (curProgressData.attackProgress[name] == false && val)
                newAttacksDone++;
            curProgressData.attackProgress[name] = val;
        }
    }

    public string GetPercent(Dictionary<string, bool> dict)
    {
        int done = 0;
        foreach (var kvp in dict)
            if (kvp.Value)
                done++;

        return Mathf.RoundToInt((float)done / dict.Count * 100f) + "%";
    }

    public void OnDisable()
    {
        PlayerSaveSystem.Save(curProgressData);
    }

    public void OnDestroy()
    {
        PlayerSaveSystem.Save(curProgressData);
    }
}

[Serializable]
public struct ProgressData
{
    public Dictionary<string, bool> effectProgress;
    public Dictionary<string, bool> ruleProgress;
    public Dictionary<string, bool> attackProgress;
}

[Serializable]
public class ProgressSaveData //JsonUtility doesn't like Dictionary and I did not know that
{
    public List<string> effectKeys = new();
    public List<bool> effectValues = new();

    public List<string> ruleKeys = new();
    public List<bool> ruleValues = new();

    public List<string> attackKeys = new();
    public List<bool> attackValues = new();
}

public static class PlayerSaveSystem
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "ProgressData.json");

    public static void Save(ProgressData data)
    {
        ProgressSaveData save = new ProgressSaveData();

        foreach (var kvp in data.effectProgress)
        {
            save.effectKeys.Add(kvp.Key);
            save.effectValues.Add(kvp.Value);
        }

        foreach (var kvp in data.ruleProgress)
        {
            save.ruleKeys.Add(kvp.Key);
            save.ruleValues.Add(kvp.Value);
        }

        foreach (var kvp in data.attackProgress)
        {
            save.attackKeys.Add(kvp.Key);
            save.attackValues.Add(kvp.Value);
        }

        string json = JsonUtility.ToJson(save, true);
        File.WriteAllText(SavePath, json);
    }

    public static bool TryLoad(out ProgressData data)
    {
        data = new ProgressData
        {
            effectProgress = new Dictionary<string, bool>(),
            ruleProgress = new Dictionary<string, bool>(),
            attackProgress = new Dictionary<string, bool>()
        };

        if (!File.Exists(SavePath))
            return false;

        string json = File.ReadAllText(SavePath);
        ProgressSaveData save = JsonUtility.FromJson<ProgressSaveData>(json);

        for (int i = 0; i < save.effectKeys.Count; i++)
            data.effectProgress[save.effectKeys[i]] = save.effectValues[i];

        for (int i = 0; i < save.ruleKeys.Count; i++)
            data.ruleProgress[save.ruleKeys[i]] = save.ruleValues[i];

        for (int i = 0; i < save.attackKeys.Count; i++)
            data.attackProgress[save.attackKeys[i]] = save.attackValues[i];

        return true;
    }

    public static void Delete()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);
    }
}