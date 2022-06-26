using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyMove : MonoBehaviour
{
    public GameObject ExMark;

    private void OnEnable()
    {
        //두더지가 활성화 되면 이동 코루틴 활성화
        StartCoroutine(Move());
    }

    /// <summary>
    /// 이동 애니메이션 코루틴
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 충돌 할 때
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        //충돌체가 망치면?
        if(collision.gameObject.CompareTag("Hammer"))
        {
            //점수를 추가하고 오브젝트를 끈다. 재생성을 위채 SendMsg
            ExMark.SetActive(true);

            SendMessageUpwards("Dead");
            SendMessageUpwards("SummonEnemy");
            GameManager.Instance.AddScore(100);
            gameObject.SetActive(false);
        }
    }

}
