using UnityEngine;

namespace BehaviourModules.Implementation.Player
{
    /// <summary>
    /// Simple player controller for testing.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        private Rigidbody m_rigidbody;
        private Vector3 m_moveDirection = new Vector3(0f, 0f, 0f);

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            m_moveDirection.x = Input.GetAxis("Horizontal");
            m_moveDirection.z = Input.GetAxis("Vertical");
            m_rigidbody.MovePosition(m_rigidbody.position + m_moveDirection * (moveSpeed * Time.fixedDeltaTime));
        }
    }
}
