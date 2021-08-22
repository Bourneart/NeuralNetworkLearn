﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork
{
    public class NeuralNetwork //Класс нейронной сети
    {
        public Topology Topology { get; }
        public List<Layer> Layers { get; }

        public NeuralNetwork(Topology topology)
        {
            Topology = topology;
            Layers = new List<Layer>();

            CreateInputLayer();
            CreateHiddenLayers();
            CreateOutputLayer();
        }

        public Neuron FeedForward (params double[] inputSignals)
        {
            SendSignalsToInputNeurons(inputSignals);
            FeedForwardAllLayersAfterInput();

            if (Topology.OutputCount == 1)
            {
                return Layers.Last().Neurons[0];
            }
            else
            {
                return Layers.Last().Neurons.OrderByDescending(n => n.Output).First();
            }
        }

        public double Learn(double[] expected, double[,] inputs, int epoch) //Обучение нейронной сети
        {
            var signals = Normalization(inputs);
            var error = 0.0;
            for (int i = 0; i < epoch; i++)
            {
                for (int j = 0; j < expected.Length; j++)
                {
                    var output = expected[j];
                    var input = GetRow(signals, j);

                    error += Backpropagation(output, input);
                }
            }
            var result = error / epoch;
            return result;
        }

        public static double[] GetRow(double[,] matrix, int row)
        {
            var cols = matrix.GetLength(1);
            var array = new double[cols];

            for(int i = 0; i < cols; i++)
            {
                array[i] = matrix[row, i];
            }
            return array;
        }

        private double[,] Scalling(double[,] inputs) //Алгоритм масштабирования нейронной сети
        {
            var result = new double[inputs.GetLength(0), inputs.GetLength(1)];

            for(int col = 0; col < inputs.GetLength(1); col++)
            {
                var min = inputs[0, col];
                var max = inputs[0, col];

                for(int row = 1; row < inputs.GetLength(0); row++)
                {
                    var item = inputs[row, col];

                    if(item < min)
                    {
                        min = item;
                    }
                    if(item > max)
                    {
                        max = item;
                    }
                }
                var divider = max - min;
                for (int row = 1; row < inputs.GetLength(0); row++)
                {
                    result[row, col] = (inputs[row, col] - min) / divider;
                }
            }

            return result;
        }

        private double[,] Normalization(double[,] inputs) //Алгоритм нормализации нейронной сети
        {
            var result = new double[inputs.GetLength(0), inputs.GetLength(1)];

            for (int col = 0; col < inputs.GetLength(1); col++)
            {
                //Вычисляем среднее значение сигнала нейрона
                var sum = 0.0;
                for (int row = 0; row < inputs.GetLength(0); row++)
                {
                    sum += inputs[row, col];
                }
                var average = sum / inputs.GetLength(0);

                //Вычисляем значение стандартного квадратичного отклонения сигнала нейрона
                var error = 0.0;
                for (int row = 0; row < inputs.GetLength(0); row++)
                {
                    error += Math.Pow((inputs[row, col] - average), 2);
                }
                var standartError = Math.Sqrt(error / inputs.GetLength(0));

                //Вычисляем и записываем новое значение сигнала нейрона
                for (int row = 0; row < inputs.GetLength(0); row++)
                {
                    result[row, col] = (inputs[row, col] - average) / standartError;
                }
            }

            return result;
        }

            private double Backpropagation(double expected, params double[] inputs) //Алгоритм обратного распространения ошибки для обучения нейронной сети
        {
            var actual = FeedForward(inputs).Output;
            var difference = actual - expected;

            foreach (var neuron in Layers.Last().Neurons)
            {
                neuron.Learn(difference, Topology.LearningRate);
            }

            for (int j = Layers.Count - 2; j >= 0; j--)
            {
                var layer = Layers[j];
                var prevLayer = Layers[j + 1];

                for (int i = 0; i < layer.NeuronCount; i++)
                {
                    var neuron = layer.Neurons[i];

                    for (int k = 0; k < prevLayer.NeuronCount; k++)
                    {
                        var prevNeuron = prevLayer.Neurons[k];
                        var error = prevNeuron.Weights[i] * prevNeuron.Delta;
                        neuron.Learn(error, Topology.LearningRate);
                    }
                }
            }

            var result = difference * difference;
            return result;
        }

        private void FeedForwardAllLayersAfterInput()
        {
            for (int i = 1; i < Layers.Count; i++)
            {
                var layer = Layers[i];
                var previousLayerSignals = Layers[i - 1].GetSignals();

                foreach (var neuron in layer.Neurons)
                {
                    neuron.FeedForward(previousLayerSignals);
                }
            }
        }

        private void SendSignalsToInputNeurons (params double[] inputSignals)
        {
            for (int i = 0; i < inputSignals.Length; i++)
            {
                var signal = new List<double>() { inputSignals[i] };
                var neuron = Layers[0].Neurons[i];

                neuron.FeedForward(signal);
            }
        }

        private void CreateOutputLayer() //Создаем слой выходящих нейронов
        {
            var outputNeurons = new List<Neuron>();
            var lastLayer = Layers.Last();
            for (int i = 0; i < Topology.OutputCount; i++)
            {
                var neuron = new Neuron(lastLayer.NeuronCount, NeuronType.Output);
                outputNeurons.Add(neuron);
            }
            var outputLayer = new Layer(outputNeurons, NeuronType.Output);
            Layers.Add(outputLayer);
        }

        private void CreateHiddenLayers() //Создание необходимого количества скрытых слоев
        {
            for (int j = 0; j < Topology.HiddenLayers.Count; j++)
            {
                var hiddenNeurons = new List<Neuron>();
                var lastLayer = Layers.Last();
                for (int i = 0; i < Topology.HiddenLayers[j]; i++)
                {
                    var neuron = new Neuron(lastLayer.NeuronCount);
                    hiddenNeurons.Add(neuron);
                }
                var hiddenLayer = new Layer(hiddenNeurons);
                Layers.Add(hiddenLayer);
            }
        }

        private void CreateInputLayer() //Создаем слой входящих нейронов
        {
            var inputNeurons = new List<Neuron>();

            for (int i = 0; i < Topology.InputCount; i++)
            {
                var neuron = new Neuron(1, NeuronType.Input);
                inputNeurons.Add(neuron);
            }
            var inputLayer = new Layer(inputNeurons, NeuronType.Input);
            Layers.Add(inputLayer);
        }
    }
}
