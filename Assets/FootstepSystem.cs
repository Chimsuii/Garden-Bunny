using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem; // 1. Add this namespace

public class FootstepSystem : MonoBehaviour {

    [Range(0,20f)]
    public float frequency = 10.0f;
    public UnityEvent onFootStep;

    float Sin;

    void Update() {
        // 2. Use Keyboard.current or Gamepad.current instead of Input.GetAxis
        Vector2 moveInput = Vector2.zero;

        if (Keyboard.current != null) {
            float x = Keyboard.current.dKey.isPressed ? 1 : (Keyboard.current.aKey.isPressed ? -1 : 0);
            float y = Keyboard.current.wKey.isPressed ? 1 : (Keyboard.current.sKey.isPressed ? -1 : 0);
            moveInput = new Vector2(x, y);
        }

        // 3. Check magnitude
        if (moveInput.magnitude > 0) {
            StartFootsteps();
        }
    }

    private void StartFootsteps() {
        // Your logic remains the same
        if (Mathf.Sin(Time.time * frequency) > 0.97f) {
            onFootStep.Invoke();
        }
    }
}