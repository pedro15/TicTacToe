using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TicTacToe.Core;

namespace TicTacToe.Gameplay.GUI
{
    public class GameButton : MonoBehaviour
    {
        [SerializeField]
        private Button m_button = default;

        public Vector2Int Movecoord;

        private void Start()
        {
            m_button.onClick.AddListener(() => GameInput.TriggerMoveSquare(Movecoord));
        }

        public void Init(Vector2Int coord)
        {
            Movecoord = coord;
        }
    }
}