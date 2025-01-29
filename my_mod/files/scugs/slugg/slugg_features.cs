namespace slugg_features
{
    public class hooks
    {
        public static ManualLogSource Logger { get => welp.Plugin.Logger; }
        public static SlugcatStats.Name Slugg { get => welp.Plugin.slugg; }

        public static void Enable()
        {
            ft_2spear.spear_hooks();        // [ SPEAR ] calls the Spear hooks
            ft_deathSounds.death_hooks();   // [ DEATH ] calls the Death hooks
        }
    }
    public class ft_2spear
    {
        public static ManualLogSource Logger { get => welp.Plugin.Logger; }
        public static SlugcatStats.Name Slugg { get => welp.Plugin.slugg; }

        public static void spear_hooks()
        {
            On.Player.Grabability += spears;    // [ SPEAR ] allows Slugg to pick 2 spears in both of hands.
        }

        #region slugg_spearDealer

        public static Player.ObjectGrabability spears(On.Player.orig_Grabability orig, Player self, PhysicalObject obj)
        {
            // allows Slugg to get 2 spears with 2 hands
            if (self.SlugCatClass == Slugg)
            {
                if (obj is Spear)
                {
                    return (Player.ObjectGrabability)1;
                }
            }
            return orig(self, obj);
        }

        #endregion
    }
    public class ft_deathSounds
    {
        public static ManualLogSource Logger { get => welp.Plugin.Logger; }
        public static SlugcatStats.Name Slugg { get => welp.Plugin.slugg; }

        public static void death_hooks()
        {
            On.Player.Die += slugg_dies_illa;   // [ COSMETIC ] plays the random sound everytime in your death.
        }

        #region RANDOM DEATH SOUNDS

        /// <summary>
        /// Every time if some scug dies, will play a random sound. Cool, right?
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        public static void slugg_dies_illa(On.Player.orig_Die orig, Player self)
        {
            try
            {
                //variables.
                Room room = self.room;

                if (slugg_options.cb_deathNoises.Value == true && self.dead != true)
                {
                    room.PlaySound(DeathSounds.random_sound[UnityEngine.Random.Range(1, DeathSounds.random_sound.Length)], self.mainBodyChunk.pos);
                    room.AddObject(new ShockWave(self.mainBodyChunk.pos, 130f, 50f, 10, true));
                }

                orig(self);
            }
            catch (Exception ex)
            {
                Logger.LogError("player was slain by ExcreptionError. " + ex);
            }
        }

        #endregion
    }
}