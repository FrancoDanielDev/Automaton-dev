using System;
using UnityEngine;
using System.Linq;

//[ExecuteInEditMode]
public abstract class GridEntity : MonoBehaviour
{
    public event Action<GridEntity> OnMove = delegate {};

    [HideInInspector] public SpatialGrid myGrid;
    [HideInInspector] public bool onGrid;

    protected virtual void Awake()
    {
        myGrid = GetComponentsInParent<SpatialGrid>().First();       
    }

    protected virtual void Start()
    {
        if (myGrid == null) return;

        OnMove += myGrid.UpdateEntity;
        myGrid.UpdateEntity(this);
    }

    protected virtual void Update()
    {
	    OnMove(this);
	}

    /*
    public IEnumerable<T> GetNearby<T>() where T : GridEntity
    {
        return myGrid.Query(
            transform.position + new Vector3(-radius, 0, -radius),
            transform.position + new Vector3(radius, 0, radius),
            x =>
            {
                var position2D = x - transform.position;
                position2D.y = 0;
                return position2D.sqrMagnitude < radius * radius;
            })
            .OfType<T>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);
    }*/
}
