using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class HorizontalCardHolder : MonoBehaviour
{

    [SerializeField] private Card selectedCard;
    [SerializeReference] private Card hoveredCard;

    [SerializeField] private GameObject slotPrefab;
    private RectTransform rect;

    [Header("Spawn Settings")]
    [SerializeField] private int cardsToSpawn = 7;
    public List<Card> cards;

    bool isCrossing = false;
    [SerializeField] private bool tweenCardReturn = true;

    [SerializeField] private HorizontalCardHolder otherHolder;

    public PartyManager partyManager;

    void Start()
    {
        for (int i = 0; i < cardsToSpawn; i++)
        {
            Instantiate(slotPrefab, transform);
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
                if (cards[i].cardVisual != null)
                    cards[i].cardVisual.UpdateIndex(transform.childCount);
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

    void Swap(int index)
    {
        int selectedIndex = cards.IndexOf(selectedCard);

        isCrossing = true;

        Transform focusedParent = selectedCard.transform.parent;
        Transform crossedParent = cards[index].transform.parent;

        cards[index].transform.SetParent(focusedParent);
        cards[index].transform.localPosition = cards[index].selected ? new Vector3(0, cards[index].selectionOffset, 0) : Vector3.zero;
        selectedCard.transform.SetParent(crossedParent);



        isCrossing = false;

        if (cards[index].cardVisual == null)
            return;

        bool swapIsRight = cards[index].ParentIndex() > selectedCard.ParentIndex();
        cards[index].cardVisual.Swap(swapIsRight ? -1 : 1);

        

        if (otherHolder != null)
            otherHolder.ExternalSwap(selectedIndex, index);


        //Updated Visual Indexes
        foreach (Card card in cards)
        {
            card.cardVisual.UpdateIndex(transform.childCount);
        }
    }


    public void ExternalSwap(int currCardIndex, int index)
    {
        isCrossing = true;

        Card firstCard = cards[currCardIndex];
        Card secondCard = cards[index];

        Transform firstParent = firstCard.transform.parent;
        Transform secondParent = secondCard.transform.parent;

        firstCard.transform.SetParent(secondParent);
        secondCard.transform.SetParent(firstParent);

        firstCard.transform.localPosition = firstCard.selected ? new Vector3(0, firstCard.selectionOffset, 0) : Vector3.zero;
        secondCard.transform.localPosition = secondCard.selected ? new Vector3(0, secondCard.selectionOffset, 0) : Vector3.zero;

        isCrossing = false;

        if (firstCard.cardVisual != null && secondCard.cardVisual != null)
        {
            bool swapIsRight = secondCard.ParentIndex() > firstCard.ParentIndex();
            firstCard.cardVisual.Swap(swapIsRight ? 1 : -1);
            secondCard.cardVisual.Swap(swapIsRight ? -1 : 1);
        }

        foreach (Card card in cards)
        {
            if (card.cardVisual != null)
                card.cardVisual.UpdateIndex(transform.childCount);
        }
    }

}
