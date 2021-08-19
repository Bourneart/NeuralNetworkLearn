using System;
using System.Collections.Generic;

namespace NeuralNetwork
{
    public class Neuron
    {
        public List<double> Weights { get; } //Массив коэфициентов для расчета веса нейрона
        public List<double> Inputs { get; } //Сохраняем входящие сигналы для обучения нейронной сети
        public NeuronType neuronType { get; } //Тип нейрона
        public double Output { get; private set; } //Результаты после выполнения всех операций с нейронами
        public double Delta { get; private set; } //Переменная дельта для корректировки весов во время обучения сети

        public Neuron(int inputCount, NeuronType type = NeuronType.Normal) //Конструктор для создания нейронов (количество входных нейронов, тип нейронов)
        {
            neuronType = type;
            Weights = new List<double>(); //Определяем (инициализируем) новый лист для коэфициентов
            Inputs = new List<double>(); //Инициализируем список входных сигналов

            InitWeightsRandomValue(inputCount);
        }

        private void InitWeightsRandomValue(int inputCount)
        {
            var rnd = new Random();
            for (int i = 0; i < inputCount; i++) //Заполняем лист коэфициентами в зависимости от входящих нейронов
            {
                if(neuronType == NeuronType.Input)
                {
                    Weights.Add(1);
                }
                else
                {
                    Weights.Add(rnd.NextDouble());
                }
                Inputs.Add(0);
            }
        }

        public double FeedForward(List<double> inputs)
        {
            for (int i = 0; i < inputs.Count; i++) //Сохранение входящих сигналов
            {
                Inputs[i] = inputs[i];
            }

            var sum = 0.0;
            for (int i = 0; i < inputs.Count; i++)
            {
                sum += inputs[i] * Weights[i];
            }

            if (neuronType != NeuronType.Input)
            {
                Output = Sigmoid(sum);
            }
            else
            {
                Output = sum;
            }
            return Output;
        }

        private double Sigmoid(double x) // Вычисление сигмоидальной функции
        {
            var result = 1.0 / 1.0 + (Math.Pow(Math.E, -x));
            return result;
        }

        private double SigmoidDx(double x) //Вычисление производной сигмода
        {
            var sigmoid = Sigmoid(x);
            var result = sigmoid / (1 - sigmoid);
            return result;
        }

        public void Learn (double error, double learningRate)
        {
            if(neuronType == NeuronType.Input)
            {
                return;
            }
            Delta = error * SigmoidDx(Output);

            for (int i = 0; i < Weights.Count; i++)
            {
                var weights = Weights[i];
                var inputs = Inputs[i];

                var newWeight = weights - inputs * Delta * learningRate;
                Weights[i] = newWeight;
            }
        }

        public override string ToString()
        {
            return Output.ToString();
        }
    }
}
