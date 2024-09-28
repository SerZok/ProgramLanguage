using ProgramLanguage.Model;
using ReactiveUI;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
namespace ProgramLanguage.ViewModels;

public class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        lexA = new LexAnal();
        LexAnalysisCommand = ReactiveCommand.Create(LexAnalysis);

    }
    private void LexAnalysis()
    {
        lexA.Code = Code;
        var tokens = lexA.Scanner();
        Lexem = string.Join("\n", tokens.Select(t => t.ToString()));

        PositionCode = "Строка: " + lexA.CountLines.ToString() + " Позиция: " +  lexA.CountChar.ToString();
    }

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
        set=> this.RaiseAndSetIfChanged(ref _code, value);
    }
    public ReactiveCommand<Unit, Unit> LexAnalysisCommand { get; }
}
