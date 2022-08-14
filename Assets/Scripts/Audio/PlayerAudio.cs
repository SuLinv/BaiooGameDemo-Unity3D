using System.Collections;
using System.Collections.Generic;
using UnityEngine;using System.IO;

public class PlayerAudio : MonoBehaviour
{
    AudioSource source;
    AudioManager audioManager;
    AudioClip clip;
    PlayerController controller;
    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<AudioSource>();
        controller = GetComponent<PlayerController>();

        // ע��ص�
        // controller.OnRun += PlayRun;
        // controller.OnWalk += PlayWalk;
        // controller.AfterHit += PlayHit;
        // controller.NotHit += PlayNotHit;
    }

    private void Start()
    {
        audioManager = AudioManager.Instance;
    }

    AudioClip GetFromPool(string clipName)
    {
        return audioManager.GetFromPool(clipName);
    }

    // �ܲ���Ч
    void PlayRun()
    {
        AudioClip clip = GetFromPool("RunSound");
        PlayClip(clip);
    }

    // ��·��Ч
    void PlayWalk()
    {
        AudioClip clip = GetFromPool("WalkSound");
        PlayClip(clip);
    }

    // ������Ч
    void PlayHit()
    {
        AudioClip clip = GetFromPool("PlayerHit");
        PlayClip(clip);
    }

    void PlayNotHit()
    {
        AudioClip clip = GetFromPool("PlayerNotHit");
        PlayClip(clip);
    }

    void PlayClip(AudioClip clip)
    {
        if (clip != null)
            source.clip = clip;
        else
            Debug.Log("�Ҳ���");

        if(!source.isPlaying)
            source.Play();
    }
}
