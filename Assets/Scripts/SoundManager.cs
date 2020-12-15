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
    private void Start()
    {
        for (int i = 0; i < soundEventListeners.Length; i++)
        {
            soundEventListeners[i].response = () => PlaySound(soundEventListeners[i].gameEvent.GetInstanceID());
        }
    }
    void PlaySound(int instanceId)
    {
        AudioSource _source;
        audioSources.TryGetValue(instanceId, out _source);

        if (_source)
            _source.Play();
    }
}
