using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{

    [SerializeField]
    private GameObject EnemySpawner;
    [SerializeField]
    private GameObject door;


    private void Start()
    {
        EnemySpawner.SetActive(false);
    }


    void OnTriggerEnter(Collider collision)
    {
      if(collision.CompareTag("Player"))
        {
           EnemySpawner.SetActive(true);
            StartCoroutine(DisappearDetector());
            door.SetActive(true);
        }
    }

    IEnumerator DisappearDetector()
    {
        Destroy(gameObject);
        yield return 3f;
       
    }


}
