using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorClose : MonoBehaviour
{
    public GameObject door;
    //用来关闭前面的触发器
    public GameObject sensor;
    Vector3 ori;
    // Start is called before the first frame update
    void Start()
    {
        ori = door.transform.position;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.transform.position = ori;
            GetComponent<BoxCollider>().isTrigger = false;
            sensor.GetComponent<BoxCollider>().isTrigger = false;
        }
    }
}

