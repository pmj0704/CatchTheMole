using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyMove : MonoBehaviour
{

    private void OnEnable()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        //코루틴을 사용하여 반복 이동
        while (true)
        {
            //월드 Y가 로컬 Z로 바뀜
            transform.DOLocalMoveZ(0f, 0.8f);
            yield return new WaitForSeconds(0.9f);
            transform.DOLocalMoveZ(-0.65f, 0.8f);
            yield return new WaitForSeconds(0.9f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Hammer"))
        {
            Debug.Log("아!");
            SendMessageUpwards("SummonEnemy");
            GameManager.Instance.AddScore(100);
            gameObject.SetActive(false);
        }
    }

}
