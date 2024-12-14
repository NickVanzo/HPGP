using UnityEngine;
using Unity.Entities;

public class SoundManager : MonoBehaviour
{
    public AudioSource jumpSound;
    public AudioSource fish;
    public AudioSource turtle;
    public AudioSource rabbit;

    public void PlayJumpSound()
    {

        jumpSound.Stop();
        fish.Stop();
        turtle.Stop();
        rabbit.Stop();
        int randInt = Random.Range(1, 5);

        if (randInt == 1)
        {
            jumpSound.Play();
        }
        if (randInt == 2)
        {
            fish.Play();
        }
        if (randInt == 3)
        {
            turtle.Play();
        }
        if (randInt == 4)
        {
            rabbit.Play();
        }


    }
}

public struct PlaySoundComponent : IComponentData
{
    public bool playJumpSound;
}
