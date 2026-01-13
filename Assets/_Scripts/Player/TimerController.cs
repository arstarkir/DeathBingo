using EasyTextEffects;
using System;
using TMPro;
using UnityEngine;

public class TimerController : Singleton<TimerController>
{
    [SerializeField] float time = 120;
    [SerializeField] float fastTimerAnimationStartTime = 50; // Cahnge to a better name
    [SerializeField] TMP_Text timer;

    string timerTag = "";
    [SerializeField] string calmTimerTag = "<link=CalmTimer>";
    [SerializeField] string fastTimerTag = "<link=FastTimerGitter+FastTimer>";

    void FixedUpdate()
    {
        time -= Time.fixedDeltaTime;
        UpdateTimer();
    }

    public int GetTime()
    {
        return Convert.ToInt32(time);
    }

    void UpdateTimer() // I know that this is a very strange way of doing it but I have to because of the TextEffect thing
    {
        TimeSpan ts = TimeSpan.FromSeconds(GetTime());
        string final = $"{timerTag}{string.Format("{0:0}:{1:00}", ts.Minutes, ts.Seconds)}</link>";

        if (timerTag == "")
        {
            timerTag = calmTimerTag;
            final = $"{timerTag}{string.Format("{0:0}:{1:00}", ts.Minutes, ts.Seconds)}</link>";
            timer.text = final;
            timer.GetComponent<TextEffect>().Refresh();
        }
        if (timerTag != fastTimerTag && time < fastTimerAnimationStartTime)
        {
            timerTag = fastTimerTag;
            final = $"{timerTag}{string.Format("{0:0}:{1:00}", ts.Minutes, ts.Seconds)}</link>";
            timer.text = final;
            timer.GetComponent<TextEffect>().Refresh();
        }

        if (timer.text != final)
            timer.text = final;
    }
}
