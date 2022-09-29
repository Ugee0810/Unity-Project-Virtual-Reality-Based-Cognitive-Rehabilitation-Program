using UnityEngine;

// 직렬화를 통해 다른 클래스도 인스펙터창에서 확인
[System.Serializable]
public class VRMap
{
    // VR 타겟이라는 VR 변환을 위해 실행 가능한 클래스 4가지
    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void Map()
    {
        rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class VRRig : MonoBehaviour
{
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    // Add Call Add 제약을 위한 두 개의 실행 가능한 필록 전송 변수
    public Transform headConstraint;

    // 머리와 몸체 사이의 초기 위치 차이가 될 집합 변수
    public Vector3 headBodyOffset;

    public int turnSmoothness;

    private void Start()
    {
        // 최초에 위치 변환을 해준다.
        headBodyOffset = transform.position - headConstraint.position;
    }

    private void Update()
    {
        // 머리 위치에 따라 몸체는 수시로 변화한다.
        // 머리의 위치에 오프셋을 추가한 포지션 값을 부모에게 전달한다.
        transform.position = headConstraint.position + headBodyOffset;

        // 이 부분은 3D 캐릭터의 헤드 축에 따라서 수시로 변경되어야 한다.
        // Robot Kyle의 경우 머리는 정면이 녹색(y)축이므로 걸음에 up을 넣어준다.
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized, Time.deltaTime * turnSmoothness);

        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
}