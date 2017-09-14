//boolean
var isDone = false;
//Number
var decLiteral = 6;
var hexLiteral = 0xf00d; //16进制
var binaryLiteral = 10; //2进制
var octalLiteral = 484; //8进制
//字符串
{
    var name_1 = "marson";
    name_1 = "shine";
}
//模板字符串
{
    var name_2 = "marson";
    var age = 24;
    var sentence = "Hello,my name is " + name_2 + ".\n    i'll be " + (age + 1) + " years old next year.";
}
//数组
var x;
x = ['hello', 10];
//当访问超过索引值
x[3] = 'world';
console.log(x[5].toString());
//x[6] = true;    //error 索引超过范围，x只允许在联合类型中存在(string,number)
//枚举
var Color;
(function (Color) {
    Color[Color["RedA"] = 1] = "RedA";
    Color[Color["GreenA"] = 2] = "GreenA";
    Color[Color["BlueA"] = 3] = "BlueA";
})(Color || (Color = {}));
var c = Color.BlueA;
//根据枚举值得到名字
var _colorName = Color[2];
alert(_colorName);
//Nerver类型  表示永远不存在的值
function error(message) {
    throw new Error(message);
}
//推断的返回类型是never
function fail() {
    return error("Something failed");
}
//返回的函数的返回值必须是无法到达终点的
function infiniteLoop() {
    while (true) {
    }
}
//类型推断断言
var someValue = "this is string";
var strLength = someValue.length;
var strLengthAs = someValue.length;
//解构数组
var input = [1, 2];
var first = input[0], second = input[1];
console.log(first, second);
_a = [second, first], first = _a[0], second = _a[1];
function f(_a) {
    var first = _a[0], second = _a[1];
    console.log(first, second);
}
var _b = [1, 2, 3, 4, 5], third = _b[0], rest = _b.slice(1);
console.log(third, rest);
//解构对象
var o = {
    a: "foo",
    b: 12,
    c: "bar"
};
//let { a, b } = o;
//({ a, b } = { a: "bar", b: 101 });
//let { a, ...passthrongh } = o;
//let total = passthrongh.b + passthrongh.c.length;
//属性重命名
var newName = o.a, newNameB = o.b;
var newNameA = o.a, newNameBB = o.b;
//默认值 缺省值
function keepWholeObject(wholeObject) {
    var a = wholeObject.a, _a = wholeObject.b, b = _a === void 0 ? 1001 : _a;
}
var _a;
//# sourceMappingURL=caseType.js.map