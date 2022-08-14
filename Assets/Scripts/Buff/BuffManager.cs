using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour {
    public PlayerController player;
    public PlayerBuff playerBuff;
    private static BuffManager _instance;
    public static BuffManager Instance
    {
        get { return _instance; }
    }

    void Awake(){
        _instance = this;
        for(int i = 0;i < buffBase.Count;i++){
            buffBase[i].buffID = i;
            buffDict[i] = buffBase[i];
        }
    }

    // ���е�buff
    public List<BuffBase> buffBase = new List<BuffBase>();
    private Dictionary<int,BuffBase> buffDict = new Dictionary<int, BuffBase>();
    public static Dictionary<int,BuffBase> BuffDict{
        get {
            return BuffDict;
        }
    }

    void Start(){
        Debug.Log(InputManager.instance.player);
        player = InputManager.instance.player.GetComponent<PlayerController>();
        playerBuff = InputManager.instance.player.GetComponent<PlayerBuff>();
    }

    public static IEnumerator buffEffectDurable(BuffBase buffBase){
        float duraTime = buffBase.durableBuffVar.frequency;
        while(duraTime<=buffBase.durableBuffVar.duration){
            switch(buffBase.buffType){
                case BuffBase.BuffType.AttributeModify:
                    if(Instance.buffActiveRateCal(buffBase.buffActiveRate) == 0)    Instance.buffEffectAttribute(buffBase);
                    break;
            }
            duraTime += buffBase.durableBuffVar.frequency;
            yield return new WaitForSeconds(buffBase.durableBuffVar.frequency);
        }
        Instance.playerBuff.RemoveBuffFromStatus(buffBase);
    }

    public static void buffEffectInfinite(BuffBase buffBase){
        switch(buffBase.buffType){
            case BuffBase.BuffType.DebugLog:
                if(Instance.buffActiveRateCal(buffBase.buffActiveRate) == 0)    Debug.Log(buffBase.buffName + Time.time);
                break;
            case BuffBase.BuffType.AttackGiveBuff:
                // int buffLayerNum = buffBase.buffLayer;
                Instance.buffAttackGive(buffBase);
                break;
        }
    }

    private void buffAttackGive(BuffBase buffBase){
        if((int)buffBase.attackGiveBuffVar.giveBuffVar == 0){
            int giveBuffID = buffBase.attackGiveBuffVar.bfID;
            BuffBase giveBuff;
            if(buffDict.ContainsKey(giveBuffID)){
                giveBuff = buffDict[giveBuffID];
                player.AfterHit += (() => {if(Instance.buffActiveRateCal(buffBase.buffActiveRate) == 0) buffEffectInstant(giveBuff);});
            }else{
                Debug.Log("没有此BUFF");
            }
        }
    }
    public static void buffEffectInstant(BuffBase buffBase){
        switch(buffBase.buffType){
            case BuffBase.BuffType.DebugLog:
                if(Instance.buffActiveRateCal(buffBase.buffActiveRate) == 0)    Debug.Log(buffBase.buffName + Time.time);
                Instance.playerBuff.RemoveBuffFromStatus(buffBase);
                break;
            case BuffBase.BuffType.AttributeModify:
                if(Instance.buffActiveRateCal(buffBase.buffActiveRate) == 0)    Instance.buffEffectAttribute(buffBase);
                Instance.playerBuff.RemoveBuffFromStatus(buffBase);
                break;
            case BuffBase.BuffType.AttackGiveBuff:
                // int buffLayerNum = buffBase.buffLayer;
                Instance.buffAttackGive(buffBase);
                // Debug.Log("buff");
                break;
        }
    }

    private void buffEffectAttribute(BuffBase buffBase){
        int attributeType = (int)buffBase.attributeModifyVar.attributeType;
        float preAttr = player.attribute[attributeType];
        float maxAttr = player.maxAttribute[attributeType];
        float value = buffBase.attributeModifyVar.value;
        if(buffBase.attributeModifyVar.addValue)
            player.attribute[attributeType] = Mathf.Min(maxAttr, preAttr + value);
        else
            player.attribute[attributeType] = Mathf.Max(0, preAttr - value);
        HealthBarController.instance.ChangeLen(player.attribute[0],player.maxAttribute[0]);
    }

    //触发几率
    private int buffActiveRateCal(float rate){
        int num = (int)Mathf.Round(1 / rate);
        int num1 = Random.Range(0,num);
        return num1;
    }
}
