using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    Player player;
    int level;

    void Awake()
    {
        Initialize();
    }

    internal void Initialize()
    {
        level = 1;
        player = GetComponent<Player>();
    }

    public int GetLevel()
    {
        return level;
    }

    public Player GetPlayer()
    {
        return player;
    }

    public void IncreaseLevel()
    {
        level++;
    }

    internal abstract IEnumerator StartAttack();
}
