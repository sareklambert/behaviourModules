#include "TimeManager.h"

float TimeManager::get_delta_time() const
{
    return m_delta_time;
}

void TimeManager::set_delta_time(const float t_dt)
{
    m_delta_time = t_dt;
}
