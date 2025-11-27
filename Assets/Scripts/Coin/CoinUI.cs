using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour
{
    public Text coinText;

    void Update()
    {
        coinText.text = CoinManager.Instance.coin.ToString();
    }
}
