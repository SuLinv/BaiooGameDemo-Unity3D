using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ItemFactory
{
    public void SetIcon();
    public void SetPrice();
    public void SetDescription();
    public GameObject CreateItem();
}
