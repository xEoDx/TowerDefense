using Buildings;
using Player;
using UnityEngine;

namespace UI
{
    public class TowerCreatorButton : MonoBehaviour
    {
        [SerializeField] private GameObject towerPrefab;

        [SerializeField] private float cost;

        private TowerPlacer _towerPlacer;
    
        private void Start()
        {
            _towerPlacer = FindObjectOfType<TowerPlacer>();
        }

        public void Create()
        {
            //todo check cost with economysystem
            if (!_towerPlacer.IsPlacing)
            {
                var towerObject = Instantiate(towerPrefab);
                _towerPlacer.Create(towerObject.GetComponent<Tower>());
            }
        }
    }
}
