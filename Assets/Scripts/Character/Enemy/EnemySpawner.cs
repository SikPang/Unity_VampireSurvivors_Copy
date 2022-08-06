using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform player;
    const float maxX = 10;
    const float maxY = 16;
    float spawnDelay = 1f;
    int stage = 1;

    enum Direction
    {
        North,
        South,
        West,
        East
    }

    void Start()
    {
        StartCoroutine(SpawnEnemy());
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
                    newEnemy = ObjectPooling.GetObject(CharacterData.CharacterType.Skeleton);
                    break;
                case 4:
                    newEnemy = ObjectPooling.GetObject(CharacterData.CharacterType.Mushroom);
                    break;
            }

            newEnemy.transform.position = RandomPosition();
            newEnemy.SetActive(true);

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
}
