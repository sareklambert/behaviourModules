// Set up states map
states = ds_map_create();
currentState = noone;

var newState;

// Define idle state
newState = new AIState(AI_STATE.IDLE);
newState.AddBehaviour(new AIBehaviourWait(.5, 2), 0.8);
newState.AddBehaviour(new AIBehaviourWander(400), 0.2);
newState.AddCondition(AI_STATE.DEATH, new AITriggerNoHP());
newState.AddCondition(AI_STATE.ANTICIPATE, new AITriggerPlayerIsInArea(400, 50, 400, 50));
newState.AddAnimation("idle", 0, 1);
newState.AddAnimation("flavour1", 1, 3, 10);
newState.AddAnimation("flavour2", 2, 3, 10);
states[? newState.type] = newState;

// Set initial state
currentState = states[? AI_STATE.IDLE];
