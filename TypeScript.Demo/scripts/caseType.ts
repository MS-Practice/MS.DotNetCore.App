//boolean
let isDone: boolean = false;
//Number
let decLiteral: number = 6;
let hexLiteral: number = 0xf00d;    //16进制
let binaryLiteral: number = 0b1010; //2进制
let octalLiteral: number = 0o744;   //8进制
//字符串
{
    let name: string = "marson";
    name = "shine";
}
//模板字符串
{
    let name: string = `marson`;
    let age: number = 24;
    let sentence: string = `Hello,my name is ${name}.
    i'll be ${age + 1} years old next year.`;
}
//数组
let x: [string, number];
x = ['hello', 10];
//当访问超过索引值
//x[3] = 'world';
//console.log(x[5].toString());
//x[6] = true;    //error 索引超过范围，x只允许在联合类型中存在(string,number)
//枚举
enum Color { RedA = 1, GreenA, BlueA }
let c: Color = Color.BlueA;
//根据枚举值得到名字
let _colorName: string = Color[2];
alert(_colorName);
//Nerver类型  表示永远不存在的值
function error(message: string): never {
    throw new Error(message);
}
//推断的返回类型是never
function fail() {
    return error("Something failed");
}
//返回的函数的返回值必须是无法到达终点的
function infiniteLoop(): never {
    while (true) {

    }
}
//类型推断断言
let someValue: any = "this is string";
let strLength: number = (<string>someValue).length;
let strLengthAs: number = (someValue as string).length;

//解构数组
let input = [1, 2];
let [first, second] = input;
console.log(first, second);
[first, second] = [second, first];
function f([first, second]: [number, number]) {
    console.log(first, second);
}
let [third, ...rest] = [1, 2, 3, 4, 5];
console.log(third, rest);

//解构对象
let o = {
    a: "foo",
    b: 12,
    c: "bar"
};
//let { a, b } = o;
//({ a, b } = { a: "bar", b: 101 });
//let { a, ...passthrongh } = o;
//let total = passthrongh.b + passthrongh.c.length;
//属性重命名
let { a: newName, b: newNameB } = o;
let { a: newNameA, b: newNameBB }: { a: string, b: number } = o;
//默认值 缺省值
function keepWholeObject(wholeObject: { a: string, b?: number }) {
    let { a, b = 1001 } = wholeObject;
}
