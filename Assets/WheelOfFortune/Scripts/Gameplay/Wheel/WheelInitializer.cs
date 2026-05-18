using UnityEngine;
using UnityEngine.Android;

namespace WheelOfFortune.Gameplay.Wheel
{
    public class WheelInitializer : MonoBehaviour
    {
        [SerializeField] private Transform _cellContainer;
        [SerializeField] private WheelCellDisplay _cellPrefab;

        private WheelCellDisplay[] _cells;

        public void InitializeWheelCell(WheelConfig config)
        {
            _cells = new WheelCellDisplay[config.SliceCount];

            float angleStep = 360f / config.SliceCount;

            for (int i = 0; i < config.SliceCount; i++)
            {
                WheelCellDisplay cell = Instantiate(_cellPrefab, _cellContainer);
                _cells[i] = cell;

                float angle = angleStep * i;
                float radian = angle * Mathf.Deg2Rad;

                Vector3 localPosition = new Vector3(Mathf.Cos(radian) * config.CellRadius, Mathf.Sin(radian) * config.CellRadius, 0f);

                cell.transform.localPosition = localPosition;
                cell.transform.localRotation = Quaternion.FromToRotation(Vector3.up, localPosition.normalized);
            }
        }

        public void InitializeWheel(IWheelCellContent[] contents)
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                _cells[i].Initialize(contents[i]);
            }
        }
    }
}