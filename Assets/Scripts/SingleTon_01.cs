using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon_01<T> : MonoBehaviour where T : MonoBehaviour //MonoBehaviour 에서 허용한 T만 사용한다
{
    //클래스 타입의 인스턴스를 스테틱으로 생성
    static T _instance = null;

    //인스턴스 : 생성
    public static T Instance
    {
        //get
        get
        {
            //검색
            _instance = (T)FindObjectOfType(typeof(T));
            //없으면
            if (_instance == null)
            {
                //생성
                var _newObject = new GameObject(typeof(T).ToString());
                _instance = _newObject.AddComponent<T>();
            }

            //인스턴스 반환
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if(_instance == null)
        {
            _instance = this as T;
        }
        DontDestroyOnLoad(gameObject);
    }

}
