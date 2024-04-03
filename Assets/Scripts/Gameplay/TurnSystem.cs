using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystem : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyPrefabs;
    public enum FightPhase
    {
        INIT,
        PLAYERTURN,
        ENEMYTURN,
        WIN,
        LOSE
    }

    CombatGrid grid;
    private Player _player;

    private Entity _entity;
    private List<Enemy> _enemies = new List<Enemy>();

    [SerializeField] Text _playerCurrentHPText;
    [SerializeField] Text _playerMaxHPText;

    bool playerTurnInitalized = false;
    bool enemyTurnInitalized = false;

    public FightPhase CurrentState = FightPhase.INIT;

    void Start()
    {
        grid = GameObject.FindWithTag("CombatGrid").GetComponent<CombatGrid>();
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        SetUpBattle();
        Enemy[] _enemiesArray = FindObjectsOfType<Enemy>();
        _enemies.AddRange(_enemiesArray);
    }

    void Update()
    {
        StateSwitch();
    }

    private void SetUpBattle()
    {
        int x = -1;
        int y = -1;
        GameObject WeakEnemy = Instantiate(enemyPrefab, new Vector3(-1, 0.01f, -1), Quaternion.identity);
        WeakEnemy.GetComponent<WeakEnemy>().name = "Enemy";
        WeakEnemy.GetComponent<WeakEnemy>().CurrentPos = new Coord(-1, -1);
        WeakEnemy.GetComponent<WeakEnemy>().speed = 2;

        grid.AddEnemy(new Coord(x + grid.GetMaxX() / 2, y + grid.GetMaxY() / 2), WeakEnemy.GetComponent<Enemy>());
        CurrentState = FightPhase.PLAYERTURN;
    }

    private void PlayerTurn()
    {
        if (!playerTurnInitalized)
        {
            _player.CurrentAP = _player.MaxAP.GetValue();
            playerTurnInitalized = true;
            enemyTurnInitalized = false;
        }
        //Debug.Log("TurnPlayer");
    }
 
    public void EnemyTurn()
    {

        if (!enemyTurnInitalized)
        {
            playerTurnInitalized = false;
            enemyTurnInitalized = true;
        }

        for (int i = 0; i < _enemies.Count; i++)
        {
            _enemies[i].ItsTurn = true;
            UpdatePlayerHPText();
        }
        CurrentState = FightPhase.PLAYERTURN;
    }

    private void StateSwitch()
    {
        switch (CurrentState)
        {
            case FightPhase.PLAYERTURN:
                UpdatePlayerHPText();
                PlayerTurn();
                break;

            case FightPhase.ENEMYTURN:
                EnemyTurn();
                _player.ResetAP();
                break;

            case FightPhase.WIN:
                Win();
                break;

            case FightPhase.LOSE:
                Lose();
                break;

            default:
                CurrentState = FightPhase.INIT;
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
        if (CurrentState != FightPhase.PLAYERTURN)
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
            CurrentState = FightPhase.ENEMYTURN;
        }
    }

    public void UpdatePlayerHPText()
    {
        _playerCurrentHPText.text = _player.GetComponent<Entity>().CurrentHP.ToString();
        _playerMaxHPText.text = _player.GetComponent<Entity>().MaxHP.GetValue().ToString();
    }

    #region Attaque


    public void OnAbility1Button()
    {
        _entity = _player.GetEnemy();
        if (CurrentState != FightPhase.PLAYERTURN || _entity == null)
        {
            Debug.Log("no enemy select");
            return;
        }
        else
        {
            _player.DebugEnemyStr();
            _player.CastAbility1(_entity);
        }
    }

    public void OnAbility2Button()
    {
        _entity = _player.GetEnemy();
        if (CurrentState != FightPhase.PLAYERTURN || _entity == null)
        {
            return;
        }
        else
        {
            _player.DebugEnemyStr();
            _player.CastAbility2(_entity);
            //detection porter
        }
    }

    public void OnFriend1Button()
    {
        if (CurrentState != FightPhase.PLAYERTURN)
        {
            return;
        }
        else
        {
            _player.CastFriendAbility1();
            Debug.Log("Friend Ability 1");
        }
    }

    public void OnFriend2Button()
    {
        if (CurrentState != FightPhase.PLAYERTURN)
        {
            return;
        }
        else
        {
            _player.CastFriendAbility2();
            Debug.Log("Friend Ability 2");
        }
    }

        public void OnFriend3Button()
    {
        if (CurrentState != FightPhase.PLAYERTURN)
        {
            return;
        }
        else
        {
            _player.CastFriendAbility3();
            Debug.Log("Friend Ability 3");
        }
    }
#endregion
}
