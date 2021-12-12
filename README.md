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
"a++"              //Assign value of variable 'a' with post increment expression
"a--"              //Assign value of variable 'a' with post decrement expression 
"--a"              //Assign value of variable 'a' with pre decrement expression 
"++a"              //Assign value of variable 'a' with pre increment expression 
"a+=1"             //Assign value of variable 'a' with add expression 
"a-=1"             //Assign value of variable 'a' with substract expression 
"a*=2"             //Assign value of variable 'a' with multiply expression 
"a/=2"             //Assign value of variable 'a' with divide expression
"a%=2"             //Assign value of variable 'a' with modulo expression 
"a<<=1"            //Assign value of variable 'a' with left-shift expression 
"a>>=1"            //Assign value of variable 'a' with right-shift expression 
"a&=7"             //Assign value of variable 'a' with bitwise-and expression  
"a|=7"             //Assign value of variable 'a' with bitwise-or expression
"a^=7"             //Assign value of variable 'a' with bitwise-xor expression
"a=(a+7)".         //Assign value of variable 'a' with a simple expression
"a=(a+(Int16)7)"   //Assign value of variable 'a' with a complex expression 
"a=Int32.MaxValue" //Assign a constant value to variable 'a'. Is also an example of Field or property access on exported type. Requires Int32 to be added to exported type
```
### Simple logical expressions
```
"a < 7"           //Compare < between variable 'a' and int constant
"a <= 7"          //Compare <= between variable 'a' and int constant
"a == 6"          //Compare for equality between variable 'a' and int constant
"a != 6"          //Compare for not equal between variable 'a' and int constant
"a > 7"           //Compare > between variable 'a' and int constant
"a >= 7"          //Compare >= between variable 'a' and int constant
"a && 7"          //Logical and between variable 'a' and int constant
"a || 0"          //Logical or between variable 'a' and int constant
"true && true".   //Logical and between boolean constants
"true || false".  //Logical or between boolean constants
"a >= 7000000000" //Compare variable 'a' (int) with long constant
```
### Object creation expression
```
"new DateTime(50000000000)"   //Create a new instance of DateTime based on Ticks (long). Requires DateTime to be added to ExportedTypes in Context
"a=new DateTime(50000000000)" //Create a new instance of DateTime and assign to variable 'a'. Requires DateTime to be added to ExportedTypes in Context
```
### Method invocation expression
```
"a.AddTicks((Int64)100)".  //Invokes method of variable (or on Exported Type)
"a=a.AddTicks((Int64)100)" //Invokes method of variable and assign value to variable 'a'
"Int32.Parse(\"100\")".    //Invokes method on exported type. Requires Int32 to be added to ExportedTypes in Context
```
### Other complex example expression
```
"String.Concat(obj[\"channel\"][\"item\"][0][\"title\"].ToString().Substring(0,8), suffix)" //Operates on variable 'obj' of type JObject and 'suffix' of type String
"Int32.Parse(((Int32)Math.Sqrt(a * a * 1.0)).ToString()) == 5" //Operates on variable 'a' of type int
```
### Reference - Test Cases
[Functional Tests](https://github.com/sriksun/ExpressionEvaluator/blob/main/ExpressionEvaulatorTests/FunctionalTests.cs)
