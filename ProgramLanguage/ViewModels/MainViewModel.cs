using ProgramLanguage.Model;
using ReactiveUI;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
namespace ProgramLanguage.ViewModels;
using System.Windows.Input;
using Avalonia.Input;


public class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        lexA = new LexAnal();
        LexAnalysisCommand = ReactiveCommand.Create(LexAnalysis);
    }
    public string SelectedFilePath { get;set; }
    private void LexAnalysis()
    {
        lexA.Code = Code;
        TextErrors = string.Empty;
        var tokens = lexA.Scanner();
        _isError = lexA.IsError;

        Lexem = string.Join("\n", tokens.Select(t => t.ToString()));
        if (_isError)
        {
            _charError = lexA.CharError;
            TextErrors += $"Обнаружена лексическая ошибка LexErr_1: Лексема не может начинаться с символа {_charError} " +
                $"номер строки: {lexA.CountLines} позиция: {lexA.CountChar}";
        }
        //PositionCode = "Строка: " + lexA.CountLines.ToString() + " Позиция: " + lexA.CountChar.ToString();
    }
    private bool _isError = false;
    private char _charError;

    private readonly LexAnal lexA;

    private string _textError;
    public string TextErrors
    {
        get => _textError;
        set =>this.RaiseAndSetIfChanged(ref _textError, value);
    }

    private string _positionCode;
    public string PositionCode
    {
        get => _positionCode;
        set => this.RaiseAndSetIfChanged(ref _positionCode, value);
    }

    private string _lexem;
    public string Lexem { 
        get => _lexem;
        set => this.RaiseAndSetIfChanged(ref _lexem, value);
    }

    private string _code;
    public string Code
    {
        get => _code;
        set {
            this.RaiseAndSetIfChanged(ref _code, value);
        }


    }
    public ReactiveCommand<Unit, Unit> LexAnalysisCommand { get; }
    public ReactiveCommand<Unit, Unit> FileReadCommand { get; }
}
