#include "TimeManager.h"
#include "AIBehaviour.h"
#include "AIState.h"
#include <iostream>

void AiBehaviourBase::set_parent(AiState* t_parent)
{
    this->parent = t_parent;
}

int AiBehaviourBase::get_module_weight() const
{
    return m_module_weight;
}

void AiBehaviourBase::set_module_weight(const int t_weight)
{
    m_module_weight = t_weight;
}

#pragma region Wait at position
void AiBehaviourWaitAtPosition::reset()
{
    std::random_device rd;
    std::mt19937 generator(rd());
    std::uniform_real_distribution<float> distribution(m_duration_min, m_duration_max);

    // Generate the random timer
    m_timer = distribution(generator);

    std::cout << "WaitAtPosition: Waiting for " << m_timer << " seconds.\n";
}

void AiBehaviourWaitAtPosition::execute()
{
    // Advance the timer with delta time
    m_timer -= TimeManager::get_instance().get_delta_time();

    // Check if the timer is complete
    if (m_timer <= 0)
    {
        // Reset the behaviour
        std::cout << "WaitAtPosition: Timer complete.\n";
        parent->reset_current_behaviour();
    }
}
#pragma endregion
#pragma region Wander
void AiBehaviourWander::reset()
{
    
}

void AiBehaviourWander::execute()
{
    std::cout << "Wander: Wandering " << m_max_distance_from_spawn << " units from spawn.\n";
    parent->reset_current_behaviour();
}
#pragma endregion
#pragma region Run towards player
void AiBehaviourRunTowardsPlayer::reset()
{
    
}

void AiBehaviourRunTowardsPlayer::execute()
{
    std::cout << "Running towards player.\n";
}
#pragma endregion
