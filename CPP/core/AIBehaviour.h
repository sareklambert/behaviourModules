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
    void reset() override;
    void execute() override;

    explicit AiBehaviourWaitAtPosition(const float dur_min, const float dur_max) : m_duration_min(dur_min),
        m_duration_max(dur_max), m_timer(0)
    {
    }
    
private:
    float m_duration_min;
    float m_duration_max;
    float m_timer;
};

/**
 * @class AiBehaviourWander
 * @brief Represents an AI behaviour for wandering within a defined distance from a spawn point.
 *
 * This behaviour allows an AI entity to move randomly within a specified maximum distance from its spawn location.
 */
class AiBehaviourWander final : public AiBehaviourBase
{
public:
    void reset() override;
    void execute() override;

    explicit AiBehaviourWander(const float dist_max) : m_max_distance_from_spawn(dist_max)
    {
    }
    
private:
    float m_max_distance_from_spawn;
};

/**
 * @class AiBehaviourRunTowardsPlayer
 * @brief AI behaviour responsible for making an entity run towards the player.
 *
 * This behaviour enables an AI entity to move directly towards the player's position.
 */
class AiBehaviourRunTowardsPlayer final : public AiBehaviourBase
{
public:
    void reset() override;
    void execute() override;
};
