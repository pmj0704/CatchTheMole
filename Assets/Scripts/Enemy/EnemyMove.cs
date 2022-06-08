using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyMove : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (true)
        {
            transform.DOLocalMoveY(0f, 0.8f);
            yield return new WaitForSeconds(0.9f);
            transform.DOLocalMoveY(-0.65f, 0.8f);
            yield return new WaitForSeconds(0.9f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Hammer"))
        {
            Debug.Log("¾Æ!");
            GameManager.Instance.AddScore(100);
        }
    }
}
