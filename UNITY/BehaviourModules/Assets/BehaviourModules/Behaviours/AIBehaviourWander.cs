using UnityEngine;
using BehaviourModules.Core;

namespace BehaviourModules.Behaviours
{
    /// <summary>
    /// Picks a random point within range and moves towards it.
    /// </summary>
    public class AIBehaviourWander : AIBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float maxRange;

        private Rigidbody m_rigidbody;
        private Vector3 m_startPosition;
        private Vector3 m_targetPosition;

        private void Awake()
        {
            m_rigidbody = GetComponentInParent<Rigidbody>();
            m_startPosition = transform.position;
        }

        public override void Reset()
        {
            var randomX = Random.Range(-maxRange, maxRange);
            var randomZ = Random.Range(-maxRange, maxRange);
            m_targetPosition = new Vector3(m_startPosition.x + randomX, m_startPosition.y, m_startPosition.z + randomZ);
        }

        public override bool Execute()
        {
            var moveDirection = (m_targetPosition - transform.position).normalized;
            m_rigidbody.MovePosition(m_rigidbody.position + moveDirection * (moveSpeed * Time.fixedDeltaTime));

            return Vector3.Distance(transform.position, m_targetPosition) < .1f;
        }
    }
}
