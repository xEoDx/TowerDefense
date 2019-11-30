using UnityEngine;

namespace Player.Input
{
    public class InputController : MonoBehaviour
    {
        private Vector3 _movementDirection;
        public Vector3 MovementDirection => _movementDirection;

        private void Update()
        {
            UpdateCameraMovementDirection();
        }

        public bool IsTouching()
        {
            bool isTouching;
            
#if UNITY_EDITOR || UNITY_STANDALONE
                isTouching = UnityEngine.Input.GetMouseButton(0);
#endif
            //TODO implement for other platforms
            return isTouching;
        }

        public Vector3 GetTouchPosition()
        {
            Vector3 touchPosition;

#if UNITY_EDITOR || UNITY_STANDALONE
            touchPosition = UnityEngine.Input.mousePosition;
#endif
            //TODO implement for other platforms
            return touchPosition;
        }

        private void UpdateCameraMovementDirection()
        {
            _movementDirection = Vector3.zero;

#if UNITY_EDITOR || UNITY_STANDALONE
            if (UnityEngine.Input.GetKey(KeyCode.W) || UnityEngine.Input.GetKey(KeyCode.UpArrow))
            {
                _movementDirection.z = -1;
            }
            else if (UnityEngine.Input.GetKey(KeyCode.S) || UnityEngine.Input.GetKey(KeyCode.DownArrow))
            {
                _movementDirection.z = 1;
            }

            if (UnityEngine.Input.GetKey(KeyCode.A) || UnityEngine.Input.GetKey(KeyCode.LeftArrow))
            {
                _movementDirection.x = 1;
            }
            else if (UnityEngine.Input.GetKey(KeyCode.D) || UnityEngine.Input.GetKey(KeyCode.RightArrow))
            {
                _movementDirection.x = -1;
            }

            if (UnityEngine.Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                _movementDirection.y = -1;
            }

            if (UnityEngine.Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                _movementDirection.y = 1;
            }
#endif

            //TODO implement for other platforms
        }
    }
}