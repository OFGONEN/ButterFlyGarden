using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public EventListenerDelegateResponse[] soundEventListeners;

    Dictionary<int, AudioSource> audioSources;
    private void Awake()
    {
        audioSources = new Dictionary<int, AudioSource>(soundEventListeners.Length);

        for (int i = 0; i < soundEventListeners.Length; i++)
        {
            var _audioComponent = gameObject.AddComponent<AudioSource>();
            _audioComponent.playOnAwake = false;
            _audioComponent.loop = false;

            var _audioEvent = soundEventListeners[i].gameEvent as SoundEvent;

            _audioComponent.clip = _audioEvent.audioClip;
            audioSources.Add(_audioEvent.GetInstanceID(), _audioComponent);
        }
    }
    private void OnEnable()
    {
        for (int i = 0; i < soundEventListeners.Length; i++)
        {
            soundEventListeners[i].OnEnable();
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < soundEventListeners.Length; i++)
        {
            soundEventListeners[i].OnDisable();
        }
    }
    private void Start()
    {
        foreach (var soundEventListener in soundEventListeners)
        {
            soundEventListener.response = (() => PlaySound(soundEventListener.gameEvent.GetInstanceID()));
        }
    }
    void PlaySound(int instanceId)
    {
        AudioSource _source;
        audioSources.TryGetValue(instanceId, out _source);

        if (_source != null && !_source.isPlaying)
        {
            _source.Play();
        }
    }
}
