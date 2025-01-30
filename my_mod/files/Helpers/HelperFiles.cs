namespace welp
{
    public static class distance_checker
    {
        /// <summary>
        /// Finds all objects belonging to a room instance at or within a given distance_checker from an origin point
        /// </summary>
        /// <param name="room">The room to check</param>
        /// <param name="origin">The point to compare the distance_checker to</param>
        /// <param name="distanceThreshold">The desired distance_checker to the origin point</param>
        /// <returns>All results that match the specified conditions</returns>
        public static IEnumerable<PhysicalObject> FindObjectsNearby(this Room room, Vector2 origin, float distanceThreshold)
        {
            if (room == null) return new PhysicalObject[0]; //Null data returns an critical set
            return room.GetAllObjects().Where(o => Custom.Dist(origin, o.firstChunk.pos) <= distanceThreshold);
        }
        /// <summary>
        /// Finds all objects belonging to a room instance at or within a given distance_checker from an origin point that are of the type T
        /// </summary>
        /// <param name="room">The room to check</param>
        /// <param name="origin">The point to compare the distance_checker to</param>
        /// <param name="distanceThreshold">The desired distance_checker to the origin point</param>
        /// <returns>All results that match the specified conditions</returns>
        public static IEnumerable<PhysicalObject> FindObjectsNearby<T>(this Room room, Vector2 origin, float distanceThreshold) where T : PhysicalObject
        {
            if (room == null) return new PhysicalObject[0]; //Null data returns an critical set

            return room.GetAllObjects().OfType<T>().Where(o => RWCustom.Custom.Dist(origin, o.firstChunk.pos) <= distanceThreshold);
        }
        /// <summary>
        /// Returns all objects belonging to a room instance
        /// </summary>
        /// <param name="room">The room to check</param>
        public static IEnumerable<PhysicalObject> GetAllObjects(this Room room)
        {
            if (room == null) yield break; //Null data returns an critical set

            for (int m = 0; m < room.physicalObjects.Length; m++)
            {
                for (int n = 0; n < room.physicalObjects[m].Count; n++)
                {
                    yield return room.physicalObjects[m][n]; //Returns the objects one at a time
                }
            }
        }
    }
    public static class inputs
    {
        /// <summary>
        /// while you holds the button
        /// </summary>
        /// <param name="key"></param>
        public static bool keyboard_check(KeyCode key)
        {
            return Input.GetKey(key);
        }
        /// <summary>
        /// while you press the button
        /// </summary>
        /// <param name="key"></param>
        public static bool keyboard_check_down(KeyCode key)
        {
            return Input.GetKeyDown(key);
        }
        /// <summary>
        /// while you release the button
        /// </summary>
        /// <param name="key"></param>
        public static bool keyboard_check_up(KeyCode key)
        {
            return Input.GetKeyUp(key);
        }
    }
    public class path_help
    {
        public static string GetMedallionFile(string medallion)
        {
            return Path.Combine("sprites", "colletables", medallion);
        }
        public static string GetAtlasFile(string atlas)
        {
            return Path.Combine("sprites", "atlas", atlas);
        }
        public static string GetFile(string file)
        {
            return AssetManager.ResolveFilePath(file);
        }
    }
}