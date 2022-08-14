using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{

    Text coinCount;
    // Start is called before the first frame update
    void Awake()
    {
        coinCount = GetComponent<Text>();
    }

    private void Start()
    {
        InputManager.instance.player.GetComponent<PlayerController>().OnCoinChange += UpdateCoinCount;
    }

    void UpdateCoinCount(int curCoin)
    {
        coinCount.text = curCoin + "g";
    }
}
