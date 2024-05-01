using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Choose a program to run:");
                Console.WriteLine("1. Program1");
                Console.WriteLine("2. Program2");
                Console.WriteLine("3. Program3");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice (1, 2, 3, or 0): ");

                string? choice = Console.ReadLine();

                if (choice == "1")
                {
                    RunProgram1();
                }
                else if (choice == "2")
                {
                    RunProgram2();
                }
                else if (choice == "3")
                {
                    RunProgram3();
                }
                else if (choice == "0")
                {
                    Console.WriteLine("Exiting program.");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter either 1, 2, 3, or 0.");
                }
            }
        }

        static void RunProgram1()
        {
            Console.WriteLine("Enter a string to permute (Examples: 'ab'):");
            string? input = Console.ReadLine();
            while (input == null)
            {
                Console.WriteLine("Input cannot be null. Please enter a valid string:");
                input = Console.ReadLine();
            }
            if (input.StartsWith("'") && input.EndsWith("'") || input.StartsWith("\"") && input.EndsWith("\""))
            {
                input = input.Substring(1, input.Length - 2);
                List<string> permutations = GeneratePermutations(input);
                string jsonPermutations = JsonSerializer.Serialize(permutations);

                jsonPermutations = jsonPermutations.Replace('"', '\'');
                Console.WriteLine(jsonPermutations);
            }
            else
            {
                Console.WriteLine("Please enter a valid string Examples: 'ab':");
            }

        }

        static List<string> GeneratePermutations(string input)
        {
            var result = new HashSet<string>();
            Permute(input.ToCharArray(), 0, input.Length - 1, result);
            return result.ToList();
        }

        static void Permute(char[] array, int start, int end, HashSet<string> result)
        {
            if (start == end)
            {
                result.Add(new string(array));
            }
            else
            {
                for (int i = start; i <= end; i++)
                {
                    Swap(array, start, i);
                    Permute(array, start + 1, end, result);
                    Swap(array, start, i);
                }
            }
        }

        static void Swap(char[] array, int i, int j)
        {
            char temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }

        static void RunProgram2()
        {
            Console.WriteLine("Enter an array of integers (Examples: [1,2,2,3,3,3,4,3,3,3,2,2,1]):");
            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Input cannot be null or empty. Please try again.");
                return;
            }

            object[]? array = ParseMixedInputArray(input);

            if (array == null)
            {
                Console.WriteLine("Failed to parse input. Please try again.");
                return;
            }

            int oddInt = FindOddInteger(array);

            Console.WriteLine($"The integer that appears an odd number of times is: {oddInt}");
        }

        static object[] ParseMixedInputArray(string input)
        {
            input = input.Replace('\'', '"');
            if (input.StartsWith("[") && input.EndsWith("]"))
            {
                try
                {
                    JsonElement[]? elements = JsonSerializer.Deserialize<JsonElement[]>(input);

                    if (elements == null)
                    {
                        throw new InvalidOperationException("Invalid input format: unable to parse array.");
                    }

                    object[] mixedArray = elements
                        .Select(element =>
                        {
                            if (element.ValueKind == JsonValueKind.Number)
                            {
                                return (object)element.GetInt32();
                            }
                            else if (element.ValueKind == JsonValueKind.String)
                            {
                                return (object)element.ToString();
                            }
                            else
                            {
                                throw new InvalidOperationException("Invalid input format.");
                            }
                        })
                        .ToArray();

                    return mixedArray;
                }
                catch (JsonException ex)
                {
                    throw new InvalidOperationException($"Invalid input format: {ex.Message}");
                }
            }
            else
            {
                throw new InvalidOperationException("Input must be in the format of a JSON array.");
            }
        }

        static int FindOddInteger(object[] array)
        {
            Dictionary<int, int> counts = new Dictionary<int, int>();

            foreach (var obj in array)
            {
                if (obj is int num)
                {
                    if (counts.ContainsKey(num))
                    {
                        counts[num]++;
                    }
                    else
                    {
                        counts[num] = 1;
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Found non-integer object: {obj.GetType().Name}");
                }
            }

            foreach (var pair in counts)
            {
                if (pair.Value % 2 != 0)
                {
                    return pair.Key;
                }
            }

            throw new InvalidOperationException("No integer found that appears an odd number of times.");
        }

        static void RunProgram3()
        {
            Console.WriteLine("Enter an array of smiley faces (Examples: [':)', ';(', ';}', ':-D']):");
            string? input = Console.ReadLine();

            if (!string.IsNullOrEmpty(input))
            {
                object[]? smileyArray = ParseMixedInputArray(input);

                int count = countSmileys(smileyArray);

                Console.WriteLine($"Total number of smiling faces: {count}");
            }
            else
            {
                Console.WriteLine("Total number of smiling faces: 0");
            }
        }



        static int countSmileys(object[] arr)
        {
            string[] validEyes = { ":", ";" };
            string[] validNoses = { "", "-", "~" };
            string[] validMouths = { ")", "D" };

            int count = 0;

            foreach (string smiley in arr)
            {
                if (smiley.Length == 2 || smiley.Length == 3)
                {
                    if (validEyes.Contains(smiley[0].ToString()) &&
                        validMouths.Contains(smiley[smiley.Length - 1].ToString()) &&
                        (smiley.Length == 2 || validNoses.Contains(smiley[1].ToString())))
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
