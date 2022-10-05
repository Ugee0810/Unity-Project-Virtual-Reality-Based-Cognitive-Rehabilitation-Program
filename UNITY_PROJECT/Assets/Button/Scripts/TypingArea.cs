using UnityEngine;

public class TypingArea : MonoBehaviour
{
    public GameObject layLeftHand;
    public GameObject layRightHand;

    public GameObject layLeftColl;
    public GameObject layRightColl;

    public GameObject leftHand;
    public GameObject rightHand;

    public GameObject leftTypingHand;
    public GameObject rightTypingHand;

    private void OnTriggerEnter(Collider c)
    {
        GameObject hand = c.gameObject;
        if (hand == null)
            return;
        if (hand == layLeftColl || hand == leftHand)
        {
            leftHand.SetActive(true);
            layLeftHand.SetActive(false);
            leftTypingHand.SetActive(true);
        }
        else if (hand == layRightColl || hand == rightHand)
        {
            rightHand.SetActive(true);
            layRightHand.SetActive(false);
            rightTypingHand.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider c)
    {
        GameObject hand = c.gameObject;
        if (hand == null)
            return;
        if (hand == layLeftHand || hand == leftHand)
        {
            layLeftHand.SetActive(true);
            leftHand.SetActive(false);
            leftTypingHand.SetActive(false);
        }
        else if (hand == layRightHand || hand == rightHand)
        {
            layRightHand.SetActive(true);
            rightHand.SetActive(false);
            rightTypingHand.SetActive(false);
        }
    }
}