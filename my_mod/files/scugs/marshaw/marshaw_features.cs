namespace marshaw_features
{
    // ft = feature
    // lgcy = legacy
    // lgcy_ft = legacy feature
    // i'm really cool :huntercool:
    public class hooks
    {
        public static void Enable()
        {
            // Features (ft)
            ft_craft.craft_hooks();     // [ CRAFT ] calls the method of the Craft hooks
            ft_pup.pup_hooks();         // [ PUP ] calls the method of the Pupify hooks
            ft_2spear.spear_hooks();    // [ SPEARS ] calls the method of the Spear Dealer hooks
            ft_sanity.sanity_hooks();   // [ SANITY ] calls the method of the Sanity hooks
            ft_gui.gui_hooks();         // [ GUI ] adds the GUI eleme

            // legacy features (lgcy_ft)

            // Medallions (md)
            md_wind.wind_hooks();       // [ DOUBLE J ] calls the method of the Double Jump medallion
            md_stun.stun_hooks();       // [ STUN ] calls the method of the Stun medallion
            md_stealth.stealth_hooks(); // [ STEALTH ] calls the method of the Stealth medallion
            md_aqua.aqua_hooks();       // [ AQUA ] calls the method of the Aqua medallion
        }
    }
    public class ft_2spear
    {
        public static SlugcatStats.Name marshaw { get => welp.Plugin.Marshaw; }    //name of my slugcat
        public static ManualLogSource Logger { get => welp.Plugin.Logger; }

        // Base Settings for Marshaw
        public static void spear_hooks()
        {
            On.Player.Grabability += SpearDealer;     // [ PLAYER ] makes Marshaw hold 2 spears with 2 hands
        }

        #region SpearDealer

        public static Player.ObjectGrabability SpearDealer(On.Player.orig_Grabability orig, Player self, PhysicalObject obj)
        {
            // checks if the scug is Marshaw
            if (self.SlugCatClass == marshaw)
            {
                // if object is Spear
                if (obj is Spear)
                {
                    // return the grab ability with 2 hands (1)
                    return (Player.ObjectGrabability)1;
                }
            }
            // call the orig as return
            return orig(self, obj);
        }

        #endregion
    }
    public class ft_craft
    {
        public static SlugcatStats.Name marshaw { get => welp.Plugin.Marshaw; }    //name of my slugcat
        public static ManualLogSource Logger { get => welp.Plugin.Logger; }

        // Base Settings for Marshaw
        public static void craft_hooks()
        {
            On.Player.CraftingResults += marshaw_cResults;      // [ CRAFT ] Craft: the results of Crafting.
            On.Player.GraspsCanBeCrafted += marshaw_gapes;      // [ CRAFT ] hand.
            On.RainWorld.PostModsInit += marshaw_postmod;       // [ CRAFT ] affter the mods initialize.
        }

        #region MarshawCraft_PostMod - PostModsInit

        /// <summary>
        /// create the hooks for let the craft works
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        public static void marshaw_postmod(On.RainWorld.orig_PostModsInit orig, RainWorld self)
        {
            // On.RainWorld.PostModsinit hook
            try
            {
                //Create the hook for [ GourmandCombos.CraftingResults ]
                On.MoreSlugcats.GourmandCombos.CraftingResults += Marshaw_GCombos;

                //Call the orig
                orig(self);
            }
            catch (Exception ex)    //if gets the error instead
            {
                //log the error. Shrimple
                Logger.LogError(ex);
            }
        }

        #endregion
        #region Marshaw_Gapes - GraspsCanBeCrafted

        public static bool marshaw_gapes(On.Player.orig_GraspsCanBeCrafted orig, Player self)
        {
            //On.player.GraspsCanBeCrafted hook

            //if the player its Marshaw
            if (self.SlugCatClass == marshaw)
            {
                //return this input for up and method is not null
                return self.input[0].y == 1 && self.CraftingResults() != null;
            }
            //return (call) the orig
            return orig(self);
        }

        #endregion
        #region Player_CraftingResults

        public static objType marshaw_cResults(On.Player.orig_CraftingResults orig, Player self)
        {
            //On.player.CraftingResults hook

            //if the grasps length is less than 2 and the class is not Marshaw
            if (self.grasps.Length < 2 || self.SlugCatClass != marshaw)
            {
                //call orig
                return orig(self);
            }

            //craft results for the hands and the craft results and i dont remember anymore sorry
            var craftingResult = Marshaw_CraftFinally(self, self.grasps[0], self.grasps[1]);           //variable for the hands full already for craft
            //these [ ? ] syntax just confuse me in the most of time.
            return craftingResult?.type;
        }

        #endregion
        #region GourmandCombos_CraftingResults

        public static objPhy Marshaw_GCombos(On.MoreSlugcats.GourmandCombos.orig_CraftingResults orig, PhysicalObject crafter, Creature.Grasp graspA, Creature.Grasp graspB)
        {
            //On.MoreSlugcats.GourmandCombos.CraftingResults hook

            /// THIS CODE BELOW IS OPTIONAL

            //If the player is Marshaw
            if ((crafter as Player).SlugCatClass == marshaw)
            {
                //Make a sound :3 this is optional for craft
                crafter.room.PlaySound(SoundID.SS_AI_Give_The_Mark_Boom, crafter.firstChunk.pos);

                /// THIS CODE BELOW IS TOTALLY OBRIGATORY FOR WORK
                return Marshaw_CraftFinally(crafter as Player, graspA, graspB); //return the method that allows you to make the spears
            }

            //return (call) orig
            return orig(crafter, graspA, graspB);
        }

        #endregion
        #region Marshaw_CraftFinally (recipes here)

        /// <summary>
        /// Finally, you can make your recipe here BRUHHHHH <br/>
        /// The recipes of craftings are stored here.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="graspA"></param>
        /// <param name="graspB"></param>
        /// <returns>a result of the craft.</returns>
        public static objPhy Marshaw_CraftFinally(Player player, Creature.Grasp graspA, Creature.Grasp graspB)
        {
            var spear = new AbstractSpear(player.room.world, null, player.abstractCreature.pos, player.room.game.GetNewID(), false);            //normal spear
            var spear_ex = new AbstractSpear(player.room.world, null, player.abstractCreature.pos, player.room.game.GetNewID(), true);          //explosive spear
            var spear_el = new AbstractSpear(player.room.world, null, player.abstractCreature.pos, player.room.game.GetNewID(), false, true);   //electric spear

            //if have nothing
            if (player == null || graspA?.grabbed == null || graspB?.grabbed == null)
            {
                return null;          //return null if have nothing to do
            }

            //if this scug is Marshaw (if not check it will affect EVERY SCUG)
            if (player.slugcatStats.name == marshaw)
            {
                // -- Crafts:
                // > Rock + Rock = Spear
                // > Spear + Grenade = Spear (explosive)
                // > Spear + Flashbang = Spear (electric | charged)
                // > Flashbang + Grenade = SingularityBomb (this is totally joke, i will allow that ONLY if got a specific medallion >:)
                // 
                // 

                // Flashbang + Grenade = SingularityBomb
                if (Test(graspA, graspB, objType.FlareBomb, objType.ScavengerBomb) )
                {
                    return new AbstractPhysicalObject(player.room.world, MSC_objtype.SingularityBomb, null, player.abstractCreature.pos, player.room.game.GetNewID());
                }

                // Rock + Rock = Spear
                if (Test(graspA, graspB, objType.Rock, objType.Rock) )
                {
                    return spear;   //craft Spear
                }

                // Spear + Bomb = Explosion Spear
                if (Test(graspA, graspB, objType.Spear, objType.ScavengerBomb) )
                {
                    return spear_ex;
                }

                // Spear + Flashbang = Electric Spear (charged)
                if (Test(graspA, graspB, objType.Spear, objType.FlareBomb) )
                {
                    spear_el.electricCharge = 0;
                    return spear_el;
                }
            }

            return null;    //nothing to do. Is the final of the code
        }

        #endregion

        public static bool Test(Creature.Grasp graspA, Creature.Grasp graspB, objType obj1, objType obj2)
        {
            var grabbedA = graspA.grabbed.abstractPhysicalObject.type;          //hand A
            var grabbedB = graspB.grabbed.abstractPhysicalObject.type;          //hand B

            return grabbedA == obj1 && grabbedB == obj2 || grabbedA == obj2 && grabbedB == obj1;
        }
    }
    public class ft_gui
    {
        public static SlugcatStats.Name marshaw { get => welp.Plugin.Marshaw; }

        public static void gui_hooks()
        {
            On.Player.Update += gui_add;
        }

        #region gui_add

        public static void gui_add(On.Player.orig_Update orig, Player self, bool eu)
        {
            // checks if the scug is Marshaw
            if (self.slugcatStats.name == marshaw)
            {
                // add the Sanity bar thing
                SanitySystem.GraphicManage(self);
            }

            // add this cooldown bar
            cooldown_bar.cooldownBar_Add(self.room.game.cameras[0], self);

            orig(self, eu);
        }

        #endregion
    }
    public class ft_pup
    {
        public static SlugcatStats.Name marshaw { get => welp.Plugin.Marshaw; }    //name of my slugcat
        public static ManualLogSource Logger { get => welp.Plugin.Logger; }

        // Base Settings for Marshaw
        public static void pup_hooks()
        {
            On.Player.ctor += pupify;  // [ PUP ] pup
        }

        #region pupify

        public static void pupify(On.Player.orig_ctor orig, Player self, AbstractCreature abstractCreature, World world)
        {
            orig(self, abstractCreature, world);

            if (self.slugcatStats.name == marshaw)      //check the slugcar is marshaw
            {
                self.playerState.forceFullGrown = false;    //makes the mess stop forcing the growth of vegetarian vegetation that grows in your favor as nature does 
                self.playerState.isPup = true;              //is pup. Only this
            }
        }

        #endregion
    }
    public class ft_sanity
    {
        public static SlugcatStats.Name marshaw { get => welp.Plugin.Marshaw; }    //name of my slugcat
        public static ManualLogSource Logger { get => welp.Plugin.Logger; }

        // Base Settings for Marshaw
        public static void sanity_hooks()
        {
            On.Player.Update += distance;                                       // [ SANITY ] makes the distance_checker work, in player ev.
            On.SaveState.SessionEnded += SanitySystem.ResetSanity ;     // [ SANITY ] resets the bar when you pass the cycle.
        }

        #region distance

        public static void distance(On.Player.orig_Update orig, Player self, bool eu)
        {
            // if the scug is Marshaw
            if (self.slugcatStats.name == marshaw)
            {
                // calls the method that makes everything work
                SanitySystem.SanityCalculate(self);
            }

            orig(self, eu);
        }

        #endregion
    }
}