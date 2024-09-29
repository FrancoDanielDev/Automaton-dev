using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class GridQuery : GridEntity
{
    [Header ("QUERY")]
    public bool isBox;
    public float radius = 20f;
    public float width = 15f;
    public float height = 30f;

    public IEnumerable<GridEntity> selected = new List<GridEntity>();

    public IEnumerable<T> Query<T>() where T : GridEntity
    {
        if (isBox)
        {
            var h = height * 0.5f;
            var w = width * 0.5f;

            return myGrid.Query(
                transform.position + new Vector3(-w, 0, -h),
                transform.position + new Vector3(w, 0, h),
                x => true)
                .Where(x => x != null && x.gameObject.activeInHierarchy)
                .OfType<T>();
        }
        else
        {
            return myGrid.Query(
                transform.position + new Vector3(-radius, 0, -radius),
                transform.position + new Vector3(radius, 0, radius),
                x => {
                    var position2d = x - transform.position;
                    position2d.y = 0;
                    return position2d.sqrMagnitude < radius * radius;
                })
                .Where(x => x != null && x.gameObject.activeInHierarchy)
                .OfType<T>();
        }
    }

    protected virtual void OnDrawGizmos()
    {
        if (myGrid == null)
            return;

        //Flatten the sphere we're going to draw
        Gizmos.color = Color.cyan;
        if (isBox)
            Gizmos.DrawWireCube(transform.position, new Vector3(width, 0, height));
        else
        {
            Gizmos.matrix *= Matrix4x4.Scale(Vector3.forward + Vector3.right);
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        if (Application.isPlaying)
        {
            selected = Query<GridEntity>();
            var temp = FindObjectsOfType<GridEntity>().Where(x=>!selected.Contains(x));
            foreach (var item in temp)
            {
                item.onGrid = false;
            }
            foreach (var item in selected)
            {
                item.onGrid = true;
            }

        }
    }


    protected virtual void OnGUI()
    {
        GUI.Label( new Rect(0,0,20,20), "HOLA");
    }
}
