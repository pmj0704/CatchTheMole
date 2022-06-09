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
        //�ڷ�ƾ�� ����Ͽ� �ݺ� �̵�
        while (true)
        {
            //���� Y�� ���� Z�� �ٲ�
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
            Debug.Log("��!");
            SendMessageUpwards("SummonEnemy");
            GameManager.Instance.AddScore(100);
            gameObject.SetActive(false);
        }
    }

}
