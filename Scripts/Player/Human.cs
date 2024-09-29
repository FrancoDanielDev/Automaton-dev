using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using IA2;

[Serializable]
public class WorldState
{
    [Range(0, 10)]
    public int coins;
    [Range(0.1f, 10f)]
    public float muscularity;
    [Range(0f, 1f)]
    public float stealth;
    public bool hasKey;
    public bool doorOpen;
    public Weapon weapon;

    public WorldState Clone()
    {
        return new WorldState
        {
            coins = this.coins,
            muscularity = this.muscularity,
            stealth = this.stealth,
            hasKey = this.hasKey,
            doorOpen = this.doorOpen,
            weapon = this.weapon
        };
    }
}

public enum HumanAction { Null, Idle, Victory, BuySword, BuyKnife,
                          KillOrc, OpenDoor, GrabCoin, StealKey, OpenChest }

public enum Weapon { BareHands, Sword, Knife }

public class Human : GridQuery
{
    [Header("NORMAL VARIABLES")]
    [Space]
    [SerializeField] private float _startPlanDelay = 0.5f;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private int _watchdog = 200;
    [Space]
    [SerializeField] private WorldState _myWorldState;
    [Space] [Space]
    [SerializeField] private GameObject _sword;
    [SerializeField] private GameObject _knife;
    [SerializeField] private GameObject _key;
    [Space]
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private AudioSource _victoryAudio;
    [SerializeField] private MenuManager _menuManager;

    private WorldState _savedTrigger = new();
    private Queue<string> _allSteps = new();
    private List<State<string>> _allStates = new();
    private List<Item> _items;

    private EventFSM<string> _FSM;
    private Vector3 _originalLocation;

    protected override void Awake()
    {
        base.Awake();
        _originalLocation = transform.position;
        SetFSM();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        _FSM.Update();
    }

    public void StartGamePlan(WorldState wS)
    {
        _myWorldState = wS;
        SetInitialValues();
        this.StartDelayedMethod(ExecutePlan, _startPlanDelay);
    }

    public void ResetHuman()
    {
        transform.position = _originalLocation;
        _sword.SetActive(false);
        _knife.SetActive(false);
        _key.SetActive(false);
    }

    private void SetInitialValues()
    {
        _savedTrigger = _myWorldState.Clone();
        _items = Query<Item>().ToList();

        if (_myWorldState.doorOpen) _items.OfType<Door>().First().OpenTheDoor();
        if (_myWorldState.hasKey) TurnObject(_key);

        switch (_myWorldState.weapon)
        {
            case Weapon.Sword: TurnObject(_sword);
                break;
            case Weapon.Knife: TurnObject(_knife);
                break;
        }
    }

    private void SetFSM()
    {
        #region Creating States and their Transitions

        var Null      = CreateState(HumanAction.Null);
        var Idle      = CreateState(HumanAction.Idle);
        var Victory   = CreateState(HumanAction.Victory);
        var BuySword  = CreateState(HumanAction.BuySword);
        var BuyKnife    = CreateState(HumanAction.BuyKnife);
        var KillOrc   = CreateState(HumanAction.KillOrc);
        var OpenDoor  = CreateState(HumanAction.OpenDoor);
        var GrabCoin  = CreateState(HumanAction.GrabCoin);
        var StealKey  = CreateState(HumanAction.StealKey);
        var OpenChest = CreateState(HumanAction.OpenChest);

        CreateTransition(Null);
        CreateTransition(Idle);
        CreateTransition(Victory);
        CreateTransition(BuySword);
        CreateTransition(BuyKnife);
        CreateTransition(KillOrc);
        CreateTransition(OpenDoor);
        CreateTransition(GrabCoin);
        CreateTransition(StealKey);
        CreateTransition(OpenChest);

        State<string> CreateState(HumanAction name)
        {
            var state = new State<string>(name.ToString());
            _allStates.Add(state);
            return state;
        }

        void CreateTransition(State<string> state)
        {
            var st = StateConfigurer.Create(state);

            _allStates
                .Where(transition => transition.Name != state.Name)
                .ToList()
                .ForEach(transition => st.SetTransition(transition.Name, transition));

            st.Done();
        }

        #endregion

        #region Applying functionality to every Transition

        Idle.OnEnter += x => Debug.Log("Idle");

        Victory.OnEnter += x =>
        {
            _particle.Play();
            _victoryAudio.Play();
            _menuManager.RestartGame();
            Debug.Log("Victory!! Let's celebrate!");
        };

        BuySword.OnEnter += x => DoSequence(_items.OfType<Seller>(), new BuySword());
        BuySword.OnExit  += x => TurnObject(_sword, _knife);

        BuyKnife.OnEnter += x => DoSequence(_items.OfType<Seller>(), new BuyKnife());
        BuyKnife.OnExit  += x => TurnObject(_knife, _sword);

        KillOrc.OnEnter += x => DoSequence(_items.OfType<Orc>(), new KillOrc());
        KillOrc.OnExit  += x => TurnObject(_key);

        OpenDoor.OnEnter += x => DoSequence(_items.OfType<Door>(), new OpenDoor());

        GrabCoin.OnEnter += x => DoSequence(_items.OfType<Coin>(), new GrabCoin());

        StealKey.OnEnter += x => DoSequence(_items.OfType<Orc>(), new StealKey());
        StealKey.OnExit  += x => TurnObject(_key);

        OpenChest.OnEnter += x => DoSequence(_items.OfType<Chest>(), new OpenChest());

        #endregion

        // Default State
        _FSM = new EventFSM<string>(Idle);
    }

    private void DoSequence<T>(IEnumerable<T> items, IAction interaction) where T : Item
    {
        var result = items
            .OrderBy(x => Vector3.Distance(x.transform.position, transform.position))
            .First();

        result.Set(transform, _speed, _savedTrigger, NextMove, interaction)
              .DoAction();

        _items = _items.Where(x => x != result).ToList();
    }

    private void NextMove()
    {
        _myWorldState = _savedTrigger.Clone();

        if (!_allSteps.Any())
        {
            StopAutomaton();
            return;
        }

        var result = _allSteps.Dequeue();

        if (_FSM.Current.Name == result)
            _FSM.SendInput(HumanAction.Null.ToString());

        _FSM.SendInput(result);
    }

    private void StartAutomaton()
    {
        if (!_allSteps.Any())
        {
            StopAutomaton();
            return;
        }
        
        var result = _allSteps.Dequeue();
        var state = _allStates.FirstOrDefault(s => s.Name == result);
        if (state != null) _FSM = new EventFSM<string>(state);
    }

    private void StopAutomaton()
    {
        _FSM.SendInput(HumanAction.Victory.ToString());
    }

    private void TurnObject(GameObject on, GameObject off = null)
    {
        on.SetActive(true);
        if (off != null) off.SetActive(false);
    }

    private void ExecutePlan() 
    {
        GoapState initialState = new GoapState();
        initialState.worldState = _myWorldState.Clone();

        Func<GoapState, bool> goalState = (curr) =>
        {
            return curr.worldState.doorOpen &&
                   curr.worldState.coins >= 3;
        };

        Func<GoapState, float> heuristic = (curr) =>
        {
            int count = 0;

            if ( curr.worldState.coins < 3) count += 2;
            if (!curr.worldState.doorOpen)  count++;

            return count;
        };

        var actions = PossibleActionsList();
        var plan = Goap.Execute(initialState, null, goalState, heuristic, actions, _watchdog);

        if (plan == null)
        {
            Debug.Log("Couldn't plan");
            _menuManager.RestartGame();
            return;
        }

        _allSteps = new Queue<string>(plan.Select(action => action.Name));
        StartAutomaton();
    }

    private List<GOAPAction> PossibleActionsList()
    {
        List<IAction> allActions = new List<IAction>
        {
            new BuySword(),
            new BuyKnife(),
            new KillOrc(),
            new OpenDoor(),
            new GrabCoin(),
            new StealKey(),
            new OpenChest()
        };

        return allActions.Select(action => new GOAPAction(action.Name())
            .SetCost(action.Cost())
            .Pre((gS) => action.Precondition(gS))
            .Effect((gS) =>
            {
                gS.worldState = action.Effect(gS.worldState);
                return gS;
            }))
            .ToList();
    }
}
