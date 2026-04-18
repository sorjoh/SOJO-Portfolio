Public Class Calculator

    Public Const DOT_SIGN As String = ","

    ' ========= OPERATORS =========
    Public Enum Operators
        ADD
        SUBTRACT
        MULTIPLY
        DIVIDE
        MODULO
        PERCENT
        SQUARE
        CUBE
        PI
        POWER
        SQUARE_ROOT
        CUBE_ROOT
        RECIPROCAL
        RND

        NONE
    End Enum

    ' ========= GET OPERATOR SIGN =========
    Public Shared Function getOperatorSign(ByVal op As Operators)
        Dim sign As String = ""

        Select Case op
            Case Operators.ADD
                sign = "+"
            Case Operators.SUBTRACT
                sign = "−"
            Case Operators.MULTIPLY
                sign = "x"
            Case Operators.DIVIDE
                sign = "÷"
            Case Operators.MODULO
                sign = "≡"
            Case Operators.PERCENT
                sign = "%"
            Case Operators.SQUARE
                sign = "x²"
            Case Operators.CUBE
                sign = "x³"
            Case Operators.PI
                sign = "π"
            Case Operators.POWER
                sign = "^"
            Case Operators.SQUARE_ROOT
                sign = "√"
            Case Operators.CUBE_ROOT
                sign = "∛"
            Case Operators.RECIPROCAL
                sign = "1/x"
            Case Operators.RND
                sign = "rnd"
        End Select

        Return sign
    End Function


    ' ========= GET OPERATOR =========
    Public Shared Function getOperator(ByVal key As String)
        Dim op As Calculator.Operators

        Select Case key
            Case "+"
                op = Calculator.Operators.ADD
            Case "−", "-"
                op = Calculator.Operators.SUBTRACT
            Case "x", "×"
                op = Calculator.Operators.MULTIPLY
            Case "÷", "/"
                op = Calculator.Operators.DIVIDE
            Case "mod", "≡" ', "%"
                op = Calculator.Operators.MODULO
            Case "%"
                op = Calculator.Operators.PERCENT
            Case "x²"
                op = Calculator.Operators.SQUARE
            Case "x³"
                op = Calculator.Operators.CUBE
            Case "π"
                op = Calculator.Operators.PI
            Case "^"
                op = Calculator.Operators.POWER
            Case "√"
                op = Calculator.Operators.SQUARE_ROOT
            Case "∛"
                op = Calculator.Operators.CUBE_ROOT
            Case "1/x"
                op = Calculator.Operators.RECIPROCAL
            Case "rnd"
                op = Calculator.Operators.RND

            Case Else
                op = Calculator.Operators.NONE
        End Select

        Return op
    End Function

    ' ========= CALCULATION OF CUBED ROOT =========
    Public Shared Function CubedRoot(ByVal dNum As Double) As Double
        Return CType(CType(dNum ^ (1 / 3), Decimal), Double)
    End Function


    ' ========= CALCULATE RESULT =========
    Public Shared Function Calculate(ByVal firstValue As Double, ByVal secondValue As Double, ByVal operation As Calculator.Operators) As Double
        Dim result As Double = 0

        Select Case operation
            Case Calculator.Operators.ADD
                result = firstValue + secondValue
            Case Calculator.Operators.SUBTRACT
                result = firstValue - secondValue
            Case Calculator.Operators.MULTIPLY
                result = firstValue * secondValue
            Case Calculator.Operators.DIVIDE
                result = firstValue / secondValue
            Case Calculator.Operators.MODULO
                result = firstValue Mod secondValue
            Case Calculator.Operators.PERCENT
                result = firstValue / 100
            Case Calculator.Operators.SQUARE
                result = firstValue * firstValue
            Case Calculator.Operators.CUBE
                result = firstValue * firstValue * firstValue
            Case Calculator.Operators.PI
                result = Math.PI
            Case Calculator.Operators.POWER
                result = Math.Pow(firstValue, secondValue)
            Case Operators.SQUARE_ROOT
                result = Math.Sqrt(firstValue)
            Case Operators.CUBE_ROOT
                result = Calculator.CubedRoot(firstValue)
            Case Operators.RECIPROCAL
                result = 1 / firstValue

            Case Else
                Debug.Print("Case Else")
        End Select

        Return result
    End Function


    ' ========= FORMAT VALUE =========
    Public Shared Function FormatValue(ByVal input As String, Optional ByVal forceFormat As Boolean = False) As String
        If input.Length > 1 And input.Substring(0, 1) = "0" And input.Contains(DOT_SIGN) = False Then
            Return input.Substring(1)
        ElseIf input.Contains(DOT_SIGN) Then
            If forceFormat Then
                Return Format(CDbl(input), "##,##0.#####################")
            Else
                Return input
            End If
        Else
            Return Format(CDbl(input), "##,##0.#####################")
        End If
    End Function

End Class
