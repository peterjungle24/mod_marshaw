namespace shader_manage
{
    public static class gui_flags
    {
        public static bool sanity_is_bar_or_circle;
    }
    public class shaders
    {
        public static string HoldButtonCircle   = "HoldButtonCircle";
        public static string VectorCircle       = "VectorCircle";
        public static string GhostDisto         = "GhostDisto";
    }
    public class sanity_bar_aqctually_a_sanity_bar
    {
        public static FSprite newsprite = new FSprite("Futile_White");
        public static ManualLogSource Logger { get => Plugin.Logger; }

        public static void add_bar(RoomCamera self, Player player)
        {
            if (gui_flags.sanity_is_bar_or_circle == false)
            {
                newsprite.height = 4;
                newsprite.color = Color.white;
            }
        }
        public static void add_to_GUI(RoomCamera rcam)
        {
            rcam.ReturnFContainer("HUD").AddChild(newsprite);    //put the sprite in a HUD layer
        }
        public static void delete_of_GUI(RoomCamera rcam)
        {
            rcam.ReturnFContainer("HUD").RemoveChild(newsprite);    //put the sprite in a HUD layer
        }

        public static void initialize()
        {
            newsprite.anchorX = 0;
            newsprite.y = 672;
            newsprite.x = 1236;
            newsprite.width = 200f;
        }
    }
    public class cooldown_bar
    {
        public static FSprite sprite = new("Futile_White");
        public static ManualLogSource log { get => Plugin.Logger; }

        #region cooldownBar add

        public static void cooldownBar_Add(RoomCamera self, Player player)
        {
            try
            { 
                //create circles
                Vector2 campos = self.CamPos(0);
                var cwt = player.Skill();

                if (cwt.HasStealthMedallion)
                {
                    self.ReturnFContainer("HUD").AddChild(sprite);    //put the sprite in a HUD layer
                    sprite.shader = self.game.rainWorld.Shaders[shaders.HoldButtonCircle];  //sets the shader to that circle
                    sprite.scale = 2f;    //
                    sprite.y = player.bodyChunks[0].pos.y - campos.y;        //
                    sprite.x = player.bodyChunks[0].pos.x - campos.x;        //
                    sprite.color = Color.yellow;

                    if (cwt.stealthCooldown.is_equal)
                    {
                        sprite.alpha = 0f;
                    }
                    else
                    {
                        sprite.alpha += 0.010f;
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError("{ shader_manage/cooldownBar_add() } Some error was occured.");
                log.LogError(ex);
            }
        }

        #endregion
    }
    public class corners
    {
        public static ManualLogSource log { get => Plugin.Logger; }
        public static FSprite A         = new("Futile_White");
        public static FSprite B         = new("Futile_White");
        public static FSprite C         = new("Futile_White");
        public static FSprite D         = new("Futile_White");
        public static FSprite rect      = new("Futile_White");

        public static void corners_add(RoomCamera self, Player player)
        {
            // create the blue rect
            self.ReturnFContainer("Foreground").AddChild(rect);

            // blue rectanngle settings
            rect.x = 300f;
            rect.y = 300f;
            rect.color = Color.blue;
            rect.width = 100f;
            rect.height = 100f;

            // Vector2
            Vector2 top_left = new Vector2(rect.x - rect.width / 2, rect.y + rect.height / 2);  // math side for the Top Left
            Vector2 top_right = new Vector2(rect.x + rect.width / 2, rect.y + rect.height / 2); // math side for the Top Right
            Vector2 bot_left = new Vector2(rect.x - rect.width / 2, rect.y - rect.height / 2);  // math side for the Bottom Left
            Vector2 bot_right = new Vector2(rect.x + rect.width / 2, rect.y - rect.height / 2); // math side for the Bottom Right
            Vector2 pos = player.mainBodyChunk.pos - self.pos;                                  // the player position
            Vector2 camera_pos = self.pos;                                                      // still unused due indecision

            // floats
            float top = top_left.y;
            float bottom = bot_left.y;
            float left = top_left.x;
            float right = top_right.x;

            bool x_inside = pos.x > left && pos.x < right;  // checks if is inside of the point on horizontal
            bool y_inside = pos.y > bottom && pos.x < top;  // checks if is inside of the point on vertical

            // compares the position of the player with the left, right, top, and bottom using corners as reference
            if (x_inside && y_inside)
            {
                // creates the effect when inside the rect
                player.room.AddObject(new Explosion.ExplosionLight(player.bodyChunks[1].pos, 120f, 40, 10, Color.green));
            }
            
            // oh for the love of god please ITS SO WEIRD SEEING THIS CODE OF THE CORNERS BEING COVERED BY THE GREEN CUBES

            self.ReturnFContainer("HUD").AddChild(A);
            A.x = top_left.x;
            A.y = top_left.y;
            A.width = 10f;
            A.height = 10f;
            A.color = Color.green;

            self.ReturnFContainer("HUD").AddChild(B);
            B.x = top_right.x;
            B.y = top_right.y;
            B.width = 10f;
            B.height = 10f;
            B.color = Color.green;

            self.ReturnFContainer("HUD").AddChild(C);
            C.x = bot_left.x;
            C.y = bot_left.y;
            C.width = 10f;
            C.height = 10f;
            C.color = Color.green;

            self.ReturnFContainer("HUD").AddChild(D);
            D.x = bot_right.x;
            D.y = bot_right.y;
            D.width = 10f;
            D.height = 10f;
            D.color = Color.green;
        }
    }
}

/*

public static void corners_add(RoomCamera self, Player player)
        {
            self.ReturnFContainer("Foreground").AddChild(rect);

            rect.x = 300f;
            rect.y = 300f;
            rect.color = Color.blue;
            rect.width = 100f;
            rect.height = 100f;

            self.ReturnFContainer("HUD").AddChild(sprite);

            sprite.color = Color.red;
            sprite.width = 10f;
            sprite.height = 10f;

            sprite.x = rect.x;
            sprite.y = rect.y;

            // Vector2
            Vector2 top_left    = new Vector2(rect.x - rect.width / 2, rect.y + rect.height / 2);
            Vector2 top_right   = new Vector2(rect.x + rect.width / 2, rect.y + rect.height / 2);
            Vector2 bot_left    = new Vector2(rect.x - rect.width / 2, rect.y + rect.height / 2);
            Vector2 bot_right   = new Vector2(rect.x + rect.width / 2, rect.y - rect.height / 2);
            Vector2 pos         = player.mainBodyChunk.pos;

            // these operator order is so confusing
            if ( pos.x > top_left.x && pos.x < top_right.x && pos.y < bot_left.y && pos.y > bot_right.y )
            {
                // creates the effect when inside.
                // i am a flaw in the math
                player.room.AddObject(new Explosion.ExplosionLight(player.bodyChunks[1].pos, 120f, 40, 10, Color.green));
            }
        }

 */