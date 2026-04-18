using System.Diagnostics;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace SOJO_Calculator
{
    // ***************************************************
    // 
    // SOJO Calculator
    // 
    // ***************************************************

    public partial class MainForm : Form
    {
        // Status indicator if equal button or another calculation has presented a total result
        private bool hasResult;

        // Status indicator if value is stored in memory
        private bool hasMemoryRecall = false;

        // Variables holding current and previous operations
        private Calculator.Operators currentOperation = Calculator.Operators.NONE;
        private Calculator.Operators previousOperation = Calculator.Operators.NONE;

        // Variables holding the first and second values
        private double firstValue = 0d;
        private double secondValue = 0d;

        // Variable holding the memory value and status
        private double memoryValue = 0d;

        // Variable if Clear Entry (CE) has been pressed
        private bool clearEntry = false;

        // Max length of the display numbers
        private const int MAX_LENGTH = 17;

        public MainForm()
        {
            InitializeComponent();

            this.KeyPreview = true;
            KeyDown += new KeyEventHandler(MainForm_KeyDown);
            KeyPress += new KeyPressEventHandler(MainForm_KeyPress);
        }

        // ========= MAIN FORM - Control Added =========
        private void MainForm_ControlAdded(object sender, ControlEventArgs e) =>
            // Center application on the current screen
            StartPosition = FormStartPosition.CenterParent;


        // ========= MAIN FORM - Load =========
        private void MainForm_Load(object sender, EventArgs e)
        {
            ActiveControl = null;           

            // Change period symbol for current language
            btnDot.Text = Calculator.DOT_SIGN;
            lblMemoryStatus.Visible = false;

            // Select the result display as default
            lblCurrentResult.Select();
        }

        // ========= NUMBER BUTTONS =========
        private void NumberButton_Click(object sender, EventArgs e)        {

            // Get the current selected button
            Button btn = (Button)sender;
                        
            // Reset the screen if a result has been calculated
            if (hasResult | hasMemoryRecall)
            {
                lblCurrentResult.Text = "0";
                hasResult = false;
                hasMemoryRecall = false;

                if (currentOperation == Calculator.Operators.NONE & previousOperation == Calculator.Operators.NONE)
                {
                    lblPreviousResult.Text = "";
                }
                else
                {
                    lblPreviousResult.Text = Conversions.ToString(Operators.ConcatenateObject(Calculator.FormatValue(firstValue.ToString()) + " ", Calculator.getOperatorSign(currentOperation)));
                }
            }

            // Check if the current number is not longer than the MAX_LENGTH
            if (lblCurrentResult.Text.Length >= MAX_LENGTH)
            {
                return;
            }

            // Check if a period exist and if not check if its a empty string and if so add a zero before the period
            if ((btn.Text ?? "") == Calculator.DOT_SIGN)
            {
                if (lblCurrentResult.Text.Contains(Calculator.DOT_SIGN))
                {
                    return;
                }
                if (string.IsNullOrEmpty(lblCurrentResult.Text))
                {
                    lblCurrentResult.Text = "0";
                }
            }


            // Add key to the value
            lblCurrentResult.Text += btn.Text;

            // Format the current result 
            lblCurrentResult.Text = Calculator.FormatValue(lblCurrentResult.Text);

            // Adjust the font and decrease it if needed
            AdjustLabelFont(lblCurrentResult);

            // Set the status of Clear Entry to false
            clearEntry = false;

            // Select the result display as default
            lblCurrentResult.Select();
        }


        // ========= OPERATOR BUTTONS =========
        private void OperatorButton_Click(object sender, EventArgs e)

        {

            // Get the current selected button
            Button btn = (Button)sender;

            // Initialize the result variable and set it to zero
            string result = "0";

            // Set the status of Clear Entry to false
            hasResult = false;
            hasMemoryRecall = false;

            // Set previous operation
            previousOperation = currentOperation;

            // Set current operation
            currentOperation = (Calculator.Operators)Conversions.ToInteger(Calculator.getOperator(btn.Text));

            // Debug.Print("Operator:" & firstNumber & ", " & secondNumber & "->" & Calculator.getOperatorSign(operation) & "," & Calculator.getOperatorSign(newOperation))

            // Save current values for later use
            double oldFirstValue = firstValue;
            double oldSecondValue = secondValue;

            // Set the firstValue if it's zero or else set the second value
            if (firstValue == 0d)
            {
                double.TryParse(lblCurrentResult.Text, out firstValue);
            }
            else
            {
                double.TryParse(lblCurrentResult.Text, out secondValue);
            }

            // If both no operator and second value or else both first value and second value are present
            if (currentOperation != Calculator.Operators.NONE && !string.IsNullOrEmpty(lblCurrentResult.Text) && Conversions.ToBoolean(secondValue))
            {
                // Calculate and display the result

                // Percent is current operation
                if (currentOperation == Calculator.Operators.PERCENT)
                {
                    secondValue = Conversions.ToDouble(Calculator.Calculate(secondValue, 0d, currentOperation).ToString());
                    result = Calculator.Calculate(firstValue, secondValue, previousOperation).ToString();

                    // Use previous operator
                    switch (previousOperation)
                    {
                        // === ADD BUTTON ===
                        case Calculator.Operators.ADD:
                            {
                                result = Calculator.Calculate(firstValue, 1d + secondValue, Calculator.Operators.MULTIPLY).ToString();
                                break;
                            }

                        // === SUBRACT BUTTON ===
                        case Calculator.Operators.SUBTRACT:
                            {
                                result = Calculator.Calculate(firstValue, 1d - secondValue, Calculator.Operators.MULTIPLY).ToString();
                                break;
                            }

                        // === MULTIPLY BUTTON ===
                        case Calculator.Operators.MULTIPLY:
                            {
                                result = Calculator.Calculate(firstValue, secondValue, Calculator.Operators.MULTIPLY).ToString();
                                break;
                            }

                        // === DIVIDE BUTTON ===
                        case Calculator.Operators.DIVIDE:
                            {
                                result = Calculator.Calculate(firstValue, secondValue, Calculator.Operators.DIVIDE).ToString();
                                break;
                            }

                        default:
                            {
                                Debug.Print(Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("Case Else: firstValue=" + firstValue + ", secondValue=" + secondValue + " result=" + result + ", currentOperation=", Calculator.getOperatorSign(currentOperation)), ", previousOperation="), Calculator.getOperatorSign(previousOperation))));
                                break;
                            }
                    }

                    // Display the result
                    displayResult(firstValue.ToString(), (secondValue * 100d).ToString(), previousOperation, Conversions.ToDouble(result), Conversions.ToString(Calculator.getOperatorSign(currentOperation)));

                    // FLag that a calculation has been done
                    hasResult = true;

                    // Reset values
                    resetValues();
                }

                // Percent is previous operation
                else if (previousOperation == Calculator.Operators.PERCENT)
                {
                    // Fix when calculating xx % xx - xx
                    if (currentOperation != Calculator.Operators.PERCENT)
                    {
                        // Set the second value             
                        double.TryParse(lblCurrentResult.Text, out secondValue);

                        // Calculate and view the result
                        result = Calculator.Calculate(firstValue, secondValue, Calculator.Operators.MULTIPLY).ToString();
                        displayResult((firstValue * 100d).ToString(), secondValue.ToString(), Calculator.Operators.MULTIPLY, Conversions.ToDouble(result), "", Conversions.ToString(Calculator.getOperatorSign(Calculator.Operators.PERCENT)));

                        // Mark that a calculation result has been made
                        hasResult = true;

                        // Reset variables
                        firstValue = Conversions.ToDouble(result);
                        secondValue = 0d;
                    }

                    // Other percent calculation when the percent sign shall be showed
                    else
                    {
                        // Calculate and view the result
                        result = Calculator.Calculate(firstValue, secondValue, Calculator.Operators.MULTIPLY).ToString();
                        displayResult((firstValue * 100d).ToString(), secondValue.ToString(), Calculator.Operators.MULTIPLY, Conversions.ToDouble(result), "", Conversions.ToString(Calculator.getOperatorSign(Calculator.Operators.PERCENT)));

                        // Mark that a calculation has been made
                        hasResult = true;

                        // Reset variables
                        resetValues();
                    }
                }
                else
                {
                    // Operations with Square, Square Root, Cube, Cube Root and Power, Reciprocal
                    switch (currentOperation)
                    {
                        // === SQUARE BUTTON ===
                        case Calculator.Operators.SQUARE:
                            {
                                // Calculate and view the result
                                result = Calculator.Calculate(firstValue, secondValue * secondValue, previousOperation).ToString();
                                displayResult(firstValue.ToString(), secondValue.ToString(), previousOperation, Conversions.ToDouble(result), "²", "");

                                // Flag that a calculation has been made
                                hasResult = true;

                                // Reset variables
                                resetValues();
                                break;
                            }

                        // === CUBE BUTTON ===
                        case Calculator.Operators.CUBE:
                            {
                                // Calculate and view the resut
                                result = Calculator.Calculate(firstValue, secondValue * secondValue * secondValue, previousOperation).ToString();
                                displayResult(firstValue.ToString(), secondValue.ToString(), previousOperation, Conversions.ToDouble(result), "³", "");

                                // Flag that a calculation has been made
                                hasResult = true;

                                // Reset variables
                                resetValues();
                                break;
                            }

                        // === POWER BUTTON ===
                        case Calculator.Operators.POWER:
                            {
                                // Show the value and the power sign in previous result
                                lblPreviousResult.Text = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Calculator.FormatValue(firstValue.ToString()) + " ", Calculator.getOperatorSign(previousOperation)), " "), secondValue), "^"));

                                // Set current result to zero
                                lblCurrentResult.Text = "0";

                                // Adjust the size of the labels depending on how many numbers is present
                                AdjustLabelFont(lblPreviousResult);
                                AdjustLabelFont(lblCurrentResult);

                                // Set focus to current result label
                                lblCurrentResult.Select();
                                break;
                            }

                        // === SQUARE ROOT BUTTON ===
                        case Calculator.Operators.SQUARE_ROOT:
                            {
                                // Calculate and view the result
                                result = Calculator.Calculate(firstValue, Math.Sqrt(secondValue), previousOperation).ToString();
                                lblPreviousResult.Text = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Calculator.FormatValue(firstValue.ToString()) + " ", Calculator.getOperatorSign(previousOperation)), " "), Calculator.getOperatorSign(currentOperation)), secondValue), " ="));
                                lblCurrentResult.Text = Calculator.FormatValue(result);

                                // Adjust the size of the labels depending on how many numbers is present
                                AdjustLabelFont(lblPreviousResult);
                                AdjustLabelFont(lblCurrentResult);

                                // Set focus to current resut label
                                lblCurrentResult.Select();

                                // Flag that a calculation has been made
                                hasResult = true;

                                // Reset variables
                                resetValues();
                                break;
                            }

                        // === CUBE ROOT BUTTON ===
                        case Calculator.Operators.CUBE_ROOT:
                            {
                                // Calculate and view the result
                                result = Calculator.Calculate(firstValue, Calculator.CubedRoot(secondValue), previousOperation).ToString();
                                lblPreviousResult.Text = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Calculator.FormatValue(firstValue.ToString()) + " ", Calculator.getOperatorSign(previousOperation)), " "), Calculator.getOperatorSign(currentOperation)), secondValue), " ="));
                                lblCurrentResult.Text = Calculator.FormatValue(result);

                                // Adjust the size of the labels depending on how many numbers is present
                                AdjustLabelFont(lblPreviousResult);
                                AdjustLabelFont(lblCurrentResult);

                                // Set focus to current resut label
                                lblCurrentResult.Select();

                                // Flag that a calculation has been made
                                hasResult = true;

                                // Reset variables
                                resetValues();
                                break;
                            }

                        // === RECIPROCAL BUTTON ===
                        case Calculator.Operators.RECIPROCAL:
                            {
                                // Calculate and view the result
                                result = Calculator.Calculate(firstValue, 1d / secondValue, previousOperation).ToString();
                                displayResult(firstValue.ToString(), (1d / secondValue).ToString(), previousOperation, Conversions.ToDouble(result));

                                // Flag that a calculation has been made
                                hasResult = true;

                                // Reset variables
                                resetValues();
                                break;
                            }

                        default:
                            {
                                // Power Button is previous operation
                                if (previousOperation == Calculator.Operators.POWER)
                                {
                                    // Only the first power number is present otherwise add the saved old second value for the second power number
                                    if (oldSecondValue == 0d)
                                    {
                                        // Calculate the result
                                        result = Calculator.Calculate(firstValue, secondValue, previousOperation).ToString();
                                    }
                                    else
                                    {
                                        // Calculate the total calculation
                                        string powerValue = Calculator.Calculate(oldSecondValue, secondValue, previousOperation).ToString();
                                        result = Calculator.Calculate(firstValue, Conversions.ToDouble(powerValue), currentOperation).ToString();
                                    }
                                }
                                else
                                {
                                    // Calculate other calculations
                                    result = Calculator.Calculate(firstValue, secondValue, previousOperation).ToString();
                                }

                                // Display the result in previous result and set current result to zero
                                displayResult(result, "", currentOperation, Conversions.ToDouble("0"));

                                // Set the result to the first value and reset the second value
                                firstValue = Conversions.ToDouble(result);
                                secondValue = 0d;
                                break;
                            }
                    }
                }
            }
            // Power is previous operation
            else if (previousOperation == Calculator.Operators.POWER)
            {
                // Calculate and display the result
                result = Math.Pow(firstValue, secondValue).ToString();
                lblPreviousResult.Text = Conversions.ToString(Operators.ConcatenateObject(Calculator.FormatValue(result) + " ", Calculator.getOperatorSign(currentOperation)));

                // Adjust the font size depending on how many numbers
                AdjustLabelFont(lblPreviousResult);

                // Set focus to current result label
                lblCurrentResult.Select();
            }
            else
            {
                // Calculate the result
                result = Calculator.Calculate(firstValue, secondValue, currentOperation).ToString();

                // Display the result for current operation
                switch (currentOperation)
                {
                    // === PERCENT OPERATION ===
                    case Calculator.Operators.PERCENT:
                        {
                            // Set the result as first value
                            firstValue = Conversions.ToDouble(result);
                            // Display the result
                            displayResult(firstValue.ToString(), "", Calculator.Operators.NONE, 0d);
                            break;
                        }

                    // === SQUARE OPERATION ===
                    case Calculator.Operators.SQUARE:
                        {
                            // Calculate and show the result
                            result = Calculator.Calculate(firstValue, firstValue, Calculator.Operators.SQUARE).ToString();
                            displayResult(firstValue.ToString(), firstValue.ToString(), Calculator.Operators.NONE, Conversions.ToDouble(result), "", "²");

                            // Flag that a calculation has been made
                            hasResult = true;

                            // Reset variables
                            resetValues();
                            break;
                        }

                    // === CUBE OPERATION ===
                    case Calculator.Operators.CUBE:
                        {
                            // Calculate and show the result
                            result = Calculator.Calculate(firstValue, firstValue, Calculator.Operators.CUBE).ToString();
                            displayResult(firstValue.ToString(), firstValue.ToString(), Calculator.Operators.NONE, Conversions.ToDouble(result), "", "³");

                            // Flag that a calculation has been made
                            hasResult = true;

                            // Reset variables
                            resetValues();
                            break;
                        }

                    // === SQUARE ROOT OPERATION ===
                    case Calculator.Operators.SQUARE_ROOT:
                    case Calculator.Operators.CUBE_ROOT:
                        {
                            // Calculate the result
                            result = Calculator.Calculate(firstValue, firstValue, currentOperation).ToString();

                            // Display the result
                            lblPreviousResult.Text = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Calculator.getOperatorSign(currentOperation), Calculator.FormatValue(firstValue.ToString())), " ="));
                            lblCurrentResult.Text = Calculator.FormatValue(result);

                            // Adjust the font size depending on how many numbers
                            AdjustLabelFont(lblPreviousResult);
                            AdjustLabelFont(lblCurrentResult);

                            // Set focus to current result label
                            lblCurrentResult.Select();

                            // Flag that a calculation has been made
                            hasResult = true;

                            // Reset variables
                            resetValues();
                            break;
                        }

                    // === RECIPROCAL OPERATION ===
                    case Calculator.Operators.RECIPROCAL:
                        {
                            // Calculate and show the result
                            result = Calculator.Calculate(1d, firstValue, Calculator.Operators.DIVIDE).ToString();
                            displayResult(1.ToString(), firstValue.ToString(), Calculator.Operators.DIVIDE, Conversions.ToDouble(result));

                            // Flag that a calculation has been made
                            hasResult = true;

                            // Reset variables
                            resetValues();
                            break;
                        }

                    default:
                        {
                            displayResult(firstValue.ToString(), "", currentOperation, Conversions.ToDouble(result));
                            break;
                        }
                }
            }
        }


        // ========= MEMORY BUTTONS =========
        private void MemoryButton_Click(object sender, EventArgs e)
        {

            // Get the current selected button
            Button btn = (Button)sender;

            // Get the current value
            string currentValue = lblCurrentResult.Text;

            // Reset the memory recall status
            hasMemoryRecall = false;

            // Get selected memory button
            switch (btn.Text ?? "")
            {
                // === MEMORY CLEAR ===
                case "MC":
                    {
                        // Clear the memory and hide the memory label
                        memoryValue = 0d;
                        lblMemoryStatus.Visible = false;
                        break;
                    }

                // === MEMORY RECALL ===
                case "MR":
                    {
                        // Show the memory value
                        lblCurrentResult.Text = Calculator.FormatValue(memoryValue.ToString());
                        // Set memory recall status to true
                        hasMemoryRecall = true;

                        // Adjust the font size depending on how many numbers
                        AdjustLabelFont(lblCurrentResult);

                        // Set focus to current result label
                        lblCurrentResult.Select();
                        break;
                    }

                // === MEMORY STORE ===
                case "MS":
                    {
                        // Store the current value in memory and show the memory label
                        memoryValue = Conversions.ToDouble(currentValue);
                        lblMemoryStatus.Visible = true;
                        break;
                    }

                // === MEMORY ADD ===
                case "M+":
                    {
                        // Add the current value to the memory and show the memory label
                        memoryValue += Conversions.ToDouble(currentValue);
                        lblMemoryStatus.Visible = true;
                        break;
                    }

                // === MEMORY SUBTRACT
                case "M-":
                    {
                        // Subtract the current value to the memory and show the memory label
                        memoryValue -= Conversions.ToDouble(currentValue);
                        lblMemoryStatus.Visible = true;
                        break;
                    }
            }
        }


        // ========= PI BUTTON =========
        private void btnPi_Click(object sender, EventArgs e)
        {
            // Debug.Print("PI: firstValue=" & firstValue & ", secondValue=" & secondValue & ", currentOperation=" & Calculator.getOperatorSign(currentOperation) & ", previousOperation=" & Calculator.getOperatorSign(previousOperation))

            // Set the firstValue if it's zero or else set the second value to PI (3.14159...)
            if (firstValue == 0d)
            {
                if (hasResult == false)
                {
                    firstValue = Math.PI;
                    displayResult(firstValue.ToString(), 0.ToString(), currentOperation, firstValue, "", Conversions.ToString(Calculator.getOperatorSign(Calculator.Operators.PI)));
                    firstValue = 0d;

                    // Flag that a calculation has been made
                    hasResult = true;
                }
            }
            else
            {
                secondValue = Math.PI;
                lblCurrentResult.Text = Calculator.FormatValue(secondValue.ToString());
                AdjustLabelFont(lblCurrentResult);
                lblCurrentResult.Select();
            }
        }


        // ========= RND BUTTON =========
        private void btnRnd_Click(object sender, EventArgs e)
        {
            // Debug.Print("RND: firstValue=" & firstValue & ", secondValue=" & secondValue & ", currentOperation=" & Calculator.getOperatorSign(currentOperation) & ", previousOperation=" & Calculator.getOperatorSign(previousOperation))

            // Get a random integer between 1 and 9
            int rndValue = (int)Math.Round(Conversion.Int(9f * VBMath.Rnd() + 1f));

            // Set the firstValue if it's zero or else set the second value to the created random value
            if (firstValue == 0d)
            {
                firstValue = rndValue;

                // Display the random value
                lblPreviousResult.Text = Conversions.ToString(Operators.ConcatenateObject(Calculator.getOperatorSign(Calculator.Operators.RND), " ="));
                lblCurrentResult.Text = Calculator.FormatValue(firstValue.ToString());

                // Adjust the font size depending on the number size
                AdjustLabelFont(lblPreviousResult);
                AdjustLabelFont(lblCurrentResult);
                lblCurrentResult.Select();

                firstValue = 0d;

                // Flag that a calculation has been made
                hasResult = true;
            }
            else
            {
                secondValue = rndValue;
                lblCurrentResult.Text = Calculator.FormatValue(secondValue.ToString());
                AdjustLabelFont(lblCurrentResult);
                lblCurrentResult.Select();
            }
        }


        // ========= EQUAL BUTTON =========
        private void btnEqual_Click(object sender, EventArgs e)
        {
            // Debug.Print("Equal: firstValue=" & firstValue & ", secondValue=" & secondValue & ", currentOperation=" & Calculator.getOperatorSign(currentOperation) & ", previousOperation=" & Calculator.getOperatorSign(previousOperation))

            // Save the current values for later use
            double oldFirstValue = firstValue;
            double oldSecondValue = secondValue;

            // Make the calculation if no calculation has been done and if there is no zero values
            if (hasResult == false)
            {
                string result = "0";
                if (Conversions.ToDouble(lblCurrentResult.Text) != 0d)
                {
                    // Set the firstValue if it's zero or else set the second value
                    if (firstValue == 0d)
                    {
                        double.TryParse(lblCurrentResult.Text, out firstValue);
                    }
                    else
                    {
                        double.TryParse(lblCurrentResult.Text, out secondValue);
                    }

                    // Calculate and show the result
                    if (currentOperation == Calculator.Operators.PERCENT)
                    {
                        result = Calculator.Calculate(firstValue, secondValue, Calculator.Operators.MULTIPLY).ToString();
                        if (previousOperation == Calculator.Operators.NONE)
                        {
                            displayResult((firstValue * 100d).ToString(), secondValue.ToString(), Calculator.Operators.MULTIPLY, Conversions.ToDouble(result), "", Conversions.ToString(Calculator.getOperatorSign(Calculator.Operators.PERCENT)));
                        }
                        else
                        {
                            displayResult((firstValue * 100d).ToString(), secondValue.ToString(), currentOperation, Conversions.ToDouble(result));
                        }
                    }
                    else if (currentOperation == Calculator.Operators.POWER)
                    {
                        if (oldSecondValue == 0d)
                        {
                            result = Calculator.Calculate(firstValue, secondValue, currentOperation).ToString();
                            lblPreviousResult.Text = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Calculator.FormatValue(firstValue.ToString()), Calculator.getOperatorSign(currentOperation)), Calculator.FormatValue(secondValue.ToString())), " = "));
                        }
                        else
                        {
                            string powerValue = Calculator.Calculate(oldSecondValue, secondValue, currentOperation).ToString();
                            result = Calculator.Calculate(firstValue, Conversions.ToDouble(powerValue), previousOperation).ToString();
                            lblPreviousResult.Text = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Calculator.FormatValue(firstValue.ToString()) + " ", Calculator.getOperatorSign(previousOperation)), " "), Calculator.FormatValue(oldSecondValue.ToString())), Calculator.getOperatorSign(currentOperation)), Calculator.FormatValue(secondValue.ToString())), " = "));
                        }

                        lblCurrentResult.Text = Calculator.FormatValue(result);

                        AdjustLabelFont(lblPreviousResult);
                        AdjustLabelFont(lblCurrentResult);

                        lblCurrentResult.Select();
                    }
                    else if (currentOperation == Calculator.Operators.NONE)
                    {
                        result = firstValue.ToString();
                        displayResult(firstValue.ToString(), 0.ToString(), currentOperation, Conversions.ToDouble(result));
                    }
                    else
                    {
                        result = Calculator.Calculate(firstValue, secondValue, currentOperation).ToString();
                        displayResult(firstValue.ToString(), secondValue.ToString(), currentOperation, Conversions.ToDouble(result));
                    }
                }
                else if (currentOperation == Calculator.Operators.POWER)
                {
                    result = Math.Pow(firstValue, secondValue).ToString();
                    lblPreviousResult.Text = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Calculator.FormatValue(firstValue.ToString()), Calculator.getOperatorSign(currentOperation)), Calculator.FormatValue(secondValue.ToString())), " ="));
                    lblCurrentResult.Text = Calculator.FormatValue(result);

                    AdjustLabelFont(lblPreviousResult);
                    AdjustLabelFont(lblCurrentResult);

                    lblCurrentResult.Select();
                }
                else
                {
                    result = firstValue.ToString();
                    if (currentOperation == Calculator.Operators.PERCENT)
                    {
                        displayResult((firstValue * 100d).ToString(), 0.ToString(), Calculator.Operators.NONE, Conversions.ToDouble(result), "", "%");
                    }
                    else
                    {
                        displayResult(firstValue.ToString(), 0.ToString(), Calculator.Operators.NONE, Conversions.ToDouble(result));
                    }
                }

                // Mark that a calculation result has been made
                hasResult = true;

                // Reset variables
                resetValues();
            }
        }


        // ========= TOGGLE (+/-) BUTTON =========
        private void btnToggle_Click(object sender, EventArgs e)
        {
            // Debug.Print("Toggle: firstValue=" & firstValue & ", secondValue=" & secondValue & ", currentOperation=" & Calculator.getOperatorSign(currentOperation) & ", previousOperation=" & Calculator.getOperatorSign(previousOperation))

            // Don't toggle the value if empty or zero
            if (string.IsNullOrEmpty(lblCurrentResult.Text))
            {
                return;
            }

            // Reset variable and previous result if calculation result is set to true, but save status for hasResult
            if (hasResult)
            {
                lblPreviousResult.Text = "";
            }

            // Remove any whitespaces
            string value = lblCurrentResult.Text.Trim();

            // Toggle the value
            if (value.StartsWith("-"))
            {
                lblCurrentResult.Text = value.Substring(1);
            }
            else
            {
                lblCurrentResult.Text = "-" + value;
            }

            // Set focus to current result label
            lblCurrentResult.Select();

            // Set CE to false
            clearEntry = false;
        }


        // ========= CLEAR (C) BUTTON =========
        private void btnClear_Click(object sender, EventArgs e)
        {
            // Debug.Print("Clear / Clear Entry: firstValue=" & firstValue & ", secondValue=" & secondValue & ", currentOperation=" & Calculator.getOperatorSign(currentOperation) & ", previousOperation=" & Calculator.getOperatorSign(previousOperation))

            // Clear everything if Clear Entry (CE) is set to true
            if (clearEntry)
            {
                // Set CE to false
                clearEntry = false;

                // Reset all values
                lblPreviousResult.Text = "";
                lblCurrentResult.Text = "0";
                AdjustLabelFont(lblCurrentResult, true);
                AdjustLabelFont(lblPreviousResult, true);

                resetValues();

                hasResult = false;
                hasMemoryRecall = false;
                lblCurrentResult.Select();
            }
            else
            {
                // Clear the entry only

                // Reset variable and previous result if calculation result is set to true
                if (hasResult)
                {
                    lblPreviousResult.Text = "";
                    hasResult = false;
                }

                hasMemoryRecall = false;

                // Set CE to true
                clearEntry = true;

                // Reset values
                lblCurrentResult.Text = "0";
                hasResult = false;
                lblCurrentResult.Select();
            }
        }


        // ========= DELETE BUTTON =========
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Debug.Print("Delete: firstValue=" & firstValue & ", secondValue=" & secondValue & ", currentOperation=" & Calculator.getOperatorSign(currentOperation) & ", previousOperation=" & Calculator.getOperatorSign(previousOperation))

            // Reset variable and previous result if calculation result is set to true
            if (hasResult)
            {
                lblPreviousResult.Text = "";
                hasResult = false;
                hasMemoryRecall = false;
            }

            // Delete number from the end if no calculation has been made
            if (!hasResult && lblCurrentResult.Text.Trim().Length > 0)
            {
                string output = lblCurrentResult.Text.Substring(0, lblCurrentResult.Text.Length - 1);
                lblCurrentResult.Text = output.Trim();
                AdjustLabelFont(lblCurrentResult);
            }

            // If the value is empty replaces it with a zero
            if (string.IsNullOrEmpty(lblCurrentResult.Text.Trim()))
            {
                lblCurrentResult.Text = "0";
            }

            // Set CE to false
            clearEntry = false;
            lblCurrentResult.Select();
        }

        // ========= KEYBOARD INPUT =========
        private void MainForm_KeyPress(object? sender, KeyPressEventArgs e)
        {           
            char key = e.KeyChar;

            // ===== DIGITS & DECIMAL =====
            if (char.IsDigit(key) || Conversions.ToString(key) == Calculator.DOT_SIGN)
            {

                // Prevent exceeding max length
                if (lblCurrentResult.Text.Length >= MAX_LENGTH)
                {
                    e.Handled = true;
                    return;
                }

                // Decimal handling
                if (key == '.')
                {
                    if (lblCurrentResult.Text.Contains(Calculator.DOT_SIGN))
                    {
                        e.Handled = true;
                        return;
                    }

                    if (string.IsNullOrEmpty(lblCurrentResult.Text))
                    {
                        lblCurrentResult.Text = "0";
                    }
                }

                if (hasResult)
                {
                    lblCurrentResult.Text = "0";
                    lblPreviousResult.Text = "";
                    AdjustLabelFont(lblPreviousResult);
                    AdjustLabelFont(lblCurrentResult);
                    hasResult = false;
                }

                // Append digit/decimal
                lblCurrentResult.Text += Conversions.ToString(key);

                // Format with commas (strip commas first)
                lblCurrentResult.Text = Calculator.FormatValue(lblCurrentResult.Text.Replace(".", ""));

                AdjustLabelFont(lblCurrentResult);

                e.Handled = true;
                return;
            }

            // ===== OPERATORS =====
            switch (key)
            {
                case '+':
                case '-':
                case '*':
                case '/':
                    {
                        var opBtn = new Button();

                        switch (key)
                        {
                            case '+':
                                {
                                    opBtn.Text = Conversions.ToString(Calculator.getOperatorSign(Calculator.Operators.ADD));
                                    break;
                                }
                            case '-':
                                {
                                    opBtn.Text = Conversions.ToString(Calculator.getOperatorSign(Calculator.Operators.SUBTRACT));
                                    break;
                                }
                            case '*':
                                {
                                    opBtn.Text = Conversions.ToString(Calculator.getOperatorSign(Calculator.Operators.MULTIPLY));
                                    break;
                                }
                            case '/':
                                {
                                    opBtn.Text = Conversions.ToString(Calculator.getOperatorSign(Calculator.Operators.DIVIDE));
                                    break;
                                }
                        }

                        OperatorButton_Click(opBtn, EventArgs.Empty);
                        e.Handled = true;
                        return;
                    }
            }

            // ===== ENTER (=) =====
            if (key == '\r' || key == '=')
            {
                btnEqual_Click(btnEqual, EventArgs.Empty);
                e.Handled = true;
                return;
            }

            // ===== BACKSPACE =====
            if (key == '\b')
            {
                btnDelete_Click(btnDelete, EventArgs.Empty);
                e.Handled = true;
                return;
            }
        }


        // Handles special keys (like numpad multiply/divide)
        private void MainForm_KeyDown(object? sender, KeyEventArgs e)
        {          
            switch (e.KeyCode)
            {
                case Keys.Multiply:
                    {
                        OperatorButton_Click(new Button() { Text = Conversions.ToString(Calculator.getOperatorSign(Calculator.Operators.MULTIPLY)) }, null);
                        e.Handled = true;
                        break;
                    }

                case Keys.Divide:
                    {
                        OperatorButton_Click(new Button() { Text = Conversions.ToString(Calculator.getOperatorSign(Calculator.Operators.DIVIDE)) }, null);
                        e.Handled = true;
                        break;
                    }

                case Keys.Add:
                    {
                        OperatorButton_Click(new Button() { Text = Conversions.ToString(Calculator.getOperatorSign(Calculator.Operators.ADD)) }, null);
                        e.Handled = true;
                        break;
                    }

                case Keys.Subtract:
                    {
                        OperatorButton_Click(new Button() { Text = Conversions.ToString(Calculator.getOperatorSign(Calculator.Operators.SUBTRACT)) }, null);
                        e.Handled = true;
                        break;
                    }

                    // Case Keys.Decimal
                    // NumberButton_Click(New Button() With {.Text = "."}, Nothing)
                    // e.Handled = True
            }
        }


        // ========= FONT AUTO-RESIZE =========
        private void AdjustLabelFont(Label targetLabel, bool reset = false)
        {
            float baseFontSize = 34f; // Default for CurrentResultLabel
            float minFontSize = 12f;  // Smallest allowed

            if (targetLabel.Name == "lblPeviousResult")
            {
                baseFontSize = 22f;
                minFontSize = 11f;
            }

            if (reset || string.IsNullOrEmpty(targetLabel.Text))
            {
                targetLabel.Font = new Font(targetLabel.Font.FontFamily, baseFontSize, targetLabel.Font.Style);
                return;
            }

            int currentLength = targetLabel.Text.Length;
            float newFontSize = baseFontSize;

            if (currentLength > MAX_LENGTH)
            {
                newFontSize = baseFontSize - (currentLength - MAX_LENGTH) * 2;
            }

            if (newFontSize < minFontSize)
            {
                newFontSize = minFontSize;
            }
            else if (newFontSize > baseFontSize)
            {
                newFontSize = baseFontSize;
            }

            targetLabel.Font = new Font(targetLabel.Font.FontFamily, newFontSize, targetLabel.Font.Style);
        }


        // ========= RESET VALUES =========
        public void resetValues()
        {
            firstValue = 0d;
            secondValue = 0d;
            currentOperation = Calculator.Operators.NONE;
            previousOperation = Calculator.Operators.NONE;
        }


        // ========= DISPLAY RESULT =========
        public void displayResult(string firstDisplayValue, string secondDisplayValue, Calculator.Operators operation, double result, string extraSignRight = "", string extraSignLeft = "")
        {
            // Debug.Print("Display result: firstValue=" & firstDisplayValue & ", secondDisplayValue=" & secondValue & ", operation=" & Calculator.getOperatorSign(operation))

            lblCurrentResult.Text = "";

            if (string.IsNullOrEmpty(secondDisplayValue))
            {
                if (operation == Calculator.Operators.POWER)
                {
                    lblPreviousResult.Text = Conversions.ToString(Operators.ConcatenateObject(Calculator.FormatValue(firstDisplayValue, true), Calculator.getOperatorSign(operation)));
                }
                else
                {
                    lblPreviousResult.Text = Conversions.ToString(Operators.ConcatenateObject(Calculator.FormatValue(firstDisplayValue, true) + " ", Calculator.getOperatorSign(operation)));
                }
                lblCurrentResult.Text = Calculator.FormatValue("0", true);
            }
            else if (hasResult)
            {
                lblPreviousResult.Text = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Calculator.FormatValue(firstDisplayValue, true) + " ", Calculator.getOperatorSign(operation)), " "), Calculator.FormatValue(secondDisplayValue, true)));
                lblCurrentResult.Text = Calculator.FormatValue(result.ToString(), true);
            }
            else if (operation == Calculator.Operators.NONE)
            {
                if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(extraSignLeft, Calculator.getOperatorSign(Calculator.Operators.PI), false)))
                {
                    lblPreviousResult.Text = Conversions.ToString(Operators.ConcatenateObject(Calculator.getOperatorSign(Calculator.Operators.PI), " = "));
                }
                else
                {
                    lblPreviousResult.Text = Calculator.FormatValue(firstDisplayValue, true) + extraSignLeft + " =";
                }

                lblCurrentResult.Text = Calculator.FormatValue(result.ToString(), true);
            }
            else
            {
                lblPreviousResult.Text = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Calculator.FormatValue(firstDisplayValue, true) + extraSignLeft + " ", Calculator.getOperatorSign(operation)), " "), Calculator.FormatValue(secondDisplayValue, true)), extraSignRight), " ="));
                lblCurrentResult.Text = Calculator.FormatValue(result.ToString(), true);
            }

            AdjustLabelFont(lblPreviousResult);
            AdjustLabelFont(lblCurrentResult);

            lblCurrentResult.Select();
        }
    }
}