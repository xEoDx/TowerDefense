﻿using Buildings;
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

        public bool IsPlacing { get; private set; }

        private InputController _inputController;
        private void Awake()
        {
            IsPlacing = false;
            _camera = Camera.main;
            _inputController = FindObjectOfType<InputController>();
        }

        void Update()
        {
            if (IsPlacing)
            {
                RaycastHit hit;
                bool isValidPosition = false;
                if(Physics.Raycast(_camera.ScreenPointToRay(_inputController.GetTouchPosition()), out hit))
                {
                    isValidPosition = NavMesh.SamplePosition(hit.point, out var centerNavMeshHit, 1, NavMesh.AllAreas)
                                      && NavMesh.SamplePosition(hit.point - (Vector3.forward / 2), out var navMeshHitBottom, 1, NavMesh.AllAreas)
                                      && NavMesh.SamplePosition(hit.point + (Vector3.forward / 2), out var navMeshHitTop, 1, NavMesh.AllAreas)
                                      && NavMesh.SamplePosition(hit.point - (Vector3.right / 2), out var navMeshHitLeft, 1, NavMesh.AllAreas)
                                      && NavMesh.SamplePosition(hit.point + (Vector3.right / 2), out var navMeshHitRight, 1, NavMesh.AllAreas);

                    if (isValidPosition)
                    {
                        _tower.SetPlaceable();
                        _tower.transform.position = centerNavMeshHit.position;
                    }
                    else
                    {
                        _tower.SetUnplaceable();
                    }
                }

                if (isValidPosition && _inputController.IsTouching() && !EventSystem.current.IsPointerOverGameObject())
                {
                    _tower.PlaceTower();
                    IsPlacing = false;
                    _tower.GetComponent<NavMeshObstacle>().enabled = true;
                }
            }
        }

        public void Create(Tower tower)
        {
            _tower = tower;
            _tower.GetComponent<NavMeshObstacle>().enabled = false;
            IsPlacing = true;
        }
    }
}
