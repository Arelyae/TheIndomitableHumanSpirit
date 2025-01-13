using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SubtitlesData", menuName = "ScriptableObjects/SubtitlesDataScriptableObject", order = 1)]
public class SubtitlesData : ScriptableObject
{
    public List<SubtitlesEvents> SubtitlesEventList = new List<SubtitlesEvents>();
}

[System.Serializable]
public class SubtitlesEvents
{
    [Header("Name Of The Event")]
    public string name;
    public List<LocalSubtitles> LocalSubtitlesList = new List<LocalSubtitles>();
}

[System.Serializable]
public class LocalSubtitles
{
    [Header("Name must contains the content of the subtitles")]
    public string name;
    [Header("The Audio file to play")]
    public AudioClip audioFileToPlay;
    [Header("Amount of time before next sentence.")]
    public float offset;
}