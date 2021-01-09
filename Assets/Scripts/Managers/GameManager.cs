namespace redd096
{
    using UnityEngine;
    using RogueBall;

    [AddComponentMenu("redd096/Singletons/Game Manager")]
    public class GameManager : Singleton<GameManager>
    {
        public MapManager mapManager { get; private set; }
        public Player player { get; private set; }

        protected override void SetDefaults()
        {
            //get references
            mapManager = FindObjectOfType<MapManager>();
            player = FindObjectOfType<Player>();
        }
    }
}