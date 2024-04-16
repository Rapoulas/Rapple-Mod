using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RappleMod.Content.Projectiles.SpiritStaff
{
	public class SpiritStaffProjectile : ModProjectile
	{
        private enum AIState {
            Idle,
            Spawning,
            Moving
        }

        private AIState CurrentAIState{
            get => (AIState)Projectile.ai[0];
            set => Projectile.ai[0] = (float)value;
        }
        public override void SetStaticDefaults() {
            Main.projFrames[Projectile.type] = 13;
		}
		public override void SetDefaults() {
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 1; 
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
		}

        public override void AI()
        {

            Projectile.timeLeft = 5;
            Player player = Main.player[Projectile.owner];
            if (Projectile.alpha > 1) Projectile.alpha -= 7;

            switch (CurrentAIState){
                case AIState.Idle: {
                    Projectile.frameCounter++;
                    if (Projectile.frameCounter >= 12) {
                        Projectile.frameCounter = 0;
                        if (++Projectile.frame >= 10)
                            Projectile.frame = 0;
                    }

                    Projectile.velocity *= 0;

                    if (!player.channel){
                        CurrentAIState = AIState.Spawning;
                    }
                    break;
                }
                case AIState.Spawning: {
                    Projectile.friendly = true;
                    if (++Projectile.frameCounter >= 5) {
                        Projectile.frameCounter = 0;
                        if (Projectile.frame < 10 || ++Projectile.frame >= Main.projFrames[Projectile.type]) Projectile.frame = 10;
                    }

                    Projectile.ai[1]++;

                    if (Projectile.ai[1] > 30) CurrentAIState = AIState.Moving;

                    Projectile.velocity *= 0;
                    break;
                }
                case AIState.Moving: {
                    if (++Projectile.frameCounter >= 5) {
                        Projectile.frameCounter = 0;
                        if (Projectile.frame < 10 || ++Projectile.frame >= Main.projFrames[Projectile.type]) Projectile.frame = 10;
                    }

                    NPC closestNPC = FindClosestNPC(2000);
                    Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;

                    Projectile.ai[1]++;

                    
                    if (closestNPC != null){
                        Vector2 desiredVelocity = Projectile.DirectionTo(closestNPC.Center) * 17;
                        Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 0.05f);
                    }
                    
                    break;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 3; i++){
				Dust.NewDust(Projectile.Center, 1, 1, DustID.GoldCoin, Projectile.velocity.X/2, Projectile.velocity.Y/2, 1, Color.LightBlue);
			}
            SoundEngine.PlaySound(SoundID.NPCDeath39, Projectile.position);
        }

        public NPC FindClosestNPC(float maxDetectDistance) {
			NPC closestNPC = null;

			float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

			for (int k = 0; k < Main.maxNPCs; k++) {
				NPC target = Main.npc[k];
				if (target.CanBeChasedBy()) {
					float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

					if (sqrDistanceToTarget < sqrMaxDetectDistance) {
						sqrMaxDetectDistance = sqrDistanceToTarget;
						closestNPC = target;
					}
				}
			}
            return closestNPC;
        }

        public override Color? GetAlpha(Color lightColor) {
            Color color = Color.White * (1f-(Projectile.alpha/255f)); 
            return color;
        }
    }
}
