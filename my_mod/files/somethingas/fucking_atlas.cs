namespace atlases
{
    public class fucking_atlas
    {
        private static FAtlas atlas_eyes;

        public static ManualLogSource log { get => Plugin.Logger; }

        public static void atlas_hooks()
        {
            On.RainWorld.Awake += init_atlas_files;
        }

        private static void init_atlas_files(On.RainWorld.orig_Awake orig, RainWorld self)
        {
            var eyes_path = path_help.GetAtlasFile("atlas_eye_test");
            atlas_eyes = Futile.atlasManager.LoadAtlas(eyes_path);

            if (atlas_eyes != null)
            {
                log.LogMessage("Its loaded?\n");
            }

            orig(self);
        }
    }
}