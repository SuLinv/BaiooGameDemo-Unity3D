using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* ���ƹ���ͷ��Ѫ�� */
public class UIController : MonoBehaviour
{
    Transform cam;

    Image UIBar;

    float visiableTime = 2;
    float visiableTimer;

    private void Awake()
    {
        // Ѫ�����
        UIBar = transform.GetChild(1).GetComponent<Image>();
        transform.parent.GetComponent<EnemyController>().onEnemyHealthChange += UpdateHealthBar;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        cam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        // ����������
        transform.forward = -cam.forward;
        // ����Ѫ��
        if(visiableTimer > 0)
        {
            visiableTimer -= Time.deltaTime; ;
            if (visiableTimer <= 0) gameObject.SetActive(false);
        }
        
    }

    void UpdateHealthBar(float curHealth, float maxHealth)
    {
        // ���ÿɼ�
        gameObject.SetActive(true);
        UIBar.fillAmount = curHealth / maxHealth;
        visiableTimer = visiableTime;
    }
}
