using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QuickQuotes
{
    /// <summary>
    /// Controller for the app
    /// </summary>
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// Model for the collection of quotes
        /// </summary>
        public QuoteCollection quoteCollection { get; private set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            quoteCollection = new QuoteCollection();
            InitialiseUI();
            LoadData();
        }

        /// <summary>
        /// Initalises the UI to a "loading" state
        /// </summary>
        private void InitialiseUI()
        {
            quoteHeadingLabel.IsVisible = false;
            quoteAuthorHeadingLabel.IsVisible = false;
            quoteTextLabel.Text = "Loading ...";
            quoteAuthorLabel.Text = "please wait...";
            randomQuoteButton.IsEnabled = false;
            addQuoteButton.IsEnabled = false;
            quoteAddedLabel.IsVisible = false;
        }

        /// <summary>
        /// Loads saved data, and updates the UI when done
        /// </summary>
        private async void LoadData()
        {
            await quoteCollection.LoadDataFromFile();
            UpdateUI();
        }

        /// <summary>
        /// Boolean to prevent race conditions between UpdateUI being called, and it triggering an event which results in another call to UpdateUI
        /// </summary>
        private bool uiIsUpdating = false;

        /// <summary>
        /// Updates all UI elements based on the current state of the quoteCollection model
        /// </summary>
        private void UpdateUI()
        {
            uiIsUpdating = true;
            if (quoteCollection.quotes.Count == 0)
            {
                // When empty, display a notice instead of the current quote
                quoteHeadingLabel.IsVisible = false;
                quoteTextLabel.Text = "No quotes in your collection yet.";
                quoteTextLabel.IsVisible = true;
                quoteAuthorHeadingLabel.IsVisible = false;
                quoteAuthorLabel.IsVisible = false;
                randomQuoteButton.IsEnabled = false;
            }
            else
            {
                quoteHeadingLabel.Text = String.Format("Quote #{0} of {1}", quoteCollection.currentQuoteIndex + 1, quoteCollection.quotes.Count);
                quoteHeadingLabel.IsVisible = true;
                quoteTextLabel.Text = quoteCollection.currentQuote.quotation;
                quoteTextLabel.IsVisible = true;
                quoteAuthorHeadingLabel.IsVisible = true;
                quoteAuthorLabel.Text = quoteCollection.currentQuote.author;
                quoteAuthorLabel.IsVisible = true;
                randomQuoteButton.IsEnabled = true;
            }

            quoteTextEditor.Text = quoteCollection.quoteBeingEdited.quotation;
            quoteAuthorEntry.Text = quoteCollection.quoteBeingEdited.author;
            addQuoteButton.IsEnabled = quoteCollection.quoteBeingEdited.IsValid();
            uiIsUpdating = false;
        }

        /// <summary>
        /// Handles "Random Quote" button clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RandomQuoteButton_Clicked(object sender, EventArgs e)
        {
            quoteCollection.SelectRandomQuote();
            UpdateUI();
        }

        /// <summary>
        /// Handles "Add Quote" button clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddQuoteButton_Clicked(object sender, EventArgs e)
        {
            // Call the async version of this method (Xamarin doesn't allow 
            // async methods to be directly attached as event handlers)
            AddQuoteButtonClickedAsync();
        }

        /// <summary>
        /// Asynchronasly handles  "Add Quote" button clicks
        /// </summary>
        private async void AddQuoteButtonClickedAsync()
        {
            // Add the quote to the collection, show feedback, disbale button to prevent double addition
            quoteCollection.AddQuote(quoteTextEditor.Text, quoteAuthorEntry.Text);
            quoteAddedLabel.IsVisible = true;
            addQuoteButton.IsEnabled = false;
            // After a short delay, empty the quote/author text in the model (and update the UI) 
            await Task.Delay(500);
            quoteCollection.quoteBeingEdited.Empty();
            UpdateUI();
            // After a slightly longer delay, the feedback message can be hidden again.
            await Task.Delay(1000);
            quoteAddedLabel.IsVisible = false;
        }

        /// <summary>
        /// Handles text change events for the quoteTextEditor and quoteAuthorEntry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuoteBeingEdited_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!uiIsUpdating)
            {
                // Update the values stored in the model
                quoteCollection.quoteBeingEdited.quotation = string.IsNullOrWhiteSpace(quoteTextEditor.Text) ? "" : quoteTextEditor.Text;
                quoteCollection.quoteBeingEdited.author = string.IsNullOrWhiteSpace(quoteAuthorEntry.Text) ? "" : quoteAuthorEntry.Text;
                // Enable or disabled the button based on the validity of the current values
                //addQuoteButton.IsEnabled = quoteCollection.quoteBeingEdited.IsValid();
                UpdateUI();
            }
        }
    }
}
