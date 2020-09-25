using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Toggle TrackToggle;
    [SerializeField] private Toggle EffectsToggle;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        //Debug.Log(AudioManager.instance.isTrackEnabled);
        TrackToggle.isOn = AudioManager.instance.isTrackEnabled;
        EffectsToggle.isOn = AudioManager.instance.isEffectsEnabled;
        volumeSlider.value = AudioManager.instance.max;
    }

    private void Update()
    {
        
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void ToggleTrack()
    {
        AudioManager.instance.SetTrackActive(!AudioManager.instance.isTrackEnabled);
    }

    public void ToggleEffect()
    {
        AudioManager.instance.SetEffectsActive(!AudioManager.instance.isEffectsEnabled);
    }

    public void OnValueChanged()
    {
        AudioManager.instance.SetMax(volumeSlider.value);
    }
}
