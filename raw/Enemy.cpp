#include "Enemy.h"

std::unique_ptr<AiState>& Enemy::add_state(const state_type t_type)
{
    // Create an instance of AIState
    auto new_state = std::make_unique<AiState>();

    // Set parent reference
    new_state->set_parent(this);

    // Add the new state to the map
    auto& state_ref = m_states[t_type] = std::move(new_state);

    // Return a reference to the unique_ptr in the map
    return state_ref;
}

void Enemy::update() const
{
    // Update the current state
    if (m_current_state && *m_current_state)
    {
        (*m_current_state)->update();
    }
}

void Enemy::set_current_state(const state_type t_type)
{
    // Find the state in the list
    const auto it = m_states.find(t_type);
    if (it != m_states.end())
    {
        // Set the current state
        m_current_state = &it->second;
    }
}
