# Unity Soccer Simulation (FC 24-inspired)

This folder contains a staged Unity implementation plan and starter code for a PC soccer simulation game inspired by EA Sports FC 24 gameplay pillars.

## Recommended engine/toolkit

- **Engine**: Unity 2022.3+ LTS
- **Language**: C#
- **Input**: Unity **New Input System** package (`com.unity.inputsystem`)
- **Physics**: Unity built-in physics (`Rigidbody`, colliders)
- **Animation**: Mecanim Animator + Blend Trees
- **Data storage**: JSON files in `Assets/Data/`

This gives fast iteration, strong physics support, and straightforward controller/keyboard support.

## Project structure

```text
unity-soccer-sim/
  Assets/
    Data/
      teams.json                   # Team + player stats
    Scripts/
      Core/
        PlayerMovementController.cs # Player locomotion + tackle/pass/shoot input
        BallPhysicsController.cs    # Ball movement, drag, kick/pass impulse
        MatchBootstrap.cs           # Match-level orchestration (spawning wiring)
      AI/
        DefenderAIController.cs     # Defender chase/mark/pressure behavior
      Animation/
        PlayerAnimationController.cs # Animator parameter bridge (run/pass/shoot)
      UI/
        MenuFlowController.cs       # Main menu -> team select -> match flow scaffold
```

## Development stages

1. **Physics & movement** (implemented here)
   - Player acceleration, turning, sprint
   - Ball impulse-based pass/shoot and friction/drag
   - Player-ball and player-player collisions
2. **Simple AI** (implemented baseline here)
   - Defenders chase attackers, maintain pressure distance, intercept lanes
3. **UI & menus** (scaffolded)
   - Main menu, team select, match screen transitions
4. **Multiplayer (optional)**
   - Local split keyboard/controller or NGO/Photon for online sessions
5. **Save & load**
   - Persist custom squads, controller prefs, difficulty, career-like progression

## Setup and run locally

1. Install **Unity Hub** and Unity **2022.3 LTS**.
2. Create a new 3D Core project and copy `Assets/` from this folder into your Unity project.
3. Open **Package Manager** and install **Input System**.
4. In `Project Settings > Player > Active Input Handling`, set to `Input System Package (New)` or `Both`.
5. Create scene objects:
   - Field plane + boundaries with colliders
   - Ball object with `Rigidbody` + sphere collider + `BallPhysicsController`
   - Player prefabs with `Rigidbody`, capsule collider, `PlayerMovementController`, `PlayerAnimationController`, and `PlayerInput`
6. Hook input actions:
   - `Move` (`Vector2`), `Sprint`, `Pass`, `Shoot`, `Tackle`
   - Assign references in `PlayerMovementController`
7. Add `DefenderAIController` to non-user players and set targets.
8. Press Play.

## Keyboard/controller defaults (recommended)

- Move: `WASD` / left stick
- Sprint: `Left Shift` / gamepad south shoulder
- Pass: `J` / gamepad west
- Shoot: `K` / gamepad east
- Tackle: `L` / gamepad south

