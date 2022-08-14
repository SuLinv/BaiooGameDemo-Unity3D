using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public Dictionary<string, AudioClip> audioPool;

    private void Awake()
    {
        base.Awake();
        audioPool = new Dictionary<string, AudioClip>();
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");
        foreach(AudioClip clip in clips)
        {
            audioPool.Add(clip.name, clip);
        }
    }

    // 从池中获取，不存在则返回null
    public AudioClip GetFromPool(string clipName)
    {
        if (audioPool.ContainsKey(clipName))
        {
            return audioPool[clipName];
        }

        return null;
    }
}
