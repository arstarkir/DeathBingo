using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I don't even know where I got this script from
public class HoverMove : MonoBehaviour
{
    public List<Transform> targets;
    public float duration = 0.1f;

    readonly Queue<float> posQueue = new Queue<float>();
    readonly Queue<float> scaleQueue = new Queue<float>();
    readonly Queue<float> rotQueue = new Queue<float>();
    bool isMoving;
    bool isScaling;
    bool isRotating;

    public void Trigger(float dist)
    {
        posQueue.Enqueue(dist);
        if (!isMoving) StartCoroutine(PosWorker());
    }

    IEnumerator PosWorker()
    {
        isMoving = true;
        while (posQueue.Count > 0)
            yield return StartCoroutine(MoveAll(posQueue.Dequeue()));
        isMoving = false;
    }

    IEnumerator MoveAll(float dist)
    {
        float time = 0f;
        var startPos = new List<Vector3>(targets.Count);
        var endPos = new List<Vector3>(targets.Count);
        for (int i = 0; i < targets.Count; i++)
        {
            var t = targets[i];
            startPos.Add(t.position);
            endPos.Add(t.position + t.forward * dist);
        }
        while (time < duration)
        {
            time += Time.deltaTime;
            float a = Mathf.Clamp01(time / duration);
            for (int i = 0; i < targets.Count; i++)
                targets[i].position = Vector3.Lerp(startPos[i], endPos[i], a);
            yield return null;
        }
    }

    public void TriggerScale(float scaleFactor)
    {
        scaleQueue.Enqueue(scaleFactor);
        if (!isScaling) StartCoroutine(ScaleWorker());
    }

    IEnumerator ScaleWorker()
    {
        isScaling = true;
        while (scaleQueue.Count > 0)
            yield return StartCoroutine(ScaleAll(scaleQueue.Dequeue()));
        isScaling = false;
    }

    IEnumerator ScaleAll(float scaleFactor)
    {
        float time = 0f;
        var startScale = new List<Vector3>(targets.Count);
        var endScale = new List<Vector3>(targets.Count);
        for (int i = 0; i < targets.Count; i++)
        {
            var t = targets[i];
            startScale.Add(t.localScale);
            endScale.Add(t.localScale * scaleFactor);
        }
        while (time < duration)
        {
            time += Time.deltaTime;
            float a = Mathf.Clamp01(time / duration);
            for (int i = 0; i < targets.Count; i++)
                targets[i].localScale = Vector3.Lerp(startScale[i], endScale[i], a);
            yield return null;
        }
    }

    public void TriggerRotateZ(float degrees)
    {
        rotQueue.Enqueue(degrees);
        if (!isRotating) StartCoroutine(RotateWorker());
    }

    IEnumerator RotateWorker()
    {
        isRotating = true;
        while (rotQueue.Count > 0)
            yield return StartCoroutine(RotateAllZ(rotQueue.Dequeue()));
        isRotating = false;
    }

    IEnumerator RotateAllZ(float degrees)
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

        while (time < duration)
        {
            time += Time.deltaTime;
            float a = Mathf.Clamp01(time / duration);
            for (int i = 0; i < targets.Count; i++)
                targets[i].rotation = Quaternion.Slerp(startRot[i], endRot[i], a);
            yield return null;
        }
    }
}
