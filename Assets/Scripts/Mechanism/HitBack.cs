using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBack : Singleton<HitBack>
{
    public void active(GameObject target, Vector3 force)
    {
        Rigidbody rig = target.GetComponent<Rigidbody>();
        rig.AddForce(force, ForceMode.VelocityChange);
    }
}
