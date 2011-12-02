using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICMCalculator
{
    public class ICMCalculator
    {
        // public int[] Structure { get; set; }
        // public int[] Chips { get; set; }

        public double[] CalcEV(int[] structure, int[] chips)
        {
            // j番目のプレイヤーがi位になる確率
            double[,] probabilitys = new double[structure.Length, chips.Length];
            // j番目のプレイヤーの期待値
            double[] EVs = new double[chips.Length];
            int[] players = new int[chips.Length];
            for(int i = 0; i < players.Length; ++i)
                players[i] = i;
            IEnumerable<int[]> permutations;

            for (int i = 0; i < structure.Length; ++i)
            {
                permutations = (new Permutation()).Enumerate(players, i + 1);

                foreach (int[] permutation in permutations)
                {
                    probabilitys[i, permutation[i]] += CalcPermutationProbability(permutation, chips);
                }
            }

            for (int i = 0; i < structure.Length; ++i)
            {
                for (int j = 0; j < chips.Length; ++j)
                    EVs[j] += probabilitys[i, j] * structure[i];
            }

            return EVs;
        }

        private double CalcPermutationProbability(int[] permutations, int[] chips)
        {
            double probability = 1.0F;
            int chips_sum = chips.Sum();

            for (int i = 0; i < permutations.Length; ++i)
            {
                probability *= System.Convert.ToDouble(chips[permutations[i]]) / System.Convert.ToDouble(chips_sum);
                chips_sum -= chips[permutations[i]];
            }

            return probability;
        }
    }
}
