# ExpressionEvaluator
C# Dynamic Expression Evaluator

This is a simple expression evaluation library targeting .NET

## Usage
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
## Examples

### Simple binary expressions
```
"a".     //return value of variable 'a'
"a+1"    //return value of variable 'a' + 1 - add
"a-1"    //return value of variable 'a' - 1 - subtract
"a*2"    //return value of variable 'a' * 2 - multiply  
"a/2"    //return value of variable 'a' / 2 - divide
"a % 2"  //return value of variable 'a' % 2 - modulo
"a << 1" //return value of variable 'a' << 1 - left-shift
"a >> 1" //return value of variable 'a' >> 1 - right-shift
"a & 7"  //return value of variable 'a' & 1 - bitwise-and
"a | 2"  //return value of variable 'a' | 2 - bitwise-or
"a ^ a"  //return value of variable 'a' & 'a' - bitwise-xor
```
### Simple assignment expressions - overwriting variable value
```
"a++"              //Assign value of post increment expression
"a--"              //Assign value of post decrement expression 
"--a"              //Assign value of pre decrement expression 
"++a"              //Assign value of pre increment expression 
"a+=1"             //Assign value of add expression 
"a-=1"             //Assign value of substract expression 
"a*=2"             //Assign value of multiply expression 
"a/=2"             //Assign value of divide expression
"a%=2"             //Assign value of modulo expression 
"a<<=1"            //Assign value of left-shift expression 
"a>>=1"            //Assign value of right-shift expression 
"a&=7"             //Assign value of bitwise-and expression  
"a|=7"             //Assign value of bitwise-or expression
"a^=7"             //Assign value of bitwise-xor expression
"a=(a+7)".         //Assign value of a simple expression
"a=(a+(Int16)7)"   //Assign value of a complex expression 
"a=Int32.MaxValue" //Assign a constant value
```
