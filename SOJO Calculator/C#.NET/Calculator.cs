using System;
using System.Diagnostics;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace SOJO_Calculator
{
    public class Calculator
    {

        public const string DOT_SIGN = ",";
        public const float BASE_FONT_SIZE = 24f;

        // ========= OPERATORS =========
        public enum Operators
        {
            ADD,
            SUBTRACT,
            MULTIPLY,
            DIVIDE,
            MODULO,
            PERCENT,
            SQUARE,
            CUBE,
            PI,
            POWER,
            SQUARE_ROOT,
            CUBE_ROOT,
            RECIPROCAL,
            RND,
            EQUAL,

            NONE
        }

        // ========= GET OPERATOR SIGN =========
        public static object getOperatorSign(Operators op)
        {
            string sign = "";

            switch (op)
            {
                case Operators.ADD:
                    {
                        sign = "+";
                        break;
                    }
                case Operators.SUBTRACT:
                    {
                        sign = "−";
                        break;
                    }
                case Operators.MULTIPLY:
                    {
                        sign = "x";
                        break;
                    }
                case Operators.DIVIDE:
                    {
                        sign = "÷";
                        break;
                    }
                case Operators.MODULO:
                    {
                        sign = "≡";
                        break;
                    }
                case Operators.PERCENT:
                    {
                        sign = "%";
                        break;
                    }
                case Operators.SQUARE:
                    {
                        sign = "x²";
                        break;
                    }
                case Operators.CUBE:
                    {
                        sign = "x³";
                        break;
                    }
                case Operators.PI:
                    {
                        sign = "π";
                        break;
                    }
                case Operators.POWER:
                    {
                        sign = "^";
                        break;
                    }
                case Operators.SQUARE_ROOT:
                    {
                        sign = "√";
                        break;
                    }
                case Operators.CUBE_ROOT:
                    {
                        sign = "∛";
                        break;
                    }
                case Operators.RECIPROCAL:
                    {
                        sign = "1/x";
                        break;
                    }
                case Operators.RND:
                    {
                        sign = "rnd";
                        break;
                    }
                case Operators.EQUAL:
                    {
                        sign = "=";
                        break;
                    }
            }

            return sign;
        }


        // ========= GET OPERATOR =========
        public static object getOperator(string key)
        {
            Operators op;

            switch (key ?? "")
            {
                case "+":
                    {
                        op = Operators.ADD;
                        break;
                    }
                case "−":
                case "-":
                    {
                        op = Operators.SUBTRACT;
                        break;
                    }
                case "x":
                case "×":
                    {
                        op = Operators.MULTIPLY;
                        break;
                    }
                case "÷":
                case "/":
                    {
                        op = Operators.DIVIDE;
                        break;
                    }
                case "mod":
                case "≡": // , "%"
                    {
                        op = Operators.MODULO;
                        break;
                    }
                case "%":
                    {
                        op = Operators.PERCENT;
                        break;
                    }
                case "x²":
                    {
                        op = Operators.SQUARE;
                        break;
                    }
                case "x³":
                    {
                        op = Operators.CUBE;
                        break;
                    }
                case "π":
                    {
                        op = Operators.PI;
                        break;
                    }
                case "^":
                    {
                        op = Operators.POWER;
                        break;
                    }
                case "√":
                    {
                        op = Operators.SQUARE_ROOT;
                        break;
                    }
                case "∛":
                    {
                        op = Operators.CUBE_ROOT;
                        break;
                    }
                case "1/x":
                    {
                        op = Operators.RECIPROCAL;
                        break;
                    }
                case "rnd":
                    {
                        op = Operators.RND;
                        break;
                    }
                case "=":
                    {
                        op = Operators.EQUAL;
                        break;
                    }

                default:
                    {
                        op = Operators.NONE;
                        break;
                    }
            }

            return op;
        }

        // ========= CALCULATION OF CUBED ROOT =========
        public static double CubedRoot(double dNum)
        {
            return (double)(decimal)Math.Pow(dNum, 1d / 3d);
        }


        // ========= CALCULATE RESULT =========
        public static double Calculate(double firstValue, double secondValue, Operators operation)
        {
            double result = 0d;

            switch (operation)
            {
                case Operators.ADD:
                    {
                        result = firstValue + secondValue;
                        break;
                    }
                case Operators.SUBTRACT:
                    {
                        result = firstValue - secondValue;
                        break;
                    }
                case Operators.MULTIPLY:
                    {
                        result = firstValue * secondValue;
                        break;
                    }
                case Operators.DIVIDE:
                    {
                        result = firstValue / secondValue;
                        break;
                    }
                case Operators.MODULO:
                    {
                        result = firstValue % secondValue;
                        break;
                    }
                case Operators.PERCENT:
                    {
                        result = firstValue / 100d;
                        break;
                    }
                case Operators.SQUARE:
                    {
                        result = firstValue * firstValue;
                        break;
                    }
                case Operators.CUBE:
                    {
                        result = firstValue * firstValue * firstValue;
                        break;
                    }
                case Operators.PI:
                    {
                        result = Math.PI;
                        break;
                    }
                case Operators.POWER:
                    {
                        result = Math.Pow(firstValue, secondValue);
                        break;
                    }
                case Operators.SQUARE_ROOT:
                    {
                        result = Math.Sqrt(firstValue);
                        break;
                    }
                case Operators.CUBE_ROOT:
                    {
                        result = CubedRoot(firstValue);
                        break;
                    }
                case Operators.RECIPROCAL:
                    {
                        result = 1d / firstValue;
                        break;
                    }

                default:
                    {
                        Debug.Print("Case Else");
                        break;
                    }
            }

            return result;
        }


        // ========= FORMAT VALUE =========
        public static string FormatValue(string input, bool forceFormat = false)
        {
            if (input.Length > 1 & input.Substring(0, 1) == "0" & input.Contains(DOT_SIGN) == false)
            {
                return input.Substring(1);
            }
            else if (input.Contains(DOT_SIGN))
            {
                if (forceFormat)
                {
                    return Strings.Format(Conversions.ToDouble(input), "##,##0.#####################");
                }
                else
                {
                    return input;
                }
            }
            else
            {
                return Strings.Format(Conversions.ToDouble(input), "##,##0.#####################");
            }
        }

    }
}