//enum
enum Color { Red = 1, Green = 2, Blue = 3 }
let c: Color = Color.Blue;
let colorName: string = Color[2];
alert(colorName);
//any type used to pass complier
let notSure: any = 4;
notSure = "maybe a string instead";
notSure = false;    //finally the value is a boolean type