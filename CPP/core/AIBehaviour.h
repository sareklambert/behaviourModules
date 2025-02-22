#pragma once

// Forward declarations
class AiState;

/**
 * @class AiBehaviourBase
 * @brief Abstract base class representing a general AI behaviour.
 *
 * This class provides the basic framework for implementing AI behaviours,
 * encapsulating functionalities such as managing module weight and assigning a parent AI state.
 * Derived classes must implement the reset() and execute() functions to define specific behaviour logic.
 */
class AiBehaviourBase
{
public:
    virtual ~AiBehaviourBase() = default;

    // Set the parent reference
    void set_parent(AiState* t_parent);

    // Module weight getter and setter
    [[nodiscard]] int get_module_weight() const;
    void set_module_weight(const int t_weight);

    // Module functions that need to be implemented
    virtual void reset() = 0;
    virtual void execute() = 0;
    
private:
    int m_module_weight = 0;
    
protected:
    AiState* parent = nullptr;
};
