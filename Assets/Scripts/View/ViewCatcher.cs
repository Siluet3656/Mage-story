using UnityEngine;

namespace View
{
    public class ViewCatcher : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Animator _animator;
        
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public Animator Animator => _animator;
    }
}
