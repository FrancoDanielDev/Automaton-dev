using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class Goap : MonoBehaviour
{
    public static IEnumerable<GOAPAction> Execute(GoapState from, GoapState to, Func<GoapState, bool> satisfies, Func<GoapState, float> h, IEnumerable<GOAPAction> actions, int watchDog = 200)
    {
        int watchdog = watchDog;

        IEnumerable<GoapState> seq = AStarNormal<GoapState>.Run(
            from,
            to,
            (curr, goal) => h(curr),
            satisfies,
            curr =>
            {
                if (watchdog == 0)
                    return Enumerable.Empty<AStarNormal<GoapState>.Arc>();
                else
                    watchdog--;

                return actions
                    .Where(action => action.Preconditions(curr))
                    .Aggregate(new FList<AStarNormal<GoapState>.Arc>(), (possibleList, action) =>
                    {         
                        var newState = new GoapState(curr);
                        newState = action.Effects(newState);
                        newState.generatingAction = action;
                        newState.step = curr.step + 1;
                        return possibleList + new AStarNormal<GoapState>.Arc(newState, action.Cost);
                    });
            });

        if (seq == null)
        {
            return null;
        }

        return seq.Skip(1).Select(x => x.generatingAction);
    }
}
