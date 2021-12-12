# ExpressionEvaluator
C# Dynamic Expression Evaluator

This is a simple expression evaluation library targeting .NET

Usage of this library is fairly simple.

Step 1: Create and Initialize an Evaluation Context
    ```
    Context context = new Context();
    
    context.DeclareVariable("var1", typeof(int));
    context.DeclareVariable("var2", typeof(string));
    
    context.ExportType(typeof(Math));
    content.ExportType(typeof(DateTime));
    ```

Step 2: Compile an expression for evaluation
    ```
    CompiledExpression compiledExpr = new ExpressionCompiler(context).Compile("var1+=3");
    ```

Step 3: Set variables values to be used during evaluation at runtime
    ```
    ExpressionEvaluator evaluator = new ExpressionEvaluator(compiledExpr);
    evaluator.SetVariable("var1", 5);
    ```

Step 4: Evaluate the expression
    ```
    Func<int> func = evaluator.Evaluate<int>();
    func();
    ```

