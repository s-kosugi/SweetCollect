using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    private AudioSource m_SEAudioSorce;
    Dictionary<string, AudioClip> m_SEDictionary = new Dictionary<string, AudioClip>();

    private AudioSource m_BGMAudioSorce;
    Dictionary<string, AudioClip> m_BGMDictionary = new Dictionary<string, AudioClip>();

    override protected void Awake()
    {
        // 重複したら削除
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        m_BGMAudioSorce = gameObject.AddComponent<AudioSource>();
        m_SEAudioSorce = gameObject.AddComponent<AudioSource>();
        m_SEAudioSorce.spatialBlend = 0.0f;

        AudioClip[] BGMAudioClip = Resources.LoadAll<AudioClip>("Sound/BGM");
        AudioClip[] SEAudioClip = Resources.LoadAll<AudioClip>("Sound/SE");

        for (int i = 0; i < BGMAudioClip.Length; i++)
        {
            m_BGMDictionary.Add(BGMAudioClip[i].name, BGMAudioClip[i]);
        }

        for (int i = 0; i < SEAudioClip.Length; i++)
        {
            m_SEDictionary.Add(SEAudioClip[i].name, SEAudioClip[i]);
        }
    }

    // SEの再生
    public void PlaySE(string name)
    {
        m_SEAudioSorce.PlayOneShot(m_SEDictionary[name]);
    }

    // SEの停止
    public void StopSE()
    {
        m_SEAudioSorce.Stop();
        m_SEAudioSorce.clip = null;
    }

    // BGM再生
    public void PlayBGM(string name)
    {
        // 同じBGMだったら処理しない
        if (m_BGMAudioSorce.clip == m_BGMDictionary[name]) return;

        m_BGMAudioSorce.clip = m_BGMDictionary[name];
        m_BGMAudioSorce.loop = true;
        m_BGMAudioSorce.Play();
    }

    // BGM停止
    public void StopBgm()
    {
        m_BGMAudioSorce.Stop();
        m_BGMAudioSorce.clip = null;
    }
    public void SetBGMSpeed(float speed)
    {

        m_BGMAudioSorce.pitch = speed;
    }
}
