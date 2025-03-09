using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BehaviourModules.Core
{
    /// <summary>
    /// Represents a state for AI agents, managing behavior modules and transitions based on conditions.
    /// </summary>
    [Serializable]
    public class AIState
    {
        [SerializeField] private int stateId;
        [SerializeField] private List<AIBehaviourWeight> behaviours = new List<AIBehaviourWeight>();
        [SerializeField] private List<AICondition> conditions = new List<AICondition>();
        [SerializeField] private AIAgentComponent parent;
        
        private AIBehaviour m_currentBehaviour;
        
        // Selects a random index based on weighted probabilities.
        private int WeightedRandomSelect(List<int> weights)
        {
            int totalWeight = weights.Sum();
            int randomValue = Random.Range(0, totalWeight);
        
            int currentWeight = 0;
            for (int i = 0; i < weights.Count; i++)
            {
                currentWeight += weights[i];
                if (randomValue < currentWeight)
                {
                    return i;
                }
            }
        
            throw new ArgumentException("Failed to select an index.");
        }
        
        // Clears the states current AI behavior, forcing it to select a new one.
        public void ResetCurrentBehaviour()
        {
            m_currentBehaviour = null;
        }
        
        /// Updates the current AI state, selecting and executing behaviors.
        public void UpdateState()
        {
            // Make sure we have a behaviour module selected
            if (!m_currentBehaviour)
            {
                // Select a new behaviour based on weighted probabilities
                List<int> weights = behaviours.Select(behaviour => behaviour.Weight).ToList();
                int newIndex = WeightedRandomSelect(weights);
                m_currentBehaviour = behaviours[newIndex].Behaviour;
                m_currentBehaviour.Reset();
            }
            
            // Execute the current behaviour and check if it has finished
            if (m_currentBehaviour.Execute())
            {
                ResetCurrentBehaviour();
            }
            
            // Check all the condition's triggers to determine if the state should change
            foreach (AICondition condition in conditions.Where(condition => condition.Triggers.Any(trigger => trigger.Check())))
            {
                parent.SetCurrentState(condition.NextState);
            }
        }
    }
}
