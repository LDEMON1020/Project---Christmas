using System.Collections;
using UnityEngine;

public class AchievementSystem : MonoBehaviour
{
    public enum AchievementType
    {
        SpacePressed,
        EscPressed
        // 업적 추가 하고 싶다면 이곳에 업적 이름 추가 후
        // AchievementManager에서 achievementSystem.UnlockAchievement(AchievementSystem.AchievementType.업적 이름);으로 불러오기
    }

    [System.Serializable]
    public class Achievement
    {
        public AchievementType type;
        public string description;
        public bool isUnlocked;
        public GameObject uiPrefab;
        public Transform popupRoot;

        public void Unlock()
        {
            if (isUnlocked) return;

            isUnlocked = true;
            Debug.Log($"업적 해금: {type}");

            // 업적 UI 활성화
            if (uiPrefab != null)
            {
                GameObject ui = Instantiate(uiPrefab, popupRoot);
                ui.transform.localScale = Vector3.one;
            }
        }

        private void ShowAchievementAlert()
        {
            Debug.Log($"알람: {type} 달성!");
        }
    }

    public Achievement[] achievements;

    public void Initialize()
    {
        foreach (Achievement ac in achievements)
        {
            ac.isUnlocked = false;
        }
    }

    public void UnlockAchievement(AchievementType type)
    {
        foreach (Achievement ac in achievements)
        {
            if (!ac.isUnlocked && ac.type == type)
            {
                ac.Unlock();
            }
        }
    }
}
