using UnityEngine;
using Unity.Entities;

public class SoundManager : MonoBehaviour
{
    public AudioSource jumpSound;

    public void PlayJumpSound()
    {
        jumpSound.Play();
    }
}

public struct PlaySoundComponent : IComponentData
{
    public bool playJumpSound;
}
