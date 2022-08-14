using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ItemController : MonoBehaviour
{

    private BuffBase buff;
    public BuffBase Buff
    {
        get { return buff; }
        set { buff = value; }
    }

    public void OnClick()
    {
        ShopManager.Instance.SetChosen(gameObject);
    }
}
