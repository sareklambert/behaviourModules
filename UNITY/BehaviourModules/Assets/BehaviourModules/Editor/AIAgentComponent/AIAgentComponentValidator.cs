using UnityEditor;

namespace BehaviourModules.Editor.AIAgentComponent
{
    /// <summary>
    /// A class used for detecting config errors and setting error message values.
    /// </summary>
    public class AIAgentComponentValidator
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

        public ConfigError ErrorId { get; private set; } = ConfigError.None;
        public int ErrorListIndex { get; private set; } = -1;
        public string ErrorMessage { get; private set; } = "";

        // Checks for different config errors and sets error messages accordingly
        public void CheckForErrors(SerializedProperty statesListProperty, int currentStateIndex)
        {
            // Reset
            SetError(ConfigError.None, "");

            // Check: No state name assigned
            var currentStateProperty = statesListProperty.GetArrayElementAtIndex(currentStateIndex);
            var ownStateId = currentStateProperty.FindPropertyRelative("stateId").intValue;
            if (ownStateId == -1)
            {
                SetError(ConfigError.NoStateName, "No state name assigned");
                return;
            }
            
            // Check: Same state name assigned multiple times
            for (var i = 0; i < statesListProperty.arraySize; i++)
            {
                if (i == currentStateIndex) continue;
                
                var currentStateId = statesListProperty.GetArrayElementAtIndex(i).FindPropertyRelative("stateId").intValue;
                if (ownStateId != currentStateId) continue;
                
                SetError(ConfigError.StateNameAssignedMultipleTimes, "State name assigned to multiple states");
                return;
            }
            
            // Check: Behaviour list is empty
            var behaviourListProperty = currentStateProperty.FindPropertyRelative("behaviours");
            if (behaviourListProperty.arraySize == 0)
            {
                SetError(ConfigError.BehaviourListEmpty, "Behaviour list is empty");
                return;
            }

            // Check: No behaviour object assigned
            for (var i = 0; i < behaviourListProperty.arraySize; i++)
            {
                var behaviourProperty = behaviourListProperty.GetArrayElementAtIndex(i).FindPropertyRelative("behaviour");
                if (behaviourProperty.objectReferenceValue) continue;
                
                SetError(ConfigError.NoBehaviourObjectAssigned, "No behaviour object assigned", i);
                return;
            }
            
            // Check: Condition list is empty
            var conditionListProperty = currentStateProperty.FindPropertyRelative("conditions");
            if (conditionListProperty.arraySize == 0) return;
            
            // Check: Multiple
            for (var i = 0; i < conditionListProperty.arraySize; i++)
            {
                var conditionProperty = conditionListProperty.GetArrayElementAtIndex(i);
                var triggersListProperty = conditionProperty.FindPropertyRelative("triggers");
                var nextStateProperty = conditionProperty.FindPropertyRelative("nextState");

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
                for (var j = 0; j < triggersListProperty.arraySize; j++)
                {
                    var triggerProperty = triggersListProperty.GetArrayElementAtIndex(j);
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
