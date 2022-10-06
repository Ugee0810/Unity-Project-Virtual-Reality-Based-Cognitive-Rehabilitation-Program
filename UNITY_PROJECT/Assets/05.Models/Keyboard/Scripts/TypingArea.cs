using UnityEngine;

public class TypingArea : MonoBehaviour
{
    public GameObject controllerLeft;
    public GameObject controllerRight;

    public GameObject rayLeftInteractor;
    public GameObject rayRightInteractor;

    public GameObject leftTypingHand;
    public GameObject rightTypingHand;

    private void OnTriggerEnter(Collider c)
    {
        GameObject hand = c.gameObject;
        if (hand == null) return;
        if (hand == controllerLeft)
        {
            rayLeftInteractor.SetActive(false);
            leftTypingHand.SetActive(true);
        }
        else if (hand == controllerRight)
        {
            rayRightInteractor.SetActive(false);
            rightTypingHand.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider c)
    {
        GameObject hand = c.gameObject;
        if (hand == null) return;
        if (hand == controllerLeft)
        {
            rayLeftInteractor.SetActive(true);
            leftTypingHand.SetActive(false);
        }
        else if (hand == controllerRight)
        {
            rayRightInteractor.SetActive(true);
            rightTypingHand.SetActive(false);
        }
    }
}