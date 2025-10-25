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
public class NoteData
{
    [NonSerialized]
    public int id;  //NoteMove 在QArray的下标，用于击打音符后，消除数据的同时消除对应的NoteMove
    [CustomPropertyText("音符位置")]
    public ENotePos notePos;
    [CustomPropertyText("音符类型")]
    public ENoteType noteType;
    [CustomPropertyText("判定时间")]
    public float judgeTime;
    [CustomPropertyText("创建时间")]
    public float createTime;
    [CustomPropertyText("长按时间(非长按音符忽略)")]
    public float longPressTime;
    public float moveTime;
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

    public void AddSingleClickNote(float nowMusicTime, ENotePos notePos, ENoteType noteType)
    {
        if (notePos == ENotePos.Up)
        {
            upNoteDatas.Add(new NoteData { notePos = notePos, noteType = noteType, judgeTime = nowMusicTime, createTime = nowMusicTime - reachToBitPosTime, moveTime = reachToBitPosTime});
        }
        else
        {
            downNoteDatas.Add(new NoteData { notePos = notePos, noteType = noteType, judgeTime = nowMusicTime, createTime = nowMusicTime - reachToBitPosTime, moveTime = reachToBitPosTime });
        }
    }

    public void AddLongNote(float nowMusicTime, float longPressTime, ENotePos notePos)
    {
        if (notePos == ENotePos.Up)
        {
            upNoteDatas.Add(new NoteData { notePos = notePos, noteType = ENoteType.LongClick, judgeTime = nowMusicTime, createTime = nowMusicTime - reachToBitPosTime, moveTime = reachToBitPosTime });
        }
        else
        {
            downNoteDatas.Add(new NoteData { notePos = notePos, noteType = ENoteType.LongClick, judgeTime = nowMusicTime, createTime = nowMusicTime - reachToBitPosTime, moveTime = reachToBitPosTime });
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
            qArray.Add(value);
        }
        return qArray;
    }
    public void RemoveClickNote(ENotePos eNotePos)
    {
        switch (eNotePos)
        {
            case ENotePos.Up:
                upNoteDatas.RemoveAt(upNoteDatas.Count - 1);
                break;
            case ENotePos.Down:
                downNoteDatas.RemoveAt(downNoteDatas.Count - 1);
                break;
        }
    }

}
