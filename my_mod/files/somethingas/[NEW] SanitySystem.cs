namespace sanity
{
    public class SanitySystem
    {
        public static SlugcatStats.Name marshaw { get => Plugin.Marshaw; }    //name of my slugcat
        public static ManualLogSource Logger { get => Plugin.Logger; }
        public static Dictionary<CreatureTemplate.Type, float> crit_dict = new Dictionary<CreatureTemplate.Type, float>();
        public static List<CreatureTemplate.Type> friendly_creature_types = new List<CreatureTemplate.Type>();
        public static float lastThreat = 0f;  //last threat;

        #region Sanity while Active

        /// <summary>
        /// A method that manages things for GUI
        /// </summary>
        public static void GraphicManage(Player self)
        {
            // the factor of Alpha
            float alphaFactor = 0.01f;
            // the factor of Move
            float move_factor = 0.50f;

            // if this scug its Marshaw
            if (self.slugcatStats.name == marshaw)
            {
                // add|appear|control
                SanityGraphics.AddGUI(self.room.game.cameras[0], self);
                SanityGraphics.AddChild(self.room.game.cameras[0]);
                SanityGraphics.EffectController(self.room.game.cameras[0]);

                // change alpha
                if (Input.GetKey(KeyCode.W))
                {
                    SanityGraphics.graphic.alpha += alphaFactor;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    SanityGraphics.graphic.alpha -= alphaFactor;
                }

                // change size
                if (Input.GetKey(KeyCode.N))
                {
                    SanityGraphics.graphic.scaleX += 0.25f;
                    SanityGraphics.graphic.scaleY += 0.25f;
                }
                if (Input.GetKey(KeyCode.M))
                {
                    SanityGraphics.graphic.scaleX -= 0.25f;
                    SanityGraphics.graphic.scaleY -= 0.25f;
                }

                // move bar
                if (Input.GetKey(KeyCode.Keypad2))
                {
                    SanityGraphics.graphic.y += move_factor;
                }
                else if (Input.GetKey(KeyCode.Keypad4))
                {
                    SanityGraphics.graphic.x += move_factor;
                }
                else if (Input.GetKey(KeyCode.Keypad8))
                {
                    SanityGraphics.graphic.y -= move_factor;
                }
                else if (Input.GetKey(KeyCode.Keypad6))
                {
                    SanityGraphics.graphic.x -= move_factor;
                }
            }
        }

        #endregion

        #region Different Values by Creatures

        /// <summary>
        /// Dictionary of the creatures that recover or DESTROY your sanity. (literal)
        /// </summary>
        public static void crit_dict_values()
        {
            //hmmmm
            /// - check the dictionary to get a valid Svalue from it.
            /// - otherwise you check if it's in the friendly list.
            /// - any other situation, you apply the default

            // Dict
            crit_dict[CreatureTemplate.Type.Scavenger] = 0.0035f;
            crit_dict[CreatureTemplate.Type.LizardTemplate] = 0.0022f;
            crit_dict[CreatureTemplate.Type.Vulture] = 0.0025f;
            crit_dict[CreatureTemplate.Type.Centipede] = 0.0040f;
            crit_dict[CreatureTemplate.Type.PoleMimic] = 0.0015f;
            crit_dict[CreatureTemplate.Type.Centiwing] = 0.0045f;
            crit_dict[CreatureTemplate.Type.SmallCentipede] = 0.0020f;
            crit_dict[CreatureTemplate.Type.RedCentipede] = 0.0050f;

            // List
            friendly_creature_types.Add(CreatureTemplate.Type.Fly);
            friendly_creature_types.Add(CreatureTemplate.Type.CicadaA);
            friendly_creature_types.Add(CreatureTemplate.Type.CicadaB);
        }

        #endregion
        #region Sanity Calculation

        /// <summary>
        /// Calculates everything.
        /// </summary>
        public static void SanityCalculate(Player self)
        {
            // the acumulative
            float accumulative = 0f;
            // the Player current room
            Room room = self.room;
            // current position
            Vector2 position = self.mainBodyChunk.pos;

            foreach (var obj in room.FindObjectsNearby<Creature>(position, 120f) )
            {
                if (obj != self && obj is Creature creature)
                {
                    //template.
                    var template = creature.Template.type;
                    //ancestor
                    var ancestor = creature.Template.ancestor;
                    //Calculates the distance_checker between a creature and the _player
                    var dist = (creature.mainBodyChunk.pos - self.mainBodyChunk.pos).magnitude;

                    //if is NOT dead
                    if (creature.dead == false)
                    {
                        //if the distance_checker its below than 120f
                        if (dist <= 120f)
                        {
                            //Get the sanity Svalue for this creature
                            accumulative += get_sanity_value(creature.Template);
                        }
                    }
                    else
                    {
                        //otherwise, does nothing
                        continue;
                    }
                }
            }

            ///------------------------------------------------------
            ////END OF THE FOREACH LOOP
            ///------------------------------------------------------

            //if creature its in the distance_checker, will have the [ threat ] flag
            bool threat = accumulative > 0;
            // i dont remember
            float idwtwton = 0.0015f;
            // a timer
            float timer = 100f;

            // subtract alpha with accumulative
            SanityGraphics.graphic.alpha -= accumulative;

            // if its not threatm and its not dead or not conscious
            if (!threat && (!self.dead || !self.Consious) )
            {
                if (lastThreat >= timer)
                {
                    SanityGraphics.graphic.alpha += idwtwton;
                }

                lastThreat += 1f;
            }
            else
            {
                lastThreat = 0f;
            }

            // if the alpha is less than 0.10f
            if (SanityGraphics.graphic.alpha <= 0.10f)
            {
                // add my ported effect from Lizard to Player :monksilly:
                room.AddObject(new PlayerBubbles(self, 1f, 1f, 1f, new Color(255, 90, 0) ) );
                // eyes closed
                self.Blink(5);
            }
        }

        #endregion
        #region The Values

        public static float get_sanity_value(CreatureTemplate crit)
        {
            //Constants variables CAN'T BE CHANGED.
            const float def = 0.0005f;  //Default Svalue.
            const float friendly_regen = 0.0035f;   //Sanity change may be positive or negative

            if (crit == null)
            {
                return def;
            }

            //....i think i already defined
            var crit_type = crit.type;                  //type
            var crit_ancestor = crit.ancestor;     //ancestors
            float value;                                //Svalue

            ///_______________________________________________________
            ///             MedalType
            ///_______________________________________________________

            //check the crit_type
            if (crit_dict.TryGetValue(crit_type, out value) )
            {
                return value;
            }

            //check the List
            if (friendly_creature_types.Contains(crit_type) )
            {
                return -friendly_regen; //creature restores sanity of _player when it is nearby
            }

            ///_______________________________________________________
            ///             Ancestor
            ///_______________________________________________________


            if (crit_ancestor != null && crit_ancestor.type != null)      //if ancestor its NOT null
            {
                //check the ancestor
                if (crit_dict.TryGetValue(crit_ancestor.type, out value))
                {
                    return value;
                }
                if (friendly_creature_types.Contains(crit_ancestor.type))
                {
                    return -friendly_regen; //creature restores sanity of _player when it is nearby
                }
            }

            return def;
        }

        #endregion
        #region Reset the Bar

        /// <summary>
        /// Resets the sanity bar when the cycle pass..
        /// </summary>
        public static void ResetSanity(On.SaveState.orig_SessionEnded orig, SaveState self, RainWorldGame game, bool survived, bool newMalnourished)
        {
            lastThreat = 0f;
            SanityGraphics.graphic.alpha = 1f;

            orig(self, game, survived, newMalnourished);
        }

        #endregion
    }
    public class SanityGraphics
    {
        public static SlugcatStats.Name scug_marshaw { get => Plugin.Marshaw; }
        public static FSprite graphic = new FSprite("Futile_White");
        public static ManualLogSource log { get => Plugin.Logger; }
        public static bool isCritical { get => graphic.alpha <= 0.25f; }

        #region init

        /// <summary>
        /// Initialize method that initialize.
        /// </summary>
        public static void Initialize()
        {
            // initialize the size
            graphic.scaleX = 10f;
            graphic.scaleY = 10f;
            // initialize the coordinates
            graphic.y = 668f;
            graphic.x = 1234;
            // initialize the color
            graphic.color = Color.white;
        }

        #endregion
        #region Add to GUI

        /// <summary>
        /// add a GUI of Sanity graphics to the game
        /// </summary>
        public static void AddGUI(RoomCamera self, Player player)
        {
            try
            {
                // set the shader
                graphic.shader = self.game.rainWorld.Shaders[shaders.HoldButtonCircle];

                // if player its Marshaw
                if (player.slugcatStats.name == scug_marshaw)
                {
                    // circle is visible
                    graphic.isVisible = true;
                }
                else
                {
                    // remove child from HUD
                    self.ReturnFContainer("HUD").RemoveChild(graphic);
                }

                // if thats critical
                if (isCritical == true)
                {
                    // randomize the moviment, like, it shakes
                    graphic.x = Range(1234f, 1245f);
                    graphic.y = Range(668f, 679f);
                }
            }
            catch (Exception ex)
            {
                log.LogError("{ shader_manage/sanityBar_add() } Some error was occured.");
                log.LogError(ex);
            }
        }

        #endregion
        #region Lerping Effect

        /// <summary>
        /// a effect that adds while the alpha/length its getting lower
        /// </summary>
        public static void EffectController(RoomCamera rcam)
        {
            // increases desaturation, and a little bit of farkness
            rcam.effect_desaturation = Mathf.InverseLerp(1f, 0.10f, graphic.alpha);
            rcam.effect_darkness = (1f - graphic.alpha) * 0.25f;
        }

        #endregion
        #region Add and Remove Sanity Graphic

        public static void AddChild(RoomCamera rcam)
        {
            rcam.ReturnFContainer("HUD").AddChild(graphic);    //put the sprite in a HUD layer
        }
        public static void RemoveChild(RoomCamera rcam)
        {
            rcam.ReturnFContainer("HUD").RemoveChild(graphic);    //put the sprite in a HUD layer
        }

        #endregion
    }
}