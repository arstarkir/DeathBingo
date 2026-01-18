using System.Collections;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenUI : Singleton<EndScreenUI>
{
    public GameObject winScreen;
    public TMP_Text completionTime;
    public TMP_Text lifeLost;
    public TMP_Text maxRuleCombo;
    public TMP_Text newRules;
    public TMP_Text newAttacks;
    public TMP_Text overAllRules;
    public TMP_Text overAllAttack;

    public void WinScreen()
    {
        PlayerProgressTracker tracker = PlayerProgressTracker.instance;
        winScreen.SetActive(true);

        completionTime.text = TimerController.instance.GetTime();
        lifeLost.text = Health.instance.livesLostInRun.ToString();
        maxRuleCombo.text = BingoController.instance.maxRuleCombo.ToString();
        newRules.text = tracker.newRuelsDone.ToString();
        newAttacks.text = tracker.newAttacksDone.ToString();
        overAllRules.text = tracker.GetPercent(tracker.curProgressData.ruleProgress);
        overAllAttack.text = tracker.GetPercent(tracker.curProgressData.attackProgress);

        TimerController.instance.enabled = false;
        EnemyAttackSelection.instance.enabled = false;
        EffectsManager.instance.enabled = false;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneWithSmallDelay(string sceneName)
    {
        StartCoroutine(LoadSceneThingy(sceneName));
    }

    IEnumerator LoadSceneThingy(string sceneName)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }
}
