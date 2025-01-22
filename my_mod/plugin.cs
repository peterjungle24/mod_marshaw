//self.room.game.cameras[0]
namespace welp // @sl_objects of the space init
{
    [BepInPlugin(PLUGIN_ID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_ID = "grey.grey.grey.grey";        //the ID for the my mod in [ modinfo.json]
        public const string PLUGIN_NAME = "Marshawwwwwwwwwwwww";        //mthe @sl_objects for my mod in [ modinfo.json]
        public const string PLUGIN_VERSION = "0.1.1";                      //the version for my mod in [ modinfo.json]

        public static readonly SlugcatStats.Name Marshaw = new SlugcatStats.Name("marshaw");    //@sl_objects of the Marshaw
        public static readonly SlugcatStats.Name slugg = new SlugcatStats.Name("slugg_the_scug");    //@sl_objects of the Slugg
        public static new ManualLogSource Logger { get; private set; }                          //for logs
        private OptionInterface options;                        //options for the remix menu ones LESS GOOOOOOO
        public static slugg_options slugg_options_tab;           //options for the remix menu ones LESS GOOOOOOO
        public static Trigger trg;

        // Custom Features to the "marshaw.json"

        //Add hooks to the hooks for the mod work bc the codes mod can't run without hooks
        public void OnEnable()
        {
            //sanity_bar_aqctually_a_sanity_bar.newsprite.width = 200;

            Logger = base.Logger;                                                           //for the log base
            pom_objects();                                                                  //register POM objects
            On.RainWorld.Update += UpdateTimerFrames;                                       //update the timer frames for the Timer helper
            On.Room.AddObject += Medallion.add;                                             //add the meddallions to the 'Room.Loaded' 

            //please, dont enter on this site, you will see my favourite cyan lizard ;-;
            //https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQTK8R7tsGQsYuwrsrv6VRIbSgcOI9rr1OZ0w&s

            //////// Marshaw
            marshaw_features.hooks.Enable();        //call Marsahaw hooks
            On.RainWorld.OnModsInit += init;        //[ INIT ] do Updat when the mod is initialized

            //////// Marshaw - MEDALLION HOOKS
            marshaw_features.hooks.Enable();        //call Skills hooks

            //////// slugg
            slugg_features.hooks.Enable();          //call Slugg hooks

            //////// objects
            FireBall.OnHooks();

            /////// RoomScripts

            //////// test
            On.Player.Update += fireball_collision;
            On.Room.AddObject += i_added_this_hook;
        }

        private void i_added_this_hook(On.Room.orig_AddObject orig, Room self, UpdatableAndDeletable obj)
        {
            var cwt = self.issue();
            var tr = obj as Trigger;

            if (tr != null)
            {
                cwt.triggers.Add(tr);
            }

            orig(self, obj);
        }
        private void fireball_collision(On.Player.orig_Update orig, Player self, bool eu)
        {
            // Fuckin To Do: tries to get a Rect or Circle or idk what shape, checks if player is touching on it, and THEN
            // do something when touchs.
            // is for Fireball code, so, i need create it from a specific key meanwhile the fire medallion is on
            // Adding one more comment line for 4 TO DO comments instead of 3
            Room room = self.room;
            Vector2 position = self.mainBodyChunk.pos;

            foreach (FireBall ball in room.FindObjectsNearby<FireBall>(position, 120f))  //checks for the distance_checker
            {
                // Radius is 16.... i guess
                // like, its literally [ width / 2 ], so, it would be 32 originally i guess

                var distance = (ball.position - self.mainBodyChunk.pos).magnitude;

                // if the player is touching on the distance_checker
                if (distance <= 120f)
                {
                    //room.AddObject(new Explosion.ExplosionLight(self.mainBodyChunk.pos, 20f, 1f, 30, Color.white));
                }
            }

            orig(self, eu);
        }

        #region UpdateTimerFrames

        private void UpdateTimerFrames(On.RainWorld.orig_Update orig, RainWorld self)
        {
            timer_manage.ApplyHooks();
            orig(self);
        }
        #endregion
        #region POM objects

        /// <summary>
        /// add POM objects
        /// </summary>
        private void pom_objects()
        {
            var others = "Slugg others";
            var misc = "Slugg misc";

            // triggers

            //register the sl_objects.
            RegisterManagedObject<Trigger, Trigger_data, Trigger_REPR>("Thing Trigger", others, true);

            //register the meed
            Medallion.medal_POM();

            // miscs

            //register the misc
            RegisterManagedObject<UpsideDown, UpsideDown_Data, UpsideDown_REPR>("Upside Down gravity", misc, true);
            RegisterManagedObject<defualt, defualt_Data, defualt_REPR>("defualt object. does nothing", misc, true);
            RegisterManagedObject<HL2, HL2_Data, HL2_REPR>("HALF LIFE 2 LETS GOOOOOOOOOOOOOOOOOO", misc, true);
        }

        #endregion
        #region init

        /// <summary>
        /// log this text every time when the mod initialize. Requires 10 or 20 QI in Modding for understand
        /// </summary>
        /// <param @object="orig"> original code </param>
        /// <param @object="self"> _player </param>
        private void init(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            orig(self);

            try
            {
                Logger.LogInfo("THIS IS THE GIT BUILD");

                if (ModManager.MSC)
                {
                    Logger.LogInfo(">>MSC Is Enabled!<<");
                }

                //ASSIGN FIELDS / CREATE VARIABLES

                //CREATE INSTANCES
                slugg_options_tab = new slugg_options();

                //REGISTER
                sanity.sanity_bar.crit_dict_values();

                //SOUNDS
                sounded.DeathSounds.DeathSound_Init();
                sounded.CustomSFX.CustomSFX_Init();

                //MEDALLIONS
                Medallion.medal_init();

                //INIT REMIX MENU INTERFACES
                MachineConnector.SetRegisteredOI(PLUGIN_ID, slugg_options_tab);

                slugg_options_tab.Initialize();
            }
            catch (Exception ex)
            {
                Logger.LogError("crashed lol");
                Logger.LogError(ex);
            }
        }

        #endregion
    }
}

#region Credits

// FluffBall
// Nuclear Pasta
// Thalber
// BensoneWhite
// Magica
// slime_cubed
// NaCio
// Alduris
// luna ☾fallen/endspeaker☽
// Pocky(Burnout/Forager/Siren)
// Elliot (Solace's creator)
// IWannaPresents
// Rose
// doppelkeks
// Tat011
// Human Resource
// @verityoffaith
// dogcat
// hootis (always ping pls)
// Tuko (bc for my region in first time)
// Ethan Barron
// Bro
// Orinaari (kiki the Scugs)
// Nope
// AT
// GreatestGrasshopper
// StormTheCat (Slugpup Safari Dev)

#endregion
/*  IDrawable


public void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
{ }
public void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float Float, Vector2 camPos)
{ }
public void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette pal)
{ }
public void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer fContainer)
{ }

*/