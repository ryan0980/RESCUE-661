using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Singleton is a Generic class, this means that we can provide a Type when we inherit from it
//The 'where' means that we force the T-parameter, to inherit from this class as well
//We call this a 'constraint'
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{

	//We create a variable of the type 'T'. This T is provided by any inheriting class
	public static T instance;

	protected virtual void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarningFormat("{0} Singleton already exists in the scene! Destroying this version...", this.name);
			Destroy(gameObject);
			return;
		}
		//Just like any Singleton, we assign the static variable to the object
		//We have to cast 'this' from the type Singleton, to the type 'T'
		//This basically tells C# that this object really is of the type T
		instance = (T)this;
	}

}
