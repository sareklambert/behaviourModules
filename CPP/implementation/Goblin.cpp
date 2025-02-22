#include "Goblin.h"

// Include behaviours
#include "behaviours/AiBehaviourWaitAtPosition.h"
#include "behaviours/AiBehaviourWander.h"
#include "behaviours/AiBehaviourRunTowardsPlayer.h"

// Include triggers
#include "triggers/AiTriggerPlayerIsInRange.h"

// Example enemy. Add states, behaviours and conditions to quickly define complex AI agents.
Goblin::Goblin()
{
    // Define states
    // Idle state
    const std::unique_ptr<AiState>& idle_state = add_state(state_type::idle);
    idle_state->add_behaviour(std::make_unique<AiBehaviourWaitAtPosition>(0.5, 2), 4);
    idle_state->add_behaviour(std::make_unique<AiBehaviourWander>(400), 1);
    idle_state->add_condition(state_type::chase, std::make_unique<AiTriggerPlayerIsInRange>());
    
    // Chase state
    const std::unique_ptr<AiState>& chase_state = add_state(state_type::chase);
    chase_state->add_behaviour(std::make_unique<AiBehaviourRunTowardsPlayer>(), 1);
    
    // Set initial state
    set_current_state(state_type::idle);
}
