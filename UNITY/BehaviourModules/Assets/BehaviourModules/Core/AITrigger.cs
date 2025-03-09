using System;
using UnityEngine;

namespace BehaviourModules.Core
{
    /// <summary>
    /// Represents a single trigger, used for state change conditions.
    /// </summary>
    public abstract class AITrigger : MonoBehaviour
    {
        public virtual bool Check()
        {
            throw new NotImplementedException();
        }
    }
}
