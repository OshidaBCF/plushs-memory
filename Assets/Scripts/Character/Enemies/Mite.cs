public class Mite : Enemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void AbilitiesInitialization()
    {
        _ability1.Damage = 1;
        _ability1.Cost = 1;
        _ability2.Damage = 2;
        _ability2.Cost = 2;
    }

    public override void CastAbility1(Entity target)
    {
        base.CastAbility1(target);
        animator.SetTrigger("Attack");
    }
    public override void CastAbility2(Entity target)
    {
        CurrentAP -= _ability2.Cost;
        animator.SetTrigger("Attack");
        target.TakeDamage(_ability2.Damage + Attack.GetValue());
    }
}
