using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    GameObject m_audioSourcePrefab = null;

    // TODO: turn these into config SO's
    [SerializeField]
    int m_bgmPoolCount = 8;

    [SerializeField]
    int m_oneShotPoolCount = 64;

    // TODO: integrate with volume config SO

    ObjectPool<AudioSourceController> m_oneShotPool = null;
    ObjectPool<AudioSourceController> m_bgmPool = null;

    // TESTING
    [SerializeField]
    AudioClip m_testOneShotClip = null;

    // TESTING
    [SerializeField]
    AudioClip m_testBGMClip = null;

    [Button]
    void TestPlayOneShot()
    {
        PlayOneShot(new AudioClipInfo { AudioClip = m_testOneShotClip, Pitch = 1.0f, Volume = 0.5f });
    }

    [Button]
    void TestPlayBGM()
    {
        PlayBGM(new AudioClipInfo { AudioClip = m_testBGMClip, Pitch = 1.0f, Volume = 0.1f });
    }

    private void Start()
    {
        m_oneShotPool = CreateNewObjectPool(m_oneShotPoolCount, AudioSourceController.Type.kOneShot);
        m_bgmPool = CreateNewObjectPool(m_bgmPoolCount, AudioSourceController.Type.kLooping);
    }

    private ObjectPool<AudioSourceController> CreateNewObjectPool(int count, AudioSourceController.Type type)
    {
        List<AudioSourceController> tempList = new List<AudioSourceController>();
        for (int i = 0; i < count; i++)
        {
            var component = Instantiate(m_audioSourcePrefab, transform)
                .GetComponent<AudioSourceController>();
            component.Configure(type);
            component.gameObject.name = type.ToString() + component.gameObject.name;
            tempList.Add(component);
        }

        return new ObjectPool<AudioSourceController>(tempList);
    }

    public void PlayOneShot(AudioClipInfo audioClipInfo)
    {
        var (src, index) = m_oneShotPool.GetNextAvailable();
        src.Play(audioClipInfo, () => { m_oneShotPool.SetAvailable(index); });
    }

    public int PlayBGM(AudioClipInfo audioClipInfo)
    {
        var (src, index) = m_bgmPool.GetNextAvailable();
        src.Play(audioClipInfo);
        return index;
    }

    public void PauseBGM(int index)
    {
        m_bgmPool.Get(index).Pause();
    }
    public void ResumeBGM(int index)
    {
        m_bgmPool.Get(index).Resume();
    }

    public void StopBGM(int index)
    {
        m_bgmPool.Get(index).Stop();
        m_bgmPool.SetAvailable(index);
    }
}
