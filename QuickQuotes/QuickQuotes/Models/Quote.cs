namespace QuickQuotes
{
    /// <summary>
    /// Model for a quote
    /// </summary>
    public class Quote
    {
        /// <summary>
        /// Text of the quote
        /// </summary>
        public string quotation { get; set; } = "";
        /// <summary>
        /// Quote author
        /// </summary>
        public string author { get; set; } = "";

        /// <summary>
        /// Constructor for an empty quote
        /// </summary>
        public Quote() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="quotation">Text of quote</param>
        /// <param name="author">Author's name</param>
        public Quote(string quotation, string author)
        {
            this.quotation = quotation.Trim();
            this.author = author.Trim();
        }

        /// <summary>
        /// Checks if a quote is valid (both quotation and author are not empty)
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return quotation.Trim() != "" && author.Trim() != "";
        }

        /// <summary>
        /// Sets or resets the quote to an empty state
        /// </summary>
        public void Empty()
        {
            this.quotation = "";
            this.author = "";
        }
    }
}
