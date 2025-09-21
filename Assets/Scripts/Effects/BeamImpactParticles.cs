using UnityEngine;

namespace Effects
{
    public class BeamImpactParticles : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private ParticleSystem _impactParticles;

        void LateUpdate()
        {
            if (_lineRenderer == null || _impactParticles == null || _lineRenderer.positionCount < 2) return;

            // конец луча
            Vector3 end = _lineRenderer.GetPosition(_lineRenderer.positionCount - 1);

            // ставим систему частиц в эту точку
            _impactParticles.transform.position = end;

            // включаем/отключаем эмиссию в зависимости от длины
            var emission = _impactParticles.emission;
            emission.enabled = (_lineRenderer.GetPosition(0) - end).sqrMagnitude > 0.01f;
        }
    }
}