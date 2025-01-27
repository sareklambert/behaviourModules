#include "AIState.h"
#include "Enemy.h"
#include <vector>

void AiState::reset_current_behaviour()
{
    m_current_behaviour = nullptr;
}

void AiState::set_parent(Enemy* t_parent)
{
    parent = t_parent;
}

void AiState::add_behaviour(std::unique_ptr<AiBehaviourBase> t_new_behaviour, const int t_weight)
{
    // Set the parent reference
    t_new_behaviour->set_parent(this);

    // Store the module weight
    t_new_behaviour->set_module_weight(t_weight);

    // Add the new behaviour to the list
    m_behaviours.push_back(std::move(t_new_behaviour));
}

void AiState::update()
{
    // Make sure we have a behaviour module selected
    if (m_current_behaviour == nullptr)
    {
        // Get behaviour module random weights
        std::vector<int> weights;
        for (const auto& behaviour : m_behaviours)
        {
            weights.push_back(behaviour->get_module_weight());
        }

        // Select a new behaviour module
        const int selected_index = weighted_random_select(weights);

        // Set current behaviour to the selected behaviour module
        const auto it = std::next(m_behaviours.begin(), selected_index);
        m_current_behaviour = &(*it);

        // Reset the newly selected module
        m_current_behaviour->get()->reset();
    }

    // Execute the current behaviour module
    m_current_behaviour->get()->execute();

    // Check for state change conditions
    for (const auto& condition : m_conditions)
    {
        if (condition->check_triggers())
        {
            parent->set_current_state(condition->get_new_state());
        }
    }
}
