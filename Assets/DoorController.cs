using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public RoomLayout roomLayoutLeft;
    public RoomLayout roomLayoutEnter;
    public LevelSpawner levelSpawner;
    public bool isVisible;
    public PlayerControls playerControls;
    public TurnState turnState;
    public int side = 0;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Player") && isVisible)
        {
            levelSpawner.ClearRoom();
            switch (side)
            {   
                case 1:
                    playerControls.transform.position = new Vector3(6.551f, -1.01f, 0f);
                    levelSpawner.entry = 3;
                    levelSpawner.positionX -= 1;
                    break;
                case 2:
                    playerControls.transform.position = new Vector3(-0.449f, -4.01f, 0f);
                    levelSpawner.entry = 4;
                    levelSpawner.positionY -= 1;
                    break;
                case 3:
                    playerControls.transform.position = new Vector3(-7.449f, -1f , 0f);
                    levelSpawner.entry = 1;
                    levelSpawner.positionX += 1;
                    break;
                case 4:
                    playerControls.transform.position = new Vector3(-1.449f, 2.99f, 0f);
                    levelSpawner.entry = 2;
                    levelSpawner.positionY += 1;
                    break;
                default:
                    break;
            }
            playerControls.movePoint.position = playerControls.transform.position;
            if (roomLayoutEnter != null)
            {
                roomLayoutEnter.spawnFromLayout();
            }
            else
            {
                for(int i = 0; i < 4; i++)
                { 
                    levelSpawner.spawnedDoors[i] = false;
                }
                switch(side)
                {
                    case 1:
                        levelSpawner.spawnedDoors[3] = true;
                        break;
                    case 2:
                        levelSpawner.spawnedDoors[4] = true;
                        break;
                    case 3:
                        levelSpawner.spawnedDoors[1] = true;
                        break;
                    case 4:
                        levelSpawner.spawnedDoors[2] = true;
                        break;
                }
                levelSpawner.spawnRoom();
            }
            isVisible = false;
            turnState.state = State.PLAYERTURN;

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isVisible == true)
        {
            this.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
