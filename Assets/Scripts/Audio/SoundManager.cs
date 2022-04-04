using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource Source;

    private void Start()
    {
        Instance = this;
        Source.playOnAwake = false;
        Source.loop = false;
    }

    public void PlaySFX(string sfx)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Audio/{sfx}");
        Source.clip = clip;
        Source.Play();
    }
}