using System;
using System.Collections.Generic;
using System.Linq;

namespace Fuzzy
{
    /// <summary>
    /// Алгортим Мамдани
    /// </summary>
    public class Mamdani
    {
        public List<Variable> InputVariables { get; } = new List<Variable>();
        public List<Variable> OutputVariables { get; } = new List<Variable>();
        public List<Rule> Rules { get; } = new List<Rule>();

        /// <summary>
        /// Делает расчет модели, принимает значения входных переменных, возвращает значения выходных переменных
        /// </summary>
        public IEnumerable<double> Calculate(params double[] inpVarValues)
        {
            var varVals = new Dictionary<string, double>();
            for (int i = 0; i < inpVarValues.Length; i++)
                varVals[InputVariables[i].Name] = inpVarValues[i];

            //для каждой выходной перменной...
            foreach (var outputVar in OutputVariables)
            {
                var union = new Union();

                //для каждого выходного терма
                foreach (var outTerm in outputVar.Terms.Values)
                {
                    //перебираем правила для этого выходного терма
                    foreach (var rule in Rules.Where(r => r.Conclusion == outTerm))
                    {
                        //ищем худшее значение из условий (Фаззификация и Агрегирование)
                        var min = rule.Conditions.Min(c => c.MembershipFunction(varVals[c.Variable.Name]));
                        //добавляем в объединение термов активизированный терм
                        union.Add(
                            new Term()
                            {
                                Name = outTerm.Name,
                                Variable = outTerm.Variable,
                                MembershipFunction = (x) => Math.Min(outTerm.MembershipFunction(x),
                                min * rule.Weight)
                            }
                            );
                    }
                }
                yield return union.CenterMass(outputVar.Min, outputVar.Max);
            }
        }
    }
}