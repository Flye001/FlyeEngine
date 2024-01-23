using FlyeEngine;
using OpenTK.Mathematics;

namespace TestGame.MyScripts
{
    internal class CarMovementScript : Script
    {
        private int _carDirection;
        private const float Speed = 0.4f;

        private float t = 0; // Parameter for Bezier curve
        private Vector3[] controlPoints = new Vector3[4]; // Control points for Bezier curve

        public CarMovementScript(GameObject self) : base(self)
        {
            // Initialize control points here
            // For example:
            controlPoints[0] = new Vector3(0, 0, 20);
            controlPoints[1] = new Vector3(-10, 0, 50);
            controlPoints[2] = new Vector3(10, 0, 50);
            controlPoints[3] = new Vector3(0, 0, 80);
        }

        public override void OnUpdate(float deltaTime)
        {
            t += Speed * deltaTime;
            if (t > 1)
            {
                t = 0;
                _carDirection++;
                // Update control points for the next curve here
                switch (_carDirection % 4)
                {
                    case 0:
                        controlPoints[0] = new Vector3(0, 0, 20);
                        controlPoints[1] = new Vector3(-10, 0, 40);
                        controlPoints[2] = new Vector3(10, 0, 60);
                        controlPoints[3] = new Vector3(0, 0, 80);
                        break;
                    case 1:
                        controlPoints[0] = new Vector3(0, 0, 80);
                        controlPoints[1] = new Vector3(25, 0, 90);
                        controlPoints[2] = new Vector3(50, 0, 70);
                        controlPoints[3] = new Vector3(80, 0, 80);
                        break;

                    case 2:
                        controlPoints[0] = new Vector3(80, 0, 80);
                        controlPoints[1] = new Vector3(70, 0, 60);
                        controlPoints[2] = new Vector3(90, 0, 40);
                        controlPoints[3] = new Vector3(80, 0, 20);
                        break;
                    case 3:
                        controlPoints[0] = new Vector3(80, 0, 20);
                        controlPoints[1] = new Vector3(60, 0, 30);
                        controlPoints[2] = new Vector3(20, 0, 10);
                        controlPoints[3] = new Vector3(0, 0, 20);
                        break;
                }
                Self.Rotation += new Vector3(0, float.Pi / 2f, 0);
            }

            // Calculate position on Bezier curve
            Vector3 position = (1 - t) * (1 - t) * (1 - t) * controlPoints[0] +
                               3 * (1 - t) * (1 - t) * t * controlPoints[1] +
                               3 * (1 - t) * t * t * controlPoints[2] +
                               t * t * t * controlPoints[3];
            position.Y = Self.Position.Y;

            Self.UpdatePosition(position);
            
        }
    }
}
