using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fuzzy
{
    /// <summary>
    /// Переменная. Содержит список термов.
    /// </summary>
    public class Variable
    {
        /// <summary>
        /// Имя переменной
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Минимальное значение (только для выходных пермеменных)
        /// </summary>
        public double Min { get; set; }

        /// <summary>
        /// Максимальное значение (только для выходных пермеменных)
        /// </summary>
        public double Max { get; set; }

        /// <summary>
        /// Термы (пары имя_терма - терм)
        /// </summary>
        public Dictionary<string, Term> Terms { get; } = new Dictionary<string, Term>();
    }
}
