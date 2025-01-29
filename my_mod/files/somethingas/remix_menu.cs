namespace remix_menu
{
    public class REMIX_menuses : OptionInterface
    {
        public static Configurable<bool> cb_deathNoises;   //IS STATIC
        public static Configurable<bool> cb_sanity_is_circle_or_bar;
        public static bool initialized; //if was initialized

        public REMIX_menuses()
        {
            //checkbox
            cb_deathNoises = config.Bind("cb_deathNoises", true, new ConfigurableInfo(Translate("disable/enable the death sounds for all the slugcats"), null, "", new object[] { }));
            cb_sanity_is_circle_or_bar = config.Bind("cb_sanity_is_circle_or_bar", false, new ConfigurableInfo(Translate("if checked, will be a CIRCLE. if unchecked, will be a BAR."), null, "", new object[] { }));
        }

        public override void Initialize()
        {
            try
            {
                base.Initialize();
                
                Debug.Log($"--------- {this} is initialized now. --------- ");

                Tabs = new OpTab[]
                {
                    new OpTab(this, Translate("Slugg") )
                    {
                        colorCanvas = Color.yellow
                    },
                    new OpTab(this, Translate("Sanity") )
                    {
                        colorCanvas = Color.gray
                    }

                };

                UIelement[] Tab0_Array = new UIelement[]        //array of elements
                {
                    new OpLabel(10f, 550f, Translate("Cosmetic ones"), true),       //creates a big text
                    new OpCheckBox(cb_deathNoises, new Vector2(10f, 480))                      //creates a checkbox....
                    {
                        description = cb_deathNoises.info.description,
                        colorEdge = Color.yellow,
                    },
                    new OpLabel(60f, 480f, Translate("Enable the Death Random Sounds for all the slugcats")),   //creats a text
                };
                UIelement[] Tab1_Array = new UIelement[]        //array of elements
                {
                    new OpLabel(10f, 550f, Translate("its Bar or Circle?"), true),       //creates a big text
                    new OpCheckBox(cb_sanity_is_circle_or_bar, new Vector2(10f, 480) )                      //creates a checkbox....
                    {
                        description = cb_sanity_is_circle_or_bar.info.description,
                        colorEdge = Color.gray,
                    },
                };


                Tabs[0].AddItems(Tab0_Array); //adds the elements to the tab [ SLUGG ]
                Tabs[1].AddItems(Tab1_Array); //adds the elements to the tab [ SANITY ]
            }
            catch (Exception eu)
            {
                Debug.Log($">: Marshaw/SluggOptions/Initialize :<");
                Debug.Log(eu);

                //nah
                //i remember i deleted somethings about
                //none of excreption
            }
        }
    }
}