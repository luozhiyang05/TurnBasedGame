using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameSystem.MusicNoteSystem.Main;
using Tool.CustomAttribute;
using Tool.Mono;
using Tool.Utilities;
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
    DoubleClick,
}
[Serializable]
public struct NoteData
{
    [NonSerialized]
    public int id;
    [NonSerialized]
    public int noteMoveId;
    [CustomPropertyText("音符位置")]
    public ENotePos notePos;
    [CustomPropertyText("音符类型")]
    public ENoteType noteType;
    [CustomPropertyText("判定时间")]
    public float judgeTime;
    [CustomPropertyText("创建时间")]
    public float createTime;
    public void SetNoteMoveId(int noteMoveId)
    {
        this.noteMoveId = noteMoveId;
    }
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
    public List<NoteData> upNoteDatas, downNoteDatas;

    public void AddNote(float nowMusicTime, ENotePos notePos, ENoteType noteType)
    {
        if (notePos == ENotePos.Up)
        {
            upNoteDatas.Add(new NoteData { notePos = notePos, noteType = noteType, judgeTime = nowMusicTime + reachToBitPosTime, createTime = nowMusicTime });
        }
        else
        {
            downNoteDatas.Add(new NoteData { notePos = notePos, noteType = noteType, judgeTime = nowMusicTime + reachToBitPosTime, createTime = nowMusicTime });
        }
    }
    public void ClearNote()
    {
        upNoteDatas.Clear();
        downNoteDatas.Clear();
    }
    public QArray<NoteData> GetUpNoteQArray()
    {
        QArray<NoteData> qArray = new QArray<NoteData>();
        for (int i = 0; i < upNoteDatas.Count; i++)
        {
            var value = upNoteDatas[i];
            value.id = i;
            qArray.Add(value);
        }
        return qArray;
    }
    
    public QArray<NoteData> GetDownNoteQArray()
    {
        QArray<NoteData> qArray = new QArray<NoteData>();
        for (int i = 0; i < downNoteDatas.Count; i++)
        {
            var value = downNoteDatas[i];
            value.id = i;
            qArray.Add(value);
        }
        return qArray;
    }

}
