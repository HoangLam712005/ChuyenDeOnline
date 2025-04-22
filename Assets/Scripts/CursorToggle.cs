using UnityEngine;

public class CursorToggle : MonoBehaviour
{
    private bool cursorVisible = false;

    void Start()
    {
        ToggleCursor(false); // Ẩn chuột khi bắt đầu
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorVisible = !cursorVisible;
            ToggleCursor(cursorVisible);
        }
    }

    void ToggleCursor(bool show)
    {
        Cursor.visible = show;
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
