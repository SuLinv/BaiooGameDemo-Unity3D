using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapPool : Singleton<CapPool>
{

    public GameObject capPrefab;

    Stack<GameObject> capStack = new Stack<GameObject>();

     public GameObject GetCap()
    {

        GameObject cap;
        if(capStack.Count == 0)
        {
            cap = Instantiate(capPrefab,gameObject.transform.position, Quaternion.identity );
        }
        else
            cap = capStack.Pop();
        cap.transform.position = transform.position;
        cap.SetActive(true);
        return cap;
    }

    public void Recycle(GameObject cap)
    {
        cap.SetActive(false);
        capStack.Push(cap);
    }
}
