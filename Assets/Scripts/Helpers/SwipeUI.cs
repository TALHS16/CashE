using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SwipeUI : MonoBehaviour
{
    public Scrollbar scrollBar; // Check the current page based on the
    [SerializeField]
    private float swipeTime = 0.2f; // Time when the page is swiped
    [SerializeField]
    private float swipeDistance; // Minimum distance the page must move to be swipeable

    private float startTouchX; // touch start position
    private float endTouchX; // touch end position
    private bool isSwipeMode = false; // Check if Swipe is currently being used

    private void Update()
    {
        UpdateInput();
    }

    private void UpdateInput()
    {
        // If swipe is currently in progress, touch is not possible
        if (isSwipeMode) return;

        #if UNITY_EDITOR
        // Once when the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            // Touch start point (Swipe direction distinction)
            startTouchX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // Touch end point (Swipe direction distinction)
            endTouchX = Input.mousePosition.x;

            UpdateSwipe();
        }
        #endif

        #if UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Touch start point (Swipe direction distinction)
                startTouchX = touch.position.x;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // Touch end point (Swipe direction distinction)
                endTouchX = touch.position.x;

                UpdateSwipe();
            }
        }
        #endif
    }

    private void UpdateSwipe()
    {
        if (Mathf.Abs(startTouchX - endTouchX) < swipeDistance)
        {
            // Swipe to return to the original page
            StartCoroutine(OnSwipeOneStep(0));
            return;
        }

        // Check if the touch is within the bounds of the current object
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
            return;

        // Swipe direction
        bool isLeft = startTouchX < endTouchX;

        // When the direction of movement is left
        if (isLeft)
        {
            StartCoroutine(OnSwipeOneStep(0));
        }
        // Movement direction is right
        else
        {
            StartCoroutine(OnSwipeOneStep(swipeDistance)); // Adjust the distance as needed
        }
    }

    /// <summary>
    /// Play the Swipe effect that turns a page sideways
    /// </summary>
    private IEnumerator OnSwipeOneStep(float distance)
    {
        float start = scrollBar.value;
        float current = 0;
        float percent = 0;

        isSwipeMode = true;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / swipeTime;

            scrollBar.value = Mathf.Lerp(start, distance, percent);

            yield return null;
        }

        isSwipeMode = false;
    }
}
