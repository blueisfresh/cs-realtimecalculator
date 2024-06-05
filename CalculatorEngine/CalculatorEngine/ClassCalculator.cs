namespace CalculatorEngine
{
    public class ClassCalculator
    {
        // CHANGES
        // EvaluateStack: Old Version Operands were popped without considering order, which is incorrect for subtraction and divisions (a - b correct; b - a incorrect)
        // Old Version called EvaluateStack as long as there were two operands, causing unnecessary evaluations
        // Simplified logic by recognizing current operator precedence

        // Attributes
        string inputString = "0"; // input number, as a string
        public bool isNumber = true; // input a number or Operator?
        public decimal buffer = 0; // Subtotal

        // Stacks
        public Stack<decimal> operandsStack = new Stack<decimal>();
        public Stack<char> operatorsStack = new Stack<char>();

        // History
        public List<string> history = new List<string>();

        // Methods
        public ClassCalculator()
        {
            operandsStack.Push(0);
            operatorsStack.Push('+');
        }

        // Returns inputString else top value of operandsStack
        public string GetInputString()
        {
            if (isNumber)
                return inputString;
            return operandsStack.Peek().ToString();
        }

        // inputString Builder
        public void SetNumber(string number)
        {
            if (number.Length > 1)
            {
                inputString = number;
            }
            else
            {
                if (inputString == "0" && number == "0")
                    return;
                if (inputString == "0")
                    inputString = "";
                inputString += number;
            }
        }

        // isNumber to true, indicates that number is being entered
        public void SetInput()
        {
            if (!isNumber)
                isNumber = true;
        }

        public void SetComma()
        {
            // , and . -> comma, but no double commas
            if (!inputString.Contains(','))
                inputString += ",";
        }

        public void EvaluateStack()
        {
            // if there are more than 1 operand on the stack perform calculation 
            // using the top operator from operatorStack
            if (operandsStack.Count >= 2)
            {
                char oldOp = operatorsStack.Pop();
                decimal operand2 = operandsStack.Pop();
                decimal operand1 = operandsStack.Pop();
                decimal result = 0;

                switch (oldOp)
                {
                    case '+':
                        result = operand1 + operand2;
                        break;
                    case '-':
                        result = operand1 - operand2;
                        break;
                    case '*':
                        result = operand1 * operand2;
                        break;
                    case '/':
                        if (operand2 == 0)
                        {
                            throw new DivideByZeroException("Cannot divide by zero.");
                        }
                        result = operand1 / operand2;
                        break;
                    default:
                        throw new InvalidOperationException($"Invalid operator: {oldOp}");
                }

                buffer = result;
                operandsStack.Push(buffer); // push Intermediate result to stack
                history.Add($"{operand1} {oldOp} {operand2} = {result}"); // record history

                // If Values are still in the operand Stack than perform the calculation again 
                if (operatorsStack.Count > 0 && operandsStack.Count >= 2)
                {
                    EvaluateStack();
                }
            }
        }

        public void AddOperator(char op)
        {
            isNumber = false; // Operator will be entered

            // Convert current input string to decimal & pushes it to the stack
            decimal lastInputNumber = Convert.ToDecimal(inputString);
            operandsStack.Push(lastInputNumber);

            // Current Operator is point and dash -> keep pushing to stack
            if ((op == '*' || op == '/') && (operatorsStack.Peek() == '+' || operatorsStack.Peek() == '-'))
            {
                operatorsStack.Push(op);
            }
            else // Let all the values in the stack be evaluated
            {
                EvaluateStack();
                operatorsStack.Push(op);
            }

            history.Add($"Operator: {op}"); // record operator

            // Reset current input
            inputString = "0";
        }

        // Output History 
        public List<string> GetHistory()
        {
            return history;
        }

        // Trigonometry Methods
        public string CalculateSine(string input)
        {
            decimal lastInputNumber = Convert.ToDecimal(input);
            double radians = (double)lastInputNumber * (Math.PI / 180);
            buffer = (decimal)Math.Sin(radians);
            inputString = buffer.ToString();
            return inputString;
        }

        public string CalculateCosine(string input)
        {
            decimal lastInputNumber = Convert.ToDecimal(input);
            double radians = (double)lastInputNumber * (Math.PI / 180);
            buffer = (decimal)Math.Cos(radians);
            inputString = buffer.ToString();
            return inputString;
        }

        public string CalculateTangent(string input)
        {
            decimal lastInputNumber = Convert.ToDecimal(input);
            double radians = (double)lastInputNumber * (Math.PI / 180);
            if ((radians / Math.PI) % 0.5 == 0)
            {
                throw new InvalidOperationException("Tangent is undefined at this angle.");
            }
            buffer = (decimal)Math.Tan(radians);
            inputString = buffer.ToString();
            return inputString;
        }
    }
}
