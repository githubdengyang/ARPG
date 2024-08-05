﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using RPG.Core;

namespace RPG.Dialogue
{
	public class PlayerConversant : MonoBehaviour
	{
		Dialogue currentDialogue;
		DialogueNode currentNode = null;
		bool isChoosing = false;

		public event Action onConversationUpdated;
		AIConversant currentConversant = null;
		[SerializeField] string playerName;

		public void StartDialogue(AIConversant newConversant, Dialogue newDialogue)
		{
			currentConversant = newConversant;
			currentDialogue = newDialogue;
			currentNode = currentDialogue.GetRootNode();
			TriggerEnterAction();
			onConversationUpdated();
		}

		public void Quit()
		{
			currentDialogue = null;
			TriggerExitAction();
			currentNode = null;
			isChoosing = false;
			currentConversant = null;
			onConversationUpdated();
		}

		public bool IsActive()
		{
			return currentDialogue != null;
		}

		public bool IsChoosing()
		{
			return isChoosing;
		}

		public string GetText()
		{
			if (currentNode == null)
			{
				return "";
			}

			return currentNode.GetText();
		}

		public void Next()
		{
			int numPlayerResponses = FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode)).Count();
			if (numPlayerResponses > 0)
			{
				isChoosing = true;
				TriggerExitAction();
				onConversationUpdated();
				return;
			}

			DialogueNode[] children = FilterOnCondition(currentDialogue.GetAIChildren(currentNode)).ToArray();
			int randomIndex = UnityEngine.Random.Range(0, children.Count());
			TriggerExitAction();
			currentNode = children[randomIndex];
			TriggerEnterAction();
			onConversationUpdated();
		}

		public bool HasNext()
		{
			return FilterOnCondition(currentDialogue.GetAllChildren(currentNode)).Count() > 0;
		}

		public IEnumerable<DialogueNode> GetChoices()
		{
			return FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode));
		}

		public void SelectChoice(DialogueNode chosenNode)
		{
			currentNode = chosenNode;
			TriggerEnterAction();
			isChoosing = false;
			Next();
		}

		

		private void TriggerEnterAction()
		{
			if (currentNode != null)
			{
				TriggerAction(currentNode.GetOnEnterAction());
			}
		}

		private void TriggerExitAction()
		{
			if (currentNode != null )
			{
				TriggerAction(currentNode.GetOnExitAction());
			}
		}

		private void TriggerAction(string action)
		{
			if (action == "") return;

			foreach (DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>())
			{
				trigger.Trigger(action);
			}
		}

		public string GetCurrentConversantName()
		{
			if (isChoosing)
			{
				return playerName;
			}
			else
			{
				return currentConversant.GetName();
			}
		}

		private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNode)
		{
			foreach (var node in inputNode)
			{
				if (node.CheckCondition(GetEvaluators()))
				{
					yield return node;
				}
			}
		}

		private IEnumerable<IPredicateEvaluator> GetEvaluators()
		{
			return GetComponents<IPredicateEvaluator>();
		}
	}
}