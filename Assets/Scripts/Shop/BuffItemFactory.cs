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
        // ���ö�Ӧ��ϵ
        item.GetComponent<ItemController>().Buff = buff;
        return item;
    }

    public void SetDescription()
    {
        // ��������
        item.transform.GetChild(2).GetComponent<Text>().text = buff.buffDescription;
    }

    public void SetIcon()
    {
        // ����ͼ��
        item.transform.GetChild(0).GetComponent<Image>().sprite = SetIconByType(buff);
    }

    public void SetPrice()
    {
        // ���ü۸�
        item.transform.GetChild(1).GetComponent<Text>().text = buff.buffCost + "g";
    }

    // ����buffͼ��
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
