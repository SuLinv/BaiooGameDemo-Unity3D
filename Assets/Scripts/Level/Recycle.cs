using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recycle : MonoBehaviour
{

    float liveTime;
    // Start is called before the first frame update
    void OnEnable()
    {
        liveTime = 2;
    }

    private void Update()
    {
        if((liveTime -= Time.deltaTime) <= 0)
        {
            CapPool.Instance.Recycle(gameObject);
        }
    }


}
