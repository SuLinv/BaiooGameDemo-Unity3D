using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ActionManager : MonoBehaviour
{
    public bool isPause;

    protected void Start()
    {
        InputManager.instance.actonList.Add(this);
    }
}
