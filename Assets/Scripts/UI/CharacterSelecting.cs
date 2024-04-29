using UnityEngine;

public class CharacterSelecting : MonoBehaviour
{
    public float swipeThreshold = 50f; // Minimum swipe distance to trigger character change
    public float swipeSpeed = 5f; // Speed of the swipe animation
    private Vector3 touchStartPos;
    private Vector3 touchEndPos;
    [SerializeField] private MainMenu mainMenu;

    void Update()
    {
        // Detect swipe input
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            touchEndPos = Input.mousePosition;
            float swipeDistance = touchEndPos.x - touchStartPos.x;

            if (Mathf.Abs(swipeDistance) > swipeThreshold)
            {
                // Swipe left
                if (swipeDistance < 0)
                {
                    mainMenu.LeftButton();
                }
                // Swipe right
                else
                {
                    mainMenu.RightButton();
                }
            }
        }
    }
}

