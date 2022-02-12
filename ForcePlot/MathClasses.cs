//------------------------------------------------------------------------------------------------
//Copyright © 2012 Nuwan Yapa
//
//This file is part of ForcePlot.
//
//ForcePlot is free software: you can redistribute it and/or modify it under the terms of the
//GNU General Public License as published by the Free Software Foundation, either version 2 of
//the License, or (at your option) any later version.
//
//ForcePlot is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
//even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//General Public License for more details.
//
//You should have received a copy of the GNU General Public License along with ForcePlot. If not,
//see http://www.gnu.org/licenses/.
//------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForcePlot
{
    public enum MathOperations { add, substract, multiply, divide, raise, log, NA }
    public enum MathFunctions { Zero, sin, cos, tan, cosec, sec, cot, asin, acos, atan, abs }

    public interface term
    {
        double getValue(double x, double y);
    }

    public class singleValue : term
    {
        public singleValue(double value)
        {
            val = value;
        }
        public double getValue(double x, double y)
        {
            return val;
        }
        public override string ToString()
        {
            return val.ToString();
        }
        double val;
    }

    public class variable : term
    {
        public variable(bool true_for_X_false_for_Y)
        {
            isX = true_for_X_false_for_Y;
        }
        public double getValue(double x, double y)
        {
            if (isX) { return x; }
            else { return y; }
        }
        public override string ToString()
        {
            if (isX)
            { return "x"; }
            else
            { return "y"; }
        }
        bool isX;
    }

    public class function : term
    {
        public function(term input, MathFunctions funType)
        {
            term1 = input;
            fun = funType;
        }
        public double getValue(double x, double y)
        {
            switch (fun)
            {
                case MathFunctions.sin:
                    return sin(term1.getValue(x, y));
                case MathFunctions.cos:
                    return cos(term1.getValue(x, y));
                case MathFunctions.tan:
                    return tan(term1.getValue(x, y));
                case MathFunctions.cosec:
                    return cosec(term1.getValue(x, y));
                case MathFunctions.sec:
                    return sec(term1.getValue(x, y));
                case MathFunctions.cot:
                    return cot(term1.getValue(x, y));
                case MathFunctions.asin:
                    return asin(term1.getValue(x, y));
                case MathFunctions.acos:
                    return acos(term1.getValue(x, y));
                case MathFunctions.atan:
                    return atan(term1.getValue(x, y));
                case MathFunctions.abs:
                    return abs(term1.getValue(x, y));
                default:
                    return term1.getValue(x, y);
            }
        }
        double sin(double a)
        {
            a = Math.Sin(a);
            return a;
        }
        double cos(double a)
        {
            a = Math.Cos(a);
            return a;
        }
        double tan(double a)
        {
            a = Math.Tan(a);
            return a;
        }
        double cosec(double a)
        {
            a = 1 / Math.Sin(a);
            return a;
        }
        double sec(double a)
        {
            a = 1 / Math.Cos(a);
            return a;
        }
        double cot(double a)
        {
            a = 1 / Math.Tan(a);
            return a;
        }
        double asin(double a)
        {
            a = Math.Asin(a);
            return a;
        }
        double acos(double a)
        {
            a = Math.Acos(a);
            return a;
        }
        double atan(double a)
        {
            a = Math.Atan(a);
            return a;
        }
        double abs(double a)
        {
            a = Math.Abs(a);
            return a;
        }
        public override string ToString()
        {
            switch (fun)
            {
                case MathFunctions.sin:
                    return "sin(" + term1.ToString() + ")";
                case MathFunctions.cos:
                    return "cos(" + term1.ToString() + ")";
                case MathFunctions.tan:
                    return "tan(" + term1.ToString() + ")";
                case MathFunctions.cosec:
                    return "cosec(" + term1.ToString() + ")";
                case MathFunctions.sec:
                    return "sec(" + term1.ToString() + ")";
                case MathFunctions.cot:
                    return "cot(" + term1.ToString() + ")";
                case MathFunctions.asin:
                    return "asin(" + term1.ToString() + ")";
                case MathFunctions.acos:
                    return "acos(" + term1.ToString() + ")";
                case MathFunctions.atan:
                    return "atan(" + term1.ToString() + ")";

                case MathFunctions.abs:
                    return "abs(" + term1.ToString() + ")";
                default:
                    return term1.ToString();
            }
        }
        term term1;
        MathFunctions fun;
    }

    public class expression : term
    {
        public expression(term t1, term t2, MathOperations operation)
        {
            term1 = t1;
            term2 = t2;
            op = operation;
        }
        public double getValue(double x, double y)
        {
            switch (op)
            {
                case MathOperations.add:
                    return add(term1.getValue(x, y), term2.getValue(x, y));
                case MathOperations.substract:
                    return substract(term1.getValue(x, y), term2.getValue(x, y));
                case MathOperations.multiply:
                    return multiply(term1.getValue(x, y), term2.getValue(x, y));
                case MathOperations.divide:
                    return divide(term1.getValue(x, y), term2.getValue(x, y));
                case MathOperations.raise:
                    return pow(term1.getValue(x, y), term2.getValue(x, y));
                case MathOperations.log:
                    return log(term1.getValue(x, y), term2.getValue(x, y));
                default:
                    return term1.getValue(x, y);
            }
        }
        double add(double a, double b)
        {
            double ret;
            ret = a + b;
            return ret;
        }
        double substract(double a, double b)
        {
            double ret;
            ret = a - b;
            return ret;
        }
        double multiply(double a, double b)
        {
            double ret;
            ret = a * b;
            return ret;
        }
        double divide(double a, double b)
        {
            double ret;
            ret = a / b;
            return ret;
        }
        double pow(double a, double b)
        {
            double ret;
            ret = Math.Pow(a, b);
            return ret;
        }
        double log(double a, double b)
        {
            double ret;
            ret = Math.Log(b, a); //parameters reversed
            return ret;
        }
        public override string ToString()
        {
            switch (op)
            {
                case MathOperations.add:
                    return "(" + term1.ToString() + " + " + term2.ToString() + ")";
                case MathOperations.substract:
                    return "(" + term1.ToString() + " - " + term2.ToString() + ")";
                case MathOperations.multiply:
                    return "(" + term1.ToString() + " * " + term2.ToString() + ")";
                case MathOperations.divide:
                    return "(" + term1.ToString() + " / " + term2.ToString() + ")";
                case MathOperations.raise:
                    return "(" + term1.ToString() + " ^ " + term2.ToString() + ")";
                case MathOperations.log:
                    return "log(" + term1.ToString() + ", " + term2.ToString() + ")";
                default:
                    return term1.ToString();
            }
        }
        term term1;
        term term2;
        MathOperations op;
    }
}
