#pragma once

#include "AIState.h"
#include <unordered_map>
#include <memory>

/**
 * @class Enemy
 * @brief Represents an enemy entity with AI states that govern its behavior.
 *
 * The Enemy class provides functionality for managing AI states
 * and executing behaviors based on the current state. Each state
 * is represented by an `AiState` and can have its own unique behaviors
 * and transition conditions.
 *
 * The Enemy can transition between states when specific conditions
 * are met, enabling dynamic and flexible AI behavior.
 */
class Enemy
{
public:
    Enemy() : m_current_state(nullptr)
    {
    }

    // Set the current state
    void set_current_state(state_type t_type);

    // Add a state to the enemy
    std::unique_ptr<AiState>& add_state(state_type t_type);

    // Execute the current state
    void update() const;
    
private:
    std::unordered_map<state_type, std::unique_ptr<AiState>> m_states;
    std::unique_ptr<AiState>* m_current_state;
};
