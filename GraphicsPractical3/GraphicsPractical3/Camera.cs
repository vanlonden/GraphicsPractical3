﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsPractical3
{
    /// <summary>
    /// This class is used to control the camera and calculate the corresponding view and projection matrices.
    /// </summary>
    class Camera
    {
        // Camera properties
        private Vector3 up;
        private Vector3 eye;
        private Vector3 focus;

        // Calculated matrices
        private Matrix viewMatrix;
        private Matrix projectionMatrix;

        private BoundingFrustum fustrum;

        public Camera(Vector3 camEye, Vector3 camFocus, Vector3 camUp, float aspectRatio = 4.0f / 3.0f)
        {
            this.up = camUp;
            this.eye = camEye;
            this.focus = camFocus;

            // Create matrices.
            this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1.0f, 300.0f);
            this.updateViewMatrix();
        }

        private void updateViewMatrix()
        {
            this.viewMatrix = Matrix.CreateLookAt(eye, focus, up);
            // Update the fustrum of the camera
            this.fustrum = new BoundingFrustum(viewMatrix * projectionMatrix);
        }

        public Vector3 Eye
        {
            get { return this.eye; }
            set { this.eye = value; this.updateViewMatrix(); }
        }

        public Vector3 Focus
        {
            get { return this.focus; }
            set { this.focus = value; this.updateViewMatrix(); }
        }

        public Matrix ViewMatrix
        {
            get { return this.viewMatrix; }
        }

        public Matrix ProjectionMatrix
        {
            get { return this.projectionMatrix; }
        }

        public BoundingFrustum Fustrum
        {
            get { return this.fustrum; }
        }

        public void SetEffectParameters(Effect effect)
        {
            // Set the right matrices in the effect.
            effect.Parameters["View"].SetValue(this.ViewMatrix);
            effect.Parameters["Projection"].SetValue(this.ProjectionMatrix);

            // If the shader has a global called "CameraPosition", we set it to the right Eye position of the camera.
            EffectParameter cameraPosition = effect.Parameters["CameraPosition"];
            if (cameraPosition != null)
                effect.Parameters["CameraPosition"].SetValue(this.Eye);
        }
    }
}
