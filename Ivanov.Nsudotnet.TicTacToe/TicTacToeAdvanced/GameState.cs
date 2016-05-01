namespace TicTacToeAdvanced
{
    public class GameState
    {
        public GameState(FieldCell[,] field, Winner winner, PlayerType nextTurnPlayer)
        {
            this.Field = field;
            this.Winner = winner;
            NextTurnPlayer = nextTurnPlayer;
        }

        public FieldCell[,] Field { get; private set; }
        public Winner Winner { get; private set; }

        public PlayerType NextTurnPlayer { get; private set; }
    }
}