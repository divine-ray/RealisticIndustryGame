using TwoPiGrid;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField] private Camera m_Camera = default;
    [SerializeField] private GridSettingsSerialized gridSettings = default;
    [SerializeField] private Transform objectToMove = default;

    private BaseGrid grid;

    private void Start()
    {
        grid = new BaseGrid(gridSettings);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(m_Camera.ScreenPointToRay(Input.mousePosition), out var hit, 100, LayerMask.GetMask("Default"))) //Hit the sphere
            {
                var index = grid.GetIndexOfClosestVertexTo(hit.point);
                objectToMove.transform.position = grid.GetCellPosition(index);
            }
        }
    }
}
