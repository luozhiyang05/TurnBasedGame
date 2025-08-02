using System;
using System.Collections;
using System.Collections.Generic;
using Tool.CustomAttribute;
using Tool.Mono;
using UnityEngine;

public enum ENotePos
{
    Up,
    Down,
}
public enum ENoteType
{
    Click,
    LongClick,
}
[Serializable]
public struct NoteData
{
    [CustomPropertyText("音符位置")]
    public ENotePos notePos;
    [CustomPropertyText("音符类型")]
    public ENoteType noteType;
    [CustomPropertyText("判定时间")]
    public float judgeTime;
    [CustomPropertyText("创建时间")]
    public float createTime;
}

[CreateAssetMenu(fileName = "MusicSo", menuName = "音乐资源/MusicSo")]
public class MusicSo : ScriptableObject
{
    [CustomPropertyText("音乐资源")]
    public AudioClip audioClip;
    [CustomPropertyText("音符到达打击点时间")]
    public float reachToBitPosTime;
    public float musicTime => audioClip ? audioClip.length : 0;
    [CustomPropertyText("播放速度(音响音调)")]
    [Range(0f, 1f)]
    public float musicSpeed = 1;
    public List<NoteData> noteDatas;

    public void AddNote(float nowMusicTime,ENotePos notePos,ENoteType noteType)
    {
        noteDatas.Add(new NoteData() { judgeTime = nowMusicTime, notePos = notePos, noteType = noteType, createTime = nowMusicTime - reachToBitPosTime });
    }
    public void ClearNote()
    {
        noteDatas.Clear();
    }
}
