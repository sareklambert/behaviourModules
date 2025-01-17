enum AI_STATE {
	IDLE,
	DEATH,
	FALL,
	ANTICIPATE
};

/// @function		AIState(type);
/// @param {Real} type	The state type used for identification. Use AI_STATE enum.
/// @description	Creates an AI state. Behaviour and Trigger modules can be added.
function AIState(type) constructor {
	#region Define private member variables
	self.type = type;
	currentBehaviour = undefined;
	currentRunTime = 0;
	
	behavioursList = ds_list_create();
	conditionsList = ds_list_create();
	animationsList = ds_list_create();
	
	animationTracksAmount = 5;
	#endregion
	
	#region Define state functions
	/// @function			AddBehaviour(behaviour, weight);
	/// @description		Adds a new behaviour to the state.
	/// @param {Struct} behaviour	The behaviour struct to add.
	/// @param {Real} weight	The weight (chance) the behaviour gets choosen.
	AddBehaviour = function(behaviour, weight) {
		// Store the parent reference and weight inside the behaviour struct
		behaviour.parent = self;
		behaviour.weight = weight;
		
		// Add the behaviour to the list
		ds_list_add(behavioursList, behaviour);
	};
	
	/// @function			AddCondition(newState, trigger1, trigger2, trigger3);
	/// @description		Adds a state change condition to the state.
	/// @param {Real} newState	The state to change to if the condition is fulfulled.
	/// @param {Struct} trigger1	The first trigger to check.
	/// @param {Struct} [trigger2]	The second trigger to check. (Optional)
	/// @param {Struct} [trigger3]	The third trigger to check. (Optional)
	AddCondition = function(newState, trigger1, trigger2, trigger3) {
		// Store the parent reference inside the trigger structs
		trigger1.par = self;
		if (trigger2 != undefined) trigger2.par = self;
		if (trigger3 != undefined) trigger3.par = self;
		
		// Add the condition to the list
		ds_list_add(conditionsList, {triggers : [trigger1, trigger2, trigger3], newState : newState});
	};
	
	/// @function			AddAnimation(name, track, timerMin, timerMax);
	/// @description		Adds an animation for the state.
	/// @param {String} name	The name of the animation.
	/// @param {Real} track		The track of the animation.
	/// @param {Real} [timerMin]	The minimum time (in seconds) to wait before the animation triggers.
	/// @param {Real} [timerMax]	The maximum time (in seconds) to wait before the animation triggers.
	AddAnimation = function(name, track, timerMin, timerMax) {
		// Create a new animation data struct
		var animation = {};
		animation.name = name;
		animation.timer = -1;
		animation.timerMin = timerMin;
		animation.timerMax = timerMax;
		
		// Add the new animation to the list
		ds_list_add(animationsList, animation);
	};
	
	/// @function		Step();
	/// @description	Simulates one step (frame) of the AI agent.
	Step = function() {
		// Count the time this state has run
		currentRunTime += oMain.delta;
		
		// Make sure we have a behaviour module selected
		if (currentBehaviour == undefined) {
			// Get behaviour module random weights
			var weights = [];
			var behaviourAmount = ds_list_size(behavioursList);
			for (var i = 0; i < behaviourAmount; i++) weights[i] = behavioursList[| i].weight;
			
			// Select a new behaviour module and reset it to initialize
			currentBehaviour = behavioursList[| RollMultiArray(weights)];
			currentBehaviour.Reset();
		}
		
		// Execute the current behaviour module
		currentBehaviour.Execute();
		
		// Update the AI agent's animation
		with (other) {
			// Define temporary helper variables
			var currentAnimationData = {};
			var animationAmount = ds_list_size(currentState.animationsList);
			
			// Go through animation tracks and check if we have animation data
			for (var currentTrack = 0; currentTrack < animationTracksAmount; currentTrack++) {
				if (animationAmount <= currentTrack) {
					// Clear unused animation tracks
					skeleton_animation_clear(currentTrack);
				} else {
					// Update the current animation track
					var currentAnimationData = currentState.animationsList[| currentTrack];
					
					if (currentTrack > 0) {
						// Start sub animations on other tracks once their respective timers run out
						currentAnimationData.timer -= oMain.delta;
						
						if (currentAnimationData.timer <= 0) {
							currentAnimationData.timer = random_range(currentAnimationData.timerMin, currentAnimationData.timerMax) * game_get_speed(gamespeed_fps);
							AnimationSetExt(currentAnimationData.name, currentTrack);
						}
					} else {
						// Always set the base animation on track 0
						AnimationSetExt(currentAnimationData.name, currentTrack);
					}
				}
			}
		}
		
		// Check for state change conditions
		var conditionAmount = ds_list_size(conditionsList);
		var currentCondition, currentConditionFulfilled;
		
		for (var i = 0; i < conditionAmount; i++) {
			currentCondition = conditionsList[| i];
			currentConditionFulfilled = true;
			
			// Check all triggers of the current condition
			for (var j = 0; j < 3; j++) {
				if (currentCondition.trigger[j] != undefined) currentConditionFulfilled = (currentConditionFulfilled && currentCondition.trigger[j].Check());
			}
			
			if (currentConditionFulfilled) {
				// Reset the current state
				currentBehaviour = undefined;
				currentRunTime = 0;
				
				// Switch the AI agent's state to the new one
				other.currentState = other.states[? currentCondition.newState];
				break;
			}
		}
	};
	
	/// @function		Cleanup();
	/// @description	Destroys data structures.
	Cleanup = function() {
		if (ds_exists(behavioursList, ds_type_list)) ds_list_destroy(behavioursList);
		if (ds_exists(conditionsList, ds_type_list)) ds_list_destroy(conditionsList);
		if (ds_exists(animationsList, ds_type_list)) ds_list_destroy(animationsList);
	};
	#endregion
};
