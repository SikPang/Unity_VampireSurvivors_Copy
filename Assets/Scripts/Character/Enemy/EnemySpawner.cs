using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] TextMeshProUGUI killCountText;
    static EnemySpawner instance;
    List<GameObject> enemyList = new List<GameObject>(500);
    const float maxX = 10;
    const float maxY = 16;
    const float DecreseSpawnDelayTime = 0.8f;
    float spawnDelay;
    int stage;
    int killCount;

    private EnemySpawner() { }
    
    enum Direction
    {
        North,
        South,
        West,
        East
    }

    void Awake()
    {
        Initialize();
    }

    void Start()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(listChecker());
    }

    void Initialize()
    {
        instance = this;
        spawnDelay = 0.5f;
        stage = 1;
        killCount = 0;
    }

    IEnumerator SpawnEnemy()
    {
        GameObject newEnemy;

        while (true)
        {
            switch (stage)
            {
                default:
                case 1:
                    newEnemy = ObjectPooling.GetObject(CharacterData.CharacterType.FlyingEye);
                    break;
                case 2:
                    newEnemy = ObjectPooling.GetObject(CharacterData.CharacterType.Goblin);
                    break;
                case 3:
                    newEnemy = ObjectPooling.GetObject(CharacterData.CharacterType.Mushroom);
                    break;
                case 4:
                case 5:
                    newEnemy = ObjectPooling.GetObject(CharacterData.CharacterType.Skeleton);
                    break;
            }

            newEnemy.transform.position = RandomPosition();
            newEnemy.SetActive(true);
            enemyList.Add(newEnemy);

            if(stage == 5)
            {
                newEnemy = ObjectPooling.GetObject(CharacterData.CharacterType.FlyingEye);
                newEnemy.transform.position = RandomPosition();
                newEnemy.SetActive(true);
                enemyList.Add(newEnemy);
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    Vector3 RandomPosition()
    {
        Vector3 pos = new Vector3();

        Direction direction = (Direction)Random.Range(0, 4);

        switch (direction)
        {
            case Direction.North:
                pos.x = Random.Range(player.transform.position.x - maxX, player.transform.position.x + maxX);
                pos.y = player.transform.position.y + 10f;
                break;
            case Direction.South:
                pos.x = Random.Range(player.transform.position.x - maxX, player.transform.position.x + maxX);
                pos.y = player.transform.position.y - 10f;
                break;
            case Direction.West:
                pos.x = player.transform.position.x - 16f;
                pos.y = Random.Range(player.transform.position.y - maxY, player.transform.position.y + maxY);
                break;
            case Direction.East:
                pos.x = player.transform.position.x + 15f;
                pos.y = Random.Range(player.transform.position.y - maxY, player.transform.position.y + maxY);
                break;
        }

        return pos;
    }

    public Vector2 GetNearestEnemyPosition()
    {
        float[] min = {0, int.MaxValue};

        for(int i = 0; i < enemyList.Count; i++) { 
            if(min[1] > (enemyList[i].transform.position - Player.GetInstance().GetPosition()).sqrMagnitude)
            {
                min[0] = i;
                min[1] = (enemyList[i].transform.position - Player.GetInstance().GetPosition()).sqrMagnitude;
            }
        }

        return enemyList[(int)min[0]].transform.position;
    }

    public Vector2 GetRandomEnemyPosition()
    {
        int random = Random.Range(0, enemyList.Count);

        return enemyList[random].transform.position;
    }

    IEnumerator listChecker()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            for(int i=0; i<enemyList.Count; i++)
            {
                if(!enemyList[i].activeSelf)
                    enemyList.RemoveAt(i);
            }
        }
    }

    public void IncreaseStage()
    {
        ++stage;

        switch (stage)
        {
            case 3:
            case 4:
                spawnDelay *= DecreseSpawnDelayTime;
                break;
        }
    }
    
    public void IncreaseKillCount()
    {
        ++killCount;

        killCountText.text = killCount.ToString();
    }

    public static EnemySpawner GetInstance()
    {
        return instance;
    }

    public int GetListCount()
    {
        return enemyList.Count;
    }
}
