using UnityEngine;

namespace Effects
{
    [RequireComponent(typeof(ParticleSystem))]
    public class BeamParticles : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _thickness = 0.12f; // толщина (по Y локальной системы)
        [SerializeField] private float _depth = 0.12f;     // глубина (по Z)
        [SerializeField] private bool _useLineWidth = true; // брать толщину из lineRenderer.widthMultiplier

        private ParticleSystem _particleSystem;
        private ParticleSystem.ShapeModule _shape;

        void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _shape = _particleSystem.shape;
            _shape.shapeType = ParticleSystemShapeType.Box;
            // make sure shape is centered (если нужно, можно настраивать)
            _shape.position = Vector3.zero;
            _shape.rotation = Vector3.zero;
        }

        void LateUpdate()
        {
            if (_lineRenderer == null || _lineRenderer.positionCount < 2) return;

            Vector3 start = _lineRenderer.GetPosition(0);
            Vector3 end = _lineRenderer.GetPosition(1);

            // центр и позиция эмиттера
            Vector3 mid = (start + end) * 0.5f;
            transform.position = mid;

            // направление и длина
            Vector3 dir = end - start;
            float length = dir.magnitude;
            if (length <= Mathf.Epsilon) return;

            // Выравниваем локальную ось X объекта по направлению луча
            transform.right = dir.normalized; // локальная X теперь вдоль луча

            // определяем толщину: можно привязать к lineRenderer ширине
            float usedThickness = _thickness;
            if (_useLineWidth && _lineRenderer != null)
            {
                // если lineRenderer использует widthCurve, можно взять среднюю ширину
                usedThickness = _lineRenderer.widthMultiplier;
                // умножитель можно подогнать, например:
                // usedThickness = lineRenderer.widthMultiplier * 0.5f;
            }

            // Устанавливаем размеры Box (в локальных осях: X = длина, Y = толщина, Z = глубина)
            _shape.scale = new Vector3(length, usedThickness, _depth);

            // Если хочешь, чтобы эмиттер не рендерился вне линии, можно включать/выключать emission
            // ps.emission.enabled = length > 0.1f;
        }
    }
}
