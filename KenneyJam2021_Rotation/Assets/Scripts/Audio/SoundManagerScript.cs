using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Thought I'd do a quick touching up of the audio library you provided. Feel free to use this in your personal projects.
/// </summary>
public class SoundManagerScript : MonoBehaviour
{
    [Header("Sound effects information")] 
    [SerializeField] private string sfx_folder;
    [SerializeField] private List<string> sfx_filenames;
    private static Dictionary<string, AudioClip> sfx; // Dictionary objects unfortunately cannot be made viewable in the Unity inspector by default, but they are very handy

    [Header("Background music information")]
    [SerializeField] private string bgm_folder;
    [SerializeField] private List<string> bgm_filenames;
    private static Dictionary<string, AudioClip> bgm;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float sfx_volume;
    [Range(0f, 1f)] public float bgm_volume;
    private static float global_sfx_volume;
    private static float global_bgm_volume;

    static bool exists = false;
    static AudioSource sfx_audioSrc;
    static AudioSource bgm_audioSrc;

    void Awake()
    {
        // Check to see if we already have an existing SoundManagerScript
        if (!exists)
        {
            exists = true;
            InitializeAudioSources();
            InitializeVolumes();
            InitializeSFXLibrary();
            InitializeBGMLibrary();
            DontDestroyOnLoad(this); // Only create one of these per game. Try to load it as early as possible, probably on your title screen or in a preload scene.
        }
        else
        {
            // We don't want multiple copies of this object to exist.
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Make sure to place at least 2 Audio Sources on this GameObject in the inspector.
    /// The SFX source should be the one on top and the BGM one will be below it.
    /// </summary>
    private void InitializeAudioSources()
    {
        AudioSource[] sources = GetComponents<AudioSource>();

        sfx_audioSrc = sources[0];
        bgm_audioSrc = sources[1];
    }

    /// <summary>
    /// Sets the global playback volumes for our two libraries according to the slider values we set in the inspector.
    /// </summary>
    private void InitializeVolumes()
    {
        global_sfx_volume = sfx_volume;
        global_bgm_volume = bgm_volume;
    }

    /// <summary>
    /// Place all SFX files in Resources/[sfx_folder]/...
    /// </summary>
    private void InitializeSFXLibrary()
    {
        sfx = new Dictionary<string, AudioClip>();

        foreach (string item in sfx_filenames)
        {
            AudioClip new_clip = Resources.Load<AudioClip>(sfx_folder + "/" + item);

            if (new_clip != null)
            {
                sfx.Add(item, new_clip);
            }
        }
    }

    /// <summary>
    /// Place all BGM files in Resources/[bgm_folder]/...
    /// </summary>
    private void InitializeBGMLibrary()
    {
        bgm = new Dictionary<string, AudioClip>();

        foreach (string item in bgm_filenames)
        {
            AudioClip new_clip = Resources.Load<AudioClip>(bgm_folder + "/" + item);

            if (new_clip != null)
            {
                bgm.Add(item, new_clip);
            }
        }
    }

    /// <summary>
    /// Attempts to play a sound from the loaded SFX library.
    /// </summary>
    /// <param name="clip">Name of the clip we're going to play. If using files from a subfolder, include the full directory (e.g. Player/Jump, or Enemy/Shoot).</param>
    /// <param name="volume">Value between 0 and 1.</param>
    public static void PlaySound(string clip, float volume)
    {
        sfx.TryGetValue(clip, out AudioClip clip_to_play);

        if (clip_to_play != null)
        {
            sfx_audioSrc.PlayOneShot(clip_to_play, volume);
        }
        else
        {
            Debug.LogError("SFX file [" + clip + "] not found.");
        }
    }

    /// <summary>
    /// Uses global SFX volume for playback.
    /// </summary>
    public static void PlaySound(string clip)
    {
        PlaySound(clip, global_sfx_volume);
    }

    /// <summary>
    /// Attempts to play a song from the BGM library.
    /// If the song is not found, an error is thrown.
    /// </summary>
    /// <param name="clip">Name of the clip we're going to play. If using files from a subfolder, include the full directory (e.g. Event/Intro, or Levels/Forest).</param>
    /// <param name="volume">Value between 0 and 1.</param>
    public static void PlayMusic(string clip, float volume)
    {
        bgm.TryGetValue(clip, out AudioClip clip_to_play);

        if (clip_to_play != null)
        {
            bgm_audioSrc.Stop();
            bgm_audioSrc.volume = volume;
            bgm_audioSrc.clip = clip_to_play;
            bgm_audioSrc.Play();
        }
        else
        {
            Debug.LogError("BGM file [" + clip + "] not found.");
        }
    }

    /// <summary>
    /// Uses global BGM volume for playback.
    /// </summary>
    public static void PlayMusic(string clip)
    {
        PlayMusic(clip, global_bgm_volume);
    }

    /// <summary>
    /// Stops the currently playing song.
    /// </summary>
    public static void StopMusic()
    {
        if (bgm_audioSrc != null)
        {
            bgm_audioSrc.Stop();
        }
    }
}
