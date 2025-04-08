using Evaluator.Logic;

try
{
    var resut2 = FunctionEvaluator.Evaluate("4*5/(4+6)");
    var resut1 = FunctionEvaluator.Evaluate("4*(5+6-(8/2^3)-7)-1");
    var resut3 = FunctionEvaluator.Evaluate("9^(1/2)");
    var resut4 = FunctionEvaluator.Evaluate("100+ 25*( 50 -20 ) / 10");
    Console.WriteLine(resut1);
    Console.WriteLine(resut2);
    Console.WriteLine(resut3);
    Console.WriteLine(resut4);
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}