using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct AudioClipInfo
{
    public AudioClip AudioClip;
    public float Volume;
    public float Pitch;
}

public class AudioSourceController : MonoBehaviour
{
    public enum Type
    {
        kInvalid = 0,
        kOneShot = 1,
        kLooping = 2,
    }

    AudioSource m_audioSource = null;
    UnityAction m_onPlayFinishCb = null;

    private void Awake()
    {
        TryGetComponent(out m_audioSource);
    }

    public void Configure(Type type)
    {
        switch (type)
        {
            case Type.kOneShot:
                m_audioSource.loop = false;
                break;
            case Type.kLooping:
                m_audioSource.loop = true;
                break;
            default:
                Debug.LogErrorFormat("Trying to configure {0} with type {1}", name, type);
                break;
        }
    }

    public void Play(AudioClipInfo clip)
    {
        m_audioSource.volume = clip.Volume;
        m_audioSource.pitch = clip.Pitch;
        m_audioSource.clip = clip.AudioClip;
        m_audioSource.Play();
    }

    public void Play(AudioClipInfo clip, UnityAction onPlayFinishCb)
    {

        Play(clip);
        m_onPlayFinishCb = onPlayFinishCb;
        if (!m_audioSource.loop)
            Invoke(kTriggerFinishFuncName, clip.AudioClip.length);
    }

    public void Pause()
    {
        m_audioSource.Pause();
    }

    public void Resume()
    {
        m_audioSource.UnPause();
    }

    public void Stop()
    {
        m_audioSource.Stop();
        TriggerFinishCb();
    }

    const string kTriggerFinishFuncName = "TriggerFinishCb";
    private void TriggerFinishCb()
    {
        m_onPlayFinishCb.Invoke();
        ResetComponent();
    }

    private void ResetComponent()
    {
        m_onPlayFinishCb = null;
        m_audioSource.clip = null;
    }
}
