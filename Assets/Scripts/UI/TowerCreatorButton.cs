using Buildings;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class TowerCreatorButton : MonoBehaviour
    {
        [SerializeField] private GameObject towerPrefab;

        [SerializeField] private int cost;

        private Button _button;
        private TowerPlacer _towerPlacer;
        private PlayerController _playerController;
    
        private void Awake()
        {
            _button = GetComponent<Button>();
            _towerPlacer = FindObjectOfType<TowerPlacer>();
            _playerController = FindObjectOfType<PlayerController>();
        }

        private void Start()
        {
            _playerController.OnIncomeUpdated += OnIncomeUpdatedListener;
        }

        private void OnIncomeUpdatedListener(int newIncomeAmount)
        {
            _button.interactable = newIncomeAmount >= cost;
        }

        public void Create()
        {
            if (!_towerPlacer.IsPlacing)
            {
                var towerObject = Instantiate(towerPrefab);
                _towerPlacer.Create(towerObject.GetComponent<Tower>());
                _playerController.SubtractIncome(cost);
            }
        }
    }
}
