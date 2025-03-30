using System.Text;

namespace Evaluator.Logic;

public class FunctionEvaluator
{
    public static double Evaluate(string infix)
    {
        var postfix = ToPostfix(infix);
        return Calculate(postfix);
    }

    private static double Calculate(string postfix)
    {
        var stack = new Stack<double>();
        StringBuilder numberBuffer = new StringBuilder();

        foreach (char item in postfix)
        {
            if (item == ' ') 
            {
                if (numberBuffer.Length > 0)
                {
                    stack.Push(double.Parse(numberBuffer.ToString()));
                    numberBuffer.Clear();
                }
                continue;
            }

            if (IsOperator(item))
            {
                // Si hay un número en el buffer, procesarlo primero
                if (numberBuffer.Length > 0)
                {
                    stack.Push(double.Parse(numberBuffer.ToString()));
                    numberBuffer.Clear();
                }

                var operator2 = stack.Pop();
                var operator1 = stack.Pop();
                stack.Push(Result(operator1, item, operator2));
            }
            else
            {
                // Acumular dígitos
                numberBuffer.Append(item);
            }
        }

        // Procesar cualquier numero restante en el buffer
        if (numberBuffer.Length > 0)
        {
            stack.Push(double.Parse(numberBuffer.ToString()));
        }

        return stack.Pop();
    }

    private static double Result(double operator1, char item, double operator2)
    {
        return item switch
        {
            '+' => operator1 + operator2,
            '-' => operator1 - operator2,
            '*' => operator1 * operator2,
            '/' => operator1 / operator2,
            '^' => Math.Pow(operator1, operator2),
            _ => throw new Exception("Invalid expression"),
        };
    }

    private static string ToPostfix(string infix)
    {
        var stack = new Stack<char>();
        var postfix = new StringBuilder();
        StringBuilder numberBuffer = new StringBuilder();

        foreach (var item in infix)
        {
            if (IsOperator(item))
            {
                // Si hay un número en el buffer, agregarlo a postfix primero
                if (numberBuffer.Length > 0)
                {
                    postfix.Append(numberBuffer.ToString()).Append(' ');
                    numberBuffer.Clear();
                }

                if (stack.Count == 0)
                {
                    stack.Push(item);
                }
                else
                {
                    if (item == ')')
                    {
                        do
                        {
                            postfix.Append(stack.Pop()).Append(' ');
                        } while (stack.Peek() != '(');
                        stack.Pop(); // Quitar el '(' de la pila
                    }
                    else
                    {
                        if (PriorityExpression(item) > PriorityStack(stack.Peek()))
                        {
                            stack.Push(item);
                        }
                        else
                        {
                            while (stack.Count > 0 && PriorityExpression(item) <= PriorityStack(stack.Peek()))
                            {
                                postfix.Append(stack.Pop()).Append(' ');
                            }
                            stack.Push(item);
                        }
                    }
                }
            }
            else if (char.IsDigit(item) || item == '.')
            {
                // Acumular dígitos y punto decimal
                numberBuffer.Append(item);
            }
            else if (item == ' ')
            {
                // Ignorar espacios en la entrada
                continue;
            }
            else
            {
                throw new Exception($"Carácter no válido: {item}");
            }
        }

        // Agregar cualquier número restante en el buffer
        if (numberBuffer.Length > 0)
        {
            postfix.Append(numberBuffer.ToString()).Append(' ');
        }

        // Vaciar la pila
        while (stack.Count > 0)
        {
            postfix.Append(stack.Pop()).Append(' ');
        }

        return postfix.ToString().Trim();
    }

    private static int PriorityStack(char item)
    {
        return item switch
        {
            '^' => 3,
            '*' => 2,
            '/' => 2,
            '+' => 1,
            '-' => 1,
            '(' => 0,
            _ => throw new Exception("Invalid expression."),
        };
    }

    private static int PriorityExpression(char item)
    {
        return item switch
        {
            '^' => 4,
            '*' => 2,
            '/' => 2,
            '+' => 1,
            '-' => 1,
            '(' => 5,
            _ => throw new Exception("Invalid expression."),
        };
    }

    private static bool IsOperator(char item) => "()^*/+-".IndexOf(item) >= 0;
}