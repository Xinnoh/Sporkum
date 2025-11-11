using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class HorizontalCardHolder : MonoBehaviour
{

    [Header("Spawn Settings")]
    [SerializeField] private GameObject slotPrefab;
    private RectTransform rect;

    private int cardsToSpawn = 2;
    [HideInInspector] public List<Card> cards;

    bool isCrossing = false;
    [SerializeField] private bool tweenCardReturn = true;

    [SerializeField] private HorizontalCardHolder otherHolder, diceHolder;

    [Tooltip("If true, on start, takes vals from player slot and applies them to move slot")]
    [SerializeField] private GroupType groupType = GroupType.Moves;
    public GroupType GroupType => groupType;

    public PartyManager partyManager;
    private PartyMembers partyMembersObject;
    private bool isEnemy;

    private bool canSelectCards;

    [Header("Debug")]
    [SerializeField] private Card selectedCard;
    [SerializeReference] private Card hoveredCard;

    void Start()
    {
    }

    public void StartCombat()
    {
        if (partyManager != null)
        {
            partyMembersObject = partyManager.party;

            isEnemy = partyManager.isEnemy;
            cardsToSpawn = partyMembersObject.members.Length;
        }

        for (int i = 0; i < cardsToSpawn; i++)
        {

            GameObject slot = Instantiate(slotPrefab, transform);
            CharacterData currentCharacter = partyMembersObject.members[i];

            if (groupType == GroupType.Character || groupType == GroupType.Moves)
            {
                Card card = slot.GetComponentInChildren<Card>();
                if (card != null && partyMembersObject != null && i < partyMembersObject.members.Length)
                {
                    card.characterData = currentCharacter;
                    card.isEnemy = isEnemy;
                }
            }

            else if(groupType == GroupType.Dice)
            {
                Dice dice = slot.GetComponentInChildren<Dice>();
                if(dice != null)
                {
                    dice.SetMaxDieValue(currentCharacter);
                }
            }

        }


        rect = GetComponent<RectTransform>();
        cards = GetComponentsInChildren<Card>().ToList();

        int cardCount = 0;

        foreach (Card card in cards)
        {
            card.PointerEnterEvent.AddListener(CardPointerEnter);
            card.PointerExitEvent.AddListener(CardPointerExit);
            card.BeginDragEvent.AddListener(BeginDrag);
            card.EndDragEvent.AddListener(EndDrag);
            card.name = cardCount.ToString();
            cardCount++;
        }

        StartCoroutine(Frame());

        IEnumerator Frame()
        {
            yield return new WaitForSecondsRealtime(.1f);
            for (int i = 0; i < cards.Count; i++)
            {
                var currCardVis = cards[i].cardVisual;

                if (currCardVis != null) continue;
                
                currCardVis.UpdateIndex(transform.childCount);
            }


            SyncMovesToDice();
        }

    }


    public void SyncMovesToDice()
    {
        if (groupType == GroupType.Moves)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                var card = cards[i];
                if (card == null || card.cardVisual == null) continue;

                int parentIndex = card.ParentIndex();

                var matchingDie = diceHolder.cards.FirstOrDefault(d => d != null && d.ParentIndex() == parentIndex);
                var diceScript = matchingDie?.GetComponent<Dice>();
                if (diceScript == null) continue;

                card.cardVisual.UpdateAttack(diceScript.currDiceVal);
                otherHolder.cards[i].attackHandler.UpdateAttackIndex(diceScript.currDiceVal);
                
            }
        }

    }


    private void BeginDrag(Card card)
    {
        selectedCard = card;
    }


    void EndDrag(Card card)
    {
        if (selectedCard == null)
            return;

        selectedCard.transform.DOLocalMove(selectedCard.selected ? new Vector3(0,selectedCard.selectionOffset,0) : Vector3.zero, tweenCardReturn ? .15f : 0).SetEase(Ease.OutBack);

        rect.sizeDelta += Vector2.right;
        rect.sizeDelta -= Vector2.right;

        selectedCard = null;
    }

    void CardPointerEnter(Card card)
    {
        hoveredCard = card;
    }

    void CardPointerExit(Card card)
    {
        hoveredCard = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (hoveredCard != null)
            {
                if (otherHolder != null)
                {
                    int indexToRemove = hoveredCard.ParentIndex();
                    Card cardToRemove = otherHolder.cards.FirstOrDefault(c => c.ParentIndex() == indexToRemove);
                    if (cardToRemove != null)
                    {
                        Destroy(cardToRemove.transform.parent.gameObject);
                        otherHolder.cards.Remove(cardToRemove);
                    }
                }



                Destroy(hoveredCard.transform.parent.gameObject);
                cards.Remove(hoveredCard);

            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            foreach (Card card in cards)
            {
                card.Deselect();
            }
        }

        if (selectedCard == null)
            return;

        if (isCrossing)
            return;

        for (int i = 0; i < cards.Count; i++)
        {

            if (selectedCard.transform.position.x > cards[i].transform.position.x)
            {
                if (selectedCard.ParentIndex() < cards[i].ParentIndex())
                {
                    Swap(i);
                    break;
                }
            }

            if (selectedCard.transform.position.x < cards[i].transform.position.x)
            {
                if (selectedCard.ParentIndex() > cards[i].ParentIndex())
                {
                    Swap(i);
                    break;
                }
            }
        }
    }


    public void SetTurnActive(bool active)
    {
        canSelectCards = active;
        foreach (var card in cards)
        {
            if (card != null)
            {

                card.CardSelectable(active);

                if (!active)
                {
                    if(card.selected)
                        card.Deselect();


                    // BUG: This doesn't actually work. Just don't hold while dragging.
                    if (card.isDragging)
                        EndDrag(card);
                    
                }
            }


        }
    }

    public bool IsTurnActive()
    {
        return canSelectCards;
    }



    void Swap(int index)
    {
        int selectedIndex = cards.IndexOf(selectedCard);
        SwapCards(selectedCard, cards[index]);

        if (otherHolder != null)
            otherHolder.ExternalSwap(selectedIndex, index);

        UpdateAllCardVisualIndexes();
    }

    public void ExternalSwap(int currCardIndex, int index)
    {
        SwapCards(cards[currCardIndex], cards[index]);
        UpdateAllCardVisualIndexes();
    }

    private void SwapCards(Card a, Card b)
    {
        isCrossing = true;

        Transform parentA = a.transform.parent;
        Transform parentB = b.transform.parent;

        a.transform.SetParent(parentB);
        b.transform.SetParent(parentA);

        a.transform.localPosition = a.selected ? new Vector3(0, a.selectionOffset, 0) : Vector3.zero;
        b.transform.localPosition = b.selected ? new Vector3(0, b.selectionOffset, 0) : Vector3.zero;

        if (a.cardVisual != null && b.cardVisual != null)
        {
            bool swapIsRight = b.ParentIndex() > a.ParentIndex();
            a.cardVisual.Swap(swapIsRight ? 1 : -1);
            b.cardVisual.Swap(swapIsRight ? -1 : 1);
        }

        isCrossing = false;
    }

    private void UpdateAllCardVisualIndexes()
    {
        foreach (Card card in cards)
        {
            card.cardVisual?.UpdateIndex(transform.childCount);
        }

        SyncMovesToDice();
    }

    public HorizontalCardHolder GetDiceHolder()
    {
        return diceHolder;
    }

    public HorizontalCardHolder GetOtherHolder()
    {
        return otherHolder;
    }
}
