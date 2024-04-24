namespace QE.Context
{
    public partial class SColor
    {
        /// <summary>
        /// Ключ
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Префикс
        /// </summary>
        public string ColorValue { get; set; } = null!;
        /// <summary>
        /// Префикс
        /// </summary>
        public string ColorText { get; set; } = null!;
    }
}
