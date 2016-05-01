namespace TicTacToeAdvanced
{
    public class GameController
    {
        private GameCore gameCore;
        private IGameInterface gameInterface;

        public IGameInterface GameInterface
        {
            get { return gameInterface; }
            set
            {
                gameCore.GameInterface = value;
                gameInterface = value;
            }
        }


        public void StartNewGame(PlayerType startingPlayer)
        {
            gameCore = new GameCore(startingPlayer);
            if (GameInterface != null)
            {
                gameCore.GameInterface = GameInterface;
            }
        }

        public void MakeTurn(int x, int y)
        {
            gameCore.MakeTurn(x, y);
        }
    }
}