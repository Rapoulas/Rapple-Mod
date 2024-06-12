using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;


namespace RappleMod.Content.Projectiles.Heartbreaker
{   
    public class HeartbreakerBullet : ModProjectile
    {
        bool isHeartbeat = false;
        int heartbeatTimer = 0;
        int heartbeatProgress = 1;
        int direction = 1;
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.MeteorShot;
        public override void SetDefaults() {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.penetrate = 4;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = false;
            Projectile.timeLeft = 600;
        }

        public override void AI(){
            float rotation = 0;
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;

            if (isHeartbeat){
                if (heartbeatTimer == 0) heartbeatProgress = 1;
                else if (heartbeatTimer == 15) heartbeatProgress = 2;
                else if (heartbeatTimer == 30) heartbeatProgress = 3;
                else if (heartbeatTimer == 45) heartbeatProgress = 4;
                else if (heartbeatTimer == 60) heartbeatProgress = 5;
                else if (heartbeatTimer == 75) heartbeatProgress = 6;
                else if (heartbeatTimer == 90) heartbeatProgress = 7;

                if (heartbeatProgress == 1){
                    rotation = -70 * direction;
                    Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(rotation));
                    heartbeatProgress = -1;
                }
                if (heartbeatProgress == 2){
                    rotation = 140 * direction;
                    Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(rotation));
                    heartbeatProgress = -1;
                }
                else if (heartbeatProgress == 3){
                    rotation = 210 * direction;
                    Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(rotation));
                    heartbeatProgress = -1;
                }
                else if (heartbeatProgress == 4){
                    rotation = 140 * direction;
                    Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(rotation));
                    heartbeatProgress = -1;
                }
                else if (heartbeatProgress == 5){
                    rotation = 210 * direction;
                    Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(rotation));
                    heartbeatProgress = -1;
                }
                else if (heartbeatProgress == 6){
                    rotation = 140 * direction;
                    Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(rotation));
                    heartbeatProgress = -1;
                }
                else if (heartbeatProgress == 7){
                    rotation = -50 * direction;
                    Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(rotation));
                    heartbeatProgress = -1;
                    isHeartbeat = false;
                    heartbeatTimer = -1;
                }
                
                heartbeatTimer++;
            }
            else Projectile.ai[0]++;

            if (Projectile.ai[0] % 60 == 0){
                    Projectile.ai[0] = 1;
                    isHeartbeat = true;
                    direction = Projectile.direction;
            }

            int bulletTrail = Dust.NewDust(Projectile.Center, 0, 0, DustID.Firework_Yellow, 0f, 0f, 0, Color.Transparent);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Vector2 velocity = Projectile.velocity;

            Projectile.netUpdate = true;
            for (int i = 0; i < 1; i++) {
                Collision.HitTiles(Projectile.position, velocity, Projectile.width, Projectile.height);
            }

            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            Projectile.Kill();

            return false;
        }

    }
}