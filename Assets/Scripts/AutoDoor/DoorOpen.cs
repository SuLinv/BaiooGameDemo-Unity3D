using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public int enemyCount = 0;

    public GameObject door;
    public bool isopen;
    //�ƶ��ٶ�
    public int movespeed;
    //����ߴ�
    Vector3 length;
    //�ŵĸ߶�
    float height;
    //ԭʼλ��
    Vector3 ori;


    void Start()
    {
        ori = door.transform.position;
        isopen = false;
        length = GetComponent<MeshFilter>().mesh.bounds.size;
        height = door.transform.position.y - length.y * door.transform.lossyScale.y;
        GetComponent<BoxCollider>().isTrigger = true;
    }


    // Update is called once per frame
    //void Update()
    //{
    //    
    //        if (door.transform.position.y > height) {
    //            door.transform.Translate(Vector3.down * movespeed* Time.deltaTime);
    //        }
    //    }   
    //}
    private void OnTriggerStay(Collider target)
    {
        if (isopen)
        {
            if (target.CompareTag("Player") && enemyCount == 0)
            {
                if (door.transform.position.y > height)
                {
                    door.transform.Translate(Vector3.down * movespeed * Time.deltaTime);
                }
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isopen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isopen = false;
        }
    }

}
