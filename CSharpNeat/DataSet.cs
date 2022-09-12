using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpNeat
{
    class DataSet
    {
        private double[] inputs;
        private double[] targetOutputs;
        public DataSet(double[] inputs, double[] targetOutputs)
        {

            this.Inputs = inputs;
            this.TargetOutputs = targetOutputs;

        }

        public double[] Inputs { get => inputs; set => inputs = value; }
        public double[] TargetOutputs { get => targetOutputs; set => targetOutputs = value; }
    }
}
