using System;
using System.Collections;
using System.Collections.Generic;
using Tool.CustomAttribute;
using UnityEngine;

[Serializable]
public class NoteData
{
    public float judgeTime;
    public float perfectTime;
    public float greatTime;
}

[CreateAssetMenu(fileName = "MusicSo", menuName = "音乐资源/MusicSo")]
public class MusicSo : ScriptableObject
{
    [CustomPropertyText("音乐资源")]
    public AudioClip audioClip;
    public float musicTime => audioClip ? audioClip.length : 0;
    [CustomPropertyText("Perfect时间")]
    public float perfectTime;
    [CustomPropertyText("Great时间")]
    public float greatTime;
    public List<NoteData> noteDatas;

    public void AddNote(float nowMusicTime)
    {
        noteDatas.Add(new NoteData() { judgeTime = nowMusicTime, perfectTime = nowMusicTime + 0.05f, greatTime = nowMusicTime + 0.1f });
    }
    public void ClearNote()
    {
        noteDatas.Clear();
    }
}
