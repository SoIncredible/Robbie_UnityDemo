using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public GameObject FXExploration;
    public int playerLayer;
    // Start is called before the first frame update
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        GameManager.RegisteOrbs(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == playerLayer)
        {

            AudioManager.PlayerOrbAudio();
            Instantiate(FXExploration, transform.position, transform.rotation);
            GameManager.PlayerGrabedOrb(this);
            gameObject.SetActive(false);
        }
    }
}
