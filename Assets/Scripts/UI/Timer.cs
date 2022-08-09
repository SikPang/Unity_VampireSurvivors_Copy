using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    Timer instance;
    TextMeshProUGUI timeText;
    int minute;
    int second;

    private Timer() { }

    void Awake()
    {
        Initialize();
    }

    void Start()
    {
        StartCoroutine(CountTime());
    }

    void Initialize()
    {
        instance = this;
        timeText = GetComponent<TextMeshProUGUI>();
        minute = 0;
        second = 0;
    }

    IEnumerator CountTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            ++second;
            if (second >= 60)
            {
                ++minute;
                second = 0;

                //if(minute >= 10)
                // ¿£µù

                if (minute != 0 && minute % 2 == 0)
                    EnemySpawner.GetInstance().IncreaseStage();
            }

            if (second < 10)
                timeText.text = minute.ToString() + " : 0" + second.ToString();
            else
                timeText.text = minute.ToString() + " : " + second.ToString();
        }
    }

    public int GetMinute()
    {
        return minute;
    }
}
