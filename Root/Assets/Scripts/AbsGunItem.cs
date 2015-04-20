using UnityEngine;
using System.Collections;

public abstract class AbsGunItem : AbsItem
{
    protected AbsGunItem( string name, bool isDoubleHanded):
		base(name, isDoubleHanded)	{
	}
}

