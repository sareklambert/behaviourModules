#pragma once

#include "../../core/AIBehaviour.h"
#include "../../core/AIState.h"
#include <iostream>

/**
 * @class AiBehaviourWander
 * @brief Represents an AI behaviour for wandering within a defined distance from a spawn point.
 *
 * This behaviour allows an AI entity to move randomly within a specified maximum distance from its spawn location.
 */
class AiBehaviourWander final : public AiBehaviourBase
{
public:
    void reset() override {};
    void execute() override
    {
        std::cout << "Wander: Wandering " << m_max_distance_from_spawn << " units from spawn.\n";
        parent->reset_current_behaviour();
    };

    explicit AiBehaviourWander(const float dist_max) : m_max_distance_from_spawn(dist_max)
    {
    }
    
private:
    float m_max_distance_from_spawn;
};
