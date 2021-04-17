using System.Collections.Generic;

namespace Fuzzy
{
    /// <summary>
    /// Правило
    /// </summary>
    public class Rule
    {
        /// <summary>
        /// Условия по AND.
        /// </summary>
        public List<Term> Conditions { get; } = new List<Term>();

        /// <summary>
        /// Заключение.
        /// </summary>
        public Term Conclusion { get; set; }

        /// <summary>
        /// Вес правила
        /// </summary>
        public double Weight { get; set; } = 1;
    }
}