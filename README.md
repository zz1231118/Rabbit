# Rabbit

数学公式解析器
====================

## 一：功能介绍

### 1.示例：
* 数学公式：f(x)=4^x+5
* 使用：
```C#
var domain = new RabbitDomain();
var expression = domain.CompileFromSource("f(x)=4^x+5");
var result = expression.Eval(new RuntimeContext()
{
	["x"] = 2,
});

//result: 21
```

### 2.内置数学函数:
* abs
* ceiling
* floor
* exp
* sin
* ....


### 3.内置逻辑公式
* if

```C#
var domain = new RabbitDomain();
var expression = domain.CompileFromSource("f(x)=if(x>0,10/x,0)");
var result = expression.Eval(new RuntimeContext()
{
	["x"] = 2,
});

//result: 5
```


### 4.自定义公式调用:
```C#
var domain = new RabbitDomain();
domain.CompileFromSource("exp(lv)=lv*10");
var expression = domain.CompileFromSource("maxhp(lv)=exp(lv)+100");
var result = expression.Eval(new RuntimeContext()
{
	["lv"] = 4,
});

//result: 140
```
	
### 5.域隔离:
```C#
var lhdomain = new RabbitDomain();
lhdomain.CompileFromSource("vf(x)=x+1");
var expr1 = lhdomain.CompileFromSource("exp(lv)=vf(lv)*10");

var rhdomain = new RabbitDomain();
rhdomain.CompileFromSource("vf(x)=x+2");
var expr2 = rhdomain.CompileFromSource("exp(lv)=vf(lv)*10");

var result1 = expr1.Eval(new RuntimeContext()
{
	["lv"] = 4,
});
var result2 = expr2.Eval(new RuntimeContext()
{
	["lv"] = 4,
});

//result1:50
//result2:60
```