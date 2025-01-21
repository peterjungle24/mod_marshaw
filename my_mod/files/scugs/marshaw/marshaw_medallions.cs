namespace medals
{
    public class md_stealth
    {
        public static void stealth_hooks()
        {
            On.PlayerGraphics.DrawSprites += stealth_skill;    // [ STEALTH ] adds a Stealth skill and also makes your stealth stealth like a stealth
        }

        #region stealthValues

        public static void stealth_skill(On.PlayerGraphics.orig_DrawSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            var player = self.player;    //easier for use the player variable
            var cwt = player.Skill();    //CWT local variable

            if (cwt.HasStealthMedallion == true)    //if they have a medallion
            {
                //logic below:
                if (cwt.stealthTimer == null)  //if its null
                {
                    cwt.stealthTimer = new(151);   //creates a instance with the [float counter] defined
                    cwt.stealthCooldown = new(151); //creates a instance for the cooldown
                    cwt.stealthCooldown.Start();
                }

                if (!cwt.stealthTimer.is_running && cwt.stealthTimerReady && player.input[1].thrw && player.input[0].thrw)   //if this input
                {
                    cwt.stealthTimer.Start();  //start the stealthTimer
                }

                if (cwt.stealthTimer.is_running)
                {
                    for (var i = 0; i < sLeaser.sprites.Length; i++)
                    {
                        sLeaser.sprites[i].alpha -= 0.10f;
                    }
                }
                else
                {
                    for (var i = 0; i < sLeaser.sprites.Length; i++)
                    {
                        sLeaser.sprites[i].alpha += 0.10f;
                    }
                }

                if (cwt.stealthTimer.value_reached)
                {
                    cwt.stealthTimer.Stop();
                    cwt.stealthCooldown.Start();
                }
            }

            orig(self, sLeaser, rCam, timeStacker, camPos);
        }

        #endregion
    }
    public class md_stun
    {
        public static void stun_hooks()
        {
            On.Player.Update += stun_skill;     // [ STUN ] stuns all the creatures in a specific radius
        }

        #region stun_skill

        public static void stun_skill(On.Player.orig_Update orig, Player self, bool eu)
        {
            var room = self.room;
            var cwt = self.Skill();
            var i = self.input;

            if (cwt.HasStunMedallion == true)   //if the scug have medallion
            {
                foreach (Creature c in room.FindObjectsNearby<Creature>(self.mainBodyChunk.pos, 200f))  //checks for the distance_checker
                {
                    if (i[0].thrw && i[1].thrw && i[0].y == 1)  //if its pressing some inputs
                    {
                        c.Stun(100);  //stuns
                    }
                }
            }

            orig(self, eu);
        }

        #endregion
    }
    public class md_wind
    {
        public static void wind_hooks()
        {
            On.Player.MovementUpdate += movement_upd;   // [ DOUBLE JUMP ] update for the movement check.
            On.Player.Jump += jump_state;               // [ DOUBLE JUMP ] when you jump. Manage the boolean
            On.Player.TerrainImpact += jump_ground;     // [ DOUBLE JUMP ] sets the boolean to False when you are on the ground
        }

        #region jump_ground

        public static void jump_ground(On.Player.orig_TerrainImpact orig, Player self, int chunk, RWCustom.IntVector2 direction, float speed, bool firstContact)
        {
            var cwt_skill = self.Skill();

            if (cwt_skill.HasDoubleJumpMedallion == true)
            {
                cwt_skill.playerAlreadyJumped = false;
            }

            orig(self, chunk, direction, speed, firstContact);
        }

        #endregion
        #region movement_upd

        public static void movement_upd(On.Player.orig_MovementUpdate orig, Player self, bool eu)
        {
            Room room = self.room;
            var cwt_skill = self.Skill(); ;

            if (self.Skill().playerAlreadyJumped == true && cwt_skill.HasDoubleJumpMedallion == true && !self.input[1].jmp && self.input[0].jmp)
            {
                room.PlaySound(CustomSFX.EFF_doubleJump, self.mainBodyChunk.pos);
                room.AddObject(new PlayerBubbles(self, 3f, 0f, 1f, self.ShortCutColor()));
                self.canJump = 1;
                self.wantToJump++;
            }

            orig(self, eu); //call Orig

            if (self.Skill().HasDoubleJumpMedallion == true && self.Skill().playerAlreadyJumped == true)
            {
                if (!self.Consious ||
                self.Stunned || self.animation == Player.AnimationIndex.HangFromBeam ||
                self.animation == Player.AnimationIndex.ClimbOnBeam || self.animation == Player.AnimationIndex.AntlerClimb ||
                self.animation == Player.AnimationIndex.VineGrab || self.animation == Player.AnimationIndex.ZeroGPoleGrab ||
                self.bodyMode == Player.BodyModeIndex.WallClimb || self.bodyMode == Player.BodyModeIndex.Swimming ||
                (self.bodyMode == Player.BodyModeIndex.ZeroG || self.room.gravity <= 0.5f) && (self.wantToJump > 0))
                {
                    self.Skill().playerAlreadyJumped = false;
                }
            }
        }

        #endregion
        #region jump_state

        public static void jump_state(On.Player.orig_Jump orig, Player self)
        {
            orig(self);

            var cwt_skill = self.Skill(); ;

            if (cwt_skill.HasDoubleJumpMedallion == true)
            {
                if (self.Skill().playerAlreadyJumped == true)
                {
                    self.Skill().playerAlreadyJumped = false;
                }
                else
                {
                    self.Skill().playerAlreadyJumped = true;
                    self.canJump = 1;
                }
            }
        }

        #endregion
    }
    public class md_aqua
    {
        public static SlugcatStats.Name Marshaw { get => welp.Plugin.Marshaw; }
        public static SlugcatStats.Name slugg { get => welp.Plugin.slugg; }

        public static void aqua_hooks()
        {
            On.Player.Update += AquaUpdate;     // [ AQUA ] lets you breath more on the aqua (EXCEPT RIVULET)
        }

        #region aqua
        public static void AquaUpdate(On.Player.orig_Update orig, Player self, bool eu)
        {
            ///TO DO: breathe more in the water
            var cwt = self.Skill();

            if (cwt.HasAquaMedallion)
            {
                /// VANILLA
                if (self.slugcatStats.name == SlugcatStats.Name.Yellow ||
                    self.slugcatStats.name == SlugcatStats.Name.White ||
                    self.slugcatStats.name == SlugcatStats.Name.Red)
                {
                    cwt.AppliedLungFactor = 0.50f;
                }

                /// MSC (i can't use a switch here bc it needs to be a constant Svalue and is imcompatible)
                else if (self.slugcatStats.name == MoreSlugcatsEnums.SlugcatStatsName.Spear ||
                    self.slugcatStats.name == MoreSlugcatsEnums.SlugcatStatsName.Gourmand)
                {
                    cwt.AppliedLungFactor = 0.75f;
                }

                else if (self.slugcatStats.name == MoreSlugcatsEnums.SlugcatStatsName.Saint ||
                    self.slugcatStats.name == MoreSlugcatsEnums.SlugcatStatsName.Sofanthiel ||
                    self.slugcatStats.name == MoreSlugcatsEnums.SlugcatStatsName.Slugpup)
                {
                    cwt.AppliedLungFactor = 0.60f;
                }

                /// MY MODDED ONES
                else if (self.slugcatStats.name == Marshaw)
                {
                    cwt.AppliedLungFactor = 0.90f;
                }
                else if (self.slugcatStats.name == slugg)
                {
                    cwt.AppliedLungFactor = 0.70f;
                }

                self.slugcatStats.lungsFac = cwt.AppliedLungFactor;
            }

            orig(self, eu);
        }
        #endregion
    }
}