using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySphere : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float damage = 10.0f;
    private void OnCollisionEnter(Collision player) {
        if(player.gameObject.GetComponent<PlayerController>()){
            Debug.Log("被球砸到了");
            player.gameObject.GetComponent<PlayerController>().attribute[0] -= damage;
            HealthBarController.instance.ChangeLen(player.gameObject.GetComponent<PlayerController>().attribute[0]
            ,player.gameObject.GetComponent<PlayerController>().maxAttribute[0]);
        }
        if(player.gameObject.layer == 6){
            Invoke("destorySelf",0.5f);
        }
    }

    private void destorySelf(){
        gameObject.SetActive(false);
    }
}
