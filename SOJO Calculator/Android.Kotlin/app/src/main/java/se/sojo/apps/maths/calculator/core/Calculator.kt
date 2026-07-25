package se.sojo.apps.maths.calculator.core

import android.util.Log
import se.sojo.apps.maths.calculator.MainActivity.Companion.DEBUG
import java.math.BigDecimal
import java.math.RoundingMode
import java.text.NumberFormat
import java.util.Locale
import kotlin.math.PI
import kotlin.math.cbrt
import kotlin.math.pow
import kotlin.math.sqrt

class Calculator {
    enum class Operator {
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
        EQUALS,
        NONE
    }

    @Suppress("DEPRECATION")
    companion object {
        var DECIMAL_SEPARATOR: String = "."
        var THOUSAND_SEPARATOR: String = ","

        var NOT_A_NUMBER = "NaN"

        // Max length of the display numbers
        const val MAX_LENGTH: Int = 24
        const val MAX_DECIMALS_AND_ZEROS = 10

        var HISTORY_ITEM_TEXT_SIZE = 20f

        var numberFormat: NumberFormat = NumberFormat.getNumberInstance(Locale("en", "US"))

        // Set the base font size
        const val BASE_FONT_SIZE: Float = 24f

        @JvmStatic
        fun getTextSize(): Float {
            return HISTORY_ITEM_TEXT_SIZE
        }

        fun getOperatorSign(op: Operator): String {
            return when (op) {
                Operator.ADD -> "+"
                Operator.SUBTRACT -> "-"
                Operator.MULTIPLY -> "x"
                Operator.DIVIDE -> "÷"
                Operator.MODULO -> "≡"
                Operator.PERCENT -> "%"
                Operator.SQUARE -> "x²"
                Operator.CUBE -> "x³"
                Operator.PI ->  "π"
                Operator.POWER -> "^"
                Operator.SQUARE_ROOT -> "√"
                Operator.CUBE_ROOT -> "∛"
                Operator.RECIPROCAL -> "1/x"
                Operator.RND -> "rnd"
                Operator.EQUALS -> "="
                Operator.NONE -> ""
            }
        }

        fun getOperator(key: String): Operator {
            return when (key) {
                "+" -> Operator.ADD
                "−", "-" -> Operator.SUBTRACT
                "x", "×" -> Operator.MULTIPLY
                "÷", "/" -> Operator.DIVIDE
                "mod", "≡" -> Operator.MODULO
                "%" -> Operator.PERCENT
                "x²" -> Operator.SQUARE
                "x³" -> Operator.CUBE
                "π" -> Operator.PI
                "^" -> Operator.POWER
                "√" -> Operator.SQUARE_ROOT
                "∛" -> Operator.CUBE_ROOT
                "1/x"-> Operator.RECIPROCAL
                "rnd" -> Operator.RND
                "=" -> Operator.EQUALS
                else -> Operator.NONE
            }
        }

        fun formatValue(input: String, forceFormat: Boolean = false): String {
            if (DEBUG) Log.d("SOJO Debug:", "formatValue -> $input, $forceFormat")

            val inputFixed = input.replace(NOT_A_NUMBER, "")

            val result = if (inputFixed.length > 1 && inputFixed.take(1) == "0" && !inputFixed.contains(DECIMAL_SEPARATOR)) {
                if (DEBUG) Log.d("SOJO Debug:", "formatValue -> IF START")

                inputFixed.cleanUpMinusSign().substring(1).cleanUpZeroValue()
            } else if (inputFixed.contains(DECIMAL_SEPARATOR)) {
                if (DEBUG) Log.d("SOJO Debug:", "formatValue -> IF START -> ELSE IF")

                if (forceFormat) {
                    if (DEBUG) Log.d("SOJO Debug:", "formatValue -> IF 1")

                    val cleanupValue = inputFixed.replace(160.toChar().toString(),"")
                    numberFormat.format(cleanupValue.cleanUpMinusSign().removeThousandSeparator().cleanUpDecimalSeparator().toDouble()).cleanUpZeroValue()
                } else {
                    if (DEBUG) Log.d("SOJO Debug:", "formatValue -> IF START -> ELSE IF -> ELSE")

                    //input.cleanUpMinusSign().cleanUpZeroValue()
                    numberFormat.format(inputFixed.cleanUpMinusSign().removeThousandSeparator().cleanUpDecimalSeparator().toDouble()).cleanUpZeroValue()
                }
            } else {
                if (DEBUG) Log.d("SOJO Debug:", "formatValue -> IF START -> ELSE")

                val cleanupValue = inputFixed.replace(160.toChar().toString(),"")
                numberFormat.format(cleanupValue.cleanUpMinusSign().removeThousandSeparator().cleanUpDecimalSeparator().toDouble()).cleanUpZeroValue()
            }

            return result.replace(NOT_A_NUMBER, "")
        }

        fun calculate(firstValue: Double, secondValue: Double, operation: Operator): Double {
            var result = BigDecimal(0) //0.0

            try {
                when (operation) {
                    Operator.ADD -> result = BigDecimal(firstValue + secondValue).setScale(
                        MAX_DECIMALS_AND_ZEROS,
                        RoundingMode.HALF_EVEN
                    ).stripTrailingZeros()

                    Operator.SUBTRACT -> result = BigDecimal(firstValue - secondValue).setScale(
                        MAX_DECIMALS_AND_ZEROS,
                        RoundingMode.HALF_EVEN
                    ).stripTrailingZeros()

                    Operator.MULTIPLY -> result = BigDecimal(firstValue * secondValue).setScale(
                        MAX_DECIMALS_AND_ZEROS,
                        RoundingMode.HALF_EVEN
                    ).stripTrailingZeros()

                    Operator.DIVIDE -> result = BigDecimal(firstValue / secondValue).setScale(
                        MAX_DECIMALS_AND_ZEROS,
                        RoundingMode.HALF_EVEN
                    ).stripTrailingZeros()

                    Operator.MODULO -> result = BigDecimal(firstValue % secondValue).setScale(
                        MAX_DECIMALS_AND_ZEROS,
                        RoundingMode.HALF_EVEN
                    ).stripTrailingZeros()

                    Operator.PERCENT -> result = BigDecimal(firstValue / 100).setScale(
                        MAX_DECIMALS_AND_ZEROS,
                        RoundingMode.HALF_EVEN
                    ).stripTrailingZeros()

                    Operator.SQUARE -> result = BigDecimal(firstValue * firstValue).setScale(
                        MAX_DECIMALS_AND_ZEROS,
                        RoundingMode.HALF_EVEN
                    ).stripTrailingZeros()

                    Operator.CUBE -> result =
                        BigDecimal(firstValue * firstValue * firstValue).setScale(
                            MAX_DECIMALS_AND_ZEROS,
                            RoundingMode.HALF_EVEN
                        ).stripTrailingZeros()

                    Operator.PI -> result =
                        BigDecimal(PI).setScale(MAX_DECIMALS_AND_ZEROS, RoundingMode.HALF_EVEN)
                            .stripTrailingZeros()

                    Operator.POWER -> result = BigDecimal(firstValue.pow(secondValue)).setScale(
                        MAX_DECIMALS_AND_ZEROS,
                        RoundingMode.HALF_EVEN
                    ).stripTrailingZeros()

                    Operator.SQUARE_ROOT -> result = BigDecimal(sqrt(firstValue)).setScale(
                        MAX_DECIMALS_AND_ZEROS,
                        RoundingMode.HALF_EVEN
                    ).stripTrailingZeros()

                    Operator.CUBE_ROOT -> result = BigDecimal(cbrt(firstValue)).setScale(
                        MAX_DECIMALS_AND_ZEROS,
                        RoundingMode.HALF_EVEN
                    ).stripTrailingZeros()

                    Operator.RECIPROCAL -> result = BigDecimal(1 / firstValue).setScale(
                        MAX_DECIMALS_AND_ZEROS,
                        RoundingMode.HALF_EVEN
                    ).stripTrailingZeros()

                    else -> if (DEBUG) Log.d("SOJO Debug:", "calculate -> Case Else")
                }
            } catch (e: Exception) {
                result = (-999999999999999999).toBigDecimal()
                if (DEBUG) Log.d("SOJO Debug:", "calculate -> catch: $e")
            }

            return result.toDouble()
        }
    }
}