using UnityEngine;
using BehaviourModules.Core;

namespace BehaviourModules.Behaviours
{
    /// <summary>
    /// Picks a random wait time up to the maximum and waits until completion.
    /// </summary>
    public class AIBehaviourWait : AIBehaviour
    {
        [SerializeField] private float maxTime;
        private float m_timer;

        public override void Reset()
        {
            m_timer = 0.0f;
        }
        
        public override bool Execute()
        {
            m_timer += Time.deltaTime;

            return !(m_timer < maxTime);
        }
    }
}
