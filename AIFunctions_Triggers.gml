/*
References:

par -> state{}
other -> object
*/

/// @function		AITriggerNoHP();
/// @description	The objects hp is <= 0.
function AITriggerNoHP() constructor {
	Check = function() {
		return (other.hp <= 0);
	};
};
