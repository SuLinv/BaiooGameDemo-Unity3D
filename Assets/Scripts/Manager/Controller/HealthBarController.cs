using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{

    public static HealthBarController instance;

    // Ѫ����ʼ����
    float oriLen;
    // Ѫ����ǰ����
    float curLen;

    RectTransform rectTransform;

    Image image;
    private void Awake()
    {
        // ��֤����
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        rectTransform = GetComponent<RectTransform>();
        oriLen = rectTransform.rect.width;
    }

    public void ChangeLen(float cur, float total)
    {
        curLen = oriLen * cur / total;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, curLen);
    }

}
