using System;

namespace Fuzzy
{
    /// <summary>
    /// Терм с функцией принадлежности
    /// </summary>
    public class Term
    {
        /// <summary>
        /// Переменная, к которой относится терм
        /// </summary>
        public Variable Variable { get; set; }

        /// <summary>
        /// Имя терма
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Функция принадлежности
        /// </summary>
        public Func<double, double> MembershipFunction { get; set; }
    }
}