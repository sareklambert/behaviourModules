using UnityEngine;
using BehaviourModules.Behaviours;
using BehaviourModules.Triggers;

namespace BehaviourModules.Implementation.Enemy
{
    /// <summary>
    /// Visualizes the AI agent's behaviour for testing using a line renderer.
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class VisualizeEnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private AITriggerObjectInRange objectInRangeTrigger;
        [SerializeField] private AIBehaviourMoveTowards behaviourMoveTowards;
        
        private const int Segments = 50;
        private LineRenderer m_lineRenderer;
        private Transform m_target;
        private bool m_visualizeDetectionRange = true;

        private void Start()
        {
            m_lineRenderer = GetComponent<LineRenderer>();
            m_lineRenderer.useWorldSpace = true;
            SetVisualizationMode(true);
        }

        private void FixedUpdate()
        {
            if (m_visualizeDetectionRange)
            {
                DrawCircle();
            }
            else if (m_target)
            {
                DrawLineToTarget();
            }
            
            if (objectInRangeTrigger.Check())
            {
                SetVisualizationMode(false, behaviourMoveTowards.TargetTransform);
            }
        }

        private void SetVisualizationMode(bool visualizeDetectionRange, Transform target = null)
        {
            m_visualizeDetectionRange = visualizeDetectionRange;
            m_target = target;
            m_lineRenderer.positionCount = visualizeDetectionRange ? Segments + 1 : 2;
            m_lineRenderer.loop = visualizeDetectionRange;
        }

        private void DrawCircle()
        {
            const float angleStep = 360f / Segments;
            var points = new Vector3[Segments + 1];

            for (var i = 0; i <= Segments; i++)
            {
                var angle = Mathf.Deg2Rad * i * angleStep;
                var x = Mathf.Cos(angle) * objectInRangeTrigger.Range;
                var z = Mathf.Sin(angle) * objectInRangeTrigger.Range;
                points[i] = new Vector3(x, 0, z) + transform.position;
            }

            m_lineRenderer.SetPositions(points);
        }

        private void DrawLineToTarget()
        {
            var points = new Vector3[2];
            points[0] = transform.position;
            points[1] = m_target.position;
            m_lineRenderer.SetPositions(points);
        }
    }
}
