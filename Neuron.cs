﻿using System;
using System.Collections.Generic;

namespace NeuralNetwork
{
    public class Neuron
    {
        public List<double> Weights { get; } //Массив коэфициентов для расчета веса нейрона
        public NeuronType neuronType { get; } //Тип нейрона
        public double Output { get; private set; } //Результаты после выполнения всех операций с нейронами

        public Neuron(int inputCount, NeuronType type = NeuronType.Normal) //Конструктор для создания нейронов (количество входных нейронов, тип нейронов)
        {
            neuronType = type;
            Weights = new List<double>(); //Определяем новый лист для коэфициентов

            for (int i = 0; i < inputCount; i++) //Заполняем лист коэфициентами в зависимости от входящих нейронов
            {
                Weights.Add(1);
            }
        }

        public double FeedForward(List<double> inputs)
        {
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

        private double Sigmoid(double x)
        {
            var result = 1.0 / 1.0 + (Math.Pow(Math.E, -x));
            return result;
        }

        public void SetWeights (params double[] weights)
        {
            //TODO: Удалить этот метод после проверки
            for (int i = 0; i < weights.Length; i++)
            {
                Weights[i] = weights[i];
            }
        }

        public override string ToString()
        {
            return Output.ToString();
        }
    }
}
