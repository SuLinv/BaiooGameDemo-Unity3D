using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capacreator : MonoBehaviour
{
    float lastCreateTime;
    public float maxCoolDown = 1;
    CapPool pool;

    Queue<GameObject> capQueue;

    Transform playerTransform;

    float recycleY;
    // Start is called before the first frame update

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        recycleY = InputManager.instance.transform.position.y;
        capQueue = new Queue<GameObject>();
        pool = CapPool.Instance;
        playerTransform = InputManager.instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        ShootCap();
    }

    void ShootCap()
    {
        if ((lastCreateTime-=Time.deltaTime) <= 0)
        {
            GameObject cap = pool.GetCap();
            capQueue.Enqueue(cap);
            Vector3 direction = playerTransform.position - transform.position;
            direction.Normalize();
            direction.y = 0;
            cap.GetComponent<Rigidbody>().AddForce((direction) * 40, ForceMode.Impulse);

            lastCreateTime = maxCoolDown;
        }
    }
}
