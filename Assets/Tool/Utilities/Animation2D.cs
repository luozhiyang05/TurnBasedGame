using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tool.Utilities
{
    public class Animation2D : MonoBehaviour
    {
        public List<Sprite> sprites;
        public int oneSecondFrames;
        private Image _image;
        private float _time;
        private float _intervalTime;
        private int _index;

        void Awake()
        {
            _image = GetComponent<Image>();
        }

        void Start()
        {
            _intervalTime = 1f / oneSecondFrames;
            _time = 0;
            _index = 1;
            if (sprites.Count != 0)
            {
                _image.sprite = sprites[0];
            }
        }

        void Update()
        {
            _time += Time.deltaTime;
            if (_time >= _intervalTime)
            {
                _time = 0;
                _image.sprite = sprites[_index];
                _index = (_index + 1) % sprites.Count;
            }
        }

        public void SetSprites(Sprite sprite)
        {
            sprites.Add(sprite);
        }

        public void SetFrames(int frames)
        {
            oneSecondFrames = frames;
            _intervalTime = 1f / oneSecondFrames;
        }

        public void ClearSprites()
        {
            sprites.Clear();
        }
    }
}