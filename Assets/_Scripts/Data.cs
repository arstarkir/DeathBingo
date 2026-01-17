using UnityEngine;

public class Data : ScriptableObject
{
    public string dataName;
    public string dataDescription;
    [HideInInspector] public bool wasDone = false;
    [HideInInspector] public DataType dataType;

    public void SetWasDone(bool val)
    {
        this.wasDone = val;
    }
}

public enum DataType
{
    Rule = 0,
    Attack = 1,
    Effect = 2
}
