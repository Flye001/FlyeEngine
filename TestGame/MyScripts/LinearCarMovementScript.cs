using FlyeEngine;
using OpenTK.Mathematics;

namespace TestGame.MyScripts
{
    internal class LinearCarMovementScript : Script
    {
        private int _carDirection;
        private const float Speed = 20f;

        public LinearCarMovementScript(GameObject self) : base(self)
        {
        }

        public override void OnUpdate(float deltaTime)
        {
            switch (_carDirection % 4)
            {
                case 0:
                    Self.UpdatePosition(Self.Position + new Vector3(0, 0, Speed * deltaTime));
                    if (Self.Position.Z >= 80)
                    {
                        _carDirection++;
                        Self.Rotation += new Vector3(0, float.Pi / 2f, 0);
                    }
                    break;
                case 1:
                    Self.UpdatePosition(Self.Position + new Vector3(Speed * deltaTime, 0, 0));
                    if (Self.Position.X >= 80)
                    {
                        _carDirection++;
                        Self.Rotation += new Vector3(0, float.Pi / 2f, 0);
                    }
                    break;

                case 2:
                    Self.UpdatePosition(Self.Position + new Vector3(0, 0, -Speed * deltaTime));
                    if (Self.Position.Z <= 20)
                    {
                        _carDirection++;
                        Self.Rotation += new Vector3(0, float.Pi / 2f, 0);
                    }
                    break;
                case 3:
                    Self.UpdatePosition(Self.Position + new Vector3(-Speed * deltaTime, 0, 0));
                    if (Self.Position.X <= 0)
                    {
                        _carDirection++;
                        Self.Rotation += new Vector3(0, float.Pi / 2f, 0);
                    }
                    break;
            }
        }
    }
}
