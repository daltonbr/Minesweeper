# Minesweeper

Implementing a **State Machine pattern** (among other things in Unity), with Minesweeper as an excuse.

_**Youtube Teaser**_
[![Minesweeper](https://img.youtube.com/vi/fAvM8WOdaTk/0.jpg)](https://www.youtube.com/watch?v=fAvM8WOdaTk)

## State Machine Diagram

![State Machine Diagram](/Docs/StateMachineMinesweeper.png)

_**State Machine Diagram**_.

## Problem to solve

Reordering an app workflow could be a complex task, separating the concerns between logic and UI, managing events.
State Machine Pattern helps us with that, facilitating debug and maintenance.

## Where to Find

`Assets/Scripts/State/States`  
`Assets/Scripts/State/UIStates`

`UIStates` run in conjunction with 'regular' states, decoupling UI concerns from the game logic.

## Interesting features

* Use of my [custom git Package](https://github.com/daltonbr/com.daltonlima.core) - for singletons
* [CameraManager](/Assets/Scripts/Managers/CameraManager.cs) that dynamically adjusts the Camera Height accordingly.
  
