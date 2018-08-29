# Unity Template Project

Unity Template Project is a constant work in progress.

This project is meant to provide a starting template for other Unity projects.

This template follows the patterns and architecture [here](https://hackmd.io/_XCk-FyzRgqpJ5cK9qOukQ).

## Usage
The fastest way is to clone it and remove .git folder, or you can download it as .zip file on github. After that, rename the folder to your game's name and start developing!

## Current Services

* __Debugging
    * FPSVisualizer
    * RenderDepth (for testing shaders that require depth textures)

* __Utility
    * ObjectPool
    * EventHandler
    * AngleMath
    * ColorMath

* _GameServices (Constant)
    * AudioService
    * SceneService
    * LanguageService

* MobileInputSystem (with JoyStick support)
* TextDisplaySystem

* Shader
    * 2D (UI)
        * BottomTextFieldShader.shader
    * 3D 
        * PivotFadeOutShader.shader
        * SingleColorTransparent.shader
    * CustomPBR
        * DissolveSurfaceShader.shader
        * WaterShader.shader
        * VertexLitSingleColor.shader