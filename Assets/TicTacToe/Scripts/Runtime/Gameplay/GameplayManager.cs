using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TicTactoe.Gameplay.GUI;
using TicTactoe.Gameplay.Players;

namespace TicTactoe.Gameplay
{
    public enum GameMode
    {
        HumanVsHuman,
        HumanVsAI,
        AIVsAI
    }
    public class GameplayManager : MonoBehaviour
    {
        private static GameplayManager _instance = null;

        public static GameplayManager Instance
        {
            get 
            {
                if(!_instance) _instance = FindObjectOfType<GameplayManager>();
                return _instance;
            }
        }
        [SerializeField]
        private string notation = default;
        [SerializeField]
        private GameMode gameMode = GameMode.HumanVsAI;
        [SerializeField]
        private Sprite Icon_X = default;
        [SerializeField]
        private Sprite Icon_O = default;
        [SerializeField]
        private BoardButton[] m_Boardbuttons = default;
        [SerializeField]
        private Button m_replay_button = default;

        private GameController gameController = null;

        private Dictionary<Vector2Intx,int> buttonsMap = new Dictionary<Vector2Intx, int>();

        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        private void OnDestroy()
        {
            UnSuscribeEvents();
            _instance = null;
        }

        private void UnSuscribeEvents()
        {
            gameController.OnMovementMade -= GameController_OnMovementMade;
        }

        private void SubscribeEvents()
        {
            gameController.OnMovementMade += GameController_OnMovementMade;
        }

        private void Start()
        {
            m_replay_button.onClick.AddListener(() => ResetGame());
            Init();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log(gameController.board.Serialize());
            }
        }
        
        public void NewBoardTap(Vector2Intx _position)
        {
            gameController.NewButtonTap(_position);
        }

        public void ResetGame()
        {
            gameController.ResetGame();
        }

        private void Init()
        {
            InitializeButtons();
            switch(gameMode)
            {
                case GameMode.AIVsAI:
                    gameController = new GameController(new AIPlayer() , new AIPlayer() , PlayerSide.Player_O , notation); 
                break;

                case GameMode.HumanVsAI:
                    gameController = new GameController(new HumanPlayer() , new AIPlayer() , PlayerSide.Player_O , notation);
                break;

                case GameMode.HumanVsHuman:
                    gameController = new GameController(new HumanPlayer() , new HumanPlayer() , PlayerSide.Player_O, notation);
                break;
            }
            
            InitializeBoard();
            SubscribeEvents();
            gameController.StartGame();
        }

        private void InitializeBoard()
        {
            int[,] boardData = gameController.board.GetBoardData();
            for (int x = 0; x < Board.board_size; x++)
            {
                for (int y = 0; y < Board.board_size; y++)
                {
                    PlayerSide side = (PlayerSide)boardData[x,y];
                    if (side != PlayerSide.None)
                    {
                        BoardButton btn = GetButton(new Vector2Intx(x,y));
                        if (btn != null)
                        {
                            if(side == PlayerSide.Player_O)
                                btn.Graphic.sprite = Icon_O;
                            else if (side == PlayerSide.Player_X)
                                btn.Graphic.sprite = Icon_X;
                        }
                    }
                }
            }
        }

        private void InitializeButtons()
        {
            for (int y = 0; y < Board.board_size; y++)
            {
                for (int x = 0; x < Board.board_size; x++)
                {
                    int index = (y * Board.board_size) + x;
                    m_Boardbuttons[index].position = new Vector2Intx(x,y);
                    buttonsMap.Add(new Vector2Intx(x,y) , index);    
                }
            }
        }

        private BoardButton GetButton(Vector2Intx _position)
        {
            if (buttonsMap.TryGetValue(_position , out int index))
            {
                if (index >= 0 && index < m_Boardbuttons.Length)
                    return m_Boardbuttons[index];
            }
            return null;
        }

        private void GameController_OnMovementMade(PlayerSide player , Vector2Intx _position)
        {
            BoardButton btn = GetButton(_position);
            if (btn != null)
            {
                Sprite target_sprite = (player == PlayerSide.Player_O) ? Icon_O : Icon_X;
                btn.Graphic.sprite = target_sprite;
            }
        }

    }
}