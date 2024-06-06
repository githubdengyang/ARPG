using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace RPG.Core
{
	public interface IAction
	{
		void Cancel();
	}
}