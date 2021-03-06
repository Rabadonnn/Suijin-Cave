using Love;

namespace Suijin_cave.GameObjects
{
    public class Handgun : Gun
    {
        private Vector2 initialParentOffset;
        public Handgun(GameObject parent)
        {
            Type = GunType.Pistol;

            Parent = parent;
            Texture = Assets.Handgun;
            Scale = new Vector2(1.8f);

            Origin = new Vector2(Texture.GetWidth() / 2, Texture.GetHeight() / 2);
            ParentOffset = new Vector2(50, 40);
            initialParentOffset = ParentOffset;

            Ammmo = 30;

            InitParticleSystem();
        }

        protected override void InitParticleSystem()
        {
            particleSystem = Graphics.NewParticleSystem(Help.CirclePrimitve, 32);
            particleSystem.SetParticleLifetime(0.1f, 0.2f);
            particleSystem.SetColors(new Vector4(1, 0.7f, 0, 1), new Vector4(1, 0.6f, 0, 1));
            particleSystem.SetSizes(0.5f, 0f);
            particleSystem.SetRadialAcceleration(1, 10);
            particleSystem.SetSpeed(20, 50);
            particleSystem.SetSpread(Mathf.PI * 2);
        }

        private static Vector2 bulletOffset = new Vector2(12, 0);

        protected override Vector2 GetBulletPosition(Vector2 direction, Vector2 bulletOffset)
        {
            // Stupid ass dog
            // Increase the rotation a bit so you can get the accurate shooting point
            // Dumbass

            var pos = Parent.Position;

            float a = Rotation;

            if (flipped)
            {
                a += Mathf.Deg2Rad * 9;
            }
            else
            {
                a -= Mathf.Deg2Rad * 9;
            }

            pos.X += Mathf.Cos(a) * (ParentOffset.X + bulletOffset.X);
            pos.Y += Mathf.Sin(a) * (ParentOffset.Y + bulletOffset.Y);

            return pos;
        }

        public override void Shoot(Vector2 direction)
        {
            if (Ammmo > 0)
            {
                var pos = GetBulletPosition(direction, bulletOffset);
                Bullets.Add(new Bullet(pos, direction));

                particleSystem.SetPosition(pos.X, pos.Y);
                particleSystem.Emit(10);

                // Recoil
                ParentOffset = initialParentOffset * 0.65f;
                Ammmo--;
            }
        }

        public override void Update(float dt, Vector2 target)
        {
            // Reset recoil
            if (ParentOffset.X < initialParentOffset.X) ParentOffset += new Vector2(120 * dt, 0);
            if (ParentOffset.Y < initialParentOffset.Y) ParentOffset += new Vector2(0, 120 * dt);

            RotateToTarget(target);
            FlipToTarget(target);
        }
    }
}