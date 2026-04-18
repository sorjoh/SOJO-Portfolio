' ***************************************************
'
'               SOJO Calculator
'
' ***************************************************


' ========= MAIN FORM =========
Public Class Main
    ' Status indicator if equal button or another calculation has presented a total result
    Dim hasResult As Boolean

    ' Status indicator if value is stored in memory
    Dim hasMemoryRecall As Boolean = False

    ' Variables holding current and previous operations
    Dim currentOperation As Calculator.Operators = Calculator.Operators.NONE
    Dim previousOperation As Calculator.Operators = Calculator.Operators.NONE

    ' Variables holding the first and second values
    Dim firstValue As Double = 0
    Dim secondValue As Double = 0

    ' Variable holding the memory value and status
    Dim memoryValue As Double = 0

    ' Variable if Clear Entry (CE) has been pressed
    Dim clearEntry As Boolean = False

    ' Max length of the display numbers
    Private Const MAX_LENGTH As Integer = 17


    ' ========= MAIN FORM - Control Added =========
    Private Sub frmMain_ControlAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.ControlEventArgs) Handles Me.ControlAdded
        ' Center application on the current screen
        Me.StartPosition = FormStartPosition.CenterScreen
    End Sub


    ' ========= MAIN FORM - Load =========
    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Me.ActiveControl = Nothing

        ' Change period symbol for current language
        btnDot.Text = Calculator.DOT_SIGN
        lblMemoryStatus.Visible = False

        ' Select the result display as default
        lblCurrentResult.Select()
    End Sub

    ' ========= NUMBER BUTTONS =========
    Private Sub NumberButton_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnZero.Click, btnOne.Click, btnTwo.Click, btnThree.Click, btnFour.Click, btnFive.Click, btnSix.Click, btnSeven.Click, btnEight.Click, btnNine.Click, btnDot.Click

        ' Get the current selected button
        Dim btn = CType(sender, Button)

        'Debug.Print("Number: firstValue=" & firstValue & ", secondValue=" & secondValue & ", currentOperation=" & Calculator.getOperatorSign(currentOperation) & ", previousOperation=" & Calculator.getOperatorSign(previousOperation))

        ' Reset the screen if a result has been calculated
        If hasResult Or hasMemoryRecall Then
            lblCurrentResult.Text = "0"
            hasResult = False
            hasMemoryRecall = False

            If currentOperation = Calculator.Operators.NONE And previousOperation = Calculator.Operators.NONE Then
                lblPreviousResult.Text = ""
            Else
                lblPreviousResult.Text = Calculator.FormatValue(firstValue) & " " & Calculator.getOperatorSign(currentOperation)
            End If
        End If

        ' Check if the current number is not longer than the MAX_LENGTH
        If lblCurrentResult.Text.Length >= MAX_LENGTH Then
            Return
        End If

        ' Check if a period exist and if not check if its a empty string and if so add a zero before the period
        If btn.Text = Calculator.DOT_SIGN Then
            If lblCurrentResult.Text.Contains(Calculator.DOT_SIGN) Then
                Return
            End If
            If lblCurrentResult.Text = "" Then
                lblCurrentResult.Text = "0"
            End If
        End If


        ' Add key to the value
        lblCurrentResult.Text &= btn.Text

        ' Format the current result 
        lblCurrentResult.Text = Calculator.FormatValue(lblCurrentResult.Text)

        ' Adjust the font and decrease it if needed
        AdjustLabelFont(lblCurrentResult)

        ' Set the status of Clear Entry to false
        clearEntry = False

        ' Select the result display as default
        lblCurrentResult.Select()
    End Sub


    ' ========= OPERATOR BUTTONS =========
    Private Sub OperatorButton_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnAdd.Click, btnSubtract.Click, btnMultiply.Click, btnDivide.Click, btnModulo.Click, btnPercent.Click, _
        btnSquare.Click, btnCube.Click, btnPower.Click, btnSquareRoot.Click, btnCubeRoot.Click, btnReciprocal.Click

        ' Get the current selected button
        Dim btn As Button = CType(sender, Button)

        ' Initialize the result variable and set it to zero
        Dim result = "0"

        ' Set the status of Clear Entry to false
        hasResult = False
        hasMemoryRecall = False

        ' Set previous operation
        previousOperation = currentOperation

        ' Set current operation
        currentOperation = Calculator.getOperator(btn.Text)

        'Debug.Print("Operator:" & firstNumber & ", " & secondNumber & "->" & Calculator.getOperatorSign(operation) & "," & Calculator.getOperatorSign(newOperation))

        ' Save current values for later use
        Dim oldFirstValue = firstValue
        Dim oldSecondValue = secondValue

        ' Set the firstValue if it's zero or else set the second value
        If firstValue = 0 Then
            Double.TryParse(lblCurrentResult.Text, firstValue)
        Else
            Double.TryParse(lblCurrentResult.Text, secondValue)
        End If

        ' If both no operator and second value or else both first value and second value are present
        If currentOperation <> Calculator.Operators.NONE AndAlso lblCurrentResult.Text <> "" AndAlso secondValue Then
            ' Calculate and display the result

            ' Percent is current operation
            If currentOperation = Calculator.Operators.PERCENT Then
                secondValue = Calculator.Calculate(secondValue, 0, currentOperation).ToString()
                result = Calculator.Calculate(firstValue, secondValue, previousOperation).ToString()

                ' Use previous operator
                Select Case previousOperation
                    ' === ADD BUTTON ===
                    Case Calculator.Operators.ADD
                        result = Calculator.Calculate(firstValue, (1 + secondValue), Calculator.Operators.MULTIPLY).ToString()

                        ' === SUBRACT BUTTON ===
                    Case Calculator.Operators.SUBTRACT
                        result = Calculator.Calculate(firstValue, (1 - secondValue), Calculator.Operators.MULTIPLY).ToString()

                        ' === MULTIPLY BUTTON ===
                    Case Calculator.Operators.MULTIPLY
                        result = Calculator.Calculate(firstValue, secondValue, Calculator.Operators.MULTIPLY).ToString()

                        ' === DIVIDE BUTTON ===
                    Case Calculator.Operators.DIVIDE
                        result = Calculator.Calculate(firstValue, secondValue, Calculator.Operators.DIVIDE).ToString()

                    Case Else
                        Debug.Print("Case Else: firstValue=" & firstValue & ", secondValue=" & secondValue & " result=" & result & ", currentOperation=" & Calculator.getOperatorSign(currentOperation) & ", previousOperation=" & Calculator.getOperatorSign(previousOperation))
                End Select

                ' Display the result
                displayResult(firstValue, secondValue * 100, previousOperation, result, Calculator.getOperatorSign(currentOperation))

                ' FLag that a calculation has been done
                hasResult = True

                ' Reset values
                resetValues()

                ' Percent is previous operation
            ElseIf previousOperation = Calculator.Operators.PERCENT Then
                ' Fix when calculating xx % xx - xx
                If currentOperation <> Calculator.Operators.PERCENT Then
                    ' Set the second value             
                    Double.TryParse(lblCurrentResult.Text, secondValue)

                    ' Calculate and view the result
                    result = Calculator.Calculate(firstValue, secondValue, Calculator.Operators.MULTIPLY).ToString()
                    displayResult(firstValue * 100, secondValue, Calculator.Operators.MULTIPLY, result, "", Calculator.getOperatorSign(Calculator.Operators.PERCENT))

                    ' Mark that a calculation result has been made
                    hasResult = True

                    ' Reset variables
                    firstValue = result
                    secondValue = 0

                    ' Other percent calculation when the percent sign shall be showed
                Else
                    ' Calculate and view the result
                    result = Calculator.Calculate(firstValue, secondValue, Calculator.Operators.MULTIPLY).ToString()
                    displayResult(firstValue * 100, secondValue, Calculator.Operators.MULTIPLY, result, "", Calculator.getOperatorSign(Calculator.Operators.PERCENT))

                    ' Mark that a calculation has been made
                    hasResult = True

                    ' Reset variables
                    resetValues()
                End If
            Else
                ' Operations with Square, Square Root, Cube, Cube Root and Power, Reciprocal
                Select Case currentOperation
                    ' === SQUARE BUTTON ===
                    Case Calculator.Operators.SQUARE
                        ' Calculate and view the result
                        result = Calculator.Calculate(firstValue, secondValue * secondValue, previousOperation).ToString()
                        displayResult(firstValue, secondValue, previousOperation, result, "²", "")

                        ' Flag that a calculation has been made
                        hasResult = True

                        ' Reset variables
                        resetValues()

                        ' === CUBE BUTTON ===
                    Case Calculator.Operators.CUBE
                        ' Calculate and view the resut
                        result = Calculator.Calculate(firstValue, secondValue * secondValue * secondValue, previousOperation).ToString()
                        displayResult(firstValue, secondValue, previousOperation, result, "³", "")

                        ' Flag that a calculation has been made
                        hasResult = True

                        ' Reset variables
                        resetValues()

                        ' === POWER BUTTON ===
                    Case Calculator.Operators.POWER
                        ' Show the value and the power sign in previous result
                        lblPreviousResult.Text = Calculator.FormatValue(firstValue) & " " & Calculator.getOperatorSign(previousOperation) & " " & secondValue & "^"

                        ' Set current result to zero
                        lblCurrentResult.Text = "0"

                        ' Adjust the size of the labels depending on how many numbers is present
                        AdjustLabelFont(lblPreviousResult)
                        AdjustLabelFont(lblCurrentResult)

                        ' Set focus to current result label
                        lblCurrentResult.Select()

                        ' === SQUARE ROOT BUTTON ===
                    Case Calculator.Operators.SQUARE_ROOT
                        ' Calculate and view the result
                        result = Calculator.Calculate(firstValue, Math.Sqrt(secondValue), previousOperation).ToString()
                        lblPreviousResult.Text = Calculator.FormatValue(firstValue) & " " & Calculator.getOperatorSign(previousOperation) & " " & Calculator.getOperatorSign(currentOperation) & secondValue & " ="
                        lblCurrentResult.Text = Calculator.FormatValue(result)

                        ' Adjust the size of the labels depending on how many numbers is present
                        AdjustLabelFont(lblPreviousResult)
                        AdjustLabelFont(lblCurrentResult)

                        ' Set focus to current resut label
                        lblCurrentResult.Select()

                        ' Flag that a calculation has been made
                        hasResult = True

                        ' Reset variables
                        resetValues()

                        ' === CUBE ROOT BUTTON ===
                    Case Calculator.Operators.CUBE_ROOT
                        ' Calculate and view the result
                        result = Calculator.Calculate(firstValue, Calculator.CubedRoot(secondValue), previousOperation).ToString()
                        lblPreviousResult.Text = Calculator.FormatValue(firstValue) & " " & Calculator.getOperatorSign(previousOperation) & " " & Calculator.getOperatorSign(currentOperation) & secondValue & " ="
                        lblCurrentResult.Text = Calculator.FormatValue(result)

                        ' Adjust the size of the labels depending on how many numbers is present
                        AdjustLabelFont(lblPreviousResult)
                        AdjustLabelFont(lblCurrentResult)

                        ' Set focus to current resut label
                        lblCurrentResult.Select()

                        ' Flag that a calculation has been made
                        hasResult = True

                        ' Reset variables
                        resetValues()

                        ' === RECIPROCAL BUTTON ===
                    Case Calculator.Operators.RECIPROCAL
                        ' Calculate and view the result
                        result = Calculator.Calculate(firstValue, 1 / secondValue, previousOperation).ToString()
                        displayResult(firstValue, 1 / secondValue, previousOperation, result)

                        ' Flag that a calculation has been made
                        hasResult = True

                        ' Reset variables
                        resetValues()

                    Case Else
                        ' Power Button is previous operation
                        If previousOperation = Calculator.Operators.POWER Then
                            ' Only the first power number is present otherwise add the saved old second value for the second power number
                            If oldSecondValue = 0 Then
                                ' Calculate the result
                                result = Calculator.Calculate(firstValue, secondValue, previousOperation).ToString()
                            Else
                                ' Calculate the total calculation
                                Dim powerValue = Calculator.Calculate(oldSecondValue, secondValue, previousOperation).ToString()
                                result = Calculator.Calculate(firstValue, powerValue, currentOperation).ToString()
                            End If
                        Else
                            ' Calculate other calculations
                            result = Calculator.Calculate(firstValue, secondValue, previousOperation).ToString()
                        End If

                        ' Display the result in previous result and set current result to zero
                        displayResult(result, "", currentOperation, "0")

                        ' Set the result to the first value and reset the second value
                        firstValue = result
                        secondValue = 0
                End Select
            End If
        Else
            ' Power is previous operation
            If previousOperation = Calculator.Operators.POWER Then
                ' Calculate and display the result
                result = Math.Pow(firstValue, secondValue)
                lblPreviousResult.Text = Calculator.FormatValue(result) & " " & Calculator.getOperatorSign(currentOperation)

                ' Adjust the font size depending on how many numbers
                AdjustLabelFont(lblPreviousResult)

                ' Set focus to current result label
                lblCurrentResult.Select()
            Else
                ' Calculate the result
                result = Calculator.Calculate(firstValue, secondValue, currentOperation).ToString()

                ' Display the result for current operation
                Select Case currentOperation
                    ' === PERCENT OPERATION ===
                    Case Calculator.Operators.PERCENT
                        ' Set the result as first value
                        firstValue = result
                        ' Display the result
                        displayResult(firstValue, "", Calculator.Operators.NONE, 0)

                        ' === SQUARE OPERATION ===
                    Case Calculator.Operators.SQUARE
                        ' Calculate and show the result
                        result = Calculator.Calculate(firstValue, firstValue, Calculator.Operators.SQUARE).ToString()
                        displayResult(firstValue, firstValue, Calculator.Operators.NONE, result, "", "²")

                        ' Flag that a calculation has been made
                        hasResult = True

                        ' Reset variables
                        resetValues()

                        ' === CUBE OPERATION ===
                    Case Calculator.Operators.CUBE
                        ' Calculate and show the result
                        result = Calculator.Calculate(firstValue, firstValue, Calculator.Operators.CUBE).ToString()
                        displayResult(firstValue, firstValue, Calculator.Operators.NONE, result, "", "³")

                        ' Flag that a calculation has been made
                        hasResult = True

                        ' Reset variables
                        resetValues()

                        ' === SQUARE ROOT OPERATION ===
                    Case Calculator.Operators.SQUARE_ROOT, Calculator.Operators.CUBE_ROOT
                        ' Calculate the result
                        result = Calculator.Calculate(firstValue, firstValue, currentOperation).ToString()

                        ' Display the result
                        lblPreviousResult.Text = Calculator.getOperatorSign(currentOperation) & Calculator.FormatValue(firstValue) & " ="
                        lblCurrentResult.Text = Calculator.FormatValue(result)

                        ' Adjust the font size depending on how many numbers
                        AdjustLabelFont(lblPreviousResult)
                        AdjustLabelFont(lblCurrentResult)

                        ' Set focus to current result label
                        lblCurrentResult.Select()

                        ' Flag that a calculation has been made
                        hasResult = True

                        ' Reset variables
                        resetValues()

                        ' === RECIPROCAL OPERATION ===
                    Case Calculator.Operators.RECIPROCAL
                        ' Calculate and show the result
                        result = Calculator.Calculate(1, firstValue, Calculator.Operators.DIVIDE).ToString()
                        displayResult(1, firstValue, Calculator.Operators.DIVIDE, result)

                        ' Flag that a calculation has been made
                        hasResult = True

                        ' Reset variables
                        resetValues()
                    Case Else
                        displayResult(firstValue, "", currentOperation, result)
                End Select
            End If
        End If
    End Sub


    ' ========= MEMORY BUTTONS =========
    Private Sub MemoryButton_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnMemoryClear.Click, btnMemoryRecall.Click, btnMemoryStore.Click, btnMemoryAdd.Click, btnMemorySub.Click

        ' Get the current selected button
        Dim btn As Button = CType(sender, Button)

        ' Get the current value
        Dim currentValue = lblCurrentResult.Text

        ' Reset the memory recall status
        hasMemoryRecall = False

        ' Get selected memory button
        Select Case btn.Text
            ' === MEMORY CLEAR ===
            Case "MC"
                ' Clear the memory and hide the memory label
                memoryValue = 0
                lblMemoryStatus.Visible = False

                ' === MEMORY RECALL ===
            Case "MR"
                ' Show the memory value
                lblCurrentResult.Text = Calculator.FormatValue(memoryValue)
                ' Set memory recall status to true
                hasMemoryRecall = True

                ' Adjust the font size depending on how many numbers
                AdjustLabelFont(lblCurrentResult)

                ' Set focus to current result label
                lblCurrentResult.Select()

                ' === MEMORY STORE ===
            Case "MS"
                ' Store the current value in memory and show the memory label
                memoryValue = currentValue
                lblMemoryStatus.Visible = True

                ' === MEMORY ADD ===
            Case "M+"
                ' Add the current value to the memory and show the memory label
                memoryValue += currentValue
                lblMemoryStatus.Visible = True

                ' === MEMORY SUBTRACT
            Case "M-"
                ' Subtract the current value to the memory and show the memory label
                memoryValue -= currentValue
                lblMemoryStatus.Visible = True
        End Select
    End Sub


    ' ========= PI BUTTON =========
    Private Sub btnPi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPi.Click
        'Debug.Print("PI: firstValue=" & firstValue & ", secondValue=" & secondValue & ", currentOperation=" & Calculator.getOperatorSign(currentOperation) & ", previousOperation=" & Calculator.getOperatorSign(previousOperation))

        ' Set the firstValue if it's zero or else set the second value to PI (3.14159...)
        If firstValue = 0 Then
            If hasResult = False Then
                firstValue = Math.PI
                displayResult(firstValue, 0, currentOperation, firstValue, "", Calculator.getOperatorSign(Calculator.Operators.PI))
                firstValue = 0

                ' Flag that a calculation has been made
                hasResult = True
            End If
        Else
            secondValue = Math.PI
            lblCurrentResult.Text = Calculator.FormatValue(secondValue)
            AdjustLabelFont(lblCurrentResult)
            lblCurrentResult.Select()
        End If
    End Sub


    ' ========= RND BUTTON =========
    Private Sub btnRnd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRnd.Click
        'Debug.Print("RND: firstValue=" & firstValue & ", secondValue=" & secondValue & ", currentOperation=" & Calculator.getOperatorSign(currentOperation) & ", previousOperation=" & Calculator.getOperatorSign(previousOperation))

        ' Get a random integer between 1 and 9
        Dim rndValue As Integer = CInt(Int((9 * Rnd()) + 1))

        ' Set the firstValue if it's zero or else set the second value to the created random value
        If firstValue = 0 Then
            firstValue = rndValue

            ' Display the random value
            lblPreviousResult.Text = Calculator.getOperatorSign(Calculator.Operators.RND) & " ="
            lblCurrentResult.Text = Calculator.FormatValue(firstValue)

            ' Adjust the font size depending on the number size
            AdjustLabelFont(lblPreviousResult)
            AdjustLabelFont(lblCurrentResult)
            lblCurrentResult.Select()

            firstValue = 0

            ' Flag that a calculation has been made
            hasResult = True
        Else
            secondValue = rndValue
            lblCurrentResult.Text = Calculator.FormatValue(secondValue)
            AdjustLabelFont(lblCurrentResult)
            lblCurrentResult.Select()
        End If
    End Sub


    ' ========= EQUAL BUTTON =========
    Private Sub btnEqual_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEqual.Click
        'Debug.Print("Equal: firstValue=" & firstValue & ", secondValue=" & secondValue & ", currentOperation=" & Calculator.getOperatorSign(currentOperation) & ", previousOperation=" & Calculator.getOperatorSign(previousOperation))

        ' Save the current values for later use
        Dim oldFirstValue = firstValue
        Dim oldSecondValue = secondValue

        ' Make the calculation if no calculation has been done and if there is no zero values
        If hasResult = False Then
            Dim result = "0"
            If lblCurrentResult.Text <> 0 Then
                ' Set the firstValue if it's zero or else set the second value
                If firstValue = 0 Then
                    Double.TryParse(lblCurrentResult.Text, firstValue)
                Else
                    Double.TryParse(lblCurrentResult.Text, secondValue)
                End If

                ' Calculate and show the result
                If currentOperation = Calculator.Operators.PERCENT Then
                    result = Calculator.Calculate(firstValue, secondValue, Calculator.Operators.MULTIPLY).ToString()
                    If previousOperation = Calculator.Operators.NONE Then
                        displayResult(firstValue * 100, secondValue, Calculator.Operators.MULTIPLY, result, "", Calculator.getOperatorSign(Calculator.Operators.PERCENT))
                    Else
                        displayResult(firstValue * 100, secondValue, currentOperation, result)
                    End If
                ElseIf currentOperation = Calculator.Operators.POWER Then
                    If oldSecondValue = 0 Then
                        result = Calculator.Calculate(firstValue, secondValue, currentOperation).ToString()
                        lblPreviousResult.Text = Calculator.FormatValue(firstValue) & Calculator.getOperatorSign(currentOperation) & Calculator.FormatValue(secondValue) & " = "
                    Else
                        Dim powerValue = Calculator.Calculate(oldSecondValue, secondValue, currentOperation).ToString()
                        result = Calculator.Calculate(firstValue, powerValue, previousOperation).ToString()
                        lblPreviousResult.Text = Calculator.FormatValue(firstValue) & " " & Calculator.getOperatorSign(previousOperation) & " " & Calculator.FormatValue(oldSecondValue) & Calculator.getOperatorSign(currentOperation) & Calculator.FormatValue(secondValue) & " = "
                    End If

                    lblCurrentResult.Text = Calculator.FormatValue(result)

                    AdjustLabelFont(lblPreviousResult)
                    AdjustLabelFont(lblCurrentResult)

                    lblCurrentResult.Select()
                Else
                    If currentOperation = Calculator.Operators.NONE Then
                        result = firstValue
                        displayResult(firstValue, 0, currentOperation, result)
                    Else
                        result = Calculator.Calculate(firstValue, secondValue, currentOperation).ToString()
                        displayResult(firstValue, secondValue, currentOperation, result)
                    End If
                End If
            Else
                If currentOperation = Calculator.Operators.POWER Then
                    result = Math.Pow(firstValue, secondValue)
                    lblPreviousResult.Text = Calculator.FormatValue(firstValue) & Calculator.getOperatorSign(currentOperation) & Calculator.FormatValue(secondValue) & " ="
                    lblCurrentResult.Text = Calculator.FormatValue(result)

                    AdjustLabelFont(lblPreviousResult)
                    AdjustLabelFont(lblCurrentResult)

                    lblCurrentResult.Select()
                Else
                    result = firstValue
                    If currentOperation = Calculator.Operators.PERCENT Then
                        displayResult(firstValue * 100, 0, Calculator.Operators.NONE, result, "", "%")
                    Else
                        displayResult(firstValue, 0, Calculator.Operators.NONE, result)
                    End If
                End If
            End If

            ' Mark that a calculation result has been made
            hasResult = True

            ' Reset variables
            resetValues()
        End If
    End Sub


    ' ========= TOGGLE (+/-) BUTTON =========
    Private Sub btnToggle_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnToggle.Click
        'Debug.Print("Toggle: firstValue=" & firstValue & ", secondValue=" & secondValue & ", currentOperation=" & Calculator.getOperatorSign(currentOperation) & ", previousOperation=" & Calculator.getOperatorSign(previousOperation))

        ' Don't toggle the value if empty or zero
        If lblCurrentResult.Text = "" Then
            Return
        End If

        ' Reset variable and previous result if calculation result is set to true, but save status for hasResult
        If hasResult Then
            lblPreviousResult.Text = ""
        End If

        ' Remove any whitespaces
        Dim value As String = lblCurrentResult.Text.Trim()

        ' Toggle the value
        If value.StartsWith("-") Then
            lblCurrentResult.Text = value.Substring(1)
        Else
            lblCurrentResult.Text = "-" & value
        End If

        ' Set focus to current result label
        lblCurrentResult.Select()

        ' Set CE to false
        clearEntry = False
    End Sub


    ' ========= CLEAR (C) BUTTON =========
    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.Click
        'Debug.Print("Clear / Clear Entry: firstValue=" & firstValue & ", secondValue=" & secondValue & ", currentOperation=" & Calculator.getOperatorSign(currentOperation) & ", previousOperation=" & Calculator.getOperatorSign(previousOperation))

        ' Clear everything if Clear Entry (CE) is set to true
        If clearEntry Then
            ' Set CE to false
            clearEntry = False

            ' Reset all values
            lblPreviousResult.Text = ""
            lblCurrentResult.Text = "0"
            AdjustLabelFont(lblCurrentResult, True)
            AdjustLabelFont(lblPreviousResult, True)

            resetValues()

            hasResult = False
            hasMemoryRecall = False
            lblCurrentResult.Select()
        Else
            ' Clear the entry only

            ' Reset variable and previous result if calculation result is set to true
            If hasResult Then
                lblPreviousResult.Text = ""
                hasResult = False
            End If

            hasMemoryRecall = False

            ' Set CE to true
            clearEntry = True

            ' Reset values
            lblCurrentResult.Text = "0"
            hasResult = False
            lblCurrentResult.Select()
        End If
    End Sub


    ' ========= DELETE BUTTON =========
    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        'Debug.Print("Delete: firstValue=" & firstValue & ", secondValue=" & secondValue & ", currentOperation=" & Calculator.getOperatorSign(currentOperation) & ", previousOperation=" & Calculator.getOperatorSign(previousOperation))

        ' Reset variable and previous result if calculation result is set to true
        If hasResult Then
            lblPreviousResult.Text = ""
            hasResult = False
            hasMemoryRecall = False
        End If

        ' Delete number from the end if no calculation has been made
        If Not hasResult AndAlso lblCurrentResult.Text.Trim().Length > 0 Then
            Dim output = lblCurrentResult.Text.Substring(0, lblCurrentResult.Text.Length - 1)
            lblCurrentResult.Text = output.Trim()
            AdjustLabelFont(lblCurrentResult)
        End If

        ' If the value is empty replaces it with a zero
        If lblCurrentResult.Text.Trim() = "" Then
            lblCurrentResult.Text = "0"
        End If

        ' Set CE to false
        clearEntry = False
        lblCurrentResult.Select()
    End Sub

    ' ========= KEYBOARD INPUT =========
    Private Sub frmMain_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles Me.KeyPress
        Dim key As Char = e.KeyChar

        ' ===== DIGITS & DECIMAL =====
        If Char.IsDigit(key) OrElse key = Calculator.DOT_SIGN Then

            ' Prevent exceeding max length
            If lblCurrentResult.Text.Length >= MAX_LENGTH Then
                e.Handled = True
                Return
            End If

            ' Decimal handling
            If key = "."c Then
                If lblCurrentResult.Text.Contains(Calculator.DOT_SIGN) Then
                    e.Handled = True
                    Return
                End If

                If lblCurrentResult.Text = "" Then
                    lblCurrentResult.Text = "0"
                End If
            End If

            If hasResult Then
                lblCurrentResult.Text = "0"
                lblPreviousResult.Text = ""
                AdjustLabelFont(lblPreviousResult)
                AdjustLabelFont(lblCurrentResult)
                hasResult = False
            End If

            ' Append digit/decimal
            lblCurrentResult.Text &= key

            ' Format with commas (strip commas first)
            lblCurrentResult.Text = Calculator.FormatValue(lblCurrentResult.Text.Replace(".", ""))

            AdjustLabelFont(lblCurrentResult)

            e.Handled = True
            Return
        End If

        ' ===== OPERATORS =====
        Select Case key
            Case "+"c, "-"c, "*"c, "/"c
                Dim opBtn As New Button()

                Select Case key
                    Case "+"c : opBtn.Text = Calculator.getOperatorSign(Calculator.Operators.ADD)
                    Case "-"c : opBtn.Text = Calculator.getOperatorSign(Calculator.Operators.SUBTRACT)
                    Case "*"c : opBtn.Text = Calculator.getOperatorSign(Calculator.Operators.MULTIPLY)
                    Case "/"c : opBtn.Text = Calculator.getOperatorSign(Calculator.Operators.DIVIDE)
                End Select

                OperatorButton_Click(opBtn, EventArgs.Empty)
                e.Handled = True
                Return
        End Select

        ' ===== ENTER (=) =====
        If key = ChrW(Keys.Enter) OrElse key = "="c Then
            btnEqual_Click(btnEqual, EventArgs.Empty)
            e.Handled = True
            Return
        End If

        ' ===== BACKSPACE =====
        If key = ChrW(Keys.Back) Then
            btnDelete_Click(btnDelete, EventArgs.Empty)
            e.Handled = True
            Return
        End If
    End Sub


    ' Handles special keys (like numpad multiply/divide)
    Private Sub frmMain_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Multiply
                OperatorButton_Click(New Button() With {.Text = Calculator.getOperatorSign(Calculator.Operators.MULTIPLY)}, Nothing)
                e.Handled = True

            Case Keys.Divide
                OperatorButton_Click(New Button() With {.Text = Calculator.getOperatorSign(Calculator.Operators.DIVIDE)}, Nothing)
                e.Handled = True

            Case Keys.Add
                OperatorButton_Click(New Button() With {.Text = Calculator.getOperatorSign(Calculator.Operators.ADD)}, Nothing)
                e.Handled = True

            Case Keys.Subtract
                OperatorButton_Click(New Button() With {.Text = Calculator.getOperatorSign(Calculator.Operators.SUBTRACT)}, Nothing)
                e.Handled = True

                'Case Keys.Decimal
                'NumberButton_Click(New Button() With {.Text = "."}, Nothing)
                'e.Handled = True
        End Select
    End Sub


    ' ========= FONT AUTO-RESIZE =========
    Private Sub AdjustLabelFont(ByVal targetLabel As Label, Optional ByVal reset As Boolean = False)
        Dim baseFontSize As Single = 34 ' Default for CurrentResultLabel
        Dim minFontSize As Single = 12  ' Smallest allowed

        If targetLabel.Name = "lblPeviousResult" Then
            baseFontSize = 22
            minFontSize = 11
        End If

        If reset OrElse String.IsNullOrEmpty(targetLabel.Text) Then
            targetLabel.Font = New Font(targetLabel.Font.FontFamily, baseFontSize, targetLabel.Font.Style)
            Return
        End If

        Dim currentLength As Integer = targetLabel.Text.Length
        Dim newFontSize As Single = baseFontSize

        If currentLength > MAX_LENGTH Then
            newFontSize = baseFontSize - ((currentLength - MAX_LENGTH) * 2)
        End If

        If newFontSize < minFontSize Then
            newFontSize = minFontSize
        ElseIf newFontSize > baseFontSize Then
            newFontSize = baseFontSize
        End If

        targetLabel.Font = New Font(targetLabel.Font.FontFamily, newFontSize, targetLabel.Font.Style)
    End Sub


    ' ========= RESET VALUES =========
    Sub resetValues()
        firstValue = 0
        secondValue = 0
        currentOperation = Calculator.Operators.NONE
        previousOperation = Calculator.Operators.NONE
    End Sub


    ' ========= DISPLAY RESULT =========
    Sub displayResult(ByVal firstDisplayValue As String, ByVal secondDisplayValue As String, ByVal operation As Calculator.Operators, ByVal result As Double, Optional ByVal extraSignRight As String = "", Optional ByVal extraSignLeft As String = "")
        'Debug.Print("Display result: firstValue=" & firstDisplayValue & ", secondDisplayValue=" & secondValue & ", operation=" & Calculator.getOperatorSign(operation))

        lblCurrentResult.Text = ""

        If String.IsNullOrEmpty(secondDisplayValue) Then
            If operation = Calculator.Operators.POWER Then
                lblPreviousResult.Text = Calculator.FormatValue(firstDisplayValue, True) & Calculator.getOperatorSign(operation)
            Else
                lblPreviousResult.Text = Calculator.FormatValue(firstDisplayValue, True) & " " & Calculator.getOperatorSign(operation)
            End If
            lblCurrentResult.Text = Calculator.FormatValue("0", True)
        ElseIf hasResult Then
            lblPreviousResult.Text = Calculator.FormatValue(firstDisplayValue, True) & " " & Calculator.getOperatorSign(operation) & " " & Calculator.FormatValue(secondDisplayValue, True)
            lblCurrentResult.Text = Calculator.FormatValue(result, True)
        Else
            If operation = Calculator.Operators.NONE Then
                If extraSignLeft = Calculator.getOperatorSign(Calculator.Operators.PI) Then
                    lblPreviousResult.Text = Calculator.getOperatorSign(Calculator.Operators.PI) & " = "
                Else
                    lblPreviousResult.Text = Calculator.FormatValue(firstDisplayValue, True) & extraSignLeft & " ="
                End If

                lblCurrentResult.Text = Calculator.FormatValue(result, True)
            Else
                lblPreviousResult.Text = Calculator.FormatValue(firstDisplayValue, True) & extraSignLeft & " " & Calculator.getOperatorSign(operation) & " " & Calculator.FormatValue(secondDisplayValue, True) & extraSignRight & " ="
                lblCurrentResult.Text = Calculator.FormatValue(result, True)
            End If
        End If

        AdjustLabelFont(lblPreviousResult)
        AdjustLabelFont(lblCurrentResult)

        lblCurrentResult.Select()
    End Sub

End Class
