using System;
using System.Collections.Generic;
using System.Linq;

namespace Fuzzy
{
    /// <summary>
    /// Алгортим Сугено
    /// </summary>
    public class Sugeno
    {

        public List<Variable> InputVariables { get; } = new List<Variable>();
        public List<Variable> OutputVariables { get; } = new List<Variable>();
        public List<Rule_sugeno> Rules { get; } = new List<Rule_sugeno>();

        public IEnumerable<double> Calculate(params double[] inpVarValues)
        {
            {
                var varVals = new Dictionary<string, double>();
                for (int i = 0; i < inpVarValues.Length; i++)
                    varVals[InputVariables[i].Name] = inpVarValues[i];

                foreach (var outputVar in OutputVariables)
                {
                    var sum = 0d;
                    var num = 0;
                    foreach (var outTerm in outputVar.Terms.Values)
                    {
                        foreach (var rule in Rules.Where(r => r.Conclusion == outTerm))
                        {
                            rule.Weight = rule.Conditions.Min(c => c.MembershipFunction(varVals[c.Variable.Name]));


                            rule.Output = inpVarValues[0] * rule.W1 + inpVarValues[1] * rule.W2;
                        }
                    }

                    yield return Rules.Sum(r => r.Weight * r.Output)/(Rules.Sum(r => r.Weight)*10);
                }
            }
        }
    }
}