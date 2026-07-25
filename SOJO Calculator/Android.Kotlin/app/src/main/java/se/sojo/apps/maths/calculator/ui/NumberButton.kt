package se.sojo.apps.maths.calculator.ui

import android.annotation.SuppressLint
import android.util.Log
import android.view.MotionEvent
import android.widget.Button
import se.sojo.apps.maths.calculator.core.Calculator.Companion.DECIMAL_SEPARATOR
import se.sojo.apps.maths.calculator.core.Calculator.Companion.MAX_LENGTH
import se.sojo.apps.maths.calculator.MainActivity
import se.sojo.apps.maths.calculator.MainActivity.Companion.DEBUG
import se.sojo.apps.maths.calculator.R
import se.sojo.apps.maths.calculator.core.Calculator
import se.sojo.apps.maths.calculator.core.performHapticFeedback

// Number buttons
@SuppressLint("ClickableViewAccessibility")
fun MainActivity.setNumberButtons() {
    btnOne = findViewById(R.id.btn_one)
    btnTwo = findViewById(R.id.btn_two)
    btnThree = findViewById(R.id.btn_three)
    btnFour = findViewById(R.id.btn_four)
    btnFive = findViewById(R.id.btn_five)
    btnSix = findViewById(R.id.btn_six)
    btnSeven = findViewById(R.id.btn_seven)
    btnEight = findViewById(R.id.btn_eight)
    btnNine = findViewById(R.id.btn_nine)
    btnZero = findViewById(R.id.btn_zero)
    btnDot = findViewById(R.id.btn_dot)

    /******************************************************
     * Number Buttons onClick
     */
    btnOne?.setOnTouchListener { _, motionEvent -> numberButtonAction(btnOne!!, motionEvent) }
    btnTwo?.setOnTouchListener { _, motionEvent -> numberButtonAction(btnTwo!!, motionEvent) }
    btnThree?.setOnTouchListener { _, motionEvent -> numberButtonAction(btnThree!!, motionEvent) }
    btnFour?.setOnTouchListener { _, motionEvent -> numberButtonAction(btnFour!!, motionEvent) }
    btnFive?.setOnTouchListener { _, motionEvent -> numberButtonAction(btnFive!!, motionEvent) }
    btnSix?.setOnTouchListener { _, motionEvent -> numberButtonAction(btnSix!!, motionEvent) }
    btnSeven?.setOnTouchListener { _, motionEvent -> numberButtonAction(btnSeven!!, motionEvent) }
    btnEight?.setOnTouchListener { _, motionEvent -> numberButtonAction(btnEight!!, motionEvent) }
    btnNine?.setOnTouchListener { _, motionEvent -> numberButtonAction(btnNine!!, motionEvent) }
    btnZero?.setOnTouchListener { _, motionEvent -> numberButtonAction(btnZero!!, motionEvent) }
    btnDot?.setOnTouchListener { _, motionEvent -> numberButtonAction(btnDot!!, motionEvent) }
}

// ========= NUMBER BUTTON ACTIONS =========if (DEBUG)
private fun MainActivity.numberButtonAction(btn: Button, motionEvent: MotionEvent): Boolean {
    if (DEBUG) Log.d("SOJO Debug:",
        "numberButtonAction -> $firstValue, $secondValue, $currentOperator, $previousOperator"
    )

    when (motionEvent.action) {
        MotionEvent.ACTION_UP -> {
            // Reset the screen if a result has been calculated
            if (hasResult || hasMemoryRecall) {
                if (DEBUG) Log.d("SOJO Debug:", "numberButtonAction -> IF START (hasResult || hasMemoryRecall)")

                tvCurrentResult?.text = "0"
                hasResult = false
                hasMemoryRecall = false

                when (currentOperator) {
                    Calculator.Operator.NONE if previousOperator == Calculator.Operator.NONE -> {
                        if (DEBUG) Log.d(
                            "SOJO Debug:",
                            "numberButtonAction -> IF START (hasResult || hasMemoryRecall) -> IF"
                        )

                        tvPreviousResult?.text = ""
                    }
                    Calculator.Operator.EQUALS -> {
                        if (DEBUG) Log.d(
                            "SOJO Debug:",
                            "numberButtonAction -> IF START (hasResult || hasMemoryRecall) -> IF ELSE"
                        )

                        firstValue = 0.0
                        secondValue = 0.0
                        currentOperator = Calculator.Operator.NONE
                        previousOperator = Calculator.Operator.NONE

                        tvPreviousResult?.text = ""
                    }
                    else -> {
                        if (DEBUG) Log.d(
                            "SOJO Debug:",
                            "numberButtonAction -> IF START (hasResult || hasMemoryRecall) -> IF -> ELSE"
                        )

                        tvPreviousResult?.text = buildString {
                            append(Calculator.formatValue(firstValue.toString(), true))
                            append(" ")
                            append(Calculator.getOperatorSign(currentOperator))
                        }
                    }
                }
            }

            // Check if the current number is not longer than the MAX_LENGTH
            tvCurrentResult?.text?.length?.let {
                if (it >= MAX_LENGTH) return performHapticFeedback(btn, motionEvent)
            }

            // Check if a period exist and if not check if its a empty string and if so add a zero before the period
            if (btn.text == DECIMAL_SEPARATOR) {
                if (DEBUG) Log.d("SOJO Debug:", "numberButtonAction -> IF START (BTN == DECIMAL_SEPARATOR")

                if (tvCurrentResult?.text?.contains(DECIMAL_SEPARATOR) == true)
                    return performHapticFeedback(btn, motionEvent)

                if (tvCurrentResult?.text == "")
                    tvCurrentResult!!.text = "0"
            }

            // Add key to the value
            if (btn.text.toString() != DECIMAL_SEPARATOR || !(tvCurrentResult?.text.toString().contains(DECIMAL_SEPARATOR))) { // } || tvCurrentResult?.text.toString().contains(DECIMAL_SEPARATOR))) {
                if (DEBUG) Log.d("SOJO Debug:", "numberButtonAction -> IF START (BTN != DECIMAL_SEPARATOR || !CurrentResult.contains(DECIMAL_SEPARATOR))")

                tvCurrentResult?.append(btn.text)
            }

            // Format the current result
            if (btn.text.toString() == DECIMAL_SEPARATOR) {
                if (DEBUG) Log.d("SOJO Debug:", "numberButtonAction -> IF START (BTN == DECIMAL_SEPARATOR)")

                if (!tvCurrentResult?.text.toString().contains(DECIMAL_SEPARATOR))

                if (DEBUG) Log.d("SOJO Debug:", "numberButtonAction -> IF START (BTN == DECIMAL_SEPARATOR) -> IF")

                tvCurrentResult?.text = buildString {
                    append(
                        tvCurrentResult?.text.toString()
                            .substring(0, tvCurrentResult?.text.toString().length - 1)
                    )
                    append(DECIMAL_SEPARATOR)
                }
            } else {
                if (DEBUG) Log.d("SOJO Debug:", "numberButtonAction -> IF START (BTN == DECIMAL_SEPARATOR) -> ELSE")

                if (tvCurrentResult?.text.toString()[tvCurrentResult?.text.toString().length -1] == '0' && tvCurrentResult?.text.toString().contains(DECIMAL_SEPARATOR)) {
                    if (DEBUG) Log.d("SOJO Debug:", "numberButtonAction -> IF START (BTN == DECIMAL_SEPARATOR) -> ELSE -> IF")

                    val splitCurrentResult = tvCurrentResult?.text.toString().split(DECIMAL_SEPARATOR)

                    tvCurrentResult?.text = try {
                        Calculator.formatValue(splitCurrentResult[0])
                    } catch (_: Exception) {
                        Calculator.formatValue(tvCurrentResult?.text.toString())
                    }

                    tvCurrentResult?.text = buildString {
                        append(tvCurrentResult?.text.toString())
                        append(
                            try {
                                DECIMAL_SEPARATOR + splitCurrentResult[1]
                            } catch (_: Exception) {
                                ""
                            }
                        )
                    }
                } else {
                    if (DEBUG) Log.d("SOJO Debug:", "numberButtonAction -> IF START (BTN == DECIMAL_SEPARATOR) -> ELSE -> IF ELSE")

                    tvCurrentResult?.text = Calculator.formatValue(tvCurrentResult!!.text.toString())
                }
            }

            // Adjust the font and decrease it if needed
            //adjustLabelFont(tvCurrentResult)

            // Set the status of Clear Entry to false
            clearEntry = false
        }
    }

    return performHapticFeedback(btn, motionEvent)
}