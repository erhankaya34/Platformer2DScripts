using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    private float levelStartTime;
    private float levelCompletionTime;

    void Start()
    {
        StartLevel();
    }

    public void StartLevel()
    {
        levelStartTime = Time.time;
    }

    public void EndLevel()
    {
        levelCompletionTime = Time.time - levelStartTime;
        AdjustBossDifficulty(levelCompletionTime);
    }

    private void AdjustBossDifficulty(float completionTime)
    {
        if (completionTime < 30f)
        {
            // High difficulty
            BossEnemy.Instance.SetStats(500, 50); //High health and damage
        }
        else if (completionTime < 60f)
        {
            // Medium difficulty
            BossEnemy.Instance.SetStats(300, 30); //Medium health and damage
        }
        else
        {
            // Low difficulty
            BossEnemy.Instance.SetStats(200, 15); //Low health and damage
        }
    }
}