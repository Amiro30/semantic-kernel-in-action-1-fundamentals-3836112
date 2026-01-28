using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace _03_05b;


public class MyMathPlugin
{
    [KernelFunction, Description ("take the square root of a number.")]
    public static double Sqrt(double number)
    {
        return Math.Sqrt(number);   
    }
}