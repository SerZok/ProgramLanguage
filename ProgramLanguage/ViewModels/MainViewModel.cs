using ProgramLanguage.Model;
using ReactiveUI;
using System.ComponentModel;
using System.Reactive;
namespace ProgramLanguage.ViewModels;

public class MainViewModel : ViewModelBase, INotifyPropertyChanged
{
    public MainViewModel()
    {
        lexA = new LexAnal();
        LexAnalysisCommand = ReactiveCommand.Create(LexAnalysis);

    }
    private void LexAnalysis()
    {
        lexA.Code = Code;
        Lexem = lexA.Scanner();
    }

    public LexAnal lexA;
    public string TextErrors => "Текст с ошибками";
    public string TextNumError => "Позиция с ошибками";

    private string _lexem;
    public string Lexem { get => _lexem;
        set
        {
            _lexem = value;
            OnPropertyChanged("Lexem");
        }
    }

    private string _code;
    public string Code
    {
        get => _code;
        set
        {
            _code = value;
            OnPropertyChanged("Code");
        }
    }




    public ReactiveCommand<Unit, Unit> LexAnalysisCommand { get; }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
