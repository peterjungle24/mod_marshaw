namespace pom_objects
{
    public class HL2_Data : ManagedData
    {
        //the custom fields are added as a parameter for the base class
        public HL2_Data(PlacedObject own) : base(own, null)
        {
            this.owner = own;
        }
    }
    public class HL2 : UpdatableAndDeletable , IDrawable
    {
        //FAtlas hl2_logo;
        PlacedObject self;
        ManualLogSource logger { get => Plugin.Logger; }

        public HL2(Room room, PlacedObject obj)
        {
            //hl2_logo = Futile.atlasManager.LoadImage(Path.Combine("sprites", "HL2_logo"));
        }

        /*     IDRAWABLE        */
        public void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            /// InitiateSprites is when the element is initialized
            /// Is called once

            //  STEPS TO IDRAWABLE -- InitiateSprites
            // Create a array of sprites
            sLeaser.sprites = new FSprite[1];

            // Set the array of sprite (1) ito the sprite
            sLeaser.sprites[0] = new FSprite(Path.Combine("sprites", "HL2_logo"), true);

            // Set the color to White, if the sprite is colored
            sLeaser.sprites[0].color = Color.white;

            // Edit
            //sLeaser.sprites[0].imagescale = 2.2f;

            // Add the thing to the Container
            AddToContainer(sLeaser, rCam, null);
        }
        public void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float Float, Vector2 camPos)
        {
            // sets the x and y of the image
            sLeaser.sprites[0].x = self.pos.x - rCam.pos.x;
            sLeaser.sprites[0].y = self.pos.y - rCam.pos.y;

            // set the sprite scale
            sLeaser.sprites[0].scale = 1f;
        }
        public void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette pal)
        {
            
        }
        public void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer fContainer)
        {
            fContainer ??= rCam.ReturnFContainer("Items");

            foreach (FSprite fsprite in sLeaser.sprites)
            {
                fContainer.AddChild(fsprite);
            }
        }
    }
    public class HL2_REPR : ManagedRepresentation
    {
        public HL2_REPR(PlacedObject.Type type, ObjectsPage object_page, PlacedObject placed_object) : base(type, object_page, placed_object)
        {
        }
    }
}

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