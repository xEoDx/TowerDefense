using Buildings;
using Gameplay;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class TowerCreatorButton : MonoBehaviour
    {
        [SerializeField] private TowerType towerType;
        [SerializeField] private GameObject towerPrefab;

        private Button _buildButton;
        public Text costText;
        public Text damageText;
        public Text attackSpeedText;
        
        private TowerPlacer _towerPlacer;
        private PlayerController _playerController;
        private PlayerData _playerData;

        private int _cost;
        private float _attackSpeed;
        private float _damage;
        
        private void Awake()
        {
            _buildButton = GetComponent<Button>();
            _towerPlacer = FindObjectOfType<TowerPlacer>();
            _playerController = FindObjectOfType<PlayerController>();
            _playerData = FindObjectOfType<PlayerData>();
        }

        private void Start()
        {
            _playerController.OnIncomeUpdated += OnIncomeUpdatedListener;

            _cost = _playerData.GetTowerCost(towerType);
            var attributes = _playerData.GetTowerAttributes(towerType);

            _attackSpeed = attributes.OffensiveAttributesData.AttackSpeed;
            _damage = attributes.OffensiveAttributesData.Damage;

            attackSpeedText.text = _attackSpeed.ToString();
            damageText.text = _damage.ToString();
            costText.text = _cost.ToString();
        }

        private void OnIncomeUpdatedListener(int newIncomeAmount)
        {
            _buildButton.interactable = newIncomeAmount >= _cost;
        }

        public void Create()
        {
            if (!_towerPlacer.IsPlacing)
            {
                var towerObject = Instantiate(towerPrefab);
                _towerPlacer.Create(towerObject.GetComponent<CanonTower>());
                _playerController.SubtractIncome(_cost);
            }
        }
    }
}
