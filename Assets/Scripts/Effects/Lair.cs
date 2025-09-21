using System;
using System.Collections;
using UnityEngine;

namespace Effects
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Animator))]
    public class Lair : MonoBehaviour
    {
        [SerializeField] private float _waitTime;
        [SerializeField] private ParticleSystem _particleSystem;
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_particleSystem != null)
            {
                _particleSystem.Play();
                _animator.SetBool("blow", true);
                StopAllCoroutines();
                StartCoroutine(WaitAnimEnd());
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            StopAllCoroutines();
            _animator.SetBool("blow", false);
        }

        private IEnumerator WaitAnimEnd()
        {
            yield return new WaitForSeconds(_waitTime);
            _animator.SetBool("blow", false);
        }
    }
}
