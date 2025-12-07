using UnityEngine;

public class GoalObject : MonoBehaviour
{
    public bool isGameClear = false;
    public GameObject GameClearPanel;
    public CoinManager coinManager;
    public int RewardCoins = 0;


    private void Awake()
    {
        coinManager = FindObjectOfType<CoinManager>();
    }
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isGameClear = true;
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                Time.timeScale = 0f; // 게임 일시정지
                GameClearPanel.SetActive(true); // 게임 클리어 패널 활성화
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            coinManager.coin += RewardCoins; // 보너스 코인 추가
        }
    }
}
