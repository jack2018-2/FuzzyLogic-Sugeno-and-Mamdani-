using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Fuzzy
{
    public static class Parser
    {
        /// <summary>
        /// Для Сугено
        /// </summary>
        /// <param name="calculator"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static double[] Parse(Sugeno calculator, StreamReader reader)
        {
            double[] values = new double[0];
            var input = reader.ReadToEnd();
            foreach (Match block in Regex.Matches(input, @"\[(?<name>.+?)\](?<body>[^\[]+)"))
            {
                var name = block.Groups["name"].Value;
                var body = block.Groups["body"].Value.Trim();

                switch (name)
                {
                    case "Variables and terms":
                        ParseVar(calculator, body, ref reader);
                        break;
                    case "Values":
                        values = ParseVarValues(calculator, body);
                        break;
                    case "Rules":
                        ParseRules(calculator, body);
                        break;
                    default:
                        throw new Exception("Неверный формат входных данных");
                }
            }
            return values;
        }

        /// <summary>
        /// Для сугено
        /// </summary>
        /// <param name="calculator"></param>
        /// <param name="text"></param>
        /// <param name="reader"></param>
        private static void ParseVar(Sugeno calculator, string text, ref StreamReader reader)
        {
            foreach (var line in text.Split('\n'))
            {
                var variable = new Variable() { Name = line.Split(' ')[0] };
                var isOutVar = false;

                foreach (Match block in Regex.Matches(line, @"(?<name>\S+):(?<body>.+?)(?=\w+:|$)"))
                {
                    var name = block.Groups["name"].Value;
                    var body = block.Groups["body"].Value.Replace('\t', ' ');
                    var parts = body.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    //парсим терм
                    switch (name)
                    {
                        case "MAX": variable.Max = float.Parse(parts[0], CultureInfo.InvariantCulture); isOutVar = true; break;
                        case "MIN": variable.Min = float.Parse(parts[0], CultureInfo.InvariantCulture); isOutVar = true; break;
                        default:
                            var c = double.Parse(parts[0], CultureInfo.InvariantCulture);
                            var b = double.Parse(parts[1], CultureInfo.InvariantCulture);
                            var term = new Term() { Name = name, Variable = variable, MembershipFunction = MathHelper.Gauss(c, b) };
                            variable.Terms.Add(name, term);
                            break;
                    }
                }
                if (isOutVar)
                    calculator.OutputVariables.Add(variable);
                else
                    calculator.InputVariables.Add(variable);
            }
        }

        /// <summary>
        /// Для Сугено
        /// </summary>
        /// <param name="calculator"></param>
        /// <param name="text"></param>
        private static void ParseRules(Sugeno calculator, string text)
        {
            foreach (var line in text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var rule = new Rule_sugeno();

                foreach (Match block in Regex.Matches(line.Trim(), @"(?<name>\S+):(?<body>.+?)(?=\w+:|$)"))
                {
                    var name = block.Groups["name"].Value;
                    var body = block.Groups["body"].Value;
                    var parts = body.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var variable = calculator.InputVariables.FirstOrDefault(v => v.Name == name);
                    if (variable != null)
                        rule.Conditions.Add(variable.Terms[parts[0]]);
                    //
                    variable = calculator.OutputVariables.FirstOrDefault(v => v.Name == name);
                    if (variable != null)
                    {
                        rule.Conclusion = variable.Terms[parts[0]];
                        rule.W1 = double.Parse(parts[1], CultureInfo.InvariantCulture);
                        rule.W2 = double.Parse(parts[2], CultureInfo.InvariantCulture);
                    }
                }

                calculator.Rules.Add(rule);
            }
        }

        /// <summary>
        /// Для Сугено
        /// </summary>
        /// <param name="calculator"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static double[] ParseVarValues(Sugeno calculator, string text)
        {
            var res = new double[calculator.InputVariables.Count];

            foreach (Match block in Regex.Matches(text, @"(?<name>\S+):(?<body>.+?)(?=\w+:|$)"))
            {
                var name = block.Groups["name"].Value;
                var body = block.Groups["body"].Value;
                var i = calculator.InputVariables.FindIndex(v => v.Name == name);
                res[i] = double.Parse(body, CultureInfo.InvariantCulture);
            }

            return res;
        }


        /// <summary>
        /// Для Мамдани
        /// </summary>
        /// <param name="calculator"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static double[] Parse(Mamdani calculator, StreamReader reader)
        {
            double[] values = new double[0];
            var input = reader.ReadToEnd();
            foreach (Match block in Regex.Matches(input, @"\[(?<name>.+?)\](?<body>[^\[]+)"))
            {
                var name = block.Groups["name"].Value;
                var body = block.Groups["body"].Value.Trim();

                switch (name)
                {
                    case "Variables and terms":
                        ParseVar(calculator, body, ref reader);
                        break;
                    case "Rules":
                        ParseRules(calculator, body);
                        break;
                    case "Values":
                        values = ParseVarValues(calculator, body);
                        break;
                    default:
                        throw new Exception("Неверный формат входных данных");
                }
            }
            return values;
        }

        /// <summary>
        /// Для мамдани
        /// </summary>
        /// <param name="calculator"></param>
        /// <param name="text"></param>
        /// <param name="reader"></param>
        private static void ParseVar(Mamdani calculator, string text, ref StreamReader reader)
        {
            foreach (var line in text.Split('\n'))
            {
                var variable = new Variable() { Name = line.Split(' ')[0] };
                var isOutVar = false;

                foreach (Match block in Regex.Matches(line, @"(?<name>\S+):(?<body>.+?)(?=\w+:|$)"))
                {
                    var name = block.Groups["name"].Value;
                    var body = block.Groups["body"].Value.Replace('\t', ' ');
                    var parts = body.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    //парсим терм
                    switch (name)
                    {
                        case "MAX": variable.Max = float.Parse(parts[0], CultureInfo.InvariantCulture); isOutVar = true; break;
                        case "MIN": variable.Min = float.Parse(parts[0], CultureInfo.InvariantCulture); isOutVar = true; break;
                        default:
                            var c = double.Parse(parts[0], CultureInfo.InvariantCulture);
                            var b = double.Parse(parts[1], CultureInfo.InvariantCulture);
                            var term = new Term() { Name = name, Variable = variable, MembershipFunction = MathHelper.Gauss(c, b) };
                            variable.Terms.Add(name, term);
                            break;
                    }
                }
                if (isOutVar)
                    calculator.OutputVariables.Add(variable);
                else
                    calculator.InputVariables.Add(variable);
            }
        }

        /// <summary>
        /// Для мамдани
        /// </summary>
        /// <param name="calculator"></param>
        /// <param name="text"></param>
        private static void ParseRules(Mamdani calculator, string text)
        {
            foreach (var line in text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var rule = new Rule();

                foreach (Match block in Regex.Matches(line.Trim(), @"(?<name>\S+):(?<body>.+?)(?=\w+:|$)"))
                {
                    var name = block.Groups["name"].Value;
                    var body = block.Groups["body"].Value;
                    var parts = body.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var variable = calculator.InputVariables.FirstOrDefault(v => v.Name == name);
                    if (variable != null)
                        rule.Conditions.Add(variable.Terms[parts[0]]);
                    variable = calculator.OutputVariables.FirstOrDefault(v => v.Name == name);
                    if (variable != null)
                    {
                        rule.Conclusion = variable.Terms[parts[0]];
                        rule.Weight = double.Parse(parts[1], CultureInfo.InvariantCulture);
                    }
                }

                calculator.Rules.Add(rule);
            }
        }

        /// <summary>
        /// Для Мамдани
        /// </summary>
        /// <param name="calculator"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static double[] ParseVarValues(Mamdani calculator, string text)
        {
            var res = new double[calculator.InputVariables.Count];

            foreach (Match block in Regex.Matches(text, @"(?<name>\S+):(?<body>.+?)(?=\w+:|$)"))
            {
                var name = block.Groups["name"].Value;
                var body = block.Groups["body"].Value;
                var i = calculator.InputVariables.FindIndex(v => v.Name == name);
                res[i] = double.Parse(body, CultureInfo.InvariantCulture);
            }

            return res;
        }
    }
}