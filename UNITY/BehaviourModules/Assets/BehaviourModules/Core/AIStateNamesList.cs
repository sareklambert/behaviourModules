using UnityEngine;

namespace BehaviourModules.Core
{
    [CreateAssetMenu(fileName = "AIStateNamesList", menuName = "BehaviourModules/StateNamesList")]
    public class AIStateNamesList : ScriptableObject
    {
        [SerializeField] private string[] stateNames;
        public string[] StateNames => stateNames;
    }
}
