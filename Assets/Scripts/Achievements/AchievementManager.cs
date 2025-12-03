using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public AchievementSystem achievementSystem;

    void Start()
    {
        achievementSystem.Initialize();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            achievementSystem.UnlockAchievement(AchievementSystem.AchievementType.SpacePressed);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            achievementSystem.UnlockAchievement(AchievementSystem.AchievementType.EscPressed);
        }
    }
}
