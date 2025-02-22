#pragma once

#include "../InputManager.h"
#include "../../core/AITrigger.h"

/**
 * @class AiTriggerPlayerIsInRange
 * @brief AI trigger to check if the player is within a specific range.
 *
 * This class is a concrete implementation of the AiTriggerBase interface.
 * It provides logic to evaluate whether the player is in range, triggering a state transition.
 */
class AiTriggerPlayerIsInRange final : public AiTriggerBase
{
public:
    bool check() override
    {
        // Check for the spacebar in the captured inputs
        if (InputManager& input_manager = InputManager::get_instance(); input_manager.check_for_key(' '))
        {
            return true;
        }

        return false;
    };
};
