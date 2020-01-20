using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcAllPossibleTimes
{
    /// <summary>
    /// Calculates all possible times that can be created using given digits.
    /// Restrictoins are logical, for example first digit can be only 0, 1 or 2
    /// Second digit cannot be more than 3 when first digit is 2.
    /// Third digit cannot be more than 5.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            List<string> result = GetPossibleTimesList(1, 8, 3, 2).ToList();
            //List<string> result = GetPossibleTimesList(2, 3, 3, 2).ToList();
            //List<string> result = GetPossibleTimesList(6, 2, 4, 7).ToList();
            //List<string> result = GetPossibleTimesList(0, 0, 0, 0).ToList();
            //List<string> result = GetPossibleTimesList(2, 3, 5, 9).ToList();
            result.ForEach(x => { Console.WriteLine(x); });
            Console.ReadLine();
        }


        /// <summary>
        /// Calculates all possible times that can be gathered from given digits.
        /// Every digit can be used only once.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="D"></param>
        /// <returns></returns>
        public static IEnumerable<String> GetPossibleTimesList(int A, int B, int C, int D)
        {
            List<String> possibleTimes = new List<String>();
            List<int> givenDigitsList = new List<int> { A, B, C, D };
            if(givenDigitsList.FindAll(x => x < 0 || x > 9).Count > 0)
            {
                // Log or throw some exception.
                Console.WriteLine("Given digits should be within [0,9] range.");
                return possibleTimes;
            }

            Dictionary<int, int> dict = GetAvailableDigitsDict(givenDigitsList);
            Dictionary<int, int> dictClone = new Dictionary<int, int>();
            foreach(var item in dict)
            {
                dictClone.Add(item.Key, item.Value);
            }

            StringBuilder time;
            foreach(var hour1 in dictClone.Keys)
            {
                time = new StringBuilder("");
                dict = GetAvailableDigitsDict(givenDigitsList);

                if (hour1 > 2)
                    continue;

                if (!UpdateAvailableDigits(dict, hour1))
                    continue;

                time.Append(hour1);
                foreach (var hour2 in dictClone.Keys)
                {
                    if (hour1 < 2 || hour1 == 2 && hour2 < 4)
                    {
                        if (!UpdateAvailableDigits(dict, hour2))
                            continue;

                        time.Append(hour2);
                        foreach (var minute1 in dictClone.Keys)
                        {
                            if (minute1 > 5)
                                continue;

                            if (!UpdateAvailableDigits(dict, minute1))
                                continue;

                            time.Append(":");
                            time.Append(minute1);
                            foreach (var minute2 in dictClone.Keys)
                            {
                                if (!UpdateAvailableDigits(dict, minute2))
                                    continue;

                                time.Append(minute2);
                                possibleTimes.Add(time.ToString());
                                dict[minute2]++;
                                time.Remove(time.Length - 1, 1);
                            }

                            dict[minute1]++;
                            time.Remove(time.Length - 1, 1);
                            time.Remove(time.Length - 1, 1); // two dots
                        }

                        dict[hour2]++;
                        time.Remove(time.Length - 1, 1);
                    }
                }

                dict[hour1]++;
                time.Remove(time.Length - 1, 1);
            }
           
            return possibleTimes;
        }

        /// <summary>
        ///  Creates a dictionary of available digits in a list.
        ///  For example if there is two 3 digits in a list, in dictionary will be (3, 2) pair.
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        static Dictionary<int, int> GetAvailableDigitsDict(List<int> arr)
        {
            Dictionary<int, int> availableDigits = new Dictionary<int, int>();
            foreach (int item in arr)
            {
                if (availableDigits.ContainsKey(item))
                {
                    availableDigits[item]++;
                }
                else
                {
                    availableDigits.Add(item, 1);
                }
            }

            return availableDigits;
        }

        static bool UpdateAvailableDigits(Dictionary<int, int> availableDigits, int digit)
        {
            // If map contains the digit 
            if (availableDigits.ContainsKey(digit) && availableDigits[digit] > 0) // mi hat esi stugel
            {
                // Decrement the availability of the digit by 1 
                availableDigits[digit]--;

                // True here indicates that the digit was found in the map 
                return true;
            }

            return false; // Digit not found 
        }

    }
}
