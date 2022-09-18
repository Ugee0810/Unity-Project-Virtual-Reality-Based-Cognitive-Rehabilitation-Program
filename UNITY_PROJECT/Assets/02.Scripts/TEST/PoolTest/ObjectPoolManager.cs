using System;
using System.Collections.Generic;
using UnityEngine;
using KeyType = System.String;

/// <summary> 
/// 오브젝트 풀 관리 싱글톤
/// </summary>
[DisallowMultipleComponent]
public class ObjectPoolManager : MonoBehaviour
{
    // 인스펙터에서 오브젝트 풀링 대상 정보 추가
    [SerializeField]
    private List<PoolObjectData> _poolObjectDataList = new List<PoolObjectData>(4);

    // 복제될 오브젝트의 원본 딕셔너리
    private Dictionary<KeyType, PoolObject> _sampleDict;

    // 풀링 정보 딕셔너리
    private Dictionary<KeyType, PoolObjectData> _dataDict;

    // 풀 딕셔너리
    private Dictionary<KeyType, Stack<PoolObject>> _poolDict;

    public static ObjectPoolManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        int len = _poolObjectDataList.Count;
        if (len == 0) return;

        // 1. Dictionary 생성
        _sampleDict = new Dictionary<KeyType, PoolObject>(len);
        _dataDict = new Dictionary<KeyType, PoolObjectData>(len);
        _poolDict = new Dictionary<KeyType, Stack<PoolObject>>(len);

        // 2. Data로부터 새로운 Pool 오브젝트 정보 생성
        foreach (var data in _poolObjectDataList)
        {
            Register(data);
        }
    }

    /// <summary> Pool 데이터로부터 새로운 Pool 오브젝트 정보 등록 </summary>
    private void Register(PoolObjectData data)
    {
        // 중복 키는 등록 불가능
        if (_poolDict.ContainsKey(data.key))
        {
            return;
        }

        // 1. 샘플 게임오브젝트 생성, PoolObject 컴포넌트 존재 확인
        GameObject sample = Instantiate(data.prefab);
        if (!sample.TryGetComponent(out PoolObject po))
        {
            po = sample.AddComponent<PoolObject>();
            po.key = data.key;
        }
        sample.SetActive(false);

        // 2. Pool Dictionary에 풀 생성 + 풀에 미리 오브젝트들 만들어 담아놓기
        Stack<PoolObject> pool = new Stack<PoolObject>(data.maxObjectCount);
        for (int i = 0; i < data.initialObjectCount; i++)
        {
            PoolObject clone = po.Clone();
            pool.Push(clone);
        }

        // 3. 딕셔너리에 추가
        _sampleDict.Add(data.key, po);
        _dataDict.Add(data.key, data);
        _poolDict.Add(data.key, pool);
    }

    /// <summary> 풀에서 꺼내오기 </summary>
    public PoolObject Spawn(KeyType key)
    {
        // 키가 존재하지 않는 경우 null 리턴
        if (!_poolDict.TryGetValue(key, out var pool))
        {
            return null;
        }

        PoolObject po;

        // 1. 풀에 재고가 있는 경우 : 꺼내오기
        if (pool.Count > 0)
        {
            po = pool.Pop();
        }
        // 2. 재고가 없는 경우 샘플로부터 복제
        else
        {
            po = _sampleDict[key].Clone();
        }

        po.Activate();

        return po;
    }

    /// <summary> 풀에 집어넣기 </summary>
    public void Despawn(PoolObject po)
    {
        // 키가 존재하지 않는 경우 종료
        if (!_poolDict.TryGetValue(po.key, out var pool))
        {
            return;
        }

        KeyType key = po.key;

        // 1. 풀에 넣을 수 있는 경우 : 풀에 넣기
        if (pool.Count < _dataDict[key].maxObjectCount)
        {
            pool.Push(po);
            po.Deactivate();
        }
        // 2. 풀의 한도가 가득 찬 경우 : 파괴하기
        else
        {
            Destroy(po.gameObject);
        }
    }
}