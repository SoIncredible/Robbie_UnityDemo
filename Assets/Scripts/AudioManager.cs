using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{

    static AudioManager current;


    [Header("环境声音")]
    public AudioClip AmbientClip;
    public AudioClip MusicClip;

    [Header("Robbie的音效")]
    public AudioClip[] stepClips;
    public AudioClip[] crounchStepClips;
    public AudioClip jumpClips;
    public AudioClip jumpVoice;
    public AudioClip deathClips;

    // 人物死亡的声音
    public AudioClip deathVoice;

    public AudioSource AmbientSource;
    public AudioSource VoiceSource;
    public AudioSource MusicSource;
    public AudioSource PlayerSource;
    public AudioSource FxSource;


    [Header("FX特效")]
    public AudioClip deathFX;

    [Header("宝珠声音")]
    public AudioClip Orb;
    public AudioClip OrbVoice;

    private void Awake()
    {
        if(current != null)
        {
            Destroy(gameObject);
            return;
        }
        current = this;

        DontDestroyOnLoad(gameObject);
        AmbientSource = gameObject.AddComponent<AudioSource>();
        VoiceSource = gameObject.AddComponent<AudioSource>();
        MusicSource = gameObject.AddComponent<AudioSource>();
        PlayerSource = gameObject.AddComponent<AudioSource>();
        FxSource = gameObject.AddComponent<AudioSource>();

        StartLevelAudio();
    }

    void StartLevelAudio()
    {
        current.AmbientSource.clip = current.AmbientClip;
        current.MusicSource.clip = current.MusicClip;
        current.AmbientSource.loop = true;
        current.MusicSource.loop = true;
        current.AmbientSource.Play();
        current.MusicSource.Play();

    }

    public static void PlayerFootstepAudio()
    {
        int index = Random.Range(0, current.stepClips.Length);
        current.PlayerSource.clip = current.stepClips[index];
        current.PlayerSource.Play();
    }

    public static void PlayerCrouchstepAudio()
    {
        int index = Random.Range(0, current.crounchStepClips.Length);
        current.PlayerSource.clip = current.crounchStepClips[index];
        current.PlayerSource.Play();
    }



    public static void PlayerJumpAudio()
    {
        current.PlayerSource.clip = current.jumpClips;
        current.VoiceSource.clip = current.jumpVoice;
        current.PlayerSource.Play();
        current.VoiceSource.Play();
    }



    public static void PlayerDeathAudio()
    {
        current.PlayerSource.clip = current.deathClips;
        current.VoiceSource.clip = current.deathVoice;
        current.FxSource.clip = current.deathVoice;

        current.PlayerSource.Play();
        current.VoiceSource.Play();
        current.FxSource.Play();
    }


    public static void PlayerOrbAudio()
    {
        current.PlayerSource.clip = current.Orb;
        current.VoiceSource.clip = current.OrbVoice;
        current.PlayerSource.Play();
        current.VoiceSource.Play();
    }
}
