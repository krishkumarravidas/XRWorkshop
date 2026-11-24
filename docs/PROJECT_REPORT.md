# XRWorkshop – Project Report and Task Split

Repo: https://github.com/krishkumarravidas/XRWorkshop
Date: 2026-11-24

## 1. What we did so far

**Setup**
- Created a Unity 6 (6000.6.9f1) project and configured it for Meta Quest 3 (Android, XR Plug-in Management, Oculus loader).
- Installed Meta XR SDK 209.0.0, XR Interaction Toolkit 3.6.1, XR Oculus 4.5.6, Input System 1.20.1, plus MCP for Unity so the editor can be driven by Claude.
- Updated project settings (quality, audio, tags, input actions) for VR.

**Scene**
- Built the arena in `SampleScene`: floor, four walls, trim, pillars, platform, rings, with matching materials in `Assets/Materials`.
- Added the OVR camera rig and about 36 grabbable "Prop" objects, plus a world-space prop menu (Pause, Speed, Size, Random colors, Drop, Reset, Close).

**Scripts** (`Assets/Scripts`)
- `PropAnimator`: looping spin/bob/sway/pulse; pauses while held and continues from where it is released; Drop/Float/Reset support.
- `PropController`: connects menu buttons and sliders to all props; menu spawns in front of the player; left X button toggles it.
- `ThumbstickLocomotion`: gaze-relative movement with arena bounds.
- `SineWaveMovement` and `GrabPausesMovement`: sine-path movement that stops while an object is grabbed.

**Build**
- Produced a Quest 3 APK at `Builds/Quest3/PropArena.apk`.

## 2. Status

| Area | Status |
|---|---|
| Project and XR setup | Done |
| Arena and materials | Done (first pass) |
| Props, grabbing, animation | Done |
| VR menu | Done |
| Locomotion | Done |
| Testing on device, polish, docs | Open |

## 3. Task split (proposed – adjust as you agree)

| Owner | Tasks |
|---|---|
| **Krish** | Repo owner: commit and push the current work, merge pull requests, final build and APK, keep scene `SampleScene` as the single source of truth |
| **Harsh** | Gameplay and scripting: add new prop behaviours (e.g. sounds on grab, collision effects), tune `PropAnimator` values per prop, extend the menu (new buttons or sliders), code cleanup and comments |
| **Manya** | Art, UX and testing: improve arena look (lighting, materials, skybox), make the menu readable in VR (font size, spacing, colours), test on the Quest 3 and log bugs, take screenshots or video for the demo |

### Suggested first tasks

- [ ] Krish: push the current project to GitHub and share the repo access
- [ ] Harsh: add a grab sound and a hit sound to props
- [ ] Harsh: add a "reset speed/size only" button to the menu
- [ ] Manya: lighting and skybox pass on the arena
- [ ] Manya: on-device test checklist (grab, throw, menu, locomotion, bounds) with bug list
- [ ] All: record a short demo and write the final presentation notes

## 4. Working rules

- One person edits `SampleScene.unity` at a time, because Unity scene files conflict badly in Git.
- Use branches (`feature/harsh-...`, `feature/manya-...`) and pull requests into `main`.
- Do not commit `Library/`, `Temp/`, `Logs/`, `UserSettings/`.

## 5. Open questions

- Final deliverable and deadline for the workshop
- Whether hand tracking must be supported or controllers only
- Who owns presentation and demo
