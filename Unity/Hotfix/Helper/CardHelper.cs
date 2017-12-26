using System.Collections.Generic;
using Model;

namespace Hotfix
{
    public static class CardHelper
    {
        public static void Sort(List<Card> cards)
        {
            for (int i = cards.Count; i > 0; i--)
            {
                for (int j = 0; j < i - 1; j++)
                {
                    //先按照权重降序，再按花色升序
                    if (-CompareTo((int)cards[j].CardWeight, (int)cards[j + 1].CardWeight) * 2 +
                        CompareTo((int)cards[j].CardSuits, (int)cards[j + 1].CardSuits) > 0)
                    {
                        Card temp = cards[j];
                        cards[j] = cards[j + 1];
                        cards[j + 1] = temp;
                    }
                }
            }
            //cards.Sort((a, b) =>
            //{
            //    //先按照权重降序，再按花色升序
            //    return -a.CardWeight.CompareTo(b.CardWeight) * 2 +
            //        a.CardSuits.CompareTo(b.CardSuits);
            //});
        }

        public static int CompareTo(int a, int b)
        {
            int result;
            if (a > b)
            {
                result = 1;
            }
            else if (a < b)
            {
                result = -1;
            }
            else
            {
                result = 0;
            }
            return result;
        }
    }
}
