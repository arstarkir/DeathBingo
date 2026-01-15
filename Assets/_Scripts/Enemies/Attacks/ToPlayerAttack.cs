using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// That's what happans when you rush (refering to the cahnges)
public class ToPlayerAttack : MonoBehaviour, IAttackHandler
{
    public AttackSO attackSO { get; set; }

    readonly Queue<TimePlayerPos> playerPoses = new Queue<TimePlayerPos>();
    CharacterController cc;

    void Start()
    {
        cc = CharacterController.instance;
    }

    void Update()
    {
        playerPoses.Enqueue(new TimePlayerPos(Time.time, new Vector3(cc.transform.position.x,0, cc.transform.position.z))); 

        float targetTime = Time.time - Mathf.Max(0f, attackSO.playerPosDelay);
        while (playerPoses.Count > 1 && playerPoses.Peek().t < targetTime)
            playerPoses.Dequeue();

        this.transform.position = playerPoses.Peek().pos;
    }
}

public struct TimePlayerPos
{
    public float t;
    public Vector3 pos;

    public TimePlayerPos(float t, Vector3 pos)
    {
        this.t = t;
        this.pos = pos;
    }
}