namespace RogueBall
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("RogueBall/Managers/Limits Manager")]
    public class LimitsManager : MonoBehaviour
    {
        [Header("Regen")]
        [SerializeField] bool regen = false;

        [Header("Walls")]
        [SerializeField] List<Transform> walls = new List<Transform>();

        Camera cam;

        float width;
        float height;

        void OnValidate()
        {
            //create and set walls
            if(regen)
            {
                regen = false;

                cam = Camera.main;
                walls.Clear();      //force recreate limits

                CreateLimits();
                SetLimits();
            }
        }

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
            //if there aren't 4 walls
            if(walls == null || walls.Count < 4)
            {
                //remove every child
                foreach (Transform child in transform)
                {
                    if (UnityEditor.EditorApplication.isPlaying)
                    {
                        Destroy(child.gameObject);
                    }
                    else
                    {
                        UnityEditor.EditorApplication.delayCall += () =>
                        {
                            DestroyImmediate(child.gameObject);
                        };
                    }
                }

                //reset list
                walls = new List<Transform>();

                //then create walls
                for (int i = 0; i < 4; i++)
                {
                    GameObject wall = new GameObject("Wall", typeof(BoxCollider2D), typeof(SpriteRenderer));
                    wall.transform.SetParent(transform);

                    walls.Add(wall.transform);
                }
            }
        }

        void SetLimits()
        {
            //get size of the walls
            float depthScreen = cam.WorldToViewportPoint(transform.position).z;
            Vector3 size = GetScale(depthScreen);

            float movementX = size.x / 2;
            float movementY = size.y / 2;

            CreateWall(walls[0], new Vector3(1, 0.5f, depthScreen), size, new Vector3(movementX, 0, 0));        //right
            CreateWall(walls[1], new Vector3(0, 0.5f, depthScreen), size, new Vector3(-movementX, 0, 0));       //left
            CreateWall(walls[2], new Vector3(0.5f, 1, depthScreen), size, new Vector3(0, movementY, 0));        //up
            CreateWall(walls[3], new Vector3(0.5f, 0, depthScreen), size, new Vector3(0, -movementY, 0));       //down
        }

        Vector3 GetScale(float depth)
        {
            //get size for the wall from the screen width and height
            Vector3 left = cam.ViewportToWorldPoint(new Vector3(0, 0, depth));
            Vector3 right = cam.ViewportToWorldPoint(new Vector3(2, 2, depth));

            Vector3 size = right - left;

            return new Vector3(size.x, size.y, 1);
        }

        void CreateWall(Transform wall, Vector3 viewportPoint, Vector3 size, Vector3 movement)
        {
            //move and set size
            wall.position = cam.ViewportToWorldPoint(viewportPoint);
            wall.localScale = size;
            wall.position += movement;
        }
    }
}