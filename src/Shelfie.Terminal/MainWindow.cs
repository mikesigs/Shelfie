using Shelfie.Core.BoardGameGeek;
using Shelfie.Core.Data;
using Terminal.Gui;

public class MainWindow : Window
{
    private readonly IShelfieRepository _shelfieRepository;
    private readonly IBggApiClient _bggApiClient;

    public MainWindow(IShelfieRepository shelfieRepository, IBggApiClient bggApiClient)
    {
        _shelfieRepository = shelfieRepository;
        _bggApiClient = bggApiClient;

        Title = "Shelfie (Ctrl+Q to quit)";

        // Create input components and labels
        var gameNameLabel = new Label()
        {
            Text = "Board Game:"
        };

        var gameNameText = new TextField("")
        {
            // Position text field adjacent to the label
            X = Pos.Right(gameNameLabel) + 1,

            // Fill remaining horizontal space
            Width = Dim.Fill(),
        };

        // Create login button
        var btnSearch = new Button()
        {
            Text = "Search",
            Y = Pos.Bottom(gameNameLabel) + 1,
            // center the login button horizontally
            X = Pos.Center(),
            IsDefault = true,
        };

        // When login button is clicked display a message popup
        btnSearch.Clicked += () =>
        {
            var result = _bggApiClient.Search(gameNameText.Text.ToString()!);
            //if (gameNameText.Text == "admin" && passwordText.Text == "password")
            //{
            //    MessageBox.Query("Logging In", "Login Successful", "Ok");
            //    Application.RequestStop();
            //}
            //else
            //{
            //    MessageBox.ErrorQuery("Logging In", "Incorrect username or password", "Ok");
            //}
        };

        // Add the views to the Window
        Add(gameNameLabel, gameNameText, btnSearch);
    }
}
