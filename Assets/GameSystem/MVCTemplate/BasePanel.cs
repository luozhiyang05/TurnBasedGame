using System;
using Tool.AudioMgr;
using UnityEngine;

namespace GameSystem.MVCTemplate
{
    public abstract class BasePanel : MonoBehaviour
    {
        public bool UseMaskPanel => MaskPanel();
        public bool UseClickMaskPanel => ClickMaskPanel();
        private string _openAudioPath;
        private string _bgmAudioPath;
        private string _closeAudioPath;
        protected bool useAudio = false;
        protected virtual void AutoInitUI(){}
        protected abstract void OnInit();
        public abstract void OnShow();
        public abstract void OnHide();
        public virtual void OnRelease() { }
        protected virtual void SetAudio(string openAudioPath, string closeAudioPath)
        {
            useAudio = true;
            _openAudioPath = openAudioPath;
            _closeAudioPath = closeAudioPath;
        }
        protected virtual void SetBgm(string bgmAudioPath)
        {
            useAudio = true;
            _bgmAudioPath = bgmAudioPath;
        }
        protected void PlayAudio(EAudioType audioType, bool isShow = true)
        {
            switch (audioType)
            {
                case EAudioType.Effect:
                    if (!string.IsNullOrEmpty(_openAudioPath)) AudioManager.GetInstance().PlayAudio(isShow ? _openAudioPath : _closeAudioPath);
                    break;
                case EAudioType.Bgm:
                    if (!string.IsNullOrEmpty(_bgmAudioPath)) AudioManager.GetInstance().PlayAudio(_bgmAudioPath, EAudioType.Bgm);
                    break;
                default:
                    throw new Exception("没有这个音频类型");
            }
        }
        protected  void CloseBgm()
        {
            if (!string.IsNullOrEmpty(_bgmAudioPath))
                AudioManager.GetInstance().StopAudio(_bgmAudioPath);
        }
   
        public virtual bool MaskPanel()
        {
            return false;
        }

        public virtual bool ClickMaskPanel()
        {
            return false;
        }

        public virtual void OnClickMaskPanel() {}
    }
}