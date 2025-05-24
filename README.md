# Lift

A simple, interactive elevator (lift) simulation built with .NET 8 and WinUI. The application models a lift system with a graphical interface, allowing users to call the lift, select floors, open/close doors, and trigger an alarm.

## Features

- **Simulated Lift Movement:** Move the lift between floors (1–6) with realistic delays.
- **Call and Select Floor:** Call the lift to a floor or select a destination from inside.
- **Door Controls:** Open and close the lift doors with dedicated buttons.
- **Status Indicators:** Real-time display of current floor, door state, and lift status (moving up, moving down, stopped).
- **Alarm:** Sound an alarm for emergencies.
- **Visual Feedback:** Button highlights and status updates in the UI.

## Behavior

- The lift processes floor requests in order, moving one floor at a time with a delay to simulate travel.
- Doors automatically close before moving and open upon arrival.
- The UI updates in real time to reflect the lift's state and actions.
- Multiple floor requests are queued and handled sequentially.

## Dependencies

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [WinUI 3](https://learn.microsoft.com/en-us/windows/apps/winui/winui3/) (Windows App SDK)

## Getting Started

1. **Clone the repository**
2. **Open the solution in Visual Studio 2022**
3. **Restore NuGet packages**
4. **Build and run the solution**

## Project Structure

- `Lift.Core`: Contains the core lift logic and state management.
- `Lift.UI`: WinUI 3 project providing the graphical user interface.

## Usage

- Use the left column to call the lift to a specific floor.
- Use the main grid to select a destination floor from inside the lift.
- Use the door and alarm buttons for additional controls.
