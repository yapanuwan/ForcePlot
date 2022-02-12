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

namespace ForcePlot
{
    public static class MathConverter
    {
        /// <summary>
        /// Parses a string to a 'term' type
        /// </summary>
        /// <param name="expression">A mathematical expression in proper notation with spacing(eg. x ^ 2 is OK, x^2 is bad)</param>
        /// <returns></returns>
        public static term FromString(string expression)
        {
            return toExp(optimizeExp(expression));
        }

        /// <summary>
        /// Rewrites an equation in proper notation with spacing
        /// </summary>
        /// <param name="eq">A mathematical equation (eg. y=x^2)</param>
        /// <returns></returns>
        public static string FormatEquation(string eq)
        {
            bool absStarted = false;
            for (int i = 0; i < eq.Length; i++)
            {
                if (eq[i] == '|')
                {
                    if (!absStarted)
                    {
                        eq = eq.Remove(i, 1);
                        eq = eq.Insert(i, "abs(");
                    }
                    absStarted = !absStarted;
                }
            }
            eq = eq.Replace('|', ')');

            foreach (string part in eq.Split('='))
            {
                int brackets = 0;
                foreach (char c in part.ToCharArray())
                {
                    if (c == '(') { brackets++; }
                    else if (c == ')') { brackets--; }
                }
                if (!(brackets == 0)) { throw new Exception("Brackets?"); }
            }

            eq = eq.Replace("⁰", "^0");
            eq = eq.Replace("¹", "^1");
            eq = eq.Replace("²", "^2");
            eq = eq.Replace("³", "^3");
            eq = eq.Replace("⁴", "^4");
            eq = eq.Replace("⁵", "^5");
            eq = eq.Replace("⁶", "^6");
            eq = eq.Replace("⁷", "^7");
            eq = eq.Replace("⁸", "^8");
            eq = eq.Replace("⁹", "^9");

            eq = eq.ToLower();
            eq = " " + eq.Replace(" ", "") + " ";

            eq = eq.Replace("[", "(");
            eq = eq.Replace("]", ")");

            eq = eq.Replace(")(", ")*(");

            int bracketLevel;
            int indOf = eq.IndexOf("sqrt(");
            while (indOf > 0)
            {
                bracketLevel = 0;
                for (int i = indOf + 4; i < eq.Length; i++)
                {
                    if (eq[i] == '(') { bracketLevel++; }
                    else if (eq[i] == ')')
                    {
                        bracketLevel--;
                        if (bracketLevel < 1)
                        {
                            eq = eq.Insert(i+1, "^0.5");
                            break;
                        }
                    }
                }
                indOf = eq.IndexOf("sqrt(", indOf + 4);
            }
            eq = eq.Replace("sqrt(", "(");

            indOf = eq.IndexOf("log(");
            while (indOf > 0)
            {
                bracketLevel = 0;
                bool commaMissing = false;
                for (int i = indOf + 3; i < eq.Length; i++)
                {
                    if (eq[i] == '(') { bracketLevel++; }
                    else if (eq[i] == ')')
                    {
                        bracketLevel--;
                        if (bracketLevel < 1)
                        {
                            commaMissing = true;
                            break;
                        }
                    }
                    else if (eq[i] == ',') { break; }
                }

                if (commaMissing)
                {
                    eq = eq.Insert(indOf + 4, "10,");
                }
                indOf = eq.IndexOf("log(", indOf + 3);
            }

            eq = eq.Replace("pi", "p'i");

            eq = eq.Replace("log(", "l'o'g[");
            eq = eq.Replace("lg(", "l'g[");
            eq = eq.Replace("ln(", "l'n[");
            eq = eq.Replace("asin(", "a's'i'n[");
            eq = eq.Replace("acos(", "a'c'o's[");
            eq = eq.Replace("atan(", "a't'a'n[");
            eq = eq.Replace("sin(", "s'i'n[");
            eq = eq.Replace("cos(", "c'o's[");
            eq = eq.Replace("tan(", "t'a'n[");
            eq = eq.Replace("cosec(", "c'o's'e'c[");
            eq = eq.Replace("sec(", "s'e'c[");
            eq = eq.Replace("cot(", "c'o't[");
            eq = eq.Replace("abs(", "a'b's[");

            eq = eq.Replace("+-", "-");
            eq = eq.Replace("-+", "-");
            eq = eq.Replace("--", "+");

            for (int i = 1; i < eq.Length; i++)
            {
                if (eq[i] == '(')
                {
                    if (char.IsLetter(eq[i - 1]) || char.IsDigit(eq[i - 1]))
                    {
                        eq = eq.Substring(0, i) + "*" + eq.Substring(i);
                    }
                } 
                if (eq[i] == ')')
                {
                    if (char.IsLetter(eq[i + 1]) || char.IsDigit(eq[i + 1]))
                    {
                        eq = eq.Substring(0, i + 1) + "*" + eq.Substring(i + 1);
                    }
                }
                else if (eq[i] == '-' || eq[i] == '+')
                {
                    if (char.IsLetter(eq[i - 1]) || char.IsDigit(eq[i - 1]) || eq[i - 1] == ')')
                    {
                        eq = eq.Substring(0, i) + " " + eq[i] + " " + eq.Substring(i + 1);
                    }
                }
                else if (char.IsLetter(eq[i]))
                {
                    if (char.IsLetter(eq[i - 1]) || char.IsDigit(eq[i - 1]))
                    {
                        eq = eq.Substring(0, i) + "*" + eq.Substring(i);
                    }
                }
            }

            eq = eq.Replace("=", " = ");
            eq = eq.Replace("^", " ^ ");
            eq = eq.Replace("*", " * ");
            eq = eq.Replace("/", " / ");
            eq = eq.Replace(",", ", ");

            eq = eq.Replace("'", "");
            eq = eq.Replace("[", "(");

            eq = eq.Replace("+(", "(");
            eq = eq.Replace("+x", "x");
            eq = eq.Replace("+y", "y");

            return eq.Trim();
        }

        private static string optimizeExp(string exp)
        {
            exp = exp.Trim();

            exp = exp.Replace(",", " ,");

            exp = exp.Replace("-x", "-(x)");
            exp = exp.Replace("-y", "-(y)");

            exp = exp.Replace("pi", "π");

            exp = exp.Replace("log(", "(");
            exp = exp.Replace("lg(", "(10 , ");
            exp = exp.Replace("ln(", "(e , ");

            exp = exp.Replace("asin(", "(7 | ");
            exp = exp.Replace("acos(", "(8 | ");
            exp = exp.Replace("atan(", "(9 | ");

            exp = exp.Replace("sin(", "(1 | ");
            exp = exp.Replace("cos(", "(2 | ");
            exp = exp.Replace("tan(", "(3 | ");

            exp = exp.Replace("cosec(", "(4 | ");
            exp = exp.Replace("sec(", "(5 | ");
            exp = exp.Replace("cot(", "(6 | ");

            exp = exp.Replace("abs(", "(10 | ");

            foreach (char c in exp.ToCharArray())
            {
                if (char.IsNumber(c)
                    || c == '.'
                    || c == '+'
                    || c == '-'
                    || c == '*'
                    || c == '/'
                    || c == '^'
                    || c == ','
                    || c == '|'
                    || c == '('
                    || c == ')'
                    || c == ' '
                    || c == 'x'
                    || c == 'y'
                    || c == 'e'
                    || c == 'π'
                    )
                { }
                else
                { throw new Exception("Invalid chars"); }
            }

            return exp;
        }

        private static term toExp(string expression)
        {
            object[] obj = breakexp(expression);

            if (obj.Length == 1)
            {
                double uselsessVariable = 0;
                string obstr = (string)obj[0];

                if (obstr.StartsWith("("))
                {
                    return toExp(obstr.Substring(1, obstr.Length - 2));
                }
                else if (obstr.StartsWith("-("))
                {
                    return new expression(new singleValue(-1), toExp(obstr.Substring(2, obstr.Length - 3)), MathOperations.multiply);
                }
                else if (obstr == "x" || obstr == "y")
                {
                    return new variable(obstr == "x");
                }
                else if (obstr == "e" || obstr == "π")
                {
                    switch(obstr)
                    {
                        case "e":
                            return new constant(MathConstants.e);
                        case "π":
                            return new constant(MathConstants.pi);
                    }
                }
                else if (double.TryParse(obstr, out uselsessVariable))
                {
                    return new singleValue(double.Parse(obstr));
                }
                else
                {
                    throw new Exception("Unknown term - " + obstr);
                }
            }

            if ((string)obj[1] == "|")
            {
                return new function(toExp(expression.Substring(((string)obj[0]).Length + 3)), MathFunctions.Zero + int.Parse((string)obj[0]));
            }
            int currPosition = 0;
            for (int i = 0; i < obj.Length; i++)
            {
                if ((string)obj[i] == ",")
                {
                    return new expression(toExp(expression.Substring(0, currPosition - 1)), toExp(expression.Substring(currPosition + 2)), MathOperations.log);
                }
                currPosition += ((string)obj[i]).Length + 1;
            }

            for (int i = 0; i < obj.Length; i++)
            {
                double uselsessVariable = 0;
                string obstr = (string)obj[i];
                if (obstr.StartsWith("("))
                {
                    obj[i] = toExp(obstr.Substring(1, obstr.Length - 2));
                }
                else if (obstr.StartsWith("-("))
                {
                    obj[i] = new expression(new singleValue(-1), toExp(obstr.Substring(2, obstr.Length - 3)), MathOperations.multiply);
                }
                else if (obstr == "x" || obstr == "y")
                {
                    obj[i] = new variable(obstr == "x");
                }
                else if (obstr == "e" || obstr == "π")
                {
                    switch (obstr)
                    {
                        case "e":
                            obj[i] = new constant(MathConstants.e);
                            break;
                        case "π":
                            obj[i] = new constant(MathConstants.pi);
                            break;
                    }
                }
                else if (double.TryParse(obstr, out uselsessVariable))
                {
                    obj[i] = new singleValue(double.Parse(obstr));
                }
                else if (obstr.Length == 1)
                {
                    switch (obstr)
                    {
                        case "+":
                            obj[i] = MathOperations.add;
                            break;

                        case "-":
                            obj[i] = MathOperations.substract;
                            break;

                        case "*":
                            obj[i] = MathOperations.multiply;
                            break;

                        case "/":
                            obj[i] = MathOperations.divide;
                            break;

                        case "^":
                            obj[i] = MathOperations.raise;
                            break;

                        default:
                            throw new Exception("Unknown operation - " + obstr);
                    }
                }
                else
                {
                    throw new Exception("Unknown term - " + obstr);
                }
            }

            return combine(obj);
        }

        private static term combine(object[] obj)
        {
            if (obj.Length == 1)
            {
                return (term)obj[0];
            }

            for (int i = 1; i < obj.Length; i++)
            {
                if (obj[i] is MathOperations)
                {
                    if ((MathOperations)obj[i] == MathOperations.raise)
                    {
                        obj = replaceWithResults(obj, i, new exponent((term)obj[i - 1], (term)obj[i + 1]));
                        return combine(obj);
                    }
                }
            }
            for (int i = 1; i < obj.Length; i++)
            {
                if (obj[i] is MathOperations)
                {
                    if ((MathOperations)obj[i] == MathOperations.multiply || (MathOperations)obj[i] == MathOperations.divide)
                    {
                        obj = replaceWithResults(obj, i, new expression((term)obj[i - 1], (term)obj[i + 1], (MathOperations)obj[i]));
                        return combine(obj);
                    }
                }
            }
            for (int i = 1; i < obj.Length; i++)
            {
                if (obj[i] is MathOperations)
                {
                    if ((MathOperations)obj[i] == MathOperations.add || (MathOperations)obj[i] == MathOperations.substract)
                    {
                        obj = replaceWithResults(obj, i, new expression((term)obj[i - 1], (term)obj[i + 1], (MathOperations)obj[i]));
                        return combine(obj);
                    }
                }
            }
            throw new Exception("No operations found");
        }

        private static object[] breakexp(string expression)
        {
            string[] exp = expression.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            object[] ret = new object[exp.Length];

            int bracketLevel = 0;
            int word = 0;
            foreach (string w in exp)
            {
                if (bracketLevel < 1)
                {
                    ret[word] = w;
                }
                else
                {
                    ret[word] += " " + w;
                }

                foreach (char c in w.ToCharArray())
                {
                    if (c == '(') { bracketLevel++; }
                    else if (c == ')') { bracketLevel--; }
                }

                if (bracketLevel < 1)
                {
                    word++;
                    bracketLevel = 0;
                }
            }

            Array.Resize<object>(ref ret, word);

            return ret;
        }

        private static object[] replaceWithResults(object[] words, int i, term replace)
        {
            object[] ret = new object[words.Length - 2];

            words[i - 1] = replace; words[i] = ""; words[i + 1] = "";
            for (int e = 0; e < i - 1; e++)
            {
                ret[e] = words[e];
            }
            ret[i - 1] = replace;
            for (int e = i; e < ret.Length; e++)
            {
                ret[e] = words[e + 2];
            }

            return ret;
        }
    }
}