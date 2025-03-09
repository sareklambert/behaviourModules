using UnityEditor;

namespace BehaviourModules.Editor.AIAgentComponent
{
    public enum ConfigError
    {
        None,
        NoStateName,
        StateNameAssignedMultipleTimes,
        BehaviourListEmpty,
        TriggerListEmpty,
        NoBehaviourObjectAssigned,
        NoTriggerObjectAssigned,
        NoNextStateName,
        NextStateIsStateName
    }
    
    /// <summary>
    /// A class used for detecting config errors and setting error message values.
    /// </summary>
    public class AIAgentComponentValidator
    {
        private const string PropertyNameStateId = "stateId";
        private const string PropertyNameBehaviours = "behaviours";
        private const string PropertyNameConditions = "conditions";
        private const string PropertyNameTriggers = "triggers";
        private const string PropertyNameNextState = "nextState";
        
        public ConfigError ErrorId { get; private set; } = ConfigError.None;
        public int ErrorListIndex { get; private set; } = -1;
        public string ErrorMessage { get; private set; } = "";

        // Checks for different config errors and sets error messages accordingly
        public void CheckForErrors(SerializedProperty statesListProperty, int currentStateIndex)
        {
            // Reset
            SetError(ConfigError.None, "");

            // Check: No state name assigned
            SerializedProperty currentStateProperty = statesListProperty.GetArrayElementAtIndex(currentStateIndex);
            int ownStateId = currentStateProperty.FindPropertyRelative(PropertyNameStateId).intValue;
            if (ownStateId == -1)
            {
                SetError(ConfigError.NoStateName, "No state name assigned");
                return;
            }
            
            // Check: Same state name assigned multiple times
            for (int i = 0; i < statesListProperty.arraySize; i++)
            {
                if (i == currentStateIndex) continue;
                
                int currentStateId = statesListProperty.GetArrayElementAtIndex(i).FindPropertyRelative(PropertyNameStateId).intValue;
                if (ownStateId != currentStateId) continue;
                
                SetError(ConfigError.StateNameAssignedMultipleTimes, "State name assigned to multiple states");
                return;
            }
            
            // Check: Behaviour list is empty
            SerializedProperty behaviourListProperty = currentStateProperty.FindPropertyRelative(PropertyNameBehaviours);
            if (behaviourListProperty.arraySize == 0)
            {
                SetError(ConfigError.BehaviourListEmpty, "Behaviour list is empty");
                return;
            }

            // Check: No behaviour object assigned
            for (int i = 0; i < behaviourListProperty.arraySize; i++)
            {
                SerializedProperty behaviourProperty = behaviourListProperty.GetArrayElementAtIndex(i).FindPropertyRelative(PropertyNameBehaviours);
                if (behaviourProperty.objectReferenceValue) continue;
                
                SetError(ConfigError.NoBehaviourObjectAssigned, "No behaviour object assigned", i);
                return;
            }
            
            // Check: Condition list is empty
            SerializedProperty conditionListProperty = currentStateProperty.FindPropertyRelative(PropertyNameConditions);
            if (conditionListProperty.arraySize == 0) return;
            
            // Check: Multiple
            for (int i = 0; i < conditionListProperty.arraySize; i++)
            {
                SerializedProperty conditionProperty = conditionListProperty.GetArrayElementAtIndex(i);
                SerializedProperty triggersListProperty = conditionProperty.FindPropertyRelative(PropertyNameTriggers);
                SerializedProperty nextStateProperty = conditionProperty.FindPropertyRelative(PropertyNameNextState);

                // Check: No next state name assigned
                if (nextStateProperty.intValue == -1)
                {
                    SetError(ConfigError.NoNextStateName, "No next state name assigned", i);
                    return;
                }
                
                // Check: Next state is same as state name
                if (nextStateProperty.intValue == ownStateId)
                {
                    SetError(ConfigError.NextStateIsStateName, "Next state is same as this state", i);
                    return;
                }
                
                // Check: Trigger list is empty
                if (triggersListProperty.arraySize == 0)
                {
                    SetError(ConfigError.TriggerListEmpty, "Trigger list is empty", i);
                    return;
                }

                // Check: No trigger object assigned
                for (int j = 0; j < triggersListProperty.arraySize; j++)
                {
                    SerializedProperty triggerProperty = triggersListProperty.GetArrayElementAtIndex(j);
                    if (triggerProperty.objectReferenceValue) continue;
                    
                    SetError(ConfigError.NoTriggerObjectAssigned, "No trigger object assigned", j);
                    return;
                }
            }
        }

        // Sets the values needed to display a new error
        private void SetError(ConfigError error, string message, int index = -1)
        {
            ErrorId = error;
            ErrorMessage = message;
            ErrorListIndex = index;
        }
    }
}
