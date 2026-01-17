using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerProgressTracker : Singleton<PlayerProgressTracker>
{
    public ProgressData curProgressData;

    private void Awake()
    {
        base.Awake();

        if(PlayerSaveSystem.TryLoad(out ProgressData data))
            curProgressData = data;
        else
        {
            
        }
    }

    public void UpdateProgressData(string name, bool val, DataType dataType)
    {

    }
}

[Serializable]
public struct ProgressData
{
    Dictionary<string, bool> attackProgress;
    Dictionary<string, bool> ruleProgress;
    Dictionary<string, bool> effectProgress;
}

public static class PlayerSaveSystem
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, $"ProgressData.json");

    public static void Save(ProgressData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);

        Debug.Log(SavePath);
    }

    public static bool TryLoad(out ProgressData data)
    {
        if (!File.Exists(SavePath))
        {
            data = default;
            return false;
        }

        string json = File.ReadAllText(SavePath);
        data = JsonUtility.FromJson<ProgressData>(json);
        return true;
    }

    public static void Delete(string name)
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);
    }
}