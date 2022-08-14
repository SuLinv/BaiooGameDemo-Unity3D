using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffBase
{
    public int buffID;
    public string buffName;
    public string buffDescription;
    public int buffLayer = 1;//Buff叠加层数
    public float buffActiveRate = 1.0f;//Buff触发几率，范围0-1
    public int buffCost; // Buff 价格

    public BuffType buffType;
    public enum BuffType{
        DebugLog,
        AttributeModify,
        AttackGiveBuff,
    }

    /* AttributeModify类型的参数 */
    public AttributeModifyVar attributeModifyVar;
    [System.Serializable]
    public struct AttributeModifyVar{
        public bool addValue;//增加还是减少数值
        public float value;
        public AttributeType attributeType;
        public enum AttributeType{
            HP = 0,
            MP = 1,
            Damage = 2,
        } 
    }

    /* AttackGiveBuff类型的参数 */
    public AttackGiveBuffVar attackGiveBuffVar;
    [System.Serializable]
    public struct AttackGiveBuffVar{
        public GiveBuffVar giveBuffVar;
        public enum GiveBuffVar{
            ForSelf = 0,
            ForAttacked = 1,
        }
        public int bfID;
    }

    public BuffTimeType buffTimeType;
    public enum BuffTimeType{
        Instant,
        Infinite,
        durable,
    }

    public DurableBuffVar durableBuffVar;
    [System.Serializable]
    public struct DurableBuffVar{
        public float frequency;//触发频率
        public float duration;//持续时间
    }

}
