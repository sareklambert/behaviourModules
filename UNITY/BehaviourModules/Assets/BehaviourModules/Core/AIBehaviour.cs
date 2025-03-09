using System;
using UnityEngine;

namespace BehaviourModules.Core
{
    /// <summary>
    /// Represents a behaviour for AI agents, derived classes implement specific behaviours for executing and resetting.
    /// </summary>
    [Serializable]
    public abstract class AIBehaviour : MonoBehaviour
    {
        // Resets the behaviour. Implement if needed.
        public abstract void Reset();
        
        // Executes the behaviour. Returns whether it has finished or not.
        public abstract bool Execute();
    }
}
