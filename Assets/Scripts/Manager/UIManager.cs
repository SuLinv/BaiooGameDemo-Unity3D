using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    ShopManager shopManager;
    // Start is called before the first frame update
    void Start()
    {
        shopManager = ShopManager.Instance; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            shopManager.ShowShop();
        }
    }
}
