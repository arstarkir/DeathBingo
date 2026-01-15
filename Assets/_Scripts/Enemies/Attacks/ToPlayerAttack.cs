using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToPlayerAttack : MonoBehaviour, IAttackHandler
{
    public AttackSO attackSO { get; set; }

    readonly Queue<PastPlayerPos> samples = new Queue<PastPlayerPos>();

    void Start()
    {
        StartCoroutine(EndAfterTime(attackSO.duration, attackSO));
    }

    void Update()
    {
        samples.Enqueue(new PastPlayerPos(Time.time, CharacterController.instance.transform.position)); 
        float targetTime = Time.time - Mathf.Max(0f, attackSO.playerPosDelay);
        while (samples.Count > 1 && samples.Peek().t < targetTime)
            samples.Dequeue();
        this.transform.position = samples.Peek().pos;
    }

    public IEnumerator EndAfterTime(float s, AttackSO attack)
    {
        yield return new WaitForSeconds(s);
        attack.EndAttack();
    }
}

public struct PastPlayerPos
{
    public float t;
    public Vector3 pos;
    public PastPlayerPos(float t, Vector3 pos)
    {
        this.t = t;
        this.pos = pos;
    }
}