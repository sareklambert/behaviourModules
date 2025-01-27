#pragma once

#include "AIBehaviour.h"
#include "AITrigger.h"
#include <list>
#include <memory>
#include <vector>
#include <random>
#include <numeric>

// Forward declarations
class Enemy;

enum class state_type
{
    idle,
    chase
};

/**
 * @class AiState
 * @brief Represents the artificial intelligence (AI) state for an entity.
 *        This class manages behavior modules and state transition conditions.
 *
 * The AiState class defines an AI state and encapsulates the behaviors and conditions
 * required to transition between states. It allows the addition and execution of AI
 * behavior modules and provides mechanisms to define state-change triggers.
 */
class AiState
{
public:
    AiState() : parent(nullptr), m_current_behaviour(nullptr)
    {
    }

    // Reset the current behaviour
    void reset_current_behaviour();

    // Set the parent reference
    void set_parent(Enemy* t_parent);

    // Add a behaviour module
    void add_behaviour(std::unique_ptr<AiBehaviourBase> t_new_behaviour, int t_weight);

    // Add a state change condition
    // Template function is defined in header file
    template <typename... Triggers>
    void add_condition(const state_type t_new_state, Triggers&&... t_triggers)
    {
        // Create a new condition container
        auto new_condition = std::make_unique<AiCondition>();

        // Add new state
        new_condition->set_new_state(t_new_state);

        // Add all triggers
        (new_condition->add_trigger(std::forward<Triggers>(t_triggers)), ...);

        // Set the parent reference
        new_condition->set_parent(this);

        // Add the new condition to the list
        m_conditions.push_back(std::move(new_condition));
    }

    // Execute the state
    void update();

private:
    std::list<std::unique_ptr<AiBehaviourBase>> m_behaviours;
    std::list<std::unique_ptr<AiCondition>> m_conditions;

    std::unique_ptr<AiBehaviourBase>* m_current_behaviour;

    // Weighted random function
    static int weighted_random_select(const std::vector<int>& t_weights)
    {
        if (t_weights.empty())
        {
            throw std::invalid_argument("Weights cannot be empty.");
        }

        // Calculate the total weight sum
        const int total_weight = std::accumulate(t_weights.begin(), t_weights.end(), 0);

        // Generate a random number between 0 and totalWeight
        std::random_device rd;
        std::mt19937 gen(rd());
        std::uniform_int_distribution<> dis(0, total_weight - 1);
        const int random_value = dis(gen);

        // Select the index based on the cumulative weight
        int cumulative_weight = 0;
        for (size_t i = 0; i < t_weights.size(); ++i)
        {
            cumulative_weight += t_weights[i];

            if (random_value < cumulative_weight)
            {
                return static_cast<int>(i);
            }
        }

        // Just in case, but should never reach here
        throw std::runtime_error("Failed to select an index.");
    }

protected:
    Enemy* parent;
};
