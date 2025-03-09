using UnityEngine;

namespace BehaviourModules.Core
{
    /// <summary>
    /// Wrapper class for AI behaviours and their random weights.
    /// </summary>
    [System.Serializable]
    public class AIBehaviourWeight
    {
        [SerializeField] private AIBehaviour behaviour;
        [SerializeField][Range(1, 100)] private int weight = 1;

        public int Weight => weight;
        public AIBehaviour Behaviour => behaviour;
    }
}
