using System.Collections.Generic;

namespace NeuralNetwork
{
    public class Topology //Определение топологии нейронной сети (набор ее свойств)
    {
        public int InputCount { get; } //Количество входных данных в нейронную сеть
        public int OutputCount { get; } //Количество выходных данных из нейронной сети
        public List<int> HiddenLayers { get; } //Количество скрытых слоев внутри нейронной сети
        public Topology(int inputCount, int outputCount, params int[] layers)
        {
            InputCount = inputCount;
            OutputCount = outputCount;
            HiddenLayers = new List<int>();
            HiddenLayers.AddRange(layers);
        }
    }
}
