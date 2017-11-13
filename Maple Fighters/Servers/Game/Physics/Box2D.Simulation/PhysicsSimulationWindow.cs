﻿using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Box2DX.Dynamics;
using OpenTK.Graphics;
using OpenTK.Input;
using Color = System.Drawing.Color;

namespace Physics.Box2D.PhysicsSimulation
{
    public class PhysicsSimulationWindow : GameWindow
    {
        public World World { get; set; }

        private const float CAMERA_MOVEMENT_SPEED = 0.01f;
        private const float MOVE_SPEED_VIA_KEYBOARD_MINIMUM_VALUE = 1;

        private float moveSpeedViaKeyboard = 1;

        private readonly string windowTitle;
        private readonly CameraView cameraView = new CameraView();

        public PhysicsSimulationWindow(string title, int width, int height)
            : base(width, height, GraphicsMode.Default, title, GameWindowFlags.FixedWindow)
        {
            windowTitle = title;
        }

        private void SimulateWorld()
        {
            // Prepare for simulation. Typically we use a time step of 1/60 of a
            // second (60Hz) and 10 iterations. This provides a high quality simulation
            // in most game scenarios.
            const float TIME_STEP = 1.0f / 60.0f;
            const int VELOCITY_ITERATIONS = 8;
            const int POSITION_ITERATIONS = 3;

            // Instruct the world to perform a single step of simulation. It is
            // generally best to keep the time step and iterations fixed.
            World.Step(TIME_STEP, VELOCITY_ITERATIONS, POSITION_ITERATIONS);
        }

        private void MoveCameraViewViaMouse()
        {
            if (!IsMouseKeyDown() || !Focused)
            {
                return;
            }

            var direction = GetMousePosition() - cameraView.Position;
            cameraView.Position += direction * CAMERA_MOVEMENT_SPEED;
        }

        private void MoveCameraViewViaKeyboard()
        {
            if (!Focused || IsMouseKeyDown())
            {
                return;
            }

            if (Keyboard.GetState().IsKeyDown(Key.Left))
            {
                cameraView.Position += new Vector2(-1, 0) * moveSpeedViaKeyboard;
            }

            if (Keyboard.GetState().IsKeyDown(Key.Right))
            {
                cameraView.Position += new Vector2(1, 0) * moveSpeedViaKeyboard;
            }

            if (Keyboard.GetState().IsKeyDown(Key.Up))
            {
                cameraView.Position += new Vector2(0, 1) * moveSpeedViaKeyboard;
            }

            if (Keyboard.GetState().IsKeyDown(Key.Down))
            {
                cameraView.Position += new Vector2(0, -1) * moveSpeedViaKeyboard;
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            var value = (float)e.Value / 1000;
            if (value < CameraView.MINIMUM_CAMERA_ZOON_VALUE)
            {
                value = CameraView.MINIMUM_CAMERA_ZOON_VALUE;
            }

            cameraView.Zoom = value;
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.KeypadPlus)
            {
                moveSpeedViaKeyboard += MOVE_SPEED_VIA_KEYBOARD_MINIMUM_VALUE;
            }

            if (e.Key == Key.KeypadMinus)
            {
                if (Math.Abs(moveSpeedViaKeyboard) <= MOVE_SPEED_VIA_KEYBOARD_MINIMUM_VALUE)
                {
                    moveSpeedViaKeyboard = MOVE_SPEED_VIA_KEYBOARD_MINIMUM_VALUE;
                    return;
                }

                moveSpeedViaKeyboard -= MOVE_SPEED_VIA_KEYBOARD_MINIMUM_VALUE;
            }

            if (e.Key == Key.Enter)
            {
                cameraView.Reset();
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            Title = $"{windowTitle} - FPS: {RenderFrequency:0.0} - Move Speed Via Keyboard: {moveSpeedViaKeyboard}";

            MoveCameraViewViaMouse();
            MoveCameraViewViaKeyboard();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.CornflowerBlue);

            cameraView.Update();

            if (World != null)
            {
                SimulateWorld();
            }

            SwapBuffers();
        }
        
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            World?.SetDebugDraw(null);
            World = null;
        }

        private bool IsMouseKeyDown()
        {
            const MouseButton CAMERA_MOVEMENT_MOUSE_KEY = MouseButton.Right;
            return OpenTK.Input.Mouse.GetState().IsButtonDown(CAMERA_MOVEMENT_MOUSE_KEY);
        }

        private Vector2 GetMousePosition()
        {
            return new Vector2(OpenTK.Input.Mouse.GetState().X, -OpenTK.Input.Mouse.GetState().Y);
        }
    }
}