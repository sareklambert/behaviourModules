#pragma once

/**
 * @class TimeManager
 * @brief A singleton class responsible for handling delta time.
 *
 * TimeManager is implemented as a singleton to ensure there is only one instance
 * responsible for tracking and updating the time differences (delta time) between
 * consecutive frames or events within the application.
 *
 * The class provides a thread-safe interface to retrieve or update the delta time.
 */
class TimeManager
{
public:
    // Singleton pattern
    static TimeManager& get_instance()
    {
        static TimeManager instance;
        return instance;
    }

    // Delta time getter and setter
    [[nodiscard]] float get_delta_time() const;
    void set_delta_time(float t_dt);

    // Prevent direct instantiation or copying
    TimeManager(const TimeManager&) = delete;
    TimeManager& operator =(const TimeManager&) = delete;
private:
    TimeManager() = default;
    ~TimeManager() = default;
    
    float m_delta_time = 0.0f;
};
