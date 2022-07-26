using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform player;
    const float maxX = 10;
    const float maxY = 16;
    float spawnDelay = 0.5f;
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
                    newEnemy = ObjectPooling.GetEnemy(Character.CharacterType.FlyingEye);
                    break;
                case 2:
                    newEnemy = ObjectPooling.GetEnemy(Character.CharacterType.Goblin);
                    break;
                case 3:
                    newEnemy = ObjectPooling.GetEnemy(Character.CharacterType.Skeleton);
                    break;
                case 4:
                    newEnemy = ObjectPooling.GetEnemy(Character.CharacterType.Mushroom);
                    break;
            }

            newEnemy.transform.position = (Vector3)RandomPosition();
            newEnemy.SetActive(true);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    Vector2 RandomPosition()
    {
        Vector2 pos = new Vector2();

        Direction direction = (Direction)Random.Range(0, 4);

        Debug.Log(direction);

        switch (direction)
        {
            case Direction.North:
                pos.x = Random.Range(player.transform.position.x- maxX, 2 * maxX);
                pos.y = player.transform.position.y + 10f;
                break;
            case Direction.South:
                pos.x = Random.Range(player.transform.position.x - maxX, 2 * maxX);
                pos.y = player.transform.position.y -10f;
                break;
            case Direction.West:
                pos.x = player.transform.position.x - 16f;
                pos.y = Random.Range(player.transform.position.y - maxY, 2 * maxY);
                break;
            case Direction.East:
                pos.x = player.transform.position.x + 15f;
                pos.y = Random.Range(player.transform.position.y - maxY, 2 * maxY);
                break;
        }

        return pos;
    }
}
