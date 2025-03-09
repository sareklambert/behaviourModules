using UnityEngine;

namespace BehaviourModules.Implementation.Player
{
    /// <summary>
    /// Simple player controller for testing.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        private Vector2 m_moveInput;
        private Rigidbody m_rigidbody;

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            m_moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        private void FixedUpdate()
        {
            var moveDirection = new Vector3(m_moveInput.x, 0, m_moveInput.y);
            m_rigidbody.MovePosition(m_rigidbody.position + moveDirection * (moveSpeed * Time.fixedDeltaTime));
        }
    }
}
