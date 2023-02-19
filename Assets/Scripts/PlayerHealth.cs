using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public GameObject deatVFXPrefab;
    public int trapsLayer;
    // Start is called before the first frame update
    void Start()
    {
        trapsLayer = LayerMask.NameToLayer("Traps");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == trapsLayer)
        {

            Instantiate(deatVFXPrefab, transform.position, transform.rotation);
            AudioManager.PlayerDeathAudio();
            gameObject.SetActive(false);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.PlayerDie();
            
        }
    }
}
