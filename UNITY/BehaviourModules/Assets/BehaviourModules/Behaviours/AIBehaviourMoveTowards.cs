using UnityEngine;
using BehaviourModules.Core;

namespace BehaviourModules.Behaviours
{
    /// <summary>
    /// Moves the agent's rigidbody towards the set target.
    /// </summary>
    public class AIBehaviourMoveTowards : AIBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Transform targetTransform;

        public Transform TargetTransform => targetTransform;

        private Rigidbody m_rigidbody;
        
        private void Awake()
        {
            m_rigidbody = GetComponentInParent<Rigidbody>();
        }
        
        public override void Reset()
        {
            
        }
        
        public override bool Execute()
        {
            Vector3 moveDirection = (targetTransform.position - transform.position).normalized;
            m_rigidbody.MovePosition(m_rigidbody.position + moveDirection * (moveSpeed * Time.fixedDeltaTime));
            
            return Vector3.SqrMagnitude(transform.position - targetTransform.position) <= .1f;
        }
    }
}
