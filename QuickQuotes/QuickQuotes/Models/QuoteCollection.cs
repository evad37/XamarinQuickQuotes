using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using PCLStorage;
using System.Threading.Tasks;

namespace QuickQuotes
{
    /// <summary>
    /// Model for the aingle-page quotation app. 
    /// </summary>
    public class QuoteCollection
    {
        // Private attributes
        /// <summary>
        /// Random instance for random number geberation
        /// </summary>
        private Random rand = new Random();
        /// <summary>
        /// Folder where quotes will be saved
        /// </summary>
        private string dataFolderName = "QuickQuotes";
        /// <summary>
        /// Filename where quotes will be saved
        /// </summary>
        private string dataFileName = "quotations.json";
        /// <summary>
        /// List of the quotes in the collection; initially empty
        /// </summary>

        // Public attributes

        /// <summary>
        /// Collection of quotes
        /// </summary>
        public List<Quote> quotes { get; private set; } = new List<Quote>();
        /// <summary>
        /// Indicates if loading from file has been completed
        /// </summary>
        public bool isLoaded { get; private set; } = false;
        /// <summary>
        /// Index number of the currently-displayed quotes
        /// </summary>
        public int currentQuoteIndex { get; private set; } = 0;
        /// <summary>
        /// Currently-displayed quote
        /// </summary>
        public Quote currentQuote
        {
            get
            {
                if (quotes.Count == 0)
                {
                    // Create a placeholder quote
                    return new Quote("< no quotes in your collection >", "");
                }
                return quotes[currentQuoteIndex];
            }
        }
        /// <summary>
        /// Quote that is being edited by the user (prior to being added)
        /// </summary>
        public Quote quoteBeingEdited { get; private set; } = new Quote();

        /// <summary>
        /// Adds a quote to the collection
        /// </summary>
        /// <param name="quotation">Text of quote</param>
        /// <param name="author">Author of quote</param>
        public void AddQuote(string quotation, string author)
        {
            quotes.Add(new Quote(quotation, author));
        }
        /// <summary>
        /// Randomly selects a new quote from the collection (updates currentQuote and currentQuoteIndex attributes)
        /// </summary>
        public void SelectRandomQuote()
        {
            int newQuoteIndex;
            // Loop through random numbers until the new index is diffefent to the old one
            // (unless there are less than 2 quotes in the collection)
            do
            {
                newQuoteIndex = rand.Next(0, quotes.Count);
            } while (newQuoteIndex == currentQuoteIndex && quotes.Count > 1);
            currentQuoteIndex = newQuoteIndex;
        }

        /// <summary>
        /// Loads quotes from file
        /// </summary>
        /// <returns></returns>
        public async Task LoadDataFromFile()
        {
            IFolder folder = FileSystem.Current.LocalStorage;
            folder = await folder.CreateFolderAsync(dataFolderName, CreationCollisionOption.OpenIfExists);
            IFile file = await folder.CreateFileAsync(dataFileName, CreationCollisionOption.OpenIfExists);
            string serializedData = await file.ReadAllTextAsync();
            Quote[] savedQuotes = JsonConvert.DeserializeObject<Quote[]>(serializedData);
            if (savedQuotes != null)
            {
                quotes = savedQuotes.ToList();
                SelectRandomQuote();
            }
            isLoaded = true;
        }

        /// <summary>
        /// Saves quotes to file
        /// </summary>
        public async void SaveDataToFile()
        {

            IFolder folder = FileSystem.Current.LocalStorage;
            folder = await folder.CreateFolderAsync(dataFolderName, CreationCollisionOption.OpenIfExists);
            IFile file = await folder.CreateFileAsync(dataFileName, CreationCollisionOption.OpenIfExists);
            await file.WriteAllTextAsync(JsonConvert.SerializeObject(quotes.ToArray()));
        }
    }
}
