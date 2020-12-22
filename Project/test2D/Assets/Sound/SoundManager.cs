using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    private AudioSource m_SEAudioSorce;
    Dictionary<string, AudioClip> m_SEDictionary = new Dictionary<string, AudioClip>();

    [SerializeField]private AudioSource m_BGMAudioSorce;
    Dictionary<string, AudioClip> m_BGMDictionary = new Dictionary<string, AudioClip>();

    [SerializeField] float defaultBGMVolume = 0.5f;
    [SerializeField] float defaultSEVolume = 0.5f;

    public float m_SEVolume { get; private set; } = 0.5f;
    public float m_BGMVolume { get; private set; } = 0.5f;

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

        // BGM音量の初期セット
        m_BGMVolume = m_BGMAudioSorce.volume = defaultBGMVolume;
        m_SEVolume = defaultSEVolume;
    }
    public void Update()
    {
    }

    // SEの再生
    public void PlaySE(string name)
    {
        m_SEAudioSorce.PlayOneShot(m_SEDictionary[name],m_SEVolume);
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
        m_BGMAudioSorce.volume = m_BGMVolume;
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

    public void SetBGMVolume(float volume)
    {
        if (volume > 1.0f)
        {
            volume = 0f;
        }
        if (volume < 0.0f)
        {
            volume = 0f;
        }
        m_BGMVolume = volume;
        // 現在再生しているBGMの音量を設定する
        m_BGMAudioSorce.volume = m_BGMVolume;
    }
    public void SetSEVolume(float volume)
    {
        if (volume > 1.0f)
        {
            volume = 0f;
        }
        if (volume < 0.0f)
        {
            volume = 0f;
        }
        m_SEVolume = volume;
    }
}
