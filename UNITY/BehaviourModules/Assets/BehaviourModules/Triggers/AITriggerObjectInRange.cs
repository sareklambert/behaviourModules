using UnityEngine;
using BehaviourModules.Core;

namespace BehaviourModules.Triggers
{
    /// <summary>
    /// Triggers if the target transform is within the range.
    /// </summary>
    public class AITriggerObjectInRange : AITrigger
    {
        [SerializeField] private Transform target;
        [SerializeField] private float range;
        public float Range => range;
        
        public override bool Check()
        {
            return Vector3.SqrMagnitude(transform.position - target.position) <= range * range;
        }
    }
}
