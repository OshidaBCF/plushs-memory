using UnityEngine;
using UnityEngine.Events;

 public class TurnSystem : MonoBehaviour
{
    public enum FightPhase 
    {
        INIT,
        PLAYERTURN,
        ENEMIETURN,
        WIN,
        LOSE
    }

    private GameObject Player;
    private GameObject Enemie;

    private UnityEvent EndTurn;  

    public FightPhase CurentState = FightPhase.INIT;
   
    void Start()
    {
         SetUpBattle();
    }

    void Update()
    {
        StateSwitch();
    }

    private void SetUpBattle()
    {
        Instantiate(Player);
        Instantiate(Enemie); //replace by liste
        CurentState = FightPhase.PLAYERTURN;
    }

    private void PlayerTurn()
    {
        Debug.Log("TurnPlayer");
        OnEndTurnButton();
    }

    public void EnemieTurn()
    {
        Debug.Log("TurnEnemey");
/*      if( nbEnemie <=0 && Player.health > 0 )
        {
            current_state = EnumTurn.Win
        }

        else if( Player.health <= 0)
        {
            current_state = EnumTurn.lose;
        }*/
        CurentState = FightPhase.PLAYERTURN;
    }

    private void StateSwitch()
    {
        
        switch(CurentState)
        {
            case FightPhase.PLAYERTURN:
            PlayerTurn();
            break;

            case FightPhase.ENEMIETURN:
            EnemieTurn();
            break;

            case FightPhase.WIN:
            Win();
            break;
            
            case FightPhase.LOSE:
            Lose();
            break;

            default:
            break;
        }
    }


    private void Win()
    {
        Debug.Log("Win");
    }

    private void Lose()
    {
        Debug.Log("lose");
    }

    public void OnEndTurnButton()
    {
        if (CurentState!= FightPhase.PLAYERTURN)
        {
            return;
        }
        /*if( nbEnemie <=0 && Player.health > 0 )
        {
            current_state = EnumTurn.Win
        }
        else if( Player.health <= 0)
        {
            current_state = EnumTurn.lose;
        }*/
        else
        {
            CurentState = FightPhase.ENEMIETURN;
        }
    }
}
