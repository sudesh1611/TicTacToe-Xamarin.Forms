using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tester.ViewModels
{
    public class GameArenaViewModel : INotifyPropertyChanged
    {
        /* Private Fields */

        bool isPlayerTurn;
        bool isOpponentTurn;
        bool isGameOver;
        bool isLoading;
        string playerName;
        string opponentName;
        char playerSymbol;
        char opponentSymbol;
        long playerWins;
        long playerLoses;
        long playerTies;
        long opponentWins;
        long opponentLoses;
        long opponentTies;
        char gameResult;
        int filled;
        char[,] arenaMatrix = new char[3, 3] { { ' ', ' ', ' ' }, { ' ', ' ', ' ' }, { ' ', ' ', ' ' } };

        /* Public Fields */

        public event PropertyChangedEventHandler PropertyChanged;
        
        public string PlayerName
        {
            get
            {
                return playerName;
            }
            set
            {
                playerName = value;
                OnPropertyChanged();
            }
        }
        public string OpponentName
        {
            get
            {
                return opponentName;
            }
            set
            {
                opponentName = value;
                OnPropertyChanged();
            }
        }
        public char PlayerSymbol
        {
            get
            {
                return playerSymbol;
            }
            set
            {
                playerSymbol = value;
                OnPropertyChanged();
            }
        }
        public char OpponentSymbol
        {
            set
            {
                opponentSymbol = value;
                OnPropertyChanged();
            }
            get
            {
                return opponentSymbol;
            }
        }
        public bool IsOpponentTurn
        {
            get
            {
                return isOpponentTurn;
            }
            set
            {
                isOpponentTurn = value;
                OnPropertyChanged();
            }
        }

        /* private Field Definitions */

        public bool IsPlayerTurn
        {
            get
            {
                return isPlayerTurn;
            }
            set
            {
                isPlayerTurn = value;
                IsOpponentTurn = !value;
                OnPropertyChanged();
            }
        }

        public bool IsGameOver
        {
            get
            {
                return isGameOver;
            }
            set
            {
                isGameOver = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            set
            {
                isLoading = value;
                OnPropertyChanged();
            }
            get
            {
                return isLoading;
            }
        }

        public char[,] ArenaMatrix
        {
            get
            {
                return arenaMatrix;
            }
            set
            {
                arenaMatrix = value;
                OnPropertyChanged();
            }
        }

        public char this[int i, int j]
        {
            get
            {
                return arenaMatrix[i, j];
            }
            set
            {
                arenaMatrix[i, j] = value;
                OnPropertyChanged(nameof(ArenaMatrix));
            }
        }

        public long PlayerWins
        {
            get
            {
                return playerWins;
            }
            set
            {
                playerWins = value;
                OnPropertyChanged();
            }
        }

        public long PlayerLoses
        {
            get
            {
                return playerLoses;
            }
            set
            {
                playerLoses = value;
                OnPropertyChanged();
            }
        }

        public long PlayerTies
        {
            get
            {
                return playerTies;
            }
            set
            {
                playerTies = value;
                OnPropertyChanged();
            }
        }

        public long OpponentWins
        {
            get
            {
                return opponentWins;
            }
            set
            {
                opponentWins = value;
                OnPropertyChanged();
            }
        }

        public long OpponentLoses
        {
            get
            {
                return opponentLoses;
            }
            set
            {
                opponentLoses = value;
                OnPropertyChanged();
            }
        }

        public long OpponentTies
        {
            get
            {
                return opponentTies;
            }
            set
            {
                opponentTies = value;
                OnPropertyChanged();
            }
        }

        public int Filled
        {
            get
            {
                return filled;
            }
            set
            {
                filled = value;
                OnPropertyChanged();
            }
        }

        public char GameResult
        {
            get
            {
                return gameResult;
            }
            private set
            {
                gameResult = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void UpdateGameArena(int[] index)
        {
            bool IfWon = false;
            char symbol = (this.IsPlayerTurn) ? this.PlayerSymbol : this.OpponentSymbol;
            this.ArenaMatrix[index[0], index[1]] = symbol;
            

            if (this.ArenaMatrix[0,0] == symbol && this.ArenaMatrix[0,1] == symbol && this.ArenaMatrix[0,2] == symbol)
            {
                IfWon = true;
            }
            else if (this.ArenaMatrix[1,0] == symbol && this.ArenaMatrix[1,1] == symbol && this.ArenaMatrix[1,2] == symbol)
            {
                IfWon = true;
            }
            else if (this.ArenaMatrix[2,0] == symbol && this.ArenaMatrix[2,1] == symbol && this.ArenaMatrix[2,2] == symbol)
            {
                IfWon = true;
            }
            else if (this.ArenaMatrix[0,0] == symbol && this.ArenaMatrix[1,0] == symbol && this.ArenaMatrix[2,0] == symbol)
            {
                IfWon = true;
            }
            else if (this.ArenaMatrix[0,1] == symbol && this.ArenaMatrix[1,1] == symbol && this.ArenaMatrix[2,1] == symbol)
            {
                IfWon = true;
            }
            else if (this.ArenaMatrix[0,2] == symbol && this.ArenaMatrix[1,2] == symbol && this.ArenaMatrix[2,2] == symbol)
            {
                IfWon = true;
            }
            else if (this.ArenaMatrix[0,0] == symbol && this.ArenaMatrix[1,1] == symbol && this.ArenaMatrix[2,2] == symbol)
            {
                IfWon = true;
            }
            else if (this.ArenaMatrix[0,2] == symbol && this.ArenaMatrix[1,1] == symbol && this.ArenaMatrix[2,0] == symbol)
            {
                IfWon = true;
            }
            else if (this.ArenaMatrix[0,0] != ' ' && this.ArenaMatrix[0,1] != ' ' && this.ArenaMatrix[0,2] != ' ' && this.ArenaMatrix[1,0] != ' ' && this.ArenaMatrix[1,1] != ' ' && this.ArenaMatrix[1,2] != ' ' && this.ArenaMatrix[2,0] != ' ' && this.ArenaMatrix[2,1] != ' ' && this.ArenaMatrix[2,2] != ' ')
            {
                this.GameResult = 'T';
                this.IsPlayerTurn = false;
                this.IsOpponentTurn = false;
                this.IsGameOver = true;
            }
            else
            {
                this.IsPlayerTurn = !IsPlayerTurn;
            }

            if(IfWon==true)
            {
                if (symbol == this.playerSymbol)
                {
                    this.GameResult = 'P';
                }
                else
                {
                    this.GameResult = 'O';
                }
                this.IsPlayerTurn = false;
                this.IsOpponentTurn = false;
                this.IsGameOver = true;
            }

        }

    }
}
