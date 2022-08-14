using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BuffManager))]
public class BuffEditor : Editor
{
    
    BuffManager t;
    SerializedObject GetTarget;
    SerializedProperty ThisList;
    int ListSize;

    void OnEnable() {
        t = (BuffManager)target;
        GetTarget = new SerializedObject(t);
        ThisList = GetTarget.FindProperty("buffBase");
    }

    public override void OnInspectorGUI(){
        GetTarget.Update();
        ListSize = ThisList.arraySize;
        EditorGUILayout.PropertyField(GetTarget.FindProperty("player"));
        EditorGUILayout.PropertyField(GetTarget.FindProperty("playerBuff"));
        EditorGUILayout.LabelField("全部BUFF数量:" + ListSize);
        // ListSize = EditorGUILayout.IntField ("全部BUFF数量", ListSize);

        for(int i = 0; i < ThisList.arraySize; i++){
            SerializedProperty buff = ThisList.GetArrayElementAtIndex(i);
            SerializedProperty buffID = buff.FindPropertyRelative("buffID");
            SerializedProperty buffName = buff.FindPropertyRelative("buffName");
            SerializedProperty buffDescription = buff.FindPropertyRelative("buffDescription");
            SerializedProperty buffLayer = buff.FindPropertyRelative("buffLayer");
            SerializedProperty buffActiveRate = buff.FindPropertyRelative("buffActiveRate");
            SerializedProperty buffCost = buff.FindPropertyRelative("buffCost");
            SerializedProperty buffType = buff.FindPropertyRelative("buffType");
            SerializedProperty attributeModifyVar = buff.FindPropertyRelative("attributeModifyVar");
            SerializedProperty attackGiveBuffVar = buff.FindPropertyRelative("attackGiveBuffVar");
            SerializedProperty buffTimeType = buff.FindPropertyRelative("buffTimeType");
            SerializedProperty durableBuffVar = buff.FindPropertyRelative("durableBuffVar");

            if(EditorGUILayout.PropertyField(buff, new GUIContent("BUFF " + i + ":" + buffName.stringValue), false)){
                
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(buffID);
                EditorGUILayout.PropertyField(buffName);
                EditorGUILayout.PropertyField(buffDescription);
                EditorGUILayout.PropertyField(buffLayer);
                EditorGUILayout.Slider(buffActiveRate,0,1.0f);
                EditorGUILayout.PropertyField(buffCost);
                EditorGUILayout.PropertyField(buffType);
                if(buffType.enumValueIndex == (int)BuffBase.BuffType.AttributeModify)
                    EditorGUILayout.PropertyField(attributeModifyVar);
                else if(buffType.enumValueIndex == (int)BuffBase.BuffType.AttackGiveBuff)
                    EditorGUILayout.PropertyField(attackGiveBuffVar);

                EditorGUILayout.PropertyField(buffTimeType);

                if(buffTimeType.enumValueIndex == (int)BuffBase.BuffTimeType.durable)
                    EditorGUILayout.PropertyField(durableBuffVar);

                EditorGUI.indentLevel--;
                EditorGUILayout.LabelField("删除这个BUFF");
                if(GUILayout.Button("点击删除")){
                    ThisList.DeleteArrayElementAtIndex(i);
                }
            }
            
        }
        EditorGUILayout.Space ();
        EditorGUILayout.Space ();
        EditorGUILayout.LabelField("添加一个新BUFF:");
        if(GUILayout.Button("添加")){
            t.buffBase.Add(new BuffBase());
        }
        GetTarget.ApplyModifiedProperties();
        EditorGUILayout.Space ();
        EditorGUILayout.Space ();
        // base.DrawDefaultInspector();
    }
}
