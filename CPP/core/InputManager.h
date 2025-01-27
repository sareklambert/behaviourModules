#pragma once

#include <vector>
#include <mutex>

/**
 * @class InputManager
 * @brief A singleton class responsible for handling user keyboard inputs.
 *
 * This class captures keyboard inputs, processes them, and provides functionality
 * to check for specific key presses and clear captured inputs. It ensures thread-safe
 * interaction with the captured key list using a mutex.
 */
class InputManager
{
public:
    // Singleton pattern
    static InputManager& get_instance()
    {
        static InputManager instance;
        return instance;
    }

    // Capture all current key inputs using _kbhit and _getch
    void capture_input();

    // Check for a specific key's presence in the captured input
    bool check_for_key(const char t_key);

    // Clear all captured inputs (to be called at the end of main loop)
    void clear_inputs();

    // Prevent direct instantiation or copying
    InputManager(const InputManager&) = delete;
    InputManager& operator =(const InputManager&) = delete;

private:
    InputManager() = default;
    ~InputManager() = default;

    std::vector<char> m_key_list;
    std::mutex m_input_mutex;
};
