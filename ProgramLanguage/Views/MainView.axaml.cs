using Avalonia.Controls;
using System.Diagnostics;
using System.Reflection.Emit;

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

        // Определяем позицию в строке (после последнего символа новой строки)
        int lastNewLineIndex = text.LastIndexOf('\n', caretPosition - 1);
        int columnPosition = caretPosition - (lastNewLineIndex + 1);

        // Обновляем TextBoxPos с информацией о строке и позиции
        textBoxPos.Text = $"Строка: {lineNumber}; Позиция: {columnPosition}";
    }
}
