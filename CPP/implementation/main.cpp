#include "Goblin.h"
#include "InputManager.h"
#include "TimeManager.h"
#include <chrono>
#include <thread>
#include <iostream>

// Constants
constexpr int FRAME_DELAY_MS = 16;      // Frame delay in milliseconds, ~60 FPS.
constexpr char TERMINATION_KEY = 13;    // Key to terminate the simulation.

// Run the simulation
static void run_simulation()
{
    // Initialize game logic
    const auto goblin1 = Goblin();

    // Get the manager instances
    InputManager& input_manager = InputManager::get_instance();
    TimeManager& time_manager = TimeManager::get_instance();

    // Initial message
    std::cout << "Press 'Enter' to quit the simulation.\n";

    // Simulation loop running flag
    bool running = true;

    // Time tracking
    auto previous_time = std::chrono::high_resolution_clock::now();

    // Simulation loop
    while (running)
    {
        // Update delta time
        auto current_time = std::chrono::high_resolution_clock::now();
        std::chrono::duration<float> delta_time = current_time - previous_time;
        previous_time = current_time;

        time_manager.set_delta_time(delta_time.count());

        // Capture input for this frame
        input_manager.capture_input();

        // Run game logic
        goblin1.update();

        // Terminate simulation
        if (input_manager.check_for_key(TERMINATION_KEY))
        {
            std::cout << "Terminating simulation...\n";
            running = false;
        }

        // Clear inputs at the end of this frame
        input_manager.clear_inputs();

        // Delay to simulate frame timing
        std::this_thread::sleep_for(std::chrono::milliseconds(FRAME_DELAY_MS));
    }
}

// Main function
int main(int argc, char* argv[])
{
    run_simulation();
    return 0;
}
