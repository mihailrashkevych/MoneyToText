using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
   class Transformer
   {
      private string startText;
      private string finalText = String.Empty;

      public void GetStartText()
      {
         bool repeat = true;
         string pattern = @"^[0-9]{1,35}( [0-9]{3})?( [0-9]{3})?(\.[0-9]{2})?(,[0-9]{3})?(,[0-9]{3})?(\.[0-9]{2})?$";

         while (repeat)
         {
            Console.WriteLine("Enter amount of money in format xx,xxx,xxx.xx or xx xxx xxx.xx or xxxxxx.xx:");

            Match match = Regex.Match(Console.ReadLine(), pattern);

            if (match.Success)
            {
               this.startText = match.Value.Replace(" ", "").Replace(",", "");
            }
            else
            {
               Console.WriteLine("Match not found...");
               continue;
            }

            GetTextFromNumb();

            Console.Write("Go again? Y/N: ");
            string go = Console.ReadLine();
            if (go.ToUpper() != "Y")
            {
               repeat = false;
            }
         }
      }

      public void GetTextFromNumb()
      {
         int ptr = startText.IndexOf('.');

         Queue<string> digits = new Queue<string>();


         for (; ptr > 0; ptr--)
         {
            digits.Enqueue(startText[ptr - 1].ToString());
         }

         Queue<string> ranks = new Queue<string>();

         while (digits.Count > 0)
         {
            string rank = String.Empty;

            for (int i = 0; i < 3; i++)
            {
               string a = String.Empty;
               if (digits.TryDequeue(out a))
               {
                  rank = rank.Insert(0, a);
               }
               else
               {
                  break;
               }
            }
            ranks.Enqueue(rank);
         }

         int itr = 0;
         int hrnptr = startText.IndexOf('.') - 1;
         var hrnUnits = hrnUnitsUntilThousend;
         while (ranks.Count > 0)
         {
            string rank = String.Empty;
            string rankText = String.Empty;

            if (!ranks.TryDequeue(out rank))
            {
               break;
            }

            int length = rank.Length;

            if (itr > 1)
            {
               hrnUnits = hrnUnitsAfterThousend;
            }

            if (length == 3)
            {
               rankText = hundreds[rank[0]];

               if (rank[1] == '1')
               {
                  rankText += teens[rank[1].ToString() + rank[2].ToString()];
                  finalText = finalText.Insert(0, rankText + rankNames[itr]['5']);

                  if (ranks.Count == 0)
                  {
                     finalText += hrnText['5'];
                  }

               }
               else
               {
                  rankText += dozens[rank[1]] + hrnUnits[rank[2]];
                  finalText = finalText.Insert(0, rankText + rankNames[itr][rank[2]]);

                  if (ranks.Count == 0)
                  {
                     finalText += hrnText[startText[hrnptr]];
                  }

               }
            }
            else if (length == 2)
            {
               if (rank[0] == '1')
               {
                  rankText = teens[rank[0].ToString() + rank[1].ToString()];
                  finalText = finalText.Insert(0, rankText + rankNames[itr]['5']);
                  if (ranks.Count == 0)
                  {
                     finalText += hrnText['5'];
                  }
               }
               else
               {
                  rankText = dozens[rank[0]] + hrnUnits[rank[1]];
                  finalText = finalText.Insert(0, rankText + rankNames[itr][rank[1]]);

                  if (ranks.Count == 0)
                  {
                     finalText += hrnText[startText[hrnptr]];
                  }
               }
            }
            else
            {
               if (rank[0] == '0')
               {
                  rankText = "нуль ";
                  finalText = finalText.Insert(0, rankText + rankNames[itr][rank[0]]);
               }
               else
               {
                  rankText = hrnUnits[rank[0]];
                  finalText = finalText.Insert(0, rankText + rankNames[itr][rank[0]]);
               }

               if (ranks.Count == 0)
               {
                  finalText += hrnText[startText[hrnptr]];
               }
            }
            itr++;
         }

         ptr = startText.IndexOf('.');
         string centsText = startText[ptr + 1].ToString() + startText[ptr + 2].ToString();

         if (startText[ptr + 1] == '0')
         {
            finalText += copUnits[startText[ptr + 2]] + cop[startText[ptr + 2]];
         }
         else if (startText[ptr + 1] == '1')
         {
            finalText += teens[startText[ptr + 1].ToString() + startText[ptr + 2].ToString()] + cop['5'];
         }
         else
         {
            finalText += dozens[startText[ptr + 1]] + copUnits[startText[ptr + 2]] + cop[startText[ptr + 2]];
         }

         Console.WriteLine(finalText);
         finalText = string.Empty;
      }
      readonly Dictionary<char, string> hundreds = new Dictionary<char, string>()
      {
         ['0'] = "",
         ['1'] = "сто ",
         ['2'] = "двiстi ",
         ['3'] = "триста ",
         ['4'] = "чотириста ",
         ['5'] = "п'ятсот ",
         ['6'] = "шiстсот ",
         ['7'] = "сiмсот ",
         ['8'] = "вiсiмсот ",
         ['9'] = "дев'ятсот "
      };
      readonly Dictionary<char, string> dozens = new Dictionary<char, string>()
      {
         ['0'] = "",
         ['2'] = "двадцять ",
         ['3'] = "тридцять ",
         ['4'] = "сорок ",
         ['5'] = "п'ятдесят ",
         ['6'] = "шiстдесят ",
         ['7'] = "сiмдесят ",
         ['8'] = "вiсiмдесят ",
         ['9'] = "дев'яносто "
      };
      readonly Dictionary<string, string> teens = new Dictionary<string, string>()
      {
         ["10"] = "десять ",
         ["11"] = "одинадцять ",
         ["12"] = "дванадцять ",
         ["13"] = "тринадцять ",
         ["14"] = "чотирнадцять ",
         ["15"] = "п'ятнадцять ",
         ["16"] = "шiстнадцять ",
         ["17"] = "сiмнадцять ",
         ["18"] = "вiсiмнадцять ",
         ["19"] = "дев'ятнадцять "
      };
      readonly Dictionary<char, string> hrnUnitsAfterThousend = new Dictionary<char, string>()
      {
         ['0'] = "",
         ['1'] = "один ",
         ['2'] = "два ",
         ['3'] = "три ",
         ['4'] = "чотири ",
         ['5'] = "п'ять ",
         ['6'] = "шiсть ",
         ['7'] = "сiм ",
         ['8'] = "вiсiм ",
         ['9'] = "дев'ять "
      };
      readonly Dictionary<char, string> hrnUnitsUntilThousend = new Dictionary<char, string>()
      {
         ['0'] = "",
         ['1'] = "одна ",
         ['2'] = "двi ",
         ['3'] = "три ",
         ['4'] = "чотири ",
         ['5'] = "п'ять ",
         ['6'] = "шiсть ",
         ['7'] = "сiм ",
         ['8'] = "вiсiм ",
         ['9'] = "дев'ять "
      };
      readonly Dictionary<char, string> copUnits = new Dictionary<char, string>()
      {
         ['0'] = "нуль ",
         ['1'] = "одна ",
         ['2'] = "двi ",
         ['3'] = "три ",
         ['4'] = "чотири ",
         ['5'] = "п'ять ",
         ['6'] = "шiсть ",
         ['7'] = "сiм ",
         ['8'] = "вiсiм ",
         ['9'] = "дев'ять "
      };
      readonly Dictionary<char, string> cop = new Dictionary<char, string>()
      {
         ['0'] = "копiйок ",
         ['1'] = "копiйка ",
         ['2'] = "копiйки ",
         ['3'] = "копiйки ",
         ['4'] = "копiйки ",
         ['5'] = "копiйок ",
         ['6'] = "копiйок ",
         ['7'] = "копiйок ",
         ['8'] = "копiйок ",
         ['9'] = "копiйок "
      };
      readonly Dictionary<char, string> hrnText = new Dictionary<char, string>()
      {
         ['0'] = "гривень ",
         ['1'] = "гривня ",
         ['2'] = "гривнi ",
         ['3'] = "гривнi ",
         ['4'] = "гривнi ",
         ['5'] = "гривень ",
         ['6'] = "гривень ",
         ['7'] = "гривень ",
         ['8'] = "гривень ",
         ['9'] = "гривень "
      };
      readonly Dictionary<int, Dictionary<char, string>> rankNames = new Dictionary<int, Dictionary<char, string>>()
      {
         [0] = new Dictionary<char, string>()
         {
            ['0'] = "",
            ['1'] = "",
            ['2'] = "",
            ['3'] = "",
            ['4'] = "",
            ['5'] = "",
            ['6'] = "",
            ['7'] = "",
            ['8'] = "",
            ['9'] = ""
         },

         [1] = new Dictionary<char, string>()
         {
            ['0'] = "",
            ['1'] = "тисяча ",
            ['2'] = "тисячi ",
            ['3'] = "тисячi ",
            ['4'] = "тисячi ",
            ['5'] = "тисяч ",
            ['6'] = "тисяч ",
            ['7'] = "тисяч ",
            ['8'] = "тисяч ",
            ['9'] = "тисяч "
         },

         [2] = new Dictionary<char, string>()
         {
            ['0'] = "",
            ['1'] = "мiльйон ",
            ['2'] = "мiльйони ",
            ['3'] = "мiльйони ",
            ['4'] = "мiльйони ",
            ['5'] = "мiльйонiв ",
            ['6'] = "мiльйонiв ",
            ['7'] = "мiльйонiв ",
            ['8'] = "мiльйонiв ",
            ['9'] = "мiльйонiв "
         },

         [3] = new Dictionary<char, string>()
         {
            ['0'] = "",
            ['1'] = "мiльярд ",
            ['2'] = "мiльярди ",
            ['3'] = "мiльярди ",
            ['4'] = "мiльярди ",
            ['5'] = "мiльярдiв ",
            ['6'] = "мiльярдiв ",
            ['7'] = "мiльярдiв ",
            ['8'] = "мiльярдiв ",
            ['9'] = "мiльярдiв "
         },

         [4] = new Dictionary<char, string>()
         {
            ['0'] = "",
            ['1'] = "трильйон ",
            ['2'] = "трильйони ",
            ['3'] = "трильйони ",
            ['4'] = "трильйони ",
            ['5'] = "трильйонiв ",
            ['6'] = "трильйонiв ",
            ['7'] = "трильйонiв ",
            ['8'] = "трильйонiв ",
            ['9'] = "трильйонiв "
         },
      };
   }

}