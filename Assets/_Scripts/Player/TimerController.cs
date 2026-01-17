using EasyTextEffects;
using System;
using TMPro;
using UnityEngine;

public class TimerController : Singleton<TimerController>
{
    [SerializeField] float time = 0;
    [SerializeField] float fastTimerAnimationStartTime = 50; // Cahnge to a better name
    [SerializeField] TMP_Text timer;

    string timerTag = "";
    [SerializeField] string calmTimerTag = "<link=CalmTimer>";
    [SerializeField] string fastTimerTag = "<link=FastTimerGitter+FastTimer>";

    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        UpdateTimer();
    }

    public string GetTime()
    {
        TimeSpan ts = TimeSpan.FromSeconds(Convert.ToInt32(time));
        return $"{timerTag}{string.Format("{0:0}:{1:00}", ts.Minutes, ts.Seconds)}</link>";
    }

    void UpdateTimer() // I know that this is a very strange way of doing it but I have to because of the TextEffect thing
    {
        if (timerTag == "")
        {
            timerTag = calmTimerTag;
            timer.text = GetTime();
            timer.GetComponent<TextEffect>().Refresh();
        }
        //if (timerTag != fastTimerTag && time < fastTimerAnimationStartTime)
        //{
        //    timerTag = fastTimerTag;
        //    timer.text = GetTime();
        //    timer.GetComponent<TextEffect>().Refresh();
        //}

        timer.text = GetTime();
    }
}
