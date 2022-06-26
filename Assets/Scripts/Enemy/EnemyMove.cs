using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyMove : MonoBehaviour
{
    public GameObject ExMark;

    private void OnEnable()
    {
        //�δ����� Ȱ��ȭ �Ǹ� �̵� �ڷ�ƾ Ȱ��ȭ
        StartCoroutine(Move());
    }

    /// <summary>
    /// �̵� �ִϸ��̼� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// �浹 �� ��
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        //�浹ü�� ��ġ��?
        if(collision.gameObject.CompareTag("Hammer"))
        {
            //������ �߰��ϰ� ������Ʈ�� ����. ������� ��ä SendMsg
            ExMark.SetActive(true);

            SendMessageUpwards("Dead");
            SendMessageUpwards("SummonEnemy");
            GameManager.Instance.AddScore(100);
            gameObject.SetActive(false);
        }
    }

}
