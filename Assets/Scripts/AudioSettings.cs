using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour {
    [FMODUnity.EventRef]
    public string testSFX = "";
    public FMOD.Studio.EventInstance sfxtestSfxInstance;

    [FMODUnity.EventRef]
    public string testdialogue = "";
    public FMOD.Studio.EventInstance dialoguetestSfxInstance;

    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus SFX;
    FMOD.Studio.Bus Master;
    FMOD.Studio.Bus Dialogue;

    float MasterVolume = 1f;
    float MusicVolume = 1f;
    float SFXVolume = 1f;
    float DialogueVolume = 1f;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider dialogueSlider;

    string masterSliderString = "masterSlider";
    string musicSliderString = "musicSlider";
    string sfxSliderString = "sfxSlider";
    string dialogueSliderString = "dialogueSlider";

    void Awake()
    {
        MasterVolume = PlayerPrefs.GetFloat(masterSliderString, MasterVolume);
        MusicVolume = PlayerPrefs.GetFloat(musicSliderString, MusicVolume);
        SFXVolume = PlayerPrefs.GetFloat(sfxSliderString, SFXVolume);
        DialogueVolume = PlayerPrefs.GetFloat(dialogueSliderString, DialogueVolume);
        masterSlider.value = MasterVolume;
        musicSlider.value = MusicVolume;
        sfxSlider.value = SFXVolume;
        dialogueSlider.value = DialogueVolume;

        Music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        Dialogue = FMODUnity.RuntimeManager.GetBus("bus:/Master/Dialogue");
        sfxtestSfxInstance = FMODUnity.RuntimeManager.CreateInstance(testSFX);
        dialoguetestSfxInstance = FMODUnity.RuntimeManager.CreateInstance(testdialogue);
        Master.setVolume(MasterVolume);
        Music.setVolume(MusicVolume);
        SFX.setVolume(SFXVolume);
        Dialogue.setVolume(DialogueVolume);
    }

    public void MasterVolumeLevel(float newMasterVolume)
    {
        MasterVolume = newMasterVolume;
        PlayerPrefs.SetFloat(masterSliderString, MasterVolume);
        Master.setVolume(MasterVolume);
    }

    public void MusicVolumeLevel(float newMusicVolume)
    {
        MusicVolume = newMusicVolume;
        PlayerPrefs.SetFloat(musicSliderString, MusicVolume);
        Music.setVolume(MusicVolume);
    }

    public void SFXVolumeLevel(float newSFXVolume)
    {
        SFXVolume = newSFXVolume;
        PlayerPrefs.SetFloat(sfxSliderString, SFXVolume);
        FMOD.Studio.PLAYBACK_STATE PbState;
        sfxtestSfxInstance.getPlaybackState(out PbState);
        if (PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
            sfxtestSfxInstance.start();
        }
        SFX.setVolume(SFXVolume);
    }

    public void DialogueVolumeLevel(float newDialogueVolume)
    {
        DialogueVolume = newDialogueVolume;
        PlayerPrefs.SetFloat(dialogueSliderString, DialogueVolume);
        FMOD.Studio.PLAYBACK_STATE PbState;
        dialoguetestSfxInstance.getPlaybackState(out PbState);
        if (PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
            dialoguetestSfxInstance.start();
        }
        Dialogue.setVolume(DialogueVolume);
    }
}