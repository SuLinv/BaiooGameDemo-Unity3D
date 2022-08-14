using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerBuff : MonoBehaviour
{
    private Dictionary<int,BuffBase> buffDict = new Dictionary<int, BuffBase>();//存储player当前buff的字典
    public int buffNum;
    public Dictionary<int,BuffBase> buffInStatus = new Dictionary<int, BuffBase>();//存储player状态栏上存在的buff
    // Start is called before the first frame update
    void Start()
    {
        //初始buff
        foreach(var bf in BuffManager.Instance.buffBase){
            buffDict[bf.buffID] = bf;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //遍历每个Buff
        buffNum = buffDict.Count;
        for(int i=0;i<buffDict.Count;){
            var bf = buffDict.ElementAt(i);
            //每个Buff叠加了多少层Layer则重复触发多少次
            int buffLayerNum = bf.Value.buffLayer;
            for(int j=0;j<buffLayerNum;j++){
                switch(bf.Value.buffTimeType){
                    case BuffBase.BuffTimeType.durable:
                        StartCoroutine(BuffManager.buffEffectDurable(bf.Value));
                        RemoveBuff(bf.Value);
                        break;
                    case BuffBase.BuffTimeType.Infinite:
                        BuffManager.buffEffectInfinite(bf.Value);
                        if(j==buffLayerNum-1)   i++;
                        break;
                    case BuffBase.BuffTimeType.Instant:
                        BuffManager.buffEffectInstant(bf.Value);
                        RemoveBuff(bf.Value);
                        break;
                }
            }
        }                 
    }

    public void AddBuff(BuffBase buffbase){
        if(buffDict.ContainsKey(buffbase.buffID)){
            buffDict[buffbase.buffID].buffLayer++;
            buffInStatus[buffbase.buffID].buffLayer++;
        }else{
            buffDict[buffbase.buffID] = buffbase;
            buffInStatus[buffbase.buffID] = buffbase;
        }
    }

    public void AddBuffByID(int bfID){
        AddBuff(BuffManager.BuffDict[bfID]);
    }

    public void RemoveBuff(BuffBase buffbase){
        if(buffDict.ContainsKey(buffbase.buffID)){
            if(buffDict[buffbase.buffID].buffLayer <= 1){
                buffDict.Remove(buffbase.buffID);
            }else   buffDict[buffbase.buffID].buffLayer--;
        }
    }

    //从状态栏移除Buff
    public void RemoveBuffFromStatus(BuffBase buffbase){
        if(buffInStatus.ContainsKey(buffbase.buffID)){
            if(buffInStatus[buffbase.buffID].buffLayer <= 1){
                buffInStatus.Remove(buffbase.buffID);
            }else   buffInStatus[buffbase.buffID].buffLayer--;
        }        
    }
}
