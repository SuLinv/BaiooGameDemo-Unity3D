using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BuffItemFactory : ItemFactory
{
    BuffBase buff;
    GameObject item;
    Dictionary<string, Sprite> iconPool;
    public BuffItemFactory(BuffBase buff, GameObject item, Dictionary<string, Sprite> iconPool)
    {
        this.buff = buff;
        this.item = item;
        this.iconPool = iconPool;
    }

    public GameObject CreateItem()
    {
        SetIcon();
        SetDescription(); ;
        SetPrice(); 
        // 设置对应关系
        item.GetComponent<ItemController>().Buff = buff;
        return item;
    }

    public void SetDescription()
    {
        // 设置描述
        item.transform.GetChild(2).GetComponent<Text>().text = buff.buffDescription;
    }

    public void SetIcon()
    {
        // 设置图标
        item.transform.GetChild(0).GetComponent<Image>().sprite = SetIconByType(buff);
    }

    public void SetPrice()
    {
        // 设置价格
        item.transform.GetChild(1).GetComponent<Text>().text = buff.buffCost + "g";
    }

    // 设置buff图标
    Sprite SetIconByType(BuffBase buff)
    {
        string buffName = buff.buffType.ToString();
        if (iconPool.ContainsKey(buffName))
        {
            return iconPool[buffName];
        }
        return null;
    }
}
