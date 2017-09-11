ASP.NET CORE FOR TYPESCRIPT LEANNING URL:
https://www.tslang.cn/docs/handbook/asp-net-core.html
TYPESCRIPT DOCUMENTS:
EN:https://www.typescriptlang.org/docs/home.html
ZH:https://www.tslang.cn/docs/home.html


STEP1:
create new emplty project for asp.net core web application
STEP2:
add a folder for typescript and named 'scripts'
STEP3:
right-click on 'scripts' and click 'new item',choose Typescript File and name the file app.ts
and the content maybe same with me
STEP4:
"First we need to tell TypeScript how to build"
right-click on scripts folder and click new item
choose "TypeScript Configuration File" and use the default name 'tsconfig.json'
STEP5:
right-click on project and click new item and choose "NPM Configuration File" name packge.json
inside "devDependencies" add "gulp","del" then restore package
STEP6:
Finally, add a new JavaScript file named gulpfile.js. Put the following code inside:
/// <binding AfterBuild='default' Clean='clean' />
/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var del = require('del');

var paths = {
    scripts: ['scripts/**/*.js', 'scripts/**/*.ts', 'scripts/**/*.map'],
};

gulp.task('clean', function () {
    return del(['wwwroot/scripts/**/*']);
});

gulp.task('default', function () {
    gulp.src(paths.scripts).pipe(gulp.dest('wwwroot/scripts'))
});