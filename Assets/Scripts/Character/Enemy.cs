using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum State
{
    WaitForTurn,
    Movement,
    Attacking,
    EndTurn
}

public abstract class Enemy : Entity
{
    private State _currentState;
    public bool _itsTurn = false;

    CombatGrid grid;

    protected override void Start()
    {
        base.Start();
        grid = GameObject.FindWithTag("CombatGrid").GetComponent<CombatGrid>();
        _currentState = State.WaitForTurn;
    }

    protected virtual void Update()
    {
        switch (_currentState)
        {
            case State.WaitForTurn:
                if (_itsTurn)
                {
                    CurrentAP = MaxAP.GetValue();
                    ChangeState(State.Movement);
                }
                break;
            case State.Movement:

                bool hasFinishedMoving = Movement();

                if (hasFinishedMoving)
                    ChangeState(State.Attacking);
                break;
            case State.Attacking:

                Attacking();

                ChangeState(State.EndTurn);
                break;
            case State.EndTurn:

                _itsTurn = false;

                ChangeState(State.WaitForTurn);
                break;
        }
    }

    private bool Movement()
    {
        if (_isMoving)
        {
            _isMoving = !MoveOverTime();
        }
        else
        {
            // Check if the player is reachable
            Player _player = FindObjectOfType<Player>();
            List<Cell> _pathToPlayer =  AStar.FindPath(CurrentPos, _player.CurrentPos, grid.GetGridCells(), grid.GetMaxX(), grid.GetMaxY());

            _pathToPlayer.RemoveAt(_pathToPlayer.Count - 1);

            if ( _pathToPlayer.Count > 1)
            {
                if(_pathToPlayer.Count - 1 > CurrentAP)
                {
                    _pathToPlayer.RemoveRange(CurrentAP + 1, _pathToPlayer.Count - CurrentAP - 1);
                }

                // Move to the position
                CurrentAP -= _pathToPlayer.Count - 1;

                Move(_pathToPlayer);
                grid.GetGridCell(CurrentPos.X, CurrentPos.Y).HasEnemy = false;
                CurrentPos = _pathToPlayer.Last().Coord;
                grid.GetGridCell(CurrentPos.X, CurrentPos.Y).HasEnemy = true;
                _player.RefreshGridMat();
            }
        }
        return !_isMoving;

        // Verify he arrived at the position / Wait till animation is done
    }

    private void Attacking()
    {
        // Check if the player is reachable
        
        //// A MODIFIER EN CALCULANT LA DISTANCE DEPUIS LA CLASSE CELL
        Player _player = FindObjectOfType<Player>();
        List<Cell> _pathToPlayer = AStar.FindPath(CurrentPos, _player.CurrentPos, grid.GetGridCells(), grid.GetMaxX(), grid.GetMaxY());
        if (_pathToPlayer.Count == 2)
        {
            if (CurrentAP > 0)
            {
                if (CurrentAP >= _ability2.Cost)
                {
                    CastAbility2(_player);
                }
                else if (CurrentAP >= _ability1.Cost)
                {
                    CastAbility1(_player);
                }

                // Verify Attack is done / Wait till animation is done

            }
        }
        //// A MODIFIER


    }

    void ChangeState(State newState)
    {
        _currentState = newState;
    }

    public override void Death()
    {
        // Enemy Death / Inform GameManager
    }
}