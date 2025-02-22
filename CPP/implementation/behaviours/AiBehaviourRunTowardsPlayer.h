#pragma once

#include "../../core/AIBehaviour.h"
#include <iostream>

/**
 * @class AiBehaviourRunTowardsPlayer
 * @brief AI behaviour responsible for making an entity run towards the player.
 *
 * This behaviour enables an AI entity to move directly towards the player's position.
 */
class AiBehaviourRunTowardsPlayer final : public AiBehaviourBase
{
public:
    void reset() override {};
    void execute() override
    {
        std::cout << "Running towards player.\n";
    };
};
