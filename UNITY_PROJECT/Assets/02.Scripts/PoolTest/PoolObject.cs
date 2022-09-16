using UnityEngine;
using KeyType = System.String;

/// <summary>복제된 오브젝트의 컴포넌트로 들어가는 클래스. 필드로 키값을 보관한다. </summary>
[DisallowMultipleComponent]
public class PoolObject : MonoBehaviour
{
    public KeyType key;

    /// <summary> 게임오브젝트 복제 </summary>
    public PoolObject Clone()
    {
        GameObject go = Instantiate(gameObject);
        if (!go.TryGetComponent(out PoolObject po))
            po = go.AddComponent<PoolObject>();
        go.SetActive(false);

        return po;
    }

    /// <summary> 게임오브젝트 활성화 </summary>
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    /// <summary> 게임오브젝트 비활성화 </summary>
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}