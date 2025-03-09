using System.Collections.Generic;
using UnityEngine;

namespace BehaviourModules.Core
{
    /// <summary>
    /// Manages states, behaviours and state change conditions.
    /// </summary>
    public class AIAgentComponent : MonoBehaviour
    {
        [SerializeField] private AIStateNamesList stateNamesList;
        [SerializeField] private List<AIState> states = new();
        private int m_currentStateIndex;

        public void SetCurrentState(int index)
        {
            m_currentStateIndex = index;
        }
        
        private void FixedUpdate()
        {
            states[m_currentStateIndex].UpdateState();
        }
    }
}
