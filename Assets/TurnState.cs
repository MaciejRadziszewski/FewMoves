using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum State
{
    PLAYERTURN,
    ENEMYTURN
}

public class TurnState : MonoBehaviour
{
    public State state;
    public GameObject player;
    public LevelSpawner spawner;
    //public Text waveText;
    //public int wave = 1;
    public int bulletTime = 0;
    public List<GameObject> bullets = new List<GameObject>();
    public BulletController controller;

    public void menageBullets()
    {
        foreach (GameObject bullet in bullets)
        {
            controller = bullet.GetComponent<BulletController>();
            controller.movement();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        state = State.PLAYERTURN;
        //waveText.text = wave.ToString();
    }

    void Update()
    {
        if (spawner.enemies.Count == 0)
        {
            state = State.PLAYERTURN;
            spawner.layouts[0].GetComponent<RoomLayout>().spawnFromLayout();
        }

        if(state == State.ENEMYTURN)
        {
            spawner.moveEnemies();
            bulletTime++;
            if(bulletTime == 1)
            {
                menageBullets();
                bulletTime = 0;
            }
            state = State.PLAYERTURN;
        }
    }
}
