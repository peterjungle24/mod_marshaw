namespace pom_objects
{
    public class defualt_Data : ManagedData
    {
        //the custom fields are added as a parameter for the base class
        public defualt_Data(PlacedObject own) : base(own, null)
        {
            this.owner = own;
        }
    }
    public class defualt : UpdatableAndDeletable
    {
        PlacedObject self;
        ManualLogSource logger { get => Plugin.Logger; }

        public defualt(Room room, PlacedObject obj)
        {
            this.room = room;
            this.self = obj;
        }

    }
    public class defualt_REPR : ManagedRepresentation
    {
        public defualt_REPR(PlacedObject.Type type, ObjectsPage object_page, PlacedObject placed_object) : base(type, object_page, placed_object)
        {
        }
    }
}