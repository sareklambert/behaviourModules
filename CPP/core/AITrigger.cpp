#include "AITrigger.h"
#include "AIState.h"

void AiCondition::set_parent(AiState* t_parent)
{
    parent = t_parent;
}

void AiCondition::add_trigger(std::unique_ptr<AiTriggerBase> t_new_trigger)
{
    m_triggers.push_back(std::move(t_new_trigger));
}

void AiCondition::set_new_state(const state_type t_new_state)
{
    m_new_state = t_new_state;
}

state_type AiCondition::get_new_state() const
{
    return m_new_state;
}

bool AiCondition::check_triggers() const
{
    // Iterate through all triggers in the list
    for (const auto& trigger : m_triggers)
    {
        // If any trigger evaluates to true, return true
        if (trigger->check())
        {
            return true;
        }
    }

    // If no triggers evaluate to true, return false
    return false;
}
