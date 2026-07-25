package se.sojo.apps.maths.calculator.ui

import android.annotation.SuppressLint
import android.util.Log
import android.view.MotionEvent
import android.view.View
import android.widget.Button
import se.sojo.apps.maths.calculator.core.Calculator
import se.sojo.apps.maths.calculator.MainActivity
import se.sojo.apps.maths.calculator.MainActivity.Companion.DEBUG
import se.sojo.apps.maths.calculator.R
import se.sojo.apps.maths.calculator.core.Calculator.Companion.MAX_DECIMALS_AND_ZEROS
import se.sojo.apps.maths.calculator.core.Calculator.Companion.NOT_A_NUMBER
import se.sojo.apps.maths.calculator.core.Calculator.Companion.formatValue
import se.sojo.apps.maths.calculator.core.cleanUpDecimalSeparator
import se.sojo.apps.maths.calculator.core.cleanUpMinusSign
import se.sojo.apps.maths.calculator.core.cleanUpZeroValue
import se.sojo.apps.maths.calculator.core.performHapticFeedback
import se.sojo.apps.maths.calculator.core.removeOperationSign
import se.sojo.apps.maths.calculator.core.removeThousandSeparator
import se.sojo.apps.maths.calculator.core.tryParse
import java.math.BigDecimal
import java.math.RoundingMode
import kotlin.math.cbrt
import kotlin.math.pow
import kotlin.math.sqrt

// Operation buttons
@SuppressLint("ClickableViewAccessibility")
fun MainActivity.setOperationButtons() {
    btnAdd = findViewById(R.id.btn_add)
    btnSubtract = findViewById(R.id.btn_subtract)
    btnMultiply = findViewById(R.id.btn_multiply)
    btnDivide = findViewById(R.id.btn_divide)
    btnModulo = findViewById(R.id.btn_modulo)
    btnPercent = findViewById(R.id.btn_percent)
    btnSquare = findViewById(R.id.btn_square)
    btnCube = findViewById(R.id.btn_cube)
    btnPower = findViewById(R.id.btn_power)
    btnSquareRoot = findViewById(R.id.btn_square_root)
    btnCubeRoot = findViewById(R.id.btn_cube_root)
    btnReciprocal = findViewById(R.id.btn_reciprocal)

    // PI, RND, Toggle (+/-) buttons
    btnPi = findViewById(R.id.btn_pi)
    btnRnd = findViewById(R.id.btn_rnd)
    btnToggle = findViewById(R.id.btn_toggle)

    // Delete, Equal, CLear buttons
    btnDelete = findViewById(R.id.btn_delete)
    btnEqual = findViewById(R.id.btn_equal)
    btnClear = findViewById(R.id.btn_clear)

    /******************************************************
     * Operation Buttons onClick
     */
    btnAdd?.setOnTouchListener { _, motionEvent -> operationButtonAction(btnAdd!!, motionEvent) }
    btnSubtract?.setOnTouchListener { _, motionEvent -> operationButtonAction(btnSubtract!!, motionEvent) }
    btnMultiply?.setOnTouchListener { _, motionEvent -> operationButtonAction(btnMultiply!!, motionEvent) }
    btnDivide?.setOnTouchListener { _, motionEvent -> operationButtonAction(btnDivide!!, motionEvent) }
    btnModulo?.setOnTouchListener { _, motionEvent -> operationButtonAction(btnModulo!!, motionEvent) }
    btnPercent?.setOnTouchListener { _, motionEvent -> operationButtonAction(btnPercent!!, motionEvent) }
    btnSquare?.setOnTouchListener { _, motionEvent -> operationButtonAction(btnSquare!!, motionEvent) }
    btnCube?.setOnTouchListener { _, motionEvent -> operationButtonAction(btnCube!!, motionEvent) }
    btnPower?.setOnTouchListener { _, motionEvent -> operationButtonAction(btnPower!!, motionEvent) }
    btnSquareRoot?.setOnTouchListener { _, motionEvent -> operationButtonAction(btnSquareRoot!!, motionEvent) }
    btnCubeRoot?.setOnTouchListener { _, motionEvent -> operationButtonAction(btnCubeRoot!!, motionEvent) }
    btnReciprocal?.setOnTouchListener { _, motionEvent -> operationButtonAction(btnReciprocal!!, motionEvent) }

    /******************************************************
     * Other Buttons onClick
     */
    btnPi?.setOnTouchListener { _, motionEvent -> piButtonAction(motionEvent) }
    btnRnd?.setOnTouchListener { _, motionEvent -> rndButtonAction(motionEvent) }
    btnToggle?.setOnTouchListener { _, motionEvent -> toggleButtonAction(motionEvent) }
    btnDelete?.setOnTouchListener { _, motionEvent -> deleteButtonAction(motionEvent) }
    btnEqual?.setOnTouchListener { _, motionEvent -> equalButtonAction(motionEvent) }
    btnClear?.setOnTouchListener { _, motionEvent -> clearButtonAction(motionEvent) }
}

// ========= OPERATOR BUTTON ACTIONS =========
private fun MainActivity.operationButtonAction(btn: Button, motionEvent: MotionEvent): Boolean {
    if (DEBUG) Log.d("SOJO Debug:", "operationButtonAction -> button=" + btn.text.toString() + ", firstValue=" + firstValue.toString() + ", secondValue=" + secondValue.toString() + ", currentOperator=" + currentOperator.toString() + ", previousOperator=" + previousOperator.toString() + ", hasResult=" + hasResult.toString())

    when (motionEvent.action) {
        MotionEvent.ACTION_UP -> {
            doTheEqualCalculating(Calculator.getOperator(btn.text.toString()))
        }
    }

    return performHapticFeedback(btn, motionEvent)
}


// ========= CLEAR (C) BUTTON =========
private fun MainActivity.clearButtonAction(motionEvent: MotionEvent): Boolean {
    when (motionEvent.action) {
        MotionEvent.ACTION_UP -> {
            // Clear everything if Clear Entry (CE) is set to true
            if (clearEntry) {
                // Set CE to false
                clearEntry = false

                // Reset all values
                tvPreviousResult?.text = ""
                tvCurrentResult?.text = "0"
                //adjustLabelFont(tvCurrentResult, true)
                //adjustLabelFont(tvPreviousResult, true)

                resetValues()

                hasResult = false
            } else {
                // Clear the entry only

                // Reset variable and previous result if calculation result is set to true
                if (hasResult) {
                    tvPreviousResult?.text = ""
                    hasResult = false
                    resetValues()
                }

                // Set CE to true
                clearEntry = true

                // Reset values
                tvCurrentResult?.text = "0"
                hasResult = false
            }
        }
    }

    return performHapticFeedback(btnClear, motionEvent)
}

// ========= DELETE BUTTON =========
private fun MainActivity.deleteButtonAction(motionEvent: MotionEvent): Boolean {
    when (motionEvent.action) {
        MotionEvent.ACTION_UP -> {
            // Reset variable and previous result if calculation result is set to true
            if (hasResult) {
                tvPreviousResult?.text = ""
                hasResult = false
                resetValues()
            }

            // Delete number from the end if no calculation has been made
            if (tvCurrentResult?.text.toString().trim().isNotEmpty()) {
                if (tvCurrentResult?.text.toString() == NOT_A_NUMBER) {
                    tvCurrentResult?.text = "0"
                } else {
                    val output = tvCurrentResult?.text?.substring(0, tvCurrentResult?.text.toString().length - 1)
                    tvCurrentResult?.text = output?.trim()
                }
                //adjustLabelFont(tvCurrentResult)
            }

            // If the value is empty replaces it with a zero
            if (tvCurrentResult?.text.toString().trim() == "")
                tvCurrentResult?.text = "0"

            // Set CE to false
            clearEntry = false
        }
    }

    return performHapticFeedback(btnDelete, motionEvent)
}


// ========= EQUAL BUTTON =========
private fun MainActivity.equalButtonAction(motionEvent: MotionEvent?): Boolean {
    if (DEBUG) Log.d("SOJO Debug:",
        "equalButtonAction -> $firstValue, $secondValue, $currentOperator, $previousOperator"
    )

    when (motionEvent?.action) {
        MotionEvent.ACTION_UP -> {
            doTheEqualCalculating(Calculator.Operator.EQUALS)
        }
    }

    return performHapticFeedback(btnEqual, motionEvent)
}

// ========= PI BUTTON =========
private fun MainActivity.piButtonAction(motionEvent: MotionEvent): Boolean {
    if (DEBUG) Log.d("SOJO Debug:",
        "piButtonAction -> $firstValue, $secondValue, $currentOperator, $previousOperator"
    )

    when (motionEvent.action) {
        MotionEvent.ACTION_UP -> {
            // Set the firstValue if it's zero or else set the second value to PI (3.14159...)
            if (firstValue == 0.0 || secondValue == 0.0 && hasResult) {
                if (DEBUG) Log.d("SOJO Debug:", "piButtonAction -> IF START (firstValue == 0.0)")

                if (firstValue == 0.0 && currentOperator != Calculator.Operator.NONE) {

                    tvCurrentResult?.text = formatValue(Math.PI.toString())
                } else {
                    firstValue = Math.PI
                    tvPreviousResult?.text = Calculator.getOperatorSign(Calculator.Operator.PI)
                    tvCurrentResult?.text = formatValue(firstValue.toString())

                    historyItemToAdd =
                        tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
                            Calculator.Operator.EQUALS
                        ) + " " + tvCurrentResult?.text.toString()

                    resetValues()

                    // Flag that a calculation has been made
                    hasResult = true
                }
            } else {
                if (DEBUG) Log.d("SOJO Debug:", "piButtonAction -> IF START (firstValue == 0.0) -> ELSE")

                secondValue = Math.PI

                tvCurrentResult?.text = formatValue(secondValue.toString())

                hasResult = true
            }
        }
    }

    return performHapticFeedback(btnPi, motionEvent)
}

// ========= RND BUTTON =========
private fun MainActivity.rndButtonAction(motionEvent: MotionEvent): Boolean {
    if (DEBUG) Log.d("SOJO Debug:",
        "rndButtonAction -> $firstValue, $secondValue, $currentOperator, $previousOperator"
    )

    when (motionEvent.action) {
        MotionEvent.ACTION_UP -> {
            // Get a random integer between 1 and 9
            val rndValue: Double = (1..9).random().toDouble()

            // Set the firstValue if it's zero or else set the second value to the created random value
            if (firstValue == 0.0 || secondValue == 0.0 && hasResult) {
                if (DEBUG) Log.d("SOJO Debug:", "rndButtonAction -> IF START (firstValue == 0.0)")

                if (firstValue == 0.0 && currentOperator != Calculator.Operator.NONE) {
                    tvCurrentResult?.text = formatValue(rndValue.toString())
                } else {
                    firstValue = rndValue

                    tvPreviousResult?.text = Calculator.getOperatorSign(Calculator.Operator.RND)
                    tvCurrentResult?.text = formatValue(firstValue.toString())

                    historyItemToAdd =
                        tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
                            Calculator.Operator.EQUALS
                        ) + " " + tvCurrentResult?.text.toString()

                    resetValues()

                    // Flag that a calculation has been made
                    hasResult = true
                }
            } else {
                if (DEBUG) Log.d("SOJO Debug:", "rndButtonAction -> IF START (firstValue == 0.0) -> ELSE")

                secondValue = rndValue

                tvCurrentResult?.text = formatValue(secondValue.toString())

                hasResult = true
            }
        }
    }

    return performHapticFeedback(btnRnd, motionEvent)
}

// ========= TOGGLE (+/-) BUTTON =========
private fun MainActivity.toggleButtonAction(motionEvent: MotionEvent): Boolean {
    if (DEBUG) Log.d("SOJO Debug:",
        "toggleButtonAction -> $firstValue, $secondValue, $currentOperator, $previousOperator"
    )

    when (motionEvent.action) {
        MotionEvent.ACTION_UP -> {
            // Don't toggle the value if empty or zero
            if (tvCurrentResult?.text == "") {
                return performHapticFeedback(btnToggle, motionEvent)
            }

            // Reset variable and previous result if calculation result is set to true, but save status for hasResult
            if (hasResult)
                tvPreviousResult?.text = ""

            // Remove any whitespaces
            val value: String = tvCurrentResult?.text.toString().cleanUpMinusSign().trim()

            // Toggle the value
            if (value.startsWith("-"))
                tvCurrentResult?.text = value.substring(1)
            else
                tvCurrentResult?.text = buildString {
                    append("-")
                    append(value)
                }

            // Set CE to false
            clearEntry = false
        }
    }

    resetValues()

    return performHapticFeedback(btnToggle, motionEvent)
}

// ========= DISPLAY RESULT =========
private fun MainActivity.displayResult(firstDisplayValue: String, secondDisplayValue: String, operation: Calculator.Operator, result: Double, extraSignLeftOf: String = " ", extraSignRightOf: String = " ", extraSignAtEnd: String = "") {
    tvCurrentResult?.text = ""

    if (DEBUG) Log.d(
        "SOJO Debug:",
        "displayResult -> $firstDisplayValue, $secondDisplayValue, $operation, $result"
    )

    if (DEBUG) Log.d("SOJO Debug:", buildString {
        append("firstDisplayValue=")
        append(firstDisplayValue)
        append(", secondDisplayValue=")
        append(secondDisplayValue)
        append(", operator=")
        append(operation.toString())
        append(", result=")
        append(result)
    })

    try {
        tvPreviousResult?.text = buildString {
            append(extraSignLeftOf).trim()
            append(formatValue(firstDisplayValue, true).cleanUpZeroValue())
            append(extraSignRightOf)
            append(Calculator.getOperatorSign(operation))
            append(" ")

            if (secondDisplayValue.isNotEmpty())
                append(formatValue(secondDisplayValue, true).cleanUpZeroValue())

            append(extraSignAtEnd)
        }.trim()
    } catch (e: Exception) {
        if (DEBUG) Log.d("SOJO Debug:", buildString {
            append("displayResult -> catch error=")
            append(e.toString())
        })
    }

    tvCurrentResult?.text = formatValue(result.toString(), true).cleanUpZeroValue()
}

fun MainActivity.addHistory(history: String) {
    historyItems.add(history)

    historyArrayAdapter?.notifyDataSetChanged()

    tvDivider?.visibility = View.VISIBLE
}


fun MainActivity.getHistoryItem(position: Int) {
    val historyItem = lvHistoryResult?.getItemAtPosition(position) as String
    val items = historyItem.split(" ")

    var text = ""

    if (historyItemToAdd.isNotBlank()) {
        addHistory(historyItemToAdd)
        historyItemToAdd = ""
    }

    items.forEachIndexed { index, item ->
        if (DEBUG) Log.d("SOJO Debug:", "getHistoryItem() -> when(index) = $index")

        try {
            when (index) {
                0 -> {
                    firstValue =
                        item.cleanUpDecimalSeparator().cleanUpMinusSign().removeOperationSign()
                            .removeThousandSeparator().toDouble()
                    text += "$item "
                }

                1 -> {
                    previousOperator = Calculator.getOperator(item)
                    if (previousOperator == Calculator.Operator.EQUALS) {
                        tvCurrentResult?.text = items[index + 1]
                    } else {
                        text += "$item "
                    }
                }

                2 -> {
                    if (items.size > 3) {
                        secondValue =
                            item.cleanUpDecimalSeparator().cleanUpMinusSign().removeOperationSign()
                                .removeThousandSeparator()
                                .toDouble()
                        text += item
                    }
                }

                3 -> {
                    if (items.size > 5)
                        text += " $item "
                    else
                        currentOperator = Calculator.getOperator(item)
                }

                4 -> {
                    if (items.size > 5)
                        text += "$item "
                    else
                        tvCurrentResult?.text = item
                }

                5 -> {

                }

                6 -> {
                    if (items.size < 8)
                        tvCurrentResult?.text = item
                }

                7 -> {
                    if (items.size < 9)
                        tvCurrentResult?.text = item
                }
            }
        } catch (e: Exception) {
            if (items[0] == Calculator.getOperatorSign(Calculator.Operator.PI) || items[0] == Calculator.getOperatorSign(
                    Calculator.Operator.RND
            )) {
                text = items[0]
            } else {
                if (DEBUG) Log.d("SOJO Debug:", e.toString())
            }
        }
    }

    tvPreviousResult?.text = text

    // Mark that a calculation result has been made
    hasResult = true

    historyItemToAdd =
        tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
            Calculator.Operator.EQUALS
        ) + " " + tvCurrentResult?.text.toString()

    // Reset variables
    resetValues()
}

fun MainActivity.doTheEqualCalculating(operator: Calculator.Operator = Calculator.Operator.NONE) {

    if (DEBUG) Log.d("SOJO Debug:", "equal()")

    // Initialize the result variable and set it to zero
    var result = BigDecimal(0)

    // Set the status of Clear Entry to false
    hasResult = false

    val oldPreviousOperator = previousOperator

    // Set previous operation
    if (operator != Calculator.Operator.NONE) {
        previousOperator = currentOperator

        // Set current operation
        currentOperator = operator
    }

    var oldFirstValue: Double = firstValue
    var oldSecondValue: Double = secondValue

    var input: String = tvCurrentResult?.text.toString().removeThousandSeparator().cleanUpDecimalSeparator()
    input = input.replace(160.toChar().toString(), "").cleanUpMinusSign()

    // Set the firstValue if it's zero or else set the second value
    if (firstValue == 0.0)
        firstValue = input.tryParse()
    else
        secondValue = input.tryParse()

    if (firstValue != 0.0 && firstValue > oldFirstValue && oldFirstValue == secondValue && operator == Calculator.Operator.EQUALS && previousOperator != Calculator.Operator.POWER) {
        secondValue = firstValue
        firstValue = oldFirstValue
        oldFirstValue = 0.0
        oldSecondValue = 0.0
    }

    if (DEBUG) Log.d("SOJO Debug:","firstValue=$firstValue, secondValue=$secondValue, oldFirstValue=$oldFirstValue, oldSecondValue=$oldSecondValue, currentOperator=$currentOperator, previousOperator=$previousOperator, input=$input")

    if (currentOperator != Calculator.Operator.NONE && tvCurrentResult?.text.toString() != "" && secondValue != 0.0) {
        if (DEBUG) Log.d("SOJO Debug:", "equal() -> IF START")

        when (operator) {
            // === EQUAL BUTTON ===
            Calculator.Operator.EQUALS -> {
                if (DEBUG) Log.d("SOJO Debug:", "equal() -> WHEN(operator = EQUALS)")

                if (currentOperator == previousOperator) {
                    if (DEBUG) Log.d("SOJO Debug:", "equal() -> WHEN(operator = EQUALS) -> IF")

                    if (previousOperator == Calculator.Operator.NONE && firstValue == secondValue || currentOperator == previousOperator) {
                        if (previousOperator == Calculator.Operator.EQUALS && currentOperator == previousOperator) {
                            if (historyItemToAdd.isNotBlank()) {
                                if (historyItemToAdd.trim()[0] == '=')
                                    addHistory("$input$historyItemToAdd")
                                else
                                    addHistory(historyItemToAdd)

                                historyItemToAdd = ""
                            }

                            val splitResult = tvPreviousResult?.text.toString().split(" ")

                            if (splitResult.size > 1) {
                                secondValue = splitResult[splitResult.size - 1].tryParse()

                                previousOperator = oldPreviousOperator

                                result = BigDecimal(
                                    Calculator.calculate(
                                        firstValue,
                                        secondValue,
                                        oldPreviousOperator
                                    )
                                ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                                    .stripTrailingZeros()

                                displayResult(
                                    firstValue.toString(),
                                    secondValue.toString(),
                                    oldPreviousOperator,
                                    result.toDouble()
                                )

                                historyItemToAdd =
                                    tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
                                        Calculator.Operator.EQUALS
                                    ) + " " + tvCurrentResult?.text.toString()
                            }
                        } else {
                            tvPreviousResult?.text = ""
                        }
                    } else {
                        if (historyItemToAdd.isNotBlank()) {
                            addHistory(historyItemToAdd)
                            historyItemToAdd = ""
                        }

                        val splitResult = tvPreviousResult?.text.toString().split(" ")

                        secondValue = splitResult[splitResult.size - 1].tryParse()

                        result = BigDecimal(
                            Calculator.calculate(
                                firstValue,
                                secondValue,
                                oldPreviousOperator
                            )
                        ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                            .stripTrailingZeros()

                        displayResult(
                            firstValue.toString(),
                            secondValue.toString(),
                            oldPreviousOperator,
                            result.toDouble()
                        )

                        historyItemToAdd =
                            tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
                                Calculator.Operator.EQUALS
                            ) + " " + tvCurrentResult?.text.toString()

                        previousOperator = oldPreviousOperator
                    }
                } else if (previousOperator == Calculator.Operator.POWER) {
                    if (DEBUG) Log.d(
                        "SOJO Debug:",
                        "equal() -> WHEN(operator = EQUALS) -> IF ELSE IF (POWER)"
                    )

                    if (historyItemToAdd.isNotBlank()) {
                        addHistory(historyItemToAdd)
                        historyItemToAdd = ""
                    }

                    // Calculate and view the result
                    result = BigDecimal(
                        Calculator.calculate(
                            firstValue,
                            secondValue,
                            previousOperator
                        )
                    ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                        .stripTrailingZeros()


                    displayResult(
                        firstValue.toString(),
                        secondValue.toString(),
                        previousOperator,
                        result.toDouble(),
                    )

                    // Flag that a calculation has been made
                    hasResult = true

                    // To high or low number (NaN)
                    if (result <= BigDecimal(-1E+18).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN).stripTrailingZeros()) tvCurrentResult?.text =
                        NOT_A_NUMBER

                    historyItemToAdd =
                        tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
                            Calculator.Operator.EQUALS
                        ) + " " + tvCurrentResult?.text.toString()
                } else {
                    if (DEBUG) Log.d("SOJO Debug:", "equal() -> WHEN(operator = EQUALS) -> IF ELSE")

                    if (historyItemToAdd.isNotBlank()) {
                        if (historyItemToAdd.trim()[0] == '=')
                            addHistory(historyItemToAdd.replace("=", "").trim() + historyItemToAdd)
                        else
                            addHistory(historyItemToAdd)

                        historyItemToAdd = ""
                    }

                    if (previousOperator == Calculator.Operator.NONE) {
                        tvPreviousResult?.text = formatValue(tvCurrentResult?.text.toString())
                    } else {
                        if (historyItemToAdd.isNotBlank()) {
                            addHistory(historyItemToAdd)
                            historyItemToAdd = ""
                        }

                        result = BigDecimal(
                            Calculator.calculate(
                                firstValue,
                                secondValue,
                                previousOperator
                            )
                        ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                            .stripTrailingZeros()

                        // Display the result in previous result and set current result to zero
                        displayResult(
                            firstValue.toString(),
                            secondValue.toString(),
                            previousOperator,
                            result.toDouble()
                        )
                    }
                }

                // Set the result to the first value and reset the second value
                firstValue = result.toDouble()
                secondValue = 0.0

                hasResult = true

                historyItemToAdd =
                    tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
                        Calculator.Operator.EQUALS
                    ) + " " + tvCurrentResult?.text.toString()
            }

            // === PERCENT BUTTON ===
            Calculator.Operator.PERCENT -> {
                if (DEBUG) Log.d("SOJO Debug:", "equal() -> WHEN(operator = PERCENT)")

                if (historyItemToAdd.isNotBlank()) {
                    addHistory(historyItemToAdd)
                    historyItemToAdd = ""
                }

                tvPreviousResult?.text = buildString {
                    append(tvPreviousResult?.text.toString())
                    append(" ")
                    append(formatValue(secondValue.toString(), true))
                    append(Calculator.getOperatorSign(currentOperator))
                }

                secondValue = Calculator.calculate(secondValue, 0.0, currentOperator)
                result = BigDecimal(
                    Calculator.calculate(
                        firstValue,
                        secondValue,
                        previousOperator
                    )
                ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                    .stripTrailingZeros()

                // Use previous operator
                when (previousOperator) {
                    // === ADD BUTTON ===
                    Calculator.Operator.ADD -> result = BigDecimal(
                        Calculator.calculate(
                            firstValue,
                            1.0 + secondValue,
                            Calculator.Operator.MULTIPLY
                        )
                    ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                        .stripTrailingZeros()

                    // === SUBTRACT BUTTON ===
                    Calculator.Operator.SUBTRACT -> result = BigDecimal(
                        Calculator.calculate(
                            firstValue,
                            1.0 - secondValue,
                            Calculator.Operator.MULTIPLY
                        )
                    ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                        .stripTrailingZeros()

                    // === MULTIPLY BUTTON ===
                    Calculator.Operator.MULTIPLY -> result = BigDecimal(
                        Calculator.calculate(
                            firstValue,
                            secondValue,
                            Calculator.Operator.MULTIPLY
                        )
                    ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                        .stripTrailingZeros()

                    // === DIVIDE BUTTON ===
                    Calculator.Operator.DIVIDE -> result = BigDecimal(
                        Calculator.calculate(
                            firstValue,
                            secondValue,
                            Calculator.Operator.DIVIDE
                        )
                    ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                        .stripTrailingZeros()

                    else -> if (DEBUG) Log.d(
                        "SOJO Debug:",
                        "equal() -> WHEN(operator = PERCENT) -> Case Else: firstValue=$firstValue, secondValue = $secondValue result=$result, currentOperation=" + Calculator.getOperatorSign(
                            currentOperator
                        ) + ", previousOperation=" + Calculator.getOperatorSign(previousOperator)
                    )
                }

                // Display the result
                displayResult(
                    firstValue.toString(),
                    (secondValue * 100.0).toString(),
                    previousOperator,
                    result.toDouble(),
                    " ",
                    " ",
                    Calculator.getOperatorSign(currentOperator)
                )

                // FLag that a calculation has been done
                hasResult = true

                resetValues()

                historyItemToAdd =
                    tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
                        Calculator.Operator.EQUALS
                    ) + " " + tvCurrentResult?.text.toString()
            }

            // === SQUARE AND CUBE BUTTON ===
            Calculator.Operator.SQUARE, Calculator.Operator.CUBE -> {
                    if (previousOperator == Calculator.Operator.EQUALS) {
                    if (historyItemToAdd.isNotBlank()) {
                        addHistory(historyItemToAdd)
                        historyItemToAdd = ""
                    }

                    result = BigDecimal(Calculator.calculate(firstValue, 0.0, currentOperator)).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN).stripTrailingZeros()

                    val powerSign = when (operator) {
                        Calculator.Operator.SQUARE -> "²"
                        Calculator.Operator.CUBE -> "³"
                        else -> ""
                    }

                    displayResult(
                        firstValue.toString(),
                        "",
                        Calculator.Operator.NONE,
                        result.toDouble(),
                        " ",
                        powerSign
                    )

                    // Flag that a calculation has been made
                    hasResult = true

                    historyItemToAdd =
                        tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
                            Calculator.Operator.EQUALS
                        ) + " " + tvCurrentResult?.text.toString()

                    resetValues()
                } else {
                    val powerCalculate = BigDecimal(when (operator) {
                        Calculator.Operator.SQUARE -> secondValue * secondValue
                        Calculator.Operator.CUBE -> secondValue * secondValue * secondValue
                        else -> 0.0
                    }).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN).stripTrailingZeros()

                    // Calculate and view the result
                    result = BigDecimal(
                        Calculator.calculate(
                            firstValue,
                            powerCalculate.toDouble(),
                            previousOperator
                        )
                    ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                        .stripTrailingZeros()

                        val powerSign = when (operator) {
                            Calculator.Operator.SQUARE -> "²"
                            Calculator.Operator.CUBE -> "³"
                        else -> ""
                    }

                    displayResult(
                        firstValue.toString(),
                        secondValue.toString(),
                        previousOperator,
                        result.toDouble(),
                        " ",
                        " ",
                        powerSign
                    )

                    // Flag that a calculation has been made
                    hasResult = true

                    historyItemToAdd =
                        tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
                            Calculator.Operator.EQUALS
                        ) + " " + tvCurrentResult?.text.toString()

                    // Reset variables
                    resetValues()
                }
            }

            //=== SQUARE AND CUBE ROOT BUTTON ===
            Calculator.Operator.SQUARE_ROOT, Calculator.Operator.CUBE_ROOT -> {
                if (previousOperator == Calculator.Operator.EQUALS)
                {
                    if (historyItemToAdd.isNotBlank()) {
                        addHistory(historyItemToAdd)
                        historyItemToAdd = ""
                    }

                    result = BigDecimal(Calculator.calculate(firstValue, 0.0, currentOperator)).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN).stripTrailingZeros()

                    displayResult(
                        firstValue.toString(),
                        "",
                        Calculator.Operator.NONE,
                        result.toDouble(),
                        Calculator.getOperatorSign(currentOperator)
                    )

                    // Flag that a calculation has been made
                    hasResult = true

                    historyItemToAdd =
                        tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
                            Calculator.Operator.EQUALS
                        ) + " " + tvCurrentResult?.text.toString()

                    // Reset variables
                    resetValues()
                } else {
                    val powerCalculate = BigDecimal(when (operator) {
                        Calculator.Operator.SQUARE_ROOT -> sqrt(secondValue)
                        Calculator.Operator.CUBE_ROOT -> cbrt(secondValue)
                        else -> 0.0
                    }).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN).stripTrailingZeros()

                    // Calculate and view the result
                    result = BigDecimal(
                        Calculator.calculate(
                            firstValue,
                            powerCalculate.toDouble(),
                            previousOperator
                        )
                    ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                        .stripTrailingZeros()

                    tvPreviousResult?.text = buildString {
                        append(formatValue(firstValue.toString()))
                        append(" ")
                        append(Calculator.getOperatorSign(previousOperator))
                        append(" ")
                        append(Calculator.getOperatorSign(currentOperator))
                        append(formatValue(secondValue.toString()))
                    }
                    tvCurrentResult?.text = formatValue(result.toString())


                    // Flag that a calculation has been made
                    hasResult = true

                    historyItemToAdd =
                        tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
                            Calculator.Operator.EQUALS
                        ) + " " + tvCurrentResult?.text.toString()

                    // Reset variables
                    resetValues()
                }
            }

            // === RECIPROCAL BUTTON ===
            Calculator.Operator.RECIPROCAL -> {
                if (previousOperator == Calculator.Operator.EQUALS) {
                    if (historyItemToAdd.isNotBlank()) {
                        addHistory(historyItemToAdd)
                        historyItemToAdd = ""
                    }

                    // Calculate and view the result
                    result = BigDecimal(
                        Calculator.calculate(
                            firstValue,
                            1.0 / secondValue,
                            currentOperator
                        )
                    ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                        .stripTrailingZeros()

                    displayResult(
                        "1",
                        firstValue.toString(),
                        Calculator.Operator.DIVIDE,
                        result.toDouble()
                    )

                    // Flag that a calculation has been made
                    hasResult = true

                    historyItemToAdd =
                        tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
                            Calculator.Operator.EQUALS
                        ) + " " + tvCurrentResult?.text.toString()

                    // Reset variables
                    resetValues()
                } else {
                    if (historyItemToAdd.isNotBlank()) {
                        addHistory(historyItemToAdd)
                        historyItemToAdd = ""
                    }

                    // Calculate and view the result
                    result = BigDecimal(
                        Calculator.calculate(
                            firstValue,
                            1.0 / secondValue,
                            previousOperator
                        )
                    ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                        .stripTrailingZeros()
                    displayResult(
                        firstValue.toString(),
                        (1.0 / secondValue).toString(),
                        previousOperator,
                        result.toDouble()
                    )

                    // Flag that a calculation has been made
                    hasResult = true

                    // Reset variables
                    resetValues()

                    historyItemToAdd =
                        tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
                            Calculator.Operator.EQUALS
                        ) + " " + tvCurrentResult?.text.toString()
                }
            }

            else -> {
                if (DEBUG) Log.d("SOJO Debug:", "equal() -> WHEN(operator -> ELSE)")

                if (previousOperator == Calculator.Operator.EQUALS) {
                    if (DEBUG) Log.d("SOJO Debug:", "equal() -> WHEN(operator -> ELSE) -> IF")

                    if (tvCurrentResult?.text.toString() == NOT_A_NUMBER)
                        firstValue = 0.0

                    displayResult(firstValue.toString(), "", currentOperator, 0.0)

                    secondValue = 0.0
                } else {
                    if (DEBUG) Log.d("SOJO Debug:", "equal() -> WHEN(operator -> ELSE) -> IF ELSE")

                    if (historyItemToAdd.isNotBlank()) {
                        addHistory(historyItemToAdd)
                        historyItemToAdd = ""
                    }

                    result = BigDecimal(
                        if (currentOperator == Calculator.Operator.POWER)
                            0.0
                        else
                            Calculator.calculate(firstValue, secondValue, previousOperator)
                    ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN).stripTrailingZeros()

                    // Display the result in previous result and set current result to zero
                    if (currentOperator == Calculator.Operator.POWER) {
                        displayResult(
                            firstValue.toString(),
                            secondValue.toString(),
                            previousOperator,
                            0.0,
                            " ",
                            " ",
                            " ^"
                        )
                    } else {
                        displayResult(result.toString(), "", currentOperator, 0.0)

                        historyItemToAdd = formatValue(firstValue.toString()) + " " + Calculator.getOperatorSign(previousOperator) + " " + formatValue(secondValue.toString()) +  " " + Calculator.getOperatorSign(
                            Calculator.Operator.EQUALS) + " " + formatValue(result.toString())

                    }

                    // Set the result to the first value and reset the second value
                    firstValue = result.toDouble()
                    secondValue = 0.0
                }
            }
        }
    } else {
        if (DEBUG) Log.d("SOJO Debug:", "equal() -> IF START ELSE")

        if (operator == Calculator.Operator.EQUALS) {
            if (DEBUG) Log.d("SOJO Debug:", "equal() -> IF START ELSE -> IF (operator = EQUALS)")

            if (secondValue == 0.0) {
                if (DEBUG) Log.d("SOJO Debug:", "equal() -> IF START ELSE -> IF (operator = EQUALS) -> IF")

                if (previousOperator == Calculator.Operator.POWER) {
                    if (DEBUG) Log.d("SOJO Debug:", "equal() -> IF START ELSE -> IF (operator = EQUALS) -> IF -> IF (POWER)")

                    if (historyItemToAdd.isNotBlank()) {
                        addHistory(historyItemToAdd)
                        historyItemToAdd = ""
                    }

                    val currentResultText = tvPreviousResult?.text.toString()
                    val splitResult = currentResultText.split(" ")

                    try {
                        val value1 = splitResult[0].tryParse()
                        val operator1 = Calculator.getOperator(splitResult[1])
                        val value2 = splitResult[2].tryParse()
                        val operator2 = Calculator.Operator.POWER
                        val value3 = firstValue

                        val powerResult = BigDecimal(Calculator.calculate(value2, value3, operator2)).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN).stripTrailingZeros()
                        result = BigDecimal(Calculator.calculate(value1, powerResult.toDouble(), operator1)).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN).stripTrailingZeros()

                        displayResult(value1.toString(), powerResult.toString(), operator1, result.toDouble())

                        tvPreviousResult?.text = buildString {
                            append(currentResultText)
                            append(" ")
                            append(formatValue(firstValue.toString()))
                        }

                        historyItemToAdd = tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
                            Calculator.Operator.EQUALS) + " " + tvCurrentResult?.text.toString()

                        firstValue = result.toDouble()

                        hasResult = true
                    } catch (e: Exception) {
                        if (DEBUG) Log.d("SOJO Debug:", "catch: $e")

                        firstValue = splitResult[0].tryParse()
                        secondValue = tvCurrentResult?.text.toString().tryParse()

                        result = BigDecimal(
                            if (oldFirstValue != 0.0 || currentOperator == Calculator.Operator.EQUALS)
                                firstValue.pow(secondValue)
                            else
                                0.0
                        ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN).stripTrailingZeros()

                        if (firstValue != 0.0 && secondValue != 0.0) {
                            displayResult(
                                firstValue.toString(),
                                secondValue.toString(),
                                previousOperator,
                                result.toDouble()
                            )

                            hasResult = true
                        } else {
                            if (firstValue > 0 || secondValue > 0) {
                                displayResult(
                                    firstValue.toString(),
                                    secondValue.toString(),
                                    previousOperator,
                                    result.toDouble()
                                )

                                hasResult = true
                            } else {
                                tvPreviousResult?.text = "0 ^ 0"
                                tvCurrentResult?.text = NOT_A_NUMBER

                                resetValues()
                                hasResult = false
                            }
                        }
                    }

                    historyItemToAdd =
                        tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
                            Calculator.Operator.EQUALS
                        ) + " " + tvCurrentResult?.text.toString()
                } else {
                    if (DEBUG) Log.d("SOJO Debug:", "equal() -> IF START ELSE -> IF (operator = EQUALS) -> IF -> IF (POWER) ELSE")

                    if (previousOperator == Calculator.Operator.NONE && firstValue == secondValue || currentOperator == previousOperator) {
                        tvPreviousResult?.text = ""
                    } else {
                        if (historyItemToAdd.isNotBlank()) {
                            addHistory(historyItemToAdd)
                            historyItemToAdd = ""
                        }

                        if (tvPreviousResult?.text.toString().contains(Calculator.getOperatorSign(Calculator.Operator.POWER)) && firstValue < 0) {
                            if (historyItemToAdd.isNotBlank()) {
                                addHistory(historyItemToAdd)
                                historyItemToAdd = ""
                            }

                            val currentResultText = tvPreviousResult?.text.toString()
                            val splitResult = currentResultText.split(" ")

                            try {
                                val value1 = splitResult[0].tryParse()
                                val operator1 = Calculator.getOperator(splitResult[1])
                                val value2 = splitResult[2].tryParse()
                                val operator2 = Calculator.Operator.POWER
                                val value3 = firstValue

                                val powerResult = BigDecimal(Calculator.calculate(value2, value3, operator2)).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN).stripTrailingZeros()
                                result = BigDecimal(Calculator.calculate(value1, powerResult.toDouble(), operator1)).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN).stripTrailingZeros()

                                displayResult(
                                    value1.toString(),
                                    powerResult.toString(),
                                    operator1,
                                    result.toDouble()
                                )

                                if (result <= (-999999999999999999).toBigDecimal())
                                    tvCurrentResult?.text = NOT_A_NUMBER

                                tvPreviousResult?.text = buildString {
                                    append(currentResultText)
                                    append(" ")
                                    append(formatValue(firstValue.toString()))
                                }

                                historyItemToAdd =
                                    tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
                                        Calculator.Operator.EQUALS
                                    ) + " " + tvCurrentResult?.text.toString()

                                firstValue = result.toDouble()

                                hasResult = true
                            } catch (e: Exception) {
                                if (DEBUG) Log.d("SOJO Debug:", "catch: $e")

                                firstValue = splitResult[0].tryParse()
                                secondValue = tvCurrentResult?.text.toString().tryParse()

                                result = BigDecimal(
                                    if (firstValue != 0.0)
                                        firstValue.pow(secondValue)
                                    else
                                        0.0
                                ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN).stripTrailingZeros()
                                displayResult(firstValue.toString(), secondValue.toString(),Calculator.Operator.POWER, result.toDouble())

                                if (firstValue == 0.0 && secondValue < 0)
                                    tvCurrentResult?.text = NOT_A_NUMBER

                                historyItemToAdd =
                                    tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
                                        Calculator.Operator.EQUALS
                                    ) + " " + tvCurrentResult?.text.toString()

                                hasResult = true
                            }
                        } else {
                            result = BigDecimal(
                                if (previousOperator != Calculator.Operator.NONE) {
                                    if (previousOperator == Calculator.Operator.POWER && firstValue == 0.0)
                                        0.0
                                    else
                                        Calculator.calculate(
                                            firstValue,
                                            secondValue,
                                            previousOperator
                                        )
                                } else {
                                    firstValue
                                }
                            ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                                .stripTrailingZeros()

                            displayResult(
                                firstValue.toString(),
                                if (previousOperator != Calculator.Operator.NONE) secondValue.toString() else "",
                                if (previousOperator != Calculator.Operator.NONE) previousOperator else Calculator.Operator.NONE,
                                result.toDouble()
                            )

                            resetValues()

                            hasResult = true

                            historyItemToAdd =
                                tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
                                    Calculator.Operator.EQUALS
                                ) + " " + tvCurrentResult?.text.toString()
                        }
                    }
                }
            } else {
                if (DEBUG) Log.d("SOJO Debug:", "equal() -> IF START ELSE -> IF (operator = EQUALS) -> IF ELSE")

                result = BigDecimal(Calculator.calculate(firstValue, secondValue, previousOperator)).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN).stripTrailingZeros()

                // Display the result in previous result and set current result to zero
                displayResult(
                    result.toString(),
                    secondValue.toString(),
                    previousOperator,
                    result.toDouble()
                )

                resetValues()

                hasResult = true
            }
        } else {
            if (DEBUG) Log.d("SOJO Debug:", "equal() -> IF START ELSE -> IF (operator = EQUALS) ELSE")

            when (currentOperator) {
                // === PERCENT OPERATION ===
                Calculator.Operator.PERCENT -> {
                    if (DEBUG) Log.d("SOJO Debug:", "equal() -> IF START ELSE -> IF (operator = EQUALS) ELSE -> when(PERCENT)")

                    result = BigDecimal(firstValue / 100).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN).stripTrailingZeros()

                    tvPreviousResult?.text = buildString {
                        append(formatValue(firstValue.toString()))
                        append(Calculator.getOperatorSign(currentOperator))
                    }

                    tvCurrentResult?.text = formatValue(result.toString())

                    historyItemToAdd = tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
                        Calculator.Operator.EQUALS) + " " + tvCurrentResult?.text.toString()

                    resetValues()

                    hasResult = true
                }

                // === SQUARE AND CUBE OPERATION ===
                Calculator.Operator.SQUARE, Calculator.Operator.CUBE -> {
                    if (DEBUG) Log.d("SOJO Debug:", "equal() -> IF START ELSE -> IF (operator = EQUALS) ELSE -> when(SQUARE, CUBE)")

                    if (historyItemToAdd.isNotBlank()) {
                        addHistory(historyItemToAdd)
                        historyItemToAdd = ""
                    }

                    result = BigDecimal(Calculator.calculate(firstValue, 0.0, currentOperator)).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN).stripTrailingZeros()

                    val powerSign = when (currentOperator) {
                        Calculator.Operator.SQUARE -> "²"
                        Calculator.Operator.CUBE -> "³"
                        else -> ""
                    }

                    displayResult(
                        firstValue.toString(),
                        "",
                        Calculator.Operator.NONE,
                        result.toDouble(),
                        "",
                        powerSign
                    )

                    // Flag that a calculation has been made
                    hasResult = true

                    resetValues()

                    historyItemToAdd = tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
                        Calculator.Operator.EQUALS) + " " + tvCurrentResult?.text.toString()
                }

                // === SQUARE AND CUBE ROOT OPERATION ===
                Calculator.Operator.SQUARE_ROOT, Calculator.Operator.CUBE_ROOT -> {
                    if (historyItemToAdd.isNotBlank()) {
                        addHistory(historyItemToAdd)
                        historyItemToAdd = ""
                    }

                    if (firstValue == 0.0) {
                        tvCurrentResult?.text = "0"
                        tvPreviousResult?.text = ""
                    } else {
                        // Calculate the result
                        result = BigDecimal(Calculator.calculate(firstValue, firstValue, currentOperator)).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN).stripTrailingZeros()

                        // Display the result
                        tvPreviousResult?.text = buildString {
                            append(Calculator.getOperatorSign(currentOperator))
                            append(formatValue(firstValue.toString()))
                        }
                        tvCurrentResult?.text = formatValue(result.toString())

                        // Flag that a calculation has been made
                        hasResult = true
                    }

                    historyItemToAdd = tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
                        Calculator.Operator.EQUALS) + " " + tvCurrentResult?.text.toString()

                    // Reset variables
                    resetValues()
                }

                // === RECIPROCAL OPERATION ===
                Calculator.Operator.RECIPROCAL -> {
                    if (tvPreviousResult?.text.toString()
                            .contains(Calculator.getOperatorSign(Calculator.Operator.POWER)) && firstValue < 0
                    ) {
                        if (historyItemToAdd.isNotBlank()) {
                            addHistory(historyItemToAdd)
                            historyItemToAdd = ""
                        }

                        val currentResultText = tvPreviousResult?.text.toString()
                        val splitResult = currentResultText.split(" ")

                        try {
                            val value1 = splitResult[0].tryParse()
                            val operator1 = Calculator.getOperator(splitResult[1])
                            val value2 = splitResult[2].tryParse()
                            val operator2 = Calculator.Operator.POWER
                            val value3 = firstValue

                            val powerResult = BigDecimal(
                                Calculator.calculate(
                                    value2,
                                    value3,
                                    operator2
                                )
                            ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                                .stripTrailingZeros()
                            result = BigDecimal(
                                Calculator.calculate(
                                    value1,
                                    powerResult.toDouble(),
                                    operator1
                                )
                            ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                                .stripTrailingZeros()

                            addHistory(tvPreviousResult?.text.toString() + " " + value3.toString() + " " + Calculator.getOperatorSign(
                                Calculator.Operator.EQUALS
                            ) + " " + formatValue(result.toString()))

                            historyItemToAdd = ""

                            displayResult(
                                result.toString(),
                                "",
                                currentOperator,
                                0.0
                            )

                            firstValue = result.toDouble()

                            hasResult = true
                        } catch (e: Exception) {
                            if (DEBUG) Log.d("SOJO Debug:", "catch: $e")

                            firstValue = splitResult[0].tryParse()
                            secondValue = tvCurrentResult?.text.toString().tryParse()

                            if (secondValue < 0) {
                                result = BigDecimal(
                                    Calculator.calculate(
                                        firstValue,
                                        1.0 / secondValue,
                                        Calculator.Operator.POWER
                                    )
                                ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                                    .stripTrailingZeros()

                                historyItemToAdd =
                                    tvPreviousResult?.text.toString() + " " + (1 / secondValue).toString() + " " + Calculator.getOperatorSign(
                                        Calculator.Operator.EQUALS
                                    ) + " " + formatValue(result.toString())

                                displayResult(
                                    firstValue.toString(),
                                    (1 / secondValue).toString(),
                                    Calculator.Operator.POWER,
                                    result.toDouble()
                                )

                                resetValues()

                                hasResult = true
                            } else {
                                result = BigDecimal(
                                    if (firstValue != 0.0)
                                        firstValue.pow(secondValue)
                                    else
                                        0.0
                                ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                                    .stripTrailingZeros()

                                historyItemToAdd =
                                    tvPreviousResult?.text.toString() + " " + secondValue.toString() + " " + Calculator.getOperatorSign(
                                        Calculator.Operator.EQUALS
                                    ) + " " + formatValue(result.toString())

                                displayResult(
                                    result.toString(),
                                    "",
                                    oldPreviousOperator,
                                    0.0
                                )

                                firstValue = result.toDouble()

                                hasResult = true
                            }
                        }
                    } else {
                        if (firstValue == 0.0) {
                            tvCurrentResult?.text = "0"
                            tvPreviousResult?.text = ""
                        } else {
                            // Calculate and show the result
                            result = BigDecimal(
                                Calculator.calculate(
                                    1.0,
                                    firstValue,
                                    Calculator.Operator.DIVIDE
                                )
                            ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                                .stripTrailingZeros()
                            displayResult(
                                "1",
                                firstValue.toString(),
                                Calculator.Operator.DIVIDE,
                                result.toDouble()
                            )

                            // Flag that a calculation has been made
                            hasResult = true

                            historyItemToAdd =
                                tvPreviousResult?.text.toString() + " " + Calculator.getOperatorSign(
                                    Calculator.Operator.EQUALS
                                ) + " " + tvCurrentResult?.text.toString()
                        }

                        // Reset variables
                        resetValues()
                    }
                }
                else -> {
                    if (previousOperator == Calculator.Operator.POWER) {
                        val currentResultText = tvPreviousResult?.text.toString()
                        val splitResult = currentResultText.split(" ")

                        try {
                            val value1 = splitResult[0].tryParse()
                            val operator1 = Calculator.getOperator(splitResult[1])
                            val value2 = splitResult[2].tryParse()
                            val operator2 = Calculator.Operator.POWER
                            val value3 = firstValue

                            val powerResult = if (currentOperator != previousOperator && value3 != 0.0)
                                     BigDecimal(
                                        Calculator.calculate(
                                            value2,
                                            value3,
                                            operator2
                                        )
                                    ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                                        .stripTrailingZeros()
                                else
                                    1

                            result = BigDecimal(
                                Calculator.calculate(
                                    value1,
                                    powerResult.toDouble(),
                                    operator1
                                )
                            ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                                .stripTrailingZeros()

                            displayResult(result.toString(), "", currentOperator, 0.0)

                            addHistory(buildString {
                                append(currentResultText)
                                append(" ")
                                append(formatValue(firstValue.toString()))
                                append(" ")
                                append(Calculator.getOperatorSign(Calculator.Operator.EQUALS))
                                append(" ")
                                append(formatValue(result.toString()))
                            })

                            historyItemToAdd = ""

                            firstValue = result.toDouble()

                            hasResult = true
                        } catch (e: Exception) {
                            if (DEBUG) Log.d("SOJO Debug:", "catch: $e")

                            secondValue = firstValue
                            firstValue = splitResult[0].tryParse()
                            previousOperator = Calculator.getOperator(splitResult[1])

                            result = BigDecimal(
                                Calculator.calculate(
                                    firstValue,
                                    secondValue,
                                    previousOperator
                                )
                            ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                                .stripTrailingZeros()

                            if (firstValue != 0.0 && secondValue != 0.0) {
                                displayResult(result.toString(), "", currentOperator, 0.0)

                                firstValue = result.toDouble()

                                hasResult = true
                            } else {
                                tvPreviousResult?.text = "0 ^ 0"
                                tvCurrentResult?.text = NOT_A_NUMBER

                                hasResult = false
                                resetValues()
                            }

                            addHistory(buildString {
                                append(currentResultText)
                                append(" ")
                                append(formatValue(firstValue.toString()))
                                append(" ")
                                append(Calculator.getOperatorSign(Calculator.Operator.EQUALS))
                                append(" ")
                                append(formatValue(result.toString()))
                            })

                            historyItemToAdd = ""
                        }
                    } else {
                        if (currentOperator == Calculator.Operator.POWER && previousOperator != Calculator.Operator.NONE) {
                            if (historyItemToAdd.isNotBlank()) {
                                addHistory(historyItemToAdd)
                                historyItemToAdd = ""
                            }

                            result = BigDecimal(
                                if (currentOperator == Calculator.Operator.POWER)
                                    0.0
                                else
                                    Calculator.calculate(firstValue, secondValue, previousOperator)
                            ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN).stripTrailingZeros()

                            // Display the result in previous result and set current result to zero
                            if (currentOperator == Calculator.Operator.POWER) {
                                displayResult(
                                    firstValue.toString(),
                                    secondValue.toString(),
                                    previousOperator,
                                    0.0,
                                    " ",
                                    " ",
                                    " ^"
                                )

                                tvPreviousResult?.text = tvPreviousResult?.text.toString().replace("= 0 " + Calculator.getOperatorSign(Calculator.Operator.POWER),
                                    Calculator.getOperatorSign(Calculator.Operator.POWER))
                            } else {
                                displayResult(result.toString(), "", currentOperator, 0.0)

                                historyItemToAdd = formatValue(firstValue.toString()) + " " + Calculator.getOperatorSign(previousOperator) + " " + formatValue(secondValue.toString()) +  " " + Calculator.getOperatorSign(
                                    Calculator.Operator.EQUALS) + " " + formatValue(result.toString())
                            }

                            // Set the result to the first value and reset the second value
                            firstValue = result.toDouble()
                            secondValue = 0.0
                        } else {
                            if (tvPreviousResult?.text.toString()
                                    .contains(Calculator.getOperatorSign(Calculator.Operator.POWER)) && firstValue < 0
                            ) {
                                if (historyItemToAdd.isNotBlank()) {
                                    addHistory(historyItemToAdd)
                                    historyItemToAdd = ""
                                }

                                val currentResultText = tvPreviousResult?.text.toString()
                                val splitResult = currentResultText.split(" ")

                                try {
                                    val value1 = splitResult[0].tryParse()
                                    val operator1 = Calculator.getOperator(splitResult[1])
                                    val value2 = splitResult[2].tryParse()
                                    val operator2 = Calculator.Operator.POWER
                                    val value3 = firstValue

                                    val powerResult = BigDecimal(
                                        Calculator.calculate(
                                            value2,
                                            value3,
                                            operator2
                                        )
                                    ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                                        .stripTrailingZeros()
                                    result = BigDecimal(
                                        Calculator.calculate(
                                            value1,
                                            powerResult.toDouble(),
                                            operator1
                                        )
                                    ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                                        .stripTrailingZeros()

                                    addHistory(tvPreviousResult?.text.toString() + " " + value3.toString() + " " + Calculator.getOperatorSign(
                                        Calculator.Operator.EQUALS
                                    ) + " " +
                                            if (result <= (-999999999999999999).toBigDecimal())
                                                NOT_A_NUMBER else formatValue(result.toString()))

                                    historyItemToAdd = ""

                                    displayResult(
                                        result.toString(),
                                        "",
                                        currentOperator,
                                        0.0
                                    )

                                    if (result <= (-999999999999999999).toBigDecimal()) {
                                        tvPreviousResult?.text = NOT_A_NUMBER
                                        tvCurrentResult?.text = "0"

                                        firstValue = 0.0
                                    } else
                                        firstValue = result.toDouble()

                                    hasResult = true
                                } catch (e: Exception) {
                                    if (DEBUG) Log.d("SOJO Debug:", "catch: $e")

                                    firstValue = splitResult[0].tryParse()
                                    secondValue = tvCurrentResult?.text.toString().tryParse()

                                    result = BigDecimal(
                                        if (firstValue != 0.0)
                                            firstValue.pow(secondValue)
                                        else
                                            0.0
                                    ).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                                        .stripTrailingZeros()

                                    if (firstValue == 0.0 && secondValue < 0) {
                                        historyItemToAdd =
                                            tvPreviousResult?.text.toString() + " " + secondValue.toString() + " " + Calculator.getOperatorSign(
                                                Calculator.Operator.EQUALS
                                            ) + " " + NOT_A_NUMBER

                                        tvPreviousResult?.text = NOT_A_NUMBER
                                        tvCurrentResult?.text = "0"

                                        firstValue = 0.0
                                        secondValue = 0.0

                                        hasResult = false
                                    } else {
                                        displayResult(
                                            result.toString(),
                                            "",
                                            currentOperator,
                                            0.0
                                        )

                                        historyItemToAdd =
                                            tvPreviousResult?.text.toString() + " " + secondValue.toString() + " " + Calculator.getOperatorSign(
                                                Calculator.Operator.EQUALS
                                            ) + " " + formatValue(result.toString())

                                        firstValue = result.toDouble()

                                        hasResult = true
                                    }
                                }
                            } else {
                                displayResult(
                                    firstValue.toString(),
                                    "",
                                    currentOperator,
                                    result.toDouble()
                                )
                            }
                        }
                    }
                }
            }
        }
    }
}


// ========= RESET VALUES =========
private fun MainActivity.resetValues() {
    firstValue = 0.0
    secondValue = 0.0
    currentOperator = Calculator.Operator.NONE
    previousOperator = Calculator.Operator.NONE
}


