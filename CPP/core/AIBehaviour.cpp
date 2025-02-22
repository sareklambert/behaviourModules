#include "AIState.h"

void AiBehaviourBase::set_parent(AiState* t_parent)
{
    this->parent = t_parent;
}

int AiBehaviourBase::get_module_weight() const
{
    return m_module_weight;
}

void AiBehaviourBase::set_module_weight(const int t_weight)
{
    m_module_weight = t_weight;
}
