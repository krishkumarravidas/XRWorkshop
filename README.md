# XRWorkshop – Prop Arena (Meta Quest 3)

A VR/mixed-reality workshop project built in **Unity 6 (6000.6.9f1)** for the **Meta Quest 3**.
You stand in a walled arena full of floating props that spin, bob and sway. You can grab them with
controllers or hands, walk around with the thumbstick, and open a world-space menu to change speed,
size and colour, drop props with gravity, or reset everything.

Repo: https://github.com/krishkumarravidas/XRWorkshop

## Features

- Roughly 36 floating props with looping animation (spin, bob, sway, pulse)
- Grab and throw props with controllers or hand tracking (Meta Interaction SDK)
- Left-thumbstick locomotion relative to your gaze, clamped to the arena walls
- In-VR menu (toggle with the **left controller X button**):
  - Pause / Resume animation
  - Speed slider and Size slider
  - Random colours
  - Drop props (gravity) / Float props
  - Reset all
  - Close
- Arena built from materials in `Assets/Materials` (floor, walls, trim, pillars, platform, rings, props)

## Tech stack

| Item | Version |
|---|---|
| Unity | 6000.6.9f1 |
| Meta XR SDK (all-in-one) | 209.0.0 |
| Unity XR Oculus | 4.5.6 |
| XR Interaction Toolkit | 3.6.1 |
| Input System | 1.20.1 |
| MCP for Unity (editor automation) | v10.3.0 |

## Project layout

```
Assets/
  Scenes/SampleScene.unity     Main scene (arena, props, menu, rig)
  Scripts/                     Gameplay scripts (see below)
  Materials/                   Arena and prop materials
  Resources/                   Meta XR / Oculus settings assets
  XR/, XRI/, Oculus/, Samples/ XR plugin settings and samples
Packages/manifest.json         Package dependencies
ProjectSettings/               Unity project settings
Builds/Quest3/PropArena.apk    Local Quest 3 build (not tracked by default)
```

### Scripts

| Script | Purpose |
|---|---|
| `PropAnimator` | Spin/bob/sway/pulse loop per prop; pauses while held; supports Drop, Float, ResetToStart; global speed/size/pause |
| `PropController` | Wires the VR menu UI to every `PropAnimator`; places the menu in front of the head; X-button toggle |
| `ThumbstickLocomotion` | Left-stick movement relative to gaze, optional right-stick snap turn, arena bounds |
| `SineWaveMovement` | Side-to-side sine movement with forward/back travel for a Rigidbody object |
| `GrabPausesMovement` | Disables `SineWaveMovement` while grabbed and re-anchors on release |

## Getting started

1. Install **Unity 6000.6.9f1** with the **Android Build Support** module.
2. Clone the repo and open it from Unity Hub.
3. Let Unity restore packages (first import takes a while).
4. Open `Assets/Scenes/SampleScene.unity`.
5. Switch Build Target to **Android**, connect a Quest 3 in developer mode, then **Build And Run**.

## Controls

| Input | Action |
|---|---|
| Left thumbstick | Move |
| Grip / pinch on a prop | Grab and move it |
| Left X button | Show / hide menu |
| Point + select on menu | Press buttons, drag sliders |

## Team

See [docs/PROJECT_REPORT.md](docs/PROJECT_REPORT.md) for what has been done and the task split between
**Krish**, **Harsh** and **Manya**.

## Contributing

1. Pull the latest `main` before starting.
2. Work on a branch named `feature/<short-name>`.
3. Unity scenes merge badly: tell the team before editing `SampleScene.unity`, or work in a copy of the scene / a prefab.
4. Do not commit `Library/`, `Temp/`, `Logs/`, `UserSettings/` or built APKs.
5. Open a pull request and have one teammate review it.
