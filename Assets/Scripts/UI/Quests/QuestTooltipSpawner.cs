using System.Collections;
using System.Collections.Generic;
using GameDevTV.Core.UI.Tooltips;
using UnityEngine;
using RPG.Quests;

namespace RPG.UI.Quests
{
	public class QuestTooltipSpawner : TooltipSpawner
	{
		public override bool CanCreateTooltip()
		{
			return true;
		}

		public override void UpdateTooltip(GameObject tooltip)
		{
			Quest quest = GetComponent<QuestItemUI>().GetQuest();
			tooltip.GetComponent<QuestTooltipUI>().Setup(quest);
		}
	}
}