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
        private CanonTower _canonTower;
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
                        _canonTower.SetPlaceable();

                        var updatedPosition = centerNavMeshHit.position;
                        updatedPosition.y = 0;
                        _canonTower.transform.position = updatedPosition;
                    }
                    else
                    {
                        _canonTower.SetUnplaceable();
                        _canonTower.transform.position = hit.point;
                    }
                }

                if (isValidPosition && _inputController.IsTouching() && !EventSystem.current.IsPointerOverGameObject())
                {
                    _canonTower.PlaceTower();
                    IsPlacing = false;
                    SetTowerEnabled(true);
                }
            }
        }

        public void Create(CanonTower canonTower)
        {
            _canonTower = canonTower;
            _towerBoundValue = _canonTower.transform.GetComponent<NavMeshObstacle>().radius;
            SetTowerEnabled(false);
            IsPlacing = true;
        }

        private void SetTowerEnabled(bool isEnabled)
        {
            _canonTower.enabled = isEnabled;
            _canonTower.GetComponent<NavMeshObstacle>().enabled = isEnabled;
            _canonTower.GetComponent<StateMachine>().enabled = isEnabled;
        }
    }
}
