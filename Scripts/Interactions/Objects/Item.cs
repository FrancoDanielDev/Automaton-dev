using System.Collections;
using UnityEngine;
using System;

public abstract class Item : GridEntity
{
    [SerializeField] protected float _closeness = 0.1f;
    [SerializeField] protected GameObject _visualModel;
    [SerializeField] protected ParticleSystem _particle;

    protected Transform _entity;
    protected float _speed;
    protected WorldState _wS;
    protected Action _method;
    protected IAction _interaction;

    public virtual Item Set(Transform entity, float speed, WorldState wS, Action method, IAction interaction)
    {
        _entity = entity;
        _speed = speed;
        _wS = wS;
        _method = method;
        _interaction = interaction;

        return this;
    }

    public virtual void DoAction()
    {
        StartCoroutine(MoveTowards());

        IEnumerator MoveTowards()
        {
            while (Vector3.Distance(_entity.position, transform.position) > _closeness)
            {
                _entity.position = Vector3.MoveTowards(_entity.position, transform.position, _speed * Time.deltaTime);
                yield return null;
            }

            ApplyEffect();
        }
    }

    protected virtual void ApplyEffect()
    {
        _wS = _interaction.Effect(_wS);
        _method.Invoke();
        ObjectEnding();
    }

    protected virtual void ObjectEnding()
    {
        _particle.Play();
    }
}
