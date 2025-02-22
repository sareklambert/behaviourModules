#pragma once

#include "../TimeManager.h"
#include "../../core/AIBehaviour.h"
#include "../../core/AIState.h"
#include <iostream>

/**
 * @class AiBehaviourWaitAtPosition
 * @brief AI behaviour module for waiting at a fixed position for a random duration.
 *
 * This behaviour allows an AI entity to wait at its current position
 * for a randomly generated duration within a specified range defined by the minimum
 * and maximum duration parameters. The behaviour completes once the timer expires.
 */
class AiBehaviourWaitAtPosition final : public AiBehaviourBase
{
public:
    void reset() override
    {
        std::random_device rd;
        std::mt19937 generator(rd());
        std::uniform_real_distribution<float> distribution(m_duration_min, m_duration_max);
        
        // Generate the random timer
        m_timer = distribution(generator);
        
        std::cout << "WaitAtPosition: Waiting for " << m_timer << " seconds.\n";
    };
    void execute() override
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
    };

    explicit AiBehaviourWaitAtPosition(const float dur_min, const float dur_max) : m_duration_min(dur_min),
        m_duration_max(dur_max), m_timer(0)
    {
    }
    
private:
    float m_duration_min;
    float m_duration_max;
    float m_timer;
};
