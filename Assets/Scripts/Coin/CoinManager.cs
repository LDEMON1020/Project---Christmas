using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public int coin;  // 현재 소지 코인

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 씬 넘어가도 유지
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 저장된 코인 불러오기
        coin = PlayerPrefs.GetInt("Coin", 0);
    }

    public void AddCoin(int amount)
    {
        coin += amount;
        PlayerPrefs.SetInt("Coin", coin);
        PlayerPrefs.Save();
    }
}