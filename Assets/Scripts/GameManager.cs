using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    static GameManager instance;
    SceneFader fader;
    List<Orb> orbs;
    public int deathCount;
    // Start和Awake谁先执行？
    public int orbNum;
    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
        //Debug.Log("GameManager先执行的");
        orbs = new List<Orb>();
        deathCount = 0;
    }

    public static void PlayerDie()
    {
        //if(instance.fader == null)
        //{
        //    Debug.Log("fader没有被实例化");
        //}
        instance.fader.FadeOut();
        instance.Invoke("RestartScene", 1.5f);
    }

    private void Update()
    {
        orbNum = instance.orbs.Count;
    }

    public static void RegisteOrbs(Orb orb)
    {
        if (!instance.orbs.Contains(orb))
        {
            instance.orbs.Add(orb);
        }
    }

    public static void RegisteSceneFader(SceneFader obj)
    {
        instance.fader = obj;
        
    }
    public static void PlayerGrabedOrb(Orb orb)
    {
        if (!instance.orbs.Contains(orb)) return;
        instance.orbs.Remove(orb);

    }

    void RestartScene()
    {
        deathCount++;

        instance.orbs.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
