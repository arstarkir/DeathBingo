using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "TimerRule", menuName = "SO/TimerRule")]
public class TimerRuleSO : DamageRuleSO
{
    public string mustContain = "1";

    public override bool CheckRule(DamageSource source)
    {
        if (trigger != source && trigger != DamageSource.Ignore)
            return false;

        if (TimerController.instance.GetTime().Contains(mustContain))
            return true;

        return false;
    }
}