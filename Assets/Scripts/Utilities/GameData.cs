using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public class GameData
    {
        private int _score;
        private float _currentTime;
        public int Score 
        {
            get 
            { 
                return _score; 
            }
            set 
            {
                if (_score == value)
                    return;

                _score = value;
                OnScoreChanged?.Invoke(value);
            }
        }

        public TimeSpan CurrentTime 
        { 
            get 
            { 
                return TimeSpan.FromSeconds(_currentTime); 
            }
        }

        public void ResetTimeCounter()
        {
            _currentTime = 0;
        }

        public void AddTime(float seconds)
        {
            _currentTime += seconds;
            OnTimeChanged?.Invoke(CurrentTime);
        }

        public event Action<int> OnScoreChanged;
        public event Action<TimeSpan> OnTimeChanged;
    }
}
