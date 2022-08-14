using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 控制怪物头上血条 */
public class UIController : MonoBehaviour
{
    Transform cam;

    Image UIBar;

    float visiableTime = 2;
    float visiableTimer;

    private void Awake()
    {
        // 血条组件
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
        // 保持面对相机
        transform.forward = -cam.forward;
        // 隐藏血条
        if(visiableTimer > 0)
        {
            visiableTimer -= Time.deltaTime; ;
            if (visiableTimer <= 0) gameObject.SetActive(false);
        }
        
    }

    void UpdateHealthBar(float curHealth, float maxHealth)
    {
        // 设置可见
        gameObject.SetActive(true);
        UIBar.fillAmount = curHealth / maxHealth;
        visiableTimer = visiableTime;
    }
}
