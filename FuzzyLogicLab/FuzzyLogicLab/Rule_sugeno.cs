using System.Collections.Generic;
using System.Linq;

namespace Fuzzy
{
    /// <summary>
    /// Правило
    /// </summary>
    public class Rule_sugeno : Rule
    {
        /// <summary>
        /// Коэффициент 1
        /// </summary>
        public double W1 { get; set; }

        /// <summary>
        /// Коэффициент 2
        /// </summary>
        public double W2 { get; set; }

        /// <summary>
        /// Выход правила
        /// </summary>
        public double Output { get; set; }
    }
}