using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CoinData", menuName = "Coin/CoinData")]
public class CoinData : ScriptableObject
{
    public string coinName = "Coin";
    public int value = 1;        // 코인 가치
    public Sprite icon;          // UI용 아이콘

    public GameObject coinPrefab;
}
