using System;
using UnityEngine;

namespace Animations
{
    public class AnimationEventCatcher : MonoBehaviour
    {
        public event Action OnAnimationEnd;
        public void OnOnAnimationEnd()
        {
            OnAnimationEnd?.Invoke();
        }
    }
}
