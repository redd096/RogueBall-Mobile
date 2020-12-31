namespace redd096
{
    using UnityEngine;

    [AddComponentMenu("redd096/Singletons/Game Manager")]
    public class GameManager : Singleton<GameManager>
    {
        public MapManager mapManager { get; private set; }

        protected override void SetDefaults()
        {
            //get references
            mapManager = FindObjectOfType<MapManager>();
        }
    }
}