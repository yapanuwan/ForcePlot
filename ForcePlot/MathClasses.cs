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
using System.Drawing;

namespace ForcePlot
{
    public enum MathOperations { add, substract, multiply, divide, raise, log, NA }
    public enum MathFunctions { Zero, sin, cos, tan, cosec, sec, cot, asin, acos, atan, abs }
    public enum MathConstants { e, pi }

    public abstract class term
    {
        public abstract double getValue(double x, double y);
        public abstract bool isConstant();
        public abstract int getPriority();
        public abstract term optimize();
        public abstract Bitmap ToBitmap(Font font, Brush brush);

        protected static Graphics GetGraphics(Bitmap ret)
        {
            Graphics graphic = Graphics.FromImage(ret);
            graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphic.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            return graphic;
        }
        protected static Bitmap encloseInBrackets(Bitmap bitmap, Font font, Brush brush)
        {
            int BrackLength = measureString(font, ")");
            Bitmap ret = new Bitmap((int)(bitmap.Width / 1.1) + (2 * BrackLength), 50);
            Graphics graphic = Graphics.FromImage(ret);
            graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphic.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            graphic.DrawString("(", font, brush, new RectangleF(0, 0, 0, 50));
            graphic.DrawImage(bitmap, BrackLength, 3, (int)(bitmap.Width / 1.1), 44);
            graphic.DrawString(")", font, brush, new RectangleF(ret.Width - BrackLength, 0, BrackLength, 50));
            return ret;
        }
        protected static int measureString(Font font, string valString)
        {
            return (int)Graphics.FromImage(new Bitmap(1, 1)).MeasureString(valString, font).Width;
        }
    }

    public class singleValue : term
    {
        public singleValue(double value)
        {
            val = value;
        }
        public override double getValue(double x, double y)
        {
            return val;
        }
        public override term optimize()
        {
            return this;
        }
        public override bool isConstant()
        {
            return true;
        }
        public override int getPriority()
        {
            return 6;
        }
        public override Bitmap ToBitmap(Font font, Brush brush)
        {
            string valString = val.ToString();
            switch (val.ToString())
            {
                case "0.333333333333333":
                    valString = "⅓";
                    break;
                case "0.666666666666667":
                    valString = "⅔";
                    break;
                case "0.125":
                    valString = "⅛";
                    break;
                case "0.25":
                    valString = "¼";
                    break;
                case "0.375":
                    valString = "⅜";
                    break;
                case "0.5":
                    valString = "½";
                    break;
                case "0.625":
                    valString = "⅝";
                    break;
                case "0.75":
                    valString = "¾";
                    break;
                case "0.875":
                    valString = "⅞";
                    break;
            }
            Bitmap ret = new Bitmap(measureString(font, valString) - 10, 50);
            Graphics graphic = GetGraphics(ret);
            graphic.DrawString(valString, font, brush, new PointF(-5, 0));
            return ret;
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
        public override double getValue(double x, double y)
        {
            if (isX) { return x; }
            else { return y; }
        }
        public override term optimize()
        {
            return this;
        }
        public override bool isConstant()
        {
            return false;
        }
        public override int getPriority()
        {
            return 6;
        }
        public override Bitmap ToBitmap(Font font, Brush brush)
        {
            string val = "y";
            if (isX) { val = "x"; }
            Bitmap ret = new Bitmap(measureString(font, val) - 10, 50);
            Graphics graphic = GetGraphics(ret);
            graphic.DrawString(val, font, brush, new PointF(-5, 0));
            return ret;
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
        public override double getValue(double x, double y)
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
                    throw new Exception("Unknown mathematical function");
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
        public override term optimize()
        {
            if (this.isConstant())
            {
                return new singleValue(this.getValue(1, 1));
            }
            else
            {
                return new function(term1.optimize(), fun);
            }
        }
        public override bool isConstant()
        {
            return term1.isConstant();
        }
        public override int getPriority()
        {
            return 5;
        }
        public override Bitmap ToBitmap(Font font, Brush brush)
        {
            string funString = "fun";
            string endBracket = "";
            switch (fun)
            {
                case MathFunctions.sin:
                    funString = "Sin";
                    break;
                case MathFunctions.cos:
                    funString = "Cos";
                    break;
                case MathFunctions.tan:
                    funString = "Tan";
                    break;
                case MathFunctions.cosec:
                    funString = "Cosec";
                    break;
                case MathFunctions.sec:
                    funString = "Sec";
                    break;
                case MathFunctions.cot:
                    funString = "Cot";
                    break;
                case MathFunctions.asin:
                    funString = "Sin⁻¹";
                    break;
                case MathFunctions.acos:
                    funString = "Cos⁻¹";
                    break;
                case MathFunctions.atan:
                    funString = "Tan⁻¹";
                    break;
                case MathFunctions.abs:
                    funString = "|";
                    endBracket = "|";
                    break;
                default:
                    throw new Exception("Unknown mathematical function");
            }
            Bitmap term1Bit = term1.ToBitmap(font, brush);
            if (!(term1 is singleValue) && !(term1 is variable) && fun != MathFunctions.abs)
            {
                term1Bit = encloseInBrackets(term1Bit, font, brush);
            }
            int funcLength = measureString(font, funString);
            int endBrackLength = measureString(font, endBracket);
            Bitmap ret = new Bitmap(funcLength + term1Bit.Width + endBrackLength, 50);
            Graphics graphic = GetGraphics(ret);
            graphic.DrawString(funString, font, brush, new RectangleF(0, 0, funcLength + 1, 50));
            graphic.DrawImageUnscaled(term1Bit, funcLength + 1, 0);
            graphic.DrawString(endBracket, font, brush, new RectangleF(ret.Width - endBrackLength, 0, endBrackLength, 50));
            return ret;
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
                    throw new Exception("Unknown mathematical function");
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
        public override double getValue(double x, double y)
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
                case MathOperations.log:
                    return log(term1.getValue(x, y), term2.getValue(x, y));
                default:
                    throw new Exception("Unknown mathematical operation");
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
        double log(double a, double b)
        {
            double ret;
                    ret = Math.Log(b, a); //parameters reversed
            return ret;
        }
        public override term optimize()
        {
            if (this.isConstant())
            {
                return new singleValue(this.getValue(1, 1));
            }
            else
            {
                return new expression(term1.optimize(), term2.optimize(), op);
            }
        }
        public override bool isConstant()
        {
            return term1.isConstant() && term2.isConstant();
        }
        public override int getPriority()
        {
            switch (op)
            {
                case MathOperations.add:
                    return 1;
                case MathOperations.substract:
                    return 1;
                case MathOperations.multiply:
                    return 2;
                case MathOperations.divide:
                    return 2;
                case MathOperations.log:
                    return 3;
                default:
                    throw new Exception("Unknown mathematical operation");
            }
        }
        public override Bitmap ToBitmap(Font font, Brush brush)
        {
            Bitmap term1Bit = term1.ToBitmap(font, brush);
            Bitmap term2Bit = term2.ToBitmap(font, brush);
            if ((op == MathOperations.multiply || op == MathOperations.divide) && (term1Bit.Tag is Bitmap[] || term2Bit.Tag is Bitmap[]))
            {
                string topJoinStr = "";
                string bottomJoinStr = "";
                Bitmap topLeft = new Bitmap(1,50);
                Bitmap topRight = new Bitmap(1, 50);
                Bitmap bottomLeft = new Bitmap(1, 50);
                Bitmap bottomRight = new Bitmap(1, 50);
                if (term1Bit.Tag is Bitmap[])
                {
                    topLeft = (Bitmap)((Array)term1Bit.Tag).GetValue(0);
                    bottomLeft = (Bitmap)((Array)term1Bit.Tag).GetValue(1);
                }
                else
                {
                    topLeft = term1Bit;
                    topJoinStr = "·";
                }
                if (term2Bit.Tag is Bitmap[])
                {
                    if (op == MathOperations.divide)
                    {
                        bottomRight = (Bitmap)((Array)term1Bit.Tag).GetValue(0);
                        topRight = (Bitmap)((Array)term1Bit.Tag).GetValue(1);
                    }
                    else
                    {
                        topRight = (Bitmap)((Array)term1Bit.Tag).GetValue(0);
                        bottomRight = (Bitmap)((Array)term1Bit.Tag).GetValue(1);
                    }
                }
                else
                {
                    if (op == MathOperations.divide)
                    {
                        bottomRight = term2Bit;
                        bottomJoinStr = "·";
                    }
                    else
                    {
                        topRight = term2Bit;
                        topJoinStr = "·";
                    }
                }
                if (term1Bit.Tag is Bitmap[] && term2Bit.Tag is Bitmap[])
                {
                    topJoinStr = "·";
                    bottomJoinStr = "·";
                }
                Bitmap top = new Bitmap(topLeft.Width + measureString(font, topJoinStr) + topRight.Width, 50);
                Graphics graphic = GetGraphics(top);
                graphic.DrawImageUnscaled(topLeft, 0, 0);
                graphic.DrawString(topJoinStr, font, brush, topLeft.Width, 0);
                graphic.DrawImageUnscaled(topRight, topLeft.Width + measureString(font, topJoinStr), 0);
                Bitmap bottom = new Bitmap(bottomLeft.Width + measureString(font, bottomJoinStr) + bottomRight.Width, 50);
                graphic = GetGraphics(bottom);
                graphic.DrawImageUnscaled(bottomLeft, 0, 0);
                graphic.DrawString(bottomJoinStr, font, brush, bottomLeft.Width, 0);
                graphic.DrawImageUnscaled(bottomRight, bottomLeft.Width + measureString(font, bottomJoinStr), 0);
                int retLength = (top.Width > bottom.Width ? top.Width / 2 : bottom.Width / 2);
                Bitmap ret = new Bitmap(retLength, 50);
                graphic = GetGraphics(ret);
                graphic.DrawImage(top, (ret.Width - (top.Width / 2)) / 2, 0, top.Width / 2, 25);
                graphic.DrawImage(bottom, (ret.Width - (bottom.Width / 2)) / 2, 25, bottom.Width / 2, 25);
                graphic.DrawLine(new Pen(brush, 2), 0, 25, ret.Width, 25);
                ret.Tag = new Bitmap[2] { top, bottom };
                return ret;
            }
            else if (op == MathOperations.log)
            {
                if (!(term2 is singleValue) && !(term2 is variable))
                {
                    term2Bit = encloseInBrackets(term2Bit, font, brush);
                }
                string funString = "log";
                bool is_natual_log = term1.ToString() == "e";
                if (is_natual_log)
                { 
                    funString = "ln";
                    term1Bit = new Bitmap(1, 1);
                }
                int funcLength = measureString(font, funString);
                Bitmap ret = new Bitmap(funcLength + (term1Bit.Width / 2) + term2Bit.Width, 50);
                Graphics graphic = GetGraphics(ret);
                graphic.DrawString(funString, font, brush, new RectangleF(0, 0, funcLength + 1, 50));
                graphic.DrawImage(term1Bit, funcLength, term1Bit.Height / 2, term1Bit.Width / 2, term1Bit.Height / 2);
                graphic.DrawImage(term2Bit, funcLength + (term1Bit.Width / 2), 0);
                return ret;
            }
            else if (op == MathOperations.divide)
            {
                int retLength = (term1Bit.Width > term2Bit.Width ? term1Bit.Width / 2 : term2Bit.Width / 2);
                Bitmap ret = new Bitmap(retLength, 50);
                Graphics graphic = GetGraphics(ret);
                graphic.DrawImage(term1Bit, (ret.Width - (term1Bit.Width / 2)) / 2, 0, term1Bit.Width / 2, 25);
                graphic.DrawImage(term2Bit, (ret.Width - (term2Bit.Width / 2)) / 2, 25, term2Bit.Width / 2, 25);
                graphic.DrawLine(new Pen(brush, 2), 0, 25, ret.Width, 25);
                ret.Tag = new Bitmap[2] { term1Bit, term2Bit };
                return ret;
            }
            else
            {
                string opString;
                bool NoMultiplication = (term2 is variable || term2 is exponent);
                if (term1.getPriority() < this.getPriority()) { term1Bit = encloseInBrackets(term1Bit, font, brush); }
                if (term2.getPriority() < this.getPriority()) { term2Bit = encloseInBrackets(term2Bit, font, brush); NoMultiplication = true; }
                switch (op)
                {
                    case MathOperations.add:
                        opString = "+";
                        break;
                    case MathOperations.substract:
                        opString = "-";
                        break;
                    case MathOperations.multiply:
                        opString = "·";
                        if (NoMultiplication)
                        {
                            if (term1.ToString() == "-1")
                            {
                                term1Bit = new Bitmap(1, 50);
                                opString = "-";
                            }
                            else
                            {
                                opString = "";
                            }
                        }
                        break;
                    case MathOperations.divide:
                        opString = "÷";
                        break;
                    default:
                        throw new Exception("Unknown mathematical operation");
                }
                int opLength = measureString(font, opString);
                Bitmap ret = new Bitmap(term1Bit.Width + term2Bit.Width + opLength, 50);
                Graphics graphic = GetGraphics(ret);
                graphic.DrawImageUnscaled(term1Bit, 0, 0);
                graphic.DrawString(opString, font, brush, new RectangleF(term1Bit.Width, 0, opLength, 50));
                graphic.DrawImageUnscaled(term2Bit, ret.Width - term2Bit.Width, 0);
                return ret;
            }
        }
        public override string ToString()
        {
            switch (op)
            {
                case MathOperations.add:
                    return "(" + term1.ToString() + "+" + term2.ToString() + ")";
                case MathOperations.substract:
                    return "(" + term1.ToString() + "-" + term2.ToString() + ")";
                case MathOperations.multiply:
                    return "(" + term1.ToString() + "*" + term2.ToString() + ")";
                case MathOperations.divide:
                    return "(" + term1.ToString() + "/" + term2.ToString() + ")";
                case MathOperations.log:
                    return "log(" + term1.ToString() + "," + term2.ToString() + ")";
                default:
                    throw new Exception("Unknown mathematical operation");
            }
        }
        term term1;
        term term2;
        MathOperations op;
    }

    public class exponent : term
    {
        public exponent(term t1, term t2)
        {
            term1 = t1;
            term2 = t2;
        }
        public override double getValue(double x, double y)
        {
            double n = term1.getValue(x, y);
            double i = term2.getValue(x, y);
            if (n < 0 && i % 1 != 0)
            {
                double nearest_integer_of_1_over_i = Math.Round(1 / i);
                if (nearest_integer_of_1_over_i % 2 == 1 && i == 1 / nearest_integer_of_1_over_i)
                {
                    return -Math.Pow(-n, i); //if i is the closest double to a reciprocal of an odd numeber, odd root is assumed
                }
                else
                {
                    return double.NaN;
                }
            }
            else
            {
            return Math.Pow(n, i);
            }
        }
        public override term optimize()
        {
            if (this.isConstant())
            {
                return new singleValue(this.getValue(1, 1));
            }
            else
            {
                return new exponent(term1.optimize(), term2.optimize()); ;
            }
        }
        public override bool isConstant()
        {
            return term1.isConstant() && term2.isConstant();
        }
        public override int getPriority()
        {
            return 5;
        }
        public override Bitmap ToBitmap(Font font, Brush brush)
        {
            Bitmap term1Bit = term1.ToBitmap(font, brush);
            if (term1.getPriority() <= this.getPriority()) { term1Bit = encloseInBrackets(term1Bit, font, brush); }
            Bitmap term2Bit = term2.ToBitmap(font, brush);
            Bitmap ret = new Bitmap(term1Bit.Width + (term2Bit.Width / 2), 50);
            Graphics graphic = GetGraphics(ret);
            graphic.DrawImageUnscaled(term1Bit, 0, 0);
            graphic.DrawImage(term2Bit, term1Bit.Width, 0, term2Bit.Width / 2, term2Bit.Height / 2);
            return ret;
        }
        public override string ToString()
        {
            return "(" + term1.ToString() + "^" + term2.ToString() + ")";
        }
        term term1;
        term term2;
    }

    public class constant : term
    {
        public constant(MathConstants c)
        {
            switch (c)
            {
                case MathConstants.e:
                    val = Math.E;
                    symbol = "e";
                    break;
                case MathConstants.pi:
                    val = Math.PI;
                    symbol = "π";
                    break;
                default:
                    throw new Exception("Unknown mathematical constant");
            }
        }
        public override double getValue(double x, double y)
        {
            return val;
        }
        public override bool isConstant()
        {
            return true;
        }
        public override int getPriority()
        {
            return 6;
        }
        public override term optimize()
        {
            return new singleValue(val);
        }
        public override Bitmap ToBitmap(Font font, Brush brush)
        {
            Bitmap ret = new Bitmap(measureString(font, symbol) - 10, 50);
            Graphics graphic = GetGraphics(ret);
            graphic.DrawString(symbol, font, brush, new PointF(-5, 0));
            return ret;
        }
        public override string ToString()
        {
            return symbol;
        }
        double val;
        string symbol;
    }
}
