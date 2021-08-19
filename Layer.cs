using System.Collections.Generic;

namespace NeuralNetwork
{
    public class Layer //Класс слоя
    {
        public List<Neuron> Neurons { get; } //Массив с количеством нейронов
        public int NeuronCount => Neurons?.Count ?? 0; //Количество нейронов (с проверкой на null)

        public NeuronType Type;

        public Layer(List<Neuron> neurons, NeuronType type = NeuronType.Normal)
        {
            //TODO: Проверка нейронов на соответсвие типам
            Neurons = neurons;
            Type = type;
        }

        public List<double> GetSignals()
        {
            var result = new List<double>();
            foreach (var neuron in Neurons)
            {
                result.Add(neuron.Output);
            }
            return result;
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
