using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rig = collision.gameObject.GetComponent<Rigidbody>();
        if(rig != null)
        {
            rig.AddForce(rig.transform.forward * 20 + Vector3.up * 20, ForceMode.Impulse);
        }
    }
}
