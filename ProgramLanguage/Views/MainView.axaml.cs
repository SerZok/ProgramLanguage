using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ProgramLanguage.ViewModels;
using System.Diagnostics;
using System.IO;
using System.Reflection.Emit;
using static System.Net.WebRequestMethods;

namespace ProgramLanguage.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        var textBoxCode = this.FindControl<TextBox>("TextBoxCode");
        textBoxCode.TextChanged += TextBoxCode_TextChanged;

    }

    private void TextBoxCode_TextChanged(object? sender, TextChangedEventArgs e)
    {
        var textBox = sender as TextBox;
        var textBoxPos = this.FindControl<TextBox>("TextBoxPos");

        // Получаем текст и позицию каретки
        string text = textBox.Text;
        int caretPosition = textBox.CaretIndex;

        // Определяем строку, в которой находится каретка
        int lineNumber = text.Substring(0, caretPosition).Split('\n').Length;
        int columnPosition = 0;

        if (caretPosition > 0)
        {
            // Определяем позицию в строке (после последнего символа новой строки)
            int lastNewLineIndex = text.LastIndexOf('\n', caretPosition - 1);
            columnPosition = caretPosition - (lastNewLineIndex + 1);
        }

        // Обновляем TextBoxPos с информацией о строке и позиции
        textBoxPos.Text = $"Строка: {lineNumber}; Позиция: {columnPosition}";
    }

    private async void OpenFileButton_Clicked(object sender, RoutedEventArgs args)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Text File",
            AllowMultiple = false
        });

        if (files.Count >= 1)
        {
            // Читаем содержимое первого выбранного файла
            await using var stream = await files[0].OpenReadAsync();
            using var streamReader = new StreamReader(stream);
            var fileContent = await streamReader.ReadToEndAsync();

            // Передаём содержимое файла в ViewModel через команду
            var viewModel = (MainViewModel)DataContext;
            viewModel.Code= fileContent;
        }
    }
}
