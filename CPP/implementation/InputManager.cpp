#include "InputManager.h"

#include <conio.h>

void InputManager::capture_input()
{
    while (_kbhit())
    {
        char key = static_cast<char>(_getch());
        std::lock_guard<std::mutex> lock(m_input_mutex);
        m_key_list.push_back(key);
    }
}

bool InputManager::check_for_key(const char t_key)
{
    std::lock_guard<std::mutex> lock(m_input_mutex);
    return std::find(m_key_list.begin(), m_key_list.end(), t_key) != m_key_list.end();
}

void InputManager::clear_inputs()
{
    std::lock_guard<std::mutex> lock(m_input_mutex);
    m_key_list.clear();
}
