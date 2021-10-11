using System;

namespace NNOne.Logic
{
    public class Network
    {
        #region Hyper Parameters
        const int n0 = 2;
        const int n1 = 4;
        const int n2 = 1;
        const double eta = 1.0;

        const int seed = 123;
        #endregion

        #region Output
        public double[] a0 = new double[n0];
        public double[] a1 = new double[n1];
        public double[] a2 = new double[n2];

        public double[,] w1 = new double[n1, n0];
        public double[,] w2 = new double[n2, n1];

        public double[] b1 = new double[n1];
        public double[] b2 = new double[n2];

        

        public double Cost { get; set; }
        #endregion

        #region Data
        private double[,] x =
        {
            { 0.000, 0.000 },
            { 0.000, 1.000 },
            { 1.000, 0.000 },
            { 1.000, 1.000 }
        };

        private double[] y =
        {
            0.000,
            1.000,
            1.000,
            0.000
        };

        private double[] z1;
        private double[] z2;
        #endregion

        private Random _random;

        #region Initial
        public Network()
        {
            _random = new Random(seed);

            SetRandomBiases();

            SetRandomWeights();
        }

        private void SetRandomBiases()
        {
            for (int i = 0; i < b1.Length; i++)
            {
                b1[i] = ((double)_random.Next(-10, 10) / 10);
            }

            for (int i = 0; i < b2.Length; i++)
            {
                b2[i] = ((double)_random.Next(-10, 10) / 10);
            }
        }

        private void SetRandomWeights()
        {
            for (int i = 0; i < w1.GetLength(0); i++)
            {
                for (int j = 0; j < w1.GetLength(1); j++)
                {
                    w1[i, j] = ((double)_random.Next(-10, 10) / 10);
                }
            }

            for (int i = 0; i < w2.GetLength(0); i++)
            {
                for (int j = 0; j < w2.GetLength(1); j++)
                {
                    w2[i, j] = ((double)_random.Next(-10, 10) / 10);
                }
            }
        }
        #endregion

        public double UpdateNetwork(int index)
        {
            Cost = 0.0;
            z1 = new double[n1];
            z2 = new double[n1];

            SetInputLayer(index);

            CalculateHiddenLayer();

            CalculateOutputLayer();

            CalculateCost(index);

            return Cost;
        }

        #region Update Network
        private void SetInputLayer(int index)
        {
            for (int k = 0; k < n0; k++)
                a0[k] = x[index, k];
        }

        private void CalculateHiddenLayer()
        {
            for (int j = 0; j < n1; j++)
            {
                z1[j] = b1[j];

                for (int k = 0; k < n0; k++)
                    z1[j] += a0[k] * w1[j, k];

                a1[j] = Sigmoid(z1[j]);
            }
        }

        private void CalculateOutputLayer()
        {
            for (int i = 0; i < n2; i++)
            {
                z2[i] = b2[i];

                for (int j = 0; j < n1; j++)
                    z2[i] += a1[j] * w2[i, j];


                a2[i] = Sigmoid(z2[i]);
            }
        }

        private void CalculateCost(int index)
        {
            for (int i = 0; i < n2; i++)
                Cost += CostFunction(y[index], a2[i]);
        }

        #endregion


        public void TrainNetworkBatchWise()
        {
            double[] dCdb2avg = new double[n2];
            double[,] dCdw2avg = new double[n2, n1];
            double[] dCdb1avg = new double[n1];
            double[,] dCdw1avg = new double[n1, n0];

            for(int index = 0; index < x.GetLength(0); index++)
            {
                UpdateNetwork(index);

                // output layer gradient for biases and weights 
                for(int i = 0; i < n2; i++)
                {
                    var dCdb2 = PartialCostPartialOutputBias(index, i);
                    dCdb2avg[i] += ((double)1 / n2) * dCdb2;

                    // update weights from previous (hidden) layer
                    for (int j = 0; j < n1; j++)
                    {
                        var dCdw2 = PartialCostPartialOutputWeight(index, i, j);
                        dCdw2avg[i, j] += ((double)1 / n2) * dCdw2;
                    }
                }

                // hidden layer gradient for biases and weights 
                for (int i = 0; i < n2; i++)
                {
                    for (int j = 0; j < n1; j++)
                    {
                        var dCdb1 = PartialCostPartialHiddenBias(index, i, j);
                        dCdb1avg[j] += ((double)1 / n1) * dCdb1;

                        for (int k = 0; k < n0; k++)
                        {
                            var dCdw1 = PartialCostPartialHiddenWeight(index, i, j, k);
                            dCdw1avg[j, k] += ((double)1 / n1) * dCdw1; 
                        }
                    }
                }
            }

            UpdateHiddenLayer(dCdb1avg, dCdw1avg);

            UpdateOutputLayer(dCdb2avg, dCdw2avg);
        }

        #region Train Network
        private void UpdateHiddenLayer(double[] dCdb1avg, double[,] dCdw1avg)
        {
            for (int j = 0; j < n1; j++)
            {
                b1[j] = b1[j] - eta * dCdb1avg[j];

                for (int k = 0; k < n0; k++)
                    w1[j, k] = w1[j, k] - eta * dCdw1avg[j, k];
            }
        }

        private void UpdateOutputLayer(double[] dCdb2avg, double[,] dCdw2avg)
        {
            for (int i = 0; i < n2; i++)
            {
                b2[i] = b2[i] - eta * dCdb2avg[i];

                for (int j = 0; j < n1; j++)
                    w2[i, j] = w2[i, j] - eta * dCdw2avg[i, j];
            }
        }
        #endregion

        #region functions
        private double CostFunction(double expected, double input)
        {
            return 0.5 * Math.Pow((expected - input), 2); ;
        }

        private double Sigmoid(double input)
        {
            return 1.0 / (1.0 + Math.Exp(-1.0 * input));
        }

        private double SigmoidPrime(double input)
        {
            // calculate the derivative of the Sigmoid function
            return Sigmoid(input) * (1 - Sigmoid(input));
        }

        private double PartialCostPartialOutputBias(int index, int i)
        {
            return (a2[i] - y[index]) * SigmoidPrime(z2[i]);
        }

        private double PartialCostPartialOutputWeight(int index, int i, int j)
        {
            return PartialCostPartialOutputBias(index, i) * a1[j];
        }


        // Only works with one neuron in output layer
        private double PartialCostPartialHiddenBias(int index, int i, int j)
        {
            return PartialCostPartialOutputBias(index, i) * w2[i, j] * SigmoidPrime(z1[j]);
        }

        // Only works with one neuron in output layer
        private double PartialCostPartialHiddenWeight(int index, int i, int j, int k)
        {
            return PartialCostPartialHiddenBias(index, i, j) * a0[k];
        }
        #endregion
    }
}
