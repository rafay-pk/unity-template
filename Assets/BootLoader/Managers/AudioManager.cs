using BootLoader.Base;
using UnityEngine;

namespace BootLoader.Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        private AudioSource audioSource;
        private float volume;
        private void Awake()
        {
            audioSource ??= GetComponent<AudioSource>();
        }

        public void PlayClip(AudioClip clip) => audioSource.PlayOneShot(clip, volume);
    }
}
