using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level : MonoBehaviour
{
    [SerializeField] Slider expSlider;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] Image levelUpBar;
    [SerializeField] ParticleSystem[] particles = new ParticleSystem[3];

    [SerializeField] GameObject levelUpWindow;
    [SerializeField] GameObject[] weaponSelect = new GameObject[4];

    int maxExpValue;
    int curExpValue;
    static int level;
    static bool isLevelUpTime;

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        maxExpValue = 50;
        curExpValue = 0;
        level = 1;
        isLevelUpTime = false;
        expSlider.maxValue = maxExpValue;
        expSlider.value = curExpValue;
    }

    public static bool GetIsLevelUpTime()
    {
        return isLevelUpTime;
    }

    public static int GetPlayerLevel()
    {
        return level;
    }

    public void GetExp(int value)
    {
        if (curExpValue + value >= maxExpValue)
        {
            curExpValue += value - maxExpValue;
            LevelUp();
        }
        else
            curExpValue += value;

        expSlider.value = curExpValue;
    }

    void LevelUp()
    {
        isLevelUpTime = true;
        StartCoroutine(GetNewItem());
        StartCoroutine(LevelUpEffects());

        level++;
        text.text = "LV " + level.ToString();

        maxExpValue *= level;
        expSlider.maxValue = maxExpValue;
    }

    IEnumerator GetNewItem()
    {
        Time.timeScale = 0f;
        levelUpWindow.SetActive(true);

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space)) break;



            yield return null;
        }

        isLevelUpTime = false;
        levelUpWindow.SetActive(false);
        Time.timeScale = 1f;
    }

    IEnumerator LevelUpEffects()
    {
        levelUpBar.gameObject.SetActive(true);
        StartParticles();

        foreach (ParticleSystem particle in particles)
        {
            particle.gameObject.SetActive(true);
            particle.Play();
        }

        while (true)
        {
            if (!isLevelUpTime) break;

            for (float i = 0f; i < 1f; i += 0.1f)
            {
                levelUpBar.color = Color.Lerp(new Color(1f, 0f, 1f), new Color(0f, 1f, 1f), i);
                yield return new WaitForSecondsRealtime(0.01f);
            }

            for (float i = 0f; i < 1f; i += 0.1f)
            {
                levelUpBar.color = Color.Lerp(new Color(0f, 1f, 1f), new Color(1f, 0f, 1f), i);
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }

        levelUpBar.gameObject.SetActive(false);
        StopParticles();
    }

    void StartParticles()
    {
        foreach (ParticleSystem particle in particles)
        {
            particle.gameObject.SetActive(true);
            particle.Play();
        }
    }

    void StopParticles()
    {
        foreach (ParticleSystem particle in particles)
        {
            particle.Stop();
            particle.gameObject.SetActive(false);
        }
    }
}
