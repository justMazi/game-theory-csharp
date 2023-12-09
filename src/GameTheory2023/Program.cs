using GameTheoryEssentials.SelfPlay;
using Python.Runtime;

Runtime.PythonDLL = @"C:\Users\Mazi\AppData\Local\Programs\Python\Python311\python311.dll";
PythonEngine.Initialize();
using (Py.GIL())
{
    int numberOfTurns = 100;
    
    var game1 = new GameSelfPlay();
    var data1 = game1.Play(numberOfTurns: numberOfTurns, averagedSelfPlay: false);
    
    var game2 = new GameSelfPlay();
    var data2 = game2.Play(numberOfTurns: numberOfTurns, averagedSelfPlay: true);
    
    dynamic plt = Py.Import("matplotlib.pyplot");

    plt.plot(data1);
    plt.plot(data2);
    
    plt.show();
}