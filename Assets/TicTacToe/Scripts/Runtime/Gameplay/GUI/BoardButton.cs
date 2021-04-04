using UnityEngine;
using UnityEngine.UI;

namespace TicTactoe.Gameplay.GUI
{
    [RequireComponent(typeof(Button))]
    public class BoardButton : MonoBehaviour
    {
        [SerializeField]
        private Image m_graphic = default;
        public Image Graphic => m_graphic;
        public Vector2Intx position = Vector2Intx.zero;
        private Button button = null;
        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() => 
            {
                GameplayManager.Instance.NewBoardTap(position);
            });
        }
    }
}