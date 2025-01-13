using System.Collections;
using TMPro;
using UnityEngine;

public class SubtitlesManager : MonoBehaviour
{
    public SubtitlesData subtitlesData;
    public AudioSource AudioSource;
    public TextMeshProUGUI subtitlesText;

    public void PlaySubtitles(int eventId)
    {
       StartCoroutine(SubtitlesEvent(eventId));
    }

    private IEnumerator SubtitlesEvent(int eventId)
    {
        SubtitlesEvents events = subtitlesData.SubtitlesEventList[eventId];
        LocalSubtitles subtitles;

        for (int i = 0; i < subtitlesData.SubtitlesEventList[eventId].LocalSubtitlesList.Count; i++)
        {
            Debug.Log(i);
            subtitles = events.LocalSubtitlesList[i];
            AudioSource.clip = subtitles.audioFileToPlay;
            subtitlesText.text = subtitles.name;
            AudioSource.Play();
            yield return new WaitForSeconds(subtitles.audioFileToPlay.length + subtitles.offset);
        }
        subtitlesText.text = "";
    }
}