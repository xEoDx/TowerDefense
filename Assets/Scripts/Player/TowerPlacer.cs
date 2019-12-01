using System;
using Buildings;
using FSM;
using Player.Input;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace Player
{
    public class TowerPlacer : MonoBehaviour
    {
        private Tower _tower;
        private Camera _camera;
        private float _towerBoundValue;

        public bool IsPlacing { get; private set; }

        private InputController _inputController;
        private void Awake()
        {
            IsPlacing = false;
            _camera = Camera.main;
            _inputController = FindObjectOfType<InputController>();
            _towerBoundValue = 1;
        }

        void Update()
        {
            if (IsPlacing)
            {
                RaycastHit hit;
                bool isValidPosition = false;
                if(Physics.Raycast(_camera.ScreenPointToRay(_inputController.GetTouchPosition()), out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
                {
                    isValidPosition = NavMesh.SamplePosition(hit.point, out var centerNavMeshHit, 1, NavMesh.AllAreas)
                                      && NavMesh.SamplePosition(hit.point - (Vector3.forward * _towerBoundValue), out var navMeshHitBottom, 1, NavMesh.AllAreas)
                                      && NavMesh.SamplePosition(hit.point + (Vector3.forward * _towerBoundValue), out var navMeshHitTop, 1, NavMesh.AllAreas)
                                      && NavMesh.SamplePosition(hit.point - (Vector3.right * _towerBoundValue), out var navMeshHitLeft, 1, NavMesh.AllAreas)
                                      && NavMesh.SamplePosition(hit.point + (Vector3.right * _towerBoundValue), out var navMeshHitRight, 1, NavMesh.AllAreas);

                    if (isValidPosition)
                    {
                        _tower.SetPlaceable();

                        var updatedPosition = centerNavMeshHit.position;
                        updatedPosition.y = 0;
                        _tower.transform.position = updatedPosition;
                    }
                    else
                    {
                        _tower.SetUnplaceable();
                        _tower.transform.position = hit.point;
                    }
                }

                if (isValidPosition && _inputController.IsTouching() && !EventSystem.current.IsPointerOverGameObject())
                {
                    _tower.PlaceTower();
                    IsPlacing = false;
                    SetTowerEnabled(true);
                }
            }
        }

        public void Create(Tower tower)
        {
            _tower = tower;
            _towerBoundValue = _tower.transform.GetComponent<NavMeshObstacle>().radius;
            SetTowerEnabled(false);
            IsPlacing = true;
        }

        private void SetTowerEnabled(bool isEnabled)
        {
            _tower.enabled = isEnabled;
            _tower.GetComponent<NavMeshObstacle>().enabled = isEnabled;
            _tower.GetComponent<StateMachine>().enabled = isEnabled;
        }
    }
}
