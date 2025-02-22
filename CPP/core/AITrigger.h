#pragma once

#include <list>
#include <memory>

// Forward declarations
class AiState;
enum class state_type;

/**
 * @class AiTriggerBase
 * @brief Abstract base class for AI trigger system.
 *
 * This class serves as a base interface for defining triggers in the AI system.
 * Derived classes implement specific trigger logic that determines if a
 * certain condition has been met during AI processing.
 */
class AiTriggerBase
{
public:
    virtual ~AiTriggerBase() = default;
    
    // Evaluate trigger
    virtual bool check() = 0;
};

/**
 * @class AiCondition
 * @brief Represents a condition for transitioning between AI states.
 *
 * This class is used to define conditions that determine when an AI entity should
 * transition from its current state to a new state. It acts as a container for
 * triggers, which are logical checks that evaluate whether the condition is met.
 * If any trigger within the condition evaluates to true, the condition is considered
 * satisfied, prompting a state change.
 *
 * Responsibilities include:
 * - Holding a collection of triggers to evaluate.
 * - Managing the associated state to transition to when the condition is met.
 * - Linking the condition with its parent AI state for contextual operations.
 */
class AiCondition
{
public:
    // Set the parent reference
    void set_parent(AiState* t_parent);

    // Add a trigger
    void add_trigger(std::unique_ptr<AiTriggerBase> t_new_trigger);

    // New state getter and setter
    [[nodiscard]] state_type get_new_state() const;
    void set_new_state(state_type t_new_state);
    
    // Evaluate triggers
    [[nodiscard]] bool check_triggers() const;
    
private:
    std::list<std::unique_ptr<AiTriggerBase>> m_triggers;
    state_type m_new_state;
    
protected:
    AiState* parent = nullptr;
};
