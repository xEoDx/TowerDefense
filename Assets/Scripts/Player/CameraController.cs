using Player.Input;
using UnityEngine;

namespace Player
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float cameraSpeed;

        private InputController _inputController;

        private void Awake()
        {
            _inputController = FindObjectOfType<InputController>();
        }

        private void Update()
        {
            transform.Translate(_inputController.MovementDirection * cameraSpeed * Time.deltaTime, Space.World);
        
        }
    }
}
