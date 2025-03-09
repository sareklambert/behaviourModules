using System.Collections.Generic;
using UnityEngine;

namespace BehaviourModules.Core
{
    /// <summary>
    /// Represents a state change condition for AI agents, holds a list of triggers and the state to transition to.
    /// </summary>
    [System.Serializable]
    public class AICondition
    {
        [SerializeField] private List<AITrigger> triggers = new List<AITrigger>();
        [SerializeField] private int nextState = -1;

        public List<AITrigger> Triggers => triggers;
        public int NextState => nextState;
    }
}
