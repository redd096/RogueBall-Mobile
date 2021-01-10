namespace RogueBall
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("RogueBall/Managers/Limits Manager")]
    public class LimitsManager : MonoBehaviour
    {
        Camera cam;

        List<GameObject> walls = new List<GameObject>();

        float width;
        float height;

        void Start()
        {
            cam = Camera.main;

            //create walls
            CreateLimits();
        }

        void Update()
        {
            //if different size, reset limits
            if(Screen.width != width || Screen.height != height)
            {
                width = Screen.width;
                height = Screen.height;

                SetLimits();
            }
        }

        void CreateLimits()
        {
            //create walls
            for(int i = 0; i < 4; i++)
            {
                GameObject wall = new GameObject("Wall", typeof(BoxCollider2D));
                wall.transform.SetParent(transform);

                walls.Add(wall);
            }
        }

        void SetLimits()
        {
            //get size of the walls
            float depthScreen = cam.WorldToViewportPoint(transform.position).z;
            Vector3 size = GetScale(depthScreen);

            float movementX = size.x / 2;
            float movementY = size.y / 2;

            CreateWall(walls[0], new Vector3(1, 0.5f, depthScreen), size, new Vector3(movementX, 0, 0));      //right
            CreateWall(walls[1], new Vector3(0, 0.5f, depthScreen), size, new Vector3(-movementX, 0, 0));      //left
            CreateWall(walls[2], new Vector3(0.5f, 1, depthScreen), size, new Vector3(0, movementY, 0));         //up
            CreateWall(walls[3], new Vector3(0.5f, 0, depthScreen), size, new Vector3(0, -movementY, 0));      //down
        }

        Vector3 GetScale(float depth)
        {
            //get size for the wall from the screen width and height
            Vector3 left = cam.ViewportToWorldPoint(new Vector3(0, 0, depth));
            Vector3 right = cam.ViewportToWorldPoint(new Vector3(2, 2, depth));

            Vector3 size = right - left;

            return new Vector3(size.x, size.y, 1);
        }

        GameObject CreateWall(GameObject wall, Vector3 viewportPoint, Vector3 size, Vector3 movement)
        {
            //move and set size
            wall.transform.position = cam.ViewportToWorldPoint(viewportPoint);
            wall.transform.localScale = size;
            wall.transform.position += movement;

            return wall;
        }
    }
}