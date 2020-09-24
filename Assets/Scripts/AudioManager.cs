using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;
    [SerializeField] private AudioSource audioSourceTitle;
    [SerializeField] private AudioSource audioSourceInGame;
    private bool isFirstSoundTrack = true;
    public float max = 0.5f;
    public bool isTrackEnabled = true;
    public bool isEffectsEnabled = true;

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            audioSourceTitle.volume = max;
            audioSourceInGame.volume = 0;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadScene(2);
        }
    }
    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "TitleScreen" || scene.name == "CreditsScreen" || scene.name == "Menu")//title, credits or menu
        {
            if (!isFirstSoundTrack)
            {
                StopAllCoroutines();
                StartCoroutine(fadeIn(audioSourceTitle));
                StartCoroutine(fadeOut(audioSourceInGame));
                isFirstSoundTrack = true;
            }
        }
        else //in game
        {
            if (isFirstSoundTrack)
            {
                StopAllCoroutines();
                StartCoroutine(fadeIn(audioSourceInGame));
                StartCoroutine(fadeOut(audioSourceTitle));
                isFirstSoundTrack = false;
            }
        }
    }

    private IEnumerator fadeOut(AudioSource audioSource)
    {
        float _t = 0;
        float _duration = 3;
        while (true)
        {
            _t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(max, 0, _t / _duration);
            if (_t >= _duration) break;
            yield return null;
        }
        audioSource.Stop();
    }

    private IEnumerator fadeIn(AudioSource audioSource)
    {
        audioSource.Play();
        float _t = 0;
        float _duration = 3;
        while (true)
        {
            _t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, max, _t / _duration);
            if (_t >= _duration) break;
            yield return null;
        }
    }

    public void SetMax(float value)
    {
        float originalPercentageInGame = audioSourceInGame.volume / max;
        float originalPercentageTitle = audioSourceTitle.volume / max;
        max = value;
        audioSourceInGame.volume = Mathf.Lerp(0, max, originalPercentageInGame);
        audioSourceTitle.volume = Mathf.Lerp(0, max, originalPercentageTitle);
    }

    public void PlaySound(AudioClip audioClip)
    {
        if (!isEffectsEnabled) return;
        AudioSource source = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        source.clip = audioClip;
        source.Play();
        StartCoroutine(DeleteSource(source));
    }

    private IEnumerator DeleteSource(AudioSource audioSource)
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        Destroy(audioSource);
    }

    public void SetTrackActive(bool state)
    {
        isTrackEnabled = state;
        if (state)
        {
            if (isFirstSoundTrack) audioSourceTitle.volume = max;
            else audioSourceInGame.volume = max;
        }
        else
        {
            if (isFirstSoundTrack) audioSourceTitle.volume = 0;
            else audioSourceInGame.volume = 0;
        }
    }

    public void SetEffectsActive(bool state)
    {
        isEffectsEnabled = state;
    }
}
